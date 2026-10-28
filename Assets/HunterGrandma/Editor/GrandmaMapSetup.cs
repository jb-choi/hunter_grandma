using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HunterGrandma.Editor
{
    [InitializeOnLoad]
    public static class GrandmaMapSetup
    {
        public const string MapPath = GrandmaProjectSetup.Root + "/Art/Maps/ruined_city_map.png";

        static GrandmaMapSetup() { EditorApplication.delayCall += ConfigureScene; }

        [MenuItem("Tools/Hunter Grandma/Configure City Map")]
        public static void ConfigureScene()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode || !File.Exists(MapPath) || !File.Exists(GrandmaProjectSetup.ScenePath)) return;
            AssetDatabase.ImportAsset(MapPath);
            TextureImporter importer = AssetImporter.GetAtPath(MapPath) as TextureImporter;
            if (importer == null) return;
            if (importer.textureType != TextureImporterType.Sprite || importer.spriteImportMode != SpriteImportMode.Single ||
                importer.filterMode != FilterMode.Point || importer.textureCompression != TextureImporterCompression.Uncompressed || importer.mipmapEnabled)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Single;
                importer.filterMode = FilterMode.Point;
                importer.textureCompression = TextureImporterCompression.Uncompressed;
                importer.mipmapEnabled = false;
                importer.npotScale = TextureImporterNPOTScale.None;
                importer.SaveAndReimport();
            }
            Sprite map = AssetDatabase.LoadAssetAtPath<Sprite>(MapPath);
            if (map == null) return;
            Scene scene = SceneManager.GetSceneByPath(GrandmaProjectSetup.ScenePath);
            bool openedForSetup = !scene.isLoaded;
            if (openedForSetup) scene = EditorSceneManager.OpenScene(GrandmaProjectSetup.ScenePath, OpenSceneMode.Additive);
            bool wasDirty = scene.isDirty;
            bool changed = false;
            foreach (GameObject root in scene.GetRootGameObjects())
            foreach (DefenseHud hud in root.GetComponentsInChildren<DefenseHud>(true))
            {
                SerializedObject serialized = new SerializedObject(hud);
                SerializedProperty property = serialized.FindProperty("cityMap");
                if (property.objectReferenceValue == map) continue;
                property.objectReferenceValue = map;
                serialized.ApplyModifiedProperties();
                EditorUtility.SetDirty(hud);
                changed = true;
            }
            if (changed)
            {
                EditorSceneManager.MarkSceneDirty(scene);
                if (!wasDirty || openedForSetup) EditorSceneManager.SaveScene(scene);
                Debug.Log("Hunter Grandma: city map connected to the defense HUD.");
            }
            if (openedForSetup) EditorSceneManager.CloseScene(scene, true);
        }
    }
}
