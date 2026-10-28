# Hunter Grandma · Stage v2

Source: [Stage·좀비 사냥·거대 보스 포획 기획 v2.2](https://app.notion.com/p/Desktop-Companion-Stage-v2-4047313398e0818db924d95c5c455419), last edited 2026-10-25. [Boss art reference](https://app.notion.com/p/4047313398e081708791ed0d9751c261). Local source snapshots are in `Docs/Specs/`.

## Play

Open `Assets/HunterGrandma/Scenes/GrandmaDefense.unity` and press Play. The scene is first in Build Settings. Stage 0 starts automatically. Grandma shoots automatically; M opens the city map, Escape closes it, Space pauses, R retries. Footer buttons provide the same controls plus retreat. Gold upgrades affect actual shotgun damage and firing interval immediately.

The map has a separate Stage 0 home entry and six city districts. Previewing a locked district is allowed, but travel is disabled. Clear the previous stage to unlock the next. A cleared district can be hunted again to capture a new individual. The map preserves the prior pause state; travel/retry cannot interrupt the automatic capture sequence.

## Implemented loop

Finite normal waves → giant host boss → subdual at zero HP → shooting stops → cart arrives and loads the intact host → travel and fade → home-yard arrival → one new captive and CLEAR. Only completed yard arrival grants a captive and next-stage unlock. Normal aliens melt into the ground, yield gold automatically, and never become captives.

| Stage | District / boss | Encounter difference |
|---|---|---|
| 0 | Home / neighborhood dog | 2 waves; two barks and dash. First dog is Kongi; repeats are different neighborhood animals. |
| 1 | Northwest residences / delivery worker | 3 slow crowd waves; box charge and recovery. |
| 2 | Northeast shops / salesperson | 3 fast waves; cart rush. |
| 3 | West warehouse / hardhat worker | 4 mixed-size waves; guard reduces damage, slam recovery increases damage. |
| 4 | Central plaza / dancer | 4 waves with left/right arrivals; rhythm cue and two rotations. |
| 5 | Southwest residences / trainer | 4 waves ending with faster aliens; crouch and leap. |
| 6 | Southeast hospital / doctor | 4 waves mixing size, speed, and direction; up to two heals with small summons. |

Original boss images from the linked Notion page are normalized to 64×64. Their large in-game scale, infection aura, profession accessories, and procedural signature motion distinguish them from normal aliens. Per-district landmarks identify the reused village setting; the return transition switches to a fenced home yard. Original sprite sheets are preserved. See `Assets/HunterGrandma/Art/Bosses/ATTRIBUTION.md`.

## Persistent rewards

`DefenseProgress` owns permanent gold, attack/fire-rate levels, per-stage clear count, captive count, first-clear timestamp, and captive records. Every captive receives a fresh GUID and source-run GUID, boss archetype, origin stage, appearance, capture time, and returned-to-yard flag. One run can settle only once. Repeat clears preserve the first-clear timestamp.

Gold is credited on normal kills and saved periodically, on purchases, retreat, failure, clear, application pause, and exit. Failure or retreat gives no captive; earned gold and past history remain. Retry resets only the current encounter. Active runs are not resumed after relaunch; the profile retains settled rewards and saved gold.

Save path: `Application.persistentDataPath/HunterGrandma/stage-progress-v1.json`. Writes validate a temporary JSON before atomic replacement, retain `.bak`, and recover valid backup data if the primary is damaged. Unrecoverable primary data is preserved with an `.invalid-<UTC>` suffix. A failed save is shown in the HUD and retried. This is the first save schema; no previous gameplay-save migration exists.

## Decisions for the prototype

- Existing automatic shooting is retained. Capture/loading/return is an automatic cutscene.
- One captive per successful run; no capacity limit in this prototype.
- Repeats alternate accessory colors; the dog name distinguishes the initial neighbor from subsequent animals. Breed/body-shape expansion is future art work.
- Wave timing and upgrade costs are tuning values. The runtime validation report records real simulated clear durations.
- Treatment, companions, shops, research, hut construction, offline progress, transparent desktop-window behavior, and richer skeletal/sprite animation are outside this stage implementation.

## Validation

`Tools → Hunter Grandma → Run Stage v2 Validation` runs the actual scene on an isolated `Library/StageV2-validation.json` profile. The old combat/map menu entries now route to this suite because endless-combat/all-districts-unlocked assumptions were superseded. Historical reports remain as baseline evidence.

The suite checks model rewards/IDs/locks/upgrades/save recovery, map input and pause behavior, all seven natural wave-and-boss encounters, intact-body loading and delayed settlement, a repeated dog capture, replay travel, retreat/failure preservation, and disk reload. Reports and 1280×720 / 1024×768 screenshots are under `Docs/Validation/StageV2-*`. Editor setup can be rerun through `Configure Stage v2`; it does not overwrite original art or existing normalized frames. No packages or project09 source changes are required.

### Verified result · 2026-10-26

Unity 6000.3.10f1 completed **115 checks** with no runtime errors/exceptions/assertions. All seven stages cleared through natural auto-combat; a repeated Stage 0 run produced an eighth unique captive. First-run stage durations were 61–106 simulated seconds under the tested upgrade progression. All normal waves had zero breaches in that run. Tests also confirmed delayed reward settlement, repeat IDs, lock enforcement, actual map-pin pointer raycast, M/Escape and pause behavior, retreat/failure preservation, atomic save/backup recovery, and disk reload.

Final map screenshots were visually checked at 1280×720 and 1024×768, along with boss/capture/yard scenes. Boss HP was moved below the playfield to keep faces visible. The regular player save file remained absent before and after validation, confirming isolation. The intentional corrupt-save recovery test emits one expected warning. Standalone player packaging and long-duration performance remain untested.
