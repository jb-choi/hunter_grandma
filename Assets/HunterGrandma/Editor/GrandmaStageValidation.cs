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
        private static readonly string Profile = Path.GetFullPath("Library/BeltScroll-validation.json");
        private static readonly List<string> checks = new List<string>(), errors = new List<string>(), runs = new List<string>();
        private static GrandmaDefense owner;
        private static DefenseHud hud;
        private static Keyboard keyboard;
        private static int step, completed, startingCaptives, subduedShots;
        private static int inputFrame;
        private static bool seenSubdued, seenLoad, seenReturn, seenMelt, seenLeft, repeat;
        private static double deadline, waitUntil;
        private static string firstDog, firstClear;
        private static DefensePhase previousPhase;
        private static Vector2 movementStart, subduedFoot, autoStart;
        private static float movementAt, cameraStart;
        private static int offLaneHealth, shotKills;
        private static ZombieActor offLaneZombie;
        private static bool dodgePending, dodgeDone;
        private static float bossStarted;
        private static Vector2 dodgeStart;
        private static int dodgeHealth;
        static GrandmaStageValidation()
        {
            if (SessionState.GetBool(RunningKey, false)) DefenseProgressStore.EditorValidationPath = Profile;
            EditorApplication.update += Tick; Application.logMessageReceived += OnLog;
            EditorApplication.playModeStateChanged += state =>
            {
                if (state == PlayModeStateChange.EnteredEditMode)
                { SessionState.SetBool(RunningKey, false); DefenseProgressStore.EditorValidationPath = null; }
            };
        }
        [MenuItem("Tools/Hunter Grandma/Run Stage v2 Validation")]
        [MenuItem("Tools/Hunter Grandma/Run Belt Scroll Validation")]
        public static void Begin()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().isDirty) { Debug.LogError("Save the scene before Stage validation."); return; }
            GrandmaStageSetup.Configure();
            checks.Clear(); errors.Clear(); runs.Clear(); owner = null; hud = null; keyboard = null;
            step = completed = startingCaptives = subduedShots = 0;
            seenSubdued = seenLoad = seenReturn = seenMelt = seenLeft = repeat = false;
            dodgePending=dodgeDone=false;
            if (File.Exists(Profile)) File.Delete(Profile);
            if (File.Exists(Profile + ".bak")) File.Delete(Profile + ".bak");
            DefenseProgressStore.EditorValidationPath = Profile;
            SessionState.SetBool(RunningKey, true); EditorSceneManager.OpenScene(GrandmaProjectSetup.ScenePath); EditorApplication.EnterPlaymode();
        }
        private static void OnLog(string text, string stack, LogType type)
        {
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
            hud?.RefreshPresentation();
            GrandmaCombatValidation.Capture("Docs/Validation/BeltScroll-" + name + ".png", width, height);
        }
        private static void ModelChecks()
        {
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
                if (step == 0)
                {
                    // Let the first game frames finish before injecting dynamic-update input.
                    if (owner.Elapsed < .3f) return;
                    deadline = EditorApplication.timeSinceStartup + 420; ModelChecks(); Time.timeScale = 8f;
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
                    Check(GameObject.Find("Stage 0 home") != null, "Stage 0 is separately selectable outside the six city pins");
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
                    Snapshot("auto-hunt");previousPhase=owner.Phase;startingCaptives=0;step=3;return;
                }
                if (step == 3)
                {
                    foreach (ZombieActor zombie in UnityEngine.Object.FindObjectsByType<ZombieActor>(FindObjectsSortMode.None))
                    {
                        if (!seenMelt && zombie.IsMelting) { seenMelt = true; Snapshot("zombie-melt"); }
                        if (owner.CurrentStageId == 4 && zombie.IsAlive && zombie.transform.position.x < owner.GrandmaX) seenLeft = true;
                    }
                    if (owner.Phase == DefensePhase.Failed) throw new Exception("Natural combat failed at Stage " + owner.CurrentStageId + "; health depleted before capture");
                    if (owner.Phase != previousPhase)
                    {
                        previousPhase = owner.Phase;
                        if (owner.Phase == DefensePhase.BossFight)
                        {
                            Snapshot("boss-"+owner.CurrentStageId);
                            if(owner.CurrentStageId==0&&!repeat&&!dodgeDone){dodgePending=true;bossStarted=owner.Elapsed;if(owner.AutoExplore)owner.ToggleAutoExplore();}
                        }
                        if (owner.Phase == DefensePhase.Subdued)
                        {
                            subduedFoot=owner.Boss.transform.position;
                            seenSubdued = true; subduedShots = owner.ShotsFired;
                            Check(owner.Boss.IsSubdued && owner.Boss.gameObject.activeSelf && owner.Progress.CaptiveCount == startingCaptives, "Stage " + owner.CurrentStageId + ": subdual keeps the host body without granting an early capture");
                            Check(!owner.CanOpenMap, "Stage " + owner.CurrentStageId + ": map travel is blocked during capture");
                            if (!repeat) Check(owner.Boss.PatternAttackCount > 0, "Stage " + owner.CurrentStageId + ": signature boss attack executed");
                            if (owner.CurrentStageId == 6) Check(owner.Boss.HealUses > 0 && owner.Boss.HealUses <= 2, "Doctor uses bounded self-heal and summons");
                            if (owner.CurrentStageId == 1) Snapshot("subdued-host");
                        }
                        if (owner.Phase == DefensePhase.LoadingCart)
                        {
                            seenLoad=true;
                            Check(Vector2.Distance(subduedFoot,owner.Boss.transform.position)<.001f,"Subdued boss stays at its belt foot position until loading starts");
                        }
                        if (owner.Phase == DefensePhase.Returning)
                        {
                            seenReturn = true;
                            Check(owner.Boss.transform.parent.name == "Capture cart" && owner.ShotsFired == subduedShots, "Stage " + owner.CurrentStageId + ": intact host is loaded on the cart and shooting stays stopped");
                            Transform cart=owner.Boss.transform.parent;
                            Check(Mathf.Abs(cart.position.x-(subduedFoot.x-.8f))<.15f&&Mathf.Abs(cart.position.y-subduedFoot.y)<.15f,"Cart loads at the actual subdual X/Y location");
                            Check(owner.Progress.CaptiveCount == startingCaptives, "Stage " + owner.CurrentStageId + ": returning cart has not yet settled a reward");
                            if (owner.CurrentStageId == 1) Snapshot("cart-return");
                        }
                    }
                    if(dodgePending&&owner.Phase==DefensePhase.BossFight&&owner.Elapsed-bossStarted>=.7f)
                    {
                        dodgeStart=owner.GrandmaFootPosition;dodgeHealth=owner.DefenseHealth;
                        InputSystem.QueueStateEvent(keyboard,new KeyboardState(dodgeStart.y>BeltSpace.CenterDepth?Key.S:Key.W));step=19;return;
                    }
                    if (owner.Phase == DefensePhase.Clear)
                    {
                        Check(seenSubdued && seenLoad && seenReturn && owner.AtYard && owner.Run.Settled, "Stage " + owner.CurrentStageId + ": full subdual, loading, return, yard, CLEAR sequence");
                        Check(owner.LastCaptive != null && owner.Progress.CaptiveCount == startingCaptives + 1 && owner.LastCaptive.returnedToYard, "Stage " + owner.CurrentStageId + ": exactly one new captive is settled at home");
                        runs.Add("Stage " + owner.CurrentStageId + ": " + owner.Elapsed.ToString("F1") + " simulated seconds, " + owner.KillCount + " kills, " + owner.ShotsFired + " shots, " + owner.BreachCount + " breaches, " + owner.DefenseHealth + " HP, boss attacks " + owner.Boss.PatternAttackCount);
                        if (owner.CurrentStageId == 0 && !repeat) { firstDog = owner.LastCaptive.captiveId; firstClear = owner.Progress.Stage(0).firstClearAt; Snapshot("yard-dog"); }
                        if (owner.CurrentStageId == 1) Snapshot("yard-human");
                        if (owner.CurrentStageId == 6 && !repeat) Snapshot("all-stages-clear");
                        if (repeat)
                        {
                            Check(owner.Progress.Stage(0).clearCount == 2 && owner.Progress.Stage(0).capturedCount == 2 && owner.LastCaptive.captiveId != firstDog && owner.Progress.Stage(0).firstClearAt == firstClear, "Actual Stage 0 replay adds a different dog and preserves the first clear");
                            Check(owner.LastCaptive.appearance.StartsWith("동네 강아지"), "Repeat dog is a new neighborhood animal rather than resurrected Kongi");
                            Click("지도  M"); Click("Region 5"); Snapshot("map-unlocked"); Snapshot("map-unlocked-4x3", 1024, 768);
                            Check(Button("Travel to selected region").interactable, "Cleared Stage 6 remains selectable for repeat capture");
                            Click("Close map"); Check(owner.EnterStage(1), "Cleared district can be re-entered"); step = 4; return;
                        }
                        completed++; startingCaptives = owner.Progress.CaptiveCount; seenSubdued = seenLoad = seenReturn = false;
                        if (completed == 7) { repeat = true; Check(owner.EnterStage(0), "All seven stages cleared; replay Stage 0"); }
                        else
                        {
                            Check(owner.Progress.IsUnlocked(completed), "First clear unlocks Stage " + completed);
                            if (completed < 6) Check(!owner.Progress.IsUnlocked(completed + 1), "Later stages stay locked after Stage " + (completed - 1));
                            owner.TryUpgradeAttack(); owner.TryUpgradeFireRate();
                            Check(owner.EnterStage(completed), "Enter next unlocked stage " + completed);
                        }
                        previousPhase = owner.Phase;
                    }
                    return;
                }
                if(step==19&&owner.Elapsed-bossStarted>=3.1f)
                {
                    Check(owner.Boss.PatternAttackCount>0&&Mathf.Abs(owner.GrandmaFootPosition.y-dodgeStart.y)>.9f,"Movement keys leave the depth lane targeted by the boss telegraph");
                    Check(owner.DefenseHealth==dodgeHealth,"A boss charge misses after grandma moves out of its depth lane");
                    Snapshot("boss-depth-dodge");InputSystem.QueueStateEvent(keyboard,new KeyboardState());
                    if(!owner.AutoExplore)owner.ToggleAutoExplore();dodgePending=false;dodgeDone=true;previousPhase=owner.Phase;step=3;return;
                }
                if (step == 4 && owner.KillCount > 0)
                {
                    int gold = owner.Progress.Gold, count = owner.Progress.CaptiveCount;
                    Check(owner.Retreat() && owner.Phase == DefensePhase.Retreated && owner.Progress.Gold == gold && owner.Progress.CaptiveCount == count && owner.Progress.Stage(1).clearCount == 1, "Retreat preserves earned gold and historical clears/captures without a new captive");
                    owner.RestartEncounter(); owner.DamageDefense(100);
                    Check(owner.Phase == DefensePhase.Failed && owner.Progress.Gold == gold && owner.Progress.CaptiveCount == count, "Defense failure grants no captive and keeps earned progress");
                    var saved = new DefenseProgressStore(Profile).Load();
                    Check(saved.CaptiveCount == 8 && saved.Stage(0).clearCount == 2 && saved.Stage(6).clearCount == 1 && saved.Gold == gold, "Disk reload retains eight unique captives, all stage clears, upgrades, and gold");
                    Check(saved.Captives.Select(c => c.captiveId).Distinct().Count() == 8 && saved.Captives.Select(c => c.sourceRunId).Distinct().Count() == 8, "All actual capture and run IDs are unique");
                    Check(seenMelt && seenLeft, "Normal zombies visibly melt; plaza waves approach from both sides");
                    owner.RestartEncounter(); Check(owner.Phase == DefensePhase.Waves && owner.KillCount == 0 && owner.Progress.CaptiveCount == 8, "Retry resets the encounter while keeping permanent progress");
                    Check(errors.Count == 0, "No runtime errors, exceptions, or assertions during the complete stage suite"); Finish(null);
                }
            }
            catch (Exception exception) { Finish(exception.Message + "\n" + exception.StackTrace); }
        }
        private static void Finish(string failure)
        {
            Directory.CreateDirectory("Docs/Validation");
            string report = "# 2D belt-scroll implementation validation\n\nUnity " + Application.unityVersion + " · " + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "\n\nStatus: " + (failure == null ? "PASS" : "FAIL") + "\n\nIsolated save: `Library/BeltScroll-validation.json`. The player profile is never reset or modified by this suite.\n\n";
            report += string.Join("\n", checks.Select(c => "- PASS: " + c)) + "\n\n" + string.Join("\n", runs.Select(r => "- " + r));
            if (failure != null) report += "\n\nFailure: " + failure;
            if (errors.Count > 0) report += "\n\nRuntime errors:\n" + string.Join("\n", errors);
            File.WriteAllText("Docs/Validation/BeltScroll-validation.md", report);
            if (keyboard != null && keyboard.added) InputSystem.RemoveDevice(keyboard);
            SessionState.SetBool(RunningKey, false); DefenseProgressStore.EditorValidationPath = null; Time.timeScale = 1f;
            Debug.Log("Hunter Grandma belt-scroll validation: " + (failure == null ? "PASS (" + checks.Count + " checks)" : "FAIL: " + failure));
            EditorApplication.ExitPlaymode();
        }
    }
}

