using UnityEngine;

namespace HunterGrandma
{
    [RequireComponent(typeof(SpriteRenderer), typeof(AudioSource))]
    public sealed class GrandmaShooter : MonoBehaviour
    {
        [SerializeField] private Sprite idleSprite;
        [SerializeField] private Sprite firingSprite;
        [SerializeField] private SundeokAnimationSet animationSet;
        [SerializeField] private Vector2 muzzleOffset = new Vector2(1.1875f, 2.5625f);
        [SerializeField, Range(0f, 1f)] private float shotVolume = 0.16f;
        [SerializeField] private AudioClip reloadSound;
        [SerializeField, Range(0f, 1f)] private float reloadVolume = .13f;
        private SpriteRenderer spriteRenderer;
        private AudioSource audioSource;
        private AudioSource reloadAudioSource;
        private bool reloadAudioPaused;
        private AudioClip shotSound;
        private Vector3 restPosition;
        private float recoil;
        private float facing = 1f;
        private GrandmaDefense encounter;
        private BeltActorVisual visual;
        private bool walking;
        private float walkAge;
        private int motionFrame = -1;
        private SundeokAnimationPlayer animation;

        public Vector3 MuzzlePosition => animation != null ? visual.Display.TransformPoint(animation.MuzzleOffset) : transform.position + new Vector3(muzzleOffset.x * facing * .8f, muzzleOffset.y * .8f, 0f);
        public Vector2 FootPosition => transform.position;
        public float FacingDirection => facing;
        public Material ActorMaterial => spriteRenderer.sharedMaterial;
        public Vector3 RestPosition => restPosition;
        public Sprite IdleSprite => animationSet != null && animationSet.IsReady ? animationSet.Clip(SundeokAction.Idle).frames[0].composite : idleSprite;
        public SundeokAnimationSet AnimationSet => animationSet;
        public SundeokAction AnimationAction => animation != null ? animation.Action : walking ? SundeokAction.Walk : SundeokAction.Idle;
        public int AnimationFrame => animation != null ? animation.FrameIndex : 0;
        public int WalkingFrame => animation != null ? animation.WalkFrameIndex : 0;
        public bool IsLayeredAnimation => animation != null && animation.IsLayered;
        public bool IsCollecting => animation != null && animation.IsCollecting;
        public float AnimationTime => animation != null ? animation.AnimationTime : 0;
        public AudioClip ReloadSound => reloadSound;
        public int ReloadSoundCues { get; private set; }
        public void SetPresentationVisible(bool visible)
        { if (!visible) StopReloadSound(); if (visual != null) { visual.Body.enabled = visible; visual.Shadow.enabled = visible; animation?.SetVisible(visible); } }
        public void InitializeBelt(GrandmaDefense owner, Sprite shadow)
        {
            encounter = owner; visual = gameObject.AddComponent<BeltActorVisual>();
            visual.Initialize(spriteRenderer, shadow, .8f); spriteRenderer = visual.Body;
            if (animationSet != null && animationSet.IsReady) animation = new SundeokAnimationPlayer(animationSet, visual, PlayReloadSound);
        }
        public void FaceTarget(float x)
        {
            facing = x < transform.position.x ? -1f : 1f;
            spriteRenderer.flipX = facing < 0f;
        }
        public void PresentTravel(Vector3 position, bool walking)
        {
            recoil = 0f;
            StopReloadSound();
            if (animation == null) spriteRenderer.sprite = idleSprite;
            animation?.StopCombat();
            if (Mathf.Abs(position.x-transform.position.x)>.005f) FaceTarget(position.x);
            this.walking = walking;
            motionFrame = Time.frameCount;
            transform.position = position;
            transform.rotation = Quaternion.identity;
        }
        public void MoveOnBelt(Vector2 position)
        {
            walking = Vector2.Distance(position, FootPosition) > .001f;
            motionFrame = Time.frameCount;
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
            reloadAudioSource = gameObject.AddComponent<AudioSource>();
            reloadAudioSource.playOnAwake = false;
            reloadAudioSource.loop = false;
            reloadAudioSource.spatialBlend = 0f;
            reloadAudioSource.dopplerLevel = 0f;
            reloadAudioSource.priority = 160;
            restPosition = transform.position;
            shotSound = CreateShotSound();
            ResetPose();
        }

