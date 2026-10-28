# UI P0 validation

Unity 6000.3.10f1 · 2026-10-28 01:11 KST

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
- PASS: Insufficient currency disables both purchase buttons
- PASS: Insufficient state has a text explanation
- PASS: Both upgrade images carry temporary identifiers
- PASS: Capture restores Canvas state: upgrades-poor
- PASS: Escape closes upgrades without resuming paused combat
- PASS: Interactable UI: Upgrade panel toggle
- PASS: Repeated upgrade open 0
- PASS: Interactable UI: Close upgrades
- PASS: Interactable UI: Upgrade panel toggle
- PASS: Repeated upgrade open 1
- PASS: Interactable UI: Close upgrades
- PASS: Interactable UI: Upgrade panel toggle
- PASS: Repeated upgrade open 2
- PASS: Interactable UI: Close upgrades
- PASS: Interactable UI: Upgrade panel toggle
- PASS: Repeated upgrade open 3
- PASS: Interactable UI: Close upgrades
- PASS: Interactable UI: Upgrade panel toggle
- PASS: Repeated upgrade open 4
- PASS: Interactable UI: Close upgrades
- PASS: Interactable UI: Upgrade panel toggle
- PASS: Repeated upgrade open 5
- PASS: Interactable UI: Close upgrades
- PASS: Repeated opening does not leave hidden blocking UI
- PASS: Interactable UI: Upgrade panel toggle
- PASS: Interactable UI: Attack upgrade
- PASS: Attack button spends exactly its displayed cost and applies the previewed damage
- PASS: Interactable UI: Fire rate upgrade
- PASS: Rate button spends exactly its displayed cost and applies the previewed interval
- PASS: Successful purchase has visible feedback
- PASS: Capture restores Canvas state: upgrades
- PASS: Capture restores Canvas state: upgrades-4x3
- PASS: Capture restores Canvas state: upgrades-small
- PASS: Interactable UI: Close upgrades
- PASS: Kill credits currency exactly once before its animation
- PASS: HUD uses the undecided resource name without a currency unit
- PASS: Repeated presentation refresh cannot duplicate a reward
- PASS: Resource pickup flies in the UI layer while combat is paused
- PASS: Capture restores Canvas state: resource-gain
- PASS: Resource flights return to their fixed-size pool after arrival
- PASS: Flight arrival does not grant currency again
- PASS: Interactable UI: Upgrade panel toggle
- PASS: M replaces upgrades with the modal map and pauses running combat
- PASS: Interactable UI: Region 0
- PASS: Locked district travel remains disabled
- PASS: Map pin is above the modal backdrop in actual UI raycasts
- PASS: Capture restores Canvas state: map
- PASS: Capture restores Canvas state: map-4x3
- PASS: Capture restores Canvas state: map-small
- PASS: Live Game View modal reviewed separately from camera-mode captures

Failure: Escape closes the map and restores running combat
  at HunterGrandma.Editor.GrandmaUiValidation.Check (System.Boolean condition, System.String message) [0x00009] in E:\unity_project\project10_hunter_grandma\Hunter_grandma\Assets\HunterGrandma\Editor\GrandmaUiValidation.cs:51 
  at HunterGrandma.Editor.GrandmaUiValidation.Tick () [0x0097f] in E:\unity_project\project10_hunter_grandma\Hunter_grandma\Assets\HunterGrandma\Editor\GrandmaUiValidation.cs:150 