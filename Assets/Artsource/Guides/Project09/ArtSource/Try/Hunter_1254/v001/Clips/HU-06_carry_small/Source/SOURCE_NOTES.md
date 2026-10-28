# HU-06 carry_small — source review

- Built-in image_gen edit, transparent background; exact 1254×1254 hunter reference.
- Selected source: `carry_small_poses_imagegen.png`, actual 1536×1024; six poses, 3 columns × 2 rows, row-major order.
- Source cell size: 512×512. Exporting at 1254×1254 adds output canvas resolution; it does not create native 1254 detail from this source.
- Pose order requested: right contact → load/push → passing → left contact → load/push → passing.
- Observed: both forearms extend forward with cupped hands; bow stowed diagonally behind the body; no animal or bundle baked into the hunter. These poses provide an empty prey attachment space.
- Refinement required: contact poses in frames 1 and 4 are similar rather than clearly opposite phases; carrying hands and hat vary slightly. Stable prey attachment and a smooth production loop require temporal cleanup.
- Empty corner and cell-gap alpha samples are 0. Source generation is preserved without resampling.
- Prompt: `../../../Prompts/HU-06_carry_small.md`.
