using UnityEngine;
using UnityEngine.Rendering;

namespace HunterGrandma
{
    public sealed class BeltActorVisual : MonoBehaviour
    {
        public Transform Display { get; private set; }
        public SpriteRenderer Body { get; private set; }
        public SpriteRenderer Shadow { get; private set; }
        public SortingGroup Group { get; private set; }
        private float baseScale = 1f;
        public void Initialize(SpriteRenderer source, Sprite shadowSprite, float scale)
        {
            if (Display != null) return;
            Group = gameObject.AddComponent<SortingGroup>();
            Display = new GameObject("Animated body").transform; Display.SetParent(transform, false);
            Body = Display.gameObject.AddComponent<SpriteRenderer>(); Body.sprite = source.sprite; Body.sharedMaterial = source.sharedMaterial; Body.sortingOrder = 10;
            source.enabled = false;
            var shadow = new GameObject("Foot shadow", typeof(SpriteRenderer)); shadow.transform.SetParent(transform, false);
            Shadow = shadow.GetComponent<SpriteRenderer>(); Shadow.sprite = shadowSprite; Shadow.sharedMaterial = source.sharedMaterial;
            Shadow.color = new Color(.035f, .03f, .05f, .45f); Shadow.sortingOrder = -20;
            SetScale(scale); RefreshDepth();
        }
        public void SetScale(float scale)
        {
            baseScale = scale; Display.localScale = Vector3.one * scale;
            Shadow.transform.localScale = new Vector3(1.6f * scale, .42f * scale, 1f);
        }
        public void Pose(Vector3 offset, float angle = 0f, float width = 1f, float height = 1f)
        {
            Display.localPosition = offset; Display.localRotation = Quaternion.Euler(0, 0, angle);
            Display.localScale = new Vector3(baseScale * width, baseScale * height, 1f);
        }
        public void RefreshDepth() { if (Group != null) Group.sortingOrder = BeltSpace.SortOrder(transform.position.y); }
        private void LateUpdate() { RefreshDepth(); }
    }
}
