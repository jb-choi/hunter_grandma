using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HunterGrandma
{
    public sealed class GrandmaDefense : MonoBehaviour
    {
        [SerializeField] private GrandmaShooter grandma;
        [SerializeField] private ZombieActor zombiePrefab;
        [SerializeField] private ShotTracer tracerPrefab;
        [SerializeField] private ZombieActor[] startingZombies;
        [SerializeField, Range(12, 64)] private int zombieCapacity = 24;
        [SerializeField] private float groundY = -2.75f;
        [SerializeField] private float shootingRange = 8f;
        [SerializeField] private Sprite[] bossSprites;
        [SerializeField] private Sprite cartSprite, coinSprite;
        [SerializeField] private SpriteRenderer beltBackdrop;
        [SerializeField] private GameObject beltLayoutPreview;
        [SerializeField] private Sprite houseTeaSprite, houseChairSprite;
        private StageZeroOpeningView houseOpening;
        private bool houseOpeningSeen;
        private readonly List<ZombieActor> zombies = new List<ZombieActor>();
        private readonly List<ShotTracer> tracers = new List<ShotTracer>();
        private DefenseHud hud;
        private DefenseProgressStore store;
        private DefenseScenery scenery;
        private BeltStageView beltView;
        private BeltActorVisual cartVisual;
        private Camera worldCamera;
        private float manualOverride;
        private Vector2 captureFoot, cartEntry, heroLoadStart;
        private Rect[] obstacles = new Rect[0];
        private Transform cart;
        private float spawnTimer, fireTimer, phaseTime, previousTimeScale = 1f, hitStopUntil, nextSave;
        private int waveSpawned;
        private bool initialized, dirty, cartLoaded;
        private GrandmaShadowSkill shadowSkill;
        private Vector3 loadingBossStart;

        public DefenseProgress Progress { get; private set; }
        public DefenseRun Run { get; private set; }
        public DefensePhase Phase { get; private set; }
        public BossActor Boss { get; private set; }
        public DefenseCaptive LastCaptive { get; private set; }
        public int CurrentStageId { get; private set; }
        public DefenseStage CurrentStage => DefenseStages.Get(CurrentStageId);
        public int CurrentRegionIndex => Mathf.Max(0, CurrentStageId - 1);
        public DefenseRegion CurrentRegion => DefenseRegions.Get(CurrentRegionIndex);
        public int KillCount { get; private set; }
        public int SpawnedCount { get; private set; }
        public int ShotsFired { get; private set; }
        public int BreachCount { get; private set; }
        public int DefenseHealth { get; private set; }
        public int WaveNumber { get; private set; }
        public bool IsPaused { get; private set; }
        public bool IsSkillActive => shadowSkill != null && shadowSkill.IsActive;
        public float SkillCooldownRemaining => shadowSkill != null ? shadowSkill.CooldownRemaining : GrandmaShadowSkill.Interval;
        public int SkillShots => shadowSkill != null ? shadowSkill.Shots : 0;
        public int SkillActivations => shadowSkill != null ? shadowSkill.Activations : 0;
        public bool IsCombatPhase => Phase == DefensePhase.Waves || Phase == DefensePhase.BossFight;
        public bool SaveWarning { get; private set; }
        public bool IsCaptureSequence => Phase == DefensePhase.Subdued || Phase == DefensePhase.LoadingCart || Phase == DefensePhase.Returning;
        public bool IsOpening => Phase == DefensePhase.Opening;
        public float OpeningAge => houseOpening != null ? houseOpening.Age : 0;
        public string OpeningCaption => houseOpening != null ? houseOpening.Caption : "";
        public int OpeningSoundCues => houseOpening != null ? houseOpening.SoundCues : 0;
        public bool CanOpenMap => initialized && !IsCaptureSequence && !IsOpening;
        public bool AtYard => Phase == DefensePhase.Clear || (Phase == DefensePhase.Returning && phaseTime >= 3.5f);
        public string LocationName => AtYard ? "할머니 집 마당" : CurrentStage.Name;
        public float Elapsed { get; private set; }
        public float DefenseLine => GrandmaX + 1.2f;
        public float GrandmaX => grandma.FootPosition.x;
        public Vector2 GrandmaFootPosition => grandma.FootPosition;
        public Sprite GrandmaPortrait => grandma.IdleSprite;
        // Retained as the legacy default lane; active actors use their own foot depth.
        public float GroundY => groundY;
        public float WaveAnchor => (WaveNumber - 1) * 10f;
        public float StageMaxX => CurrentStage.WaveCount * 10f + 14f;
        public Rect[] Obstacles => obstacles;
        public Sprite ShadowSprite => beltView.ShadowSprite;
        public float ShootingRange => shootingRange;
        public bool AutoExplore { get; private set; } = true;
        public bool CanMoveOnBelt => !IsPaused && !IsSkillActive && (Phase == DefensePhase.Waves || Phase == DefensePhase.WaveBreak || Phase == DefensePhase.BossArrival || Phase == DefensePhase.BossFight);
        public float CameraFocusX => Phase == DefensePhase.LoadingCart || Phase == DefensePhase.Subdued ? Boss.transform.position.x - 2f : Phase == DefensePhase.Returning ? cart.position.x : GrandmaX;
        public float CombatDeltaTime => IsPaused || IsOpening || IsSkillActive || (shadowSkill != null && shadowSkill.OwnsFrame) || Time.unscaledTime < hitStopUntil ? 0f : Time.deltaTime;
        public float ReturnFade => Phase == DefensePhase.Returning ? Mathf.Clamp01(1f - Mathf.Abs(phaseTime - 3.5f) / 0.6f) : 0f;
        public int PoolSize => zombies.Count;
        public int ActiveZombieCount
        {
            get { int count = 0; foreach (ZombieActor zombie in zombies) if (zombie.IsAlive) count++; return count; }
        }
        public string StateText
        {
            get
            {
                if (IsPaused) return "잠시 쉬는 중";
                if (IsSkillActive) return "할매의 밤마실 · " + SkillShots + " / 5";
                switch (Phase)
                {
                    case DefensePhase.Opening: return OpeningCaption;
                    case DefensePhase.Waves: return "웨이브 " + WaveNumber + " / " + CurrentStage.WaveCount;
                    case DefensePhase.WaveBreak: return WaveNumber < CurrentStage.WaveCount ? "앞으로 이동 → 다음 전투 구역" : "거대 감염체가 다가온다";
                    case DefensePhase.BossArrival: return "거대 숙주형 감염체 등장";
                    case DefensePhase.BossFight: return Boss.Telegraph;
                    case DefensePhase.Subdued: return "제압 성공 · 사격 중지";
                    case DefensePhase.LoadingCart: return "감염체를 수레에 싣는 중";
                    case DefensePhase.Returning: return AtYard ? "집 마당에 도착" : "집 마당으로 귀환";
                    case DefensePhase.Clear: return "STAGE CLEAR · 신규 포획 +1";
                    case DefensePhase.Failed: return "방어선 붕괴 · 강화 후 재도전";
                    default: return "후퇴 · 획득 (미정)재화 보존";
                }
            }
        }
        public void Configure(GrandmaShooter shooter, ZombieActor enemy, ShotTracer shot, ZombieActor[] initial)
        { grandma = shooter; zombiePrefab = enemy; tracerPrefab = shot; startingZombies = initial; }
        private void Start()
        {
            Application.runInBackground = true;
            if (beltLayoutPreview != null) beltLayoutPreview.SetActive(false);
            hud = GetComponent<DefenseHud>();
            if (grandma == null || zombiePrefab == null || tracerPrefab == null || bossSprites == null || bossSprites.Length != DefenseStages.Count || cartSprite == null || coinSprite == null)
            { Debug.LogError("GrandmaDefense needs its scene references and the seven Notion boss sprites.", this); enabled = false; return; }
            store = new DefenseProgressStore(DefenseProgressStore.DefaultPath);
            Progress = store.Load();
            if (startingZombies != null) zombies.AddRange(startingZombies);
            while (zombies.Count < zombieCapacity)
            {
                ZombieActor zombie = Instantiate(zombiePrefab, transform); zombie.gameObject.SetActive(false); zombies.Add(zombie);
            }
            for (int i = 0; i < 6; i++)
            { ShotTracer tracer = Instantiate(tracerPrefab, transform); tracer.gameObject.SetActive(false); tracers.Add(tracer); }
            SpriteRenderer pixel = tracerPrefab.GetComponent<SpriteRenderer>();
            beltView = new GameObject("Belt stage view", typeof(BeltStageView)).GetComponent<BeltStageView>();
            worldCamera=Camera.main;
            beltView.transform.SetParent(transform, false); beltView.Initialize(this, worldCamera, beltBackdrop, pixel.sprite, pixel.sharedMaterial);
            grandma.InitializeBelt(this, ShadowSprite);
            shadowSkill = new GrandmaShadowSkill(this, grandma, worldCamera, pixel.sprite, ShadowSprite);
            if (houseTeaSprite != null && houseChairSprite != null)
                houseOpening = StageZeroOpeningView.Create(transform, houseTeaSprite, houseChairSprite, grandma.IdleSprite, zombiePrefab.WalkFrames, pixel.sprite, pixel.sharedMaterial, true);
            scenery = new GameObject("Stage scenery", typeof(DefenseScenery)).GetComponent<DefenseScenery>();
            scenery.transform.SetParent(transform, false); scenery.Initialize(pixel.sprite, pixel.sharedMaterial);
            GameObject bossObject = new GameObject("Giant host boss", typeof(SpriteRenderer), typeof(BossActor));
            bossObject.transform.SetParent(transform, false); Boss = bossObject.GetComponent<BossActor>();
            Boss.Initialize(this, pixel.sprite, coinSprite, pixel.sharedMaterial); bossObject.SetActive(false);
            cart = new GameObject("Capture cart", typeof(SpriteRenderer)).transform;
            cart.SetParent(transform, false);
            SpriteRenderer wagon = cart.GetComponent<SpriteRenderer>(); wagon.sprite = cartSprite; wagon.sharedMaterial = pixel.sharedMaterial; wagon.sortingOrder = 15;
            cartVisual=cart.gameObject.AddComponent<BeltActorVisual>();cartVisual.Initialize(wagon,ShadowSprite,.9f);cartVisual.Body.sortingOrder=20;
            cart.gameObject.SetActive(false);
            initialized = true; StartStage(0);
        }
        public bool EnterStage(int stageId)
        {
            if (!initialized || IsCaptureSequence || IsOpening || !Progress.IsUnlocked(stageId)) return false;
            if (stageId == CurrentStageId && Phase != DefensePhase.Clear && Phase != DefensePhase.Failed && Phase != DefensePhase.Retreated) return true;
            StartStage(stageId); return true;
        }
        public bool TravelToRegion(int index) => DefenseRegions.IsValid(index) && EnterStage(index + 1);
        public void RestartEncounter() { if (initialized && !IsCaptureSequence && !IsOpening) StartStage(CurrentStageId); }
        [ContextMenu("Replay Stage 0 Opening")]
        public void ReplayStageZeroOpening()
        { if (initialized && CurrentStageId == 0 && !IsCaptureSequence) { houseOpeningSeen = false; StartStage(0); } }
        private void StartStage(int id)
        {
            shadowSkill?.Reset();
            if (Run != null && !Run.Settled) Run.Abandon();
            bool keepPaused = IsPaused;
            foreach (ShotTracer tracer in tracers) tracer.Cancel();
            foreach (ZombieActor zombie in zombies) zombie.Despawn();
            Boss.Despawn(); cart.gameObject.SetActive(false);
            hud?.ResetRunFeedback();
            CurrentStageId = id; Run = new DefenseRun(id); LastCaptive = null;
            KillCount = SpawnedCount = ShotsFired = BreachCount = 0;
            Elapsed = phaseTime = hitStopUntil = 0f; DefenseHealth = 100; WaveNumber = 1; cartLoaded = false;
            obstacles = new[] { new Rect(12f,-2.65f,1.4f,.75f),new Rect(23f,-1.85f,1.5f,.7f),new Rect(34f,-3.6f,1.4f,.7f) };
            beltView.Configure(StageMaxX,obstacles);
            fireTimer = 0.3f; manualOverride=0f; grandma.ResetPose();grandma.MoveOnBelt(BeltSpace.Start); IsPaused = keepPaused;
            scenery.Show(id, false);
            bool playOpening = id == 0 && !houseOpeningSeen && houseOpening != null;
            if (houseOpening != null)
            {
                houseOpening.gameObject.SetActive(id == 0);
                if (id == 0) { if (playOpening) houseOpening.Begin(); else houseOpening.Finish(); }
            }
            scenery.gameObject.SetActive(id != 0 || houseOpening == null);
            grandma.SetPresentationVisible(!playOpening);
            if (playOpening) SetPhase(DefensePhase.Opening);
            else { if (id == 0 && houseOpening != null) grandma.MoveOnBelt(StageZeroOpeningView.Exit); BeginWave(); }
        }
        public void SkipStageOpening()
        {
            if (!IsOpening || houseOpening == null) return;
            houseOpeningSeen = true; houseOpening.Finish();
            grandma.MoveOnBelt(StageZeroOpeningView.Exit); grandma.SetPresentationVisible(true);
            fireTimer = .3f; BeginWave();
        }
        private void BeginWave()
        {
            SetPhase(DefensePhase.Waves); waveSpawned = 0; spawnTimer = CurrentStage.SpawnInterval;
            scenery.transform.position=new Vector3(WaveAnchor,1.75f);
            for (int i = 0; i < 3; i++) SpawnWaveZombie(WaveAnchor+2f + i * 4f);
        }
        private bool SpawnWaveZombie(float x)
        {
            if (waveSpawned >= CurrentStage.EnemiesPerWave) return false;
            bool mixed = CurrentStageId == 3 || CurrentStageId == 6;
            float size = mixed && waveSpawned % 3 == 0 ? 1.25f : mixed ? 0.8f : 1f;
            int hp = Mathf.RoundToInt(CurrentStage.ZombieHealth * size);
            float speed = CurrentStage.ZombieSpeed;
            if ((CurrentStageId == 5 || CurrentStageId == 6) && waveSpawned >= CurrentStage.EnemiesPerWave - 3) speed *= 1.5f;
            if ((CurrentStageId == 4 && waveSpawned % 3 == 1) || (CurrentStageId == 6 && waveSpawned % 7 == 1)) x = Mathf.Max(BeltSpace.MinX+.5f,WaveAnchor-11f);
            float depth = Mathf.Lerp(BeltSpace.MinDepth+.3f,BeltSpace.MaxDepth-.25f,((waveSpawned*3+WaveNumber)%7)/6f);
            bool atFence = CurrentStageId == 0 && WaveNumber == 1 && houseOpening != null;
            if (atFence) { Vector2 foot = StageZeroOpeningView.VisitorFoot(waveSpawned); x = foot.x; depth = foot.y; }
            if (!SpawnZombie(x, depth, speed, hp, size, atFence)) return false;
            waveSpawned++; return true;
        }
        private bool SpawnZombie(float x, float depth, float speed, int hp, float size, bool atFence = false)
        {
            foreach (ZombieActor zombie in zombies)
            {
                if (zombie.gameObject.activeSelf) continue;
                zombie.Spawn(this, FreeFootPosition(new Vector2(x,depth)), speed, SpawnedCount * 0.31f, hp, CurrentStage.GoldPerKill, size);
                if (atFence) zombie.HoldAtFence();
                SpawnedCount++; return true;
            }
            return false;
        }
        public void OnFenceStruck(Vector2 foot)
        { if (houseOpening != null && CurrentStageId == 0 && WaveNumber == 1) houseOpening.FenceImpact(foot); }
        public void SummonBossMinions()
        { for (int i = 0; i < 2; i++) SpawnZombie(WaveAnchor+7f+i*3f,i==0?-1.3f:-3.6f,1.5f,CurrentStage.ZombieHealth,.8f); }
        private void Update()
        {
            if (!initialized) return;
            scenery.Show(CurrentStageId, AtYard);
            if (houseOpening != null)
            {
                bool showHouse = CurrentStageId == 0 && !AtYard;
                if (houseOpening.gameObject.activeSelf != showHouse) houseOpening.gameObject.SetActive(showHouse);
                scenery.gameObject.SetActive(!showHouse);
            }
            if(AtYard)scenery.transform.position=Vector3.up*1.75f;
            if (dirty && Time.unscaledTime >= nextSave) SaveNow();
            if (IsOpening)
            {
                houseOpening.Advance(IsPaused ? 0 : Time.deltaTime);
                if (houseOpening.Age >= StageZeroOpeningView.Duration) SkipStageOpening();
                return;
            }
            shadowSkill.Advance(IsPaused || Time.unscaledTime < hitStopUntil ? 0 : Time.deltaTime);
            // Keep every actor frozen for the frame in which the last skill beat finishes.
            if (shadowSkill.OwnsFrame) return;
            float dt = CombatDeltaTime; if (dt <= 0f) return;
            if (houseOpening != null && houseOpening.gameObject.activeSelf) houseOpening.AdvanceFence(dt);
            UpdateMovement(dt);
            if (Phase == DefensePhase.Clear || Phase == DefensePhase.Failed || Phase == DefensePhase.Retreated) return;
            Elapsed += dt; phaseTime += dt; fireTimer -= dt;
            if (Phase == DefensePhase.Waves)
            {
                spawnTimer -= dt;
                if (spawnTimer <= 0f && waveSpawned < CurrentStage.EnemiesPerWave)
                { if (SpawnWaveZombie(WaveAnchor+13f)) spawnTimer += CurrentStage.SpawnInterval; }
                if (waveSpawned >= CurrentStage.EnemiesPerWave && ActiveZombieCount == 0) SetPhase(DefensePhase.WaveBreak);
            }
            else if (Phase == DefensePhase.WaveBreak && phaseTime >= 2f && (WaveNumber == CurrentStage.WaveCount || GrandmaX >= WaveNumber*10f-5f))
            {
                if (WaveNumber < CurrentStage.WaveCount) { WaveNumber++; BeginWave(); }
                else { SetPhase(DefensePhase.BossArrival); Boss.Spawn(CurrentStage, bossSprites[CurrentStageId], Progress.Stage(CurrentStageId).capturedCount); }
            }
            else if (Phase == DefensePhase.BossArrival && phaseTime >= 4.6f) SetPhase(DefensePhase.BossFight);
            else if (Phase == DefensePhase.Subdued && phaseTime >= 1.1f)
            {
                SetPhase(DefensePhase.LoadingCart); cart.gameObject.SetActive(true);
                captureFoot=Boss.transform.position;loadingBossStart=Boss.transform.position;
                cartEntry=new Vector2(captureFoot.x-13f,BeltSpace.MinDepth+.2f);cart.position=cartEntry;heroLoadStart=GrandmaFootPosition;
            }
            else if (Phase == DefensePhase.LoadingCart) UpdateLoading();
            else if (Phase == DefensePhase.Returning) UpdateReturn();
            bool fighting = Phase == DefensePhase.Waves || Phase == DefensePhase.BossFight;
            grandma.SetCombatTarget(fighting ? FindShotTarget(true) : null, fireTimer);
            if (fighting && fireTimer <= 0f) TryFire();
        }
        private DefenseTarget FindShotTarget(bool includeReserved)
        {
            DefenseTarget target = null; float distance = float.MaxValue;
            foreach (ZombieActor zombie in zombies)
            {
                float range = Mathf.Abs(zombie.transform.position.x - GrandmaX);
                if (!zombie.IsAlive || (!includeReserved && zombie.HasIncomingShot) || range >= distance || !CanShoot(zombie)) continue;
                target = zombie; distance = range;
            }
            if (Boss.IsAlive && (includeReserved || !Boss.HasIncomingShot) && CanShoot(Boss))
                target = target ?? Boss;
            return target;
        }
        private void TryFire()
        {
            DefenseTarget target = FindShotTarget(false);
            if (target == null) return;
            foreach (ShotTracer tracer in tracers)
            {
                if (tracer.IsFlying || !target.ReserveShot()) continue;
                grandma.FaceTarget(target.transform.position.x);
                grandma.Fire();
                tracer.Launch(grandma.MuzzlePosition, target, this, Progress.Damage);
                ShotsFired++; fireTimer = Progress.FireInterval; return;
            }
        }
        private bool CanShoot(DefenseTarget target)
        {
            float facing=target.transform.position.x<GrandmaX?-1f:1f;
            return BeltSpace.CanHit(GrandmaFootPosition,target.transform.position,facing,BeltSpace.ClearRange(GrandmaFootPosition,facing,shootingRange,obstacles),obstacles);
        }
        public void ResolveShotImpact(DefenseTarget primary, int damage, Vector2 origin, float facing, float range)
        {
            if (Phase != DefensePhase.Waves && Phase != DefensePhase.BossFight) return;
            foreach (ZombieActor zombie in zombies)
                if (zombie.IsAlive && BeltSpace.CanHit(origin,zombie.transform.position,facing,range,obstacles)) zombie.ReceiveDamage(zombie==primary?damage:Mathf.Max(1,Mathf.RoundToInt(damage*.6f)));
            if(Boss.IsAlive&&BeltSpace.CanHit(origin,Boss.transform.position,facing,range,obstacles))Boss.ReceiveDamage(damage);
        }
        public Vector2 MoveActor(Vector2 from, Vector2 delta) => BeltSpace.Move(from,delta,StageMaxX,obstacles);
        public Vector2 FreeFootPosition(Vector2 position)
        {
            position=BeltSpace.Clamp(position,StageMaxX);
            foreach(Rect obstacle in obstacles)
                if(position.x>obstacle.xMin-.3f&&position.x<obstacle.xMax+.3f&&position.y>obstacle.yMin-.3f&&position.y<obstacle.yMax+.3f)position.x=obstacle.xMax+.4f;
            return position;
        }
        public void RegisterContact() { BreachCount++; }
        public void ToggleAutoExplore() { if(CanMoveOnBelt)AutoExplore=!AutoExplore; }
        private DefenseTarget NearestTarget()
        {
            DefenseTarget target=null;float best=float.MaxValue;
            foreach(ZombieActor zombie in zombies)
            {
                if(!zombie.IsAlive)continue;
                Vector2 delta=(Vector2)zombie.transform.position-GrandmaFootPosition;float score=Mathf.Abs(delta.x)+Mathf.Abs(delta.y)*2f;
                if(score<best){best=score;target=zombie;}
            }
            return target!=null?target:Boss.IsAlive?Boss:null;
        }
        private void UpdateMovement(float dt)
        {
            if(!CanMoveOnBelt)return;
            Keyboard keyboard=Keyboard.current;Vector2 input=Vector2.zero;
            if(keyboard!=null)
            {
                if(keyboard.tabKey.wasPressedThisFrame)ToggleAutoExplore();
                input.x=(keyboard.dKey.isPressed||keyboard.rightArrowKey.isPressed?1:0)-(keyboard.aKey.isPressed||keyboard.leftArrowKey.isPressed?1:0);
                input.y=(keyboard.wKey.isPressed||keyboard.upArrowKey.isPressed?1:0)-(keyboard.sKey.isPressed||keyboard.downArrowKey.isPressed?1:0);
            }
            Vector2 position=GrandmaFootPosition, next=position;
            if(input.sqrMagnitude>.01f){grandma.InterruptCollect();manualOverride=1.1f;next=MoveActor(position,Vector2.ClampMagnitude(input,1f)*3.6f*dt);}
            else
            {
                manualOverride=Mathf.Max(0,manualOverride-dt);
                if(AutoExplore&&manualOverride<=0)
                {
                    DefenseTarget target=NearestTarget();Vector2 goal=position;
                    if (target == null && grandma.TryCollect()) { grandma.MoveOnBelt(position); return; }
                    if(target!=null)
                    {
                        Vector2 foot=target.transform.position;float sign=foot.x<position.x?-1f:1f;float range=Mathf.Abs(foot.x-position.x);
                        goal.y=foot.y;
                        if(range>shootingRange*.78f)goal.x=foot.x-sign*shootingRange*.65f;
                        else if(range<2.5f)goal.x=foot.x-sign*3.2f;
                        if(!CanShoot(target)&&Mathf.Abs(foot.y-position.y)<.15f)goal.x=foot.x-sign*2f;
                    }
                    else goal=new Vector2(Phase==DefensePhase.WaveBreak&&WaveNumber<CurrentStage.WaveCount?WaveNumber*10f-4f:WaveAnchor-4f,BeltSpace.CenterDepth);
                    goal=BeltSpace.Clamp(goal,StageMaxX);goal=BeltSpace.Waypoint(position,goal,obstacles);
                    Vector2 delta=Vector2.MoveTowards(position,goal,2.8f*dt)-position;next=MoveActor(position,delta);
                }
            }
            next.x=Mathf.Min(next.x,WaveAnchor+12f);grandma.MoveOnBelt(next);
        }
        public void OnZombieKilled(int gold, Vector3 position)
        {
            KillCount++; Progress.AddGold(gold); Run.AddGold(gold); dirty = true;
            hitStopUntil = Time.unscaledTime + 0.025f;
            hud?.ShowResourceGain(gold, position);
            if (gold > 0) grandma.QueueCollect();
        }
        public void RegisterKill() { OnZombieKilled(CurrentStage.GoldPerKill, grandma.MuzzlePosition); }
        public void RegisterBreach() { BreachCount++; DamageDefense(4); }
        public void DamageDefense(int damage)
        {
            if (IsSkillActive) return;
            if (IsOpening || IsCaptureSequence || Phase == DefensePhase.Clear || Phase == DefensePhase.Failed || Phase == DefensePhase.Retreated) return;
            DefenseHealth = Mathf.Max(0, DefenseHealth - damage);
            if (DefenseHealth > 0) return;
            Run.Abandon(); SetPhase(DefensePhase.Failed); StopActors(); SaveNow();
        }
        public bool Retreat()
        {
            if (!initialized || IsOpening || IsCaptureSequence || Phase == DefensePhase.Clear) return false;
            Run.Abandon(); SetPhase(DefensePhase.Retreated); StopActors(); SaveNow(); return true;
        }
        public void OnBossSubdued()
        {
            if (Phase != DefensePhase.BossFight) return;
            Run.SubdueBoss();
            foreach (ShotTracer tracer in tracers) tracer.Cancel();
            foreach (ZombieActor zombie in zombies) zombie.Despawn();
            SetPhase(DefensePhase.Subdued);
        }
        private void UpdateLoading()
        {
            float t = Mathf.Clamp01(phaseTime / 1.25f);
            Vector2 cartGoal=captureFoot+Vector2.left*.8f;
            cart.position=Vector2.Lerp(cartEntry,cartGoal,t);
            Vector2 heroGoal=(Vector2)cart.position+Vector2.left*2.2f;
            grandma.PresentTravel(Vector2.Lerp(heroLoadStart,heroGoal,t),true);
            if (phaseTime > 1.25f && !cartLoaded)
            {
                Boss.transform.position = Vector3.Lerp(loadingBossStart, cart.position + new Vector3(0.35f, 0.8f, 0f), Mathf.Clamp01((phaseTime - 1.25f) / 1f));
                if (phaseTime >= 2.25f) { Boss.LoadOnto(cart); cartLoaded = true; SetPhase(DefensePhase.Returning); }
            }
        }
        private void UpdateReturn()
        {
            float x=phaseTime<3.5f?Mathf.Lerp(captureFoot.x-.8f,BeltSpace.MinX-8f,Mathf.Clamp01(phaseTime/3f)):Mathf.Lerp(13f,-3.4f,Mathf.Clamp01((phaseTime-3.5f)/3.5f));
            float y=phaseTime<3.5f?Mathf.Lerp(captureFoot.y,BeltSpace.CenterDepth,Mathf.Clamp01(phaseTime/2f)):BeltSpace.CenterDepth;
            cart.position=new Vector3(x,y,0f);grandma.PresentTravel(new Vector3(x-2.2f,y,0f),true);
            if (phaseTime < 7f) return;
            Run.ReturnToYard();
            if (Progress.TryComplete(Run, Boss.Appearance, out DefenseCaptive captive)) LastCaptive = captive;
            else { Debug.LogError("Capture settlement rejected an incomplete or duplicate run."); return; }
            grandma.PresentTravel(new Vector3(-6.5f,BeltSpace.CenterDepth,0f),false);
            SetPhase(DefensePhase.Clear); dirty = true; SaveNow();
        }
        public bool TryUpgradeAttack() { bool bought = !IsOpening && Progress != null && Progress.TryUpgradeAttack(); if (bought) { dirty = true; SaveNow(); } return bought; }
        public bool TryUpgradeFireRate() { bool bought = !IsOpening && Progress != null && Progress.TryUpgradeFireRate(); if (bought) { dirty = true; SaveNow(); } return bought; }
        public void TogglePause()
        {
            if (!initialized) return;
            IsPaused = !IsPaused;
            if (IsPaused) { previousTimeScale = Time.timeScale; Time.timeScale = 0f; }
            else Time.timeScale = previousTimeScale;
            if (houseOpening != null) houseOpening.SetPaused(IsPaused);
        }
        private void SetPhase(DefensePhase phase) { Phase = phase; phaseTime = 0f; }
        private void StopActors()
        {
            shadowSkill?.Cancel();
            grandma.StopCombatPresentation();
            foreach (ShotTracer tracer in tracers) tracer.Cancel();
            foreach (ZombieActor zombie in zombies) zombie.Despawn();
            Boss.Despawn();
        }
        private void SaveNow()
        {
            if (Progress == null || store == null) return;
            SaveWarning = !store.Save(Progress); dirty = SaveWarning; nextSave = Time.unscaledTime + 0.75f;
        }
        private void OnApplicationPause(bool paused) { if (paused) SaveNow(); }
        private void OnApplicationQuit() { SaveNow(); }
        private void OnDisable()
        {
            shadowSkill?.Cancel();
            if (initialized) SaveNow();
            if (IsPaused) { Time.timeScale = previousTimeScale; IsPaused = false; }
        }
        private void OnDestroy() { shadowSkill?.Dispose(); }

        public void SetShadowSkillEnabled(bool enabled)
        {
            if (Progress == null) return;
            Progress.SetShadowSkillEnabled(enabled);
            shadowSkill?.Reset(); dirty = true; SaveNow();
        }
        internal int CollectSkillTargets(List<ZombieActor> targets, Camera camera, float facing)
        {
            targets.Clear();
            foreach (ZombieActor zombie in zombies)
            {
                if (!zombie.IsAlive || (zombie.transform.position.x - GrandmaX) * facing < 0) continue;
                Vector3 viewport = camera.WorldToViewportPoint(zombie.HitPoint);
                if (viewport.z > 0 && viewport.x > .07f && viewport.x < .93f && viewport.y > .1f && viewport.y < .9f && TrySkillFoot(zombie, out _)) targets.Add(zombie);
            }
            Vector2 origin = GrandmaFootPosition;
            targets.Sort((a, b) => ((Vector2)a.transform.position - origin).sqrMagnitude.CompareTo(((Vector2)b.transform.position - origin).sqrMagnitude));
            return targets.Count;
        }
        internal bool TrySkillFoot(ZombieActor zombie, out Vector2 position)
        {
            Vector2 foot = zombie.transform.position;
            position = FreeFootPosition(foot - Vector2.right * zombie.FacingDirection * 1.35f);
            return Mathf.Abs(position.y - foot.y) <= .05f && (position.x - foot.x) * zombie.FacingDirection <= -.4f;
        }
    }
}
