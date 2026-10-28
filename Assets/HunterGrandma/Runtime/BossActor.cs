using UnityEngine;

namespace HunterGrandma
{
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class BossActor : DefenseTarget
    {
        private GrandmaDefense encounter;
        private SpriteRenderer body, aura;
        private BeltActorVisual visual;
        private Transform prop, shoppingCart;
        private DefenseStage stage;
        private float age, cycle, flash;
        private int health, heals;
        private bool subdued, carried, attacked, cycleStarted;
        private Vector2 basePosition, attackAim, attackStart;
        public int Health => health;
        public int PatternAttackCount { get; private set; }
        public int HealUses => heals;
        public int MaxHealth => stage == null ? 1 : stage.BossHealth;
        public override bool IsAlive => gameObject.activeSelf && !subdued && encounter != null && encounter.Phase == DefensePhase.BossFight;
        public override Vector3 HitPoint => transform.position + Vector3.up * 2.05f;
        public bool IsSubdued => subdued;
        public string Appearance { get; private set; }
        public string Telegraph { get; private set; }
        public void Initialize(GrandmaDefense owner, Sprite pixel, Sprite mistSprite, Material material)
        {
            encounter=owner;visual=gameObject.AddComponent<BeltActorVisual>();visual.Initialize(GetComponent<SpriteRenderer>(),owner.ShadowSprite,1.12f);body=visual.Body;
            var mist=new GameObject("Host infection aura",typeof(SpriteRenderer));mist.transform.SetParent(visual.Display,false);
            aura=mist.GetComponent<SpriteRenderer>();aura.sprite=mistSprite;aura.sharedMaterial=material;aura.color=new Color(.31f,.12f,.42f,.20f);aura.sortingOrder=0;
            mist.transform.localPosition=new Vector3(0,1.6f);mist.transform.localScale=new Vector3(2.7f,3f,1);
            prop=new GameObject("Occupation prop",typeof(SpriteRenderer)).transform;prop.SetParent(visual.Display,false);
            SpriteRenderer accessory=prop.GetComponent<SpriteRenderer>();accessory.sprite=pixel;accessory.sharedMaterial=material;accessory.sortingOrder=14;
            shoppingCart=new GameObject("Shopping cart frame").transform;shoppingCart.SetParent(visual.Display,false);
            void Bar(float x,float y,float w,float h,Color color,Sprite sprite)
            {
                var part=new GameObject("Cart detail",typeof(SpriteRenderer));part.transform.SetParent(shoppingCart,false);part.transform.localPosition=new Vector3(x,y);part.transform.localScale=new Vector3(w,h,1);
                var renderer=part.GetComponent<SpriteRenderer>();renderer.sprite=sprite;renderer.sharedMaterial=material;renderer.sortingOrder=15;renderer.color=color;
            }
            Color metal=new Color(.57f,.69f,.66f),wheel=new Color(.16f,.18f,.20f);
            Bar(-.95f,.7f,1.9f,.12f,metal,pixel);Bar(-.95f,.12f,1.9f,.12f,metal,pixel);
            for(int n=0;n<5;n++)Bar(-1.8f+n*.43f,.42f,.08f,.6f,metal,pixel);
            Bar(.02f,.62f,.1f,1.25f,metal,pixel);Bar(.2f,1.18f,.45f,.1f,metal,pixel);
            Bar(-1.6f,-.04f,.3f,.3f,wheel,mistSprite);Bar(-.25f,-.04f,.3f,.3f,wheel,mistSprite);
        }
        public void Spawn(DefenseStage definition,Sprite sprite,int previousCaptures)
        {
            transform.SetParent(encounter.transform,false);stage=definition;health=definition.BossHealth;age=cycle=flash=0;heals=0;PatternAttackCount=0;
            subdued=carried=attacked=cycleStarted=false;ReleaseShot();
            basePosition=encounter.FreeFootPosition(new Vector2(encounter.WaveAnchor+3f,BeltSpace.CenterDepth+(definition.Id%3-1)*.55f));
            transform.SetPositionAndRotation(new Vector3(encounter.WaveAnchor+12f,basePosition.y),Quaternion.identity);transform.localScale=Vector3.one;
            visual.SetScale(1.12f);visual.Pose(Vector3.zero);visual.Group.enabled=true;visual.Shadow.enabled=true;visual.RefreshDepth();
            body.sprite=sprite;body.color=previousCaptures%2==0?Color.white:new Color(.87f,.94f,1);body.flipX=definition.Pattern==BossPattern.BarkDash;
            aura.enabled=true;prop.gameObject.SetActive(true);shoppingCart.gameObject.SetActive(definition.Pattern==BossPattern.CartRush);
            SpriteRenderer accessory=prop.GetComponent<SpriteRenderer>();accessory.color=previousCaptures%2==0?new Color(.93f,.63f,.22f):new Color(.34f,.70f,.84f);
            Appearance=previousCaptures%2==0?"주황 소품":"푸른 소품";
            if(definition.Id==0){Appearance=previousCaptures==0?"콩이 · 옆집 강아지":"동네 강아지 · "+Appearance;prop.localPosition=new Vector3(-.15f,1.05f);prop.localScale=new Vector3(.75f,.13f,1);}
            else if(definition.Id==1||definition.Id==3){prop.localPosition=new Vector3(-.8f,1.1f);prop.localScale=new Vector3(1.1f,1,1);}
            else if(definition.Id==2){prop.localPosition=new Vector3(-.85f,.35f);prop.localScale=new Vector3(1.6f,.6f,1);accessory.color=new Color(accessory.color.r,accessory.color.g,accessory.color.b,.35f);}
            else{prop.localPosition=new Vector3(0,3.18f);prop.localScale=new Vector3(.95f,.12f,1);}
            Telegraph="거대 숙주형 감염체 등장";gameObject.SetActive(true);
        }
        public void Despawn(){ReleaseShot();subdued=carried=false;transform.SetParent(encounter.transform,false);gameObject.SetActive(false);}
        public override void ReceiveDamage(int damage)
        {
            if(!IsAlive||damage<=0)return;
            float multiplier=stage.Pattern==BossPattern.GuardSlam&&cycle<1.8f?.30f:stage.Pattern==BossPattern.GuardSlam&&cycle>3f?1.4f:1;
            health=Mathf.Max(0,health-Mathf.Max(1,Mathf.RoundToInt(damage*multiplier)));flash=.1f;
            if(health>0)return;
            subdued=true;ReleaseShot();Telegraph="제압 성공 · 사격 중지";aura.enabled=false;
            visual.Pose(Vector3.zero,stage.Id==0?-92f:-62f);encounter.OnBossSubdued();
        }
        public void LoadOnto(Transform cart)
        {
            carried=true;transform.SetParent(cart,true);transform.localPosition=new Vector3(.35f,.8f);transform.localRotation=Quaternion.identity;transform.localScale=Vector3.one;
            visual.SetScale(.95f);visual.Pose(Vector3.zero,stage.Id==0?-92f:-62f);visual.Group.enabled=false;visual.Shadow.enabled=false;
        }
        private void Update()
        {
            if(encounter==null||carried||subdued)return;
            float dt=encounter.CombatDeltaTime;if(dt<=0)return;
            age+=dt;flash=Mathf.Max(0,flash-dt);
            if(encounter.Phase==DefensePhase.BossArrival)
            {
                Vector2 waypoint=BeltSpace.Waypoint(transform.position,basePosition,encounter.Obstacles);
                transform.position=encounter.MoveActor(transform.position,Vector2.MoveTowards(transform.position,waypoint,3f*dt)-(Vector2)transform.position);return;
            }
            if(!IsAlive)return;
            cycle+=dt;float period=stage.Pattern==BossPattern.CartRush?4.6f:5.6f;
            if(cycle>=period){cycle-=period;attacked=cycleStarted=false;}
            if(!cycleStarted){cycleStarted=true;attackAim=encounter.GrandmaFootPosition;attackStart=transform.position;}
            Vector2 position=transform.position;float angle=0,jump=0;
            if(cycle<1.7f)
            {
                Telegraph=stage.Pattern==BossPattern.BarkDash?(cycle<.8f?"왕!":"왕! 왕! · 돌진 준비"):
                    stage.Pattern==BossPattern.GuardSlam?"짐으로 방어 중":stage.Pattern==BossPattern.SpinDance?"하나, 둘 · 회전 준비":
                    stage.Pattern==BossPattern.Leap?"몸을 낮춘다 · 도약 준비":stage.Pattern==BossPattern.HealSummon?"자가 진료 · 회복 준비":"짐을 내린다 · 돌진 준비";
                jump=-Mathf.Clamp01(cycle/1.7f)*.12f;
                if(stage.Pattern==BossPattern.BarkDash)angle=Mathf.Sin(cycle*16)*3;
            }
            else if(cycle<2.7f)
            {
                float action=Mathf.Clamp01(cycle-1.7f),reach=Mathf.Sin(action*Mathf.PI);
                Telegraph=stage.Pattern==BossPattern.SpinDance?"회전 이동!":stage.Pattern==BossPattern.Leap?"도약!":stage.Pattern==BossPattern.HealSummon?"외계체 호출":stage.Pattern==BossPattern.GuardSlam?"내려친다!":"돌진!";
                if(stage.Pattern==BossPattern.SpinDance){position=Vector2.Lerp(attackStart,attackAim,reach);position.y+=Mathf.Sin(action*Mathf.PI*4)*.22f;angle=action*720;}
                else if(stage.Pattern==BossPattern.Leap){position=Vector2.Lerp(attackStart,attackAim,Mathf.SmoothStep(0,1,action*1.4f));jump=reach*1.6f;}
                else if(stage.Pattern==BossPattern.GuardSlam){position=Vector2.Lerp(attackStart,attackAim,reach*.45f);angle=-reach*12f;}
                else if(stage.Pattern!=BossPattern.HealSummon)position=Vector2.Lerp(attackStart,attackAim,reach);
                if(!attacked&&action>=(stage.Pattern==BossPattern.Leap?.9f:.5f))
                {
                    attacked=true;PatternAttackCount++;
                    if(stage.Pattern==BossPattern.HealSummon)
                    {if(heals<2){health=Mathf.Min(MaxHealth,health+Mathf.RoundToInt(MaxHealth*.08f));heals++;encounter.SummonBossMinions();}else Telegraph="회복 소진 · 빈틈";}
                    else
                    {
                        Vector2 appliedFoot=encounter.MoveActor(transform.position,position-(Vector2)transform.position);
                        Vector2 delta=encounter.GrandmaFootPosition-appliedFoot;
                        float reachX=stage.Pattern==BossPattern.GuardSlam?2.5f:stage.Pattern==BossPattern.SpinDance?2.1f:1.7f;
                        if(Mathf.Abs(delta.x)<reachX&&Mathf.Abs(delta.y)<.7f)encounter.DamageDefense(8+stage.Id*2);
                    }
                }
            }
            else
            {
                Telegraph="빈틈 · 자동 사격";
                Vector2 waypoint=BeltSpace.Waypoint(position,basePosition,encounter.Obstacles);position=Vector2.MoveTowards(position,waypoint,dt*4f);
            }
            transform.position=encounter.MoveActor(transform.position,position-(Vector2)transform.position);
            visual.Pose(Vector3.up*jump,angle);
            body.color=flash>0?new Color(1,.6f,.65f):new Color(.93f,.90f,.98f);
            if(stage.Id!=0)body.flipX=encounter.GrandmaX>transform.position.x;
            aura.color=new Color(.31f,.12f,.42f,.14f+Mathf.Sin(age*4)*.05f);
            prop.localRotation=Quaternion.Euler(0,0,stage.Pattern==BossPattern.GuardSlam&&cycle<1.7f?-12f:0);
        }
    }
}

