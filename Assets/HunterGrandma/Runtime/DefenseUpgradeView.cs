using UnityEngine;
using UnityEngine.UI;

namespace HunterGrandma
{
    // The supplied KRA is a connection flow: five roster slots, then the selected grandma's upgrades.
    internal sealed class DefenseUpgradeView
    {
        private readonly DefenseUiBuilder ui;
        private readonly GrandmaDefense encounter;
        private readonly System.Action refreshHud;
        private readonly RectTransform canvas, root, window, roster, detail;
        private readonly ScrollRect scroll;
        private readonly GameObject connectionHint, purchases;
        private readonly Text attackLabel, rateLabel, skillLabel, feedback, balance;
        private readonly Button attackButton, rateButton, skillButton;
        private Vector2 lastSize;
        private float feedbackUntil;
        private static readonly Color Ink = new Color(.10f, .15f, .16f);
        private static readonly Color Paper = new Color(.90f, .88f, .79f);
        private static readonly Color Muted = new Color(.65f, .71f, .66f);
        private static readonly Color Accent = new Color(.91f, .73f, .39f);
        private static readonly string[] Names = { "순덕 할매", "화방 할매", "기관총 할매", "저격 할매", "오토바이 할매" };

        public bool IsOpen => root.gameObject.activeSelf;
        public bool IsDetailOpen => IsOpen && purchases.activeSelf;

