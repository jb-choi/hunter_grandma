# Sundeok 120px animation validation

Unity 6000.3.10f1 · 2026-10-29 16:17 KST

Status: PASS

Isolated profile: Library/Sundeok120-validation.json. After one natural hunting second, prior projectiles are cancelled and dead/disabled pool slots are kept active to prevent extra spawns. One hidden reserved target allows real encounter movement/aim updates without new gameplay shots. Synthetic keyboard input drives movement; actual animation/aim/fire/collect APIs run. One synthetic +7 reward verifies that Collect does not credit currency again. No ordinary save reset.

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
- PASS: Layer registration matches source demo frame 1
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
- PASS: Tea opening hides both new world renderers
- PASS: Natural hunting after the intro uses the new sprite animation and actual automatic shots
- PASS: Stationary Sundeok plays the authored Idle composite
- PASS: Idle advances frames without moving the logical feet
- PASS: Movement API advances the authored Walk frames and the actual root
- PASS: Walking continues underneath the authored Aim upper body
- PASS: Layered aiming uses the weapon-free Walk body without duplicate arms or guns
- PASS: A committed shot starts on the authored muzzle-flash frame while walking
- PASS: Right-facing shot uses a muzzle in front of the character
- PASS: Shot recovery transitions into the authored Reload while the legs keep walking
- PASS: Pause freezes clip time, pose and feet together
- PASS: Reload returns to Idle when combat and movement stop
- PASS: A queued pickup plays the authored bending Collect composite while safe and stationary
- PASS: Collect presentation neither grants a duplicate reward nor moves the feet
- PASS: Keyboard movement interrupts Collect without repeating or removing its reward
- PASS: Left-facing fire mirrors the sprite and exact authored muzzle around the foot pivot
- PASS: Isolated visual Fire calls cannot create gameplay shots or damage
- PASS: Retry clears aim/fire/reload/collect clocks and layered state
- PASS: No runtime errors, exceptions or assertions in animation validation