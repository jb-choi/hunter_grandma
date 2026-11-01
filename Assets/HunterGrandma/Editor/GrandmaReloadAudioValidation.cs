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
    [InitializeOnLoad]
    public static class GrandmaReloadAudioValidation
    {
        private const string Running = "HunterGrandma.ReloadAudio.Validation", Batch = Running + ".Batch";
        private static readonly string Profile = Path.GetFullPath("Library/ReloadAudio-validation.json");
        private static readonly List<string> checks = new List<string>(), errors = new List<string>(), editorErrors = new List<string>();
        private static GrandmaDefense owner;
        private static GrandmaShooter grandma;
        private static AudioSource shot, reload;
        private static AudioClip clip;
        private static GameObject clockObject;
        private static object clock;
        private static Type clockType;
        private static int step, frame, clockCues;
        private static float until, animationTime, originalPitch;
        private static double deadline, waitUntil;
        private static bool originalMute, hardwarePlayback;

        static GrandmaReloadAudioValidation()
        {
            if (SessionState.GetBool(Running, false)) DefenseProgressStore.EditorValidationPath = Profile;
            if (Application.isBatchMode && SessionState.GetBool(Running, false)) AudioListener.volume = 0f;
            EditorApplication.update += Tick;
            EditorApplication.playModeStateChanged += state =>
            {
                if (state != PlayModeStateChange.EnteredEditMode) return;
                SessionState.SetBool(Running, false); DefenseProgressStore.EditorValidationPath = null;
                if (Application.isBatchMode && SessionState.GetBool(Batch + ".Done", false)) EditorApplication.Exit(SessionState.GetInt(Batch + ".Code", 1));
            };
            Application.logMessageReceived += (message, stack, type) =>
            {
                if (!SessionState.GetBool(Running, false) || (type != LogType.Error && type != LogType.Exception && type != LogType.Assert)) return;
                if (stack.Contains("UnityEditor.Search.SearchInit.IndexationOnStartup")) editorErrors.Add(message + "\n" + stack);
                else errors.Add(message + "\n" + stack);
            };
        }
        public static void RunBatch()
        {
            if (!Application.isBatchMode) throw new InvalidOperationException("Use -batchmode for RunBatch.");
            SessionState.SetBool(Batch, true); SessionState.SetBool(Batch + ".Done", false);
            AudioListener.volume = 0f;
            EditorSceneManager.OpenScene(GrandmaProjectSetup.ScenePath); Begin();
        }
        [MenuItem("Tools/Hunter Grandma/Run Reload Audio Validation")]
        public static void Begin()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().isDirty) { Debug.LogError("Save the scene before reload audio validation."); return; }
            GrandmaStageSetup.Configure(); checks.Clear(); errors.Clear(); editorErrors.Clear();
            owner = null; grandma = null; shot = reload = null; clock = null; clockObject = null; step = 0; clockCues = 0;
            foreach (string path in new[] { Profile, Profile + ".bak" }) if (File.Exists(path)) File.Delete(path);
            DefenseProgressStore.EditorValidationPath = Profile; SessionState.SetBool(Running, true);
            EditorSceneManager.OpenScene(GrandmaProjectSetup.ScenePath); EditorApplication.EnterPlaymode();
        }
        private static void Check(bool value, string message) { if (!value) throw new Exception(message); checks.Add(message); }
        private static void Clock(string method, params object[] args) => clockType.GetMethod(method).Invoke(clock, args);
        private static T ClockValue<T>(string property) => (T)clockType.GetProperty(property).GetValue(clock);
        private static void Tick()
        {
            if (!SessionState.GetBool(Running, false) || !EditorApplication.isPlaying || EditorApplication.isCompiling) return;
            try
            {
                if (owner == null) owner = UnityEngine.Object.FindFirstObjectByType<GrandmaDefense>();
                if (owner == null || owner.Progress == null) return;
                if (step != 0 && EditorApplication.timeSinceStartup > deadline) throw new Exception("Reload validation timeout at step " + step);
                if (step == 0)
                {
                    deadline = EditorApplication.timeSinceStartup + 40;
                    grandma = UnityEngine.Object.FindFirstObjectByType<GrandmaShooter>(); clip = grandma.ReloadSound;
                    Check(clip != null && AssetDatabase.GetAssetPath(clip) == "Assets/HunterGrandma/Audio/sundeok_reload_clack.wav", "The actual scene assigns the licensed reload WAV to Sundeok");
                    Check(clip.channels == 1 && clip.frequency == 44100 && clip.length > .43f && clip.length < .44f, "Imported audio preserves the prepared 44.1kHz mono 0.436-second recording");
                    var importer = (AudioImporter)AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(clip));
                    Check(importer.defaultSampleSettings.preloadAudioData && !importer.loadInBackground && importer.defaultSampleSettings.loadType == AudioClipLoadType.DecompressOnLoad && importer.defaultSampleSettings.compressionFormat == AudioCompressionFormat.PCM, "Short reload audio is preloaded PCM and does not wait on background decoding");
                    owner.SkipStageOpening(); owner.enabled = false;
                    foreach (ZombieActor zombie in UnityEngine.Object.FindObjectsByType<ZombieActor>(FindObjectsInactive.Include, FindObjectsSortMode.None)) zombie.Despawn();
                    shot = grandma.GetComponent<AudioSource>(); originalPitch = shot.pitch; originalMute = shot.mute;
                    Check(grandma.GetComponents<AudioSource>().Length == 2, "One persistent reload source supplements the existing gunshot source");
                    clockObject = new GameObject("Reload timeline fixture", typeof(SpriteRenderer), typeof(BeltActorVisual));
                    var renderer = clockObject.GetComponent<SpriteRenderer>(); renderer.sprite = grandma.IdleSprite; renderer.sharedMaterial = grandma.ActorMaterial;
                    var visual = clockObject.GetComponent<BeltActorVisual>(); visual.Initialize(renderer, owner.ShadowSprite, .8f);
                    clockType = typeof(GrandmaShooter).Assembly.GetType("HunterGrandma.SundeokAnimationPlayer");
                    clock = Activator.CreateInstance(clockType, grandma.AnimationSet, visual, new Action(() => clockCues++));
                    Clock("Advance", .3f, false, 1f);
                    Clock("QueueCollect"); Clock("TryCollect"); Clock("Advance", .2f, false, 1f);
                    Check(clockCues == 0, "Idle and Collect never request a reload sound");
                    Clock("Reset"); Clock("Fire", true, 1f, .72f); Clock("Advance", .3f, true, 1f);
                    Check(clockCues == 0, "The committed firing frame does not prematurely play a reload sound");
                    frame = Time.frameCount; step = 1; return;
                }
                if (step == 1 && Time.frameCount > frame)
                {
                    Clock("Advance", .3f, true, 1f);
                    Check(clockCues == 1 && ClockValue<SundeokAction>("Action") == SundeokAction.Reload && ClockValue<int>("FrameIndex") >= 1, "Crossing reload frame 2 in one long frame requests exactly one sound");
                    Check(ClockValue<bool>("IsLayered"), "Moving upper-body reload uses the same sound cue");
                    Clock("Advance", 0f, true, 1f); Clock("Advance", .01f, true, 1f);
                    Check(clockCues == 1, "Pause and repeated sampling of the reload frame do not duplicate the cue");
                    Clock("StopCombat"); Clock("Advance", .8f, false, 1f);
                    Check(clockCues == 1, "Cancelling an active reload never schedules a delayed cue");
                    Clock("Reset"); clockCues = 0; Clock("Fire", false, -1f, .18f); frame = Time.frameCount; step = 2; return;
                }
                if (step == 2 && Time.frameCount > frame)
                {
                    Clock("Advance", .1f, false, -1f); Clock("Advance", .001f, false, -1f);
                    Check(clockCues == 1, "Fast left-facing reload requests one cue despite skipping intermediate frames");
                    grandma.ResetPose(); grandma.Fire(); until = Time.unscaledTime + .04f; step = 3; return;
                }
                if (step == 3 && Time.unscaledTime >= until)
                {
                    Check(grandma.ReloadSoundCues == 0, "Real Sundeok does not start the reload sound during the firing pose");
                    animationTime = grandma.AnimationTime; owner.TogglePause(); waitUntil = EditorApplication.timeSinceStartup + .12; step = 4; return;
                }
                if (step == 4 && EditorApplication.timeSinceStartup >= waitUntil)
                {
                    Check(grandma.ReloadSoundCues == 0 && Mathf.Abs(grandma.AnimationTime - animationTime) < .0001f, "Paused presentation neither advances into reload nor emits a cue");
                    owner.TogglePause(); step = 5; return;
                }
                if (step == 5 && grandma.ReloadSoundCues > 0)
                {
                    Check(grandma.ReloadSoundCues == 1 && grandma.AnimationAction == SundeokAction.Reload && grandma.AnimationFrame >= 1, "Actual animation triggers one clip at the loading-hand frame");
                    reload = grandma.GetComponents<AudioSource>().Single(a => a != shot);
                    Check(reload.clip == clip && !reload.loop && !reload.playOnAwake && reload.spatialBlend == 0, "The dedicated persistent 2D source plays the correct non-looping clip");
                    Check(reload.outputAudioMixerGroup == shot.outputAudioMixerGroup && reload.mute == shot.mute && Mathf.Abs(reload.volume - shot.volume * .13f) < .0001f, "Reload respects the existing source mixer, mute and master volume");
                    Check(Mathf.Abs(reload.pitch - 1f) < .001f && shot.pitch == originalPitch, "Normal cadence preserves the recording's pitch and leaves gunshots unchanged");
                    hardwarePlayback = reload.isPlaying;
                    animationTime = grandma.AnimationTime; owner.TogglePause(); waitUntil = EditorApplication.timeSinceStartup + .12; step = 6; return;
                }
                if (step == 6 && EditorApplication.timeSinceStartup >= waitUntil)
                {
                    Check(grandma.ReloadSoundCues == 1 && Mathf.Abs(grandma.AnimationTime - animationTime) < .0001f, "Pausing an active reload preserves its one-shot event and frozen pose");
                    if (hardwarePlayback) Check(!reload.isPlaying, "Pause suspends the active AudioSource without restarting it");
                    owner.TogglePause(); until = Time.unscaledTime + .8f; step = 7; return;
                }
                if (step == 7 && Time.unscaledTime >= until)
                {
                    Check(grandma.ReloadSoundCues == 1 && grandma.AnimationAction != SundeokAction.Reload, "The completed reload does not repeat its sound in idle");
                    shot.mute = true; grandma.ResetPose(); grandma.Fire(); until = Time.unscaledTime + .8f; step = 8; return;
                }
                if (step == 8 && Time.unscaledTime >= until)
                {
                    Check(grandma.ReloadSoundCues == 0 && reload.mute, "Muted weapon audio produces no audible reload request"); shot.mute = originalMute;
                    owner.Progress.AddGold(100000); for (int i = 0; i < 15; i++) owner.Progress.TryUpgradeFireRate();
                    grandma.ResetPose(); grandma.Fire(); step = 9; return;
                }
                if (step == 9 && grandma.ReloadSoundCues > 0)
                {
                    Check(grandma.ReloadSoundCues == 1 && reload.pitch > 1f && reload.pitch <= 3f && shot.pitch == originalPitch, "Rapid-fire upgrades shorten reload audio with bounded pitch on its own source");
                    grandma.Fire(); Check(!reload.isPlaying, "A new gunshot stops the previous reload tail instead of stacking voices");
                    grandma.StopCombatPresentation(); until = Time.unscaledTime + .5f; step = 10; return;
                }
                if (step == 10 && Time.unscaledTime >= until)
                {
                    Check(grandma.ReloadSoundCues == 1 && !reload.isPlaying, "Stopping combat cancels pending/repeated reload playback");
                    typeof(GrandmaShooter).GetField("reloadSound", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(grandma, null);
                    grandma.ResetPose(); grandma.Fire(); until = Time.unscaledTime + .5f; step = 11; return;
                }
                if (step == 11 && Time.unscaledTime >= until)
                {
                    Check(grandma.ReloadSoundCues == 0, "A missing optional clip does not break animation or emit a sound");
                    typeof(GrandmaShooter).GetField("reloadSound", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(grandma, clip);
                    Check(owner.ShotsFired == 0 && owner.KillCount == 0, "Audio and animation-only calls never generate gameplay shots or kills");
                    owner.RestartEncounter(); Check(grandma.ReloadSoundCues == 0 && !reload.isPlaying && grandma.ReloadSound == clip, "Retry clears playback while preserving the borrowed clip asset");
                    Check(errors.Count == 0, "No gameplay errors, exceptions or assertions in reload audio validation"); Finish(null);
                }
            }
            catch (Exception exception) { Finish(exception.Message + "\n" + exception.StackTrace); }
        }
        private static void Finish(string failure)
        {
            string report = "# Sundeok reload audio validation\n\nUnity " + Application.unityVersion + " · " + DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(9)).ToString("yyyy-MM-dd HH:mm") + " KST\n\nStatus: " + (failure == null ? "PASS" : "FAIL") + "\n\nIsolated profile: Library/ReloadAudio-validation.json. Encounter Update is disabled after skipping the opening; audio and actual animation presentation continue. One synthetic balance funds fire-rate upgrades only in this fixture. Timeline tests use an independent presentation clone and public timeline methods via reflection; no computer-use/native UI input is used.\n\n";
            report += string.Join("\n", checks.Select(c => "- PASS: " + c));
            report += "\n\nAudioSource reported active playback: " + hardwarePlayback + ". This is routing/timing validation, not acoustic listening or a hardware-output measurement.\n";
            if (failure != null) report += "\nFailure: " + failure;
            if (errors.Count > 0) report += "\n\nGameplay errors:\n" + string.Join("\n", errors);
            if (editorErrors.Count > 0) report += "\n\nSeparate Unity Editor Search startup errors:\n" + string.Join("\n", editorErrors);
            Directory.CreateDirectory("Docs/Validation"); File.WriteAllText("Docs/Validation/ReloadAudio-validation.md", report);
            if (grandma != null) { typeof(GrandmaShooter).GetField("reloadSound", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(grandma, clip); grandma.StopCombatPresentation(); }
            if (shot != null) shot.mute = originalMute;
            if (clockObject != null) UnityEngine.Object.Destroy(clockObject);
            if (owner != null) owner.enabled = true;
            SessionState.SetBool(Running, false); DefenseProgressStore.EditorValidationPath = null; Time.timeScale = 1;
            if (SessionState.GetBool(Batch, false)) { SessionState.SetInt(Batch + ".Code", failure == null ? 0 : 1); SessionState.SetBool(Batch + ".Done", true); }
            Debug.Log("Hunter Grandma reload audio validation: " + (failure == null ? "PASS (" + checks.Count + " checks)" : "FAIL: " + failure)); EditorApplication.ExitPlaymode();
        }
    }
}