        public DefenseUpgradeView(RectTransform canvas, DefenseUiBuilder ui, GrandmaDefense encounter, Sprite[] portraits, Sprite[] lockedPortraits, Sprite fallbackPortrait, Sprite attack, Sprite rate, System.Action refreshHud)
        {
            this.canvas = canvas; this.ui = ui; this.encounter = encounter; this.refreshHud = refreshHud;
            root = ui.Rect(canvas, "Upgrade overlay", Vector2.zero, Vector2.one);
            RectTransform shade = ui.Rect(root, "Upgrade backdrop", Vector2.zero, Vector2.one);
            ui.Paint(shade, new Color(.02f, .035f, .045f, .68f), true); ui.Click(shade, Close);
            window = ui.Rect(root, "Upgrade panel", Vector2.one * .5f, Vector2.one * .5f);
            ui.Paint(window, Ink, true);
            RectTransform header = ui.Rect(window, "Upgrade header", Vector2.up, Vector2.one);
            header.offsetMin = new Vector2(4, -72); header.offsetMax = new Vector2(-4, -4); ui.Paint(header, new Color(.19f, .27f, .25f));
            ui.Label(ui.Fixed(header, "Upgrade title", 22, -10, 640, 36), "할매 업그레이드", 28, Paper);
            ui.Label(ui.Fixed(header, "Upgrade subtitle", 24, -46, 640, 20), "할매를 확인하고 오른쪽 화살표로 강화창을 열어요", 15, Muted);
            Text close = ui.MakeButton(header, "Close upgrades", new Vector2(-108, -51), new Vector2(88, 36), Close, true, true); close.text = "닫기  ×";

            roster = ui.Rect(window, "Grandma roster", Vector2.up, Vector2.up); roster.pivot = Vector2.up;
            ui.Paint(roster, new Color(.14f, .20f, .20f));
            ui.Label(ui.Fixed(roster, "Roster heading", 16, -10, 330, 30), "할매 목록  ·  1 / 5", 20, Paper);
            RectTransform viewport = ui.Rect(roster, "Grandma roster viewport", Vector2.zero, Vector2.one);
            viewport.offsetMin = new Vector2(10, 12); viewport.offsetMax = new Vector2(-10, -50);
            ui.Paint(viewport, Color.clear, true); viewport.gameObject.AddComponent<RectMask2D>();
            RectTransform content = ui.Rect(viewport, "Grandma roster content", Vector2.up, Vector2.one);
            content.pivot = Vector2.up; content.sizeDelta = new Vector2(0, 5 * 126 - 10);
            scroll = viewport.gameObject.AddComponent<ScrollRect>(); scroll.viewport = viewport; scroll.content = content;
            scroll.horizontal = false; scroll.vertical = true; scroll.movementType = ScrollRect.MovementType.Clamped; scroll.scrollSensitivity = 45;
            for (int i = 0; i < 5; i++)
            {
                Sprite portrait = Resolve(portraits, i, fallbackPortrait);
                Sprite locked = Resolve(lockedPortraits, i, null);
                BuildCard(content, i == 0 ? portrait : locked != null ? locked : portrait, i, locked != null);
            }

            detail = ui.Rect(window, "Upgrade details", Vector2.up, Vector2.up); detail.pivot = Vector2.up; ui.Paint(detail, new Color(.075f, .115f, .13f));
            connectionHint = ui.Rect(detail, "Upgrade connection hint", Vector2.zero, Vector2.one).gameObject;
            ui.Label(ui.Fixed(connectionHint.transform, "Connection title", 22, -20, 440, 40), "업그레이드창 연결", 27, Paper);
            Text hint = ui.Label(ui.Fixed(connectionHint.transform, "Connection instructions", 24, -76, 440, 70), "순덕 할매 카드의  →  를 눌러\n강화할 항목을 확인하세요.", 20, Muted); hint.horizontalOverflow = HorizontalWrapMode.Wrap;
            ui.Label(ui.Fixed(connectionHint.transform, "Undecided upgrade heading", 24, -163, 440, 30), "상세 구성 · 아직 미정", 19, Accent);
            for (int i = 0; i < 2; i++)
            {
                RectTransform placeholder = ui.Rect(connectionHint.transform, "Undecided upgrade slot " + i, new Vector2(0, 1), Vector2.one);
                placeholder.offsetMin = new Vector2(22, -294 - i * 108); placeholder.offsetMax = new Vector2(-22, -208 - i * 108);
                ui.Paint(placeholder, new Color(.16f, .21f, .22f));
                ui.Label(ui.Stretch(placeholder, "Undecided label", 8), "(미정)강화 항목", 18, Muted, TextAnchor.MiddleCenter);
            }

            purchases = ui.Rect(detail, "Sundeok upgrades", Vector2.zero, Vector2.one).gameObject;
            ui.Label(ui.Fixed(purchases.transform, "Selected grandma title", 22, -16, 420, 38), "순덕 할매 · 샷건", 26, Paper);
            ui.Label(ui.Fixed(purchases.transform, "Upgrade scope", 24, -62, 440, 25), "스킬 설정 · 성능 강화  /  휠로 항목 이동", 17, Muted);
            ui.Label(ui.Fixed(purchases.transform, "Rate explanation", 24, -91, 440, 24), "발사 간격은 짧을수록 빨라져요", 16, Accent);
            RectTransform purchaseViewport = ui.Rect(purchases.transform, "Upgrade items viewport", Vector2.zero, Vector2.one);
            purchaseViewport.offsetMin = new Vector2(0, 112); purchaseViewport.offsetMax = new Vector2(0, -126);
            ui.Paint(purchaseViewport, Color.clear, true); purchaseViewport.gameObject.AddComponent<RectMask2D>();
            RectTransform purchaseContent = ui.Rect(purchaseViewport, "Upgrade items content", Vector2.up, Vector2.one);
            purchaseContent.pivot = Vector2.up; purchaseContent.sizeDelta = new Vector2(0, 352);
            ScrollRect itemScroll = purchaseViewport.gameObject.AddComponent<ScrollRect>();
            itemScroll.viewport = purchaseViewport; itemScroll.content = purchaseContent;
            itemScroll.horizontal = false; itemScroll.movementType = ScrollRect.MovementType.Clamped; itemScroll.scrollSensitivity = 45;
            attackLabel = BuildPurchase(purchaseContent, "Attack upgrade", 120, false, attack, "Attack reference", "항목-01");
            rateLabel = BuildPurchase(purchaseContent, "Fire rate upgrade", 240, true, rate, "Rate reference", "항목-02");
            attackButton = attackLabel.GetComponentInParent<Button>(); rateButton = rateLabel.GetComponentInParent<Button>();
            RectTransform skillRect = ui.Rect(purchaseContent, "Shadow skill toggle", Vector2.up, Vector2.one);
            skillRect.offsetMin = new Vector2(22, -112); skillRect.offsetMax = new Vector2(-22, 0);
            ui.Paint(skillRect, new Color(.25f, .21f, .33f), true);
            ui.Click(skillRect, ToggleSkill); skillButton = skillRect.GetComponent<Button>();
            skillLabel = ui.Label(ui.Stretch(skillRect, "Shadow skill caption", 12), "", 19, Paper, TextAnchor.MiddleLeft);
            skillLabel.horizontalOverflow = HorizontalWrapMode.Wrap;
            RectTransform feedbackRect = ui.Rect(purchases.transform, "Upgrade feedback", Vector2.zero, Vector2.right);
            feedbackRect.offsetMin = new Vector2(22, 59); feedbackRect.offsetMax = new Vector2(-22, 109);
            feedback = ui.Label(feedbackRect, "사냥을 계속하며 강화할 수 있습니다.", 17, Muted); feedback.horizontalOverflow = HorizontalWrapMode.Wrap;
            ui.MakeButton(purchases.transform, "Back to upgrade roster", new Vector2(22, 14), new Vector2(150, 34), ShowConnection).text = "←  목록 보기";

            balance = ui.Label(ui.Fixed(window, "Upgrade balance", 22, 0, 610, 29), "", 19, Accent);
            RectTransform help = ui.Rect(window, "Upgrade help", Vector2.zero, Vector2.right);
            help.offsetMin = new Vector2(22, 7); help.offsetMax = new Vector2(-22, 29);
            ui.Label(help, "마우스 휠  목록 이동     U / ESC  닫기     SPACE  쉬기", 14, Muted, TextAnchor.MiddleRight);
            ShowConnection(); root.gameObject.SetActive(false);
        }

