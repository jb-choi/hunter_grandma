using UnityEngine;

namespace HunterGrandma
{
    public abstract class DefenseTarget : MonoBehaviour
    {
        public abstract bool IsAlive { get; }
        public abstract Vector3 HitPoint { get; }
        public bool HasIncomingShot { get; private set; }
        public bool ReserveShot()
        {
            if (!IsAlive || HasIncomingShot) return false;
            HasIncomingShot = true; return true;
        }
        public void ReleaseShot() { HasIncomingShot = false; }
        public abstract void ReceiveDamage(int damage);
    }
}
