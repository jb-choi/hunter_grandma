using UnityEngine;

namespace HunterGrandma
{
    // Presentation only. Shot timing, damage, rewards and root movement stay in GrandmaDefense.
    internal sealed class SundeokAnimationPlayer
    {
        private readonly SundeokAnimationSet data;
        private readonly BeltActorVisual visual;
        private readonly SpriteRenderer upper;
        private SundeokAction? special;
        private float specialTime, idleTime, walkTime, aimTime, cycleSpeed = 1, aimSpeed = 1, nextShot;
        private bool aiming, visible = true, collectPending, wasWalking;
        private int firedFrame = -1;
        public SundeokAction Action { get; private set; }
        public int FrameIndex { get; private set; }
        public int WalkFrameIndex { get; private set; }
        public bool IsLayered => upper.enabled;
        public bool IsCollecting => special == SundeokAction.Collect;
        public Vector2 MuzzleOffset { get; private set; }
        public float AnimationTime => special.HasValue ? specialTime : Action == SundeokAction.Walk ? walkTime : Action == SundeokAction.Aim ? aimTime : idleTime;

        public SundeokAnimationPlayer(SundeokAnimationSet data, BeltActorVisual visual)
        {
            this.data = data; this.visual = visual;
            var obj = new GameObject("120px upper body", typeof(SpriteRenderer)); obj.transform.SetParent(visual.Display, false);
            upper = obj.GetComponent<SpriteRenderer>(); upper.sharedMaterial = visual.Body.sharedMaterial; upper.sortingOrder = visual.Body.sortingOrder + 1;
            Reset();
        }
        public void Reset()
        {
            special = null; specialTime = idleTime = walkTime = aimTime = nextShot = 0;
            aiming = collectPending = wasWalking = false; firedFrame = -1; cycleSpeed = aimSpeed = 1;
            Render(false, 1);
        }
        public void SetVisible(bool value) { visible = value; visual.Body.enabled = value; if (!value) upper.enabled = false; }
        public void SetAim(bool value, float untilShot, float interval)
        {
            if (value && !aiming)
            {
                aimTime = 0;
                float preparation = data.Clip(SundeokAction.Fire).frames[0].duration / Mathf.Max(.1f, cycleSpeed);
                aimSpeed = Mathf.Max(1, data.Clip(SundeokAction.Aim).Duration / Mathf.Max(.05f, untilShot - preparation));
            }
            aiming = value; nextShot = untilShot;
            cycleSpeed = (data.Clip(SundeokAction.Fire).Duration + data.Clip(SundeokAction.Reload).Duration) / Mathf.Max(.05f, interval);
            if (aiming && IsCollecting) InterruptCollect();
        }
        public void Fire(bool walking, float facing, float interval)
        {
            cycleSpeed = (data.Clip(SundeokAction.Fire).Duration + data.Clip(SundeokAction.Reload).Duration) / Mathf.Max(.05f, interval);
            special = SundeokAction.Fire;
            // The first authored frame is shown just before the scheduled shot. Commit at the FX frame.
            specialTime = data.Clip(SundeokAction.Fire).frames[0].duration;
            firedFrame = Time.frameCount; Render(walking, facing);
        }
        public void QueueCollect() { collectPending = true; }
        public bool TryCollect()
        {
            if (IsCollecting) return true;
            if (!collectPending || aiming || special.HasValue) return false;
            collectPending = false; special = SundeokAction.Collect; specialTime = 0; return true;
        }
        public void InterruptCollect() { if (IsCollecting) { special = null; specialTime = 0; } }
        public void StopCombat()
        { special = null; aiming = collectPending = false; specialTime = 0; }
        public void Advance(float dt, bool walking, float facing)
        {
            if (dt <= 0) return;
            idleTime += dt; aimTime += dt * aimSpeed;
            if (walking) walkTime += dt; else if (wasWalking) walkTime = 0;
            wasWalking = walking;
            if (special.HasValue && firedFrame != Time.frameCount)
            {
                specialTime += dt * (IsCollecting ? 1 : cycleSpeed);
                if (specialTime >= data.Clip(special.Value).Duration)
                {
                    specialTime -= data.Clip(special.Value).Duration;
                    if (special == SundeokAction.Fire) special = SundeokAction.Reload;
                    else { special = null; specialTime = 0; }
                }
            }
            if (!walking && !aiming) TryCollect();
            Render(walking, facing);
        }
        private void Render(bool walking, float facing)
        {
            float time;
            if (special.HasValue) { Action = special.Value; time = specialTime; }
            else if (aiming && nextShot > 0 && nextShot <= data.Clip(SundeokAction.Fire).frames[0].duration / cycleSpeed)
            { Action = SundeokAction.Fire; time = data.Clip(SundeokAction.Fire).frames[0].duration - nextShot * cycleSpeed; }
            else if (aiming) { Action = SundeokAction.Aim; time = aimTime; }
            else { Action = walking ? SundeokAction.Walk : SundeokAction.Idle; time = walking ? walkTime : idleTime; }
            SundeokAnimationClip clip = data.Clip(Action);
            FrameIndex = clip.FrameAt(time, Action == SundeokAction.Idle || Action == SundeokAction.Walk);
            SundeokAnimationFrame frame = clip.frames[FrameIndex];
            bool layered = walking && (Action == SundeokAction.Aim || Action == SundeokAction.Fire || Action == SundeokAction.Reload);
            Vector2 offset = Vector2.zero;
            if (layered)
            {
                SundeokAnimationClip walk = data.Clip(SundeokAction.Walk); WalkFrameIndex = walk.FrameAt(walkTime, true);
                SundeokAnimationFrame body = walk.frames[WalkFrameIndex];
                offset = new Vector2(Mathf.Floor(body.registration.x - frame.registration.x + .5f), body.registration.y - frame.registration.y);
                visual.Body.sprite = body.body; upper.sprite = frame.upper;
            }
            else visual.Body.sprite = frame.composite;
            visual.Body.flipX = upper.flipX = facing < 0;
            upper.transform.localPosition = new Vector3(offset.x * facing, -offset.y) / SundeokAnimationSet.PixelsPerUnit;
            upper.enabled = visible && layered; visual.Body.enabled = visible;
            MuzzleOffset = new Vector2((frame.muzzle.x + offset.x - SundeokAnimationSet.PivotPixels.x) * facing, SundeokAnimationSet.PivotPixels.y - frame.muzzle.y - offset.y) / SundeokAnimationSet.PixelsPerUnit;
            visual.Pose(Vector3.zero);
        }
    }
}
