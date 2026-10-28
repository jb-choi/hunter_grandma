using UnityEngine;

namespace HunterGrandma
{
    // Lightweight district landmarks share the existing village art and sprite material.
    public sealed class DefenseScenery : MonoBehaviour
    {
        private readonly GameObject[] places = new GameObject[8];
        private Sprite pixel;
        private Material material;
        private int current = -1;
        public void Initialize(Sprite sprite, Material shared)
        {
            pixel = sprite; material = shared;
            Color wood = new Color(.33f, .23f, .17f), pale = new Color(.52f, .52f, .45f), green = new Color(.22f, .37f, .31f);
            for (int i = 0; i < places.Length; i++)
            {
                places[i] = new GameObject(i == 7 ? "Grandma home yard" : "District landmarks " + i);
                places[i].transform.SetParent(transform, false);
                if (i == 0 || i == 7)
                {
                    for (int n = 0; n < 22; n++) Box(i, -11f + n, -1.95f, .14f, 1.4f, wood);
                    Box(i, 0, -1.65f, 24, .13f, wood); Box(i, 0, -2.15f, 24, .13f, wood);
                    if (i == 7) { Box(i, 8, .2f, 4.5f, 5.5f, pale); Box(i, 8, -.5f, 1.25f, 3.8f, wood); Box(i, 6.7f, 1.1f, 1.1f, 1.4f, green); }
                }
                if (i == 1 || i == 3)
                {
                    for (int n = 0; n < 4; n++)
                    {
                        float x = 5f + n * 1.5f, y = -1.8f + n % 2 * .6f;
                        Box(i, x, y, 1.35f, 1.3f + n % 2 * 1.2f, wood);
                        Box(i, x, y, .1f, 1.3f + n % 2 * 1.2f, pale);
                    }
                }
                if (i == 2)
                {
                    Box(i, 7, .1f, 6.5f, 5f, pale); Box(i, 7, -.5f, 4.5f, 2.5f, green);
                    for (int n = 0; n < 9; n++) Box(i, 4.2f + n * .7f, 1.8f, .7f, .8f, n % 2 == 0 ? green : pale);
                }
                if (i == 4)
                {
                    for (int n = 0; n < 2; n++) { float x = n == 0 ? -9f : 9f; Box(i, x, .1f, .16f, 5.5f, wood); Box(i, x, 2.9f, .8f, .45f, new Color(.83f, .66f, .38f)); }
                    Box(i, 5.5f, -2.2f, 2.5f, .3f, pale);
                }
                if (i == 5)
                {
                    Box(i, 7, -1.5f, 4, .15f, pale);
                    for (int n = 0; n < 2; n++) { Box(i, 5.7f + n * 2.6f, -1.5f, .4f, 1.6f, wood); Box(i, 5.3f + n * 3.4f, -1.5f, .25f, 1.1f, green); }
                }
                if (i == 6)
                {
                    Box(i, 7.5f, .4f, 5.7f, 5.5f, pale); Box(i, 7.5f, -.8f, 2.2f, 3, green);
                    Color cross = new Color(.59f, .25f, .26f); Box(i, 7.5f, 2f, 1.5f, .45f, cross); Box(i, 7.5f, 2f, .45f, 1.5f, cross);
                }
                places[i].SetActive(false);
            }
        }
        private void Box(int place, float x, float y, float width, float height, Color color)
        {
            var obj = new GameObject("Landmark detail", typeof(SpriteRenderer)); obj.transform.SetParent(places[place].transform, false);
            obj.transform.localPosition = new Vector3(x, y); obj.transform.localScale = new Vector3(width, height, 1);
            SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>(); renderer.sprite = pixel; renderer.sharedMaterial = material; renderer.color = color; renderer.sortingOrder = -10;
        }
        public void Show(int stage, bool yard)
        {
            int next = yard ? 7 : stage; if (current == next) return;
            if (current >= 0) places[current].SetActive(false);
            places[next].SetActive(true); current = next;
        }
    }
}
