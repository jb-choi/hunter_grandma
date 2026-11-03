using UnityEngine;

namespace HunterGrandma
{
    // One renderer per pooled zombie. Uses the actor's depth group and combat clock.
    internal sealed class ZombiePopVisual
    {
        private readonly SpriteRenderer renderer;
        private ZombieAnimationClip clip;
        private float age;
        private const float Delay = .09f;
        private const float DisplayScale = 2.4f;
        internal ZombiePopVisual(Transform parent, Material material)
        {
            var child = new GameObject("Party death pop", typeof(SpriteRenderer));
            child.transform.SetParent(parent, false);
            renderer = child.GetComponent<SpriteRenderer>(); renderer.sharedMaterial = material;
            renderer.sortingOrder = 20; Reset();
        }
        internal void Play(ZombieAnimationClip value, float size)
        {
            Reset(); if (value == null || value.frames == null || value.frames.Length == 0) return;
            clip = value; age = -Delay;
            renderer.transform.localPosition = Vector3.up * 1.35f * size;
            renderer.transform.localScale = Vector3.one * DisplayScale * size;
        }
        internal void Tick(float dt)
        {
            if (clip == null || dt <= 0) return;
            age += dt;
            if (age < 0) return;
            if (age >= clip.Duration) { Reset(); return; }
            renderer.sprite = clip.Sample(age, false); renderer.enabled = true;
        }
        internal void Reset() { clip = null; age = 0; renderer.enabled = false; renderer.sprite = null; }
    }
}
