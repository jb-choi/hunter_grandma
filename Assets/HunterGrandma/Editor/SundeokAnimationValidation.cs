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
    public static class SundeokAnimationValidation
    {
        private const string Running = "HunterGrandma.Sundeok120.Validation";
        private const string Batch = Running + ".Batch";
        private static readonly string Profile = Path.GetFullPath("Library/Sundeok120-validation.json");
        private static readonly List<string> checks = new List<string>(), errors = new List<string>();
        private static readonly List<string> environmentErrors = new List<string>();
        private static readonly HashSet<int> walkFrames = new HashSet<int>();
        private static GrandmaDefense owner;
        private static GrandmaShooter grandma;
        private static ZombieActor target;
        private static ZombieActor[] pool;
        private static Keyboard keyboard;
        private static readonly Dictionary<SpriteRenderer, bool> rendererStates = new Dictionary<SpriteRenderer, bool>();
        private static int step, oldFrame, gold, shots;
        private static float until, clock;
        private static Vector2 foot;
        private static double deadline;
        private static InputSettings.BackgroundBehavior backgroundBehavior;
        private static InputSettings.EditorInputBehaviorInPlayMode editorInputBehavior;
        private static bool inputRoutingChanged;
        [Serializable] private sealed class TacticalMetadata { public TacticalPose[] frames; }
        [Serializable] private sealed class TacticalPose { public string action; public int local_frame; public int[] upper_offset_xy; }

        static SundeokAnimationValidation()
        {
            if (SessionState.GetBool(Running, false)) DefenseProgressStore.EditorValidationPath = Profile;
            if (Application.isBatchMode && SessionState.GetBool(Running, false)) AudioListener.volume = 0f;
            EditorApplication.update += Tick;
            EditorApplication.playModeStateChanged += state => { if (state == PlayModeStateChange.EnteredEditMode) { SessionState.SetBool(Running, false); DefenseProgressStore.EditorValidationPath = null; if (Application.isBatchMode && SessionState.GetBool(Batch + ".Done", false)) EditorApplication.Exit(SessionState.GetInt(Batch + ".Code", 1)); } };
            Application.logMessageReceived += (message, stack, type) =>
            {
                if (!SessionState.GetBool(Running, false) || (type != LogType.Error && type != LogType.Exception && type != LogType.Assert)) return;
                if (stack.Contains("UnityEditor.Search.SearchInit.IndexationOnStartup")) environmentErrors.Add(message + "\n" + stack);
                else errors.Add(message + "\n" + stack);
            };
        }
        public static void RunBatch()
        {
            if (!Application.isBatchMode) throw new InvalidOperationException("RunBatch requires -batchmode.");
            SessionState.SetBool(Batch, true); SessionState.SetBool(Batch + ".Done", false);
            AudioListener.volume = 0f; EditorSceneManager.OpenScene(GrandmaProjectSetup.ScenePath); Begin();
        }
        [MenuItem("Tools/Hunter Grandma/Run Sundeok 120 Animation Validation")]
        public static void Begin()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().isDirty) { Debug.LogError("Save the scene before animation validation."); return; }
            GrandmaStageSetup.Configure(); checks.Clear(); errors.Clear(); walkFrames.Clear(); rendererStates.Clear(); owner = null; grandma = null; target = null; pool = null; keyboard = null; step = 0;
            environmentErrors.Clear();
            foreach (string path in new[] { Profile, Profile + ".bak" }) if (File.Exists(path)) File.Delete(path);
            DefenseProgressStore.EditorValidationPath = Profile; SessionState.SetBool(Running, true);
            EditorSceneManager.OpenScene(GrandmaProjectSetup.ScenePath); EditorApplication.EnterPlaymode();
        }
        private static void Check(bool value, string message) { if (!value) throw new Exception(message); checks.Add(message); }
        private static void Snapshot(string name)
        { if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null) return; UnityEngine.Object.FindFirstObjectByType<DefenseHud>().RefreshPresentation(); GrandmaCombatValidation.Capture("Docs/Validation/Sundeok120-" + name + ".png", 1280, 720); }
        private static void Key(Key? key)
        {
            if (!Application.isBatchMode) { Type gameView = typeof(EditorWindow).Assembly.GetType("UnityEditor.GameView"); if (gameView != null) EditorWindow.GetWindow(gameView).Focus(); }
            keyboard.MakeCurrent(); InputSystem.QueueStateEvent(keyboard, key.HasValue ? new KeyboardState(key.Value) : new KeyboardState());
        }
        private static void PlaceTarget(Vector2 position)
        {
            target.Spawn(owner, position, 0, 0, 10000, 0, 1); target.ReserveShot();
            foreach (SpriteRenderer renderer in target.GetComponentsInChildren<SpriteRenderer>(true)) renderer.enabled = false;
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
                    if (owner.OpeningAge < .2f) return;
                    deadline = EditorApplication.timeSinceStartup + 90;
                    SundeokAnimationSet data = grandma.AnimationSet;
                    Check(data != null && data.IsReady, "Scene explicitly references the 120px animation asset");
                    if (Application.isBatchMode)
                    {
                        AudioListener.volume = 0f;
                        backgroundBehavior = InputSystem.settings.backgroundBehavior; editorInputBehavior = InputSystem.settings.editorInputBehaviorInPlayMode;
                        InputSystem.settings.backgroundBehavior = InputSettings.BackgroundBehavior.IgnoreFocus;
                        InputSystem.settings.editorInputBehaviorInPlayMode = InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView; inputRoutingChanged = true;
                        Check(AudioListener.volume == 0f, "Automated validation keeps hardware output muted without disabling source playback");
                    }
                    int[] counts = { 4, 6, 6, 1, 4, 4 }; float[] durations = { .88f, .84f, .90f, .42f, .42f, .68f };
                    for (int i = 0; i < 6; i++)
                    {
                        SundeokAnimationClip clip = data.Clip((SundeokAction)i);
                        Check(clip.frames.Length == counts[i] && Mathf.Abs(clip.Duration - durations[i]) < .001f, clip.action + " preserves the authored frame count and durations");
                        Check(clip.frames.All(f => f.composite != null && f.body != null && f.upper != null && f.composite.rect.size == Vector2.one * 120 && Vector2.Distance(f.composite.pivot, new Vector2(60, 2)) < .01f && f.composite.pixelsPerUnit == 32), clip.action + " has complete composite/body/upper sprites with the 120px foot pivot and PPU");
                    }
                    Check(LayersMatch(data), "Body plus exported upper layers reconstruct all 25 active source poses pixel-for-pixel");
                    var source = JsonUtility.FromJson<TacticalMetadata>(File.ReadAllText("Assets/Artsource/Drafts/GrandmaShooter120/v002/TacticalV2/pose_metadata.json"));
                    SundeokAnimationFrame ready = data.Clip(SundeokAction.Aim).frames[0];
                    foreach (TacticalPose pose in source.frames.Where(p => p.action == "walk_v2_tactical"))
                    {
                        SundeokAnimationFrame walk = data.Clip(SundeokAction.Walk).frames[pose.local_frame - 1];
                        Vector2 expected = new Vector2(pose.upper_offset_xy[0] - 5, pose.upper_offset_xy[1] - 6);
                        Vector2 offset = new Vector2(Mathf.Floor(walk.registration.x - ready.registration.x + .5f), walk.registration.y - ready.registration.y);
                        Check(offset == expected && Vector2.Distance(walk.muzzle, ready.muzzle + offset) < .001f, "Tactical walking Aim matches the authored upper placement and muzzle on gait frame " + pose.local_frame);
                    }
                    Check(data.Clip(SundeokAction.Walk).frames.Concat(data.Clip(SundeokAction.Aim).frames).Concat(data.Clip(SundeokAction.Fire).frames).All(f => AssetDatabase.GetAssetPath(f.composite).Contains("/TacticalV2/")), "Walk, held Aim and Fire explicitly use tactical exports");
                    Check(File.ReadAllBytes(AssetDatabase.GetAssetPath(ready.composite)).SequenceEqual(File.ReadAllBytes(AssetDatabase.GetAssetPath(data.Clip(SundeokAction.Fire).frames[0].composite))), "Stationary Aim holds the exact tactical ready pose without a legacy stance jump");
                    Check(owner.IsOpening && !grandma.GetComponent<BeltActorVisual>().Body.enabled && !GameObject.Find("120px upper body").GetComponent<SpriteRenderer>().enabled, "Tea opening hides both new world renderers");
                    owner.SkipStageOpening(); step = 1; return;
                }
                if (EditorApplication.timeSinceStartup > deadline) throw new Exception("Animation validation timeout at step " + step);
                if (step >= 4 && step <= 6 && !owner.IsPaused)
                {
                    target.transform.position = grandma.FootPosition + Vector2.right * (step == 4 ? 20 : 6);
                    walkFrames.Add(grandma.WalkingFrame);
                }
                if (step == 1)
                {
                    if (owner.Elapsed < 1f) return;
                    Check(owner.ShotsFired > 0 && grandma.GetComponent<BeltActorVisual>().Body.sprite.rect.width == 120, "Natural hunting after the intro uses the new sprite animation and actual automatic shots");
                    Snapshot("hunting"); shots = owner.ShotsFired;
                    foreach (ShotTracer tracer in UnityEngine.Object.FindObjectsByType<ShotTracer>(FindObjectsSortMode.None)) tracer.Cancel();
                    target = UnityEngine.Object.FindObjectsByType<ZombieActor>(FindObjectsSortMode.None).First(z => z.IsAlive);
                    pool = UnityEngine.Object.FindObjectsByType<ZombieActor>(FindObjectsInactive.Include, FindObjectsSortMode.None);
                    foreach (ZombieActor zombie in pool)
                    {
                        zombie.Despawn(); zombie.enabled = false; zombie.gameObject.SetActive(true);
                        foreach (SpriteRenderer renderer in zombie.GetComponentsInChildren<SpriteRenderer>(true)) { rendererStates[renderer] = renderer.enabled; renderer.enabled = false; }
                    }
                    if (owner.AutoExplore) owner.ToggleAutoExplore();
                    keyboard = InputSystem.AddDevice<Keyboard>("Sundeok animation validation keyboard"); Key(null);
                    grandma.StopCombatPresentation(); grandma.MoveOnBelt(StageZeroOpeningView.Exit); oldFrame = grandma.AnimationFrame; foot = grandma.FootPosition;
                    until = Time.unscaledTime + .35f; step = 2; return;
                }
                if (step == 2)
                {
                    if (Time.unscaledTime < until) return;
                    Check(grandma.AnimationAction == SundeokAction.Idle && !grandma.IsLayeredAnimation, "Stationary Sundeok plays the authored Idle composite");
                    oldFrame = grandma.AnimationFrame; until = Time.unscaledTime + .26f; Snapshot("idle"); step = 3; return;
                }
                if (step == 3)
                {
                    if (Time.unscaledTime < until) return;
                    Check(grandma.AnimationFrame != oldFrame && grandma.FootPosition == foot, "Idle advances frames without moving the logical feet");
                    PlaceTarget(grandma.FootPosition + Vector2.right * 20);
                    walkFrames.Clear(); Key(UnityEngine.InputSystem.Key.D); until = Time.unscaledTime + .95f; step = 4; return;
                }
                if (step == 4)
                {
                    // First sample plain walking, then allow the authored aiming upper body.
                    if (Time.unscaledTime < until) return;
                    Check(grandma.AnimationAction == SundeokAction.Walk && grandma.FootPosition.x > foot.x + .5f, "Movement API advances the authored Walk frames and the actual root");
                    Snapshot("walk"); target.transform.position = grandma.FootPosition + Vector2.right * 6; until = Time.unscaledTime + .50f; step = 5; return;
                }
                if (step == 5)
                {
                    if (Time.unscaledTime < until) return;
                    Check(grandma.AnimationAction == SundeokAction.Aim && grandma.IsLayeredAnimation && walkFrames.Count >= 4, "Walking continues underneath the authored Aim upper body");
                    Check(grandma.GetComponent<BeltActorVisual>().Body.sprite == grandma.AnimationSet.Clip(SundeokAction.Walk).frames[grandma.WalkingFrame].body, "Layered aiming uses the weapon-free Walk body without duplicate arms or guns");
                    Snapshot("walk-aim"); grandma.Fire();
                    Check(grandma.AnimationAction == SundeokAction.Fire && grandma.AnimationFrame == 1 && grandma.IsLayeredAnimation, "A committed shot starts on the authored muzzle-flash frame while walking");
                    Check(grandma.MuzzlePosition.x > grandma.FootPosition.x, "Right-facing shot uses a muzzle in front of the character"); Snapshot("walk-fire");
                    until = Time.unscaledTime + .35f; step = 6; return;
                }
                if (step == 6)
                {
                    if (Time.unscaledTime < until) return;
                    Check(grandma.AnimationAction == SundeokAction.Reload && grandma.IsLayeredAnimation, "Shot recovery transitions into the authored Reload while the legs keep walking"); Snapshot("walk-reload");
                    owner.TogglePause(); clock = grandma.AnimationTime; oldFrame = grandma.AnimationFrame; foot = grandma.FootPosition; until = Time.unscaledTime + .25f; step = 7; return;
                }
                if (step == 7)
                {
                    if (Time.unscaledTime < until) return;
                    Check(Mathf.Approximately(clock, grandma.AnimationTime) && oldFrame == grandma.AnimationFrame && grandma.FootPosition == foot, "Pause freezes clip time, pose and feet together");
                    owner.TogglePause(); Key(null); target.Despawn(); target.gameObject.SetActive(true); until = Time.unscaledTime + .7f; step = 8; return;
                }
                if (step == 8)
                {
                    if (Time.unscaledTime < until) return;
                    Check(grandma.AnimationAction == SundeokAction.Idle, "Reload returns to Idle when combat and movement stop");
                    gold = owner.Progress.Gold; owner.OnZombieKilled(7, grandma.FootPosition + Vector2.right * 2); foot = grandma.FootPosition;
                    until = Time.unscaledTime + .25f; step = 9; return;
                }
                if (step == 9)
                {
                    if (Time.unscaledTime < until) return;
                    Check(grandma.IsCollecting && grandma.AnimationAction == SundeokAction.Collect && !grandma.IsLayeredAnimation, "A queued pickup plays the authored bending Collect composite while safe and stationary");
                    Check(owner.Progress.Gold == gold + 7 && grandma.FootPosition == foot, "Collect presentation neither grants a duplicate reward nor moves the feet"); Snapshot("collect");
                    Key(UnityEngine.InputSystem.Key.D); until = Time.unscaledTime + .1f; step = 10; return;
                }
                if (step == 10)
                {
                    if (Time.unscaledTime < until) return;
                    Check(!grandma.IsCollecting && grandma.FootPosition.x > foot.x && owner.Progress.Gold == gold + 7, "Keyboard movement interrupts Collect without repeating or removing its reward");
                    Key(null); PlaceTarget(grandma.FootPosition + Vector2.left * 6); until = Time.unscaledTime + .1f; step = 11; return;
                }
                if (step == 11)
                {
                    if (Time.unscaledTime < until) return;
                    grandma.SetCombatTarget(target, 2f); grandma.Fire();
                    SundeokAnimationFrame fx = grandma.AnimationSet.Clip(SundeokAction.Fire).frames[1];
                    Vector3 expected = grandma.GetComponent<BeltActorVisual>().Display.TransformPoint(new Vector3((60 - fx.muzzle.x) / 32, (118 - fx.muzzle.y) / 32));
                    Check(grandma.GetComponent<BeltActorVisual>().Body.flipX && grandma.MuzzlePosition.x < grandma.FootPosition.x && Vector3.Distance(grandma.MuzzlePosition, expected) < .001f, "Left-facing fire mirrors the sprite and exact authored muzzle around the foot pivot"); Snapshot("left-fire");
                    Check(owner.ShotsFired == shots, "Isolated visual Fire calls cannot create gameplay shots or damage");
                    foreach (ZombieActor zombie in pool) zombie.enabled = true;
                    foreach (var renderer in rendererStates) if (renderer.Key != null) renderer.Key.enabled = renderer.Value;
                    owner.RestartEncounter();
                    Check(grandma.AnimationAction == SundeokAction.Idle && grandma.AnimationFrame == 0 && !grandma.IsCollecting && !grandma.IsLayeredAnimation, "Retry clears aim/fire/reload/collect clocks and layered state");
                    Check(errors.Count == 0, "No runtime errors, exceptions or assertions in animation validation"); Finish(null);
                }
            }
            catch (Exception exception) { Finish(exception.Message + "\n" + exception.StackTrace); }
        }
        private static void Finish(string failure)
        {
            string report = "# Sundeok 120px animation validation\n\nUnity " + Application.unityVersion + " · " + DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(9)).ToString("yyyy-MM-dd HH:mm") + " KST\n\nStatus: " + (failure == null ? "PASS" : "FAIL") + "\n\nIsolated profile: Library/Sundeok120-validation.json. After one natural hunting second, prior projectiles are cancelled and dead/disabled pool slots are kept active to prevent extra spawns. One hidden reserved target allows real encounter movement/aim updates without new gameplay shots. Synthetic keyboard input drives movement; actual animation/aim/fire/collect APIs run. One synthetic +7 reward verifies that Collect does not credit currency again. No ordinary save reset.\n\n";
            report += string.Join("\n", checks.Select(c => "- PASS: " + c)); if (failure != null) report += "\n\nFailure: " + failure;
            if (errors.Count > 0) report += "\n\nRuntime errors:\n" + string.Join("\n", errors);
            if (environmentErrors.Count > 0) report += "\n\nSeparate Unity Editor Search startup/indexing errors:\n" + string.Join("\n", environmentErrors);
            if (Application.isBatchMode) report += "\n\nBatch execution mutes AudioListener output; no computer-use tools or ordinary save edits.\n";
            Directory.CreateDirectory("Docs/Validation"); File.WriteAllText("Docs/Validation/Sundeok120-animation-validation.md", report);
            if (owner != null) owner.enabled = true;
            if (pool != null) foreach (ZombieActor zombie in pool) if (zombie != null) zombie.enabled = true;
            foreach (var renderer in rendererStates) if (renderer.Key != null) renderer.Key.enabled = renderer.Value;
            if (keyboard != null && keyboard.added) InputSystem.RemoveDevice(keyboard);
            if (inputRoutingChanged) { InputSystem.settings.backgroundBehavior = backgroundBehavior; InputSystem.settings.editorInputBehaviorInPlayMode = editorInputBehavior; inputRoutingChanged = false; }
            SessionState.SetBool(Running, false); DefenseProgressStore.EditorValidationPath = null; Time.timeScale = 1;
            if (Application.isBatchMode) { SessionState.SetBool(Batch + ".Done", true); SessionState.SetInt(Batch + ".Code", failure == null ? 0 : 1); }
            Debug.Log("Hunter Grandma Sundeok120 validation: " + (failure == null ? "PASS (" + checks.Count + " checks)" : "FAIL: " + failure)); EditorApplication.ExitPlaymode();
        }
        private static bool LayersMatch(SundeokAnimationSet data)
        {
            foreach (SundeokAction action in Enum.GetValues(typeof(SundeokAction)))
                foreach (SundeokAnimationFrame frame in data.Clip(action).frames)
                {
                    var textures = new Texture2D[3]; Sprite[] sprites = { frame.composite, frame.body, frame.upper };
                    for (int i = 0; i < 3; i++) { textures[i] = new Texture2D(2, 2, TextureFormat.RGBA32, false); textures[i].LoadImage(File.ReadAllBytes(AssetDatabase.GetAssetPath(sprites[i]))); }
                    Color32[] composite = textures[0].GetPixels32(), body = textures[1].GetPixels32(), upper = textures[2].GetPixels32(); bool match = true;
                    for (int i = 0; i < composite.Length; i++)
                    { Color32 value = upper[i].a > 0 ? upper[i] : body[i]; if (value.a != composite[i].a || (value.a > 0 && (value.r != composite[i].r || value.g != composite[i].g || value.b != composite[i].b))) { match = false; break; } }
                    foreach (Texture2D texture in textures) UnityEngine.Object.DestroyImmediate(texture);
                    if (!match) return false;
                }
            return true;
        }
    }
}
