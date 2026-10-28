# UI P0 validation

Unity 6000.3.10f1 · 2026-10-27 17:23 KST

Status: FAIL

Isolated profile: Library/UI-P0-validation.json. Synthetic funds and one synthetic kill are used only in this UI test profile; the ordinary player save is never reset.

- PASS: Editor preview is disabled in Play Mode; runtime owns the belt
- PASS: Runtime belt geometry remains present
- PASS: HUD starts compact with both panels closed
- PASS: Compact HUD uses the smaller top safe area
- PASS: Live UI uses overlay rendering above world actors
- PASS: Downloaded placeholder image is wired to the resource HUD
- PASS: Resource image carries its temporary identifier
- PASS: Capture restores Canvas state: hunt
- PASS: Capture restores Canvas state: hunt-small
- PASS: U opens upgrades without changing the existing pause state

Failure: Missing button: Attack upgrade
  at HunterGrandma.Editor.GrandmaUiValidation.Button (System.String name) [0x00000] in E:\unity_project\project10_hunter_grandma\Hunter_grandma\Assets\HunterGrandma\Editor\GrandmaUiValidation.cs:52 
  at HunterGrandma.Editor.GrandmaUiValidation.Tick () [0x002a8] in E:\unity_project\project10_hunter_grandma\Hunter_grandma\Assets\HunterGrandma\Editor\GrandmaUiValidation.cs:95 