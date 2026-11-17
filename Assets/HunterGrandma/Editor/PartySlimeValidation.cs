using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace HunterGrandma.Editor
{
    [InitializeOnLoad]
    public static class PartySlimeValidation
    {
        private const string Key = "HunterGrandma.PartySlime.Validation";
        private static readonly string Profile = Path.GetFullPath("Library/PartySlime-validation-save.json");
        private static readonly List<string> checks = new List<string>(), errors = new List<string>(), environmentErrors = new List<string>();
        private static GrandmaDefense owner;
        private static ZombieActor first, second;
        private static SpriteRenderer pop;
        private static Sprite pausedSprite;
        private static Vector3 pausedPosition;
        private static int step, kills, gold, children;
        private static double until, deadline;
        static PartySlimeValidation()
        {
            if (SessionState.GetBool(Key, false)) { DefenseProgressStore.EditorValidationPath = Profile; AudioListener.volume = 0; }
            EditorApplication.update += Tick;
            EditorApplication.playModeStateChanged += state =>
            {
                if (state != PlayModeStateChange.EnteredEditMode || !SessionState.GetBool(Key + ".Done", false)) return;
                SessionState.SetBool(Key, false); DefenseProgressStore.EditorValidationPath = null;
                if (Application.isBatchMode) EditorApplication.Exit(SessionState.GetInt(Key + ".Code", 1));
            };
            Application.logMessageReceived += (message, stack, type) =>
            {
                if (!SessionState.GetBool(Key, false) || (type != LogType.Error && type != LogType.Exception && type != LogType.Assert)) return;
                if (stack.Contains("UnityEditor.Search.SearchInit.IndexationOnStartup")) environmentErrors.Add(message + "\n" + stack);
                else errors.Add(message + "\n" + stack);
            };
        }
        public static void RunBatch()
        {
            if (!Application.isBatchMode) throw new InvalidOperationException("Use -batchmode.");
            try
            {
                PartySlimeSetup.Configure(); GrandmaStageSetup.Configure();
                AssetChecks(); SessionState.SetString(Key + ".Assets", string.Join("\n", checks));
                foreach (string path in new[] { Profile, Profile + ".bak" }) if (File.Exists(path)) File.Delete(path);
                SessionState.SetBool(Key, true); SessionState.SetBool(Key + ".Done", false);
                DefenseProgressStore.EditorValidationPath = Profile; AudioListener.volume = 0;
                EditorSceneManager.OpenScene(GrandmaProjectSetup.ScenePath); EditorApplication.EnterPlaymode();
            }
            catch (Exception error) { Debug.LogException(error); EditorApplication.Exit(1); }
        }
        private static void Check(bool condition, string message)
        { if (!condition) throw new Exception(message); checks.Add(message); }
        private static void AssetChecks()
        {
            for (int k = 1; k <= 10; k++)
            {
                for (int f = 1; f <= 16; f++)
                {
                    string path = PartySlimeSetup.Root + "Z" + k.ToString("00") + "/frame_" + f.ToString("00") + ".png";
                    Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                    var importer = (TextureImporter)AssetImporter.GetAtPath(path);
                    Check(sprite != null && sprite.rect.size == new Vector2(120, 120) && Vector2.Distance(sprite.pivot, new Vector2(60, 6)) < .01f &&
                        sprite.pixelsPerUnit == 32 && importer.filterMode == FilterMode.Point && !importer.mipmapEnabled &&
                        importer.textureCompression == TextureImporterCompression.Uncompressed, "Z" + k.ToString("00") + " frame " + f + ": full 120px canvas, foot pivot, PPU32, Point/uncompressed");
                }
            }
            var art = AssetDatabase.LoadAssetAtPath<ZombieAnimationSet>(ZombieAnimationSetup.AssetPath);
            Check(art != null && art.IsReady, "The existing animation asset resolves all five active zombie kinds");
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/HunterGrandma/Prefabs/Zombie.prefab");
            Check(prefab.GetComponent<ZombieActor>().AnimationSet == art, "Pool prefab resolves the same authored animation asset as scene actors");
            foreach (ZombieKind kind in new[] { ZombieKind.PartyBalloon, ZombieKind.PartyHorn })
            {
                foreach (ZombieAction action in Enum.GetValues(typeof(ZombieAction)))
                {
                    var clip = art.Clip(kind, action);
                    Check(clip.frames.Length == 4 && clip.Sample(0, true) == clip.frames[0] && clip.Sample(100, false) == clip.frames[3], kind + " " + action + ": ordered four-frame playback and one-shot ending");
                }
            }
            Check(art.PartyPop != null && art.PartyPop.frames.Length == 22, "The reviewed seeded pixelfx export resolves all 22 frames");
            foreach (Sprite frame in art.PartyPop.frames)
                Check(frame.rect.size == new Vector2(112, 112) && Vector2.Distance(frame.pivot, new Vector2(56, 48)) < .01f && frame.pixelsPerUnit == 32,
                    "PixelFX frame preserves 112px canvas, exact origin and PPU32: " + frame.name);
            for (int id = 0; id < DefenseStages.Count; id++)
            {
                var kinds = new HashSet<ZombieKind>(); var stage = DefenseStages.Get(id);
                for (int wave = 1; wave <= stage.WaveCount; wave++) for (int n = 0; n < stage.EnemiesPerWave; n++) kinds.Add(stage.EnemyKind(n, wave));
                Check(kinds.Contains(ZombieKind.PartyBalloon) == (id >= 1) && kinds.Contains(ZombieKind.PartyHorn) == (id >= 2), "Stage " + id + ": Z1 begins at 1, Z2 begins at 2");
                Check(id == 0 ? kinds.SetEquals(new[] { ZombieKind.Infected }) : kinds.Contains(ZombieKind.Normal) && kinds.Contains(ZombieKind.Infected) && kinds.Contains(ZombieKind.Large) == (id >= 5),
                    "Stage " + id + ": existing roster is retained");
            }
        }
        private static SpriteRenderer Pop(ZombieActor actor) => actor.transform.Find("Party death pop").GetComponent<SpriteRenderer>();
        private static void Spawn(ZombieActor actor, ZombieKind kind, float x)
        { actor.Spawn(owner, owner.GrandmaFootPosition + Vector2.right * x, 0, 0, 10, 7, 1, kind); }
        private static void Capture(string name)
        {
            if (SystemInfo.graphicsDeviceType != UnityEngine.Rendering.GraphicsDeviceType.Null)
                GrandmaCombatValidation.Capture("Docs/Validation/PartySlime-" + name + ".png", 1280, 720);
        }
        private static void Tick()
        {
            if (!SessionState.GetBool(Key, false) || SessionState.GetBool(Key + ".Done", false) || !EditorApplication.isPlaying || EditorApplication.isCompiling) return;
            try
            {
                if (owner == null) owner = UnityEngine.Object.FindFirstObjectByType<GrandmaDefense>();
                if (owner == null || owner.Progress == null) return;
                if (step > 0 && EditorApplication.timeSinceStartup > deadline) throw new Exception("Party validation timeout at step " + step);
                if (step == 0)
                {
                    checks.Clear(); checks.AddRange(SessionState.GetString(Key + ".Assets", "").Split('\n'));
                    deadline = EditorApplication.timeSinceStartup + 40; owner.SkipStageOpening();
                    for (int id = 0; id < 2; id++) { var run = new DefenseRun(id); run.CompleteWaves(); Check(owner.Progress.TryCompleteWaves(run), "Isolated validation profile unlocks next district"); }
                    Check(owner.EnterStage(2), "Actual entry scene enters Stage 2 with the isolated profile"); owner.enabled = false;
                    foreach (ShotTracer shot in UnityEngine.Object.FindObjectsByType<ShotTracer>(FindObjectsInactive.Include, FindObjectsSortMode.None)) shot.Cancel();
                    var actors = UnityEngine.Object.FindObjectsByType<ZombieActor>(FindObjectsInactive.Include, FindObjectsSortMode.None);
                    foreach (var actor in actors) actor.Despawn();
                    first = actors[0]; second = actors[1]; Spawn(first, ZombieKind.PartyBalloon, 3); Spawn(second, ZombieKind.PartyHorn, 6);
                    until = EditorApplication.timeSinceStartup + .18; step = 1; return;
                }
                if (EditorApplication.timeSinceStartup < until) return;
                if (step == 1)
                {
                    Check(first.IsAlive && second.IsAlive && first.AnimationAction == ZombieAction.Walk && second.AnimationAction == ZombieAction.Walk,
                        "Both party actors use their imported walk clock in the actual scene");
                    Check(first.GetComponent<BeltActorVisual>().Body.flipX && second.GetComponent<BeltActorVisual>().Body.flipX, "Party characters face left toward Sundeok");
                    Capture("lineup"); kills = owner.KillCount; gold = owner.Progress.Gold;
                    first.ReceiveDamage(99); first.ReceiveDamage(99);
                    Check(owner.KillCount == kills + 1 && owner.Progress.Gold == gold + 7 && first.IsMelting, "Repeated lethal damage credits exactly one kill and one reward");
                    pop = Pop(first); until = EditorApplication.timeSinceStartup + .19; step = 2; return;
                }
                if (step == 2)
                {
                    Check(pop.enabled && pop.sprite != null && first.AnimationAction == ZombieAction.Dead, "Authored pop/death and pixelfx confetti play together after the lethal hit");
                    Check(pop.GetComponentInParent<UnityEngine.Rendering.SortingGroup>() == first.GetComponent<BeltActorVisual>().Group && pop.sortingOrder > first.GetComponent<BeltActorVisual>().Body.sortingOrder,
                        "Pop uses the actor's foot-depth group above the body");
                    Capture("pop"); owner.TogglePause(); pausedSprite = pop.sprite; pausedPosition = first.transform.position;
                    until = EditorApplication.timeSinceStartup + .20; step = 3; return;
                }
                if (step == 3)
                {
                    Check(pop.enabled && pop.sprite == pausedSprite && first.transform.position == pausedPosition, "Pause freezes both pop presentation and actor position");
                    owner.TogglePause(); until = EditorApplication.timeSinceStartup + .85; step = 4; return;
                }
                if (step == 4)
                {
                    Check(!pop.enabled && pop.sprite == null, "One-shot pop clears itself after resuming");
                    second.ReceiveDamage(99); until = EditorApplication.timeSinceStartup + .19; step = 5; return;
                }
                if (step == 5)
                {
                    Check(Pop(second).enabled, "Z2 also triggers the shared party pop"); Capture("horn-pop");
                    owner.RestartEncounter();
                    Check(!Pop(first).enabled && !Pop(second).enabled, "Restart cancels visible and pending effects across the actor pool");
                    foreach (var actor in UnityEngine.Object.FindObjectsByType<ZombieActor>(FindObjectsInactive.Include, FindObjectsSortMode.None)) actor.Despawn();
                    Spawn(first, ZombieKind.Normal, 3); first.ReceiveDamage(99); until = EditorApplication.timeSinceStartup + .19; step = 6; return;
                }
                if (step == 6)
                {
                    Check(!Pop(first).enabled, "A reused actor keeps ordinary zombie deaths free of party effects");
                    children = first.transform.childCount;
                    for (int i = 0; i < 20; i++) { first.Despawn(); Spawn(first, i % 2 == 0 ? ZombieKind.PartyBalloon : ZombieKind.PartyHorn, i % 2 == 0 ? 3 : -3); first.ReceiveDamage(99); }
                    Check(first.transform.childCount == children, "Twenty repeated deaths reuse one persistent renderer without adding objects");
                    until = EditorApplication.timeSinceStartup + .19; step = 7; return;
                }
                if (step == 7)
                {
                    Check(Pop(first).enabled && !first.GetComponent<BeltActorVisual>().Body.flipX, "Right-facing reused actor presents its pop correctly"); Capture("right-pop");
                    first.gameObject.SetActive(false); Check(!Pop(first).enabled, "Disabling the actor immediately resets its effect");
                    Spawn(first, ZombieKind.PartyBalloon, 3); first.ReceiveDamage(99); first.Despawn();
                    Check(!Pop(first).enabled && Pop(first).sprite == null, "Despawn also cancels a pop before its delayed first frame");
                    Spawn(first, ZombieKind.PartyHorn, 3); first.ReceiveDamage(99); until = EditorApplication.timeSinceStartup + 1.65; step = 8; return;
                }
                if (step == 8)
                {
                    Check(!first.gameObject.activeSelf && !Pop(first).enabled, "Authored death, final hold and fade return the actor to its pool");
                    Check(errors.Count == 0, "No runtime errors, exceptions or assertions"); Finish(null);
                }
            }
            catch (Exception error) { Finish(error.ToString()); }
        }
        private static void Finish(string failure)
        {
            if (owner != null && owner.IsPaused) owner.TogglePause(); Time.timeScale = 1;
            bool success = failure == null;
            Directory.CreateDirectory("Docs/Validation");
            File.WriteAllText("Docs/Validation/PartySlime-validation.md", "# Party Slime validation\n\nUnity " + Application.unityVersion + " · " + SystemInfo.graphicsDeviceType + " · " + (success ? "PASS" : "FAIL") + " · " + checks.Count + " checks\n\n" +
                string.Join("\n", checks.Select(c => "- PASS: " + c)) + (success ? "" : "\n\nFAIL: " + failure) + "\n\n" + string.Join("\n", errors) + "\n\nEditor environment messages (separate from runtime):\n" + string.Join("\n", environmentErrors));
            SessionState.SetBool(Key + ".Done", true); SessionState.SetInt(Key + ".Code", success ? 0 : 1);
            Debug.Log("Party Slime validation: " + (success ? "PASS" : "FAIL") + " (" + checks.Count + ") " + failure);
            EditorApplication.ExitPlaymode();
        }
    }
}
