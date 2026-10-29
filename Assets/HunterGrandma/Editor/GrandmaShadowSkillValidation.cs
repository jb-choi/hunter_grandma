using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HunterGrandma.Editor
{
    [InitializeOnLoad]
    public static class GrandmaShadowSkillValidation
    {
        private const string Running = "HunterGrandma.ShadowSkill.Validation";
        private static readonly string Profile = Path.GetFullPath("Library/ShadowSkill-validation.json");
        private static readonly List<string> checks = new List<string>(), errors = new List<string>();
        private static GrandmaDefense owner;
        private static GrandmaShooter grandma;
        private static DefenseHud hud;
        private static ZombieActor[] pool;
        private static int step, shot, activations, kills, gold;
        private static float clock, cooldown;
        private static double deadline, wall;
        private static Vector3 cameraPosition, targetPosition;
        private static Vector2 origin;
        private static bool captured;

        static GrandmaShadowSkillValidation()
        {
            if (SessionState.GetBool(Running, false)) DefenseProgressStore.EditorValidationPath = Profile;
            EditorApplication.update += Tick;
            EditorApplication.playModeStateChanged += state =>
            { if (state == PlayModeStateChange.EnteredEditMode) { SessionState.SetBool(Running, false); DefenseProgressStore.EditorValidationPath = null; } };
            Application.logMessageReceived += (message, stack, type) =>
            { if (SessionState.GetBool(Running, false) && (type == LogType.Error || type == LogType.Exception || type == LogType.Assert)) errors.Add(message + "\n" + stack); };
        }
        [MenuItem("Tools/Hunter Grandma/Run Shadow Skill Validation")]
        public static void Begin()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().isDirty) { Debug.LogError("Save scene before skill validation."); return; }
            checks.Clear(); errors.Clear(); owner = null; grandma = null; hud = null; pool = null; step = 0; captured = false;
            foreach (string file in new[] { Profile, Profile + ".bak" }) if (File.Exists(file)) File.Delete(file);
            SessionState.SetBool(Running, true); DefenseProgressStore.EditorValidationPath = Profile;
            EditorSceneManager.OpenScene(GrandmaProjectSetup.ScenePath); EditorApplication.EnterPlaymode();
        }
        private static void Check(bool value, string message) { if (!value) throw new Exception(message); checks.Add(message); }
        private static void Click(string name) { GameObject.Find(name).GetComponent<Button>().onClick.Invoke(); hud.RefreshPresentation(); }
        private static void Snapshot(string name, int width = 1280, int height = 720)
        { hud.RefreshPresentation(); GrandmaCombatValidation.Capture("Docs/Validation/ShadowSkill-" + name + ".png", width, height); }
        private static void Targets(int count, int health = 1000, int reward = 0)
        {
            foreach (ShotTracer tracer in UnityEngine.Object.FindObjectsByType<ShotTracer>(FindObjectsSortMode.None)) tracer.Cancel();
            foreach (ZombieActor zombie in pool) { zombie.Despawn(); zombie.gameObject.SetActive(true); zombie.enabled = false; }
            grandma.MoveOnBelt(new Vector2(1, -2.6f)); grandma.FaceTarget(6);
            for (int i = 0; i < count; i++)
            {
                pool[i].Spawn(owner, new Vector2(4 + i * 1.3f, -2.6f + i * .25f), .03f, i, health, reward, 1);
                pool[i].ReserveShot(); pool[i].enabled = true;
            }
        }
        private static bool MainActorVisible => grandma.GetComponent<BeltActorVisual>().Body.enabled;
        private static void Tick()
        {
            if (!SessionState.GetBool(Running, false) || !EditorApplication.isPlaying || EditorApplication.isCompiling) return;
            try
            {
                if (owner == null) owner = UnityEngine.Object.FindFirstObjectByType<GrandmaDefense>();
                if (owner == null || owner.Progress == null) return;
                if (step == 0)
                {
                    deadline = EditorApplication.timeSinceStartup + 120;
                    grandma = UnityEngine.Object.FindFirstObjectByType<GrandmaShooter>(); hud = UnityEngine.Object.FindFirstObjectByType<DefenseHud>();
                    Check(!owner.Progress.ShadowSkillEnabled, "New profiles default to skill OFF");
                    string legacy = owner.Progress.ToJson().Replace("\"shadowSkillEnabled\": false,", "");
                    Check(!DefenseProgress.FromJson(legacy).ShadowSkillEnabled, "Old v1 saves without the new field load with skill OFF");
                    owner.SkipStageOpening(); if (owner.AutoExplore) owner.ToggleAutoExplore();
                    pool = UnityEngine.Object.FindObjectsByType<ZombieActor>(FindObjectsInactive.Include, FindObjectsSortMode.None);
                    Targets(3); Time.timeScale = 2;
                    hud.ToggleUpgrade(); Click("Open Sundeok upgrades");
                    Check(GameObject.Find("Shadow skill toggle") != null, "Sundeok upgrade details include the skill toggle");
                    Snapshot("upgrade-off");
                    Click("Shadow skill toggle");
                    Check(owner.Progress.ShadowSkillEnabled && owner.SkillCooldownRemaining == 10, "Toggle ON starts a full ten-second cooldown");
                    Check(DefenseProgress.FromJson(File.ReadAllText(Profile)).ShadowSkillEnabled, "Skill ON is saved through the existing progress store");
                    Snapshot("upgrade-on"); Snapshot("upgrade-small", 800, 450); Snapshot("upgrade-4x3", 1024, 768);
                    Canvas.ForceUpdateCanvases();
                    var hits = new List<RaycastResult>();
                    EventSystem.current.RaycastAll(new PointerEventData(EventSystem.current) { position = RectTransformUtility.WorldToScreenPoint(null, GameObject.Find("Shadow skill toggle").transform.position) }, hits);
                    Check(hits.Count > 0 && hits[0].gameObject.GetComponentInParent<Button>()?.name == "Shadow skill toggle", "Visible skill card receives pointer raycasts above the modal shade");
                    Click("Close upgrades"); clock = owner.Elapsed; step = 1; return;
                }
                if (EditorApplication.timeSinceStartup > deadline) throw new Exception("Skill validation timed out at step " + step);
                if (step == 1 && owner.SkillCooldownRemaining < 6)
                {
                    Check(!owner.IsSkillActive && owner.SkillActivations == 0, "Skill never fires before ten combat seconds");
                    owner.TogglePause(); cooldown = owner.SkillCooldownRemaining; wall = EditorApplication.timeSinceStartup; step = 2;
                }
                else if (step == 2 && EditorApplication.timeSinceStartup - wall > .35)
                {
                    Check(owner.SkillCooldownRemaining == cooldown && !owner.IsSkillActive, "Pause freezes cooldown without activating the skill");
                    owner.TogglePause(); step = 3;
                }
                else if (step == 3 && owner.IsSkillActive)
                {
                    Check(owner.Elapsed - clock >= 9.8f && owner.Elapsed - clock <= 10.2f, "Automatic activation occurs at ten elapsed combat seconds");
                    Check(owner.CombatDeltaTime == 0 && !owner.CanMoveOnBelt && !MainActorVisible, "Skill freezes combat/movement and hides the real actor");
                    origin = grandma.FootPosition; cameraPosition = Camera.main.transform.position; targetPosition = pool[0].transform.position;
                    clock = owner.Elapsed; gold = owner.DefenseHealth;
                    owner.DamageDefense(99); Check(owner.DefenseHealth == gold, "The hidden actor cannot take damage during the cinematic");
                    wall = EditorApplication.timeSinceStartup; step = 4;
                }
                else if (step == 4)
                {
                    Check(owner.Elapsed == clock && Camera.main.transform.position == cameraPosition && grandma.FootPosition == origin, "Combat clock, camera and real position stay fixed during the skill");
                    if (owner.SkillShots >= 1)
                    {
                        shot = owner.SkillShots; targetPosition = pool[0].transform.position;
                        owner.TogglePause(); wall = EditorApplication.timeSinceStartup; step = 5;
                    }
                }
                else if (step == 5 && EditorApplication.timeSinceStartup - wall > .35)
                {
                    Check(owner.IsSkillActive && owner.SkillShots == shot && pool[0].transform.position == targetPosition, "User pause freezes skill progression and enemy movement together");
                    owner.TogglePause(); Click("지도  M"); wall = EditorApplication.timeSinceStartup; step = 51;
                }
                else if (step == 51 && EditorApplication.timeSinceStartup - wall > .3)
                {
                    Check(hud.IsMapOpen && owner.IsPaused && owner.SkillShots == shot, "Opening the map freezes the active skill");
                    Click("Close map"); Check(!hud.IsMapOpen && !owner.IsPaused, "Closing the map restores the running skill pause state"); step = 6;
                }
                else if (step == 6)
                {
                    if (owner.IsSkillActive && owner.SkillShots > shot && !captured)
                    {
                        Snapshot("headshot"); captured = true;
                        float sparkY = GameObject.Find("Headshot spark").transform.position.y;
                        Check(pool.Take(3).Any(z => Mathf.Abs(z.HeadPoint.y - sparkY) < .03f && z.HeadPoint.y > z.HitPoint.y + .2f), "Headshot spark targets the visual head above the ordinary torso hit point");
                    }
                    if (!owner.IsSkillActive)
                    {
                        Check(owner.SkillShots == 5, "Three living targets receive exactly five shots by revisiting survivors");
                        Check(pool.Take(3).Sum(z => 1000 - z.Health) == 5 * owner.Progress.Damage * 2, "Each shot applies twice the current attack damage exactly once");
                        Check(pool.Take(3).All(z => z.Health < 1000), "Different targets are visited before a survivor is hit again");
                        Check(grandma.FootPosition == origin && MainActorVisible, "Normal presentation returns at the exact original foot position");
                        activations = owner.SkillActivations; clock = owner.Elapsed; step = 7;
                    }
                }
                else if (step == 7 && owner.IsSkillActive)
                {
                    Check(owner.SkillActivations == activations + 1 && owner.Elapsed - clock >= 9.8f, "ON automatically repeats after the next ten combat seconds");
                    owner.SetShadowSkillEnabled(false);
                    Check(!owner.IsSkillActive && MainActorVisible && grandma.FootPosition == origin, "Turning OFF during an active sequence safely restores the actor");
                    Check(!DefenseProgress.FromJson(File.ReadAllText(Profile)).ShadowSkillEnabled, "Skill OFF persists on disk");
                    clock = owner.Elapsed; step = 8;
                }
                else if (step == 8 && owner.Elapsed - clock > 10.2f)
                {
                    Check(!owner.IsSkillActive && owner.SkillActivations == 0, "OFF prevents automatic skill activation across another full interval");
                    Targets(1, 10, 7); kills = owner.KillCount; gold = owner.Progress.Gold;
                    owner.SetShadowSkillEnabled(true); step = 9;
                }
                else if (step == 9 && owner.IsSkillActive) { step = 10; }
                else if (step == 10 && !owner.IsSkillActive)
                {
                    Check(owner.SkillShots == 1 && owner.KillCount == kills + 1 && owner.Progress.Gold == gold + 7, "Last enemy death ends early and credits exactly one existing kill reward");
                    Check(MainActorVisible, "Early completion restores visible grandma");
                    Targets(0); owner.SetShadowSkillEnabled(true); clock = owner.Elapsed; step = 11;
                }
                else if (step == 11 && owner.Elapsed - clock > 10.2f)
                {
                    Check(!owner.IsSkillActive && owner.SkillCooldownRemaining == 0, "No targets leaves the skill ready without consuming an activation");
                    Targets(1); pool[0].transform.position = new Vector3(-3, -2.6f); grandma.FaceTarget(6);
                    // Keep normal aim from turning toward this deliberately rear-only target.
                    pool[0].transform.position += Vector3.up * 1.4f;
                    wall = EditorApplication.timeSinceStartup; step = 12;
                }
                else if (step == 12 && EditorApplication.timeSinceStartup - wall > .2)
                {
                    Check(!owner.IsSkillActive, "A rear-only off-lane zombie is excluded from a front-facing cast");
                    Targets(1); step = 13;
                }
                else if (step == 13 && owner.IsSkillActive)
                {
                    owner.RestartEncounter();
                    Check(!owner.IsSkillActive && MainActorVisible && owner.SkillCooldownRemaining == 10, "Restart cancels the sequence, removes the veil and resets cooldown");
                    Targets(1); step = 14;
                }
                else if (step == 14 && owner.IsSkillActive)
                {
                    owner.Retreat();
                    Check(!owner.IsSkillActive && MainActorVisible && owner.Phase == DefensePhase.Retreated, "Retreat restores presentation and cancels the skill");
                    owner.RestartEncounter(); Targets(5, 10, 3); kills = owner.KillCount; gold = owner.Progress.Gold;
                    owner.SetShadowSkillEnabled(true); step = 15;
                }
                else if (step == 15 && owner.IsSkillActive) { step = 16; }
                else if (step == 16 && !owner.IsSkillActive)
                {
                    Check(owner.SkillShots == 5 && owner.KillCount == kills + 5 && owner.Progress.Gold == gold + 15, "Five distinct vulnerable zombies receive five lethal shots and five rewards");
                    Targets(1); step = 17;
                }
                else if (step == 17 && owner.IsSkillActive)
                {
                    owner.enabled = false;
                    Check(!owner.IsSkillActive && MainActorVisible && GameObject.Find("Night veil") == null, "Disabling the encounter cleans up the cinematic and restores the real actor");
                    Check(errors.Count == 0, "No runtime errors, exceptions or assertions"); Finish(null);
                }
            }
            catch (Exception exception) { Finish(exception.Message + "\n" + exception.StackTrace); }
        }
        private static void Finish(string failure)
        {
            string report = "# Shadow skill validation\n\nUnity " + Application.unityVersion + " · " + DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(9)).ToString("yyyy-MM-dd HH:mm") + " KST\n\nStatus: " + (failure == null ? "PASS" : "FAIL") + "\n\nIsolated profile: Library/ShadowSkill-validation.json. Three reserved high-health targets run in the actual encounter with all unused pool slots held inactive in behavior but occupied to prevent new spawns. Game time is accelerated 2x; cooldowns are checked in combat seconds. Production save is never loaded/reset.\n\n";
            report += string.Join("\n", checks.Distinct().Select(c => "- PASS: " + c));
            if (failure != null) report += "\n\nFailure: " + failure;
            if (errors.Count > 0) report += "\n\nErrors:\n" + string.Join("\n", errors);
            Directory.CreateDirectory("Docs/Validation"); File.WriteAllText("Docs/Validation/ShadowSkill-validation.md", report);
            owner?.SetShadowSkillEnabled(false);
            Time.timeScale = 1;
            // Keep the isolated path through OnDisable/OnApplicationQuit while leaving Play Mode.
            Debug.Log("Hunter Grandma shadow skill validation: " + (failure == null ? "PASS (" + checks.Distinct().Count() + " checks)" : "FAIL: " + failure));
            EditorApplication.ExitPlaymode();
        }
    }
}
