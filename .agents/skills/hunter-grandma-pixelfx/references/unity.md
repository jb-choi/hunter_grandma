# Unity import and integration

Only install/import when the task requests Unity assets/integration. Review-only VFX tasks can finish in Library. Read `Assets/Artsource/AGENTS.md` before saving art into that subtree.

## Import a reviewed export

1. Export a fresh production version to `Assets/HunterGrandma/Art/VFX/<effect>/v001` using the wrapper. Keep its recipe and notices. Add the generated Unity metadata when committing.
2. If not already installed, copy this skill's `assets/PixelFxSheetImporter.cs` to `Assets/HunterGrandma/Editor/PixelFxSheetImporter.cs`. Let Unity compile and check Console. It is editor-only and has no extra package requirement. If that destination already exists, inspect it; do not overwrite another implementation blindly.
3. Select the exported `<name>.pixelfx.json` in Project, then choose **Assets → Hunter Grandma → Import PixelFX Manifest**. The importer reads individual PNG frames and creates `<name>.anim`; it does not depend on Unity automatically reading third-party atlas JSON.
4. Verify frame count/order, source dimensions, exact origin, full frame duration, loop, PPU32 (or chosen recipe PPU), Point filtering, no mipmaps, uncompressed and FullRect. The importer refuses an existing clip so reruns cannot silently replace a user-authored asset. Choose a new export version or make an explicit reviewed update.

The result is a SpriteRenderer AnimationClip and frame assets. It does not install an Animator, change a scene, or connect a damage event. A standalone preview can play it with a temporary Animator, but production may use frame arrays/durations through the existing custom presentation clock instead. Avoid introducing an Animator dependency just to use the exported frames.

## Connect only the requested behavior

- Use the actual muzzle or impact point and actor facing; join the relevant actor's SortingGroup or explicit world-depth convention.
- Ordinary combat presentation should follow the encounter's CombatDeltaTime. Shadow-skill effects use its separate cinematic clock. A clip driven by default Animator time does not automatically honor hit-stop, map pause or skill freeze.
- Reuse the game's bounded pooling conventions for frequent effects. Cancel/reset visible effects on completion, retry/travel, skill cancellation and disable as applicable.
- Retain existing hit detection, damage, gold/collect credit and hit-stop authority. Upstream `act.hits` is reference timing, not a second damage source or an instruction to freeze the whole game.
- Keep user-edited Aseprite/animation originals, source GUIDs and unrelated scene/settings data intact. Do not ship the grey stock actor or Patch Pixel font.

## Evidence

Compile the editor/runtime change, read Console, and run the smallest relevant scenario. Check right/left placement, repeated triggers, ending cleanup and pause/retry with an isolated save. For a visual claim, inspect an actual rendered frame/motion at the game's intended size. An importer test or null-graphics run is not native Game View or gameplay verification. Report export, import, playback, gameplay and visual evidence separately.

For the packaged importer regression, use a disposable Unity project with the same Editor version: put the example export in `Assets/PixelFXSkillValidation/impact`, and copy `assets/PixelFxSheetImporter.cs` plus `Tools/PixelFX/tests/PixelFxSkillValidation.cs` into its `Assets/Editor`. Run Unity with `-batchmode -nographics -projectPath <disposable-project> -executeMethod HunterGrandma.Editor.PixelFxSkillValidation.RunBatch -logFile <log-path>`. The fixture exits with 0/1 and writes `Library/PixelFXSkill-validation.json`. It imports the fixed 28-frame example, verifies all frame references, origins, settings and timestamps, samples the clip on a temporary SpriteRenderer+Animator, and checks overwrite/path guards. A non-legacy clip requires an Animator for `SampleAnimation`; a SpriteRenderer alone is not a valid sampling fixture. Use a fresh export directory (no existing `.anim`) for each run.
