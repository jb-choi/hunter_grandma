using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace HunterGrandma.Editor
{
    internal static class SundeokAnimationSetup
    {
        internal const string Root = "Assets/HunterGrandma/Art/Sundeok120/";
        internal const string AssetPath = Root + "Sundeok120AnimationSet.asset";
        [Serializable] private sealed class Metadata { public Pose[] frames; }
        [Serializable] private sealed class Pose { public string action; public int local_frame, duration_ms; public int[] muzzle; }

        internal static SundeokAnimationSet Configure()
        {
            string metadataPath = Root + "pose_metadata.json";
            if (!File.Exists(metadataPath)) return null;
            Metadata metadata = JsonUtility.FromJson<Metadata>(File.ReadAllText(metadataPath));
            var clips = new SundeokAnimationClip[6];
            for (int i = 0; i < clips.Length; i++)
            {
                var frames = new List<SundeokAnimationFrame>();
                foreach (Pose pose in metadata.frames)
                {
                    if (pose.action != ((SundeokAction)i).ToString()) continue;
                    string name = pose.action.ToLowerInvariant(), leaf = name + "/sundeok_" + name + "_" + pose.local_frame.ToString("00") + ".png";
                    string bodyPath = Root + "Body/" + leaf;
                    frames.Add(new SundeokAnimationFrame {
                        composite = Import(Root + "Composite/" + leaf), body = Import(bodyPath), upper = Import(Root + "Upper/" + leaf),
                        duration = pose.duration_ms / 1000f, muzzle = new Vector2(pose.muzzle[0], pose.muzzle[1]), registration = ReadRegistration(bodyPath)
                    });
                }
                clips[i] = new SundeokAnimationClip { action = (SundeokAction)i, frames = frames.ToArray() };
            }
            SundeokAnimationSet asset = AssetDatabase.LoadAssetAtPath<SundeokAnimationSet>(AssetPath);
            if (asset == null) { asset = ScriptableObject.CreateInstance<SundeokAnimationSet>(); AssetDatabase.CreateAsset(asset, AssetPath); }
            string before = EditorJsonUtility.ToJson(asset);
            asset.SetClips(clips);
            if (before != EditorJsonUtility.ToJson(asset)) { EditorUtility.SetDirty(asset); AssetDatabase.SaveAssetIfDirty(asset); }
            return asset;
        }
        private static Sprite Import(string path)
        {
            AssetDatabase.ImportAsset(path);
            var importer = (TextureImporter)AssetImporter.GetAtPath(path);
            var settings = new TextureImporterSettings(); importer.ReadTextureSettings(settings);
            Vector2 pivot = new Vector2(.5f, 2f / 120f);
            if (importer.textureType != TextureImporterType.Sprite || importer.spritePixelsPerUnit != SundeokAnimationSet.PixelsPerUnit || importer.filterMode != FilterMode.Point || importer.mipmapEnabled || importer.textureCompression != TextureImporterCompression.Uncompressed || settings.spritePivot != pivot)
            {
                importer.textureType = TextureImporterType.Sprite; importer.spriteImportMode = SpriteImportMode.Single;
                importer.spritePixelsPerUnit = SundeokAnimationSet.PixelsPerUnit; importer.filterMode = FilterMode.Point;
                importer.mipmapEnabled = false; importer.textureCompression = TextureImporterCompression.Uncompressed;
                importer.npotScale = TextureImporterNPOTScale.None; importer.alphaIsTransparency = true;
                importer.ReadTextureSettings(settings); settings.spriteAlignment = (int)SpriteAlignment.Custom; settings.spritePivot = pivot; settings.spriteMeshType = SpriteMeshType.FullRect;
                importer.SetTextureSettings(settings); importer.SaveAndReimport();
            }
            return AssetDatabase.LoadAssetAtPath<Sprite>(path);
        }
        private static Vector2 ReadRegistration(string path)
        {
            // Same hair-center/shoulder registration used by the source's walk/aim/fire demo.
            var texture = new Texture2D(2, 2, TextureFormat.RGBA32, false); texture.LoadImage(File.ReadAllBytes(path));
            Color32[] pixels = texture.GetPixels32(); int left = 120, right = 0, bottom = 0;
            for (int y = 0; y <= 70; y++) for (int x = 0; x < 120; x++)
            {
                Color32 p = pixels[(119 - y) * 120 + x]; int rgb = p.r << 16 | p.g << 8 | p.b;
                if (p.a == 0 || (rgb != 0x57475f && rgb != 0x7d6c86 && rgb != 0xaa9ba9 && rgb != 0xd7ccc8)) continue;
                left = Mathf.Min(left, x); right = Mathf.Max(right, x); bottom = Mathf.Max(bottom, y);
            }
            UnityEngine.Object.DestroyImmediate(texture);
            return new Vector2((left + right) * .5f, bottom + 10);
        }
    }
}