        private static Sprite Resolve(Sprite[] portraits, int index, Sprite fallback)
            => portraits != null && index < portraits.Length && portraits[index] != null ? portraits[index] : fallback;

        private void BuildCard(RectTransform content, Sprite portrait, int index, bool hasSilhouette)
        {
            bool unlocked = index == 0;
            RectTransform card = ui.Rect(content, "Grandma card " + index, Vector2.up, Vector2.one);
            card.offsetMin = new Vector2(0, -116 - index * 126); card.offsetMax = new Vector2(0, -index * 126);
            ui.Paint(card, unlocked ? new Color(.25f, .34f, .30f) : new Color(.16f, .20f, .21f));
            RectTransform frame = ui.Fixed(card, "Grandma portrait frame " + index, 9, -12, 88, 92);
            ui.Paint(frame, new Color(.53f, .57f, .50f));
            RectTransform picture = ui.Stretch(frame, "Grandma portrait " + index, 2);
            Image image = ui.Paint(picture, unlocked || hasSilhouette ? Color.white : new Color(.035f, .045f, .05f)); image.sprite = portrait; image.preserveAspect = true;
            RectTransform name = ui.Rect(card, "Grandma name " + index, Vector2.up, Vector2.one);
            name.offsetMin = new Vector2(108, -47); name.offsetMax = new Vector2(-62, -13);
            ui.Label(name, Names[index], 21, unlocked ? Paper : Muted);
            RectTransform description = ui.Rect(card, "Grandma state " + index, Vector2.up, Vector2.one);
            description.offsetMin = new Vector2(109, -103); description.offsetMax = new Vector2(-64, -53);
            Text info = ui.Label(description, unlocked ? "샷건\n강화창 연결 가능" : "미해금", 16, Muted); info.horizontalOverflow = HorizontalWrapMode.Wrap;
            Text arrow = ui.MakeButton(card, unlocked ? "Open Sundeok upgrades" : "Locked grandma " + index, new Vector2(-55, -82), new Vector2(44, 52), ShowDetails, true, true);
            arrow.text = unlocked ? "→" : "—"; arrow.fontSize = 28; arrow.GetComponentInParent<Button>().interactable = unlocked;
        }

        private Text BuildPurchase(Transform parent, string name, float top, bool rate, Sprite icon, string iconName, string id)
        {
            RectTransform buttonRect = ui.Rect(parent, name, Vector2.up, Vector2.one);
            buttonRect.offsetMin = new Vector2(22, -top - 108); buttonRect.offsetMax = new Vector2(-22, -top);
            ui.Paint(buttonRect, new Color(.24f, .33f, .29f), true); ui.Click(buttonRect, () => Purchase(rate));
            RectTransform caption = ui.Stretch(buttonRect, name + " caption", 10); caption.offsetMin = new Vector2(88, 8);
            Text label = ui.Label(caption, "", 20, Paper, TextAnchor.MiddleLeft); label.horizontalOverflow = HorizontalWrapMode.Wrap;
            RectTransform reference = ui.Fixed(buttonRect, iconName, 10, -20, 64, 64);
            Image image = ui.Paint(reference, new Color(.68f, .80f, .72f, .45f)); image.sprite = icon; image.preserveAspect = true;
            ui.Label(ui.Stretch(reference, "Placeholder ID", 0), id, 14, Paper, TextAnchor.MiddleCenter).gameObject.AddComponent<Shadow>().effectColor = Color.black;
            return label;
        }

