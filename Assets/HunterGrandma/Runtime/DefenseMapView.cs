using UnityEngine;
using UnityEngine.UI;

namespace HunterGrandma
{
    // A local modal view, following project09's map-pin and detail-panel composition.
    internal sealed class DefenseMapView
    {
        private readonly RectTransform canvasRoot;
        private readonly RectTransform root;
        private readonly RectTransform window;
        private readonly RectTransform mapFrame;
        private readonly RectTransform details;
        private readonly RectTransform mapPicture;
        private readonly GrandmaDefense encounter;
        private readonly DefenseUiBuilder ui;
        private readonly Image[] pins = new Image[DefenseRegions.Count];
        private readonly Text[] pinLabels = new Text[DefenseRegions.Count];
        private readonly RectTransform home;
        private readonly Image homeImage;
        private readonly Text homeLabel;
        private readonly Button travelButton;
        private readonly Text currentLocation;
        private readonly Text selectionHeading;
        private readonly Text selectedTitle;
        private readonly Text description;
        private readonly Text destinationCode;
        private readonly Text travelCaption;
        private readonly Text instructions;
        private readonly RectTransform divider;
        private readonly Color ink = new Color(0.19f, 0.23f, 0.24f);
        private readonly Color paper = new Color(0.89f, 0.87f, 0.79f);
        private readonly Color gold = new Color(0.93f, 0.73f, 0.34f);
        private readonly Color green = new Color(0.28f, 0.41f, 0.34f);
        private Vector2 lastCanvasSize = new Vector2(-1f, -1f);
        private bool resumeOnClose;
        private int selectedStage;

        public bool IsOpen => root.gameObject.activeSelf;

