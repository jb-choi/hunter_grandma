using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace HunterGrandma.Editor
{
    internal static class ZombieRosterValidation
    {
        public static void RunAssetBatch()
        {
            if (!Application.isBatchMode) throw new InvalidOperationException("RunAssetBatch requires -batchmode.");
            var results = new List<string>();
            try
            {
                GrandmaStageSetup.Configure();
                ModelChecks((valid, message) => { if (!valid) throw new Exception(message); results.Add(message); });
                var scene = EditorSceneManager.OpenScene(GrandmaProjectSetup.ScenePath);
                var actors = scene.GetRootGameObjects().SelectMany(r => r.GetComponentsInChildren<ZombieActor>(true)).ToArray();
                if (actors.Length != 3 || actors.Any(a => a.AnimationSet == null || !a.AnimationSet.IsReady || a.GetComponent<SpriteRenderer>().sprite == null)) throw new Exception("Scene zombie asset references are incomplete.");
                results.Add("All three scene zombie instances resolve the imported animation set and source sprite after GUID reconciliation");
                File.WriteAllText("Docs/Validation/ZombieCute-asset-validation.md", "# Zombie asset reference validation\n\nUnity " + Application.unityVersion + " · Status: PASS\n\n" + string.Join("\n", results.Select(r => "- PASS: " + r)) + "\n");
                Debug.Log("Zombie assets validation: PASS (" + results.Count + " checks)"); EditorApplication.Exit(0);
            }
            catch (Exception error) { Debug.LogException(error); EditorApplication.Exit(1); }
        }
        internal static void ModelChecks(Action<bool, string> check)
        {
            var art = AssetDatabase.LoadAssetAtPath<ZombieAnimationSet>(ZombieAnimationSetup.AssetPath);
            check(art != null && art.IsReady, "Three authored zombie variants are assigned with every required action");
            int total = 0;
            foreach (ZombieKind kind in Enum.GetValues(typeof(ZombieKind)))
            {
                int frames = 0; bool imported = true;
                foreach (ZombieAction action in Enum.GetValues(typeof(ZombieAction)))
                    foreach (Sprite image in art.Clip(kind, action).frames)
                    {
                        frames++;
                        var texture = (TextureImporter)AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(image));
                        imported &= image.rect.size == new Vector2(120, 120) && Vector2.Distance(image.pivot, new Vector2(52, 2)) < .01f && image.pixelsPerUnit == 32 && texture.filterMode == FilterMode.Point && !texture.mipmapEnabled;
                    }
                check(frames == 28 && imported, kind + ": 28 authored poses preserve full canvas, foot pivot, point filtering and 32 PPU"); total += frames;
                var attack = art.Clip(kind, ZombieAction.Attack); var dead = art.Clip(kind, ZombieAction.Dead);
                check(art.Attack(kind, 0) == attack.frames[0] && art.Attack(kind, .44f) == attack.frames[3] && art.Attack(kind, .85f) == attack.frames[5], kind + ": authored attack follows windup, existing damage beat and recovery");
                check(dead.Sample(10, false) == dead.frames[5] && Mathf.Abs(dead.Duration - 1.02f) < .001f, kind + ": one-shot death holds the final authored pose");
            }
            check(total == 84, "All 84 exported poses are linked without substituting legacy zombie art");
            float[] previous = { 1.7f, 1.35f, 1.25f, 1.4f, 1.35f, 1.2f, 1.25f };
            for (int id = 0; id < DefenseStages.Count; id++)
            {
                DefenseStage stage = DefenseStages.Get(id); int normal = 0, infected = 0, large = 0;
                for (int i = 0; i < 20; i++)
                { ZombieKind kind = stage.EnemyKind(i, 1); if (kind == ZombieKind.Normal) normal++; else if (kind == ZombieKind.Infected) infected++; else large++; }
                check(id == 0 ? infected == 20 : id < 5 ? normal > 0 && infected > 0 && large == 0 : normal > 0 && infected > 0 && large > 0, "Stage " + id + ": roster matches the requested early/late-stage enemy types");
                if (id > 0 && id < previous.Length) check(stage.SpawnInterval < previous[id], "Stage " + id + ": timed enemies arrive more frequently than the previous version");
            }
            var old = new DefenseSaveData { gold = 1234, attackLevel = 7, fireRateLevel = 5, shadowSkillEnabled = true };
            for (int id = 0; id < 7; id++) old.stages.Add(new DistrictProgress { stageId = id, clearCount = 2, firstClearAt = "2026-10-30T00:00:00Z" });
            DefenseProgress migrated = DefenseProgress.FromJson(JsonUtility.ToJson(old));
            check(migrated.Gold == 1234 && migrated.AttackLevel == 7 && migrated.FireRateLevel == 5 && migrated.ShadowSkillEnabled && migrated.Stage(6).clearCount == 2 && migrated.Stage(0).firstClearAt == old.stages[0].firstClearAt, "Existing seven-entry v1 save migrates without losing gold, upgrades, preference or clear history");
            check(migrated.IsUnlocked(7) && migrated.Stage(7).clearCount == 0 && !migrated.IsUnlocked(8), "Stage 6 clear unlocks appended Stage 7; nonexistent Stage 8 is rejected");
            old.stages[6].clearCount = 0;
            check(!DefenseProgress.FromJson(JsonUtility.ToJson(old)).IsUnlocked(7), "Uncleared Stage 6 keeps new Stage 7 locked after old-save migration");
            old.stages.RemoveAt(6); bool rejected = false;
            try { DefenseProgress.FromJson(JsonUtility.ToJson(old)); } catch (FormatException) { rejected = true; }
            check(rejected, "Malformed six-entry saves remain rejected rather than silently resetting progress");
            check(DefenseRegions.Count == 7 && DefenseRegions.Get(6).Name == DefenseStages.Get(7).Name, "Stage 7 has its own selectable city-map district");
        }
    }
}
