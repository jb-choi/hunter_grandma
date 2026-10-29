using System.Collections.Generic;
using UnityEngine;

namespace HunterGrandma
{
    // One owner drives this sequence. The real actor stays at its saved foot position.
    internal sealed class GrandmaShadowSkill
    {
        public const float Interval = 10f;
        private const int ShotCount = 5;
        private enum Beat { Vanish, Appear, Fire, Depart, Return }
        private readonly GrandmaDefense owner;
        private readonly GrandmaShooter grandma;
        private readonly Camera camera;
        private readonly Transform root;
        private readonly BeltActorVisual actor;
        private readonly SundeokAnimationPlayer animation;
        private readonly SpriteRenderer shade, trace, flash, impact, highlight, ghost;
        private readonly SpriteRenderer[] smoke = new SpriteRenderer[8];
        private readonly List<ZombieActor> targets = new List<ZombieActor>(64);
        private ZombieActor target;
        private Beat beat;
        private float age, facing, originalFacing, cooldown = Interval;
        private int cursor, damage;
        private Vector2 origin;
        private Vector3 muzzle, head;

        public bool IsActive { get; private set; }
        public bool OwnsFrame { get; private set; }
        public float CooldownRemaining => cooldown;
        public int Shots { get; private set; }
        public int Activations { get; private set; }

        public GrandmaShadowSkill(GrandmaDefense owner, GrandmaShooter grandma, Camera camera, Sprite pixel, Sprite mist)
        {
            this.owner = owner; this.grandma = grandma; this.camera = camera;
            root = new GameObject("Grandma shadow skill").transform; root.SetParent(owner.transform, false);
            shade = Renderer("Night veil", pixel, 20000);
            var proxy = new GameObject("Shadow grandma", typeof(SpriteRenderer)); proxy.transform.SetParent(root, false);
            var source = proxy.GetComponent<SpriteRenderer>(); source.sprite = grandma.IdleSprite; source.sharedMaterial = grandma.ActorMaterial;
            actor = proxy.AddComponent<BeltActorVisual>(); actor.Initialize(source, mist, .8f);
            actor.Group.sortingOrder = 20020;
            // The cinematic ordering stays above the veil instead of participating in belt Y sorting.
            actor.enabled = false;
            if (grandma.AnimationSet != null && grandma.AnimationSet.IsReady) animation = new SundeokAnimationPlayer(grandma.AnimationSet, actor);
            ghost = Renderer("Teleport afterimage", grandma.IdleSprite, 20010);
            highlight = Renderer("Headshot target light", grandma.IdleSprite, 20030);
            trace = Renderer("Headshot trace", pixel, 20040);
            flash = Renderer("Muzzle flash", mist, 20041);
            impact = Renderer("Headshot spark", pixel, 20042);
            for (int i = 0; i < smoke.Length; i++) smoke[i] = Renderer("Shadow wisp " + i, mist, 20015);
            root.gameObject.SetActive(false);
        }

        private SpriteRenderer Renderer(string name, Sprite sprite, int order)
        {
            var obj = new GameObject(name, typeof(SpriteRenderer)); obj.transform.SetParent(root, false);
            var renderer = obj.GetComponent<SpriteRenderer>(); renderer.sprite = sprite;
            renderer.sharedMaterial = grandma.ActorMaterial; renderer.sortingOrder = order; return renderer;
        }

        public void Advance(float dt)
        {
            OwnsFrame = IsActive;
            if (!IsActive)
            {
                if (!owner.Progress.ShadowSkillEnabled || !owner.IsCombatPhase || dt <= 0) return;
                cooldown = Mathf.Max(0, cooldown - dt);
                if (cooldown > 0 || owner.CollectSkillTargets(targets, camera, grandma.FacingDirection) == 0) return;
                Begin(); OwnsFrame = true;
            }
            if (!owner.IsCombatPhase) { Cancel(); return; }
            if (dt <= 0) return;
            age += dt;
            while (IsActive && age >= Duration)
            {
                age -= Duration;
                Next();
            }
            if (IsActive) Render(dt);
        }

        private float Duration => beat == Beat.Vanish || beat == Beat.Return ? .22f : beat == Beat.Appear ? .07f : beat == Beat.Depart ? .08f : Shots == ShotCount ? .26f : .14f;

        private void Begin()
        {
            origin = grandma.FootPosition; originalFacing = facing = grandma.FacingDirection;
            damage = Mathf.Max(1, Mathf.RoundToInt(owner.Progress.Damage * 2f));
            Shots = cursor = 0; age = 0; beat = Beat.Vanish; IsActive = true; Activations++;
            cooldown = Interval; root.gameObject.SetActive(true);
            grandma.StopCombatPresentation(); grandma.SetPresentationVisible(false);
            animation?.Reset(); animation?.SetVisible(true);
            actor.transform.position = origin; actor.Body.sprite = grandma.IdleSprite;
            actor.Body.flipX = facing < 0;
            Fit(shade, new Vector3(camera.transform.position.x, camera.transform.position.y, 0), camera.orthographicSize * camera.aspect * 2.1f, camera.orthographicSize * 2.1f);
            Render(0);
        }

