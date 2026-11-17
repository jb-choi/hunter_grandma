using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace HunterGrandma.Editor
{
    internal static class PixelFxSkillValidation
    {
        [Serializable] private sealed class Report
        {
            public bool passed;
            public string unity;
            public string failure;
            public string[] checks;
        }
        private static readonly List<string> checks = new List<string>();
        private static void Check(bool condition, string message)
        {
            if (!condition) throw new Exception(message);
            checks.Add(message);
        }
        public static void RunBatch()
        {
            var report = new Report { unity = Application.unityVersion };
            GameObject preview = null;
            try
            {
                const string folder = "Assets/PixelFXSkillValidation/impact";
                const string manifestPath = folder + "/impact-example.pixelfx.json";
                var manifest = JsonUtility.FromJson<PixelFxSheetImporter.Manifest>(File.ReadAllText(manifestPath));
                AnimationClip clip = PixelFxSheetImporter.Import(manifestPath);
                Check(clip != null, "Actual Unity asset import creates an AnimationClip");
                Check(manifest.frames.Length == 28, "All 28 exported frames are present in order");
                var binding = EditorCurveBinding.PPtrCurve("", typeof(SpriteRenderer), "m_Sprite");
                ObjectReferenceKeyframe[] keys = AnimationUtility.GetObjectReferenceCurve(clip, binding);
                Check(keys.Length == manifest.frames.Length, "Clip holds the full final frame duration");
                Check(Mathf.Abs(clip.length - 28f / 30f) < .0001f && Mathf.Abs(clip.frameRate - 30f) < .001f, "Clip length and frame rate match exact export timing");
                Check(!AnimationUtility.GetAnimationClipSettings(clip).loopTime, "One-shot effect is not silently looped");
                for (int i = 0; i < manifest.frames.Length; i++)
                {
                    var sprite = keys[i].value as Sprite;
                    Check(sprite != null && AssetDatabase.GetAssetPath(sprite) == folder + "/" + manifest.frames[i].file, "Frame " + i + ": clip references the correct exported image");
                    Check(sprite.rect.width == 128 && sprite.rect.height == 88, "Frame " + i + ": untrimmed frame dimensions are preserved");
                    Check(Mathf.Abs(sprite.pixelsPerUnit - 32) < .001f && (sprite.pivot - new Vector2(64, 16)).sqrMagnitude < .001f, "Frame " + i + ": PPU and top-left-to-Unity pivot conversion match");
                    var importer = (TextureImporter)AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(sprite));
                    Check(importer.filterMode == FilterMode.Point && !importer.mipmapEnabled && importer.textureCompression == TextureImporterCompression.Uncompressed, "Frame " + i + ": point/no-mip/uncompressed settings are applied");
                    Check(Mathf.Abs(keys[i].time - i / 30f) < .0001f, "Frame " + i + ": timestamp is ordered and exact");
                }
                preview = new GameObject("PixelFX validation preview", typeof(SpriteRenderer), typeof(Animator));
                var renderer = preview.GetComponent<SpriteRenderer>();
                clip.SampleAnimation(preview, 8.5f / 30f);
                Check(renderer.sprite == keys[8].value, "Actual Unity clip sampling displays the expected middle frame");
                clip.SampleAnimation(preview, clip.length - .001f);
                Check(renderer.sprite == keys[27].value, "Actual Unity clip sampling retains the final frame");
                string guid = AssetDatabase.AssetPathToGUID(folder + "/impact-example.anim");
                bool refused = false;
                try { PixelFxSheetImporter.Import(manifestPath); }
                catch (IOException) { refused = true; }
                Check(refused && guid == AssetDatabase.AssetPathToGUID(folder + "/impact-example.anim"), "Re-import refuses an existing clip and preserves its GUID");
                manifest.name = "invalid-path";
                manifest.frames[0].file = "../../outside.png";
                string invalidPath = folder + "/invalid.pixelfx.json";
                File.WriteAllText(invalidPath, JsonUtility.ToJson(manifest));
                refused = false;
                try { PixelFxSheetImporter.Import(invalidPath); }
                catch (ArgumentException) { refused = true; }
                Check(refused && !File.Exists(folder + "/invalid-path.anim"), "Manifest traversal is rejected before creating a clip");
                report.passed = true;
            }
            catch (Exception error) { report.failure = error.ToString(); Debug.LogException(error); }
            finally
            {
                if (preview != null) UnityEngine.Object.DestroyImmediate(preview);
                report.checks = checks.ToArray();
                File.WriteAllText("Library/PixelFXSkill-validation.json", JsonUtility.ToJson(report, true));
                Debug.Log("PixelFX skill Unity import validation: " + report.passed + " (" + checks.Count + " checks)");
                EditorApplication.Exit(report.passed ? 0 : 1);
            }
        }
    }
}
