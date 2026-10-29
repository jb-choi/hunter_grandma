# Shadow skill validation

Unity 6000.3.10f1 · 2026-10-30 00:38 KST

Status: PASS

Isolated profile: Library/ShadowSkill-validation.json. Three reserved high-health targets run in the actual encounter with all unused pool slots held inactive in behavior but occupied to prevent new spawns. Game time is accelerated 2x; cooldowns are checked in combat seconds. Production save is never loaded/reset.

- PASS: New profiles default to skill OFF
- PASS: Old v1 saves without the new field load with skill OFF
- PASS: Sundeok upgrade details include the skill toggle
- PASS: Toggle ON starts a full ten-second cooldown
- PASS: Skill ON is saved through the existing progress store
- PASS: Visible skill card receives pointer raycasts above the modal shade
- PASS: Skill never fires before ten combat seconds
- PASS: Pause freezes cooldown without activating the skill
- PASS: Automatic activation occurs at ten elapsed combat seconds
- PASS: Skill freezes combat/movement and hides the real actor
- PASS: The hidden actor cannot take damage during the cinematic
- PASS: Combat clock, camera and real position stay fixed during the skill
- PASS: User pause freezes skill progression and enemy movement together
- PASS: Opening the map freezes the active skill
- PASS: Closing the map restores the running skill pause state
- PASS: Headshot spark targets the visual head above the ordinary torso hit point
- PASS: Three living targets receive exactly five shots by revisiting survivors
- PASS: Each shot applies twice the current attack damage exactly once
- PASS: Different targets are visited before a survivor is hit again
- PASS: Normal presentation returns at the exact original foot position
- PASS: ON automatically repeats after the next ten combat seconds
- PASS: Turning OFF during an active sequence safely restores the actor
- PASS: Skill OFF persists on disk
- PASS: OFF prevents automatic skill activation across another full interval
- PASS: Last enemy death ends early and credits exactly one existing kill reward
- PASS: Early completion restores visible grandma
- PASS: No targets leaves the skill ready without consuming an activation
- PASS: A rear-only off-lane zombie is excluded from a front-facing cast
- PASS: Restart cancels the sequence, removes the veil and resets cooldown
- PASS: Retreat restores presentation and cancels the skill
- PASS: Five distinct vulnerable zombies receive five lethal shots and five rewards
- PASS: Disabling the encounter cleans up the cinematic and restores the real actor
- PASS: No runtime errors, exceptions or assertions