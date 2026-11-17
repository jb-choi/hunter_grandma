using UnityEngine;

namespace HunterGrandma
{
    public enum ZombieAttackBeat { None, Windup, Strike, Recover }
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class ZombieActor : DefenseTarget
    {
        [SerializeField] private Sprite[] walkFrames;
        [SerializeField] private ZombieAnimationSet animationSet;
        private SpriteRenderer spriteRenderer;
        private BeltActorVisual visual;
        private GrandmaDefense encounter;
        private float age, deathAge, speed, phase, size = 1f, flash, attackCooldown;
        private int health, reward, direction;
        private bool alive, contacted;
        private float attackAge = -1;
        private int attackDirection;
        private Vector2 attackOrigin;
        private bool attackResolved;
        private bool bangingFence;
        private Vector2 fenceFoot;
        private int lastFenceStrike;
        private bool authored;
        private ZombiePopVisual pop;
        public ZombieAnimationSet AnimationSet => animationSet;
        public ZombieKind Kind { get; private set; }
        public ZombieAction AnimationAction { get; private set; }
        public Sprite[] WalkFrames => animationSet != null && animationSet.IsReady ? animationSet.Clip(ZombieKind.Infected, ZombieAction.Walk).frames : walkFrames;
        public bool IsBangingFence => IsAlive && bangingFence;
        public ZombieAttackBeat AttackBeat => attackAge < 0 ? ZombieAttackBeat.None : attackAge < .32f ? ZombieAttackBeat.Windup : attackAge < .50f ? ZombieAttackBeat.Strike : ZombieAttackBeat.Recover;
        public float AttackAge => attackAge;
        public override bool IsAlive => alive && gameObject.activeSelf;
        public override Vector3 HitPoint => transform.position + Vector3.up * (1.68f * size);
        public int Health => health;
        public float FacingDirection => direction;
        public Vector3 HeadPoint => visual == null ? HitPoint : new Vector3(visual.Body.bounds.center.x, visual.Body.bounds.max.y - visual.Body.bounds.size.y * .18f, transform.position.z);
        public bool IsMelting => gameObject.activeSelf && !alive;
        public void Configure(Sprite[] frames) { walkFrames = frames; }
        private void Awake() { spriteRenderer = GetComponent<SpriteRenderer>(); }
        public void Spawn(GrandmaDefense owner, Vector3 position, float walkSpeed, float animationPhase)
        { Spawn(owner, position, walkSpeed, animationPhase, 10, 3, 1f); }
        public void Spawn(GrandmaDefense owner, Vector3 position, float walkSpeed, float animationPhase, int hp, int gold, float scale, ZombieKind kind = ZombieKind.Infected)
        {
            if (spriteRenderer == null) Awake();
            encounter = owner;
            Kind = kind; authored = animationSet != null && animationSet.IsReady;
            if (visual == null) { visual = gameObject.AddComponent<BeltActorVisual>(); visual.Initialize(spriteRenderer, owner.ShadowSprite, .8f); spriteRenderer = visual.Body; }
            if (pop == null) pop = new ZombiePopVisual(transform, spriteRenderer.sharedMaterial);
            pop.Reset();
            speed = walkSpeed; phase = animationPhase; health = hp; reward = gold; size = scale;
            age = deathAge = flash = attackCooldown = 0f; alive = true; contacted = false;
            attackAge = -1; attackResolved = false; bangingFence = false;
            direction = position.x > owner.GrandmaX ? -1 : 1; ReleaseShot();
            transform.SetPositionAndRotation(position, Quaternion.identity); transform.localScale = Vector3.one;
            visual.SetScale(size*.8f); visual.Pose(Vector3.zero); visual.Shadow.enabled=true; visual.RefreshDepth();
            spriteRenderer.color = Color.white; Face();
            if (authored) Present(ZombieAction.Walk, phase, true); else spriteRenderer.sprite = walkFrames[0];
            gameObject.SetActive(true);
        }
        public override void ReceiveDamage(int damage)
        {
            if (!IsAlive || damage <= 0) return;
            health -= damage;
            attackAge = -1; visual.Pose(Vector3.zero); attackCooldown = Mathf.Max(attackCooldown, .35f);
            transform.position = encounter.MoveActor(transform.position, Vector2.right * (-direction*.20f)); flash = .10f;
            if (health > 0) return;
            alive = false; bangingFence = false; ReleaseShot(); deathAge = 0f; encounter.OnZombieKilled(reward, HitPoint);
            if (authored && (Kind == ZombieKind.PartyBalloon || Kind == ZombieKind.PartyHorn)) pop.Play(animationSet.PartyPop, size);
            if (authored) Present(ZombieAction.Dead, 0, false);
        }
        public void Despawn() { alive = false; bangingFence = false; attackAge = -1; pop?.Reset(); ReleaseShot(); gameObject.SetActive(false); }
        private void OnDisable() { pop?.Reset(); }
        public void HoldAtFence()
        {
            bangingFence = true; fenceFoot = transform.position;
            lastFenceStrike = Mathf.FloorToInt((phase - .44f) / 1.35f);
        }
        // Shared by the opening visitors and the live first-wave actors.
        public static float FenceReach(float beat)
        {
            if (beat < .32f) return -Mathf.SmoothStep(0, 1, beat / .32f);
            if (beat < .50f) return Mathf.Lerp(-1, 1, Mathf.SmoothStep(0, 1, (beat - .32f) / .18f));
            if (beat < .85f) return 1 - Mathf.SmoothStep(0, 1, (beat - .50f) / .35f);
            return 0;
        }
        private void UpdateFence(float dt)
        {
            attackAge = -1; direction = -1; Face();
            transform.position = encounter.MoveActor(transform.position, Vector2.MoveTowards(transform.position, fenceFoot, dt * 1.5f) - (Vector2)transform.position);
            float beat = Mathf.Repeat(age + phase, 1.35f), reach = FenceReach(beat);
            if (authored) { AnimationAction = ZombieAction.Attack; spriteRenderer.sprite = animationSet.Attack(Kind, beat); }
            else spriteRenderer.sprite = walkFrames[Mathf.Min(reach > .1f ? 1 : 2, walkFrames.Length - 1)];
            visual.Pose(new Vector3(-reach * .18f, -Mathf.Max(0, reach) * .08f), reach * 11f, 1 + Mathf.Max(0, reach) * .06f, 1 + reach * .035f);
            int strike = Mathf.FloorToInt((age + phase - .44f) / 1.35f);
            if (strike > lastFenceStrike) { lastFenceStrike = strike; encounter.OnFenceStruck(fenceFoot); }
        }
        private void Update()
        {
            if (encounter == null) return;
            float dt = encounter.CombatDeltaTime; if (dt <= 0f) return;
            pop?.Tick(dt);
            if (alive)
            {
                age += dt; flash = Mathf.Max(0f, flash - dt); attackCooldown-=dt;
                spriteRenderer.color = flash > 0f ? new Color(1f,.65f,.4f) : authored ? Color.white : new Color(.70f,.73f,.80f);
                if (bangingFence) { UpdateFence(dt); return; }
                if (attackAge >= 0) { UpdateAttack(dt); return; }
                Vector2 delta = encounter.GrandmaFootPosition - (Vector2)transform.position;
                direction = delta.x < 0 ? -1 : 1; Face();
                bool moving = Mathf.Abs(delta.x) > .7f || Mathf.Abs(delta.y) > .42f;
                if (Mathf.Abs(delta.x)>.7f || Mathf.Abs(delta.y)>.42f)
                {
                    Vector2 destination=BeltSpace.Waypoint(transform.position,encounter.GrandmaFootPosition,encounter.Obstacles);
                    Vector2 movement = destination - (Vector2)transform.position;
                    movement = new Vector2(Mathf.Clamp(movement.x,-speed*dt,speed*dt),Mathf.Clamp(movement.y,-speed*.65f*dt,speed*.65f*dt));
                    transform.position=encounter.MoveActor(transform.position,movement);
                }
                else if(attackCooldown<=0f)
                {
                    attackAge = 0; attackDirection = direction; attackOrigin = transform.position;
                    attackResolved = false; attackCooldown = 1.35f; UpdateAttack(0); return;
                }
                if (authored) Present(moving ? speed >= 1.8f ? ZombieAction.Run : ZombieAction.Walk : ZombieAction.Idle, age + phase, true);
                else { int step = (int)((age + phase) * 5f) % 4; spriteRenderer.sprite = walkFrames[step == 3 ? 1 : step]; }
                visual.Pose(Vector3.zero, authored ? 0 : Mathf.Sin((age+phase)*9f)*1.5f);
                return;
            }
            deathAge += dt;
            if (authored)
            {
                Present(ZombieAction.Dead, deathAge, false); visual.Pose(Vector3.zero);
                float end = animationSet.Clip(Kind, ZombieAction.Dead).Duration + .4f;
                spriteRenderer.color = new Color(1, 1, 1, 1 - Mathf.InverseLerp(end, end + .25f, deathAge));
                if (deathAge >= end + .25f) Despawn();
                return;
            }
            float melt=Mathf.Clamp01(deathAge/.65f);
            visual.Pose(Vector3.zero,0f,1f+melt*.65f,Mathf.Max(.07f,1f-melt));
            spriteRenderer.color=Color.Lerp(new Color(.63f,.39f,.72f),new Color(.16f,.12f,.22f,0f),Mathf.InverseLerp(.25f,1f,deathAge));
            if(deathAge>=1f)Despawn();
        }
        private void UpdateAttack(float dt)
        {
            attackAge += dt;
            float travel, lean, offset, height;
            if (attackAge < .32f)
            {
                float t = Mathf.SmoothStep(0, 1, attackAge / .32f);
                travel = -.25f * t; lean = attackDirection * 8f * t; offset = -attackDirection * .18f * t; height = 1f - .05f * t;
                spriteRenderer.sprite = walkFrames[Mathf.Min(2, walkFrames.Length - 1)];
            }
            else if (attackAge < .50f)
            {
                float t = Mathf.SmoothStep(0, 1, (attackAge - .32f) / .18f);
                travel = Mathf.Lerp(-.25f, .30f, t); lean = Mathf.Lerp(attackDirection * 8f, -attackDirection * 11f, t);
                offset = Mathf.Lerp(-attackDirection * .18f, attackDirection * .24f, t); height = Mathf.Lerp(.95f, 1.04f, t);
                spriteRenderer.sprite = walkFrames[Mathf.Min(1, walkFrames.Length - 1)];
            }
            else
            {
                float t = Mathf.SmoothStep(0, 1, (attackAge - .50f) / .35f);
                travel = .30f * (1f - t); lean = -attackDirection * 11f * (1f - t); offset = attackDirection * .24f * (1f - t); height = Mathf.Lerp(1.04f, 1, t);
                spriteRenderer.sprite = walkFrames[0];
            }
            Vector2 foot = transform.position;
            if (authored) { AnimationAction = ZombieAction.Attack; spriteRenderer.sprite = animationSet.Attack(Kind, attackAge); }
            transform.position = encounter.MoveActor(foot, Vector2.right * (attackOrigin.x + attackDirection * travel - foot.x));
            visual.Pose(Vector3.right * offset, lean, 1.03f, height);
            if (!attackResolved && attackAge >= .44f)
            {
                attackResolved = true;
                Vector2 target = encounter.GrandmaFootPosition;
                if (Mathf.Abs(target.y - transform.position.y) <= .46f && BeltSpace.CanHit(transform.position, target, attackDirection, 1.05f, encounter.Obstacles))
                {
                    if (!contacted) { contacted = true; encounter.RegisterContact(); }
                    encounter.DamageDefense(4);
                }
            }
            if (attackAge >= .85f) { attackAge = -1; visual.Pose(Vector3.zero); }
        }
        private void Face() { spriteRenderer.flipX = authored ? direction < 0 : direction > 0; }
        private void Present(ZombieAction action, float time, bool loop)
        { AnimationAction = action; spriteRenderer.sprite = animationSet.Clip(Kind, action).Sample(time, loop); }
    }
}
