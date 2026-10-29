# Sundeok 120px animation validation

Unity 6000.3.10f1 · 2026-10-29 13:32 KST

Status: FAIL

Isolated profile: Library/Sundeok120-validation.json. After one natural hunting second, the encounter driver and prior projectiles are stopped to exercise actual movement/aim/fire/collect presentation APIs in isolation. One synthetic +7 reward verifies that Collect does not credit currency again. No ordinary save reset.

- PASS: Scene explicitly references the 120px animation asset
- PASS: Idle preserves the authored frame count and durations
- PASS: Idle has complete composite/body/upper sprites with the 120px foot pivot and PPU
- PASS: Walk preserves the authored frame count and durations
- PASS: Walk has complete composite/body/upper sprites with the 120px foot pivot and PPU
- PASS: Reload preserves the authored frame count and durations
- PASS: Reload has complete composite/body/upper sprites with the 120px foot pivot and PPU
- PASS: Aim preserves the authored frame count and durations
- PASS: Aim has complete composite/body/upper sprites with the 120px foot pivot and PPU
- PASS: Fire preserves the authored frame count and durations
- PASS: Fire has complete composite/body/upper sprites with the 120px foot pivot and PPU
- PASS: Collect preserves the authored frame count and durations
- PASS: Collect has complete composite/body/upper sprites with the 120px foot pivot and PPU
- PASS: Body plus exported upper layers reconstruct all 27 source composites pixel-for-pixel
- PASS: Layer registration matches source demo frame 2
- PASS: Layer registration matches source demo frame 3
- PASS: Layer registration matches source demo frame 4
- PASS: Layer registration matches source demo frame 5
- PASS: Layer registration matches source demo frame 6
- PASS: Layer registration matches source demo frame 7
- PASS: Layer registration matches source demo frame 8
- PASS: Layer registration matches source demo frame 9
- PASS: Layer registration matches source demo frame 10
- PASS: Layer registration matches source demo frame 11
- PASS: Layer registration matches source demo frame 12
- PASS: Layer registration matches source demo frame 13
- PASS: Tea opening hides both new world renderers
- PASS: Natural hunting after the intro uses the new sprite animation and actual automatic shots
- PASS: Stationary Sundeok plays the authored Idle composite
- PASS: Idle advances frames without moving the logical feet

Failure: Movement API advances the authored Walk frames and the actual root
  at HunterGrandma.Editor.SundeokAnimationValidation.Check (System.Boolean value, System.String message) [0x00009] in E:\unity_project\project10_hunter_grandma\Hunter_grandma\Assets\HunterGrandma\Editor\SundeokAnimationValidation.cs:45 
  at HunterGrandma.Editor.SundeokAnimationValidation.Tick () [0x0071c] in E:\unity_project\project10_hunter_grandma\Hunter_grandma\Assets\HunterGrandma\Editor\SundeokAnimationValidation.cs:116 