# Cute zombie runtime exports

Source: `Assets/Artsource/Drafts/GrandmaShooter120/v002/Monsters_Cute/`.
Normal = zombie_01_normal_120.aseprite; Infected = zombie_03_rescue_human_120.aseprite;
Large = zombie_02_large_120.aseprite. Native masters are preserved.

Each directory contains the Aseprite composite's 28 full-canvas 120x120 PNGs:
01-04 Idle, 05-10 Walk, 11-16 Run, 17-22 Attack, 23-28 Dead.
ZombieAnimationSetup assigns authored durations, Point filtering, 32 PPU,
and normalized foot pivot (52/120, 2/120). No art resampling or recoloring.
Runtime uses horizontal flip for left-facing poses.

The human Dead ends prone with partial slime coating, without adding a rescue
or recruitment mechanic. See Docs/ZombieCute-StageRoster.md and the pixel
comparison at Docs/Validation/ZombieCute-export-validation.json.
