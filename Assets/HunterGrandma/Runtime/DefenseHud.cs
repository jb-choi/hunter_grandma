using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace HunterGrandma
{
    public sealed class DefenseHud : MonoBehaviour
    {
        [SerializeField] private GrandmaDefense encounter;
        [SerializeField] private Sprite cityMap;
        [SerializeField] private Font font;
        [SerializeField] private Sprite resourcePlaceholder, attackPlaceholder, ratePlaceholder, upgradePlaceholder;
        [SerializeField] private Sprite[] upgradePortraits, upgradeLockedPortraits;
        private Text heading, stageLabel, gold, status, pauseLabel, bossLabel, resultTitle, resultBody, saveWarning;
        private Image bossFill, fade;
        private GameObject bossPanel, resultPanel;
        private Button mapButton, retryButton, retreatButton, upgradeToggleButton;
        private DefenseMapView mapView;
        private DefenseUpgradeView upgradeView;
        private DefenseUiBuilder ui;
        private float nextRefresh;
        private Text gainFeedback;
        private RectTransform canvasRoot, resourceTarget;
        private Camera worldCamera;
        private readonly ResourceFlight[] flights = new ResourceFlight[12];
        private float gainUntil;
        private int pendingGain;
        private Vector2 lastCanvasSize;
        private GameObject battleChrome, openingChrome;
        private Text openingCaption;
        private sealed class ResourceFlight { public RectTransform Rect; public Vector2 Start; public float Age; public bool Active; }
        public bool IsMapOpen => mapView != null && mapView.IsOpen;
        public bool IsUpgradeOpen => upgradeView != null && upgradeView.IsOpen;
        public bool IsUpgradeDetailOpen => upgradeView != null && upgradeView.IsDetailOpen;
        public void Configure(GrandmaDefense owner, Font uiFont) { encounter = owner; font = uiFont; }
        public void ConfigureMap(Sprite map) { cityMap = map; }
        public void RefreshMapLayout()
        {
            mapView?.RefreshLayout();
            upgradeView?.RefreshLayout();
            if (canvasRoot == null || canvasRoot.rect.size == lastCanvasSize) return;
            lastCanvasSize = canvasRoot.rect.size;
            status.rectTransform.sizeDelta = new Vector2(lastCanvasSize.x - 60, 29);
        }

        private void Awake()
        {
            if (font == null)
            {
                Debug.LogError("DefenseHud requires the bundled Noto Sans CJK KR font. Assign it in the scene to display Korean text.", this);
                font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            }
            ui = new DefenseUiBuilder(font);
            BuildCanvas();
            Color cream = new Color(1f, .94f, .80f), muted = new Color(.73f, .79f, .74f);
            BuildBattleHud(battleChrome.transform, cream, muted);
            BuildResourceFlights(battleChrome.transform);
            mapView = new DefenseMapView(canvasRoot, ui, cityMap, encounter);
            upgradeView = new DefenseUpgradeView(canvasRoot, ui, encounter, upgradePortraits, upgradeLockedPortraits, encounter.GrandmaPortrait, attackPlaceholder, ratePlaceholder, RefreshPresentation);
            if (FindFirstObjectByType<EventSystem>() == null)
                new GameObject("UI EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule)).transform.SetParent(transform, false);
            BuildOpeningHud(canvasRoot, cream, muted);
        }

        private void BuildCanvas()
        {
            GameObject canvasObject = new GameObject("Defense HUD", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasObject.transform.SetParent(transform, false);
            Canvas canvas = canvasObject.GetComponent<Canvas>(); canvas.renderMode = RenderMode.ScreenSpaceOverlay; canvas.sortingOrder = 100;
            CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>(); scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1280f, 720f); scaler.matchWidthOrHeight = 0.5f;
            battleChrome = new GameObject("Battle chrome", typeof(RectTransform)); battleChrome.transform.SetParent(canvasObject.transform, false);
            RectTransform chromeRect = battleChrome.GetComponent<RectTransform>(); chromeRect.anchorMin = Vector2.zero; chromeRect.anchorMax = Vector2.one; chromeRect.offsetMin = chromeRect.offsetMax = Vector2.zero;
            canvasRoot = canvasObject.GetComponent<RectTransform>(); worldCamera = Camera.main;
        }

        private void BuildBattleHud(Transform root, Color cream, Color muted)
        {
            ui.Panel(root, "Top bar", new Vector2(0, 1), Vector2.one, new Vector2(0, -96), Vector2.zero, new Color(.055f, .075f, .10f, .94f));
            heading = ui.TextAt(root, "순덕 할매", 30, new Vector2(26, -10), new Vector2(590, 40), cream);
            stageLabel = ui.TextAt(root, "STAGE 0 · 할머니 집", 20, new Vector2(28, -53), new Vector2(700, 31), new Color(.94f, .73f, .38f));
            resourceTarget = ReferenceImage(root, "Resource reference", resourcePlaceholder, "재화-01", new Vector2(-468, -19), 58, true);
            gold = ui.TextAt(root, "", 24, new Vector2(-395, -17), new Vector2(367, 35), cream, true);
            gainFeedback = ui.TextAt(root, "", 19, new Vector2(-395, -56), new Vector2(367, 28), new Color(.91f, .87f, .57f), true);
            bossPanel = ui.Panel(root, "Boss health panel", new Vector2(.5f, 1), new Vector2(.5f, 1), new Vector2(-275, -144), new Vector2(275, -100), new Color(.10f, .07f, .13f, .92f));
            bossLabel = ui.TextAt(bossPanel.transform, "", 14, new Vector2(18, -5), new Vector2(516, 22), cream);
            GameObject track = ui.Panel(bossPanel.transform, "Boss health track", Vector2.zero, Vector2.right, new Vector2(18, 8), new Vector2(-18, 15), new Color(.25f, .23f, .28f));
            bossFill = ui.Panel(track.transform, "Boss health remaining", Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero, new Color(.65f, .31f, .48f)).GetComponent<Image>();
            bossFill.type = Image.Type.Simple;
            ui.Panel(root, "Footer", Vector2.zero, Vector2.right, Vector2.zero, new Vector2(0, 110), new Color(.055f, .075f, .10f, .96f));
            status = ui.TextAt(root, "", 19, new Vector2(30, 76), new Vector2(1100, 29), new Color(.77f, .86f, .59f), false, true);
            ui.TextAt(root, "WASD 이동 · TAB 자동 · U 강화", 15, new Vector2(31, 10), new Vector2(470, 23), muted, false, true);
            Text upgradeEntry = ui.MakeButton(root, "Upgrade panel toggle", new Vector2(-766, 20), new Vector2(138, 42), ToggleUpgrade, true);
            upgradeToggleButton = upgradeEntry.GetComponentInParent<Button>();
            ReferenceImage(upgradeEntry.transform.parent, "Upgrade reference", upgradePlaceholder, "강화-01", new Vector2(4, -5), 32);
            upgradeEntry.text = "강화  U"; upgradeEntry.rectTransform.offsetMin = new Vector2(38, 2);
            Text mapLabel = ui.MakeButton(root, "지도  M", new Vector2(-615, 20), new Vector2(138, 42), ToggleMap, true);
            mapButton = mapLabel.GetComponentInParent<Button>();
            pauseLabel = ui.MakeButton(root, "쉬기", new Vector2(-464, 20), new Vector2(138, 42), encounter.TogglePause, true);
            retryButton = ui.MakeButton(root, "재도전", new Vector2(-313, 20), new Vector2(138, 42), encounter.RestartEncounter, true).GetComponentInParent<Button>();
            retreatButton = ui.MakeButton(root, "후퇴", new Vector2(-162, 20), new Vector2(132, 42), () => encounter.Retreat(), true).GetComponentInParent<Button>();
            fade = ui.Panel(root, "Return to yard fade", Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero, Color.clear).GetComponent<Image>();
            resultPanel = ui.Panel(root, "Run result", new Vector2(1, .5f), new Vector2(1, .5f), new Vector2(-491, -182), new Vector2(-30, 143), new Color(.08f, .12f, .13f, .97f));
            resultTitle = ui.TextAt(resultPanel.transform, "", 28, new Vector2(24, -22), new Vector2(414, 45), cream);
            resultBody = ui.TextAt(resultPanel.transform, "", 16, new Vector2(25, -82), new Vector2(410, 142), muted);
            resultBody.horizontalOverflow = HorizontalWrapMode.Wrap;
            ui.MakeButton(resultPanel.transform, "한 번 더 사냥", new Vector2(24, 23), new Vector2(194, 48), encounter.RestartEncounter);
            ui.MakeButton(resultPanel.transform, "지도 · 다음 구역", new Vector2(231, 23), new Vector2(206, 48), ToggleMap);
            resultPanel.SetActive(false);
            saveWarning = ui.TextAt(root, "", 13, new Vector2(32, -246), new Vector2(680, 26), new Color(1f, .75f, .45f));
        }

        private void BuildResourceFlights(Transform root)
        {
            for (int i = 0; i < flights.Length; i++)
            {
                GameObject obj = ui.Panel(root, "Resource flight " + i, Vector2.one * .5f, Vector2.one * .5f, Vector2.zero, new Vector2(27, 27), Color.white);
                Image icon = obj.GetComponent<Image>(); icon.sprite = resourcePlaceholder; icon.preserveAspect = true; icon.color = new Color(.96f, .88f, .58f);
                RectTransform rect = obj.GetComponent<RectTransform>(); rect.pivot = Vector2.one * .5f; rect.sizeDelta = Vector2.one * 27;
                obj.SetActive(false); flights[i] = new ResourceFlight { Rect = rect };
            }
        }

        private void BuildOpeningHud(Transform root, Color cream, Color muted)
        {
            openingChrome = ui.Panel(root, "Opening chrome", Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero, Color.clear);
            ui.Panel(openingChrome.transform, "Opening top band", Vector2.up, Vector2.one, new Vector2(0, -84), Vector2.zero, new Color(.015f, .02f, .035f, .9f));
            ui.Panel(openingChrome.transform, "Opening caption band", Vector2.zero, Vector2.right, Vector2.zero, new Vector2(0, 110), new Color(.015f, .02f, .035f, .94f));
            ui.TextAt(openingChrome.transform, "순덕 할매의 집 · 조용한 저녁", 25, new Vector2(28, -24), new Vector2(780, 38), cream);
            ui.MakeButton(openingChrome.transform, "Skip opening", new Vector2(-270, -62), new Vector2(242, 42), encounter.SkipStageOpening, true, true).text = "건너뛰기  ENTER";
            openingCaption = ui.TextAt(openingChrome.transform, "", 27, new Vector2(30, 42), new Vector2(1180, 58), cream, false, true);
            ui.TextAt(openingChrome.transform, "SPACE  일시정지 / 계속하기     ENTER  사냥 시작", 15, new Vector2(32, 9), new Vector2(1120, 26), muted, false, true);
            openingChrome.SetActive(false);
        }

        private void Update()
        {
            if (encounter.Progress == null) return;
            if (Keyboard.current != null)
            {
                if (encounter.IsOpening && Keyboard.current.enterKey.wasPressedThisFrame) encounter.SkipStageOpening();
                if (Keyboard.current.mKey.wasPressedThisFrame) ToggleMap();
                if (mapView.IsOpen) { if (Keyboard.current.escapeKey.wasPressedThisFrame) mapView.Close(); }
                else
                {
                    if (Keyboard.current.uKey.wasPressedThisFrame) ToggleUpgrade();
                    if (Keyboard.current.escapeKey.wasPressedThisFrame) CloseUpgrade();
                    if (Keyboard.current.spaceKey.wasPressedThisFrame) encounter.TogglePause();
                    if (Keyboard.current.rKey.wasPressedThisFrame) encounter.RestartEncounter();
                }
            }
            mapView.RefreshLayout();
            upgradeView.RefreshLayout();
            UpdateFeedback();
            if (Time.unscaledTime < nextRefresh) return;
            nextRefresh = Time.unscaledTime + .1f;
            RefreshPresentation();
        }
        public void RefreshPresentation()
        {
            if (encounter.Progress == null) return;
            battleChrome.SetActive(!encounter.IsOpening); openingChrome.SetActive(encounter.IsOpening);
            if (encounter.IsOpening)
            {
                if (IsMapOpen) mapView.Close(); CloseUpgrade();
                openingCaption.text = encounter.OpeningCaption + (encounter.IsPaused ? "\n(잠시 쉬는 중)" : ""); return;
            }
            DefenseProgress p = encounter.Progress;
            heading.text = encounter.AtYard ? "순덕 할매 · 집 마당" : "순덕 할매";
            stageLabel.text = "STAGE " + encounter.CurrentStageId + "  /  " + encounter.LocationName + "  ·  " + (encounter.AutoExplore?"자동 탐색 ON":"직접 이동");
            gold.text = "(미정)재화  " + p.Gold;
            if (Time.unscaledTime >= gainUntil) gainFeedback.text = "이번 사냥 +" + encounter.Run.EarnedGold;
            upgradeView.Refresh();
            status.text = "● " + encounter.StateText + "   ·   체력 " + encounter.DefenseHealth + "   ·   처치 " + encounter.KillCount + "   ·   포획 " + p.CaptiveCount;
            pauseLabel.text = encounter.IsPaused ? "계속하기" : "쉬기";
            mapButton.interactable = encounter.CanOpenMap; retryButton.interactable = !encounter.IsCaptureSequence;
            retreatButton.interactable = !encounter.IsCaptureSequence && encounter.Phase != DefensePhase.Clear && encounter.Phase != DefensePhase.Retreated && encounter.Phase != DefensePhase.Failed;
            bool showBoss = encounter.Boss != null && encounter.Boss.gameObject.activeSelf && !encounter.AtYard;
            bossPanel.SetActive(showBoss);
            if (showBoss)
            {
                BossActor boss = encounter.Boss;
                bossLabel.text = encounter.CurrentStage.BossName + "  ·  " + boss.Health + " / " + boss.MaxHealth + " HP";
                bossFill.rectTransform.anchorMax = new Vector2((float)boss.Health / boss.MaxHealth, 1f);
            }
            fade.color = new Color(.055f, .075f, .10f, encounter.ReturnFade);
            bool ended = encounter.Phase == DefensePhase.Clear || encounter.Phase == DefensePhase.Failed || encounter.Phase == DefensePhase.Retreated;
            upgradeToggleButton.interactable = !ended && !encounter.IsCaptureSequence;
            if (ended || encounter.IsCaptureSequence) CloseUpgrade();
            resultPanel.SetActive(ended);
            if (ended)
            {
                bool clear = encounter.Phase == DefensePhase.Clear;
                resultTitle.text = clear ? "STAGE CLEAR" : encounter.Phase == DefensePhase.Failed ? "방어선이 무너졌어요" : "이번 사냥에서 후퇴";
                if (clear && encounter.LastCaptive != null)
                {
                    DistrictProgress record = p.Stage(encounter.CurrentStageId);
                    string next = encounter.CurrentStageId < DefenseStages.Count - 1 ? "다음 구역 해금: " + DefenseStages.Get(encounter.CurrentStageId + 1).Name : "모든 구역 해금 완료 · 반복 포획 가능";
                    string identity = encounter.CurrentStageId == 0 ? encounter.LastCaptive.appearance : encounter.CurrentStage.BossName + " · " + encounter.LastCaptive.appearance;
                    resultBody.text = identity + "\n집 마당 수레에 무사히 도착 · 신규 포획 +1\n포획 ID  " + encounter.LastCaptive.captiveId.Substring(0, 8) + "\nCLEAR " + record.clearCount + "회  ·  이 구역 포획 " + record.capturedCount + "명\n" + next;
                }
                else resultBody.text = "획득한 (미정)재화 +" + encounter.Run.EarnedGold + " 보존\n이번 사냥의 포획 보상은 없습니다.\n기존 CLEAR·포획 기록도 그대로입니다.\n강화 후 다시 도전할 수 있습니다.";
            }
            saveWarning.text = encounter.SaveWarning ? "저장에 실패했습니다. 잠시 후 자동으로 다시 시도합니다." : "";
        }
        public void ToggleUpgrade()
        {
            if (IsMapOpen || encounter.Progress == null || encounter.IsOpening || encounter.IsCaptureSequence || encounter.Phase == DefensePhase.Clear || encounter.Phase == DefensePhase.Failed || encounter.Phase == DefensePhase.Retreated) return;
            upgradeView.Toggle(); RefreshPresentation();
        }
        private void CloseUpgrade() { upgradeView?.Close(); }
        private void ToggleMap() { CloseUpgrade(); mapView.Toggle(); }
        public void ShowResourceGain(int amount, Vector3 worldPosition)
        {
            if (amount <= 0) return;
            pendingGain = Time.unscaledTime < gainUntil ? pendingGain + amount : amount;
            gainUntil = Time.unscaledTime + 1.25f; gainFeedback.text = "+" + pendingGain + " 획득"; gold.color = new Color(1f, .88f, .51f);
            // The kill already credited Progress. Flying icons never grant rewards.
            foreach (ResourceFlight flight in flights)
            {
                if (flight.Active) continue;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRoot, worldCamera.WorldToScreenPoint(worldPosition), null, out flight.Start);
                flight.Age = 0; flight.Active = true; flight.Rect.anchoredPosition = flight.Start; flight.Rect.gameObject.SetActive(true); break;
            }
            RefreshPresentation();
        }
        public void ResetRunFeedback()
        {
            pendingGain = 0; gainUntil = 0;
            CloseUpgrade();
            foreach (ResourceFlight flight in flights) { flight.Active = false; flight.Rect.gameObject.SetActive(false); }
        }
        private void UpdateFeedback()
        {
            if (Time.unscaledTime >= gainUntil) gold.color = new Color(1f, .94f, .80f);
            Vector2 destination = canvasRoot.InverseTransformPoint(resourceTarget.position);
            foreach (ResourceFlight flight in flights)
            {
                if (!flight.Active) continue;
                flight.Age += Time.unscaledDeltaTime; float t = Mathf.Clamp01(flight.Age / .7f);
                flight.Rect.anchoredPosition = Vector2.Lerp(flight.Start, destination, t * t) + Vector2.up * (Mathf.Sin(t * Mathf.PI) * 45f);
                if (t >= 1) { flight.Active = false; flight.Rect.gameObject.SetActive(false); }
            }
        }
        private RectTransform ReferenceImage(Transform root, string name, Sprite sprite, string id, Vector2 topLeft, float size, bool right = false)
        {
            Vector2 anchor = new Vector2(right ? 1 : 0, 1);
            GameObject obj = ui.Panel(root, name, anchor, anchor, topLeft - new Vector2(0, size), topLeft + new Vector2(size, 0), new Color(.68f, .80f, .72f, .45f));
            Image image = obj.GetComponent<Image>(); image.sprite = sprite; image.preserveAspect = true;
            Text label = ui.TextAt(obj.transform, id, size < 40 ? 10 : 14, Vector2.zero, Vector2.one * size, new Color(1, .97f, .85f));
            RectTransform rect = label.rectTransform; rect.anchorMin = Vector2.zero; rect.anchorMax = Vector2.one; rect.offsetMin = rect.offsetMax = Vector2.zero;
            label.alignment = TextAnchor.MiddleCenter; label.gameObject.AddComponent<Shadow>().effectColor = Color.black;
            return obj.GetComponent<RectTransform>();
        }
        private void OnDisable() { mapView?.Close(); CloseUpgrade(); }
    }
}
