using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace HunterGrandma.Editor
{
    // Optional editor-only template. Install in Assets/.../Editor when Unity import is requested.
    internal static class PixelFxSheetImporter
    {
        [Serializable] internal sealed class Frame
        {
            public string file = "";
            public double duration = 0d;
        }

        [Serializable] internal sealed class Manifest
        {
            public int schemaVersion = 0;
            public string name = "";
            public float fps = 0f;
            public bool loop = false;
            public int width = 0, height = 0;
            public float pixelsPerUnit = 0f;
            public Vector2 pivot = Vector2.zero;
            public Frame[] frames = Array.Empty<Frame>();
        }

        [MenuItem("Assets/Hunter Grandma/Import PixelFX Manifest")]
        private static void ImportSelected()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            AnimationClip clip = Import(path);
            Selection.activeObject = clip;
            EditorGUIUtility.PingObject(clip);
            Debug.Log("PixelFX imported: " + AssetDatabase.GetAssetPath(clip));
        }

        [MenuItem("Assets/Hunter Grandma/Import PixelFX Manifest", true)]
        private static bool CanImportSelected() => AssetDatabase.GetAssetPath(Selection.activeObject)
            .EndsWith(".pixelfx.json", StringComparison.OrdinalIgnoreCase);

        internal static AnimationClip Import(string manifestPath)
        {
            manifestPath = manifestPath.Replace('\\', '/');
            if (!manifestPath.StartsWith("Assets/", StringComparison.Ordinal) || !manifestPath.EndsWith(".pixelfx.json", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("Select an exported .pixelfx.json inside Assets.");
            string assetsRoot = Path.GetFullPath(Application.dataPath) + Path.DirectorySeparatorChar;
            string fullPath = Path.GetFullPath(manifestPath);
            if (!fullPath.StartsWith(assetsRoot, StringComparison.OrdinalIgnoreCase)) throw new ArgumentException("Manifest escapes Assets.");
            Manifest manifest = JsonUtility.FromJson<Manifest>(File.ReadAllText(fullPath));
            if (manifest == null || manifest.schemaVersion != 1 || manifest.frames == null || manifest.frames.Length == 0 ||
                manifest.width <= 0 || manifest.height <= 0 || manifest.pixelsPerUnit <= 0 || manifest.fps < 1 || manifest.fps > 120 ||
                !System.Text.RegularExpressions.Regex.IsMatch(manifest.name, "^[a-z0-9][a-z0-9-]*$") ||
                manifest.pivot.x < 0 || manifest.pivot.x > 1 || manifest.pivot.y < 0 || manifest.pivot.y > 1)
                throw new ArgumentException("Invalid PixelFX export manifest.");
            string folder = Path.GetDirectoryName(manifestPath).Replace('\\', '/');
            string clipPath = folder + "/" + manifest.name + ".anim";
            if (File.Exists(clipPath)) throw new IOException("Clip exists. Preserve it and import a new version folder: " + clipPath);
            var paths = new string[manifest.frames.Length];
            string fullFolder = Path.GetFullPath(folder) + Path.DirectorySeparatorChar;
            for (int i = 0; i < paths.Length; i++)
            {
                Frame frame = manifest.frames[i];
                if (frame == null || string.IsNullOrEmpty(frame.file) || double.IsNaN(frame.duration) || double.IsInfinity(frame.duration) || frame.duration <= 0)
                    throw new ArgumentException("Invalid frame at index " + i);
                string framePath = Path.GetFullPath(Path.Combine(folder, frame.file));
                if (!framePath.StartsWith(fullFolder, StringComparison.OrdinalIgnoreCase) || !File.Exists(framePath))
                    throw new ArgumentException("Frame escapes the export folder or is missing: " + frame.file);
                paths[i] = (folder + "/" + frame.file).Replace('\\', '/');
            }

            var keys = new ObjectReferenceKeyframe[paths.Length];
            double time = 0d;
            for (int i = 0; i < paths.Length; i++)
            {
                AssetDatabase.ImportAsset(paths[i], ImportAssetOptions.ForceSynchronousImport);
                var importer = AssetImporter.GetAtPath(paths[i]) as TextureImporter;
                if (importer == null) throw new ArgumentException("Frame is not a texture: " + paths[i]);
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Single;
                importer.spritePixelsPerUnit = manifest.pixelsPerUnit;
                importer.filterMode = FilterMode.Point;
                importer.mipmapEnabled = false;
                importer.textureCompression = TextureImporterCompression.Uncompressed;
                importer.npotScale = TextureImporterNPOTScale.None;
                importer.alphaIsTransparency = true;
                importer.maxTextureSize = 8192;
                var settings = new TextureImporterSettings();
                importer.ReadTextureSettings(settings);
                settings.spriteAlignment = (int)SpriteAlignment.Custom;
                settings.spritePivot = manifest.pivot;
                settings.spriteMeshType = SpriteMeshType.FullRect;
                importer.SetTextureSettings(settings);
                importer.SaveAndReimport();
                Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(paths[i]);
                if (sprite == null || sprite.rect.width != manifest.width || sprite.rect.height != manifest.height)
                    throw new ArgumentException("Frame dimensions do not match the manifest: " + paths[i]);
                keys[i] = new ObjectReferenceKeyframe { time = (float)time, value = sprite };
                time += manifest.frames[i].duration;
            }
            // Unity extends PPtr curves by one frame. An extra terminal key would extend the clip again.
            var clip = new AnimationClip { name = manifest.name, frameRate = manifest.fps, wrapMode = manifest.loop ? WrapMode.Loop : WrapMode.Once };
            AnimationUtility.SetObjectReferenceCurve(clip, EditorCurveBinding.PPtrCurve("", typeof(SpriteRenderer), "m_Sprite"), keys);
            AnimationClipSettings clipSettings = AnimationUtility.GetAnimationClipSettings(clip);
            clipSettings.loopTime = manifest.loop;
            clipSettings.startTime = 0f;
            clipSettings.stopTime = (float)time;
            AnimationUtility.SetAnimationClipSettings(clip, clipSettings);
            AssetDatabase.CreateAsset(clip, clipPath);
            AssetDatabase.SaveAssetIfDirty(clip);
            return clip;
        }
    }
}
