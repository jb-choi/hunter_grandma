using System.IO;
using UnityEditor;
using UnityEngine;

namespace HunterGrandma.Editor
{
    internal static class ZombieAnimationSetup
    {
        internal const string Root = "Assets/HunterGrandma/Art/Zombies120/";
        internal const string AssetPath = Root + "Zombie120AnimationSet.asset";
        internal static ZombieAnimationSet Configure()
        {
            if (!File.Exists(Root + "Infected/frame_01.png")) return null;
            var variants = new ZombieAnimationVariant[5];
            int[] counts = { 4, 6, 6, 6, 6 };
            for (int k = 0; k < 3; k++)
            {
                var clips = new ZombieAnimationClip[5]; int frame = 1;
                for (int a = 0; a < clips.Length; a++)
                {
                    var images = new Sprite[counts[a]]; var durations = new float[counts[a]];
                    for (int f = 0; f < images.Length; f++, frame++)
                    {
                        images[f] = Import(Root + ((ZombieKind)k) + "/frame_" + frame.ToString("00") + ".png");
                        durations[f] = a == 0 ? (k == 2 ? .23f : .20f) : a == 1 ? (k == 2 ? .15f : .13f) : a == 2 ? (k == 2 ? .09f : .08f) : a == 3 ? new[] { .12f, .09f, .07f, .08f, .10f, .14f }[f] : new[] { .09f, .10f, .12f, .14f, .17f, .40f }[f];
                    }
                    clips[a] = new ZombieAnimationClip { frames = images, durations = durations };
                }
                variants[k] = new ZombieAnimationVariant { kind = (ZombieKind)k, clips = clips };
            }
            for (int k = 3; k < variants.Length; k++)
                variants[k] = PartySlimeSetup.Variant((ZombieKind)k, k == 3 ? "Z01" : "Z02");
            var asset = AssetDatabase.LoadAssetAtPath<ZombieAnimationSet>(AssetPath);
            if (asset == null) { asset = ScriptableObject.CreateInstance<ZombieAnimationSet>(); AssetDatabase.CreateAsset(asset, AssetPath); }
            string before = EditorJsonUtility.ToJson(asset); asset.SetVariants(variants);
            asset.SetPartyPop(PartySlimeSetup.ImportPop());
            if (before != EditorJsonUtility.ToJson(asset)) { EditorUtility.SetDirty(asset); AssetDatabase.SaveAssetIfDirty(asset); }
            BindPoolPrefab(asset);
            return asset;
        }
        private static void BindPoolPrefab(ZombieAnimationSet asset)
        {
            // Pool expansion instantiates the prefab, not the three scene overrides.
            const string path = "Assets/HunterGrandma/Prefabs/Zombie.prefab";
            var contents = PrefabUtility.LoadPrefabContents(path);
            try
            {
                var actor = new SerializedObject(contents.GetComponent<ZombieActor>());
                var reference = actor.FindProperty("animationSet");
                if (reference.objectReferenceValue == asset) return;
                reference.objectReferenceValue = asset; actor.ApplyModifiedPropertiesWithoutUndo();
                var renderer = contents.GetComponent<SpriteRenderer>();
                renderer.sprite = asset.Clip(ZombieKind.Infected, ZombieAction.Idle).frames[0]; renderer.flipX = true;
                PrefabUtility.SaveAsPrefabAsset(contents, path);
            }
            finally { PrefabUtility.UnloadPrefabContents(contents); }
        }
        private static Sprite Import(string path)
        {
            AssetDatabase.ImportAsset(path);
            var importer = (TextureImporter)AssetImporter.GetAtPath(path);
            var settings = new TextureImporterSettings(); importer.ReadTextureSettings(settings);
            Vector2 pivot = new Vector2(52f / 120f, 2f / 120f);
            if (importer.textureType != TextureImporterType.Sprite || importer.spritePixelsPerUnit != ZombieAnimationSet.PixelsPerUnit || importer.filterMode != FilterMode.Point || importer.mipmapEnabled || importer.textureCompression != TextureImporterCompression.Uncompressed || settings.spritePivot != pivot)
            {
                importer.textureType = TextureImporterType.Sprite; importer.spriteImportMode = SpriteImportMode.Single;
                importer.spritePixelsPerUnit = ZombieAnimationSet.PixelsPerUnit; importer.filterMode = FilterMode.Point;
                importer.mipmapEnabled = false; importer.textureCompression = TextureImporterCompression.Uncompressed;
                importer.npotScale = TextureImporterNPOTScale.None; importer.alphaIsTransparency = true;
                importer.ReadTextureSettings(settings); settings.spriteAlignment = (int)SpriteAlignment.Custom; settings.spritePivot = pivot; settings.spriteMeshType = SpriteMeshType.FullRect;
                importer.SetTextureSettings(settings); importer.SaveAndReimport();
            }
            return AssetDatabase.LoadAssetAtPath<Sprite>(path);
        }
    }
}
