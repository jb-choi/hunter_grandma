using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

namespace HunterGrandma.Editor
{
    [InitializeOnLoad]
    public static class GrandmaStageValidation
    {
        private const string RunningKey = "HunterGrandma.StageV2.Validation";
        private const string BatchKey = "HunterGrandma.StageValidation.Batch";
        private const string BatchDoneKey = BatchKey + ".Done";
        private static readonly string Profile = Path.GetFullPath("Library/BeltScroll-validation.json");
        private static readonly List<string> checks = new List<string>(), errors = new List<string>(), runs = new List<string>();
        private static readonly List<string> environmentErrors = new List<string>();
        private static GrandmaDefense owner;
        private static DefenseHud hud;
        private static Keyboard keyboard;
        private static int step, completed, transitionGold, bossFreeFrames;
        private static int inputFrame;
        private static int[] observedKinds = new int[DefenseStages.Count];
        private static bool observedAuthoredAttack, observedAuthoredDeath, observedAuthoredWalk;
        private static bool seenMelt, seenLeft, repeat;
        private static double deadline, waitUntil;
        private static string firstClear, previousRun;
        private static Vector2 movementStart, autoStart;
        private static float movementAt, cameraStart;
        private static int offLaneHealth, shotKills;
        private static ZombieActor offLaneZombie;
        private static float pausedClearRemaining;
        private static bool inputRoutingChanged;
        private static InputSettings.BackgroundBehavior previousBackgroundBehavior;
        private static InputSettings.EditorInputBehaviorInPlayMode previousEditorInputBehavior;
        static GrandmaStageValidation()
        {
            if (SessionState.GetBool(RunningKey, false)) DefenseProgressStore.EditorValidationPath = Profile;
            EditorApplication.update += Tick; EditorApplication.update += BatchTick; Application.logMessageReceived += OnLog;
            EditorApplication.playModeStateChanged += state =>
            {
                if (state == PlayModeStateChange.EnteredEditMode)
                { SessionState.SetBool(RunningKey, false); DefenseProgressStore.EditorValidationPath = null; }
            };
        }
        // CLI entry point for an isolated project copy; never closes an interactive Editor.
        public static void RunBatch()
        {
            if (!Application.isBatchMode) throw new InvalidOperationException("RunBatch requires Unity -batchmode.");
            SessionState.SetBool(BatchKey, true); SessionState.SetBool(BatchDoneKey, false);
            SessionState.SetFloat(BatchKey + ".Deadline", (float)EditorApplication.timeSinceStartup + 500f);
            EditorSceneManager.OpenScene(GrandmaProjectSetup.ScenePath);
            Begin();
        }
        private static void BatchTick()
        {
            if (!Application.isBatchMode || !SessionState.GetBool(BatchKey, false)) return;
            if (SessionState.GetBool(BatchDoneKey, false) && !EditorApplication.isPlayingOrWillChangePlaymode)
                EditorApplication.Exit(SessionState.GetInt(BatchKey + ".ExitCode", 1));
            else if (EditorApplication.timeSinceStartup > SessionState.GetFloat(BatchKey + ".Deadline", float.MaxValue))
            { Debug.LogError("Stage batch validation timed out."); EditorApplication.Exit(1); }
        }
        [MenuItem("Tools/Hunter Grandma/Run Stage v2 Validation")]
        [MenuItem("Tools/Hunter Grandma/Run Belt Scroll Validation")]
        public static void Begin()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().isDirty) { Debug.LogError("Save the scene before Stage validation."); return; }
            GrandmaStageSetup.Configure();
            checks.Clear(); errors.Clear(); runs.Clear(); environmentErrors.Clear(); owner = null; hud = null; keyboard = null;
            step = completed = transitionGold = bossFreeFrames = 0;
            seenMelt = seenLeft = repeat = false;
            observedKinds = new int[DefenseStages.Count];
            observedAuthoredAttack = observedAuthoredDeath = observedAuthoredWalk = false;
            if (File.Exists(Profile)) File.Delete(Profile);
            if (File.Exists(Profile + ".bak")) File.Delete(Profile + ".bak");
            DefenseProgressStore.EditorValidationPath = Profile;
            SessionState.SetBool(RunningKey, true); EditorSceneManager.OpenScene(GrandmaProjectSetup.ScenePath); EditorApplication.EnterPlaymode();
        }
        private static void OnLog(string text, string stack, LogType type)
        {
            if (Application.isBatchMode && SessionState.GetBool(RunningKey, false) && stack.Contains("UnityEditor.Search."))
            { environmentErrors.Add(text + "\n" + stack); return; }
            if (SessionState.GetBool(RunningKey, false) && (type == LogType.Error || type == LogType.Exception || type == LogType.Assert)) errors.Add(text + "\n" + stack);
        }
        private static void Check(bool value, string description)
        {
            if (!value) throw new Exception(description);
            checks.Add(description);
        }
        private static Button Button(string name) => GameObject.Find(name)?.GetComponent<Button>() ?? throw new Exception("Missing UI button: " + name);
        private static void Click(string name) { hud?.RefreshPresentation(); Button button = Button(name); Check(button.IsInteractable(), "Clickable UI: " + name); button.onClick.Invoke(); }
        private static void Snapshot(string name, int width = 1280, int height = 720)
        {
            if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null) return;
            hud?.RefreshPresentation();
            GrandmaCombatValidation.Capture("Docs/Validation/BeltScroll-" + name + ".png", width, height);
        }
        private static void ModelChecks()
        {
            ZombieRosterValidation.ModelChecks(Check);
            var progress = new DefenseProgress();
            Check(progress.IsUnlocked(0) && !progress.IsUnlocked(1) && !progress.IsUnlocked(-1) && !progress.IsUnlocked(7), "New profile unlocks only Stage 0; invalid stage IDs are rejected");
            Check(!progress.TryUpgradeAttack() && !progress.TryUpgradeFireRate(), "Upgrades cannot spend unavailable gold");
            var run = new DefenseRun(0);
            Check(!progress.TryComplete(run, "test", out _), "No capture before boss subdual");
            run.SubdueBoss(); Check(!progress.TryComplete(run, "test", out _), "No capture before yard arrival");
            run.ReturnToYard(); Check(progress.TryComplete(run, "dog A", out DefenseCaptive first), "Yard arrival settles one captive");
            Check(!progress.TryComplete(run, "duplicate", out _) && progress.CaptiveCount == 1, "The same run cannot settle twice");
            var second = new DefenseRun(0); second.SubdueBoss(); second.ReturnToYard();
            Check(progress.TryComplete(second, "dog B", out DefenseCaptive other) && other.captiveId != first.captiveId && other.sourceRunId != first.sourceRunId, "Repeat clear creates a distinct captive and source run ID");
            Check(progress.Stage(0).clearCount == 2 && progress.Stage(0).capturedCount == 2 && progress.Stage(0).firstClearAt == first.capturedAt && progress.IsUnlocked(1) && !progress.IsUnlocked(2), "Repeat clear preserves first-clear timestamp and sequential unlock");
            var abandoned = new DefenseRun(1); abandoned.SubdueBoss(); abandoned.Abandon(); abandoned.ReturnToYard();
            Check(!progress.TryComplete(abandoned, "abandoned", out _) && progress.CaptiveCount == 2, "Abandoning a run prevents captive rewards");
            progress.AddGold(500); int oldDamage = progress.Damage; float oldInterval = progress.FireInterval;
            Check(progress.TryUpgradeAttack() && progress.Damage > oldDamage && progress.Gold == 482, "Attack upgrade spends gold and raises actual shot damage");
            Check(progress.TryUpgradeFireRate() && progress.FireInterval < oldInterval && progress.Gold == 460, "Fire-rate upgrade spends gold and shortens the actual shot interval");
            DefenseProgress restored = DefenseProgress.FromJson(progress.ToJson());
            Check(restored.Gold == progress.Gold && restored.Captives[1].captiveId == other.captiveId && restored.Stage(0).clearCount == 2, "Save JSON round-trip preserves gold, IDs, and stage records");
            var replay = new DefenseRun(0); replay.CompleteWaves();
            Check(restored.TryCompleteWaves(replay) && restored.Stage(0).clearCount == 3 && restored.Stage(0).capturedCount == 2 && restored.CaptiveCount == 2 && restored.Stage(0).firstClearAt == first.capturedAt, "Wave-only completion preserves legacy captives and first-clear time without granting another captive");
            DefenseProgress mixed = DefenseProgress.FromJson(restored.ToJson());
            Check(mixed.Stage(0).clearCount == 3 && mixed.Captives[1].captiveId == other.captiveId && mixed.ShadowSkillEnabled == restored.ShadowSkillEnabled, "Mixed historical captures and wave-only clears round-trip in the existing save schema");
            var waves = new DefenseProgress();
            var incomplete = new DefenseRun(0);
            Check(!waves.TryCompleteWaves(null) && !waves.TryCompleteWaves(incomplete), "Missing and unfinished wave runs cannot unlock a stage");
            var locked = new DefenseRun(2); locked.CompleteWaves();
            var invalid = new DefenseRun(DefenseStages.Count); invalid.CompleteWaves();
            Check(!waves.TryCompleteWaves(locked) && !waves.TryCompleteWaves(invalid), "Wave completion rejects locked or invalid stage IDs");
            var cancelled = new DefenseRun(0); cancelled.CompleteWaves(); cancelled.Abandon();
            Check(!waves.TryCompleteWaves(cancelled), "Retreat or failure cannot settle a marked wave run");
            for (int id = 0; id < DefenseStages.Count; id++)
            {
                var waveRun = new DefenseRun(id); waveRun.CompleteWaves();
                Check(waves.TryCompleteWaves(waveRun) && waveRun.Settled && waves.Stage(id).clearCount == 1 && waves.Stage(id).capturedCount == 0 && waves.CaptiveCount == 0, "Stage " + id + ": wave-only settlement records one clear and no captive");
                Check(!waves.TryCompleteWaves(waveRun) && !waves.TryComplete(waveRun, "duplicate", out _) && waves.Stage(id).clearCount == 1, "Stage " + id + ": the same wave run cannot settle twice or grant a legacy capture");
            }
            DefenseProgress noCaptives = DefenseProgress.FromJson(waves.ToJson());
            Check(noCaptives.IsUnlocked(DefenseStages.Count - 1) && noCaptives.Stage(DefenseStages.Count - 1).clearCount == 1 && noCaptives.CaptiveCount == 0, "Every wave-only stage clear survives JSON reload with sequential unlocks");
            DefenseSaveData badCounts = JsonUtility.FromJson<DefenseSaveData>(restored.ToJson()); badCounts.stages[0].clearCount = 1;
            bool rejectedCounts = false; try { DefenseProgress.FromJson(JsonUtility.ToJson(badCounts)); } catch (FormatException) { rejectedCounts = true; }
            Check(rejectedCounts, "A save with fewer clears than historical captive records is rejected");
            DefenseSaveData badUnlock = JsonUtility.FromJson<DefenseSaveData>(waves.ToJson()); badUnlock.stages[0].clearCount = 0;
            bool rejectedUnlock = false; try { DefenseProgress.FromJson(JsonUtility.ToJson(badUnlock)); } catch (FormatException) { rejectedUnlock = true; }
            Check(rejectedUnlock, "A save with cleared stages beyond a locked predecessor is rejected");
            bool rejected = false; try { DefenseProgress.FromJson("{\"version\":99}"); } catch (Exception) { rejected = true; }
            Check(rejected, "Unsupported save versions are rejected");
            var store = new DefenseProgressStore(Profile + ".model");
            Check(store.Save(progress) && store.Save(progress) && store.Load().CaptiveCount == 2 && File.Exists(Profile + ".model.bak"), "Atomic disk replacement produces a readable save and backup");
            File.WriteAllText(Profile + ".model", "{broken");
            Check(store.Load().CaptiveCount == 2, "A corrupt primary save recovers the last valid backup");
            progress.AddGold(int.MaxValue); for (int i = 0; i < 50; i++) { progress.TryUpgradeAttack(); progress.TryUpgradeFireRate(); }
            Check(progress.AttackLevel == 30 && progress.FireRateLevel == 15 && !progress.TryUpgradeAttack() && !progress.TryUpgradeFireRate(), "Upgrade limits prevent overflow purchases");
            Vector2 origin=new Vector2(0,-2.5f);Rect[] noCover=new Rect[0];
            Check(BeltSpace.CanHit(origin,new Vector2(5,-2.5f),1,7.5f,noCover),"Shotgun hits an aligned target inside range");
            Check(!BeltSpace.CanHit(origin,new Vector2(5,-.9f),1,7.5f,noCover),"A target in a different depth lane is outside the shotgun cone");
            Check(!BeltSpace.CanHit(origin,new Vector2(-2,-2.5f),1,7.5f,noCover)&&!BeltSpace.CanHit(origin,new Vector2(8,-2.5f),1,7.5f,noCover),"Shotgun rejects targets behind the shot or beyond its range");
            Check(BeltSpace.CanHit(origin,new Vector2(-5,-2.5f),-1,7.5f,noCover),"A left-facing shot has the same depth and range rules");
            Rect[] cover={new Rect(12,-2.65f,1.4f,.75f)};
            Check(!BeltSpace.CanHit(new Vector2(10,-2.3f),new Vector2(16,-2.3f),1,7.5f,cover),"Solid cover blocks shotgun hits behind it");
            Check(BeltSpace.Move(new Vector2(10,-2.3f),Vector2.right*10,30,cover).x<12,"Movement cannot tunnel through a cover footprint");
            Vector2 route=new Vector2(10,-2.3f),goal=new Vector2(16,-2.3f);
            for(int i=0;i<250;i++){Vector2 waypoint=BeltSpace.Waypoint(route,goal,cover);route=BeltSpace.Move(route,Vector2.MoveTowards(route,waypoint,.10f)-route,30,cover);}
            Check(Vector2.Distance(route,goal)<.15f,"Automatic navigation goes around cover and reaches the other side");
            Check(BeltSpace.Clamp(new Vector2(100,100),30)==new Vector2(30,BeltSpace.MaxDepth)&&BeltSpace.Clamp(new Vector2(-100,-100),30)==new Vector2(BeltSpace.MinX,BeltSpace.MinDepth),"Belt world boundaries clamp both movement axes");
            Check(BeltSpace.SortOrder(-3.5f)>BeltSpace.SortOrder(-1.5f),"Lower feet render in front of upper feet independent of sprite height");
        }
        private static void Tick()
        {
            if (!SessionState.GetBool(RunningKey, false) || !EditorApplication.isPlaying || EditorApplication.isCompiling) return;
            try
            {
                if (owner == null) owner = UnityEngine.Object.FindFirstObjectByType<GrandmaDefense>();
                if (owner == null || owner.Progress == null) return;
                foreach (ZombieActor zombie in UnityEngine.Object.FindObjectsByType<ZombieActor>(FindObjectsSortMode.None))
                {
                    if (!zombie.gameObject.activeSelf || zombie.AnimationSet == null) continue;
                    if (zombie.IsAlive) observedKinds[owner.CurrentStageId] |= 1 << (int)zombie.Kind;
                    observedAuthoredAttack |= zombie.IsAlive && zombie.AnimationAction == ZombieAction.Attack;
                    observedAuthoredWalk |= zombie.IsAlive && (zombie.AnimationAction == ZombieAction.Walk || zombie.AnimationAction == ZombieAction.Run);
                    observedAuthoredDeath |= zombie.IsMelting && zombie.AnimationAction == ZombieAction.Dead;
                }
                if (step == 0)
                {
                    // Let the first game frames finish before injecting dynamic-update input.
                    if (owner.Elapsed < .3f) return;
                    deadline = EditorApplication.timeSinceStartup + 420; ModelChecks(); Time.timeScale = 8f;
                    if (Application.isBatchMode)
                    {
                        // There is no focused Game View in a CLI process; route only this fixture's virtual device to the player.
                        previousBackgroundBehavior = InputSystem.settings.backgroundBehavior;
                        previousEditorInputBehavior = InputSystem.settings.editorInputBehaviorInPlayMode;
                        InputSystem.settings.backgroundBehavior = InputSettings.BackgroundBehavior.IgnoreFocus;
                        InputSystem.settings.editorInputBehaviorInPlayMode = InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;
                        inputRoutingChanged = true;
                    }
                    Check(owner.CurrentStageId == 0 && owner.ActiveZombieCount >= 3 && owner.PoolSize <= 24, "Playable scene starts Stage 0 with live zombies and a bounded pool");
                    Check(!owner.EnterStage(1), "Gameplay rejects locked district travel");
                    hud = UnityEngine.Object.FindFirstObjectByType<DefenseHud>(); keyboard = InputSystem.AddDevice<Keyboard>("Stage validation keyboard");
                    keyboard.MakeCurrent(); InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.M)); inputFrame = Time.frameCount;
                    step = 1; waitUntil = EditorApplication.timeSinceStartup + 8; return;
                }
                if (EditorApplication.timeSinceStartup > deadline) throw new Exception("Stage validation exceeded its 420-second timeout at Stage " + owner.CurrentStageId + " / " + owner.Phase);
                if (step == 1 && (!hud.IsMapOpen || Time.frameCount < inputFrame + 2))
                {
                    if (EditorApplication.timeSinceStartup > waitUntil) throw new Exception("M did not open the map after game frames advanced: " + (Time.frameCount - inputFrame));
                    return;
                }
                if (step == 1)
                {
                    Check(hud.IsMapOpen && owner.IsPaused, "M opens the map and pauses the encounter");
                    Click("Region 0"); Check(!Button("Travel to selected region").interactable, "Locked map district shows a disabled travel button");
                    var pointer = new PointerEventData(EventSystem.current) { position = RectTransformUtility.WorldToScreenPoint(null, Button("Region 0").transform.position) };
                    var hits = new List<RaycastResult>(); EventSystem.current.RaycastAll(pointer, hits);
                    Check(hits.Count > 0 && hits[0].gameObject.GetComponentInParent<Button>() == Button("Region 0"), "Map pin receives the actual pointer raycast above the modal backdrop");
                    Snapshot("map-locked"); Snapshot("map-locked-4x3", 1024, 768);
                    Check(GameObject.Find("Stage 0 home") != null && GameObject.Find("Region 6") != null, "Stage 0 is separately selectable alongside seven city pins, including Stage 7");
                    InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.Escape)); inputFrame = Time.frameCount;
                    step = 2; waitUntil = EditorApplication.timeSinceStartup + 8; return;
                }
                if (step == 2 && (hud.IsMapOpen || Time.frameCount < inputFrame + 2))
                {
                    if (EditorApplication.timeSinceStartup > waitUntil) throw new Exception("Escape did not close the map after game frames advanced: " + (Time.frameCount - inputFrame));
                    return;
                }
                if (step == 2)
                {
                    Check(!hud.IsMapOpen && !owner.IsPaused && Mathf.Approximately(Time.timeScale, 8f), "Escape closes the map and restores the prior game speed");
                    owner.TogglePause(); Click("지도  M"); Click("Close map");
                    Check(owner.IsPaused && Mathf.Approximately(Time.timeScale, 0f), "Closing a map opened while already paused preserves pause");
                    owner.TogglePause();if(owner.AutoExplore)owner.ToggleAutoExplore();
                    // Start movement checks away from the edge, independent of the first target's depth.
                    UnityEngine.Object.FindFirstObjectByType<GrandmaShooter>().MoveOnBelt(BeltSpace.Start);
                    movementStart=owner.GrandmaFootPosition;movementAt=owner.Elapsed;
                    InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.W,Key.D));step=10;return;
                }
                if(step==10&&owner.Elapsed-movementAt>=.8f)
                {
                    Check(owner.GrandmaFootPosition.x>movementStart.x+1&&owner.GrandmaFootPosition.y>movementStart.y+.7f,"W+D moves grandma right and into the belt depth");
                    movementStart=owner.GrandmaFootPosition;movementAt=owner.Elapsed;InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.S,Key.A));step=11;return;
                }
                if(step==11&&owner.Elapsed-movementAt>=.8f)
                {
                    Check(owner.GrandmaFootPosition.x<movementStart.x-1&&owner.GrandmaFootPosition.y<movementStart.y-.7f,"S+A moves grandma left and toward the foreground");
                    movementAt=owner.Elapsed;InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.S,Key.D));step=12;return;
                }
                if(step==12&&owner.Elapsed-movementAt>=2f)
                {
                    Check(Mathf.Abs(owner.GrandmaFootPosition.y-BeltSpace.MinDepth)<.01f,"Held downward input stops at the near belt boundary");
                    movementAt=owner.Elapsed;InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.W,Key.D));step=13;return;
                }
                if(step==13&&owner.Elapsed-movementAt>=2f)
                {
                    Check(Mathf.Abs(owner.GrandmaFootPosition.y-BeltSpace.MaxDepth)<.01f,"Held upward input stops at the far belt boundary");
                    cameraStart=Camera.main.transform.position.x;movementAt=owner.Elapsed;InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.D));step=14;return;
                }
                if(step==14&&owner.Elapsed-movementAt>=2.5f)
                {
                    Check(Camera.main.transform.position.x>cameraStart+2f,"Camera scrolls horizontally with forward travel");
                    Check(owner.GrandmaFootPosition.x<=owner.WaveAnchor+12.01f,"Current encounter limits prevent skipping the active wave");
                    Snapshot("belt-travel");Snapshot("belt-travel-4x3",1024,768);
                    Click("지도  M");movementStart=owner.GrandmaFootPosition;waitUntil=EditorApplication.timeSinceStartup+.35;
                    InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.W,Key.D));step=15;return;
                }
                if(step==15&&EditorApplication.timeSinceStartup>=waitUntil)
                {
                    Check(Vector2.Distance(owner.GrandmaFootPosition,movementStart)<.001f,"Movement input is blocked while the map owns pause");
                    InputSystem.QueueStateEvent(keyboard,new KeyboardState());Click("Close map");inputFrame=Time.frameCount;step=16;return;
                }
                if(step==16&&Time.frameCount>=inputFrame+3)
                {
                    owner.RestartEncounter();owner.TogglePause();
                    ZombieActor[] actors=UnityEngine.Object.FindObjectsByType<ZombieActor>(FindObjectsSortMode.None).Where(z=>z.IsAlive).ToArray();
                    // Respawn the depth-test pair so Stage 0's fence assignment cannot pull teleported actors back.
                    actors[0].Spawn(owner,new Vector2(2,-1.8f),0,0,owner.CurrentStage.ZombieHealth,owner.CurrentStage.GoldPerKill,1);
                    actors[1].Spawn(owner,new Vector2(2,-2.7f),0,0,owner.CurrentStage.ZombieHealth,owner.CurrentStage.GoldPerKill,1);
                    foreach(ZombieActor actor in actors)actor.GetComponent<BeltActorVisual>().RefreshDepth();
                    Check(actors[1].GetComponent<BeltActorVisual>().Group.sortingOrder>actors[0].GetComponent<BeltActorVisual>().Group.sortingOrder,"Overlapping live actors use feet to determine their actual sorting groups");
                    Check(actors.All(a=>a.GetComponent<BeltActorVisual>().Shadow.enabled),"Every live alien has an independent grounded foot shadow");
                    Snapshot("depth-overlap");
                    UnityEngine.Object.FindFirstObjectByType<GrandmaShooter>().MoveOnBelt(new Vector2(-2,-2.45f));
                    actors[0].transform.position=new Vector2(2,-2.45f);actors[1].transform.position=new Vector2(2,-.95f);
                    for(int i=2;i<actors.Length;i++)actors[i].Despawn();
                    offLaneZombie=actors[1];offLaneHealth=offLaneZombie.Health;shotKills=owner.KillCount;
                    owner.TogglePause();movementAt=owner.Elapsed;step=17;return;
                }
                if(step==17&&owner.Elapsed-movementAt>=.75f)
                {
                    Check(owner.KillCount>shotKills&&offLaneZombie.Health==offLaneHealth,"A real shotgun flight melts the aligned alien while the off-lane alien remains unharmed");
                    owner.RestartEncounter();InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.Tab));autoStart=owner.GrandmaFootPosition;step=18;return;
                }
                if(step==18&&owner.Elapsed>=5f)
                {
                    Check(owner.AutoExplore,"Tab input turns automatic exploration back on");
                    InputSystem.QueueStateEvent(keyboard,new KeyboardState());
                    Check(Mathf.Abs(owner.GrandmaFootPosition.x-autoStart.x)>.5f&&Mathf.Abs(owner.GrandmaFootPosition.y-autoStart.y)>.1f&&owner.ShotsFired>0,"Automatic hunting approaches enemies on X/Y and fires without movement input");
                    Snapshot("auto-hunt");step=3;return;
                }
                if (step == 3 || step == 20 || step == 21 || step == 22)
                {
                    bossFreeFrames++;
                    if (owner.Boss.gameObject.activeSelf || owner.IsCaptureSequence || owner.Phase == DefensePhase.BossArrival || owner.Phase == DefensePhase.BossFight)
                        throw new Exception("A boss/capture phase appeared during wave-only progression");
                    if (owner.Phase == DefensePhase.Failed)
                        throw new Exception("Natural wave combat failed at Stage " + owner.CurrentStageId);
                }
                if (step == 3)
                {
                    foreach (ZombieActor zombie in UnityEngine.Object.FindObjectsByType<ZombieActor>(FindObjectsSortMode.None))
                    {
                        if (!seenMelt && zombie.IsMelting) { seenMelt = true; Snapshot("zombie-melt"); }
                        if (owner.CurrentStageId == 4 && zombie.IsAlive && zombie.transform.position.x < owner.GrandmaX) seenLeft = true;
                    }
                    if (owner.Phase != DefensePhase.Clear) return;
                    int id = owner.CurrentStageId;
                    Check(observedKinds[id] == (id == 0 ? 2 : id == 1 ? 11 : id < 5 ? 27 : 31), "Stage " + id + ": natural runtime spawns contain exactly the requested zombie variants");
                    Check(owner.Run.WavesCleared && owner.Run.Settled && !owner.Run.BossSubdued && !owner.Run.ReturnedToYard && !owner.AtYard, "Stage " + id + ": waves settle directly to CLEAR at the hunting location");
                    Check(owner.WaveNumber == owner.CurrentStage.WaveCount && owner.KillCount == owner.CurrentStage.WaveCount * owner.CurrentStage.EnemiesPerWave && owner.ActiveZombieCount == 0, "Stage " + id + ": every configured normal zombie is defeated before CLEAR");
                    Check(owner.Progress.CaptiveCount == 0 && owner.Progress.Stage(id).capturedCount == 0 && owner.LastCaptive == null && !owner.Boss.gameObject.activeSelf, "Stage " + id + ": no boss, cart settlement or fabricated captive");
                    Check(owner.Progress.Stage(id).clearCount == (repeat ? 2 : 1), "Stage " + id + ": exactly one clear is credited for this run");
                    Check(!owner.CanMoveOnBelt && !owner.IsCombatPhase && !GameObject.Find("Boss health panel"), "Stage " + id + ": CLEAR stops movement/combat and has no boss HUD");
                    var disk = new DefenseProgressStore(Profile).Load();
                    Check(disk.Stage(id).clearCount == owner.Progress.Stage(id).clearCount && disk.CaptiveCount == 0, "Stage " + id + ": wave-only clear is saved before advancing");
                    runs.Add("Stage " + id + ": " + owner.Elapsed.ToString("F1") + " simulated seconds, " + owner.KillCount + " kills, " + owner.ShotsFired + " shots, " + owner.DefenseHealth + " HP; no boss/capture");
                    Snapshot("stage-clear-" + id);
                    if (repeat)
                    {
                        Check(owner.Progress.Stage(0).firstClearAt == firstClear && owner.Progress.Stage(0).clearCount == 2, "Replaying Stage 0 preserves first-clear time and increments only the clear count");
                        Click("지도  M"); Click("Region 6"); Snapshot("map-unlocked"); Snapshot("map-unlocked-4x3", 1024, 768);
                        Check(Button("Travel to selected region").interactable, "Completed Stage 7 remains selectable for another wave-only hunt");
                        Click("Close map"); Check(owner.EnterStage(1), "A completed district can be re-entered"); step = 4; return;
                    }
                    completed++;
                    if (id == 0) firstClear = owner.Progress.Stage(0).firstClearAt;
                    if (!owner.HasNextStage)
                    {
                        Check(completed == DefenseStages.Count && !owner.IsAdvancingStage && owner.StateText == "ALL STAGES CLEAR", "The last stage ends the finite campaign without a nonexistent next stage");
                        Snapshot("all-stages-clear"); waitUntil = EditorApplication.timeSinceStartup + .5; step = 22; return;
                    }
                    Check(owner.IsAdvancingStage && owner.Progress.IsUnlocked(id + 1), "Stage " + id + ": CLEAR unlocks the next stage and schedules automatic travel");
                    if (id + 2 < DefenseStages.Count) Check(!owner.Progress.IsUnlocked(id + 2), "Stage " + id + ": later stages stay locked");
                    // Spend only naturally earned funds, as in the previous full-stage suite.
                    owner.TryUpgradeAttack(); owner.TryUpgradeFireRate();
                    previousRun = owner.Run.Id; transitionGold = owner.Progress.Gold;
                    if (id == 0)
                    {
                        Click("지도  M"); pausedClearRemaining = owner.StageClearRemaining;
                        waitUntil = EditorApplication.timeSinceStartup + .35; step = 20;
                    }
                    else step = 21;
                    return;
                }
                if (step == 20 && EditorApplication.timeSinceStartup >= waitUntil)
                {
                    Check(owner.CurrentStageId == 0 && owner.IsPaused && Mathf.Abs(owner.StageClearRemaining - pausedClearRemaining) < .001f && owner.Progress.Stage(0).clearCount == 1, "Map pause freezes the CLEAR countdown without duplicating its settlement");
                    Click("Close map"); Check(!owner.IsPaused, "Closing the map resumes automatic stage transition"); step = 21; return;
                }
                if (step == 21)
                {
                    if (owner.CurrentStageId == completed - 1) return;
                    Check(owner.CurrentStageId == completed && owner.Phase == DefensePhase.Waves && owner.Run.Id != previousRun, "Automatically enter Stage " + completed + " without a map selection or EnterStage call");
                    Check(owner.Run.EarnedGold == 0 && owner.KillCount == 0 && owner.ShotsFired == 0 && owner.DefenseHealth == 100 && !owner.IsPaused, "Stage " + completed + ": new run counters/health reset while the campaign continues");
                    Check(owner.Progress.Gold == transitionGold && owner.Progress.CaptiveCount == 0 && owner.Progress.Stage(completed - 1).clearCount == 1, "Stage " + completed + ": gold, upgrades and the previous clear survive automatic travel");
                    step = 3; return;
                }
                if (step == 22 && EditorApplication.timeSinceStartup >= waitUntil)
                {
                    Check(owner.CurrentStageId == DefenseStages.Count - 1 && owner.Phase == DefensePhase.Clear && owner.Progress.Stage(DefenseStages.Count - 1).clearCount == 1 && owner.ActiveZombieCount == 0, "Final CLEAR remains stable for multiple transition intervals and grants no repeated clear");
                    Check(bossFreeFrames > 0, "Every observed natural combat/transition frame remained free of bosses and capture phases");
                    repeat = true; Check(owner.EnterStage(0), "Stage 0 and all seven city stages completed; replay Stage 0"); step = 3; return;
                }
                if (step == 4 && owner.KillCount > 0)
                {
                    int gold = owner.Progress.Gold;
                    Check(owner.Retreat() && owner.Phase == DefensePhase.Retreated && owner.Progress.Gold == gold && owner.Progress.CaptiveCount == 0 && owner.Progress.Stage(1).clearCount == 1, "Retreat preserves earned gold and historical clears without another clear or captive");
                    owner.RestartEncounter(); owner.DamageDefense(100);
                    Check(owner.Phase == DefensePhase.Failed && owner.Progress.Gold == gold && owner.Progress.CaptiveCount == 0 && owner.Progress.Stage(1).clearCount == 1, "Failure keeps progress and does not settle the unfinished wave run");
                    var saved = new DefenseProgressStore(Profile).Load();
                    Check(saved.CaptiveCount == 0 && saved.Stage(0).clearCount == 2 && saved.Stage(7).clearCount == 1 && saved.Gold == gold && saved.ShadowSkillEnabled == owner.Progress.ShadowSkillEnabled, "Disk reload retains all eight wave clears, replay count, gold and skill preference without captives");
                    Check(observedAuthoredAttack && observedAuthoredDeath && observedAuthoredWalk, "Actual pooled enemies presented authored movement, fence attack and death during natural play");
                    Check(seenMelt && seenLeft, "Normal zombies visibly melt; plaza waves approach from both sides");
                    owner.RestartEncounter(); Check(owner.Phase == DefensePhase.Waves && owner.KillCount == 0 && owner.Progress.CaptiveCount == 0, "Retry resets combat while keeping permanent progress");
                    Check(errors.Count == 0, "No runtime errors, exceptions or assertions in the complete boss-free stage suite"); Finish(null);
                }
            }
            catch (Exception exception) { Finish(exception.Message + "\n" + exception.StackTrace); }
        }
        private static void Finish(string failure)
        {
            Directory.CreateDirectory("Docs/Validation");
            string report = "# Boss-free automatic stage progression validation\n\nUnity " + Application.unityVersion + " · " + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "\n\nStatus: " + (failure == null ? "PASS" : "FAIL") + "\n\nIsolated save: `Library/BeltScroll-validation.json`. The player profile is never reset or modified by this suite. Ordinary combat, automatic travel and replay run naturally; earned funds buy upgrades between stages. Synthetic keyboard input is game-test input, without computer-use tools. Legacy capture model cases verify old-save compatibility only.\n\n";
            report += string.Join("\n", checks.Select(c => "- PASS: " + c)) + "\n\n" + string.Join("\n", runs.Select(r => "- " + r));
            if (failure != null) report += "\n\nFailure: " + failure;
            if (errors.Count > 0) report += "\n\nRuntime errors:\n" + string.Join("\n", errors);
            if (environmentErrors.Count > 0) report += "\n\nSeparate Unity Editor Search startup/indexing errors (not gameplay):\n" + string.Join("\n", environmentErrors);
            if (Application.isBatchMode) report += "\n\nExecution: isolated Unity batchmode / nographics. Native UI/computer-use tools and visual screenshot verification were not used.\n";
            File.WriteAllText("Docs/Validation/BeltScroll-validation.md", report);
            if (keyboard != null && keyboard.added) InputSystem.RemoveDevice(keyboard);
            if (inputRoutingChanged)
            {
                InputSystem.settings.backgroundBehavior = previousBackgroundBehavior;
                InputSystem.settings.editorInputBehaviorInPlayMode = previousEditorInputBehavior;
                inputRoutingChanged = false;
            }
            SessionState.SetBool(RunningKey, false); DefenseProgressStore.EditorValidationPath = null; Time.timeScale = 1f;
            Debug.Log("Hunter Grandma belt-scroll validation: " + (failure == null ? "PASS (" + checks.Count + " checks)" : "FAIL: " + failure));
            if (SessionState.GetBool(BatchKey, false))
            { SessionState.SetInt(BatchKey + ".ExitCode", failure == null ? 0 : 1); SessionState.SetBool(BatchDoneKey, true); }
            EditorApplication.ExitPlaymode();
        }
    }
}

