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
    public static class GrandmaMapValidation
    {
        private const string RunningKey = "HunterGrandma.MapValidation.Running";
        private static GrandmaDefense encounter;
        private static DefenseHud hud;
        private static Canvas canvas;
        private static Keyboard keyboard;
        private static readonly List<string> checks = new List<string>();
        private static readonly List<string> errors = new List<string>();
        private static int stage;
        private static double deadline;
        private static double stepAt;
        private static float pausedElapsed;
        private static int pausedKills;

        static GrandmaMapValidation()
        {
            EditorApplication.update += Update;
            Application.logMessageReceived += OnLog;
        }

        [MenuItem("Tools/Hunter Grandma/Run Map UI Smoke Test")]
        public static void Begin()
        {
            GrandmaStageValidation.Begin();
        }

        private static bool SceneDirty() => UnityEngine.SceneManagement.SceneManager.GetActiveScene().isDirty;

        private static void OnLog(string message, string stack, LogType type)
        {
            if (SessionState.GetBool(RunningKey, false) && (type == LogType.Error || type == LogType.Exception || type == LogType.Assert)) errors.Add(message);
        }

        private static void Check(bool passed, string message)
        {
            if (!passed) throw new InvalidOperationException(message);
            checks.Add(message);
        }

        private static RectTransform Rect(string name) => canvas.GetComponentsInChildren<RectTransform>(true).First(t => t.name == name);
        private static Button Button(string name) => canvas.GetComponentsInChildren<Button>(true).First(b => b.name == name);
        private static Text Text(string name) => canvas.GetComponentsInChildren<Text>(true).First(t => t.name == name);

        private static void Update()
        {
            if (!SessionState.GetBool(RunningKey, false) || !EditorApplication.isPlaying || EditorApplication.isPaused) return;
            try
            {
                if (encounter == null)
                {
                    encounter = UnityEngine.Object.FindFirstObjectByType<GrandmaDefense>();
                    if (encounter == null) return;
                    hud = UnityEngine.Object.FindFirstObjectByType<DefenseHud>();
                    canvas = UnityEngine.Object.FindFirstObjectByType<Canvas>();
                    checks.Clear(); errors.Clear(); stage = 0;
                    deadline = EditorApplication.timeSinceStartup + 45;
                    Time.timeScale = 4f;
                }
                if (EditorApplication.timeSinceStartup > deadline) throw new TimeoutException("Map UI validation exceeded 45 seconds.");
                if (stage == 0 && encounter.Elapsed > 2f)
                {
                    Check(!hud.IsMapOpen, "Map starts closed and does not interrupt automatic combat");
                    Button("지도  M").onClick.Invoke();
                    Check(hud.IsMapOpen && encounter.IsPaused && Time.timeScale == 0f, "Map button opens a modal and pauses combat");
                    Check(Rect("City map image").GetComponent<Image>().sprite != null, "User-supplied map sprite is connected");
                    pausedElapsed = encounter.Elapsed;
                    pausedKills = encounter.KillCount;
                    stepAt = EditorApplication.timeSinceStartup;
                    for (int i = 0; i < DefenseRegions.Count; i++)
                    {
                        Button("Region " + i).onClick.Invoke();
                        Check(Text("Selected region title").text == DefenseRegions.Get(i).Name, "Map pin " + (i + 1) + " updates the selected region details");
                    }
                    Check(encounter.CurrentRegionIndex == 0, "Previewing pins does not change the active region");
                    Button("Region 4").onClick.Invoke();
                    stage = 1;
                }
                else if (stage == 1 && EditorApplication.timeSinceStartup - stepAt > 0.4)
                {
                    Canvas.ForceUpdateCanvases();
                    PointerEventData pointer = new PointerEventData(EventSystem.current)
                    {
                        position = RectTransformUtility.WorldToScreenPoint(null, Rect("Region 4").position)
                    };
                    var hits = new List<RaycastResult>();
                    EventSystem.current.RaycastAll(pointer, hits);
                    GrandmaCombatValidation.Capture("Docs/Validation/MapSelection-1280x720.png");
                    GrandmaCombatValidation.Capture("Docs/Validation/MapSelection-1024x768.png", 1024, 768);
                    if (hits.Count == 0 || hits[0].gameObject != Button("Region 4").gameObject)
                        throw new InvalidOperationException("Map pin pointer hit mismatch: " + string.Join(", ", hits.Select(h => h.gameObject.name)));
                    Check(true, "Map pin receives pointer hits above the modal backdrop");
                    Check(Mathf.Approximately(encounter.Elapsed, pausedElapsed) && encounter.KillCount == pausedKills, "Combat timers and kills remain frozen while browsing the map");
                    Button("Travel to selected region").onClick.Invoke();
                    Check(!hud.IsMapOpen && encounter.CurrentRegionIndex == 4 && !encounter.IsPaused, "Confirming a new region changes location, closes the map and resumes combat");
                    Check(encounter.KillCount == 0 && encounter.SpawnedCount == 3 && encounter.ActiveZombieCount == 3, "Region travel resets the encounter exactly once");
                    Check(Mathf.Approximately(Time.timeScale, 4f), "Map closing restores the previous game speed");
                    stage = 2;
                }
                else if (stage == 2 && encounter.Elapsed > 2f)
                {
                    Check(canvas.GetComponentsInChildren<Text>().Any(t => t.text == "현재 구역  ·  " + encounter.CurrentRegion.Name), "HUD displays the confirmed region");
                    Button("지도  M").onClick.Invoke();
                    Button("Close map").onClick.Invoke();
                    Check(encounter.CurrentRegionIndex == 4 && !encounter.IsPaused && encounter.Elapsed > 1f, "Closing without travel preserves location and encounter progress");
                    encounter.TogglePause();
                    Button("지도  M").onClick.Invoke();
                    Button("Region 1").onClick.Invoke();
                    Button("Travel to selected region").onClick.Invoke();
                    Check(encounter.CurrentRegionIndex == 1 && encounter.IsPaused && !hud.IsMapOpen, "Travel preserves an existing player pause");
                    Button("지도  M").onClick.Invoke();
                    Button("Map backdrop").onClick.Invoke();
                    Check(encounter.IsPaused && !hud.IsMapOpen, "Backdrop close also preserves an existing player pause");
                    encounter.TogglePause();
                    for (int i = 0; i < 20; i++)
                    {
                        Button("지도  M").onClick.Invoke();
                        Button("Close map").onClick.Invoke();
                    }
                    Check(!encounter.IsPaused && Mathf.Approximately(Time.timeScale, 4f) && encounter.PoolSize == 20, "Twenty open/close cycles preserve pause state and bounded pools");
                    Check(canvas.GetComponentsInChildren<RectTransform>(true).Count(t => t.name == "Map overlay") == 1, "Repeated map use reuses one overlay");
                    Check(!encounter.TravelToRegion(-1) && !encounter.TravelToRegion(DefenseRegions.Count), "Invalid region selections are rejected");
                    keyboard = InputSystem.AddDevice<Keyboard>("Map validation keyboard");
                    InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.M));
                    stage = 3;
                }
                else if (stage == 3 && hud.IsMapOpen)
                {
                    Check(encounter.IsPaused, "M input opens the map and pauses combat");
                    pausedElapsed = encounter.Elapsed;
                    pausedKills = encounter.KillCount;
                    InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.Space, Key.R));
                    stepAt = EditorApplication.timeSinceStartup;
                    stage = 4;
                }
                else if (stage == 4 && EditorApplication.timeSinceStartup - stepAt > 0.2)
                {
                    Check(hud.IsMapOpen && encounter.IsPaused && Mathf.Approximately(encounter.Elapsed, pausedElapsed) && encounter.KillCount == pausedKills, "Map blocks gameplay pause/restart shortcuts while browsing");
                    InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.Escape));
                    stage = 5;
                }
                else if (stage == 5 && !hud.IsMapOpen)
                {
                    Check(!encounter.IsPaused && Mathf.Approximately(Time.timeScale, 4f), "Escape closes the map and restores combat");
                    Check(errors.Count == 0, "No runtime errors or exceptions during map validation");
                    Finish(true, null);
                }
            }
            catch (Exception exception) { Finish(false, exception.Message); }
        }

        private static void Finish(bool passed, string failure)
        {
            Directory.CreateDirectory("Docs/Validation");
            string report = "# Map selection UI smoke test\n\nUnity " + Application.unityVersion + "\n\nStatus: " + (passed ? "PASS" : "FAIL") + "\n\n";
            report += string.Join("\n", checks.Select(c => "- PASS: " + c)) + "\n";
            if (failure != null) report += "\nFailure: " + failure + "\n";
            if (errors.Count > 0) report += "\nRuntime errors:\n" + string.Join("\n", errors) + "\n";
            File.WriteAllText("Docs/Validation/MapSelectionSmokeTest.md", report);
            if (keyboard != null && keyboard.added) InputSystem.RemoveDevice(keyboard);
            keyboard = null;
            SessionState.SetBool(RunningKey, false);
            Time.timeScale = 1f;
            Debug.Log("Hunter Grandma map UI smoke test: " + (passed ? "PASS" : "FAIL: " + failure));
            EditorApplication.ExitPlaymode();
        }
    }
}
