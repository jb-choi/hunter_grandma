# UI refactor — 2026-10-28

Scope: the game UI following the bundled Korean font fix. Preserve visual layout, labels, input, economy, progression, scene bindings and saves.

## Changes

- Added `DefenseUiBuilder`, a plain C# object shared by the HUD and map. It holds a borrowed font reference and centralizes rectangles, images, buttons, label defaults and HUD button composition. It does not own or destroy the font asset.
- Replaced duplicate uGUI construction methods in `DefenseHud` and `DefenseMapView` with builder calls.
- Split HUD startup into canvas, battle HUD, pooled resource flights and opening HUD methods, preserving construction order.
- Retained serialized field names/types, existing public HUD methods and the Noto font scene reference. No new package, scene schema or save migration.

## Validation

- Unity 6000.3.10f1 imported the new script and generated its metadata; runtime and editor assemblies recompiled.
- `dotnet build Assembly-CSharp-Editor.csproj --nologo -v quiet`: PASS, zero errors/warnings.
- Existing UI P0 Play Mode suite: **71 checks PASS**, 2026-10-28 01:55 KST. Includes keyboard controls, repeated panel opening/closing, purchases, reward accounting, resource animation, modal raycast ordering, pause restoration, maximum upgrades and isolated-profile reload. No runtime errors/exceptions/assertions.
- Existing Stage 0 opening/attack Play Mode suite: **48 checks PASS**, 2026-10-28 01:55 KST. Covers the extracted opening presentation path, pause/input/purchase guards, skip/retry and existing live zombie behavior. No runtime errors/exceptions/assertions.
- Used the Computer Use skill to inspect the real Game View modal before acknowledging the existing UI suite's visual-review checkpoint. Inspected generated map images at 1280×720, 1024×768 and 800×450, plus small upgrade and opening captures. Korean text renders and controls remain inside their panels.
- Static comparison confirms map layout/names/labels/colors/callbacks/order and all map interaction methods are preserved. HUD input, presentation, purchases, feedback and reference-image code differ only in calls to the shared builder.
- Entry scene and ProjectSettings.asset SHA-256 hashes match the pre-refactor baseline. The regular player save hash also matches; both suites used their existing isolated Library profiles.

## Limits and rollback

No WebGL/standalone player rebuild was run for this refactor. OS DPI and long-run profiling were not tested.

Pre-refactor runtime scripts, reports and screenshots are in `Docs/Backups/BeforeUIRefactor-20261028/`. To undo this refactor, restore the two runtime scripts and remove the new builder script together with its Unity-generated metadata.
