using System;
using UnityEngine;

namespace HunterGrandma
{
    public enum ZombieKind { Normal, Infected, Large }
    public enum ZombieAction { Idle, Walk, Run, Attack, Dead }

    [Serializable] public sealed class ZombieAnimationClip
    {
        public Sprite[] frames;
        public float[] durations;
        public float Duration { get { float total = 0; foreach (float value in durations) total += value; return total; } }
        public Sprite Sample(float time, bool loop)
        {
            time = loop ? Mathf.Repeat(time, Duration) : Mathf.Clamp(time, 0, Duration);
            for (int i = 0; i < frames.Length; i++) { time -= durations[i]; if (time < 0) return frames[i]; }
            return frames[frames.Length - 1];
        }
    }
    [Serializable] public sealed class ZombieAnimationVariant
    {
        public ZombieKind kind;
        public ZombieAnimationClip[] clips;
    }
    public sealed class ZombieAnimationSet : ScriptableObject
    {
        public const float PixelsPerUnit = 32f;
        [SerializeField] private ZombieAnimationVariant[] variants;
        public bool IsReady
        {
            get
            {
                if (variants == null || variants.Length != 3) return false;
                for (int k = 0; k < variants.Length; k++)
                {
                    if (variants[k] == null || (int)variants[k].kind != k || variants[k].clips == null || variants[k].clips.Length != 5) return false;
                    foreach (ZombieAnimationClip clip in variants[k].clips)
                    {
                        if (clip == null || clip.frames == null || clip.durations == null || clip.frames.Length == 0 || clip.frames.Length != clip.durations.Length) return false;
                        for (int f = 0; f < clip.frames.Length; f++) if (clip.frames[f] == null || clip.durations[f] <= 0) return false;
                    }
                }
                return true;
            }
        }
        public void SetVariants(ZombieAnimationVariant[] value) { variants = value; }
        public ZombieAnimationClip Clip(ZombieKind kind, ZombieAction action) => variants[(int)kind].clips[(int)action];
        // Preserve the established windup/strike/recovery and .44-second damage beat.
        public Sprite Attack(ZombieKind kind, float age)
        {
            ZombieAnimationClip clip = Clip(kind, ZombieAction.Attack);
            float time = age < .32f ? age / .32f * .21f : age < .50f ? .21f + (age - .32f) / .18f * .15f : .36f + Mathf.Clamp01((age - .50f) / .35f) * .24f;
            return clip.Sample(time, false);
        }
    }
}
