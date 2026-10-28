# LowerBody keypose candidate review

Source: [backed-up current idle](../../../Clips/HU-02_walk/Source/WalkKeyposes/p001/idle_breath_source.aseprite), frame 1.

Candidate: [lower_keyposes.aseprite](lower_keyposes.aseprite), one LowerBody layer, three 64×64 indexed frames mapping to walk F3/F5/F7. [Full-character inspection file](full_character_keyposes.aseprite) uses the other three source layers unchanged. [8× contact](keypose_contact_8x.png) · [1× contact](keypose_contact_1x.png) · [explicit offsets and sole positions](lower_keyposes.json) · [native validation](lower_keyposes_validation.json).

| Walk pose | Rear/left boot shift | Front/right boot shift | Inspection |
|---|---|---|---|
| F3 | +3x, −2y | −2x, 0y | Raised boot retains a visible left side and sole at y54; planted foot reaches y56. Three sole columns overlap. |
| F5 | +7x, 0y | −7x, 0y | Feet exchange their left/right order, both reaching y56. Source cuff and boot outlines separate the overlapping edge. |
| F7 | +2x, 0y | −3x, −2y | Front boot lifts to y54 and stays visible to the right; the rear foot plants at y56. Three sole columns overlap. |

Skirt and all LowerBody source rows through y48 are exact. Source boot shapes and palette indices are translated directly, without recoloring. Pants use the existing outline and green ramp to connect fixed hip anchors (28,49)/(35,49) to the moving cuffs. Rear pixels are drawn before front pixels, then the unmodified skirt overlays the top of the joints.

Visual inspection covered all three full characters at 1× and 8× and reopened native lower-body crops on a bright transparency background. Both passing poses show the raised boot and planted boot separately. The opposite contact has crossed leg connections and reversed foot positions. No new transparent gap was found in the skirt or hip connections. The small negative space between boots at the contact pose is leg separation; it is outside the skirt. Head, face, feather, bow, quiver, arms and hands retain the source drawing.

Native inspection passed: source rows 0–48 difference 0, other-layer difference 0, saved PNG difference 0, partial alpha 0 and bottom planted pixel y56 for each pose. Palette and canvas match the backed-up source. This is a keypose candidate, not a completed smooth gait; the final nine-frame file and user-requested blank frames are assembled by the parent task. No target walk file or final export metadata was edited by this subtask.
