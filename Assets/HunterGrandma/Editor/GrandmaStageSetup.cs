using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HunterGrandma.Editor
{
    [InitializeOnLoad]
    public static class GrandmaStageSetup
    {
        private const string Art = "Assets/HunterGrandma/Art/Bosses/";
        static GrandmaStageSetup() { EditorApplication.delayCall += Configure; }
        [MenuItem("Tools/Hunter Grandma/Configure Stage v2")]
        public static void Configure()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode || !File.Exists(Art + "Source/S0_Dog_Spritesheet_CC0.png")) return;
            Directory.CreateDirectory(Art + "Frames");
            string[] sources = { "S0_Dog_Spritesheet_CC0.png", "CyberCity/pizzaGuy.png", "S2_Salesperson_Source_96px_CC0.png", "S3_Pixel_Worker_Hardhat_CC0.png", "S4_Dancers_32px_CC0.png", "S5_Bodybuilder_CC0.png", "S6_Medical_Examiner_32px_CC0.png" };
            RectInt[] cells = { new RectInt(0,0,16,16), new RectInt(0,64,64,64), new RectInt(193,1,94,94), new RectInt(1,1,17,30), new RectInt(0,128,32,32), new RectInt(0,0,64,64), new RectInt(1,1,30,30) };
            Sprite[] bosses = new Sprite[DefenseStages.Count];
            for (int i = 0; i < bosses.Length; i++)
            {
                string destination = Art + "Frames/boss_stage_" + i + "_64.png";
                if (!File.Exists(destination)) Normalize(Art + "Source/" + sources[i], cells[i], destination, i == 3);
                bosses[i] = Import(destination, new Vector2(0.5f, 3f / 64f), 16f);
            }
            string cartPath = Art + "capture_cart.png", coinPath = Art + "gold_coin.png";
            if (!File.Exists(cartPath)) DrawCart(cartPath);
            if (!File.Exists(coinPath)) DrawCoin(coinPath);
            Sprite cart = Import(cartPath, new Vector2(0.5f, 0.1f), 12f);
            Sprite coin = Import(coinPath, new Vector2(0.5f, 0.5f), 8f);
            const string openingArt = "Assets/HunterGrandma/Art/Stage0/";
            Sprite tea = File.Exists(openingArt + "sundeok_tea.png") ? Import(openingArt + "sundeok_tea.png", new Vector2(.5f, 3f / 64f), 16f) : null;
            Sprite chair = File.Exists(openingArt + "sundeok_chair.png") ? Import(openingArt + "sundeok_chair.png", new Vector2(.5f, 3f / 64f), 16f) : null;
            Scene scene = SceneManager.GetSceneByPath(GrandmaProjectSetup.ScenePath);
            bool opened = !scene.isLoaded;
            if (opened) scene = EditorSceneManager.OpenScene(GrandmaProjectSetup.ScenePath, OpenSceneMode.Additive);
            bool wasDirty = scene.isDirty, changed = false;
            foreach (GrandmaDefense encounter in scene.GetRootGameObjects().SelectMany(r => r.GetComponentsInChildren<GrandmaDefense>(true)))
            {
                var serialized = new SerializedObject(encounter);
                SpriteRenderer backdrop=scene.GetRootGameObjects().SelectMany(r=>r.GetComponentsInChildren<SpriteRenderer>(true)).FirstOrDefault(r=>AssetDatabase.GetAssetPath(r.sprite)=="Assets/HunterGrandma/Art/village_dusk.png");
                if(serialized.FindProperty("beltBackdrop").objectReferenceValue!=backdrop){serialized.FindProperty("beltBackdrop").objectReferenceValue=backdrop;changed=true;}
                GameObject preview = scene.GetRootGameObjects().FirstOrDefault(r => r.name == "Belt layout preview");
                if (preview == null && backdrop != null)
                {
                    preview = CreateBeltPreview(encounter, backdrop, serialized);
                    changed = true;
                }
                if (serialized.FindProperty("beltLayoutPreview").objectReferenceValue != preview)
                { serialized.FindProperty("beltLayoutPreview").objectReferenceValue = preview; changed = true; }
                if (tea != null && chair != null)
                {
                    foreach (var binding in new[] { ("houseTeaSprite", tea), ("houseChairSprite", chair) })
                        if (serialized.FindProperty(binding.Item1).objectReferenceValue != binding.Item2) { serialized.FindProperty(binding.Item1).objectReferenceValue = binding.Item2; changed = true; }
                    Transform oldOpening = preview != null ? preview.transform.Find("Stage 0 house opening") : null;
                    if (preview != null && (oldOpening == null || !oldOpening.GetComponent<StageZeroOpeningView>().HasCurrentLayout))
                    {
                        if (oldOpening != null) Object.DestroyImmediate(oldOpening.gameObject);
                        foreach (Transform child in preview.transform)
                            if (child.name == "Grandma · belt preview" || child.name.StartsWith("Zombie · depth preview")) child.gameObject.SetActive(false);
                        var shooter = (GrandmaShooter)serialized.FindProperty("grandma").objectReferenceValue;
                        var zombie = (ZombieActor)serialized.FindProperty("zombiePrefab").objectReferenceValue;
                        var dot = ((ShotTracer)serialized.FindProperty("tracerPrefab").objectReferenceValue).GetComponent<SpriteRenderer>();
                        StageZeroOpeningView.Create(preview.transform, tea, chair, shooter.GetComponent<SpriteRenderer>().sprite, zombie.WalkFrames, dot.sprite, dot.sharedMaterial, false);
                        changed = true;
                    }
                }
                SerializedProperty array = serialized.FindProperty("bossSprites");
                if (array.arraySize != bosses.Length) { array.arraySize = bosses.Length; changed = true; }
                for (int i = 0; i < bosses.Length; i++)
                    if (array.GetArrayElementAtIndex(i).objectReferenceValue != bosses[i]) { array.GetArrayElementAtIndex(i).objectReferenceValue = bosses[i]; changed = true; }
                foreach (var binding in new[] { ("cartSprite", cart), ("coinSprite", coin) })
                    if (serialized.FindProperty(binding.Item1).objectReferenceValue != binding.Item2) { serialized.FindProperty(binding.Item1).objectReferenceValue = binding.Item2; changed = true; }
                serialized.ApplyModifiedPropertiesWithoutUndo();
            }
            foreach (DefenseHud hud in scene.GetRootGameObjects().SelectMany(r => r.GetComponentsInChildren<DefenseHud>(true)))
            {
                var ui = new SerializedObject(hud);
                foreach (var binding in new[] { ("resourcePlaceholder", "resource-01"), ("attackPlaceholder", "attack-01"), ("ratePlaceholder", "rate-01"), ("upgradePlaceholder", "upgrade-01") })
                {
                    string path = "Assets/HunterGrandma/Art/UI/Placeholders/" + binding.Item2 + ".png";
                    if (!File.Exists(path)) continue;
                    Sprite sprite = Import(path, Vector2.one * .5f, 64f);
                    if (ui.FindProperty(binding.Item1).objectReferenceValue == sprite) continue;
                    ui.FindProperty(binding.Item1).objectReferenceValue = sprite; changed = true;
                }
                ui.ApplyModifiedPropertiesWithoutUndo();
            }
            if (changed) { EditorSceneManager.MarkSceneDirty(scene); if (!wasDirty || opened) EditorSceneManager.SaveScene(scene); }
            if (opened) EditorSceneManager.CloseScene(scene, true);
        }
        private static GameObject CreateBeltPreview(GrandmaDefense encounter, SpriteRenderer backdrop, SerializedObject state)
        {
            var preview = new GameObject("Belt layout preview");
            SceneManager.MoveGameObjectToScene(preview, encounter.gameObject.scene);
            SpriteRenderer dot = ((ShotTracer)state.FindProperty("tracerPrefab").objectReferenceValue).GetComponent<SpriteRenderer>();
            BeltStageView.BuildRoad(preview.transform, dot.sprite, dot.sharedMaterial, DefenseStages.Get(0).WaveCount * 10f + 14f,
                new[] { new Rect(12,-2.65f,1.4f,.75f), new Rect(23,-1.85f,1.5f,.7f), new Rect(34,-3.6f,1.4f,.7f) });
            SpriteRenderer background = new GameObject("Belt preview backdrop", typeof(SpriteRenderer)).GetComponent<SpriteRenderer>();
            background.transform.SetParent(preview.transform, false); background.transform.position = backdrop.transform.position + Vector3.up * 2.05f;
            background.transform.localScale = backdrop.transform.lossyScale; background.sprite = backdrop.sprite; background.sharedMaterial = backdrop.sharedMaterial; background.sortingOrder = -30;
            backdrop.enabled = false;
            GrandmaShooter grandma = (GrandmaShooter)state.FindProperty("grandma").objectReferenceValue;
            PreviewActor(grandma.GetComponent<SpriteRenderer>(), BeltSpace.Start, "Grandma · belt preview");
            SerializedProperty zombies = state.FindProperty("startingZombies");
            for (int i = 0; i < zombies.arraySize; i++)
            {
                var zombie = zombies.GetArrayElementAtIndex(i).objectReferenceValue as ZombieActor;
                if (zombie != null) PreviewActor(zombie.GetComponent<SpriteRenderer>(), new Vector2(2 + i * 4, i == 0 ? -1.25f : i == 1 ? -3.45f : -2.45f), "Zombie · depth preview " + i);
            }
            return preview;
            void PreviewActor(SpriteRenderer source, Vector2 foot, string name)
            {
                SpriteRenderer body = new GameObject(name, typeof(SpriteRenderer)).GetComponent<SpriteRenderer>();
                body.transform.SetParent(preview.transform, false); body.transform.position = foot; body.transform.localScale = Vector3.one * .8f;
                body.sprite = source.sprite; body.sharedMaterial = source.sharedMaterial; body.sortingOrder = BeltSpace.SortOrder(foot.y);
                source.enabled = false;
            }
        }
        private static Sprite Import(string path, Vector2 pivot, float ppu)
        {
            AssetDatabase.ImportAsset(path);
            var importer = (TextureImporter)AssetImporter.GetAtPath(path);
            if (importer.textureType != TextureImporterType.Sprite || importer.spritePixelsPerUnit != ppu || importer.filterMode != FilterMode.Point || importer.textureCompression != TextureImporterCompression.Uncompressed)
            {
                importer.textureType = TextureImporterType.Sprite; importer.spriteImportMode = SpriteImportMode.Single;
                importer.spritePixelsPerUnit = ppu; importer.filterMode = FilterMode.Point; importer.mipmapEnabled = false;
                importer.textureCompression = TextureImporterCompression.Uncompressed; importer.npotScale = TextureImporterNPOTScale.None; importer.alphaIsTransparency = true;
                var settings = new TextureImporterSettings(); importer.ReadTextureSettings(settings);
                settings.spriteAlignment = (int)SpriteAlignment.Custom; settings.spritePivot = pivot; settings.spriteMeshType = SpriteMeshType.FullRect;
                importer.SetTextureSettings(settings); importer.SaveAndReimport();
            }
            return AssetDatabase.LoadAssetAtPath<Sprite>(path);
        }
        private static void Normalize(string path, RectInt cell, string output, bool whiteBackground)
        {
            var source = new Texture2D(2, 2, TextureFormat.RGBA32, false); source.LoadImage(File.ReadAllBytes(path));
            Color[] pixels = new Color[cell.width * cell.height];
            int minX = cell.width, minY = cell.height, maxX = -1, maxY = -1;
            for (int y = 0; y < cell.height; y++)
            for (int x = 0; x < cell.width; x++)
            {
                Color c = source.GetPixel(cell.x + x, source.height - cell.y - cell.height + y);
                if (whiteBackground && c.r > 0.93f && c.g > 0.93f && c.b > 0.93f) c.a = 0f;
                pixels[y * cell.width + x] = c;
                if (c.a < 0.7f) continue;
                minX = Mathf.Min(minX, x); minY = Mathf.Min(minY, y); maxX = Mathf.Max(maxX, x); maxY = Mathf.Max(maxY, y);
            }
            if (maxX < minX) throw new System.InvalidOperationException("No visible boss pixels: " + path);
            int w = maxX - minX + 1, h = maxY - minY + 1;
            float scale = Mathf.Min(58f / h, 60f / w);
            int newWidth = Mathf.RoundToInt(w * scale), newHeight = Mathf.RoundToInt(h * scale);
            var destination = new Texture2D(64, 64, TextureFormat.RGBA32, false); destination.SetPixels(new Color[4096]);
            for (int y = 0; y < newHeight; y++)
            for (int x = 0; x < newWidth; x++)
            {
                int sx = minX + Mathf.Min(w - 1, Mathf.FloorToInt((x + 0.5f) / scale));
                int sy = minY + Mathf.Min(h - 1, Mathf.FloorToInt((y + 0.5f) / scale));
                Color c = pixels[sy * cell.width + sx]; if (c.a < 0.7f) continue; c.a = 1f;
                destination.SetPixel((64 - newWidth) / 2 + x, 3 + y, c);
            }
            destination.Apply(); File.WriteAllBytes(output, destination.EncodeToPNG());
            Object.DestroyImmediate(source); Object.DestroyImmediate(destination);
        }
        private static void DrawCoin(string path)
        {
            var texture = new Texture2D(8, 8, TextureFormat.RGBA32, false); texture.SetPixels(new Color[64]);
            for (int y = 0; y < 8; y++) for (int x = 0; x < 8; x++)
                if ((x - 3.5f) * (x - 3.5f) + (y - 3.5f) * (y - 3.5f) < 14f) texture.SetPixel(x, y, x < 3 ? new Color(1f, 0.93f, 0.5f) : new Color(0.9f, 0.62f, 0.16f));
            texture.Apply(); File.WriteAllBytes(path, texture.EncodeToPNG()); Object.DestroyImmediate(texture);
        }
        private static void DrawCart(string path)
        {
            var texture = new Texture2D(64, 32, TextureFormat.RGBA32, false); texture.SetPixels(new Color[2048]);
            void Rect(int x, int y, int w, int h, Color c) { for (int py=y; py<y+h; py++) for (int px=x; px<x+w; px++) texture.SetPixel(px,py,c); }
            Color dark = new Color(0.18f,0.14f,0.14f), wood = new Color(0.62f,0.40f,0.22f), light = new Color(0.84f,0.64f,0.36f);
            Rect(3,10,57,3,dark); Rect(15,12,45,16,dark); Rect(17,14,41,12,wood);
            Rect(17,18,41,2,light); Rect(17,24,41,2,light); Rect(26,13,2,14,dark); Rect(46,13,2,14,dark);
            for(int cy=0;cy<2;cy++) for(int y=-5;y<=5;y++) for(int x=-5;x<=5;x++)
                if(x*x+y*y<27) texture.SetPixel(25+cy*25+x,7+y,x*x+y*y<7?light:dark);
            texture.Apply(); File.WriteAllBytes(path,texture.EncodeToPNG()); Object.DestroyImmediate(texture);
        }
    }
}
