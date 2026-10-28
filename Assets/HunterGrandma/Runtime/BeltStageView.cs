using UnityEngine;

namespace HunterGrandma
{
    public sealed class BeltStageView : MonoBehaviour
    {
        private GrandmaDefense owner;
        private Camera view;
        private Transform roadRoot;
        private Sprite pixel;
        private Material material;
        private float maxX;
        public Sprite ShadowSprite { get; private set; }
        private Texture2D shadowTexture;
        public void Initialize(GrandmaDefense encounter, Camera camera, SpriteRenderer background, Sprite dot, Material shared)
        {
            owner = encounter; view = camera; pixel = dot; material = shared;
            shadowTexture = new Texture2D(32, 12, TextureFormat.RGBA32, false) { filterMode = FilterMode.Point };
            for (int y=0;y<12;y++) for(int x=0;x<32;x++)
            { float r = Mathf.Pow((x-15.5f)/16f, 2) + Mathf.Pow((y-5.5f)/6f, 2); shadowTexture.SetPixel(x,y,new Color(1,1,1,Mathf.Clamp01((1f-r)*3f))); }
            shadowTexture.Apply(); ShadowSprite = Sprite.Create(shadowTexture, new Rect(0,0,32,12), Vector2.one*.5f, 32f);
            if (background != null)
            {
                float width = background.sprite.bounds.size.x * background.transform.lossyScale.x;
                for(int i=-1;i<4;i++)
                {
                    var tile = new GameObject("Village backdrop tile", typeof(SpriteRenderer)); tile.transform.SetParent(transform,false);
                    tile.transform.position = background.transform.position + Vector3.right * (i * width) + Vector3.up*2.05f;
                    tile.transform.localScale = background.transform.lossyScale;
                    var renderer = tile.GetComponent<SpriteRenderer>(); renderer.sprite=background.sprite; renderer.sharedMaterial=background.sharedMaterial; renderer.sortingOrder=-30;
                }
                background.enabled=false;
            }
        }
        public void Configure(float stageMaxX, Rect[] obstacles)
        {
            maxX = stageMaxX;
            if(roadRoot!=null) Destroy(roadRoot.gameObject);
            roadRoot = BuildRoad(transform, pixel, material, stageMaxX, obstacles);
            view.transform.position = new Vector3(0,0,-10);
        }
        // Editor preview and runtime use the same geometry so stopped scenes show the belt too.
        public static Transform BuildRoad(Transform parent, Sprite pixel, Material material, float stageMaxX, Rect[] obstacles)
        {
            float maxX = stageMaxX;
            Transform roadRoot;
            roadRoot = new GameObject("Walkable belt and cover").transform; roadRoot.SetParent(parent,false);
            float width = maxX - BeltSpace.MinX + 25f, center = (maxX+BeltSpace.MinX)*.5f;
            Box("Belt road", center, -2.45f, width, 3.55f, new Color(.25f,.27f,.26f), -18);
            Box("Far curb", center, -.68f, width, .16f, new Color(.65f,.62f,.49f), -17);
            Box("Near curb", center, -4.25f, width, .18f, new Color(.49f,.46f,.37f), -17);
            Box("Near pavement",center,-5.1f,width,1.5f,new Color(.20f,.21f,.20f),-18);
            for(int i=-14;i<maxX+16;i+=3)
            {
                Box("Lane dash",i,-2.45f,1.15f,.065f,new Color(.47f,.46f,.36f),-17);
                Box("Far pavement seam",i,-.45f,.045f,.35f,new Color(.38f,.39f,.33f),-17);
                if(i%2==0)Box("Near gravel",i,-4.55f,.22f,.09f,new Color(.38f,.36f,.30f),-17);
            }
            foreach(Rect cover in obstacles)
            {
                GameObject obj=Box("Solid cover",cover.center.x,cover.center.y,cover.width,1.75f,new Color(.31f,.38f,.34f),0);
                obj.transform.position = new Vector3(cover.center.x,cover.center.y+.65f);
                var group=new GameObject("Cover footprint",typeof(UnityEngine.Rendering.SortingGroup));group.transform.SetParent(roadRoot,false);
                group.transform.position=cover.center;group.GetComponent<UnityEngine.Rendering.SortingGroup>().sortingOrder=BeltSpace.SortOrder(cover.yMin);
                obj.transform.SetParent(group.transform,true);
                GameObject rim=Box("Cover lid",cover.center.x,cover.center.y+1.55f,cover.width+.15f,.16f,new Color(.50f,.52f,.39f),1);rim.transform.SetParent(group.transform,true);
                GameObject inset=Box("Recessed cover panel",cover.center.x,cover.center.y+.65f,cover.width*.78f,1.25f,new Color(.25f,.32f,.29f),1);inset.transform.SetParent(group.transform,true);
                for(int side=-1;side<=1;side+=2)
                {
                    GameObject rib=Box("Cover frame",cover.center.x+side*cover.width*.27f,cover.center.y+.65f,.055f,1.32f,new Color(.39f,.44f,.35f),2);rib.transform.SetParent(group.transform,true);
                    GameObject wheel=Box("Cover wheel",cover.center.x+side*cover.width*.32f,cover.center.y-.25f,.17f,.22f,new Color(.12f,.14f,.14f),2);wheel.transform.SetParent(group.transform,true);
                }
            }
            return roadRoot;
            GameObject Box(string name,float x,float y,float width,float height,Color color,int order)
            {
                var obj=new GameObject(name,typeof(SpriteRenderer));obj.transform.SetParent(roadRoot,false);obj.transform.position=new Vector3(x,y);obj.transform.localScale=new Vector3(width,height,1);
                var renderer=obj.GetComponent<SpriteRenderer>();renderer.sprite=pixel;renderer.sharedMaterial=material;renderer.color=color;renderer.sortingOrder=order;return obj;
            }
        }
        private void LateUpdate()
        {
            if(owner==null||owner.Progress==null)return;
            if(owner.AtYard){view.transform.position=new Vector3(0,0,-10);return;}
            float dt=owner.CombatDeltaTime;if(dt<=0)return;
            float half=view.orthographicSize*view.aspect;
            float target=Mathf.Clamp(owner.CameraFocusX+2.5f,BeltSpace.MinX+half-2f,maxX-half+2f);
            Vector3 position=view.transform.position;position.x=Mathf.Lerp(position.x,target,1f-Mathf.Exp(-dt*5f));view.transform.position=position;
        }
        private void OnDestroy(){if(ShadowSprite!=null)Destroy(ShadowSprite);if(shadowTexture!=null)Destroy(shadowTexture);}
    }
}
