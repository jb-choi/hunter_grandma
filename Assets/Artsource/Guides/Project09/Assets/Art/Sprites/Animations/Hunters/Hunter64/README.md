# Hunter64 Unity output

The editable originals remain under `ArtSource/Try/Hunter64/v001/Clips/`.
These are full, untrimmed 64 × 64 RGBA frame exports, Point filtered, uncompressed,
with mipmaps disabled. No artwork was generated or rescaled for this integration.

The connected clips are idle_breath (5), walk_retry/walk tag (8), focus (4), shoot
(4), collect (4), carry_small (6), drag_large (6), load (4): 41 frames total.
Frame durations are preserved in `Assets/Data/Hunting/HuntHunterAnimations.asset`.
Walk's ninth cycle_close reference is excluded. Idle v2, unfinished rework, blank
walk in-betweens and the colored body-part focus experiment are not exported.

The visible standing height is 50 pixels; 7 transparent rows sit below the soles.
The runtime aligns the soles to the ground and preserves the configured 42-pixel
display height and animal size ratios. It flips the full sprite when turning.

Original visible layers are used for idle, walk, focus, shoot and collect.
For carry_small, drag_large and load only, export copies hide Bow and Quiver;
DemoProps remains hidden. The original Aseprite files were not modified. Those
older carry/load poses have no new slingshot/bag artwork. The current nonlethal
loop reuses collect/carry poses and the existing procedural shake/pat movement;
dedicated shake, waking and pat frames remain future artwork.

`HuntArtSet.hunterAnimations` connects the set; the old hunterSprite is retained
as a fallback. Reapply imports/references with Hunting Idle > Apply Hunter64 and
Mountain Panorama. Setup does not export Aseprite files itself.
