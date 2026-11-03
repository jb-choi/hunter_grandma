# Hunter Grandma PixelFX skill validation

Created 2026-11-03; final verification completed 2026-11-04 KST.

Status: PASS for skill packaging, export and isolated Unity import/clip sampling. Combat integration was not requested or performed.

## Delivered

- Repo-local `.agents/skills/hunter-grandma-pixelfx/SKILL.md`, automatic-selection description, Korean UI metadata and example invocation.
- Task-specific art/export/Unity references, a deterministic Node exporter and a versioned example recipe.
- Optional editor-only `assets/PixelFxSheetImporter.cs` template. It is not installed into the original game's Assets folder.
- Vendored pixelfx 0.3.0 at `a84159688af3ca37a4be05a0d422b94451b2e24e`, with 301 retained files / approximately 2.4 MB, recorded SHA-256 hashes and original license/notices. Promotional GIFs, gallery/fonts and Claude plugin registration are excluded.
- Root `THIRD_PARTY_NOTICES.md`, Node regression tests and a reusable isolated Unity importer test fixture under `Tools/PixelFX/tests`.

The wrapper exports a transparent PNG atlas + Aseprite-style JSON, individual transparent PNG frames, Unity manifest, recipe, dark/light contact sheets and license/notices. Node >=18 is sufficient; no npm dependency is required. Optional upstream GIF/MP4 tools need ffmpeg.

## Verification

| Check | Result |
| --- | --- |
| Skill Creator quick_validate.py | PASS; frontmatter/name/scaffold validation |
| node --test Tools/PixelFX/tests/export.test.mjs | 8 PASS, 0 failures |
| Pinned source verification | All 301 retained files match their lock hashes |
| Example export | impacts/blunt-hit, seed 42, 128×88, 28 frames, 30 fps, 32 PPU |
| Independent PNG readback | All 28 atlas rectangles match individual-frame RGBA bytes; all have transparent pixels |
| Visual review | Both dark/light contact sheets opened; observed initial impulse, spokes/dust and fade; no motion/native Game View quality claim |
| Unity 6000.3.10f1 import and sampling | 149 checks PASS in isolated batchmode/nographics project |
| Original project preservation | All 109 protected runtime/editor/scene/settings/package/save files unchanged |
| Patch Pixel font files | No gallery or TTF/OTF/WOFF font files copied into the vendored subset |

Node checks cover source integrity, byte-identical repeated renders, seed variation, atlas/frame straight-RGBA equality, user-file preservation on retry, invalid paths/conflicting seed input, stock-body effect exclusion and loop export.

Unity checks cover every frame's asset reference/order, untrimmed dimensions, Point/no-mip/uncompressed import, PPU/origin conversion and timestamp; clip duration/frame rate/one-shot settings; actual middle/final SpriteRenderer frame sampling on a temporary Animator; refusal to overwrite an existing clip; and rejection of a manifest path escape. The test did not open or save the original game scene and did not enter the ordinary player profile.

## Corrections discovered by execution

1. Upstream transparent atlas paste premultiplies RGB against empty cells, so its sheet differed visibly from separately exported frames (5,487 differing visible pixels in the first sample frame). The wrapper now copies straight RGBA rows into the same upstream JSON layout. Pixel readback is a retained Node regression.
2. An extra terminal PPtr key extended the Unity clip by a frame. The importer now uses double-precision accumulated durations, one key per frame and explicit clip stop time. Actual clip length is 28/30 seconds.
3. The first sampling fixture had a SpriteRenderer without an Animator; a non-legacy clip sampled as null. A read-only probe established that adding an Animator displays the correct frame. The final fixture uses that valid preview setup. This did not require production importer or gameplay changes.

The source vendor remains byte-identical; all corrections are in the adapter/template. Failed reports are preserved rather than overwritten as successes.

## Reproduction and evidence

From the Git/Unity root:

```text
node .agents/skills/hunter-grandma-pixelfx/scripts/pixelfx.mjs verify
node --test Tools/PixelFX/tests/export.test.mjs
node .agents/skills/hunter-grandma-pixelfx/scripts/pixelfx.mjs export --recipe .agents/skills/hunter-grandma-pixelfx/assets/impact-example.json --out Library/PixelFX/impact-review-v001
```

Use a fresh output version if that directory exists. The reusable Unity fixture setup is described in the skill's `references/unity.md`.

Saved evidence:

- `Docs/Validation/PixelFXSkill-unity.json`: final 149-check Unity report.
- `Docs/Validation/PixelFXSkill-png.json`: independent atlas/frame readback.
- `Docs/Validation/PixelFXSkill-preservation.json`: protected-file comparison.
- `Docs/Validation/PixelFXSkill-impact-preview.png`: reviewed sample contact sheet.
- `Library/PixelFXSkillSetup-20261103/`: original source archive, prepared examples, failed Unity attempts and diagnostic PNG script.
- `Library/BossFreeStageBatch/pixelfx-skill-final.log`: final Unity batch log.
- `Library/BossFreeStageBatch/pixelfx-skill-probe2.log`: Animator sampling probe.

The packaged Unity regression fixture uses generic `Assets/PixelFXSkillValidation/impact`; this run used the isolated dated `impact-v003` folder to preserve preceding failures.

## Limits

No original scene/runtime code, user art, gameplay timing, damage/rewards or player save was modified. No actual combat VFX placement, pause/retry behavior in the running game, native Game View visual QA, player build, hardware profiling, or Unity loop-clip playback is claimed. Node loop export was tested separately. Install the template and connect only the requested effect when a later task asks for Unity integration.

The quick validator needed PyYAML; version 6.0.2 was installed only into an ignored local validator-deps folder, without changing the project's or user's global dependencies. The delivered rendering tools do not use it. Share the skill, tools and notices through a reviewed Git commit so other worktrees/cloud checkouts receive the same pinned files.
