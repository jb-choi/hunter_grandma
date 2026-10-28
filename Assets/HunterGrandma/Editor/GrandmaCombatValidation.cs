using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

namespace HunterGrandma.Editor
{
    [InitializeOnLoad]
    public static class GrandmaCombatValidation
    {
        private const string RunningKey = "HunterGrandma.Validation.Running";
        private static GrandmaDefense encounter;
        private static ZombieActor trackedZombie;
        private static Vector3 trackedPosition;
        private static readonly List<string> checks = new List<string>();
        private static readonly List<string> errors = new List<string>();
        private static int stage;
        private static double pausedAt;
        private static float pausedElapsed;
        private static int sustainedKills;
        private static int sustainedSpawns;
        private static double deadline;
        private static bool captured;
        private static bool observedFall;
        private static Keyboard testKeyboard;

        static GrandmaCombatValidation()
        {
            EditorApplication.update += Update;
            Application.logMessageReceived += OnLog;
        }

        [MenuItem("Tools/Hunter Grandma/Run Combat Smoke Test")]
        public static void Begin()
        {
            GrandmaStageValidation.Begin();
        }

        private static void OnLog(string message, string stack, LogType type)
        {
            if (SessionState.GetBool(RunningKey, false) && (type == LogType.Error || type == LogType.Exception || type == LogType.Assert))
                errors.Add(message);
        }

        private static void Check(bool passed, string description)
        {
            if (!passed) throw new InvalidOperationException(description);
            checks.Add(description);
        }

        private static void Update()
        {
            if (!SessionState.GetBool(RunningKey, false) || !EditorApplication.isPlaying || EditorApplication.isPaused) return;
            try
            {
                if (encounter == null)
                {
                    encounter = UnityEngine.Object.FindFirstObjectByType<GrandmaDefense>();
                    if (encounter == null) return;
                    checks.Clear(); errors.Clear(); stage = 0; captured = false; observedFall = false;
                    deadline = EditorApplication.timeSinceStartup + 75;
                    Time.timeScale = 4f;
                }
                if (EditorApplication.timeSinceStartup > deadline) throw new TimeoutException("Combat smoke test exceeded 75 seconds.");
                if (!observedFall)
                    observedFall = UnityEngine.Object.FindObjectsByType<ZombieActor>(FindObjectsInactive.Include, FindObjectsSortMode.None).Any(z => z.gameObject.activeSelf && !z.IsAlive && encounter.KillCount > 0);
                if (stage == 0 && encounter.Elapsed > 0.2f)
                {
                    Check(encounter.PoolSize == 20, "Zombie pool initialized to 20 actors");
                    Check(encounter.ActiveZombieCount >= 3, "Scene starts with three approaching zombies");
                    trackedZombie = UnityEngine.Object.FindObjectsByType<ZombieActor>(FindObjectsSortMode.None).OrderByDescending(z => z.transform.position.x).First();
                    trackedPosition = trackedZombie.transform.position;
                    stage = 1;
                }
                else if (stage == 1 && encounter.Elapsed > 1.4f)
                {
                    Check(trackedZombie.IsAlive && trackedZombie.transform.position.x < trackedPosition.x - 0.3f, "Zombie advances left toward grandma");
                    stage = 2;
                }
                else if (stage == 2 && encounter.Elapsed > 8f)
                {
                    Check(encounter.KillCount >= 3 && encounter.ShotsFired >= encounter.KillCount, "Automatic gunfire and projectile hits defeat the opening zombies");
                    stage = 3;
                }
                if (!captured && encounter.Elapsed > 16f)
                {
                    Capture();
                    captured = true;
                }
                if (stage == 3 && encounter.Elapsed > 60f)
                {
                    sustainedKills = encounter.KillCount;
                    sustainedSpawns = encounter.SpawnedCount;
                    Check(sustainedSpawns >= 35 && sustainedKills >= 25, "Spawning and kills continue for 60 simulated seconds");
                    Check(encounter.BreachCount == 0, "Default balance prevents zombies reaching grandma");
                    Check(encounter.PoolSize == 20, "Endless combat keeps a bounded actor pool");
                    Check(observedFall, "Defeated zombie remains visible during its fall and fade");
                    trackedZombie = UnityEngine.Object.FindObjectsByType<ZombieActor>(FindObjectsSortMode.None).First(z => z.IsAlive);
                    trackedPosition = trackedZombie.transform.position;
                    pausedElapsed = encounter.Elapsed;
                    encounter.TogglePause();
                    pausedAt = EditorApplication.timeSinceStartup;
                    stage = 4;
                }
                else if (stage == 4 && EditorApplication.timeSinceStartup - pausedAt > 0.6)
                {
                    Check(encounter.IsPaused && Mathf.Approximately(encounter.Elapsed, pausedElapsed) && Vector3.Distance(trackedPosition, trackedZombie.transform.position) < 0.001f, "Pause freezes timers and zombie motion");
                    encounter.TogglePause();
                    Check(!encounter.IsPaused && Mathf.Approximately(Time.timeScale, 4f), "Resume restores the previous game speed");
                    encounter.RestartEncounter();
                    Check(encounter.KillCount == 0 && encounter.SpawnedCount == 3 && encounter.ActiveZombieCount == 3, "Restart clears scores and resets the opening wave");
                    Check(!UnityEngine.Object.FindObjectsByType<ShotTracer>(FindObjectsSortMode.None).Any(), "Restart cancels in-flight projectiles");
                    stage = 5;
                }
                else if (stage == 5 && encounter.Elapsed > 5f)
                {
                    Check(encounter.KillCount >= 2, "Automatic combat resumes after restart");
                    testKeyboard = InputSystem.AddDevice<Keyboard>("Combat validation keyboard");
                    InputSystem.QueueStateEvent(testKeyboard, new KeyboardState(Key.Space));
                    stage = 6;
                }
                else if (stage == 6 && encounter.IsPaused)
                {
                    Check(true, "Space input through the Input System pauses combat");
                    InputSystem.QueueStateEvent(testKeyboard, new KeyboardState(Key.R));
                    stage = 7;
                }
                else if (stage == 7 && !encounter.IsPaused && encounter.KillCount == 0)
                {
                    Check(encounter.SpawnedCount == 3, "R input restarts combat, including while paused");
                    Check(errors.Count == 0, "No runtime errors or exceptions during the smoke test");
                    Finish(true, null);
                }
            }
            catch (Exception exception) { Finish(false, exception.Message); }
        }

