# Boss-free automatic stage progression validation

Unity 6000.3.10f1 · 2026-10-30 01:36

Status: FAIL

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



Failure: M did not open the map after game frames advanced: 11497
  at HunterGrandma.Editor.GrandmaStageValidation.Tick () [0x0020e] in E:\unity_project\project10_hunter_grandma\Hunter_grandma\Library\BossFreeStageBatch\Assets\HunterGrandma\Editor\GrandmaStageValidation.cs:184

Runtime errors:
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
