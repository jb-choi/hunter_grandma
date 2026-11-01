using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HunterGrandma
{
    // The encounter owns phase rules; this object owns movement intent and its manual override timer.
    internal sealed class GrandmaMovement
    {
        private const float ManualSpeed = 3.6f;
        private const float ExplorationSpeed = 2.8f;
        private const float ManualOverrideDuration = 1.1f;
        private readonly GrandmaDefense encounter;
        private readonly GrandmaShooter grandma;
        private readonly List<ZombieActor> zombies;
        private float manualOverride;

        public GrandmaMovement(GrandmaDefense encounter, GrandmaShooter grandma, List<ZombieActor> zombies)
        {
            this.encounter = encounter;
            this.grandma = grandma;
            this.zombies = zombies;
        }

        public void Reset() => manualOverride = 0f;

        public void Advance(float dt)
        {
            if (!encounter.CanMoveOnBelt) return;

            Keyboard keyboard = Keyboard.current;
            if (keyboard != null && keyboard.tabKey.wasPressedThisFrame) encounter.ToggleAutoExplore();
            Vector2 input = ReadMovement(keyboard);
            Vector2 position = grandma.FootPosition;
            Vector2 next = position;

            if (input.sqrMagnitude > .01f)
            {
                grandma.InterruptCollect();
                manualOverride = ManualOverrideDuration;
                next = encounter.MoveActor(position, Vector2.ClampMagnitude(input, 1f) * ManualSpeed * dt);
            }
            else
            {
                manualOverride = Mathf.Max(0f, manualOverride - dt);
                if (encounter.AutoExplore && manualOverride <= 0f)
                {
                    DefenseTarget target = NearestTarget(position);
                    if (target == null && grandma.TryCollect())
                    {
                        grandma.MoveOnBelt(position);
                        return;
                    }

                    Vector2 goal = ExplorationGoal(position, target);
                    goal = BeltSpace.Clamp(goal, encounter.StageMaxX);
                    goal = BeltSpace.Waypoint(position, goal, encounter.Obstacles);
                    Vector2 delta = Vector2.MoveTowards(position, goal, ExplorationSpeed * dt) - position;
                    next = encounter.MoveActor(position, delta);
                }
            }

            next.x = Mathf.Min(next.x, encounter.WaveAnchor + 12f);
            grandma.MoveOnBelt(next);
        }

        private static Vector2 ReadMovement(Keyboard keyboard)
        {
            if (keyboard == null) return Vector2.zero;
            return new Vector2(
                (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed ? 1 : 0) -
                (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed ? 1 : 0),
                (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed ? 1 : 0) -
                (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed ? 1 : 0));
        }

        private DefenseTarget NearestTarget(Vector2 position)
        {
            DefenseTarget target = null;
            float best = float.MaxValue;
            foreach (ZombieActor zombie in zombies)
            {
                if (!zombie.IsAlive) continue;
                Vector2 delta = (Vector2)zombie.transform.position - position;
                float score = Mathf.Abs(delta.x) + Mathf.Abs(delta.y) * 2f;
                if (score >= best) continue;
                best = score;
                target = zombie;
            }
            return target != null ? target : encounter.Boss.IsAlive ? encounter.Boss : null;
        }

        private Vector2 ExplorationGoal(Vector2 position, DefenseTarget target)
        {
            if (target == null)
            {
                bool nextWave = encounter.Phase == DefensePhase.WaveBreak && encounter.WaveNumber < encounter.CurrentStage.WaveCount;
                float x = nextWave ? encounter.WaveNumber * 10f - 4f : encounter.WaveAnchor - 4f;
                return new Vector2(x, BeltSpace.CenterDepth);
            }

            Vector2 foot = target.transform.position;
            float sign = foot.x < position.x ? -1f : 1f;
            float range = Mathf.Abs(foot.x - position.x);
            Vector2 goal = new Vector2(position.x, foot.y);
            if (range > encounter.ShootingRange * .78f) goal.x = foot.x - sign * encounter.ShootingRange * .65f;
            else if (range < 2.5f) goal.x = foot.x - sign * 3.2f;
            if (!encounter.CanShoot(target) && Mathf.Abs(foot.y - position.y) < .15f) goal.x = foot.x - sign * 2f;
            return goal;
        }
    }
}
