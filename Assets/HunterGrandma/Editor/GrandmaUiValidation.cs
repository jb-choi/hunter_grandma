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
    public static class GrandmaUiValidation
    {
        private const string Running = "HunterGrandma.UI-P0.Validation";
        private static readonly string Profile = Path.GetFullPath("Library/UI-P0-validation.json");
        private const string Observation = "Library/UI-P0-observation.txt";
        private static readonly List<string> checks = new List<string>(), errors = new List<string>();
        private static GrandmaDefense owner;
        private static DefenseHud hud;
        private static Keyboard keyboard;
        private static int step, frame, beforeGold;
        private static float feedbackStart;
        private static double deadline;
        private static float inputQueuedAt;
        static GrandmaUiValidation()
        {
            if (SessionState.GetBool(Running, false)) DefenseProgressStore.EditorValidationPath = Profile;
            EditorApplication.update += Tick;
            Application.logMessageReceived += (message, stack, type) =>
            {
                if (SessionState.GetBool(Running, false) && (type == LogType.Error || type == LogType.Exception || type == LogType.Assert)) errors.Add(message + "\n" + stack);
            };
        }
        [MenuItem("Tools/Hunter Grandma/Run UI P0 Validation")]
        public static void Begin()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().isDirty) { Debug.LogError("Save the scene before UI validation."); return; }
            GrandmaStageSetup.Configure(); checks.Clear(); errors.Clear(); owner = null; hud = null; keyboard = null; step = 0;
            foreach (string path in new[] { Profile, Profile + ".bak", Observation }) if (File.Exists(path)) File.Delete(path);
            SessionState.SetBool(Running, true); DefenseProgressStore.EditorValidationPath = Profile;
            EditorSceneManager.OpenScene(GrandmaProjectSetup.ScenePath);
            GameObject preview = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects().FirstOrDefault(r => r.name == "Belt layout preview");
            Check(preview != null && preview.activeSelf, "Stopped scene contains an enabled belt layout preview");
            Check(preview.GetComponentsInChildren<SpriteRenderer>().Any(r => r.name == "Belt road" && r.bounds.size.y > 3f), "Editor preview shows a wide walkable road instead of the old single ground line");
            EditorApplication.EnterPlaymode();
        }
        private static void Check(bool condition, string message) { if (!condition) throw new Exception(message); checks.Add(message); }
        private static Button Button(string name) => GameObject.Find(name)?.GetComponent<Button>() ?? throw new Exception("Missing button: " + name);
        private static Text Label(string container) => GameObject.Find(container)?.GetComponentInChildren<Text>() ?? throw new Exception("Missing label: " + container);
        private static void Click(string name)
        {
            hud.RefreshPresentation(); Button button = Button(name); Check(button.IsInteractable(), "Interactable UI: " + name); button.onClick.Invoke();
        }
        private static void Key(Key key)
        {
            // Manual visual review can leave the Editor/Inspector focused. Route queued keys to the player view.
            Type gameView = typeof(EditorWindow).Assembly.GetType("UnityEditor.GameView");
            if (gameView != null) EditorWindow.GetWindow(gameView).Focus();
            keyboard.MakeCurrent(); InputSystem.QueueStateEvent(keyboard, new KeyboardState(key)); frame = Time.frameCount; inputQueuedAt = Time.unscaledTime;
        }
        private static void Snapshot(string name, int width = 1280, int height = 720)
        {
            Canvas canvas = UnityEngine.Object.FindFirstObjectByType<Canvas>();
            RenderMode mode = canvas.renderMode; Camera camera = canvas.worldCamera; int sorting = canvas.sortingOrder;
            bool sortOverride = canvas.overrideSorting; float distance = canvas.planeDistance;
            GrandmaCombatValidation.Capture("Docs/Validation/UI-P0-" + name + ".png", width, height);
            Check(canvas.renderMode == mode && canvas.worldCamera == camera && canvas.sortingOrder == sorting && canvas.overrideSorting == sortOverride && Mathf.Approximately(canvas.planeDistance, distance), "Capture restores Canvas state: " + name);
        }
        private static bool ReviewAcknowledged()
        {
            // The external reviewer may briefly own the file while replacing MAP_READY with ACK.
            try { return File.Exists(Observation) && File.ReadAllText(Observation).Trim() == "ACK"; }
            catch (IOException) { return false; }
        }
        private static void Tick()
        {
            if (!SessionState.GetBool(Running, false) || !EditorApplication.isPlaying || EditorApplication.isCompiling) return;
            try
            {
                if (owner == null) owner = UnityEngine.Object.FindFirstObjectByType<GrandmaDefense>();
                if (owner == null || owner.Progress == null) return;
                if (step == 0)
                {
                    if (owner.Elapsed < .3f) return;
                    deadline = EditorApplication.timeSinceStartup + 240;
                    hud = UnityEngine.Object.FindFirstObjectByType<DefenseHud>();
                    Check(!UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects().Single(r => r.name == "Belt layout preview").activeSelf, "Editor preview is disabled in Play Mode; runtime owns the belt");
                    Check(GameObject.Find("Walkable belt and cover") != null, "Runtime belt geometry remains present");
                    Check(!hud.IsUpgradeOpen && !hud.IsMapOpen, "HUD starts compact with both panels closed");
                    Check(GameObject.Find("Top bar").GetComponent<RectTransform>().rect.height <= 97f, "Compact HUD uses the smaller top safe area");
                    Check(UnityEngine.Object.FindFirstObjectByType<Canvas>().renderMode == RenderMode.ScreenSpaceOverlay, "Live UI uses overlay rendering above world actors");
                    Check(GameObject.Find("Resource reference").GetComponent<Image>().sprite != null, "Downloaded placeholder image is wired to the resource HUD");
                    Check(Label("Resource reference").text == "재화-01", "Resource image carries its temporary identifier");
                    Snapshot("hunt"); Snapshot("hunt-small", 800, 450);
                    owner.TogglePause(); keyboard = InputSystem.AddDevice<Keyboard>("UI validation keyboard"); keyboard.MakeCurrent();
                    Key(UnityEngine.InputSystem.Key.U); step = 1; return;
                }
                if (EditorApplication.timeSinceStartup > deadline) throw new Exception("UI validation timeout at step " + step);
                if (Time.frameCount < frame + 2) return;
                if (step == 1)
                {
                    Check(hud.IsUpgradeOpen && owner.IsPaused, "U opens upgrades without changing the existing pause state");
                    Check(!Button("Attack upgrade").interactable && !Button("Fire rate upgrade").interactable, "Insufficient currency disables both purchase buttons");
                    Check(Label("Attack upgrade").text.Contains("재화 부족"), "Insufficient state has a text explanation");
                    Check(Label("Attack reference").text == "항목-01" && Label("Rate reference").text == "항목-02", "Both upgrade images carry temporary identifiers");
                    Snapshot("upgrades-poor"); Key(UnityEngine.InputSystem.Key.Escape); step = 2; return;
                }
                if (step == 2)
                {
                    Check(!hud.IsUpgradeOpen && owner.IsPaused, "Escape closes upgrades without resuming paused combat");
                    for (int i = 0; i < 6; i++) { Click("Upgrade panel toggle"); Check(hud.IsUpgradeOpen, "Repeated upgrade open " + i); Click("Close upgrades"); }
                    Check(!hud.IsUpgradeOpen, "Repeated opening does not leave hidden blocking UI");
                    owner.Progress.AddGold(200); Click("Upgrade panel toggle");
                    int damage = owner.Progress.NextDamage, cost = owner.Progress.AttackCost, gold = owner.Progress.Gold;
                    Click("Attack upgrade"); Check(owner.Progress.Gold == gold - cost && owner.Progress.Damage == damage, "Attack button spends exactly its displayed cost and applies the previewed damage");
                    float interval = owner.Progress.NextFireInterval; cost = owner.Progress.FireRateCost; gold = owner.Progress.Gold;
                    Click("Fire rate upgrade"); Check(owner.Progress.Gold == gold - cost && Mathf.Approximately(owner.Progress.FireInterval, interval), "Rate button spends exactly its displayed cost and applies the previewed interval");
                    Check(GameObject.Find("Upgrade panel").GetComponentsInChildren<Text>().Any(t => t.text.Contains("강화 완료")), "Successful purchase has visible feedback");
                    Snapshot("upgrades"); Snapshot("upgrades-4x3", 1024, 768); Snapshot("upgrades-small", 800, 450);
                    Click("Close upgrades"); beforeGold = owner.Progress.Gold; owner.OnZombieKilled(7, owner.GrandmaFootPosition + Vector2.right * 3f);
                    Check(owner.Progress.Gold == beforeGold + 7, "Kill credits currency exactly once before its animation");
                    Check(UnityEngine.Object.FindObjectsByType<Text>(FindObjectsSortMode.None).Any(t => t.text.Contains("(미정)재화")), "HUD uses the undecided resource name without a currency unit");
                    for (int i = 0; i < 8; i++) hud.RefreshPresentation();
                    Check(owner.Progress.Gold == beforeGold + 7, "Repeated presentation refresh cannot duplicate a reward");
                    feedbackStart = Time.unscaledTime; step = 3; return;
                }
                if (step == 3)
                {
                    if (Time.unscaledTime - feedbackStart < .25f) return;
                    Check(UnityEngine.Object.FindObjectsByType<Image>(FindObjectsSortMode.None).Any(i => i.name.StartsWith("Resource flight")), "Resource pickup flies in the UI layer while combat is paused");
                    Snapshot("resource-gain"); step = 4; return;
                }
                if (step == 4)
                {
                    if (Time.unscaledTime - feedbackStart < 1.5f) return;
                    Check(!UnityEngine.Object.FindObjectsByType<Image>(FindObjectsSortMode.None).Any(i => i.name.StartsWith("Resource flight")), "Resource flights return to their fixed-size pool after arrival");
                    Check(owner.Progress.Gold == beforeGold + 7, "Flight arrival does not grant currency again");
                    owner.TogglePause(); Click("Upgrade panel toggle"); Key(UnityEngine.InputSystem.Key.M); step = 5; return;
                }
                if (step == 5)
                {
                    Check(hud.IsMapOpen && !hud.IsUpgradeOpen && owner.IsPaused, "M replaces upgrades with the modal map and pauses running combat");
                    Click("Region 0"); Check(!Button("Travel to selected region").interactable, "Locked district travel remains disabled");
                    var hits = new List<RaycastResult>();
                    EventSystem.current.RaycastAll(new PointerEventData(EventSystem.current) { position = RectTransformUtility.WorldToScreenPoint(null, Button("Region 0").transform.position) }, hits);
                    Check(hits.Count > 0 && hits[0].gameObject.GetComponentInParent<Button>() == Button("Region 0"), "Map pin is above the modal backdrop in actual UI raycasts");
                    Snapshot("map"); Snapshot("map-4x3", 1024, 768); Snapshot("map-small", 800, 450);
                    File.WriteAllText(Observation, "MAP_READY"); Debug.Log("Hunter Grandma UI P0: map ready for live Game View review"); step = 6; return;
                }
                if (step == 6)
                {
                    if (!ReviewAcknowledged()) return;
                    Check(true, "Live Game View modal reviewed separately from camera-mode captures");
                    Key(UnityEngine.InputSystem.Key.Escape); step = 7; return;
                }
                if (step == 7)
                {
                    if (hud.IsMapOpen && Time.unscaledTime - inputQueuedAt < 2f) return;
                    Check(!hud.IsMapOpen && !owner.IsPaused, "Escape closes the map and restores running combat (map=" + hud.IsMapOpen + ", paused=" + owner.IsPaused + ", keyboard=" + Keyboard.current?.name + ", frames=" + (Time.frameCount - frame) + ", escape=" + keyboard.escapeKey.isPressed + ", pressed=" + keyboard.escapeKey.wasPressedThisFrame + ", mode=" + InputSystem.settings.updateMode + ", editorPause=" + EditorApplication.isPaused + ", focused=" + Application.isFocused + ", hud=" + hud.isActiveAndEnabled + ")");
                    InputSystem.QueueStateEvent(keyboard, new KeyboardState()); owner.TogglePause();
                    owner.Progress.AddGold(1000000);
                    while (owner.TryUpgradeAttack()) { }
                    while (owner.TryUpgradeFireRate()) { }
                    Click("Upgrade panel toggle"); hud.RefreshPresentation();
                    Check(!Button("Attack upgrade").interactable && !Button("Fire rate upgrade").interactable, "Both maximum upgrade buttons are disabled");
                    Check(Label("Attack upgrade").text.Contains("최대 강화") && Label("Fire rate upgrade").text.Contains("최대 강화"), "Maximum state uses an explicit label");
                    Check(owner.Progress.NextDamage == owner.Progress.Damage && Mathf.Approximately(owner.Progress.NextFireInterval, owner.Progress.FireInterval), "Preview remains at the maximum rather than promising an unavailable upgrade");
                    Snapshot("upgrades-max"); Click("Close upgrades");
                    var saved = new DefenseProgressStore(Profile).Load();
                    Check(saved.AttackLevel == 30 && saved.FireRateLevel == 15 && saved.Gold == owner.Progress.Gold, "UI purchases remain valid after isolated-profile disk reload");
                    owner.DamageDefense(100); hud.RefreshPresentation();
                    Check(!Button("Upgrade panel toggle").interactable, "Upgrade entry is visibly disabled after the run ends");
                    hud.ToggleUpgrade(); Check(!hud.IsUpgradeOpen, "Ended runs cannot reopen the upgrade panel through its keyboard action");
                    Check(errors.Count == 0, "No runtime errors, exceptions or assertions in the UI suite"); Finish(null);
                }
            }
            catch (Exception exception) { Finish(exception.Message + "\n" + exception.StackTrace); }
        }
        private static void Finish(string failure)
        {
            string report = "# UI P0 validation\n\nUnity " + Application.unityVersion + " · " + DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(9)).ToString("yyyy-MM-dd HH:mm") + " KST\n\nStatus: " + (failure == null ? "PASS" : "FAIL") + "\n\nIsolated profile: Library/UI-P0-validation.json. Synthetic funds and one synthetic kill are used only in this UI test profile; the ordinary player save is never reset.\n\n";
            report += string.Join("\n", checks.Select(c => "- PASS: " + c));
            if (failure != null) report += "\n\nFailure: " + failure;
            if (errors.Count > 0) report += "\n\nRuntime errors:\n" + string.Join("\n", errors);
            File.WriteAllText("Docs/Validation/UI-P0-validation.md", report);
            if (keyboard != null && keyboard.added) InputSystem.RemoveDevice(keyboard);
            SessionState.SetBool(Running, false); DefenseProgressStore.EditorValidationPath = null; Time.timeScale = 1;
            Debug.Log("Hunter Grandma UI P0 validation: " + (failure == null ? "PASS (" + checks.Count + " checks)" : "FAIL: " + failure));
            EditorApplication.ExitPlaymode();
        }
    }
}
