# Sundeok 120px animation validation

Unity 6000.3.10f1 · 2026-11-02 01:52 KST

Status: PASS

Isolated profile: Library/Sundeok120-validation.json. After one natural hunting second, prior projectiles are cancelled and dead/disabled pool slots are kept active to prevent extra spawns. One hidden reserved target allows real encounter movement/aim updates without new gameplay shots. Synthetic keyboard input drives movement; actual animation/aim/fire/collect APIs run. One synthetic +7 reward verifies that Collect does not credit currency again. No ordinary save reset.

- PASS: Scene explicitly references the 120px animation asset
- PASS: Automated validation keeps hardware output muted without disabling source playback
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
- PASS: Body plus exported upper layers reconstruct all 25 active source poses pixel-for-pixel
- PASS: Tactical walking Aim matches the authored upper placement and muzzle on gait frame 1
- PASS: Tactical walking Aim matches the authored upper placement and muzzle on gait frame 2
- PASS: Tactical walking Aim matches the authored upper placement and muzzle on gait frame 3
- PASS: Tactical walking Aim matches the authored upper placement and muzzle on gait frame 4
- PASS: Tactical walking Aim matches the authored upper placement and muzzle on gait frame 5
- PASS: Tactical walking Aim matches the authored upper placement and muzzle on gait frame 6
- PASS: Walk, held Aim and Fire explicitly use tactical exports
- PASS: Stationary Aim holds the exact tactical ready pose without a legacy stance jump
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

Separate Unity Editor Search startup/indexing errors:
ArgumentOutOfRangeException: Index was out of range. Must be non-negative and less than the size of the collection.
Parameter name: index
System.Collections.Generic.List`1[T].get_Item (System.Int32 index) (at <41d29b352f6a475ab1bf7c6628b82790>:0)
UnityEditor.Search.SearchDatabase+<EnumerateAll>d__80.MoveNext () (at <e5e60b34d51d47fbbbfacfb81405fd89>:0)
System.Linq.Enumerable.TryGetFirst[TSource] (System.Collections.Generic.IEnumerable`1[T] source, System.Boolean& found) (at <80564d689e48448fa2a7101993ee7201>:0)
System.Linq.Enumerable.First[TSource] (System.Collections.Generic.IEnumerable`1[T] source) (at <80564d689e48448fa2a7101993ee7201>:0)
UnityEditor.Search.SearchDatabase.GetDefaultSearchDatabase () (at <e5e60b34d51d47fbbbfacfb81405fd89>:0)
UnityEditor.Search.SearchInit.IndexationOnStartup () (at <e5e60b34d51d47fbbbfacfb81405fd89>:0)
UnityEditor.EditorApplication.Internal_CallDelayFunctions () (at <49fcb209645d4ccfb3e373da32345d37>:0)


Batch execution mutes AudioListener output; no computer-use tools or ordinary save edits.
