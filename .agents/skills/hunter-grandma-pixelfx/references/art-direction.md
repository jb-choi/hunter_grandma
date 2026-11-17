# Hunter Grandma VFX decisions

Use the current `Docs/AI/UnityProjectContext.md` and nearby actor code for facts that may change. The game is Unity 6000.3.10f1 / URP 2D, with foot-based depth sorting and explicit presentation clocks. Sundeok's 120px source is shown at a separate display scale; 120px is not a required VFX canvas size. Default export PPU is 32, consistent with current actor art; judge world-space scale alongside the actor.

| Request | Start searching | Local integration point to inspect |
| --- | --- | --- |
| Shotgun flash/smoke | `muzzle`, `gun`, `smoke` | GrandmaShooter.Fire, Sundeok animation muzzle/flash frames; avoid double-flashing |
| Ordinary hit / critical | `impacts/blunt-hit`, `impacts/crit-hit`, `bullet impact` | ZombieActor.ReceiveDamage and the actual impact point; wall debris is not automatically suitable for flesh/slime |
| Shadow skill | `blink`, `shadow`, `poof` | GrandmaShadowSkill: independent cinematic clock, saved facing/foot, cancellation |
| Reward sparkle | `coin`, `collect`, `pickups` | DefenseHud resource flights and reward accounting; the effect never grants currency |
| Death / slime | `dissolve`, `poison`, `splash` | Existing authored ZombieAction.Dead; enhance rather than replacing the user's animation implicitly |

Prioritize readable, short impulses and an unobscured target in the small desktop view. Match the requested warm/comic or shadow palette to existing art; character 26-color limits do not automatically apply to additive/translucent VFX. Review dark and light contact sheets, actual intended scale, orientation and origin, and fast repeated shots. Inspect motion for windup/peak/settling and loop boundaries when applicable.

Most directional upstream effects face right. `flip` mirrors their local artwork; the fixed origin should remain at the real impact/muzzle. The grey demo actor, demo camera shake and `act` hit-stop are references, not gameplay instructions. `act.onBody` effects paint a stock character even in a transparent sheet: the exporter rejects these until adapted.

Upstream method: [Hugo Duprez — How to make (good) pixel art effects with Claude](https://www.spritefusion.com/blog/how-to-make-good-pixel-art-effects-with-claude). Use rendered references and concrete iteration; a valid export is not a visual-quality verdict.
