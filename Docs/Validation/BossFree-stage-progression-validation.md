# Boss-free automatic stage progression validation

Unity 6000.3.10f1 · 2026-10-30 01:40

Status: PASS

Isolated save: `Library/BeltScroll-validation.json`. The player profile is never reset or modified by this suite. Ordinary combat, automatic travel and replay run naturally; earned funds buy upgrades between stages. Synthetic keyboard input is game-test input, without computer-use tools. Legacy capture model cases verify old-save compatibility only.

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
- PASS: Wave-only completion preserves legacy captives and first-clear time without granting another captive
- PASS: Mixed historical captures and wave-only clears round-trip in the existing save schema
- PASS: Missing and unfinished wave runs cannot unlock a stage
- PASS: Wave completion rejects locked or invalid stage IDs
- PASS: Retreat or failure cannot settle a marked wave run
- PASS: Stage 0: wave-only settlement records one clear and no captive
- PASS: Stage 0: the same wave run cannot settle twice or grant a legacy capture
- PASS: Stage 1: wave-only settlement records one clear and no captive
- PASS: Stage 1: the same wave run cannot settle twice or grant a legacy capture
- PASS: Stage 2: wave-only settlement records one clear and no captive
- PASS: Stage 2: the same wave run cannot settle twice or grant a legacy capture
- PASS: Stage 3: wave-only settlement records one clear and no captive
- PASS: Stage 3: the same wave run cannot settle twice or grant a legacy capture
- PASS: Stage 4: wave-only settlement records one clear and no captive
- PASS: Stage 4: the same wave run cannot settle twice or grant a legacy capture
- PASS: Stage 5: wave-only settlement records one clear and no captive
- PASS: Stage 5: the same wave run cannot settle twice or grant a legacy capture
- PASS: Stage 6: wave-only settlement records one clear and no captive
- PASS: Stage 6: the same wave run cannot settle twice or grant a legacy capture
- PASS: All seven wave-only clears survive JSON reload with sequential unlocks
- PASS: A save with fewer clears than historical captive records is rejected
- PASS: A save with cleared stages beyond a locked predecessor is rejected
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
- PASS: W+D moves grandma right and into the belt depth
- PASS: S+A moves grandma left and toward the foreground
- PASS: Held downward input stops at the near belt boundary
- PASS: Held upward input stops at the far belt boundary
- PASS: Camera scrolls horizontally with forward travel
- PASS: Current encounter limits prevent skipping the active wave
- PASS: Clickable UI: 지도  M
- PASS: Movement input is blocked while the map owns pause
- PASS: Clickable UI: Close map
- PASS: Overlapping live actors use feet to determine their actual sorting groups
- PASS: Every live alien has an independent grounded foot shadow
- PASS: A real shotgun flight melts the aligned alien while the off-lane alien remains unharmed
- PASS: Tab input turns automatic exploration back on
- PASS: Automatic hunting approaches enemies on X/Y and fires without movement input
- PASS: Stage 0: waves settle directly to CLEAR at the hunting location
- PASS: Stage 0: every configured normal zombie is defeated before CLEAR
- PASS: Stage 0: no boss, cart settlement or fabricated captive
- PASS: Stage 0: exactly one clear is credited for this run
- PASS: Stage 0: CLEAR stops movement/combat and has no boss HUD
- PASS: Stage 0: wave-only clear is saved before advancing
- PASS: Stage 0: CLEAR unlocks the next stage and schedules automatic travel
- PASS: Stage 0: later stages stay locked
- PASS: Clickable UI: 지도  M
- PASS: Map pause freezes the CLEAR countdown without duplicating its settlement
- PASS: Clickable UI: Close map
- PASS: Closing the map resumes automatic stage transition
- PASS: Automatically enter Stage 1 without a map selection or EnterStage call
- PASS: Stage 1: new run counters/health reset while the campaign continues
- PASS: Stage 1: gold, upgrades and the previous clear survive automatic travel
- PASS: Stage 1: waves settle directly to CLEAR at the hunting location
- PASS: Stage 1: every configured normal zombie is defeated before CLEAR
- PASS: Stage 1: no boss, cart settlement or fabricated captive
- PASS: Stage 1: exactly one clear is credited for this run
- PASS: Stage 1: CLEAR stops movement/combat and has no boss HUD
- PASS: Stage 1: wave-only clear is saved before advancing
- PASS: Stage 1: CLEAR unlocks the next stage and schedules automatic travel
- PASS: Stage 1: later stages stay locked
- PASS: Automatically enter Stage 2 without a map selection or EnterStage call
- PASS: Stage 2: new run counters/health reset while the campaign continues
- PASS: Stage 2: gold, upgrades and the previous clear survive automatic travel
- PASS: Stage 2: waves settle directly to CLEAR at the hunting location
- PASS: Stage 2: every configured normal zombie is defeated before CLEAR
- PASS: Stage 2: no boss, cart settlement or fabricated captive
- PASS: Stage 2: exactly one clear is credited for this run
- PASS: Stage 2: CLEAR stops movement/combat and has no boss HUD
- PASS: Stage 2: wave-only clear is saved before advancing
- PASS: Stage 2: CLEAR unlocks the next stage and schedules automatic travel
- PASS: Stage 2: later stages stay locked
- PASS: Automatically enter Stage 3 without a map selection or EnterStage call
- PASS: Stage 3: new run counters/health reset while the campaign continues
- PASS: Stage 3: gold, upgrades and the previous clear survive automatic travel
- PASS: Stage 3: waves settle directly to CLEAR at the hunting location
- PASS: Stage 3: every configured normal zombie is defeated before CLEAR
- PASS: Stage 3: no boss, cart settlement or fabricated captive
- PASS: Stage 3: exactly one clear is credited for this run
- PASS: Stage 3: CLEAR stops movement/combat and has no boss HUD
- PASS: Stage 3: wave-only clear is saved before advancing
- PASS: Stage 3: CLEAR unlocks the next stage and schedules automatic travel
- PASS: Stage 3: later stages stay locked
- PASS: Automatically enter Stage 4 without a map selection or EnterStage call
- PASS: Stage 4: new run counters/health reset while the campaign continues
- PASS: Stage 4: gold, upgrades and the previous clear survive automatic travel
- PASS: Stage 4: waves settle directly to CLEAR at the hunting location
- PASS: Stage 4: every configured normal zombie is defeated before CLEAR
- PASS: Stage 4: no boss, cart settlement or fabricated captive
- PASS: Stage 4: exactly one clear is credited for this run
- PASS: Stage 4: CLEAR stops movement/combat and has no boss HUD
- PASS: Stage 4: wave-only clear is saved before advancing
- PASS: Stage 4: CLEAR unlocks the next stage and schedules automatic travel
- PASS: Stage 4: later stages stay locked
- PASS: Automatically enter Stage 5 without a map selection or EnterStage call
- PASS: Stage 5: new run counters/health reset while the campaign continues
- PASS: Stage 5: gold, upgrades and the previous clear survive automatic travel
- PASS: Stage 5: waves settle directly to CLEAR at the hunting location
- PASS: Stage 5: every configured normal zombie is defeated before CLEAR
- PASS: Stage 5: no boss, cart settlement or fabricated captive
- PASS: Stage 5: exactly one clear is credited for this run
- PASS: Stage 5: CLEAR stops movement/combat and has no boss HUD
- PASS: Stage 5: wave-only clear is saved before advancing
- PASS: Stage 5: CLEAR unlocks the next stage and schedules automatic travel
- PASS: Automatically enter Stage 6 without a map selection or EnterStage call
- PASS: Stage 6: new run counters/health reset while the campaign continues
- PASS: Stage 6: gold, upgrades and the previous clear survive automatic travel
- PASS: Stage 6: waves settle directly to CLEAR at the hunting location
- PASS: Stage 6: every configured normal zombie is defeated before CLEAR
- PASS: Stage 6: no boss, cart settlement or fabricated captive
- PASS: Stage 6: exactly one clear is credited for this run
- PASS: Stage 6: CLEAR stops movement/combat and has no boss HUD
- PASS: Stage 6: wave-only clear is saved before advancing
- PASS: The last stage ends the finite campaign without a nonexistent next stage
- PASS: Final CLEAR remains stable for multiple transition intervals and grants no repeated clear
- PASS: Every observed natural combat/transition frame remained free of bosses and capture phases
- PASS: All seven stages completed; replay Stage 0
- PASS: Stage 0: waves settle directly to CLEAR at the hunting location
- PASS: Stage 0: every configured normal zombie is defeated before CLEAR
- PASS: Stage 0: no boss, cart settlement or fabricated captive
- PASS: Stage 0: exactly one clear is credited for this run
- PASS: Stage 0: CLEAR stops movement/combat and has no boss HUD
- PASS: Stage 0: wave-only clear is saved before advancing
- PASS: Replaying Stage 0 preserves first-clear time and increments only the clear count
- PASS: Clickable UI: 지도  M
- PASS: Clickable UI: Region 5
- PASS: Completed Stage 6 remains selectable for another wave-only hunt
- PASS: Clickable UI: Close map
- PASS: A completed district can be re-entered
- PASS: Retreat preserves earned gold and historical clears without another clear or captive
- PASS: Failure keeps progress and does not settle the unfinished wave run
- PASS: Disk reload retains all seven wave clears, replay count, gold and skill preference without captives
- PASS: Normal zombies visibly melt; plaza waves approach from both sides
- PASS: Retry resets combat while keeping permanent progress
- PASS: No runtime errors, exceptions or assertions in the complete boss-free stage suite

