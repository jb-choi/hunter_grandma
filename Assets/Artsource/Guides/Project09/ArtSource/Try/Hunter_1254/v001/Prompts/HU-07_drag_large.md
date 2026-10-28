# HU-07 drag_large — generated keyposes

Mode: built-in image_gen, transparent background, exact hunter reference.

```text
Use case: identity-preserve
Asset type: Unity hunter animation experiment, high-resolution transparent pose sheet.
Input image: Image 1 is the exact hunter identity and style reference. Preserve this design and proportions aggressively.
Subject: The same friendly right-facing chibi hunter: wide floppy green hat with cream feather, golden brown hair, small cream face with warm cheek, tiny dark vertical eye, green tunic with folded collar, diagonal brown leather strap and quiver, belt buckle, brown cuffed boots.
Style: Match the reference's rich pixel-like illustration, chunky square clusters and warm clean colors. Preserve the detailed reference's charm; do not simplify into a minimal 32-pixel character. No blur or painted strokes.
Composition: EXACTLY SIX distinct complete hunter poses, laid out as EXACTLY THREE COLUMNS AND TWO ROWS, six equal square cells. Landscape 3:2 canvas. Row-major animation order: top left, top middle, top right, bottom left, bottom middle, bottom right. Equal character height, identical proportions and foot baseline in every cell. Each full pose fits its own cell with generous 10 percent margin on all sides. All hat, feather, boots and bow must remain fully visible, no overlap between cells. High resolution sheet, target 3762 x 2508, intended for six 1254 x 1254 sprites.
Backdrop: genuinely transparent alpha throughout empty space. No floor, shadow, scenery, grid lines, labels, numbers, text or watermark.
Invariant constraints: one character per cell, no extra subjects, no animals, same right-facing perspective and same outfit in all six poses. Distinct limbs and clear feet. Six sequential keyposes, not six duplicates.
Action: A complete six-phase DRAG LARGE PREY walk-in-place loop facing right, moving forward/right while pulling a heavy animal that will be attached separately later. NO ANIMAL in this sheet.
Body mechanics: hunter leans torso and head strongly forward/right from ankles, knees bent with obvious effort, chin tucked, feet push against ground. Both arms extend BACKWARD toward the left, both hands behind the waist grip a short taut rope. The rope exits toward the left of each cell from the hands, approximately 10% of cell width long, ending well inside the cell and never clipping. It must clearly read as pulling a heavy weight behind him, not carrying it ahead. Short taut rope and hand grip only; no cargo, animal, bundle or ground.
Bow stowed diagonally on the back and quiver fixed at the back; neither hand holds a bow.
Six distinct effort-walk phases: top left long staggered stance with front/right boot planted pulling, back/left boot pushing; top middle body moves over front foot with back knee lifted; top right back boot steps through while front foot bears weight; bottom left opposite planted/left boot forward pulling, right boot pushes behind; bottom middle opposite loadedfoot and knee lift; bottom right opposite step-through preparing next long stance.
Keep a pronounced forward lean and backward-reaching arms throughout so this silhouette is clearly different from normal walk and forward-hand carry. Same exact friendly hunter outfit, all six right-facing, full hat/feather/boots/rope visible.
```

## Targeted retry: strict square-cell margins

The initial sheet crossed cell boundaries; retry adds the following constraint.

```text
CRITICAL LAYOUT CORRECTION:
Produce a 1536 x 1024 landscape sheet, six equal 512 x 512 cells (3 columns x 2 rows). Treat each cell boundary as strict though invisible.
Each full hunter PLUS rope must be NO MORE THAN 400 pixels wide and 400 pixels tall within its 512 cell, with transparent margin of at least 50 pixels on every edge. Uniformly scale down all six poses to satisfy this. Use identical scale in every cell.
Centers of cells: (256,256),(768,256),(1280,256),(256,768),(768,768),(1280,768). Feet baseline top row y=455 and bottom row y=967, topmost feather at least y=55/y=567. Rope ends stay inside left edge +55. Hat never crosses right edge -55.
Keep generous separation between poses: no part of one pose may extend into an adjacent cell. No cropping, no labels, no visible borders. The hunter must be distinctly smaller than a tight fit, so every complete pose can be cleanly cut into a square sprite without clipping. Keep forward lean and backwards rope grip, high fidelity to the original character.
```
