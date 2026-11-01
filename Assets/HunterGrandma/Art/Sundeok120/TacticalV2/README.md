# Sundeok tactical v2 runtime selection

Read-only export from the 43-frame native master at
Assets/Artsource/Drafts/GrandmaShooter120/v002/grandma_01_sundeok_120.aseprite.

Walk uses master 28-33 (6 x 140ms); Fire uses 40-43 (100/70/100/150ms).
Aim holds master 40, the authored tactical fire preparation pose, for 420ms.
There is no separate authored tactical Aim tag. Idle/Reload/Collect keep their
current runtime clips. The native master's tactical Reload remains available.

11 selected poses, each exported as full-canvas Composite/Body/Upper PNGs.
Upper combines the original non-Body layers without resampling or recoloring.
120x120, 32 PPU, foot pivot (60,2) in Unity, Point filtering.
Selection, timing, muzzle and authored attachment anchors live in ../pose_metadata.json.

Details and evidence: Docs/Sundeok-TacticalV2-Integration.md.
