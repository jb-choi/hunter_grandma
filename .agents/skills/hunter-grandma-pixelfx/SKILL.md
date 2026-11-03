---
name: hunter-grandma-pixelfx
description: "Create, adapt and export pixel VFX with pixelfx for Hunter Grandma, including visual review and Unity import. Use for muzzle flashes, impacts, skill effects and pickup feedback; not character animation or combat-rule design."
---

# Hunter Grandma PixelFX

Use the vendored pixelfx tools to make reproducible pixel effects that fit this game's art and clocks. Keep gameplay ownership in the existing combat code.

## Start with the requested outcome

- For reference selection or previews, stop at reviewed candidates; do not install an importer or change the scene.
- For production assets, export a versioned PNG/JSON package with recipe, seed, timing, origin and source provenance.
- For an explicitly requested Unity integration, also follow [references/unity.md](references/unity.md). A successful export or import alone does not prove the effect works in combat.

Repository root is the folder containing `Assets`, `Packages` and `ProjectSettings`. Paths below are relative to it. This repo-local skill and `Tools/PixelFX` must be committed together for other worktrees/cloud checkouts. Node >=18 is required; no npm install is needed for these tools. GIF/MP4 previews optionally use ffmpeg.

## Choose and make the effect

1. Read [references/art-direction.md](references/art-direction.md) for current project constraints and useful effect searches. Inspect the relevant existing gameplay/animation owner before integration.
2. Verify the fixed source and search candidates:

   ```text
   node .agents/skills/hunter-grandma-pixelfx/scripts/pixelfx.mjs verify
   node .agents/skills/hunter-grandma-pixelfx/scripts/pixelfx.mjs list impact
   ```

3. Create a recipe from [assets/impact-example.json](assets/impact-example.json). Match direction, size, palette and duration to the actual request. For a new effect, read `Tools/PixelFX/vendor/pixelfx/docs/AUTHORING.md`; place original/adapted modules in `Tools/PixelFX/custom/fx/<category>/<name>.js` and set recipe `source` to `custom`. Keep the pinned vendor files unchanged.
4. Export to a fresh review folder:

   ```text
   node .agents/skills/hunter-grandma-pixelfx/scripts/pixelfx.mjs export --recipe .agents/skills/hunter-grandma-pixelfx/assets/impact-example.json --out Library/PixelFX/impact-review-v001
   ```

   The exporter refuses nonempty destinations. Change the version/folder when refining; preserve user edits. It produces a transparent sheet, Aseprite-style JSON, individual frames, a Unity manifest, dark/light contact sheets and a reproduction recipe. See [references/export.md](references/export.md) for options and output contracts.
5. Open both contact sheets with the available image-viewing tool. Judge real-size readability, peak/silhouette, color, origin, clipping and final-frame cleanup. Use a motion preview when timing cannot be assessed from still frames. Compare variants when the visual direction is open; refine the concrete weak points rather than imposing a fixed number of drafts.

## Deliver and verify

Record which effect/recipe and source commit were used, what was visually inspected, where the outputs are, and whether Unity import, playback or combat behavior was actually tested. On integration, retain existing pause, depth sorting, pooling and retry/disable cleanup; do not add pixelfx's demo actor, hit-stop or damage rules to the game implicitly. `onBody` effects require adapting to the actual character instead of shipping the stock dummy.

Keep MIT license/notices with derived outputs and in the project's third-party notice. `gallery/fonts` (Patch Pixel) is excluded. Skill activation is task-based, not permission to modify unrelated code or to install Claude plugins.
