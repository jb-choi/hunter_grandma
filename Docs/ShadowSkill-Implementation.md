# 순덕 할매: 할매의 밤마실

Implemented 2026-10-29. Entry: **U → 순덕 할매 → 오른쪽 화살표 → 할매의 밤마실 ON/OFF**. Skill setting is free, defaults OFF, and persists with the existing profile. The first card in the detail list shows ON/OFF and remaining combat seconds. Both original purchases remain below it in the scrolling detail list.

## Behavior

- ON starts a ten-second cooldown. Only Waves/BossFight combat time counts; opening, wave breaks, boss arrival, capture, results, user/map pause, hit stop and the skill sequence do not count. After each sequence there are another ten combat seconds before the next activation. Toggling or starting/retrying/traveling to a stage resets the cooldown.
- Select living **ordinary zombies** in front of the current facing direction, within the camera's safe inner viewport. Targets whose rear position cannot fit on the belt are excluded. Bosses are not targets of this zombie execution sequence.
- Visit different candidates by distance, then revisit survivors if fewer than five are present. No targets means remain ready; all candidates dead means return early.
- Freeze encounter progression, normal projectiles, zombie/boss movement and the camera through CombatDeltaTime. The hidden real grandma retains her exact foot position/facing and is protected from damage.
- A reused presentation actor vanishes into eight dark wisps, appears behind each frozen target, fires with the existing Sundeok animation, leaves an afterimage, and returns. The veil dims world art but leaves HUD/menu controls readable. Targets remain recognizable. Headshot traces/sparks use a visual head anchor; normal shotgun collision/hit points are unchanged.
- Up to five hits; each deals **2 × current attack damage** through ZombieActor.ReceiveDamage. Existing kill, resource, save and duplicate-reward protection are reused. The last shot holds slightly longer and uses a louder shot sound.
- Full sequence is approximately 2 seconds, including the final hold. Effects use existing sprites/materials and are created once per encounter, then reused. No packages or new source artwork are required.
- Space/map pause also freezes skill presentation. OFF, retry, retreat, stage travel and component disable cancel the sequence, hide its effects and restore the real actor. Return faces the original direction.

## Ownership and files

- `Runtime/GrandmaShadowSkill.cs`: sequence, cooldown, candidate visit order, proxy animation and reusable VFX.
- `Runtime/GrandmaDefense.cs`: composition, shared combat clock, target eligibility, damage protection and lifecycle cleanup.
- `Runtime/GrandmaShooter.cs`: read-only facing/material and reused shot audio.
- `Runtime/ZombieActor.cs`: read-only facing/head anchor; existing damage rules remain authoritative.
- `Runtime/DefenseUpgradeView.cs`: first-card skill setting, countdown and scrollable purchases.
- `Runtime/DefenseProgress.cs`: optional `shadowSkillEnabled` boolean in the existing v1 DTO. Missing field loads false; existing version/path and all other records are preserved.
- `Editor/GrandmaShadowSkillValidation.cs`: isolated Play Mode validation, menu **Tools → Hunter Grandma → Run Shadow Skill Validation**, command `validate-skill` through the existing local command bridge.

No Inspector wiring or scene/prefab edits are required. The existing actor's enabled animation frames and source artwork are reused.

## Validation

Current execution evidence is recorded in `Docs/Validation/ShadowSkill-validation.md`. Screenshots use the `ShadowSkill-` prefix. Ordinary saves are never used by the suite; its profile is `Library/ShadowSkill-validation.json`.

Final Editor validation, 2026-10-29 KST: skill 33 PASS, upgrade UI 103 PASS, full belt/stages 156 PASS, original Sundeok animation 45 PASS, opening/attacks 48 PASS (**385 checks total**). Each suite recorded zero runtime errors/exceptions/assertions. Current sources compiled in the running Unity 6000.3.10f1 Editor. The existing `GrandmaShooter.animation` member-hiding warning remains; the belt suite's intentional corrupt-save recovery warning is expected. Skill/UI captures were visually inspected at 1280×720, 1024×768 and 800×450. Scene, manifest, Player Settings, Build Settings and ordinary save retain their fresh pre-task hashes. See `Docs/Validation/ShadowSkill-summary.md`.

Full player/WebGL build and long-run profiling are outside this change validation.
