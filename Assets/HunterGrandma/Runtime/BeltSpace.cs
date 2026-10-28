using UnityEngine;

namespace HunterGrandma
{
    // All gameplay coordinates are feet on the belt, independent of sprite height or jump poses.
    public static class BeltSpace
    {
        public const float MinDepth = -4f, MaxDepth = -.9f, CenterDepth = -2.45f, MinX = -10f;
        public static readonly Vector2 Start = new Vector2(-6f, CenterDepth);
        public static Vector2 Clamp(Vector2 position, float maxX)
            => new Vector2(Mathf.Clamp(position.x, MinX, maxX), Mathf.Clamp(position.y, MinDepth, MaxDepth));
        public static int SortOrder(float footY) => 1000 - Mathf.RoundToInt(footY * 100f);
        public static float Spread(float distance) => .10f + Mathf.Max(0f, distance) * .06f;
        public static float ClearRange(Vector2 origin, float facing, float range, Rect[] obstacles)
        {
            if(obstacles==null)return range;
            foreach(Rect obstacle in obstacles)
            {
                float distance=(facing>0?obstacle.xMin-origin.x:origin.x-obstacle.xMax);
                if(distance>=0&&origin.y>=obstacle.yMin&&origin.y<=obstacle.yMax)range=Mathf.Min(range,distance);
            }
            return range;
        }
        public static bool CanHit(Vector2 origin, Vector2 target, float facing, float range, Rect[] obstacles)
        {
            float forward = (target.x - origin.x) * facing;
            return forward >= -.25f && forward <= range && Mathf.Abs(target.y - origin.y) <= Spread(forward) + .35f && !Blocked(origin, target, obstacles);
        }
        public static bool Blocked(Vector2 from, Vector2 to, Rect[] obstacles)
        {
            if (obstacles == null) return false;
            foreach (Rect rect in obstacles) if (Intersects(from, to, rect)) return true;
            return false;
        }
        private static bool Intersects(Vector2 from, Vector2 to, Rect rect)
        {
            Vector2 delta = to - from; float low = 0f, high = 1f;
            for (int axis = 0; axis < 2; axis++)
            {
                float d = delta[axis], start = from[axis], min = axis == 0 ? rect.xMin : rect.yMin, max = axis == 0 ? rect.xMax : rect.yMax;
                if (Mathf.Abs(d) < .0001f) { if (start < min || start > max) return false; }
                else { float a = (min - start) / d, b = (max - start) / d; low = Mathf.Max(low, Mathf.Min(a, b)); high = Mathf.Min(high, Mathf.Max(a, b)); if (low > high) return false; }
            }
            return high >= 0f && low <= 1f;
        }
        public static Vector2 Waypoint(Vector2 from, Vector2 destination, Rect[] obstacles)
        {
            if (obstacles == null) return destination;
            foreach (Rect obstacle in obstacles)
            {
                Rect rect = new Rect(obstacle.xMin - .3f, obstacle.yMin - .3f, obstacle.width + .6f, obstacle.height + .6f);
                if (!Intersects(from, destination, rect)) continue;
                float above = Mathf.Min(MaxDepth, rect.yMax + .12f), below = Mathf.Max(MinDepth, rect.yMin - .12f);
                float y = Mathf.Abs(from.y - above) < Mathf.Abs(from.y - below) ? above : below;
                if (Mathf.Abs(from.y - y) > .08f) return new Vector2(from.x, y);
                return new Vector2(destination.x > from.x ? rect.xMax + .15f : rect.xMin - .15f, y);
            }
            return destination;
        }
        public static Vector2 Move(Vector2 from, Vector2 delta, float maxX, Rect[] obstacles, float radius = .24f)
        {
            Vector2 next = Clamp(from + delta, maxX);
            if (obstacles == null) return next;
            foreach (Rect obstacle in obstacles)
            {
                Rect rect = new Rect(obstacle.xMin - radius, obstacle.yMin - radius, obstacle.width + radius * 2, obstacle.height + radius * 2);
                if (from.y > rect.yMin && from.y < rect.yMax)
                {
                    if (from.x <= rect.xMin && next.x > rect.xMin) next.x = rect.xMin;
                    if (from.x >= rect.xMax && next.x < rect.xMax) next.x = rect.xMax;
                }
                if (next.x > rect.xMin && next.x < rect.xMax)
                {
                    if (from.y <= rect.yMin && next.y > rect.yMin) next.y = rect.yMin;
                    if (from.y >= rect.yMax && next.y < rect.yMax) next.y = rect.yMax;
                }
            }
            return next;
        }
    }
}
