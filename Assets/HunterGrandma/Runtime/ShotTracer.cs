using UnityEngine;

namespace HunterGrandma
{
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class ShotTracer : MonoBehaviour
    {
        [SerializeField, Min(1f)] private float speed = 38f;
        private DefenseTarget target;
        private GrandmaDefense encounter;
        private SpriteRenderer[] pellets;
        private Vector3 origin;
        private Vector2 footOrigin;
        private float facing, distance, range;
        private int damage;
        public bool IsFlying => gameObject.activeSelf;
        public void Launch(Vector3 muzzle, DefenseTarget victim, GrandmaDefense owner, int shotDamage)
        {
            target=victim;encounter=owner;damage=shotDamage;origin=muzzle;footOrigin=owner.GrandmaFootPosition;
            facing=victim.transform.position.x<footOrigin.x?-1f:1f;distance=0;
            range=BeltSpace.ClearRange(footOrigin,facing,owner.ShootingRange,owner.Obstacles);
            if(pellets==null)
            {
                var source=GetComponent<SpriteRenderer>();pellets=new SpriteRenderer[2];
                for(int i=0;i<2;i++)
                {
                    var obj=new GameObject("Shotgun pellet",typeof(SpriteRenderer));obj.transform.SetParent(transform,false);
                    pellets[i]=obj.GetComponent<SpriteRenderer>();pellets[i].sprite=source.sprite;pellets[i].sharedMaterial=source.sharedMaterial;pellets[i].color=source.color;
                    obj.transform.localScale=new Vector3(.7f,.7f,1);
                }
            }
            int order=BeltSpace.SortOrder(footOrigin.y)+2;GetComponent<SpriteRenderer>().sortingOrder=order;
            foreach(SpriteRenderer pellet in pellets)pellet.sortingOrder=order;
            transform.SetPositionAndRotation(origin,Quaternion.identity);gameObject.SetActive(true);
        }
        public void Cancel(){if(target!=null)target.ReleaseShot();target=null;gameObject.SetActive(false);}
        private void Update()
        {
            float dt=encounter==null?0:encounter.CombatDeltaTime;if(dt<=0)return;
            distance=Mathf.Min(range,distance+speed*dt);transform.position=origin+Vector3.right*(facing*distance);
            float spread=BeltSpace.Spread(distance);
            // Parent scale comes from the existing tracer prefab; compensate to express world-space spread.
            float verticalScale=Mathf.Max(.001f,transform.lossyScale.y);
            pellets[0].transform.localPosition=Vector3.up*(spread/verticalScale);pellets[1].transform.localPosition=Vector3.down*(spread/verticalScale);
            if(distance<range)return;
            if(target!=null)target.ReleaseShot();
            encounter.ResolveShotImpact(target,damage,footOrigin,facing,range);target=null;gameObject.SetActive(false);
        }
        private void OnDisable(){if(target!=null)target.ReleaseShot();target=null;}
    }
}
