using UnityEngine;

namespace HunterGrandma
{
    [RequireComponent(typeof(SpriteRenderer), typeof(AudioSource))]
    public sealed class GrandmaShooter : MonoBehaviour
    {
        [SerializeField] private Sprite idleSprite;
        [SerializeField] private Sprite firingSprite;
        [SerializeField] private Vector2 muzzleOffset = new Vector2(1.1875f, 2.5625f);
        [SerializeField, Range(0f, 1f)] private float shotVolume = 0.16f;
        private SpriteRenderer spriteRenderer;
        private AudioSource audioSource;
        private AudioClip shotSound;
        private Vector3 restPosition;
        private float recoil;
        private float facing = 1f;
        private GrandmaDefense encounter;
        private BeltActorVisual visual;
        private bool walking;
        private float walkAge;

        public Vector3 MuzzlePosition => transform.position + new Vector3(muzzleOffset.x * facing * .8f, muzzleOffset.y * .8f, 0f);
        public Vector2 FootPosition => transform.position;
        public Vector3 RestPosition => restPosition;
        public Sprite IdleSprite => idleSprite;
        public void SetPresentationVisible(bool visible)
        { if (visual != null) { visual.Body.enabled = visible; visual.Shadow.enabled = visible; } }
        public void InitializeBelt(GrandmaDefense owner, Sprite shadow)
        {
            encounter = owner; visual = gameObject.AddComponent<BeltActorVisual>();
            visual.Initialize(spriteRenderer, shadow, .8f); spriteRenderer = visual.Body;
        }
        public void FaceTarget(float x)
        {
            facing = x < transform.position.x ? -1f : 1f;
            spriteRenderer.flipX = facing < 0f;
        }
        public void PresentTravel(Vector3 position, bool walking)
        {
            recoil = 0f;
            spriteRenderer.sprite = idleSprite;
            if (Mathf.Abs(position.x-transform.position.x)>.005f) FaceTarget(position.x);
            this.walking = walking;
            transform.position = position;
            transform.rotation = Quaternion.identity;
        }
        public void MoveOnBelt(Vector2 position)
        {
            walking = Vector2.Distance(position, FootPosition) > .001f;
            if (Mathf.Abs(position.x-transform.position.x)>.005f) FaceTarget(position.x);
            transform.position = position;
        }

        public void Configure(Sprite idle, Sprite firing)
        {
            idleSprite = idle;
            firingSprite = firing;
            GetComponent<SpriteRenderer>().sprite = idle;
        }

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            audioSource = GetComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0f;
            restPosition = transform.position;
            shotSound = CreateShotSound();
            ResetPose();
        }

        public void Fire()
        {
            recoil = 0.14f;
            spriteRenderer.sprite = firingSprite;
            if (shotVolume > 0f) audioSource.PlayOneShot(shotSound, shotVolume);
        }

        public void ResetPose()
        {
            recoil = 0f;
            walking = false; walkAge = 0f;
            facing = 1f;
            if (spriteRenderer != null) spriteRenderer.sprite = idleSprite;
            if (spriteRenderer != null) spriteRenderer.flipX = false;
            transform.SetPositionAndRotation(restPosition, Quaternion.identity);
            visual?.Pose(Vector3.zero);
        }

        private void Update()
        {
            if (visual == null) return;
            float dt = encounter.CombatDeltaTime;
            if (dt <= 0f) return;
            if (walking) walkAge += dt;
            recoil = Mathf.Max(0f, recoil - dt);
            float kick = recoil / 0.14f;
            Vector3 offset = new Vector3(-facing*.12f*kick,walking ? Mathf.Abs(Mathf.Sin(walkAge*9f))*.055f : 0f);
            visual.Pose(offset, facing * 1.8f * kick + (walking ? Mathf.Sin(walkAge*9f)*1.2f : 0f));
            if (recoil < 0.055f) spriteRenderer.sprite = idleSprite;
        }

        private static AudioClip CreateShotSound()
        {
            const int sampleRate = 22050;
            var samples = new float[4410];
            uint noise = 1234567;
            for (int i = 0; i < samples.Length; i++)
            {
                float t = (float)i / sampleRate;
                noise = noise * 1664525u + 1013904223u;
                float white = ((noise >> 8) / 16777215f) * 2f - 1f;
                samples[i] = (white * 0.65f + Mathf.Sin(t * 470f) * 0.35f) * Mathf.Exp(-t * 35f);
            }
            AudioClip clip = AudioClip.Create("Sundeok shotgun", samples.Length, 1, sampleRate, false);
            clip.SetData(samples, 0);
            return clip;
        }

        private void OnDestroy()
        {
            if (shotSound != null) Destroy(shotSound);
        }
    }
}
