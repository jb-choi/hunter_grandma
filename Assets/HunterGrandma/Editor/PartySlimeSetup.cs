using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace HunterGrandma.Editor
{
    internal static class PartySlimeSetup
    {
        internal const string Root = "Assets/HunterGrandma/Art/PartySlime/";
        internal const string PopRoot = "Assets/HunterGrandma/Art/VFX/PartyPop/v002/";
        [Serializable] private sealed class Manifest { public Vector2 pivot; public float pixelsPerUnit; public Frame[] frames; }
        [Serializable] private sealed class Frame { public string file; public float duration; }

        // All ten characters are import-ready, with only Z01/Z02 used by stages.
        [MenuItem("Tools/Hunter Grandma/Import Party Slime Bundle")]
        public static void Configure()
        {
            for (int i = 1; i <= 10; i++)
                for (int f = 1; f <= 16; f++) Import(Root + "Z" + i.ToString("00") + "/frame_" + f.ToString("00") + ".png", new Vector2(.5f, .05f));
            ZombieAnimationSetup.Configure();
        }
        internal static ZombieAnimationVariant Variant(ZombieKind kind, string id)
        {
            var clips = new ZombieAnimationClip[5];
            int[] rows = { 0, 1, 1, 2, 3 };
            for (int a = 0; a < clips.Length; a++)
            {
                var frames = new Sprite[4]; var durations = new float[4];
                for (int f = 0; f < 4; f++)
                {
                    frames[f] = Import(Root + id + "/frame_" + (rows[a] * 4 + f + 1).ToString("00") + ".png", new Vector2(.5f, .05f));
                    durations[f] = a == 0 ? .2f : a == 1 ? .13f : a == 2 ? .08f : a == 3 ? .15f : new[] { .09f, .12f, .16f, .35f }[f];
                }
                clips[a] = new ZombieAnimationClip { frames = frames, durations = durations };
            }
            return new ZombieAnimationVariant { kind = kind, clips = clips };
        }
        internal static ZombieAnimationClip ImportPop()
        {
            var manifest = JsonUtility.FromJson<Manifest>(File.ReadAllText(PopRoot + "party-pop.pixelfx.json"));
            var frames = new Sprite[manifest.frames.Length]; var durations = new float[frames.Length];
            for (int f = 0; f < frames.Length; f++)
            {
                frames[f] = Import(PopRoot + manifest.frames[f].file, manifest.pivot);
                durations[f] = manifest.frames[f].duration;
            }
            return new ZombieAnimationClip { frames = frames, durations = durations };
        }
        internal static Sprite Import(string path, Vector2 pivot)
        {
            if (!File.Exists(path)) throw new FileNotFoundException("Missing party art", path);
            AssetDatabase.ImportAsset(path);
            var importer = (TextureImporter)AssetImporter.GetAtPath(path);
            var settings = new TextureImporterSettings(); importer.ReadTextureSettings(settings);
            if (importer.textureType != TextureImporterType.Sprite || importer.spriteImportMode != SpriteImportMode.Single ||
                importer.spritePixelsPerUnit != 32 || importer.filterMode != FilterMode.Point || importer.mipmapEnabled ||
                importer.textureCompression != TextureImporterCompression.Uncompressed || settings.spritePivot != pivot ||
                settings.spriteMeshType != SpriteMeshType.FullRect || settings.spriteAlignment != (int)SpriteAlignment.Custom)
            {
                importer.textureType = TextureImporterType.Sprite; importer.spriteImportMode = SpriteImportMode.Single;
                importer.spritePixelsPerUnit = 32; importer.filterMode = FilterMode.Point; importer.mipmapEnabled = false;
                importer.textureCompression = TextureImporterCompression.Uncompressed; importer.npotScale = TextureImporterNPOTScale.None;
                importer.alphaIsTransparency = true; importer.ReadTextureSettings(settings);
                settings.spriteAlignment = (int)SpriteAlignment.Custom; settings.spritePivot = pivot; settings.spriteMeshType = SpriteMeshType.FullRect;
                importer.SetTextureSettings(settings); importer.SaveAndReimport();
            }
            return AssetDatabase.LoadAssetAtPath<Sprite>(path);
        }
    }
}
