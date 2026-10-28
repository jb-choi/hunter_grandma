using UnityEngine;

namespace HunterGrandma
{
    // Presentation follows the three supplied boards. GrandmaDefense owns combat entry.
    public sealed class StageZeroOpeningView : MonoBehaviour
    {
        public const float TeaEnd = 2.8f, RevealEnd = 5.3f, Duration = 8f;
        public const int LayoutVersion = 4;
        private const float UpperRail = 2.15f;
        [SerializeField, HideInInspector] private int layoutVersion;
        public bool HasCurrentLayout => layoutVersion == LayoutVersion;
        public static readonly Vector2 Seat = new Vector2(-8.3f, -2.45f);
        public static readonly Vector2 Exit = new Vector2(-2.4f, -2.45f);
        private SpriteRenderer actor, chair, darkness, windowGlass, door;
        private Transform fence;
        private SpriteRenderer[] visitors, impactMarks;
        private Sprite[] zombieFrames;
        private Sprite tea, standing, pixel;
        private Material material;
        private AudioSource audioSource;
        private AudioClip growl, knock;
        private int cues;
        private float fenceShake;
        public int SoundCues => cues;
        public float Age { get; private set; }
        public string Caption => Age < TeaEnd ? "따뜻한 차 한 모금…" : Age < RevealEnd ? "그르르…  쿵, 쿵!  밖에 무슨 일이?" : "차는 나중에 마시고, 먼저 좀비부터.";
        public bool IsLeaving => Age >= RevealEnd;

