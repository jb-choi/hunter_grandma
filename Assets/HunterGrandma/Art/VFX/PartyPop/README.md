# Party death pop

Current production export: `v002/party-pop.pixelfx.json`. Custom effect `party/slime-pop`, seed 21, 30fps, 22 frames, 112×112, PPU32, origin `(56,64)` from the top left. Last frame is transparent. Duration .7s plus its exported endpoint frame.

2026-11-04 size follow-up: runtime display scale is **2.4 × actor size**, up from .8 (three times the width/height, nine times the area). The effect origin, frame timing and source export stay the same. The larger burst passed the 230-check D3D11 Play Mode fixture; see `Docs/Validation/PartySlime-large-validation.md` and `PartySlime-large-*.png`.

Reproduce from the Unity project root:

```
node .agents/skills/hunter-grandma-pixelfx/scripts/pixelfx.mjs export --recipe Tools/PixelFX/recipes/party-pop.json --out <new-empty-directory>
```

Uses the pinned pixelfx 0.3.0 core, commit `a84159688af3ca37a4be05a0d422b94451b2e24e`. Core license and notices are included in the export and root `THIRD_PARTY_NOTICES.md`; Patch Pixel font is not used. The original custom source is `Tools/PixelFX/custom/fx/party/slime-pop.js`. The first review export remains under `Library/PartySlime/pop-review-v001`, outside production assets.

`PartySlimeSetup` imports the manifest's individual frames into the existing `Zombie120AnimationSet.asset`. Runtime uses `ZombiePopVisual`, one child renderer per pooled zombie in its existing SortingGroup, above its body. Z1/Z2 lethal damage queues the pop after .09 combat seconds; the authored four-frame death remains visible. Playback advances only by `CombatDeltaTime`, pauses with combat, and resets on completion/despawn/disable/reuse/retry. It never grants rewards, applies damage, or requests hit-stop.

Reviewed dark/light contact sheets, exact final-frame cleanup, repeat-export binary identity, actual D3D11 scene renders, and pause/retry/reuse in Play Mode. See `Docs/Validation/PartySlime-validation.md` and `PartySlime-integration.md`.
