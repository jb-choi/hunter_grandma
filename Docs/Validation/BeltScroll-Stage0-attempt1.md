# 2D belt-scroll implementation validation

Unity 6000.3.10f1 · 2026-10-28 01:35

Status: FAIL

Isolated save: `Library/BeltScroll-validation.json`. The player profile is never reset or modified by this suite.

- PASS: New profile unlocks only Stage 0; invalid stage IDs are rejected
- PASS: Upgrades cannot spend unavailable gold
- PASS: No capture before boss subdual
- PASS: No capture before yard arrival
- PASS: Yard arrival settles one captive
- PASS: The same run cannot settle twice
- PASS: Repeat clear creates a distinct captive and source run ID
- PASS: Repeat clear preserves first-clear timestamp and sequential unlock
- PASS: Abandoning a run prevents captive rewards
- PASS: Attack upgrade spends gold and raises actual shot damage
- PASS: Fire-rate upgrade spends gold and shortens the actual shot interval
- PASS: Save JSON round-trip preserves gold, IDs, and stage records
- PASS: Unsupported save versions are rejected
- PASS: Atomic disk replacement produces a readable save and backup
- PASS: A corrupt primary save recovers the last valid backup
- PASS: Upgrade limits prevent overflow purchases
- PASS: Shotgun hits an aligned target inside range
- PASS: A target in a different depth lane is outside the shotgun cone
- PASS: Shotgun rejects targets behind the shot or beyond its range
- PASS: A left-facing shot has the same depth and range rules
- PASS: Solid cover blocks shotgun hits behind it
- PASS: Movement cannot tunnel through a cover footprint
- PASS: Automatic navigation goes around cover and reaches the other side
- PASS: Belt world boundaries clamp both movement axes
- PASS: Lower feet render in front of upper feet independent of sprite height
- PASS: Playable scene starts Stage 0 with live zombies and a bounded pool
- PASS: Gameplay rejects locked district travel
- PASS: M opens the map and pauses the encounter
- PASS: Clickable UI: Region 0
- PASS: Locked map district shows a disabled travel button
- PASS: Map pin receives the actual pointer raycast above the modal backdrop
- PASS: Stage 0 is separately selectable outside the six city pins
- PASS: Escape closes the map and restores the prior game speed
- PASS: Clickable UI: 지도  M
- PASS: Clickable UI: Close map
- PASS: Closing a map opened while already paused preserves pause



Failure: W+D moves grandma right and into the belt depth
  at HunterGrandma.Editor.GrandmaStageValidation.Check (System.Boolean value, System.String description) [0x00009] in E:\unity_project\project10_hunter_grandma\Hunter_grandma\Assets\HunterGrandma\Editor\GrandmaStageValidation.cs:70 
  at HunterGrandma.Editor.GrandmaStageValidation.Tick () [0x0054b] in E:\unity_project\project10_hunter_grandma\Hunter_grandma\Assets\HunterGrandma\Editor\GrandmaStageValidation.cs:174 