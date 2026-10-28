using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.U2D.Sprites;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HunterGrandma.Editor
{
    [InitializeOnLoad]
    public static class GrandmaProjectSetup
    {
        public const string Root = "Assets/HunterGrandma";
        public const string ScenePath = Root + "/Scenes/GrandmaDefense.unity";
        private static double nextCommandCheck;

        static GrandmaProjectSetup()
        {
            EditorApplication.delayCall += EnsureFirstScene;
            EditorApplication.update += CheckCommand;
        }

        private static void EnsureFirstScene()
        {
            if (Application.isBatchMode || EditorApplication.isPlayingOrWillChangePlaymode || File.Exists(ScenePath)) return;
            if (!File.Exists(Root + "/Art/Source/zombie_walk_generated.png")) return;
            CreateScene();
        }

        // Local command bridge for reproducible editor validation without a Unity MCP dependency.
        private static void CheckCommand()
        {
            if (EditorApplication.timeSinceStartup < nextCommandCheck || EditorApplication.isCompiling || EditorApplication.isUpdating) return;
            nextCommandCheck = EditorApplication.timeSinceStartup + 0.5;
            const string path = "Library/HunterGrandmaCommand.txt";
            if (!File.Exists(path)) return;
            string command = File.ReadAllText(path).Trim();
            File.Delete(path);
            if (command == "validate" || command == "validate-map" || command == "validate-stage" || command=="validate-belt") GrandmaStageValidation.Begin();
            else if (command == "validate-ui") GrandmaUiValidation.Begin();
            else if (command == "validate-opening") GrandmaOpeningValidation.Begin();
            else if (command == "configure-stage") GrandmaStageSetup.Configure();
            else if (command == "open" && !EditorApplication.isPlayingOrWillChangePlaymode) EditorSceneManager.OpenScene(ScenePath);
            else if (command == "play" && !EditorApplication.isPlayingOrWillChangePlaymode)
            {
                EditorSceneManager.OpenScene(ScenePath);
                EditorApplication.EnterPlaymode();
            }
            else if (command == "stop") EditorApplication.ExitPlaymode();
        }

        [MenuItem("Tools/Hunter Grandma/Create Defense Scene")]
        public static void CreateScene()
        {
            if (File.Exists(ScenePath))
            {
                Debug.Log("GrandmaDefense scene already exists; existing work was preserved.");
                return;
            }
            Directory.CreateDirectory(Root + "/Art");
            Directory.CreateDirectory(Root + "/Prefabs");
            Directory.CreateDirectory(Root + "/Scenes");
            PrepareZombieSheet();
            PrepareBackdrop();
            string pixelPath = Root + "/Art/pixel.png";
            Texture2D pixel = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            pixel.SetPixel(0, 0, Color.white);
            pixel.Apply();
            File.WriteAllBytes(pixelPath, pixel.EncodeToPNG());
            UnityEngine.Object.DestroyImmediate(pixel);
            AssetDatabase.Refresh();

            Sprite idle = ImportSingle(Root + "/Art/grandma_04_sundeok_idle.png", 16f, new Vector2(0.5f, 3f / 64f));
            Sprite fire = ImportSingle(Root + "/Art/grandma_04_sundeok_fire.png", 16f, new Vector2(0.5f, 3f / 64f));
            Sprite background = ImportSingle(Root + "/Art/village_dusk.png", 16f, new Vector2(0.5f, 0.5f));
            Sprite dot = ImportSingle(pixelPath, 1f, new Vector2(0.5f, 0.5f));
            Sprite[] frames = ImportZombieFrames();

            Shader shader = Shader.Find("Universal Render Pipeline/2D/Sprite-Unlit-Default");
            if (shader == null) throw new InvalidOperationException("The project's URP 2D unlit sprite shader is missing.");
            Material material = new Material(shader) { name = "Pixel Sprite Unlit" };
            AssetDatabase.CreateAsset(material, Root + "/Art/PixelSpriteUnlit.mat");

            Scene previous = SceneManager.GetActiveScene();
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);
            SceneManager.SetActiveScene(scene);
            GameObject cameraObject = new GameObject("Main Camera", typeof(Camera), typeof(AudioListener));
            cameraObject.tag = "MainCamera";
            Camera camera = cameraObject.GetComponent<Camera>();
            camera.orthographic = true;
            camera.orthographicSize = 6.75f;
            camera.transform.position = new Vector3(0f, 0f, -10f);
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.10f, 0.12f, 0.18f);
            camera.allowHDR = false;
            camera.allowMSAA = false;
            new GameObject("Sun", typeof(Light)).GetComponent<Light>().type = LightType.Directional;
            MakeSprite("Dusk village", background, material, Vector3.zero, -20);

            GrandmaShooter grandma = MakeSprite("grandma_04_sundeok", idle, material, new Vector3(-7.4f, -2.75f, 0f), 12).AddComponent<GrandmaShooter>();
            grandma.Configure(idle, fire);
            GameObject zombieObject = MakeSprite("Zombie", frames[0], material, Vector3.zero, 10);
            ZombieActor zombie = zombieObject.AddComponent<ZombieActor>();
            zombie.Configure(frames);
            GameObject zombieAsset = PrefabUtility.SaveAsPrefabAsset(zombieObject, Root + "/Prefabs/Zombie.prefab");
            UnityEngine.Object.DestroyImmediate(zombieObject);

            GameObject shotObject = MakeSprite("Shot tracer", dot, material, Vector3.zero, 20);
            shotObject.transform.localScale = new Vector3(0.58f, 0.055f, 1f);
            shotObject.GetComponent<SpriteRenderer>().color = new Color(1f, 0.88f, 0.45f);
            shotObject.AddComponent<ShotTracer>();
            GameObject tracerAsset = PrefabUtility.SaveAsPrefabAsset(shotObject, Root + "/Prefabs/ShotTracer.prefab");
            UnityEngine.Object.DestroyImmediate(shotObject);

            GameObject encounterObject = new GameObject("Endless defense");
            GrandmaDefense encounter = encounterObject.AddComponent<GrandmaDefense>();
            ZombieActor[] initial = new ZombieActor[3];
            float[] xPositions = { 0.75f, 5f, 9.5f };
            for (int i = 0; i < initial.Length; i++)
            {
                GameObject actor = (GameObject)PrefabUtility.InstantiatePrefab(zombieAsset, scene);
                actor.name = "Zombie " + (i + 1);
                actor.transform.SetParent(encounterObject.transform, false);
                actor.transform.position = new Vector3(xPositions[i], -2.75f, 0f);
                initial[i] = actor.GetComponent<ZombieActor>();
            }
            encounter.Configure(grandma, zombieAsset.GetComponent<ZombieActor>(), tracerAsset.GetComponent<ShotTracer>(), initial);
            encounterObject.AddComponent<DefenseHud>().Configure(encounter,
                AssetDatabase.LoadAssetAtPath<Font>("Assets/Fonts/NotoSansCJKkr-Regular.otf"));
            EditorSceneManager.SaveScene(scene, ScenePath);
            // This first gameplay scene is the requested entry point; preserve all existing build scenes.
            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath, true) }
                .Concat(EditorBuildSettings.scenes.Where(s => s.path != ScenePath)).ToArray();
            AssetDatabase.SaveAssets();
            GrandmaMapSetup.ConfigureScene();
            GrandmaStageSetup.Configure();
            if (previous.IsValid() && previous.name == "SampleScene" && !previous.isDirty)
                EditorSceneManager.CloseScene(previous, true);
            else
            {
                if (previous.IsValid()) SceneManager.SetActiveScene(previous);
                EditorSceneManager.CloseScene(scene, true);
            }
            Debug.Log("Hunter Grandma: defense scene and prefabs created. Open GrandmaDefense and press Play.");
        }

        private static GameObject MakeSprite(string name, Sprite sprite, Material material, Vector3 position, int order)
        {
            GameObject obj = new GameObject(name, typeof(SpriteRenderer));
            obj.transform.position = position;
            SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.sharedMaterial = material;
            renderer.sortingOrder = order;
            return obj;
        }

        private static TextureImporter PixelImporter(string path, float ppu)
        {
            AssetDatabase.ImportAsset(path);
            TextureImporter importer = (TextureImporter)AssetImporter.GetAtPath(path);
            importer.textureType = TextureImporterType.Sprite;
            importer.spritePixelsPerUnit = ppu;
            importer.filterMode = FilterMode.Point;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.mipmapEnabled = false;
            importer.alphaIsTransparency = true;
            importer.npotScale = TextureImporterNPOTScale.None;
            TextureImporterSettings settings = new TextureImporterSettings();
            importer.ReadTextureSettings(settings);
            settings.spriteMeshType = SpriteMeshType.FullRect;
            importer.SetTextureSettings(settings);
            return importer;
        }

        private static Sprite ImportSingle(string path, float ppu, Vector2 pivot)
        {
            TextureImporter importer = PixelImporter(path, ppu);
            importer.spriteImportMode = SpriteImportMode.Single;
            TextureImporterSettings settings = new TextureImporterSettings();
            importer.ReadTextureSettings(settings);
            settings.spriteAlignment = (int)SpriteAlignment.Custom;
            settings.spritePivot = pivot;
            importer.SetTextureSettings(settings);
            importer.SaveAndReimport();
            return AssetDatabase.LoadAssetAtPath<Sprite>(path);
        }

        private static Sprite[] ImportZombieFrames()
        {
            const string path = Root + "/Art/zombie_walk_64.png";
            TextureImporter importer = PixelImporter(path, 16f);
            importer.spriteImportMode = SpriteImportMode.Multiple;
            importer.SaveAndReimport();
            var factories = new SpriteDataProviderFactories();
            factories.Init();
            ISpriteEditorDataProvider provider = factories.GetSpriteEditorDataProviderFromObject(importer);
            provider.InitSpriteEditorDataProvider();
            SpriteRect[] rects = Enumerable.Range(0, 3).Select(i => new SpriteRect
            {
                name = "zombie_walk_" + i,
                spriteID = GUID.Generate(),
                rect = new Rect(i * 64, 0, 64, 64),
                alignment = SpriteAlignment.Custom,
                pivot = new Vector2(0.5f, 3f / 64f)
            }).ToArray();
            provider.SetSpriteRects(rects);
            provider.GetDataProvider<ISpriteNameFileIdDataProvider>().SetNameFileIdPairs(rects.Select(r => new SpriteNameFileIdPair(r.name, r.spriteID)));
            provider.Apply();
            importer.SaveAndReimport();
            return AssetDatabase.LoadAllAssetsAtPath(path).OfType<Sprite>().OrderBy(s => s.name).ToArray();
        }

        private static void PrepareZombieSheet()
        {
            Texture2D source = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            source.LoadImage(File.ReadAllBytes(Root + "/Art/Source/zombie_walk_generated.png"));
            int cellWidth = source.width / 3;
            RectInt[] bounds = new RectInt[3];
            for (int frame = 0; frame < 3; frame++)
            {
                int minX = cellWidth, minY = source.height, maxX = 0, maxY = 0;
                for (int y = 0; y < source.height; y++)
                for (int x = 0; x < cellWidth; x++)
                {
                    if (source.GetPixel(frame * cellWidth + x, y).a < 0.7f) continue;
                    minX = Mathf.Min(minX, x); maxX = Mathf.Max(maxX, x);
                    minY = Mathf.Min(minY, y); maxY = Mathf.Max(maxY, y);
                }
                bounds[frame] = new RectInt(minX, minY, maxX - minX + 1, maxY - minY + 1);
            }
            float scale = 58f / bounds.Max(b => b.height);
            Texture2D output = new Texture2D(192, 64, TextureFormat.RGBA32, false);
            output.SetPixels(new Color[192 * 64]);
            for (int frame = 0; frame < 3; frame++)
            {
                RectInt b = bounds[frame];
                int width = Mathf.RoundToInt(b.width * scale);
                int height = Mathf.RoundToInt(b.height * scale);
                int offsetX = frame * 64 + (64 - width) / 2;
                for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    int sx = frame * cellWidth + b.x + Mathf.Min(b.width - 1, Mathf.FloorToInt((x + 0.5f) / scale));
                    int sy = b.y + Mathf.Min(b.height - 1, Mathf.FloorToInt((y + 0.5f) / scale));
                    Color c = source.GetPixel(sx, sy);
                    if (c.a < 0.7f) continue;
                    c.a = 1f;
                    output.SetPixel(offsetX + x, y + 3, c);
                }
            }
            output.Apply();
            File.WriteAllBytes(Root + "/Art/zombie_walk_64.png", output.EncodeToPNG());
            UnityEngine.Object.DestroyImmediate(source);
            UnityEngine.Object.DestroyImmediate(output);
        }

        private static void PrepareBackdrop()
        {
            const int width = 384, height = 216;
            Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            Color[] pixels = new Color[width * height];
            Color skyTop = Hex("333d55"), skyBottom = Hex("bb8570");
            for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                pixels[y * width + x] = Color.Lerp(skyBottom, skyTop, Mathf.Clamp01((y - 78f) / 130f));
            void Rect(int x, int y, int w, int h, Color c)
            {
                for (int py = Mathf.Max(0, y); py < Mathf.Min(height, y + h); py++)
                for (int px = Mathf.Max(0, x); px < Mathf.Min(width, x + w); px++) pixels[py * width + px] = c;
            }
            void Disc(int cx, int cy, int radius, Color c)
            {
                for (int y = -radius; y <= radius; y++)
                for (int x = -radius; x <= radius; x++) if (x * x + y * y < radius * radius) Rect(cx + x, cy + y, 1, 1, c);
            }
            Disc(275, 145, 18, Hex("d9ad80"));
            for (int x = 0; x < width; x++)
            {
                int ridge = 111 + Mathf.RoundToInt(Mathf.Sin(x * 0.035f) * 13f + Mathf.Sin(x * 0.079f) * 7f);
                Rect(x, 67, 1, ridge - 67, Hex("646478"));
                int near = 91 + Mathf.RoundToInt(Mathf.Sin(x * 0.052f + 1f) * 10f);
                Rect(x, 62, 1, near - 62, Hex("444f5b"));
            }
            var random = new System.Random(40);
            for (int i = 0; i < 27; i++)
            {
                int x = random.Next(-8, width + 8), h = random.Next(24, 48);
                Rect(x, 64, 3, h, Hex("333f49"));
                for (int y = 0; y < h; y++)
                {
                    int half = (h - y) / 3;
                    Rect(x - half, 77 + y, half * 2 + 3, 1, Hex("3b4851"));
                }
            }
            // Abandoned village wall, a warm porch and a wooden boundary fence.
            Rect(0, 64, 56, 52, Hex("594f51"));
            Rect(2, 68, 51, 43, Hex("72625d"));
            for (int y = 70; y < 110; y += 7) Rect(2, y, 51, 1, Hex("665957"));
            for (int y = 0; y < 20; y++) Rect(-8 + y, 116 + y, 75 - y * 2, 1, Hex("343c48"));
            Rect(12, 81, 17, 25, Hex("343b42"));
            Rect(14, 83, 13, 21, Hex("bd925e"));
            Rect(20, 83, 2, 21, Hex("63554d"));
            Rect(14, 92, 13, 2, Hex("63554d"));
            Rect(38, 64, 12, 34, Hex("3e4143"));
            for (int x = 91; x < width; x += 17)
            {
                Rect(x, 63, 3, 21, Hex("55514f"));
                Rect(x + 1, 84, 1, 3, Hex("55514f"));
            }
            Rect(91, 76, width, 3, Hex("635b53"));
            Rect(91, 68, width, 3, Hex("635b53"));
            Rect(0, 0, width, 64, Hex("393938"));
            Rect(0, 62, width, 2, Hex("9b8970"));
            Rect(0, 58, width, 4, Hex("706452"));
            Rect(0, 53, width, 5, Hex("4c4840"));
            for (int i = 0; i < 230; i++)
            {
                int x = random.Next(width), y = random.Next(21, 56);
                Rect(x, y, random.Next(1, 5), 1, i % 3 == 0 ? Hex("605746") : Hex("474540"));
            }
            for (int i = 0; i < 45; i++)
            {
                int x = random.Next(width);
                Rect(x, 64, 1, random.Next(1, 4), Hex("72714c"));
            }
            texture.SetPixels(pixels);
            texture.Apply();
            File.WriteAllBytes(Root + "/Art/village_dusk.png", texture.EncodeToPNG());
            UnityEngine.Object.DestroyImmediate(texture);
        }

        private static Color Hex(string value)
        {
            ColorUtility.TryParseHtmlString("#" + value, out Color color);
            return color;
        }
    }
}