        private void Next()
        {
            if (beat == Beat.Vanish || beat == Beat.Depart)
            {
                if (Shots >= ShotCount || !SelectTarget())
                { beat = Beat.Return; actor.transform.position = origin; facing = originalFacing; animation?.StopCombat(); return; }
                beat = Beat.Appear; return;
            }
            if (beat == Beat.Appear)
            {
                if (target == null || !target.IsAlive) { beat = Beat.Depart; return; }
                beat = Beat.Fire; Shots++;
                animation?.Fire(false, facing, .30f);
                muzzle = actor.Display.TransformPoint(animation != null ? animation.MuzzleOffset : new Vector2(facing * 1.18f, 2.56f));
                head = target.HeadPoint;
                grandma.PlaySkillShot(Shots == ShotCount);
                target.ReceiveDamage(damage);
                return;
            }
            if (beat == Beat.Fire) { CopyGhost(); beat = Beat.Depart; return; }
            Finish();
        }

        private bool SelectTarget()
        {
            target = null;
            // Visit every candidate before revisiting a survivor when fewer than five are present.
            for (int i = 0; i < targets.Count; i++)
            {
                ZombieActor candidate = targets[cursor++ % targets.Count];
                if (candidate == null || !candidate.IsAlive) continue;
                // Do not mirror to the front when an obstacle or belt edge blocks the rear position.
                if (!owner.TrySkillFoot(candidate, out Vector2 free)) continue;
                target = candidate; facing = candidate.FacingDirection;
                actor.transform.position = free;
                animation?.StopCombat();
                animation?.SetAim(true, .07f, .30f);
                actor.Body.flipX = facing < 0;
                SpriteRenderer body = candidate.GetComponent<BeltActorVisual>().Body;
                highlight.sprite = body.sprite; highlight.flipX = body.flipX;
                highlight.transform.SetPositionAndRotation(body.transform.position, body.transform.rotation);
                highlight.transform.localScale = body.transform.lossyScale;
                return true;
            }
            return false;
        }

        private void Render(float dt)
        {
            float t = Mathf.Clamp01(age / Duration);
            float opacity = beat == Beat.Vanish ? 1 - t : beat == Beat.Appear || beat == Beat.Return ? t : beat == Beat.Depart ? 1 - t : 1;
            float darkness = beat == Beat.Vanish ? t : beat == Beat.Return ? 1 - t : 1;
            shade.color = new Color(.015f, .008f, .04f, darkness * .78f);
            animation?.Advance(dt, false, facing);
            actor.Body.color = beat == Beat.Vanish || beat == Beat.Depart ? new Color(.22f, .15f, .32f, opacity) : new Color(1, 1, 1, opacity);
            actor.Shadow.color = new Color(.02f, .01f, .04f, opacity * .6f);
            float kick = beat == Beat.Fire ? Mathf.Max(0, 1 - age / .11f) : 0;
            actor.Pose(new Vector3(-facing * .1f * kick, 0), facing * 3f * kick);
            bool shooting = beat == Beat.Fire;
            trace.enabled = flash.enabled = impact.enabled = shooting && age < .075f;
            highlight.enabled = shooting && age < .11f;
            highlight.color = new Color(1, .87f, .63f, Mathf.Max(0, 1 - age / .11f));
            if (shooting)
            {
                Vector3 delta = head - muzzle;
                Fit(trace, (head + muzzle) * .5f, delta.magnitude, Shots == ShotCount ? .065f : .035f);
                trace.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg);
                trace.color = new Color(1, .87f, .49f, 1 - Mathf.Clamp01(age / .075f));
                Fit(flash, muzzle, Shots == ShotCount ? .72f : .45f, .48f);
                flash.color = new Color(1, .91f, .61f, 1 - Mathf.Clamp01(age / .075f));
                Fit(impact, head, .16f, .16f); impact.transform.rotation = Quaternion.Euler(0, 0, 45);
                impact.color = new Color(1, .95f, .75f);
            }
            ghost.enabled = beat == Beat.Depart;
            ghost.color = new Color(.40f, .24f, .60f, (1 - t) * .4f);
            for (int i = 0; i < smoke.Length; i++)
            {
                float angle = i * 2.4f + t * 2;
                Vector3 position = actor.transform.position + new Vector3(Mathf.Cos(angle) * (.25f + t * .5f), .35f + i * .20f + t * .3f);
                Fit(smoke[i], position, .45f + i * .025f, .3f + t * .4f);
                smoke[i].enabled = !shooting;
                smoke[i].color = new Color(.07f, .025f, .12f, Mathf.Sin(t * Mathf.PI) * .75f);
            }
        }

        private void Fit(SpriteRenderer renderer, Vector3 position, float width, float height)
        {
            renderer.transform.SetPositionAndRotation(position, Quaternion.identity);
            Vector3 size = renderer.sprite.bounds.size;
            renderer.transform.localScale = new Vector3(width / size.x, height / size.y, 1);
        }
        private void CopyGhost()
        {
            ghost.sprite = actor.Body.sprite; ghost.flipX = actor.Body.flipX;
            ghost.transform.SetPositionAndRotation(actor.Body.transform.position, actor.Body.transform.rotation);
            ghost.transform.localScale = actor.Body.transform.lossyScale;
        }
        private void Finish()
        {
            IsActive = false; root.gameObject.SetActive(false);
            if (grandma != null) { grandma.StopCombatPresentation(); grandma.SetPresentationVisible(owner != null && !owner.IsOpening); }
            target = null; targets.Clear();
        }
        public void Cancel() { if (IsActive) Finish(); OwnsFrame = false; }
        public void Reset() { Cancel(); cooldown = Interval; Shots = Activations = 0; }
        public void Dispose() { Cancel(); if (root != null) Object.Destroy(root.gameObject); }
    }
}