        public void Fire()
        {
            StopReloadSound();
            recoil = 0.14f;
            if (animation != null) animation.Fire(walking && motionFrame == Time.frameCount, facing, encounter.Progress.FireInterval);
            else spriteRenderer.sprite = firingSprite;
            PlaySkillShot(false);
        }

        public void PlaySkillShot(bool final)
        { if (shotVolume > 0f) audioSource.PlayOneShot(shotSound, Mathf.Min(1f, shotVolume * (final ? 1.6f : 1f))); }

        public void ResetPose()
        {
            StopReloadSound(); ReloadSoundCues = 0;
            recoil = 0f;
            walking = false; walkAge = 0f;
            facing = 1f;
            if (spriteRenderer != null) spriteRenderer.sprite = idleSprite;
            if (spriteRenderer != null) spriteRenderer.flipX = false;
            transform.SetPositionAndRotation(restPosition, Quaternion.identity);
            visual?.Pose(Vector3.zero);
            motionFrame = -1; animation?.Reset();
        }
        public void SetCombatTarget(DefenseTarget target, float untilShot)
        {
            if (target != null) FaceTarget(target.transform.position.x);
            animation?.SetAim(target != null, untilShot, encounter.Progress.FireInterval);
        }
        public void QueueCollect() { animation?.QueueCollect(); }
        public bool TryCollect() => animation != null && animation.TryCollect();
        public void InterruptCollect() { animation?.InterruptCollect(); }
        public void StopCombatPresentation() { animation?.StopCombat(); StopReloadSound(); }

        private void PlayReloadSound()
        {
            if (reloadSound == null || reloadAudioSource == null || !isActiveAndEnabled || reloadVolume <= 0 || audioSource.mute || audioSource.volume <= 0) return;
            reloadAudioSource.outputAudioMixerGroup = audioSource.outputAudioMixerGroup;
            reloadAudioSource.ignoreListenerPause = audioSource.ignoreListenerPause;
            reloadAudioSource.clip = reloadSound;
            // Keep the normal cadence's timbre; shorten faster reloads without changing the gunshot source.
            reloadAudioSource.pitch = Mathf.Clamp(.72f / encounter.Progress.FireInterval, 1f, 3f);
            reloadAudioSource.volume = audioSource.volume * reloadVolume;
            reloadAudioSource.mute = audioSource.mute;
            reloadAudioPaused = false;
            reloadAudioSource.Play(); ReloadSoundCues++;
        }
        private void StopReloadSound()
        { if (reloadAudioSource != null) reloadAudioSource.Stop(); reloadAudioPaused = false; }
        private void UpdateReloadSound(bool frozen)
        {
            if (reloadAudioSource == null) return;
            reloadAudioSource.mute = audioSource.mute;
            reloadAudioSource.volume = audioSource.volume * reloadVolume;
            if (frozen && !reloadAudioPaused && reloadAudioSource.isPlaying)
            { reloadAudioSource.Pause(); reloadAudioPaused = true; }
            else if (!frozen && reloadAudioPaused)
            { reloadAudioSource.UnPause(); reloadAudioPaused = false; }
        }

        private void Update()
        {
            if (visual == null) return;
            if (animation != null) return;
            float dt = encounter.CombatDeltaTime;
            if (dt <= 0f) return;
            if (walking) walkAge += dt;
            recoil = Mathf.Max(0f, recoil - dt);
            float kick = recoil / 0.14f;
            Vector3 offset = new Vector3(-facing*.12f*kick,walking ? Mathf.Abs(Mathf.Sin(walkAge*9f))*.055f : 0f);
            visual.Pose(offset, facing * 1.8f * kick + (walking ? Mathf.Sin(walkAge*9f)*1.2f : 0f));
            if (recoil < 0.055f) spriteRenderer.sprite = idleSprite;
        }
        private void LateUpdate()
        {
            UpdateReloadSound(encounter != null && encounter.CombatDeltaTime <= 0f);
            if (animation != null) animation.Advance(encounter.CombatDeltaTime, walking && motionFrame == Time.frameCount, facing);
        }

        private void OnDisable() { StopReloadSound(); }

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