        public void Toggle() { if (IsOpen) Close(); else Open(); }
        private void Open()
        {
            root.gameObject.SetActive(true); root.SetAsLastSibling(); ShowConnection();
            scroll.StopMovement(); scroll.verticalNormalizedPosition = 1; lastSize = Vector2.zero; RefreshLayout(); Refresh();
        }
        public void Close() { root.gameObject.SetActive(false); }
        private void ShowConnection() { connectionHint.SetActive(true); purchases.SetActive(false); }
        private void ShowDetails() { connectionHint.SetActive(false); purchases.SetActive(true); feedbackUntil = 0; Refresh(); }
        private void Purchase(bool rate)
        {
            bool bought = rate ? encounter.TryUpgradeFireRate() : encounter.TryUpgradeAttack();
            if (bought) { feedback.text = "강화 완료 · 임시 성능에 적용했습니다."; feedbackUntil = Time.unscaledTime + 2; }
            refreshHud();
        }
        private void ToggleSkill()
        {
            encounter.SetShadowSkillEnabled(!encounter.Progress.ShadowSkillEnabled);
            feedback.text = encounter.Progress.ShadowSkillEnabled ? "스킬 ON · 전투 시간 10초마다 자동 발동" : "스킬 OFF · 일반 사격으로 사냥합니다.";
            feedbackUntil = Time.unscaledTime + 2; refreshHud();
        }
        public void Refresh()
        {
            if (!IsOpen || encounter.Progress == null) return;
            DefenseProgress p = encounter.Progress;
            balance.text = "(미정)재화  " + p.Gold;
            attackLabel.text = "항목 01 · 임시 공격력 / Lv." + p.AttackLevel + "\n" + p.Damage + (p.AttackLevel >= 30 ? " · 최대 강화" : " → " + p.NextDamage + "\n비용 " + p.AttackCost + (p.Gold < p.AttackCost ? " · 재화 부족" : " · 강화 가능"));
            rateLabel.text = "항목 02 · 임시 발사 간격 / Lv." + p.FireRateLevel + "\n" + p.FireInterval.ToString("F2") + (p.FireRateLevel >= 15 ? "초 · 최대 강화" : " → " + p.NextFireInterval.ToString("F2") + "초\n비용 " + p.FireRateCost + (p.Gold < p.FireRateCost ? " · 재화 부족" : " · 강화 가능"));
            attackButton.interactable = p.Gold >= p.AttackCost && p.AttackLevel < 30;
            rateButton.interactable = p.Gold >= p.FireRateCost && p.FireRateLevel < 15;
            skillLabel.text = "할매의 밤마실   [ " + (p.ShadowSkillEnabled ? "ON" : "OFF") + " ]\n" +
                (encounter.IsSkillActive ? "순간이동 사격 " + encounter.SkillShots + " / 5" : p.ShadowSkillEnabled ? "다음 발동 " + encounter.SkillCooldownRemaining.ToString("F1") + "초 · 전방 표적 필요" : "눌러서 켜기 · 10초마다 자동 발동") + "\n시간 정지 · 뒤통수 5연사 · 원위치 복귀";
            skillButton.GetComponent<Image>().color = p.ShadowSkillEnabled ? new Color(.32f, .28f, .43f) : new Color(.20f, .21f, .25f);
            if (Time.unscaledTime >= feedbackUntil) feedback.text = "사냥을 계속하며 강화할 수 있습니다.";
        }
        public void RefreshLayout()
        {
            if (!IsOpen || canvas.rect.size == lastSize) return;
            lastSize = canvas.rect.size;
            float width = Mathf.Min(1100, lastSize.x - 32), height = Mathf.Min(640, lastSize.y - 32);
            window.sizeDelta = new Vector2(width, height);
            float listWidth = (width - 52) * .41f, bodyHeight = height - 138;
            roster.anchoredPosition = new Vector2(16, -84); roster.sizeDelta = new Vector2(listWidth, bodyHeight);
            detail.anchoredPosition = new Vector2(32 + listWidth, -84); detail.sizeDelta = new Vector2(width - listWidth - 48, bodyHeight);
            balance.rectTransform.anchoredPosition = new Vector2(24, -height + 49);
        }
    }
}
