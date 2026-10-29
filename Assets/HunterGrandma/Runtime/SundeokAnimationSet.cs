using System;
using UnityEngine;

namespace HunterGrandma
{
    public enum SundeokAction { Idle, Walk, Reload, Aim, Fire, Collect }

    [Serializable]
    public sealed class SundeokAnimationFrame
    {
        public Sprite composite, body, upper;
        public float duration;
        // Pixel coordinates use the source canvas's top-left origin.
        public Vector2 muzzle, registration;
    }

    [Serializable]
    public sealed class SundeokAnimationClip
    {
        public SundeokAction action;
        public SundeokAnimationFrame[] frames;
        public float Duration
        {
            get { float result = 0; foreach (SundeokAnimationFrame frame in frames) result += frame.duration; return result; }
        }
        public int FrameAt(float time, bool loop)
        {
            float duration = Duration;
            time = loop && duration > 0 ? Mathf.Repeat(time, duration) : Mathf.Max(0, time);
            for (int i = 0; i < frames.Length - 1; i++) { if (time < frames[i].duration) return i; time -= frames[i].duration; }
            return frames.Length - 1;
        }
    }

    [CreateAssetMenu(menuName = "Hunter Grandma/Sundeok animation set")]
    public sealed class SundeokAnimationSet : ScriptableObject
    {
        public const float PixelsPerUnit = 32f;
        public static readonly Vector2 PivotPixels = new Vector2(60, 118);
        [SerializeField] private SundeokAnimationClip[] clips;
        public bool IsReady => clips != null && clips.Length == 6;
        public SundeokAnimationClip Clip(SundeokAction action) => clips[(int)action];
#if UNITY_EDITOR
        public void SetClips(SundeokAnimationClip[] value) { clips = value; }
#endif
    }
}
