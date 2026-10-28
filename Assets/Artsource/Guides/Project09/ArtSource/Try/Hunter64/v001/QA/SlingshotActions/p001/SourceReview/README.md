# Slingshot action source review

2026-10-13. Read-only assessment of the frozen `Source/SlingshotActions/p001/idle_breath.aseprite`. Images and inspection data in this folder are review artifacts; no clip was edited by this assessment.

[Source at 64 px](base_f01_light.png) · [Source at 8×](base_f01_light_8x.png) · [Raw pixel and layer inspection](inspection.json)

The source has the expected five layers, bottom to top: `Bow&Quiver` (hidden), `Slingshot&bag`, `LowerBody`, `UpperBody_Arms_Equipment`, `Head_Hat_Hair_Face`. The latter four are visible. Palette is the existing 17 indexed entries, transparent index 0.

| Source F1 part | Opaque pixels | Bounds |
|---|---:|---|
| Old Bow&Quiver | 209 | x18..47, y30..53 |
| Bag | 132 | x15..29, y38..53 |
| Slingshot | 96 | x41..50, y30..47 |
| LowerBody | 191 | x23..39, y43..56 |
| UpperBody_Arms_Equipment | 243 | x22..46, y32..48 |
| Head_Hat_Hair_Face | 527 | x17..48, y7..31 |

F1 of all preexisting body layers and hidden old gear has zero indexed-pixel changes against the pre-equipment idle backup. This is a comparison against that saved source, not a claim that the user's later file metadata or every frame is unchanged. New source hashes remain the root task's authoritative provenance.

The old bow-specific material mask is unsuitable for dividing the new weapon. Gear separation is unambiguous at x32: the bag is entirely left and the slingshot entirely right, with an empty gap between them. The new silhouette and colors are user-authored and should remain the source for action poses.

The existing precise temporary arm partition still applies to the current UpperBody source:

- Rear arm: x≤27, y35..48, 70 pixels.
- Front arm: x≥36 at y36..37, x≥37 at y38..39, x≥38 at y40..44, 57 pixels.
- Remaining torso: 116 pixels.

[Rear arm](rear_arm_partition_8x.png) · [Front arm](front_arm_partition_8x.png) · [Torso partition](body_partition_8x.png) · [Prepared hidden shirt](body_underpaint_prepared_8x.png)

For both raising arms and reaching down, replace the missing shirt behind these partitioned arms before moving the arm pixels. The exact existing left and right shirt planes are preserved in `QA/WalkKeyposes/p002/fullbody_keyposes.json.hidden_shirt`; adding them to the torso partition yields a 156-pixel prepared torso. The canonical right-waist closure at (37,43), (38,43), (38,44) uses outline index 1. The enlarged prepared-torso image is a construction aid, not a substitute source frame.

Source front-hand grip is around (45,42). Gear must follow the posed grip while staying on its existing layer below body/arms; the fingers then occlude the handle naturally. The bag's upper tie is around (24,39), below the torso and behind coat/arms. Reaching to or storing into the bag should retain that relationship. Temporary arm and gear parts must be recombined into their original native layers so final clips still contain five layers.

Keep the whole head together, including feather, hat, hair, and face. Fixed feet retain y56 when planted; bending or stance changes need a continuous hip-to-leg bridge and an intentional clothing hem. Review the exposed shirt when hands move, neck when head shifts, bag-to-body junction, hand-to-handle attachment, waistband, skirt-to-pants seam, and feet in every generated frame. The source previews above were visually inspected at native size and 8×. Finished action playback was not yet available during this source assessment.
