# HU-02 walk — source review

- Built-in image_gen edit, transparent background; exact 1254×1254 hunter reference.
- Selected source: `walk_poses_imagegen.png`, actual 1536×1024; six poses, 3 columns × 2 rows, row-major order.
- Source cell size: 512×512. Exporting at 1254×1254 adds output canvas resolution; it does not create native 1254 detail from this source.
- Pose order requested: right contact → load/push → passing → left contact → load/push → passing.
- Observed: six distinct leg/arm poses, matching hat, hair, cheek, tunic, strap, quiver and boots; bow held forward. Hat and feet remain on sheet.
- Refinement required: contact poses in frames 1 and 4 are similar rather than clearly opposite phases; head/hat and bow vary slightly. A smooth production loop requires temporal cleanup, not just slicing.
- Empty corner and cell-gap alpha samples are 0. Source generation is preserved without resampling.
- Prompt: `../../../Prompts/HU-02_walk.md`.