        public DefenseMapView(RectTransform canvasRoot, DefenseUiBuilder ui, Sprite cityMap, GrandmaDefense encounter)
        {
            this.canvasRoot = canvasRoot;
            this.ui = ui;
            this.encounter = encounter;
            root = ui.Rect(canvasRoot, "Map overlay", Vector2.zero, Vector2.one);
            RectTransform curtain = ui.Rect(root, "Map backdrop", Vector2.zero, Vector2.one);
            ui.Paint(curtain, new Color(0.035f, 0.045f, 0.055f, 0.84f), true);
            ui.Click(curtain, Close);
            window = ui.Rect(root, "Map window", Vector2.one * 0.5f, Vector2.one * 0.5f);
            ui.Paint(window, ink, true);
            RectTransform inner = ui.Rect(window, "Paper inset", Vector2.zero, Vector2.one);
            inner.offsetMin = Vector2.one * 4f;
            inner.offsetMax = -Vector2.one * 4f;
            ui.Paint(inner, paper);
            RectTransform header = ui.Rect(window, "Map header", new Vector2(0f, 1f), Vector2.one);
            header.offsetMin = new Vector2(4f, -70f);
            header.offsetMax = new Vector2(-4f, -4f);
            ui.Paint(header, ink);
            ui.Label(ui.Fixed(header, "Map title", 22f, -9f, 650f, 34f), "폐허 도시 · 지역 선택", 25, paper);
            ui.Label(ui.Fixed(header, "Map subtitle", 23f, -43f, 600f, 20f), "RUINED CITY  /  SELECT A DISTRICT", 11, gold);
            RectTransform close = ui.Rect(header, "Close map", Vector2.one, Vector2.one);
            close.pivot = new Vector2(1f, 0.5f);
            close.anchoredPosition = new Vector2(-16f, -33f);
            close.sizeDelta = new Vector2(88f, 38f);
            ui.Paint(close, new Color(0.31f, 0.36f, 0.35f), true);
            ui.Click(close, Close);
            ui.Label(ui.Stretch(close, "Close label", 0f), "닫기  ×", 16, paper, TextAnchor.MiddleCenter);

            mapFrame = ui.Rect(window, "City map frame", Vector2.zero, Vector2.zero);
            ui.Paint(mapFrame, new Color(0.38f, 0.43f, 0.39f));
            mapPicture = ui.Stretch(mapFrame, "City map image", 5f);
            Image mapImage = ui.Paint(mapPicture, cityMap == null ? new Color(0.34f, 0.38f, 0.35f) : Color.white);
            mapImage.sprite = cityMap;
            mapImage.preserveAspect = true;
            for (int i = 0; i < DefenseRegions.Count; i++)
            {
                int index = i;
                DefenseRegion region = DefenseRegions.Get(i);
                RectTransform pin = ui.Rect(mapPicture, "Region " + i, region.MapPosition, region.MapPosition);
                pin.sizeDelta = new Vector2(148f, 48f);
                pins[i] = ui.Paint(pin, paper, true);
                Outline edge = pin.gameObject.AddComponent<Outline>();
                edge.effectColor = new Color(0.10f, 0.14f, 0.13f, 0.95f);
                edge.effectDistance = new Vector2(2f, -2f);
                ui.Click(pin, () => SelectStage(index + 1));
                pinLabels[i] = ui.Label(ui.Stretch(pin, "Region label", 2f), "", 13, ink, TextAnchor.MiddleCenter);
            }
            home = ui.Rect(window, "Stage 0 home", Vector2.zero, Vector2.zero);
            homeImage = ui.Paint(home, paper, true);
            ui.Click(home, () => SelectStage(0));
            homeLabel = ui.Label(ui.Stretch(home, "Home label", 3f), "STAGE 0 · 할머니 집", 15, ink, TextAnchor.MiddleCenter);
            details = ui.Rect(window, "Region details", Vector2.zero, Vector2.zero);
            ui.Paint(details, new Color(0.84f, 0.82f, 0.73f));
            currentLocation = ui.Label(ui.Fixed(details, "Current location", 24f, -22f, 440f, 24f), "", 15, green);
            selectionHeading = ui.Label(ui.Fixed(details, "Selection heading", 24f, -79f, 400f, 24f), "선택한 구역", 14, ink);
            selectedTitle = ui.Label(ui.Fixed(details, "Selected region title", 22f, -113f, 440f, 47f), "", 31, ink);
            destinationCode = ui.Label(ui.Fixed(details, "Destination code", 24f, -164f, 400f, 26f), "", 13, green);
            divider = ui.Rect(details, "Detail divider", new Vector2(0f, 1f), Vector2.one);
            divider.offsetMin = new Vector2(24f, -211f);
            divider.offsetMax = new Vector2(-24f, -209f);
            ui.Paint(divider, new Color(0.64f, 0.66f, 0.57f));
            description = ui.Label(ui.Fixed(details, "Region description", 24f, -234f, 430f, 98f), "", 18, ink);
            description.horizontalOverflow = HorizontalWrapMode.Wrap;
            instructions = ui.Label(ui.Fixed(details, "Travel explanation", 24f, -356f, 430f, 88f), "", 14, green);
            instructions.horizontalOverflow = HorizontalWrapMode.Wrap;
            RectTransform travel = ui.Rect(details, "Travel to selected region", Vector2.zero, Vector2.right);
            travel.offsetMin = new Vector2(22f, 44f);
            travel.offsetMax = new Vector2(-22f, 100f);
            ui.Paint(travel, green, true);
            ui.Click(travel, ConfirmTravel);
            travelButton = travel.GetComponent<Button>();
            travelCaption = ui.Label(ui.Stretch(travel, "Travel label", 4f), "", 20, paper, TextAnchor.MiddleCenter);
            RectTransform footer = ui.Rect(details, "Map help", Vector2.zero, Vector2.right);
            footer.offsetMin = new Vector2(22f, 9f);
            footer.offsetMax = new Vector2(-22f, 36f);
            ui.Label(footer, "M  지도 열기/닫기    ESC  닫기", 12, ink, TextAnchor.MiddleCenter);
            root.gameObject.SetActive(false);
        }

        public void Open()
        {
            if (IsOpen || !encounter.CanOpenMap) return;
            resumeOnClose = !encounter.IsPaused;
            if (resumeOnClose) encounter.TogglePause();
            root.gameObject.SetActive(true);
            root.SetAsLastSibling();
            SelectStage(encounter.CurrentStageId);
            lastCanvasSize = new Vector2(-1f, -1f);
            RefreshLayout();
        }

        public void Close()
        {
            if (!IsOpen) return;
            root.gameObject.SetActive(false);
            if (resumeOnClose && encounter.IsPaused) encounter.TogglePause();
            resumeOnClose = false;
        }

        public void Toggle() { if (IsOpen) Close(); else Open(); }

