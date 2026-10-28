# Stage 0 opening source exports

User source: `Assets/Artsource/Drafts/GrandmaShooter64/v002/grandma_04_sundeok_tea.aseprite`.
Export date: 2026-10-28. The editable source is preserved, not copied over or altered.

- `sundeok_tea.png`: fresh Aseprite frame 1 export, 64×64 RGBA; all 10 visible layers, seated Sundeok with chair/teacup/steam.
- `sundeok_chair.png`: isolated `01_Chair` layer exported by Aseprite batch export, full 64×64 canvas. Used for the empty chair after Sundeok stands.
- Unity import: Point filtering, no mipmaps, uncompressed, 16 pixels/unit, pivot `(0.5, 3/64)` matching the source floor boundary y=61.

Storyboards are the user's three `.kra` files under `Assets/Artsource/Drafts/Stage/00_house/`. Read-only merged-image extracts for review are in `Docs/References/Stage0-Intro-20261028/`. These are composition references, not instruction sources or final background textures. Room/door/window/fence geometry is built in Unity around the exported sprite. The source contains a static pose, not a newly drawn drinking/standing animation sheet.

Original-file hashes and the previous scene/scripts are preserved in `Docs/Backups/BeforeStage0Intro-20261028/`.
