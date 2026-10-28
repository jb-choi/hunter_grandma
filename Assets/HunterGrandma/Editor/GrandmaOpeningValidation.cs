using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace HunterGrandma.Editor
{
    [InitializeOnLoad]
    public static class GrandmaOpeningValidation
    {
        private const string Running = "HunterGrandma.Stage0Opening.Validation";
        private static readonly string Profile = Path.GetFullPath("Library/Stage0-opening-validation.json");
        private static readonly List<string> checks = new List<string>(), errors = new List<string>();
        private static GrandmaDefense owner;
        private static GrandmaShooter grandma;
        private static ZombieActor attacker;
        private static Keyboard keyboard;
        private static int step, frame, balance, health;
        private static float introAge, waitUntil, originX;
        private static Vector2 heroFoot;
        private static Vector3 fencePose;
        private static double deadline;
        static GrandmaOpeningValidation()
        {
            if (SessionState.GetBool(Running, false)) DefenseProgressStore.EditorValidationPath = Profile;
            EditorApplication.update += Tick;
            Application.logMessageReceived += (message, stack, type) =>
            { if (SessionState.GetBool(Running, false) && (type == LogType.Error || type == LogType.Exception || type == LogType.Assert)) errors.Add(message + "\n" + stack); };
        }
        [MenuItem("Tools/Hunter Grandma/Run Stage 0 Opening Validation")]
        public static void Begin()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().isDirty) { Debug.LogError("Save the scene before opening validation."); return; }
            GrandmaStageSetup.Configure(); checks.Clear(); errors.Clear(); owner = null; grandma = null; attacker = null; keyboard = null; step = 0;
            foreach (string path in new[] { Profile, Profile + ".bak" }) if (File.Exists(path)) File.Delete(path);
            SessionState.SetBool(Running, true); DefenseProgressStore.EditorValidationPath = Profile;
            EditorSceneManager.OpenScene(GrandmaProjectSetup.ScenePath); EditorApplication.EnterPlaymode();
        }
        private static void Check(bool value, string text) { if (!value) throw new Exception(text); checks.Add(text); }
        private static void Snapshot(string name, int width = 1280, int height = 720)
        {
            UnityEngine.Object.FindFirstObjectByType<DefenseHud>().RefreshPresentation();
            GrandmaCombatValidation.Capture("Docs/Validation/Stage0-" + name + ".png", width, height);
        }
        private static void Tick()
        {
            if (!SessionState.GetBool(Running, false) || !EditorApplication.isPlaying || EditorApplication.isCompiling) return;
            try
            {
                if (owner == null) owner = UnityEngine.Object.FindFirstObjectByType<GrandmaDefense>();
                if (owner == null || owner.Progress == null) return;
                if (grandma == null) grandma = UnityEngine.Object.FindFirstObjectByType<GrandmaShooter>();
                if (step == 0)
                {
                    if (owner.OpeningAge < .5f) return;
                    deadline = EditorApplication.timeSinceStartup + 90;
                    Check(owner.IsOpening && owner.CurrentStageId == 0, "Stage 0 starts with the tea opening before combat");
                    Check(GameObject.Find("Opening Sundeok").GetComponent<SpriteRenderer>().sprite.name == "sundeok_tea", "Opening uses the fresh export of the user-specified seated tea sprite");
                    Check(GameObject.Find("Dark window").GetComponent<SpriteRenderer>().color.maxColorComponent < .01f, "Window is black before the exterior reveal");
                    Check(GameObject.Find("Unseen exterior").GetComponent<SpriteRenderer>().color.a > .99f, "Outside remains concealed while grandma drinks tea");
                    Check(owner.ActiveZombieCount == 0 && owner.ShotsFired == 0 && owner.Elapsed == 0, "Intro has no live attackers, shots or combat-clock progress");
                    Check(!grandma.GetComponent<BeltActorVisual>().Body.enabled, "Only the seated opening actor is shown during the intro");
                    Check(!owner.CanOpenMap && !owner.CanMoveOnBelt && !owner.EnterStage(1) && !owner.Retreat(), "Map travel, movement and retreat cannot interrupt the intro");
                    owner.DamageDefense(100); Check(owner.DefenseHealth == 100, "Opening cannot receive combat damage");
                    owner.Progress.AddGold(100); balance = owner.Progress.Gold;
                    Check(!owner.TryUpgradeAttack() && !owner.TryUpgradeFireRate() && owner.Progress.Gold == balance, "Upgrades cannot spend even affordable test funds during the intro");
                    Snapshot("01-tea"); Snapshot("01-tea-small", 800, 450);
                    keyboard = InputSystem.AddDevice<Keyboard>("Opening validation keyboard"); keyboard.MakeCurrent();
                    InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.W, Key.D));
                    owner.TogglePause(); introAge = owner.OpeningAge; heroFoot = owner.GrandmaFootPosition; waitUntil = Time.unscaledTime + .35f; step = 1; return;
                }
                if (EditorApplication.timeSinceStartup > deadline) throw new Exception("Opening validation timeout at step " + step);
                if (step == 1)
                {
                    if (Time.unscaledTime < waitUntil) return;
                    Check(Mathf.Approximately(owner.OpeningAge, introAge), "Pause freezes the intro timeline");
                    Check(Vector2.Distance(owner.GrandmaFootPosition, heroFoot) < .001f && owner.ShotsFired == 0, "Movement input during paused intro cannot move or fire the live grandma");
                    InputSystem.QueueStateEvent(keyboard, new KeyboardState()); owner.TogglePause(); step = 2; return;
                }
                if (step == 2)
                {
                    if (owner.OpeningAge < 4.5f) return;
                    Check(owner.IsOpening && owner.OpeningSoundCues >= 4, "Exterior reveal has a growl and repeated staggered fence strikes");
                    var posts = GameObject.Find("Opening fence").GetComponentsInChildren<SpriteRenderer>().Where(r => r.name == "Fence post").OrderBy(r => r.transform.position.x).ToArray();
                    Check(posts.Length == 11 && posts.Last().transform.position.y - posts.First().transform.position.y < -3f, "Fence runs from back-left to front-right as requested");
                    Check(posts.All(r => Quaternion.Angle(r.transform.rotation, Quaternion.identity) < .01f), "Diagonal fence placement does not tilt the upright posts");
                    Check(GameObject.Find("Unseen exterior").GetComponent<SpriteRenderer>().color.a < .5f, "Darkness lifts to reveal the zombies at the fence");
                    Check(owner.ActiveZombieCount == 0 && owner.Progress.Gold == balance && owner.KillCount == 0, "Storyboard zombies remain presentation only and grant no rewards");
                    Snapshot("02-zombies-outside"); step = 3; return;
                }
                if (step == 3)
                {
                    if (owner.OpeningAge < 7.5f) return;
                    Check(GameObject.Find("Opening Sundeok").transform.position.x > -4f, "Grandma leaves her chair and moves toward the open door");
                    Check(GameObject.Find("Empty chair").GetComponent<SpriteRenderer>().enabled, "The exported chair layer stays behind after grandma stands");
                    Check(owner.ShotsFired == 0 && owner.Elapsed == 0, "Combat still waits until the exit shot finishes");
                    Snapshot("03-leaving-house"); step = 4; return;
                }
                if (step == 4)
                {
                    if (owner.IsOpening || owner.Elapsed < .8f) return;
                    Check(owner.Phase == DefensePhase.Waves && owner.ActiveZombieCount > 0 && owner.ShotsFired > 0, "The opening releases live fence zombies and automatic shotgun fire");
                    Check(UnityEngine.Object.FindObjectsByType<ZombieActor>(FindObjectsSortMode.None).Where(z => z.IsAlive).All(z => z.IsBangingFence), "The first Stage 0 wave keeps banging the fence after the opening");
                    Check(grandma.GetComponent<BeltActorVisual>().Body.enabled && owner.CanMoveOnBelt && owner.CanOpenMap, "Live belt actor and normal controls resume after the opening");
                    Check(owner.GrandmaFootPosition.x > -4f, "Gameplay begins outside the doorway instead of returning grandma to the chair");
                    Check(!GameObject.Find("Opening Sundeok").GetComponent<SpriteRenderer>().enabled, "Opening actor is hidden without duplicating the live grandma");
                    Snapshot("04-hunting"); Snapshot("04-hunting-small", 800, 450);
                    owner.RestartEncounter(); Check(!owner.IsOpening && owner.ActiveZombieCount >= 3, "Normal retry skips the already-seen intro in this session");
                    balance = owner.Progress.Gold; owner.ReplayStageZeroOpening();
                    Check(owner.IsOpening && owner.ShotsFired == 0 && owner.ActiveZombieCount == 0, "Component context-menu replay cleanly resets the opening");
                    InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.Enter)); frame = Time.frameCount; step = 5; return;
                }
                if (step == 5)
                {
                    if (Time.frameCount < frame + 2) return;
                    Check(!owner.IsOpening && owner.Phase == DefensePhase.Waves && owner.ActiveZombieCount >= 3, "Enter skips directly into the same Stage 0 combat");
                    Check(owner.Progress.Gold == balance && owner.Progress.CaptiveCount == 0 && owner.Run.EarnedGold == 0, "Skipping grants no currency, captive or clear reward");
                    int spawned = owner.SpawnedCount; owner.SkipStageOpening(); owner.SkipStageOpening();
                    Check(owner.SpawnedCount == spawned, "Repeated skip callbacks cannot start extra waves");
                    InputSystem.QueueStateEvent(keyboard, new KeyboardState());
                    // Isolate attack animation: stop the encounter driver and its old projectiles, not the actor under test.
                    owner.enabled = false;
                    foreach (ShotTracer shot in UnityEngine.Object.FindObjectsByType<ShotTracer>(FindObjectsSortMode.None)) shot.Cancel();
                    ZombieActor[] actors = UnityEngine.Object.FindObjectsByType<ZombieActor>(FindObjectsSortMode.None);
                    attacker = actors[0]; foreach (ZombieActor actor in actors) actor.Despawn();
                    grandma.MoveOnBelt(StageZeroOpeningView.Exit); heroFoot = grandma.FootPosition; health = owner.DefenseHealth;
                    attacker.Spawn(owner, StageZeroOpeningView.VisitorFoot(0), 0, 0, 1000, 0, 1); attacker.HoldAtFence();
                    introAge = Time.unscaledTime; waitUntil = introAge + .22f; step = 14; return;
                }
                if (step == 14)
                {
                    if (Time.unscaledTime < waitUntil) return;
                    Check(attacker.IsBangingFence && attacker.GetComponent<BeltActorVisual>().Display.localPosition.x > 0, "Fence zombie draws back before striking without pursuing grandma");
                    Snapshot("fence-01-windup"); waitUntil = introAge + .47f; step = 15; return;
                }
                if (step == 15)
                {
                    if (Time.unscaledTime < waitUntil) return;
                    Check(attacker.GetComponent<BeltActorVisual>().Display.localPosition.x < 0 && Vector2.Distance(attacker.transform.position, StageZeroOpeningView.VisitorFoot(0)) < .01f, "Fence strike extends the body while keeping feet at the diagonal fence");
                    Check(owner.DefenseHealth == health, "Hitting the fence does not deal damage to grandma"); Snapshot("fence-02-strike");
                    owner.TogglePause(); fencePose = attacker.GetComponent<BeltActorVisual>().Display.localPosition; waitUntil = Time.unscaledTime + .2f; step = 16; return;
                }
                if (step == 16)
                {
                    if (Time.unscaledTime < waitUntil) return;
                    Check(Vector3.Distance(fencePose, attacker.GetComponent<BeltActorVisual>().Display.localPosition) < .001f, "Pause freezes the live fence attack pose");
                    owner.TogglePause(); attacker.ReceiveDamage(2000);
                    Check(!attacker.IsBangingFence && !attacker.IsAlive, "Killing a fence zombie cancels its repeated strike");
                    originX = heroFoot.x + .6f; attacker.Spawn(owner, new Vector2(originX, heroFoot.y), 0, 0, 1000, 0, 1);
                    Check(!attacker.IsBangingFence, "Pool reuse clears the fence assignment for normal pursuit");
                    Check(attacker.AttackBeat == ZombieAttackBeat.None, "Pooled zombie starts without a stale attack state"); step = 6; return;
                }
                if (step == 6)
                {
                    if (attacker.AttackAge < .15f) return;
                    Check(attacker.AttackBeat == ZombieAttackBeat.Windup && attacker.transform.position.x > originX, "Left-facing zombie pulls its feet backward before attacking");
                    Check(attacker.GetComponent<BeltActorVisual>().Display.localPosition.x > 0, "Windup visibly draws the body back as well as the feet");
                    Check(owner.DefenseHealth == health, "Windup does not deal contact damage"); Snapshot("zombie-01-windup"); step = 7; return;
                }
                if (step == 7)
                {
                    if (attacker.AttackAge < .46f) return;
                    Check(attacker.transform.position.x < originX && attacker.GetComponent<BeltActorVisual>().Display.localPosition.x < 0, "Zombie lunges forward with an extended body pose");
                    Check(owner.DefenseHealth == health - 4, "Forward strike deals one hit at the strike timing"); Snapshot("zombie-02-strike"); step = 8; return;
                }
                if (step == 8)
                {
                    if (attacker.AttackAge < .65f) return;
                    Check(attacker.AttackBeat == ZombieAttackBeat.Recover, "Strike is followed by a recovery pose"); Snapshot("zombie-03-recover"); step = 9; return;
                }
                if (step == 9)
                {
                    if (attacker.AttackBeat != ZombieAttackBeat.None) return;
                    Check(owner.DefenseHealth == health - 4, "Recovery does not repeat the strike damage");
                    attacker.Despawn(); health = owner.DefenseHealth;
                    attacker.Spawn(owner, heroFoot + Vector2.right * .6f, 0, 0, 1000, 0, 1); step = 10; return;
                }
                if (step == 10)
                {
                    if (attacker.AttackAge < .12f) return;
                    grandma.MoveOnBelt(heroFoot + Vector2.up * 1f); step = 11; return;
                }
                if (step == 11)
                {
                    if (attacker.AttackAge < .55f) return;
                    Check(owner.DefenseHealth == health, "Moving to a different belt depth avoids the committed strike");
                    attacker.Despawn(); attacker.Spawn(owner, grandma.FootPosition + Vector2.right * .6f, 0, 0, 10, 0, 1); step = 12; return;
                }
                if (step == 12)
                {
                    if (attacker.AttackAge < .13f) return;
                    attacker.ReceiveDamage(1000);
                    Check(!attacker.IsAlive && attacker.AttackBeat == ZombieAttackBeat.None, "Killing a winding-up zombie cancels the pending attack");
                    waitUntil = Time.unscaledTime + .6f; step = 13; return;
                }
                if (step == 13)
                {
                    if (Time.unscaledTime < waitUntil) return;
                    Check(owner.DefenseHealth == health, "A dead zombie cannot deal a delayed ghost hit");
                    attacker.Spawn(owner, grandma.FootPosition + Vector2.right * 3f, 0, 0, 1000, 0, 1);
                    Check(attacker.AttackBeat == ZombieAttackBeat.None && attacker.GetComponent<BeltActorVisual>().Display.localPosition == Vector3.zero, "Pool reuse resets attack timer and pose");
                    Check(errors.Count == 0, "No runtime errors, exceptions or assertions in opening/attack validation"); Finish(null);
                }
            }
            catch (Exception exception) { Finish(exception.Message + "\n" + exception.StackTrace); }
        }
        private static void Finish(string failure)
        {
            string report = "# Stage 0 opening and zombie attack validation\n\nUnity " + Application.unityVersion + " · " + DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(9)).ToString("yyyy-MM-dd HH:mm") + " KST\n\nStatus: " + (failure == null ? "PASS" : "FAIL") + "\n\nIsolated profile: Library/Stage0-opening-validation.json. Includes 100 synthetic test currency for the intro purchase guard. Zombie attack cases temporarily disable only the encounter driver and cancel its old projectiles; the live zombie, visual, collision and damage code continue to run.\n\n";
            report += string.Join("\n", checks.Select(c => "- PASS: " + c));
            if (failure != null) report += "\n\nFailure: " + failure;
            if (errors.Count > 0) report += "\n\nRuntime errors:\n" + string.Join("\n", errors);
            Directory.CreateDirectory("Docs/Validation"); File.WriteAllText("Docs/Validation/Stage0-opening-validation.md", report);
            if (keyboard != null && keyboard.added) InputSystem.RemoveDevice(keyboard);
            if (owner != null) owner.enabled = true;
            SessionState.SetBool(Running, false); DefenseProgressStore.EditorValidationPath = null; Time.timeScale = 1;
            Debug.Log("Hunter Grandma opening/attack validation: " + (failure == null ? "PASS (" + checks.Count + " checks)" : "FAIL: " + failure));
            EditorApplication.ExitPlaymode();
        }
    }
}