- Stage 0: 19.8 simulated seconds, 14 kills, 14 shots, 100 HP; no boss/capture
- Stage 1: 40.8 simulated seconds, 30 kills, 50 shots, 100 HP; no boss/capture
- Stage 2: 37.8 simulated seconds, 30 kills, 49 shots, 100 HP; no boss/capture
- Stage 3: 48.2 simulated seconds, 36 kills, 45 shots, 100 HP; no boss/capture
- Stage 4: 56.8 simulated seconds, 36 kills, 36 shots, 100 HP; no boss/capture
- Stage 5: 49.6 simulated seconds, 44 kills, 75 shots, 100 HP; no boss/capture
- Stage 6: 59.8 simulated seconds, 48 kills, 62 shots, 100 HP; no boss/capture
- Stage 0: 19.8 simulated seconds, 14 kills, 14 shots, 100 HP; no boss/capture

Separate Unity Editor Search startup/indexing errors (not gameplay):
ArgumentOutOfRangeException: Index was out of range. Must be non-negative and less than the size of the collection.
Parameter name: index
System.Collections.Generic.List`1[T].get_Item (System.Int32 index) (at <41d29b352f6a475ab1bf7c6628b82790>:0)
UnityEditor.Search.SearchDatabase+<EnumerateAll>d__80.MoveNext () (at <e5e60b34d51d47fbbbfacfb81405fd89>:0)
System.Linq.Enumerable.TryGetFirst[TSource] (System.Collections.Generic.IEnumerable`1[T] source, System.Boolean& found) (at <80564d689e48448fa2a7101993ee7201>:0)
System.Linq.Enumerable.First[TSource] (System.Collections.Generic.IEnumerable`1[T] source) (at <80564d689e48448fa2a7101993ee7201>:0)
UnityEditor.Search.SearchDatabase.GetDefaultSearchDatabase () (at <e5e60b34d51d47fbbbfacfb81405fd89>:0)
UnityEditor.Search.SearchInit.IndexationOnStartup () (at <e5e60b34d51d47fbbbfacfb81405fd89>:0)
UnityEditor.EditorApplication.Internal_CallDelayFunctions () (at <49fcb209645d4ccfb3e373da32345d37>:0)


Execution: isolated Unity batchmode / nographics. Native UI/computer-use tools and visual screenshot verification were not used.
