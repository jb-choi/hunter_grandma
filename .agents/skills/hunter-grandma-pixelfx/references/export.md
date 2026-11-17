# Source and export contract

`Tools/PixelFX/pixelfx.lock.json` pins upstream 0.3.0 to commit `a84159688af3ca37a4be05a0d422b94451b2e24e` and hashes each retained source file. `verify` checks those bytes before listing/exporting. `src`, tools, docs, original skill and license/notices are vendored; gallery/font/plugin registration files are excluded. No network/bootstrap is required after the files are checked out. Node >=18 is required. The upstream gallery build uses esbuild and is outside this minimal offline workflow.

Commands run from the Unity/Git root:

```text
node .agents/skills/hunter-grandma-pixelfx/scripts/pixelfx.mjs verify
node .agents/skills/hunter-grandma-pixelfx/scripts/pixelfx.mjs list muzzle
node .agents/skills/hunter-grandma-pixelfx/scripts/pixelfx.mjs export --recipe path/to/recipe.json --out Library/PixelFX/review-v001
node --test Tools/PixelFX/tests/export.test.mjs
```

Recipe fields:

- `name`: lowercase output basename, optional (defaults to effect basename).
- `source`: `pixelfx` (default) or `custom`; never edit the pinned vendor to add project variants.
- `effectId`: `category/name`, matching the selected source's module path.
- `seed`: safe integer, default 42. Randomness in `options.seed` is rejected so provenance has one authority.
- `fps`: optional 1..120; otherwise use effect metadata.
- `pixelsPerUnit`: positive number, default 32.
- `options`: effect-specific options, e.g. `palette`, `flip`, `light`. Read that effect's `meta.options` and drawing code. Stock actor overrides are intentionally unsupported.

The fresh output directory contains:

- `<name>.png` and `<name>.json`: transparent sheet and unmodified Aseprite-style layout/timing metadata.
- `<name>.pixelfx.json`: Unity-friendly array manifest; schema 1 includes untrimmed dimensions, per-frame paths/durations, PPU, loop/fps, seed/module hash/upstream commit and pivot.
- `frames/frame_0000.png` etc.: the same transparent frames exported individually, enabling package-independent Unity import.
- `contact-dark.png` / `contact-light.png`: contact sheets without the demo actor; visual review artifacts, not runtime textures.
- `recipe.json`, `LICENSE.pixelfx.txt`, `NOTICE.pixelfx.md`: reproduction inputs and attribution.

Coordinates: upstream origin is continuous pixel coordinates from the top left. Unity normalized pivot is `(ox / width, 1 - oy / height)`; do not subtract an extra pixel or crop frames independently. The source JSON retains upstream rounded millisecond durations; the Unity manifest uses exact `1/fps` seconds. The last frame of a one-shot is included by upstream; the importer preserves its duration too, with an explicit clip stop time and no redundant terminal key.

The wrapper repacks sheet pixels as straight RGBA. Upstream's transparent canvas paste darkens translucent RGB; keeping its JSON layout while copying raw frame rows avoids this discrepancy. Every atlas frame must match its individual PNG pixel-for-pixel. Do not switch back to the raw upstream sheet PNG without checking this invariant.

Glow has already been composited into RGBA; a normal alpha-blended sprite does not reproduce an independent additive light buffer on every background. Review both backgrounds and choose a suitable palette/material when integrating. Effects with persistent marks need explicit lifetime/cleanup even when their animation ends.

Custom modules belong under `Tools/PixelFX/custom/fx/<category>/<name>.js`, using the same upstream meta/draw/create contract. For drawing helpers, import `../../../vendor/pixelfx/src/core/index.js` from a module in that layout. Read the pinned `docs/AUTHORING.md` and neighboring source modules when authoring, then export with `source: custom`. Preserve derivative notices. Optional ffmpeg preview commands can use upstream render helpers, but should write outside the pinned source tree.
