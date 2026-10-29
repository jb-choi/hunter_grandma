using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace HunterGrandma.Editor
{
    // Editor-only, isolated natural-gameplay reproduction and repeatable five-target demonstration.
    [InitializeOnLoad]
    public static class GrandmaSkillInvestigation
    {
        private const string Running = "HunterGrandma.SkillInvestigation";
        private const string DemoKey = "HunterGrandma.SkillInvestigation.Demo";
        private static readonly string Profile = Path.GetFullPath("Library/SkillInvestigation-profile.json");
        private static readonly List<ZombieActor> candidates = new List<ZombieActor>();
        private static readonly List<Sample> samples = new List<Sample>();
        private static readonly List<string> errors = new List<string>();
        private static GrandmaDefense owner;
        private static GrandmaShooter grandma;
        private static bool demo, prepared;
        private static float nextSample;
        private static double deadline;
        private static readonly MethodInfo collect = typeof(GrandmaDefense).GetMethod("CollectSkillTargets", BindingFlags.Instance | BindingFlags.NonPublic);
        [Serializable] private sealed class Sample
        {
            public float combatTime, cooldown, x, y;
            public string phase;
            public int alive, eligible, activations, shots;
            public bool active, enabled;
        }
        [Serializable] private sealed class Report
        { public string mode, result; public int attack, attackLevel, fireRateLevel; public Sample[] samples; public string[] errors; }

        static GrandmaSkillInvestigation()
        {
            demo = SessionState.GetBool(DemoKey, false);
            if (SessionState.GetBool(Running, false)) DefenseProgressStore.EditorValidationPath = Profile;
            EditorApplication.update += Tick;
            EditorApplication.playModeStateChanged += state =>
            { if (state == PlayModeStateChange.EnteredEditMode) { SessionState.SetBool(Running, false); DefenseProgressStore.EditorValidationPath = null; } };
            Application.logMessageReceived += (message, stack, type) =>
            { if (SessionState.GetBool(Running, false) && (type == LogType.Error || type == LogType.Exception || type == LogType.Assert)) errors.Add(message + "\n" + stack); };
        }
        [MenuItem("Tools/Hunter Grandma/Investigate Skill in Natural Combat")]
        private static void Natural() { Begin(false); }
        [MenuItem("Tools/Hunter Grandma/Demonstrate Shadow Skill (Isolated Profile)")]
        private static void Demonstrate() { Begin(true); }
        public static void Begin(bool demonstration)
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().isDirty) { Debug.LogError("Save the scene before skill investigation."); return; }
            string regularPath = DefenseProgressStore.DefaultPath;
            DefenseProgress clone = File.Exists(regularPath) ? DefenseProgress.FromJson(File.ReadAllText(regularPath)) : new DefenseProgress();
            clone.SetShadowSkillEnabled(true);
            if (!new DefenseProgressStore(Profile).Save(clone)) return;
            samples.Clear(); errors.Clear(); candidates.Clear(); owner = null; grandma = null; prepared = false; nextSample = 0;
            demo = demonstration; SessionState.SetBool(DemoKey, demo); SessionState.SetBool(Running, true);
            DefenseProgressStore.EditorValidationPath = Profile;
            EditorSceneManager.OpenScene(GrandmaProjectSetup.ScenePath); EditorApplication.EnterPlaymode();
        }
        private static void Tick()
        {
            if (!SessionState.GetBool(Running, false) || !EditorApplication.isPlaying || EditorApplication.isCompiling) return;
            try
            {
                if (owner == null) owner = UnityEngine.Object.FindFirstObjectByType<GrandmaDefense>();
                if (owner == null || owner.Progress == null) return;
                if (!prepared)
                {
                    prepared = true; deadline = EditorApplication.timeSinceStartup + 100; Time.timeScale = 1;
                    grandma = UnityEngine.Object.FindFirstObjectByType<GrandmaShooter>();
                    owner.SkipStageOpening();
                    Type view = typeof(EditorWindow).Assembly.GetType("UnityEditor.GameView");
                    if (view != null) EditorWindow.GetWindow(view).Focus();
                    if (demo) PrepareDemonstration();
                }
                if (EditorApplication.timeSinceStartup > deadline) { Finish("TIMEOUT"); return; }
                if (Time.unscaledTime >= nextSample)
                {
                    nextSample = Time.unscaledTime + .1f;
                    collect.Invoke(owner, new object[] { candidates, Camera.main, grandma.FacingDirection });
                    samples.Add(new Sample { combatTime = owner.Elapsed, cooldown = owner.SkillCooldownRemaining, phase = owner.Phase.ToString(),
                        alive = owner.ActiveZombieCount, eligible = candidates.Count, activations = owner.SkillActivations, shots = owner.SkillShots,
                        active = owner.IsSkillActive, enabled = owner.Progress.ShadowSkillEnabled, x = grandma.FootPosition.x, y = grandma.FootPosition.y });
                    File.WriteAllText("Library/SkillInvestigation-current.json", JsonUtility.ToJson(samples[samples.Count - 1], true));
                }
                if (demo && owner.SkillActivations >= 2 && !owner.IsSkillActive) { Finish("COMPLETED_TWO_DEMONSTRATIONS"); return; }
                if (!demo && (owner.Phase == DefensePhase.Clear || owner.Phase == DefensePhase.Failed || owner.Phase == DefensePhase.Retreated)) Finish("NATURAL_RUN_FINISHED");
            }
            catch (Exception exception) { errors.Add(exception.ToString()); Finish("ERROR"); }
        }
        private static void PrepareDemonstration()
        {
            if (owner.AutoExplore) owner.ToggleAutoExplore();
            grandma.MoveOnBelt(new Vector2(0, -2.6f)); grandma.FaceTarget(8);
            foreach (ShotTracer tracer in UnityEngine.Object.FindObjectsByType<ShotTracer>(FindObjectsSortMode.None)) tracer.Cancel();
            ZombieActor[] pool = UnityEngine.Object.FindObjectsByType<ZombieActor>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (ZombieActor zombie in pool) { zombie.Despawn(); zombie.gameObject.SetActive(true); zombie.enabled = false; }
            for (int i = 0; i < 5; i++)
            {
                pool[i].Spawn(owner, new Vector2(3 + i * 1.3f, -2.6f + i * .25f), 0, i, 1000, 0, 1);
                pool[i].ReserveShot(); pool[i].enabled = true;
            }
        }
        private static void Finish(string result)
        {
            var report = new Report { mode = demo ? "FiveTargetDemonstration" : "NaturalCombatWithCurrentGrowth", result = result,
                attack = owner?.Progress.Damage ?? 0, attackLevel = owner?.Progress.AttackLevel ?? 0, fireRateLevel = owner?.Progress.FireRateLevel ?? 0,
                samples = samples.ToArray(), errors = errors.ToArray() };
            Directory.CreateDirectory("Docs/Validation");
            File.WriteAllText("Docs/Validation/SkillInvestigation-" + (demo ? "demo" : "natural") + ".json", JsonUtility.ToJson(report, true));
            Debug.Log("Skill investigation: " + report.mode + " / " + result + " / casts=" + (owner?.SkillActivations ?? 0));
            EditorApplication.ExitPlaymode();
        }
    }
}