        private void SelectStage(int index)
        {
            if (!DefenseStages.IsValid(index) || encounter.Progress == null) return;
            selectedStage = index;
            DefenseStage stage = DefenseStages.Get(index);
            DistrictProgress record = encounter.Progress.Stage(index);
            bool unlocked = encounter.Progress.IsUnlocked(index);
            bool ongoing = index == encounter.CurrentStageId && encounter.Phase != DefensePhase.Clear && encounter.Phase != DefensePhase.Failed && encounter.Phase != DefensePhase.Retreated;
            currentLocation.text = "현재 위치  ·  " + encounter.LocationName;
            selectedTitle.text = stage.Name;
            description.text = stage.WaveCount + " WAVE  ·  " + stage.WaveDescription + "\n일반 좀비를 모두 소탕하면 STAGE CLEAR\n" + (index + 1 < DefenseStages.Count ? "완료 후 다음 스테이지로 자동 이동" : "마지막 스테이지 · 완료 후 전체 CLEAR");
            destinationCode.text = "STAGE " + index + "   /   " + (!unlocked ? "잠김" : ongoing ? "진행 중" : record.clearCount > 0 ? "CLEAR" : "도전 가능");
            instructions.text = "CLEAR " + record.clearCount + "회\n" + (unlocked ? "웨이브 소탕으로 다음 Stage가 열립니다.\n이동·후퇴 시 (미정)재화는 보존됩니다." : "이전 Stage를 CLEAR하면 열립니다.");
            travelButton.interactable = unlocked;
            travelCaption.text = !unlocked ? "잠김 · 이전 Stage CLEAR 필요" : ongoing ? "현재 사냥 계속하기" : record.clearCount > 0 ? "이 구역에서 다시 사냥  →" : "이 구역에서 사냥 시작  →";
            homeImage.color = index == 0 ? gold : paper;
            homeLabel.text = "STAGE 0 · 할머니 집   /   " + State(0);
            for (int i = 0; i < pins.Length; i++)
            {
                int id = i + 1;
                pins[i].color = id == index ? gold : encounter.Progress.IsUnlocked(id) ? paper : new Color(.56f, .59f, .56f);
                pinLabels[i].text = "S" + id + "  " + DefenseStages.Get(id).Name + "\n" + State(id);
                pinLabels[i].color = ink;
            }
        }

        private string State(int id)
        {
            if (!encounter.Progress.IsUnlocked(id)) return "잠김";
            DistrictProgress record = encounter.Progress.Stage(id);
            return record.clearCount > 0 ? "CLEAR " + record.clearCount : id == encounter.CurrentStageId ? "진행 중" : "도전 가능";
        }

        private void ConfirmTravel()
        {
            if (encounter.EnterStage(selectedStage)) Close();
        }

        public void RefreshLayout()
        {
            if (!IsOpen) return;
            Vector2 size = canvasRoot.rect.size;
            if (Vector2.Distance(size, lastCanvasSize) < 0.1f) return;
            lastCanvasSize = size;
            float width = Mathf.Min(1140f, size.x - 32f);
            float height = Mathf.Min(656f, size.y - 32f);
            window.sizeDelta = new Vector2(width, height);
            float contentHeight = height - 100f;
            float mapSize = Mathf.Min(contentHeight - 52f, (width - 56f) * 0.51f);
            mapFrame.pivot = Vector2.zero;
            mapFrame.anchoredPosition = new Vector2(20f, 70f + (contentHeight - 52f - mapSize) * 0.5f);
            mapFrame.sizeDelta = Vector2.one * mapSize;
            home.pivot = Vector2.zero;
            home.anchoredPosition = new Vector2(20f, 18f);
            home.sizeDelta = new Vector2(mapSize, 42f);
            details.pivot = Vector2.zero;
            details.anchoredPosition = new Vector2(40f + mapSize, 18f);
            details.sizeDelta = new Vector2(width - mapSize - 60f, contentHeight);
            float textWidth = details.sizeDelta.x - 48f;
            foreach (Text text in new[] { currentLocation, selectedTitle, description })
                text.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, textWidth);
            selectionHeading.rectTransform.anchoredPosition = new Vector2(24f, -contentHeight * 0.14f);
            selectedTitle.rectTransform.anchoredPosition = new Vector2(22f, -contentHeight * 0.20f);
            destinationCode.rectTransform.anchoredPosition = new Vector2(24f, -contentHeight * 0.30f);
            divider.offsetMin = new Vector2(24f, -contentHeight * 0.38f - 2f);
            divider.offsetMax = new Vector2(-24f, -contentHeight * 0.38f);
            description.rectTransform.anchoredPosition = new Vector2(24f, -contentHeight * 0.42f);
            description.rectTransform.sizeDelta = new Vector2(textWidth, contentHeight * 0.23f);
            instructions.rectTransform.anchoredPosition = new Vector2(24f, -contentHeight * 0.64f);
            instructions.rectTransform.sizeDelta = new Vector2(textWidth, contentHeight * 0.17f);
        }

    }
}
