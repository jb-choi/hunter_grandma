# HU-07 drag_large — source review

- Built-in image_gen edit, transparent background; exact 1254×1254 hunter reference.
- Selected source: `drag_large_poses_imagegen.png`, actual 1536×1024; six poses visually arranged as 3 columns × 2 rows, row-major order.
- One targeted retry reduced the crowding in `drag_large_poses_imagegen_discarded_layout.png`. Selected retry is improved, but it still violates exact 512-pixel cell boundaries.
- Suggested source separation: column cuts near x=545 and x=1024; row cut y=512. Verify alpha bounds during normalization. Do not blindly cut all columns at 512 and 1024: the first hat crosses x=512.
- Exporting at 1254×1254 adds output canvas resolution; it does not create native 1254 detail from this source.
- Observed: clearly distinct forward body lean, backward hand grip and short taut rope exiting left. No animal or cargo is baked in. Quiver remains visible; the bow is not clearly distinguishable and needs cleanup.
- Pose order requested: planted pull → loaded stance/knee lift → step through → opposite planted pull → loaded stance/knee lift → step through.
- Refinement required: some opposite-side gait phases remain visually similar; head tilt, hat and rope grip vary. Rope endpoint should be replaced or aligned to the separate large-prey attachment in Unity. A smooth production loop requires temporal cleanup.
- Empty corner, cell-gap and three empty-region alpha samples are 0. Both generation outputs are preserved without resampling.
- Prompt and retry constraints: `../../../Prompts/HU-07_drag_large.md`.
