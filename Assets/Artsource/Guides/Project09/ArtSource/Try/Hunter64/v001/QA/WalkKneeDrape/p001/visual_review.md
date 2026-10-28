# Knee bend and skirt drape candidates

[Native two-frame candidate](knee_drape_keyposes.aseprite) · [before/after 8×](before_after_8x.png) · [before/after 1×](before_after_1x.png) · [after 8×](after_contact_8x.png) · [after 1×](after_contact_1x.png) · [explicit geometry](knee_drape_keyposes.json) · [actual LowerBody change masks](lower_change_masks.aseprite).

The candidate frames are based on the saved walk F3/F7. Only LowerBody is edited. Head, UpperBody and Bow&Quiver cells retain the source image pixels, position, size and opacity. The source layer order, visibility, blend and opacity settings remain exact; both frames retain 120ms duration.

The rear-leg comparison bends from hip (28,48) forward to knee (33,47), then returns to cuff (30,50). The front-leg comparison bends from hip (35,48) forward to knee (38,47), then returns to cuff (33,50). Both knees flex toward screen right. The raised boot soles are at y55, with the planted boot soles still at y56. The front raised boot shifts 1px right relative to the prior F7 pose to retain a visible separation from the support foot.

The hem responds locally over the raised thigh: the rear-side crest spans x30..33 and the front-side crest x35..38, rising from y47 to y46. Short folds use the existing green shadow, and the black underside follows the crest. Waist geometry and the opposite panel remain intact. The central coat notch at x32/33,y45/46 is preserved exactly; native comparison found zero differences there.

Visual inspection used full-character 64px and 8× comparisons, reopened native hem/leg crops, and a bright background to inspect transparency. Both new poses show a forward knee turn and backward shin return instead of a straight translated leg. The front-knee pose is more pronounced; the rear-knee bend is subtler but visible at actual size. Boots partially overlap as passing legs, while the one-pixel sole-height difference and source leather colors distinguish the lifted foot from the planted foot.

The small F10 transparency pixel at (33,49) lies between the returning shin and the support leg, below the coat. The F11 triangular transparency at x30..33,y48..49 is also between the two legs. The small gaps at (39,48)/(38,49) lie between the knee/shin contour and the bow. They are retained as negative space rather than filled as clothing. No new gap inside the green coat surface was found. The independent visual reviewer reached the same classifications and accepted these as separate comparison poses.

Native re-read verified both planted y56 rows against the source, all three non-Lower cells byte-equivalent in raw pixels and geometry, identical layer settings, and unchanged central-notch pixels. The candidate is frozen after inspection. Final walk assembly and exports are handled by the parent task; this subtask did not edit the target walk or idle files.