        internal static void Capture(string path = "Docs/Validation/GrandmaDefense-preview.png", int width = 1280, int height = 720)
        {
            Directory.CreateDirectory("Docs/Validation");
            Camera camera = Camera.main;
            var texture = new RenderTexture(width, height, 24);
            RenderTexture oldTarget = camera.targetTexture;
            RenderTexture oldActive = RenderTexture.active;
            Canvas canvas = UnityEngine.Object.FindFirstObjectByType<Canvas>();
            RenderMode oldMode = canvas.renderMode;
            Camera oldCamera = canvas.worldCamera;
            int oldSorting = canvas.sortingOrder;
            bool oldOverride = canvas.overrideSorting;
            float oldDistance = canvas.planeDistance;
            try
            {
                canvas.renderMode = RenderMode.ScreenSpaceCamera;
                canvas.worldCamera = camera;
                canvas.planeDistance = 1f;
                // Camera-mode capture otherwise puts high-Y-sort world actors above UI.
                canvas.overrideSorting = true;
                canvas.sortingOrder = short.MaxValue;
                camera.targetTexture = texture;
                Canvas.ForceUpdateCanvases();
                DefenseHud hud = UnityEngine.Object.FindFirstObjectByType<DefenseHud>();
                hud.RefreshMapLayout();
                Canvas.ForceUpdateCanvases();
                camera.Render();
                RenderTexture.active = texture;
                Texture2D pixels = new Texture2D(width, height, TextureFormat.RGB24, false);
                pixels.ReadPixels(new Rect(0, 0, width, height), 0, 0);
                pixels.Apply();
                File.WriteAllBytes(path, pixels.EncodeToPNG());
                UnityEngine.Object.DestroyImmediate(pixels);
            }
            finally
            {
                camera.targetTexture = oldTarget;
                RenderTexture.active = oldActive;
                canvas.renderMode = oldMode;
                canvas.worldCamera = oldCamera;
                canvas.sortingOrder = oldSorting;
                canvas.overrideSorting = oldOverride;
                canvas.planeDistance = oldDistance;
                Canvas.ForceUpdateCanvases();
                UnityEngine.Object.FindFirstObjectByType<DefenseHud>().RefreshMapLayout();
                texture.Release();
                UnityEngine.Object.DestroyImmediate(texture);
            }
        }

        private static void Finish(bool passed, string failure)
        {
            Directory.CreateDirectory("Docs/Validation");
            string report = "# Grandma Defense combat smoke test\n\nUnity " + Application.unityVersion + "\n\nStatus: " + (passed ? "PASS" : "FAIL") + "\n\n";
            report += "Scene: `" + GrandmaProjectSetup.ScenePath + "`\n\n";
            report += "Sustained run: " + sustainedSpawns + " spawned, " + sustainedKills + " defeated in 60 simulated seconds.\n\n";
            report += string.Join("\n", checks.Select(c => "- PASS: " + c)) + "\n";
            if (failure != null) report += "\nFailure: " + failure + "\n";
            if (errors.Count > 0) report += "\nRuntime errors:\n" + string.Join("\n", errors) + "\n";
            File.WriteAllText("Docs/Validation/CombatSmokeTest.md", report);
            if (testKeyboard != null && testKeyboard.added) InputSystem.RemoveDevice(testKeyboard);
            testKeyboard = null;
            SessionState.SetBool(RunningKey, false);
            Time.timeScale = 1f;
            Debug.Log("Hunter Grandma combat smoke test: " + (passed ? "PASS" : "FAIL: " + failure));
            EditorApplication.ExitPlaymode();
        }
    }
}
