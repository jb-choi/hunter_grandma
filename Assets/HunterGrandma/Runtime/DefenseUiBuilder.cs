using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HunterGrandma
{
    // HUD and modal views share one borrowed font and the same uGUI defaults.
    internal sealed class DefenseUiBuilder
    {
        private readonly Font font;

        public DefenseUiBuilder(Font font) { this.font = font; }

        public RectTransform Rect(Transform parent, string name, Vector2 min, Vector2 max)
        {
            var obj = new GameObject(name, typeof(RectTransform));
            obj.transform.SetParent(parent, false);
            RectTransform rect = obj.GetComponent<RectTransform>();
            rect.anchorMin = min;
            rect.anchorMax = max;
            rect.offsetMin = rect.offsetMax = Vector2.zero;
            return rect;
        }

        public RectTransform Stretch(Transform parent, string name, float inset)
        {
            RectTransform rect = Rect(parent, name, Vector2.zero, Vector2.one);
            rect.offsetMin = Vector2.one * inset;
            rect.offsetMax = -Vector2.one * inset;
            return rect;
        }

        public RectTransform Fixed(Transform parent, string name, float x, float y, float width, float height)
        {
            RectTransform rect = Rect(parent, name, Vector2.up, Vector2.up);
            rect.pivot = Vector2.up;
            rect.anchoredPosition = new Vector2(x, y);
            rect.sizeDelta = new Vector2(width, height);
            return rect;
        }

        public Image Paint(RectTransform rect, Color color, bool raycast = false)
        {
            Image image = rect.gameObject.AddComponent<Image>();
            image.color = color;
            image.raycastTarget = raycast;
            return image;
        }

        public GameObject Panel(Transform parent, string name, Vector2 min, Vector2 max, Vector2 low, Vector2 high, Color color)
        {
            RectTransform rect = Rect(parent, name, min, max);
            rect.offsetMin = low;
            rect.offsetMax = high;
            Paint(rect, color);
            return rect.gameObject;
        }

        public void Click(RectTransform rect, UnityAction action)
        {
            Button button = rect.gameObject.AddComponent<Button>();
            button.targetGraphic = rect.GetComponent<Image>();
            button.onClick.AddListener(action);
            Navigation navigation = button.navigation;
            navigation.mode = Navigation.Mode.None;
            button.navigation = navigation;
        }

        public Text Label(RectTransform rect, string value, int size, Color color, TextAnchor alignment = TextAnchor.UpperLeft)
        {
            Text text = rect.gameObject.AddComponent<Text>();
            text.font = font;
            text.text = value;
            text.fontSize = size;
            text.color = color;
            text.alignment = alignment;
            text.raycastTarget = false;
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            return text;
        }

        public Text TextAt(Transform parent, string value, int size, Vector2 position, Vector2 dimensions, Color color, bool right = false, bool bottom = false)
        {
            Vector2 anchor = new Vector2(right ? 1 : 0, bottom ? 0 : 1);
            RectTransform rect = Rect(parent, value + " label", anchor, anchor);
            rect.pivot = new Vector2(0, bottom ? 0 : 1);
            rect.anchoredPosition = position;
            rect.sizeDelta = dimensions;
            return Label(rect, value, size, color);
        }

        public Text MakeButton(Transform parent, string name, Vector2 offset, Vector2 size, UnityAction action, bool right = false, bool top = false)
        {
            Vector2 anchor = new Vector2(right ? 1 : 0, top ? 1 : 0);
            GameObject obj = Panel(parent, name, anchor, anchor, offset, offset + size, new Color(.22f, .29f, .28f));
            obj.GetComponent<Image>().raycastTarget = true;
            Click(obj.GetComponent<RectTransform>(), action);
            Text label = TextAt(obj.transform, name, 15, Vector2.zero, Vector2.zero, new Color(.96f, .91f, .79f));
            RectTransform rect = label.rectTransform;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = new Vector2(4, 2);
            rect.offsetMax = new Vector2(-4, -2);
            label.alignment = TextAnchor.MiddleCenter;
            return label;
        }
    }
}