        public static Vector2 FenceFoot(float x) => new Vector2(x, -.65f - (x + .3f) * .285f);
        public static Vector2 VisitorFoot(int index) => FenceFoot(2f + index % 3 * 3.5f) + new Vector2(.34f, .22f);
        public static StageZeroOpeningView Create(Transform parent, Sprite teaSprite, Sprite chairSprite, Sprite idle, Sprite[] zombies, Sprite dot, Material shared, bool withAudio)
        {
            var obj = new GameObject("Stage 0 house opening", typeof(StageZeroOpeningView)); obj.transform.SetParent(parent, false);
            StageZeroOpeningView view = obj.GetComponent<StageZeroOpeningView>();
            view.pixel = dot; view.material = shared; view.tea = teaSprite; view.standing = idle; view.zombieFrames = zombies;
            view.layoutVersion = LayoutVersion; view.BuildRoom(chairSprite);
            if (withAudio)
            {
                view.audioSource = obj.AddComponent<AudioSource>(); view.audioSource.playOnAwake = false;
                view.audioSource.spatialBlend = 0; view.audioSource.volume = .12f;
                view.growl = MakeSound(true); view.knock = MakeSound(false);
            }
            view.Begin(); return view;
        }
        private void BuildRoom(Sprite chairSprite)
        {
            Color wall = new Color(.30f, .25f, .25f), trim = new Color(.47f, .31f, .22f), wood = new Color(.29f, .18f, .13f);
            Box("Night sky tint", 65, 2.5f, 160, 12, new Color(.015f, .025f, .055f, .91f), -19);
            Box("House back wall", -8.7f, .8f, 10.1f, 8.4f, wall, -15);
            Box("Interior floor", -8.7f, -3.05f, 10.1f, 2.5f, new Color(.29f, .23f, .19f), -13);
            Box("Wall skirting", -8.7f, -1.78f, 10.1f, .13f, trim, -12);
            for (int i = 0; i < 11; i++) Box("Floor plank", -13.5f + i, -3.1f, .035f, 2.4f, new Color(.20f, .15f, .13f), -12);
            Box("Wall top trim", -8.7f, 4.8f, 10.1f, .15f, trim, -12);
            Box("Window frame", -5.6f, 1.35f, 2.4f, 2.7f, trim, -10);
            windowGlass = Box("Dark window", -5.6f, 1.35f, 2.08f, 2.38f, Color.black, -9);
            Box("Window mullion", -5.6f, 1.35f, .1f, 2.4f, wood, -8);
            Box("Window crossbar", -5.6f, 1.35f, 2.1f, .1f, wood, -8);
            Box("Curtain left", -6.84f, 1.4f, .24f, 3.1f, new Color(.41f, .24f, .25f), -7);
            Box("Curtain right", -4.36f, 1.4f, .24f, 3.1f, new Color(.41f, .24f, .25f), -7);
            Box("Door frame", -3.75f, -.25f, 2.0f, 4.65f, trim, -10);
            Box("Open doorway", -3.75f, -.25f, 1.73f, 4.38f, new Color(.03f, .045f, .065f), -9);
            door = Box("Opening door", -3.75f, -.25f, 1.73f, 4.38f, new Color(.37f, .24f, .17f), -8);
            Box("Warm lamp glow", -9.3f, 3.1f, 1.1f, .18f, new Color(.85f, .62f, .32f), -7);
            Box("Lamp stem", -9.3f, 3.65f, .05f, .95f, trim, -7);
            Box("Side table top", -6.75f, -2.0f, 1.1f, .12f, trim, 800);
            Box("Side table leg", -6.75f, -2.45f, .12f, .9f, wood, 800);
            chair = Image("Empty chair", chairSprite, Seat, .8f, 1200); chair.enabled = false;
            actor = Image("Opening Sundeok", tea, Seat, .8f, 1250);
            fence = new GameObject("Opening fence").transform; fence.SetParent(transform, false);
            for (int i = 0; i < 11; i++)
            {
                Vector2 foot = FenceFoot(-.3f + i * 1.25f);
                SpriteRenderer post = Box("Fence post", foot.x, foot.y + 1.175f, .16f, 2.45f, trim, 1500); post.transform.SetParent(fence, true);
                SpriteRenderer cap = Box("Fence post cap", foot.x, foot.y + 2.4f, .24f, .12f, new Color(.59f, .42f, .27f), 1501); cap.transform.SetParent(fence, true);
                if (i == 10) continue;
                foreach (float height in new[] { .95f, UpperRail })
                {
                    Vector2 a = foot + Vector2.up * height, b = FenceFoot(foot.x + 1.25f) + Vector2.up * height;
                    SpriteRenderer rail = Box("Fence rail", (a.x + b.x) * .5f, (a.y + b.y) * .5f, Vector2.Distance(a, b), .13f, wood, 1500);
                    rail.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(b.y - a.y, b.x - a.x) * Mathf.Rad2Deg); rail.transform.SetParent(fence, true);
                }
            }
            visitors = new SpriteRenderer[3]; impactMarks = new SpriteRenderer[3];
            for (int i = 0; i < visitors.Length; i++)
            {
                visitors[i] = Image("Zombie outside " + i, zombieFrames[0], VisitorFoot(i), .8f, BeltSpace.SortOrder(VisitorFoot(i).y));
                Vector2 hit = FenceFoot(VisitorFoot(i).x - .8f) + Vector2.up * UpperRail;
                impactMarks[i] = Box("Fence impact " + i, hit.x, hit.y, .28f, .07f, new Color(.94f, .77f, .46f), 1510);
                impactMarks[i].transform.rotation = Quaternion.Euler(0, 0, 50); impactMarks[i].enabled = false;
            }
            foreach (SpriteRenderer part in fence.GetComponentsInChildren<SpriteRenderer>()) part.sortingOrder = BeltSpace.SortOrder(FenceFoot(part.transform.position.x).y);
            darkness = Box("Unseen exterior", 80, 0, 165.5f, 28, Color.black, 20000);
        }
        public void Begin()
        {
            gameObject.SetActive(true); Age = 0; cues = 0; fenceShake = 0;
            if (audioSource != null) audioSource.Stop();
            actor.enabled = true; actor.sprite = tea; actor.transform.position = Seat;
            foreach (SpriteRenderer visitor in visitors) visitor.enabled = true;
            darkness.sortingOrder = 20000; chair.enabled = false;
            Present();
        }
        public void Advance(float deltaTime)
        {
            if (deltaTime <= 0) return;
            float before = Age; Age = Mathf.Min(Duration, Age + deltaTime);
            if (before < TeaEnd && Age >= TeaEnd) Cue(growl);
            for (int i = 0; i < visitors.Length; i++)
            {
                float phase = i * .31f;
                if (Age >= TeaEnd && Mathf.FloorToInt((Age - TeaEnd + phase - .44f) / 1.35f) > Mathf.FloorToInt((Mathf.Max(before, TeaEnd) - TeaEnd + phase - .44f) / 1.35f)) FenceImpact(VisitorFoot(i));
            }
            AdvanceFence(deltaTime);
            Present();
        }
        private void Cue(AudioClip sound) { cues++; if (audioSource != null && sound != null) audioSource.PlayOneShot(sound); }
        private void Present()
        {
            float reveal = Mathf.SmoothStep(0, 1, Mathf.InverseLerp(TeaEnd, 4.5f, Age));
            float leave = Mathf.SmoothStep(0, 1, Mathf.InverseLerp(RevealEnd, Duration, Age));
            darkness.color = new Color(0, .005f, .015f, Mathf.Lerp(1f, .32f, reveal) * (1f - leave));
            windowGlass.color = Color.Lerp(Color.black, new Color(.12f, .18f, .22f), reveal);
            actor.sprite = IsLeaving ? standing : tea; chair.enabled = IsLeaving;
            actor.transform.position = IsLeaving ? Vector2.Lerp(Seat, Exit, leave) : Seat;
            actor.transform.localScale = new Vector3(.8f, .8f * (1f + Mathf.Sin(Age * 2.2f) * .004f), 1f);
            if (IsLeaving) actor.transform.position += Vector3.up * (Mathf.Abs(Mathf.Sin(Age * 11f)) * .035f);
            float open = Mathf.SmoothStep(0, 1, Mathf.InverseLerp(5.05f, 5.9f, Age));
            float width = Mathf.Lerp(1.73f, .18f, open);
            door.transform.localScale = new Vector3(width, 4.38f, 1); door.transform.position = new Vector3(-4.615f + width * .5f, -.25f);
            for (int i = 0; i < visitors.Length; i++)
            {
                float beat = Mathf.Repeat(Mathf.Max(0, Age - TeaEnd) + i * .31f, 1.35f);
                float reach = ZombieActor.FenceReach(beat);
                visitors[i].sprite = zombieFrames[Mathf.Min(reach > .1f ? 1 : 2, zombieFrames.Length - 1)];
                visitors[i].transform.position = VisitorFoot(i) + new Vector2(-reach * .18f, -Mathf.Max(0, reach) * .08f);
                visitors[i].transform.localRotation = Quaternion.Euler(0, 0, reach * 11f);
                visitors[i].transform.localScale = new Vector3(.8f * (1 + Mathf.Max(0, reach) * .06f), .8f * (1 + reach * .035f), 1);
                impactMarks[i].transform.position = FenceFoot(VisitorFoot(i).x - .8f) + Vector2.up * UpperRail;
                impactMarks[i].enabled = Age >= TeaEnd && beat >= .44f && beat < .56f;
            }
        }
        public void Finish()
        {
            Age = Duration; Present(); actor.enabled = false; chair.enabled = true;
            foreach (SpriteRenderer visitor in visitors) visitor.enabled = false;
            foreach (SpriteRenderer mark in impactMarks) mark.enabled = false;
            darkness.color = Color.clear;
            if (audioSource != null) audioSource.Stop();
            fence.localPosition = Vector3.zero; fenceShake = 0;
        }
        public void FenceImpact(Vector2 zombieFoot)
        {
            fenceShake = .16f; Cue(knock);
            impactMarks[0].transform.position = FenceFoot(zombieFoot.x - .8f) + Vector2.up * UpperRail;
        }
        public void AdvanceFence(float deltaTime)
        {
            fenceShake = Mathf.Max(0, fenceShake - deltaTime);
            fence.localPosition = new Vector3(Mathf.Sin(fenceShake * 150f) * fenceShake * .3f, Mathf.Sin(fenceShake * 120f) * fenceShake * .12f);
            if (Age >= Duration) impactMarks[0].enabled = fenceShake > .06f;
        }
        public void SetPaused(bool paused)
        { if (audioSource != null) { if (paused) audioSource.Pause(); else audioSource.UnPause(); } }
        private SpriteRenderer Box(string name, float x, float y, float width, float height, Color color, int order)
        {
            SpriteRenderer renderer = Image(name, pixel, new Vector2(x, y), 1, order);
            renderer.transform.localScale = new Vector3(width, height, 1); renderer.color = color; return renderer;
        }
        private SpriteRenderer Image(string name, Sprite sprite, Vector2 position, float scale, int order)
        {
            var obj = new GameObject(name, typeof(SpriteRenderer)); obj.transform.SetParent(transform, false);
            obj.transform.position = position; obj.transform.localScale = Vector3.one * scale;
            SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>(); renderer.sprite = sprite; renderer.sharedMaterial = material; renderer.sortingOrder = order; return renderer;
        }
        private static AudioClip MakeSound(bool isGrowl)
        {
            const int sampleRate = 22050; int count = isGrowl ? 14332 : 6615;
            float[] samples = new float[count]; uint noise = 271828;
            for (int i = 0; i < count; i++)
            {
                float t = (float)i / sampleRate; noise = noise * 1664525u + 1013904223u;
                float random = ((noise >> 8) / 16777215f) * 2 - 1;
                samples[i] = isGrowl ? (Mathf.Sin(t * 430f) * .55f + random * .2f) * Mathf.Sin(Mathf.PI * i / count) : (Mathf.Sin(t * 590f) * .7f + random * .3f) * Mathf.Exp(-t * 23f);
            }
            AudioClip clip = AudioClip.Create(isGrowl ? "Outside growl" : "Fence knock", count, 1, sampleRate, false); clip.SetData(samples, 0); return clip;
        }
        private void OnDestroy() { if (growl != null) Destroy(growl); if (knock != null) Destroy(knock); }
    }
}
