# Unity Project Context

## Korea capture comic cards — 2026-10-17 (current)

Successful nonlethal catches now show a cached circular cut-in with species-specific hunter/animal slapstick for rabbit, magpie, squirrel, raccoon and boar, ending in a slingshot hit and stun stars. Only `Stunned` activates it; misses and other phases never do. The successful stun lasts `max(fallenSeconds, 2.6s)`, with the main animal gradually entering its existing fall pose and stars synchronized at 82%. Both views sample the simulation clock and freeze on pause. Existing sprites/clips are reused; card graphics are procedural uGUI with a circular mask and no input capture. Placement clears desktop notices and the expanded toolbar. Plan ETA includes the longer successful phase. Rewards remain exclusively in photography/shaking; save v7, RNG, original artwork, scenes and packages are preserved. EditMode 182/182 passed after baseline 171/171; isolated actual-Canvas QA passed at 640×360, 960×540 and 1280×720. No Player build or user-save Play session was run. See `Docs/AI/CaptureCards-2026-10-17.md`.

## Search gestures — 2026-10-17 (current)

Searching now uses the same snapshotted search deadline with a hopping patrol for the first 42%, then a stationary investigation. Successive searches alternate animal tracks, looking around nearby trees, and binocular scanning using Encounters modulo three; presentation never consumes encounter RNG. HuntSearch owns the derived action/progress and stops horizontal movement while inspecting. HuntSearchView adds a magnifier speech bubble, three paw marks, handheld props, raised hand, head/body turns and tree rustle, driven entirely by StateElapsedSeconds. HuntHunterAnimations reuses walk/collect/idle frames. Pause freezes poses; discovery clears search props and tree rotations; plan camp waiting uses idle. Three downloaded icons total 9,308 bytes, credited in Assets/Art/External/SearchMotion/ATTRIBUTION.md. No save version, scene, package or original art changes. EditMode 171/171 and 640×360 / 960×540 / 1280×720 Canvas QA passed. See `Docs/AI/SearchMotion-2026-10-17.md`; Notion documentation is directly under **개발작업**.

## First-meeting polaroids — 2026-10-17 (current)

The first successful catch per zoo species now skips Shaking and enters the appended Photographing state (1.7s; shutter at .2s). The shutter commits the existing field reward and zoo reservation once; a developing portrait flies to a pinned top-left album before the existing smile/four-turn/throw sequence. A “동물원으로” speech bubble accompanies spin/throw. Later catches retain shake/release. HuntPolaroidAlbum caches five cards, overlaps two with tilt, and supports previous/next and horizontal drag, including circular browsing. The ordered zoo list supplies photo persistence and the temporary reset, so save v7 and old records remain compatible; existing zoo entries immediately display portraits. Interrupted photography after the shutter resumes pending zoo delivery without duplicate rewards; before-shutter interruption remains uncollected. Plan ETA includes photo time. Photos compose existing animal/forest sprites; downloaded Twemoji camera/pushpin PNGs total 1,803 bytes with attribution. No scene/package/original art changes. EditMode 162/162 passed; Canvas/input QA passed at 640×360, 960×540 and 1280×720. See `Docs/AI/PolaroidFirstMeeting-2026-10-17.md` and its Notion page under **개발작업**.

## Hunting plans — 2026-10-16 (current)

The eighth menu opens a parchment plan overlay, with KR region/time and 15 unique species/grade material goals executed sequentially. Pending rows support edit/reorder/delete; paused current edits reset only that step. Completed/deferred rows can be deleted to reuse the 32-row limit. Only newly accepted drops count; finish the current animal before transitioning. Plan pause leaves free hunting running. Optional material time limits defer incomplete steps; completion defaults to free hunting, with camp waiting selectable. Equipment hut can register exact upgrade shortfalls without consuming/upgrading. Compact HUD shows current progress/mean ETA. Save v7 retains plans and preserves v1–6 progress. Existing simulation refactor remains intact. No new art/packages/scenes. See `Docs/AI/HuntPlan-2026-10-16.md` for behavior, limits and validation. Earlier v6 and feature exclusions below describe historical work.

## Simulation refactor — 2026-10-16

`HuntSimulation` remains one plain C# model with the same public API and save v6 DTO. Its existing partial-class structure now groups encounter state/movement in `HuntSimulation.cs`, recipe/online/offline production in `HuntSimulation.Production.cs`, upgrades/milestones for both modes in `HuntSimulation.Progression.cs`, and snapshot validation/application in `HuntSimulation.Persistence.cs`. `HuntNonlethal`, `HuntEquipment` and `HuntZoo` retain their feature behavior. Restore validates all subsystems before applying live state; online/offline production shares ingredient selection and completion accounting while retaining existing notification timing and offline batching.

Unity 6000.3.10f1 compilation passed with 0 errors/warnings. EditMode: 125/125 before, 132/132 after. Across both hunting modes and eight seeds per mode, 9,600 before/after snapshots (including events/history/positions/saves) matched byte-for-byte. Unity MCP has no connected instance; the existing Unity CLI/Pipeline connection is available for this project (port may change on domain reload). No scene, prefab, ScriptableObject, package, save schema or player build was changed by this refactor. Details: `Docs/AI/Refactoring-2026-10-16.md`.

## Current behavior — 2026-10-16

Desktop presentation now uses the user-selected bb.jpg as an aspect-preserved, bottom-aligned backdrop, with a 170-unit forest strip 80 units above the bottom. Hunter42 and animal relative sizes remain unchanged. Large top HUD blocks are hidden in this optional presentation; status/toast sit above the strip. Configuration height/inset changes update live. The zoo header has a temporary first-meeting reset that clears collection/delivery only, immediately saves, preserves lifetime resources/equipment/records, safely finishes a current catch by release, and rolls back on save failure. Save v6 is unchanged. See Docs/AI/DesktopReset-2026-10-16.md for asset provenance and QA.

The first successful catch of each Korea species not yet in the zoo now branches after its normal field reward: smiling hunter bubble (.8s), exactly four rotations (1.6s), then an offscreen throw (.7s). Later catches use the normal wake/pat/release loop. A seventh menu opens a blank zoo page with cached sprites/names and sequential arrival/bounce animations. Landing and reopening never grant rewards. Save v6 stores per-species reserved/launched/landed state; v1–5 preserves resources/equipment and starts with an empty zoo. Reloading a reserved catch fast-forwards its interrupted delivery to pending arrival without replaying rewards. Screen editor supports the zoo page and preview. No new art downloads/packages or original art changes. See Docs/AI/ZooFirstDiscovery-2026-10-16.md for exact scope, migration and QA. Earlier descriptions below are historical.

## Current behavior — 2026-10-15

Equipment hut now implements W2-A/W2-B browsing, explicit equip, separate Lv1–3 and species-specific material-only upgrades. Encounter equipment is snapshotted at Entering; mutations apply next encounter. Failed immediate saves roll back only the transaction. Save v5 retains v1–4 history/balances, adds two equipment states plus rare/legendary balances. Field shakes now grant common plus one exclusive bonus roll (rare10%/legendary1%/none89%), so bestiary tiers are live. A single generated room and two subtly different icons, UI level dots and tiny hunter belt markers are connected; original art remains unchanged. See Docs/AI/EquipmentHut-2026-10-15.md for migration, tuning and QA. Earlier common-only notes below are historical.

The Korea bestiary now shows 15 named drop previews from `HuntBestiaryDrops.asset`: common/rare/legendary use white/yellow/red circles with item images and 100%/additional10%/additional1%. Rare/legendary are explicitly marked planned because actual rewards remain common-only. New cotton/acorn downloads and reused Twemoji art are credited under Assets/Art/External/BestiaryDrops. See Docs/AI/BestiaryDrops-2026-10-15.md for scope and UI QA.

Hunter64 integration now connects 8 clips / 41 full 64x64 RGBA frames through `HuntArtSet.hunterAnimations`. `HuntStage` samples simulation state time, preserves the 42px visible hunter height and animal ratios, and suppresses the duplicate code weapon. A combined MountainSkyLoop/v004 panorama is connected through `farLandscape` behind the existing trees/ground. Original editable artwork is preserved. See `Docs/AI/Hunter64Integration-2026-10-15.md` for clip selection, timing, export caveat and QA.

Configuration now enables `nonLethalLoop`: slingshot→stun→approach→lift/shake→field gold/common material once→wake/pat→release. Seven new states are appended to preserve existing enum values. Legacy cart/production logic remains for old data/tests but is disabled in the active mode. The existing six species are retained and squirrel/raccoon are added; only rabbit/magpie/squirrel/raccoon/boar spawn in Korea, uniformly at 20% each. Save DTO v4 retains old progress and adds per-species shaken/released/material counts; old v1–v3 files may omit only the two new species. Interrupted encounters resume searching with committed drops preserved and no replay reward. The real world map drawer has KR enabled and five other country pins disabled. New downloaded PNGs total about482KB, with license/placeholder notes in Assets/Art/External/Nonlethal/ATTRIBUTION.md. `HuntingNonlethalSetup.Apply` imports/connects this setup without saving scenes. See Docs/AI/NonlethalKorea-2026-10-15.md for scope, migration and current QA; older sections below are history.

The Unity project root is now a Git repository with private origin `https://github.com/jb-choi/hunting-idle.git`, main branch (initial backup 24836e8). README.md and ArtSource/Assets hunter work had user changes at this feature's start; do not include or revert them indiscriminately. Runtime feature work does not alter original artwork or packages. Rare/legendary materials, byproduct upgrades, special animals/zoo and parchment plans remain outside this implementation.

<!-- unity-onboarding:generated:start -->

## Project and environment

- Analyzed: 2026-10-16 (Asia/Seoul), HEAD `24836e8` with pre-existing uncommitted work. Historical validation below is retained; the current simulation structure and validation are recorded above.
- Project root: `E:/unity_project/project09_hunting_idle/Hunting_idle`.
- Unity: `6000.3.10f1`, revision `e35f0c77bd8e` (confirmed by `ProjectSettings/ProjectVersion.txt`).
- Installed Editor: `E:/Unity hub/6000.3.10f1/Editor/Unity.exe`; Unity CLI reports the Web module installed.
- Render pipeline: URP 17.3.0 with the 2D renderer. Quality levels reference `Assets/Settings/UniversalRP.asset`, which references `Assets/Settings/Renderer2D.asset` (confirmed).
- Input: Input System 1.18.0; `activeInputHandler: 1` and `Assets/InputSystem_Actions.inputactions` (confirmed).
- Intended release platforms: unknown. Installed modules do not establish the product target.

## Assets and startup

- `Assets/Art/Sprites/Hunters`: hunter PNG and Aseprite source.
- `Assets/Art/Sprites/Animals`: rabbit, boar, deer, magpie, tiger, and fox PNG/Aseprite assets, plus a combined 32x32-per-frame sheet.
- `Assets/Scenes/HuntingPrototype.unity`: first enabled build scene; contains Main Camera, Global Light 2D, and a configured `HuntingPrototype` root. Runtime Canvas and input are created by this root.
- `Assets/Scenes/SampleScene.unity`: original template scene is preserved and remains enabled after the prototype in Build Settings.
- `Assets/Settings`: URP configuration and the 2D scene template.
- `Assets/Scripts/HuntingIdle`: `HuntingIdle.Runtime` assembly, explicit hunt simulation, sprite stage, and uGUI HUD. Runtime assembly references `UnityEngine.UI` and `Unity.InputSystem`.
- `Assets/Editor/HuntingIdle`: editor-only setup assembly. `Assets/Tests/Editor`: editor-only NUnit test assembly.
- `Assets/Data/Hunting/HuntConfiguration.asset`: hunter/6 animal sprite references, timings, speeds, catch probabilities, optional seed.
- Architecture: plain C# `HuntSimulation` owns normalized movement, nine states, outcomes, lifetime records versus consumable inventory, production/sales, and bow upgrades. `HuntingPrototype` connects it to Unity Update and versioned local persistence (`HuntProgress`, `HuntSaveStore`); `HuntStage` and `HuntHud` handle presentation. No singleton, networking, scene-loading framework, or prefab system was introduced.
- Code uses `HuntingIdle` namespaces, explicit dependencies, private serialized composition fields, and event-based HUD refreshes. Animals and hunt settings are Inspector-editable.

## Packages and validation

- Unity Test Framework 1.6.0 is installed. Fourteen EditMode tests passed via Unity TestRunner API synchronous execution; CLI async test-list retrieval did not finish during validation. Evidence: `Docs/AI/Validation/EditMode-results.xml`.
- Relevant authoring packages: 2D Animation 13.0.4, Aseprite 3.0.1, PSD Importer 12.0.1, uGUI 2.0.0, Timeline 1.8.10, Visual Scripting 1.9.9.
- Multiplayer Center is installed, but no gameplay networking implementation or networking framework was found.
- CLI installation was verified with `unity --version` and `unity editors --installed --format json`.
- Unity compilation passed with no new Console errors/warnings. Play Mode completed 40 cycles (30 loads, 10 escapes), plus pointer-driven fold/pause, 30 repeat toggles, and six management panels. Expanded 467x320 and collapsed 633x314 layouts were visually inspected. StandaloneWindows64 is the active target, but no Player build/device test was run.
- Prototype runs in the background while alive, then restores the previous application setting. Version 1 local JSON preserves committed progress, in-flight production, and UI/pause preferences; unfinished hunting resumes from searching. Offline rewards are not implemented.
- Improvement cycles: 21 → 30 → 36 passing EditMode tests; final 3-resolution / 6-panel layout checks, pointer and P/M/Esc inputs, actual log wheel scrolling, overlay regression, manual save, and whole-snapshot Play restart equality. Final cumulative live snapshot: 41 completed hunts, 29 loads/sales, 12 escapes, 448 gold, bow level 1. Details in `Docs/AI/ImprovementCycles.md` and `Docs/AI/Validation/Cycle3-qa.json`.

## Development tooling

- Official Unity CLI `1.0.0-beta.13` was installed through `winget install --id Unity.CLI --exact --source winget`.
- The `unity` command is exposed through the Windows app execution alias at `C:/Users/Junbeom_Choi/AppData/Local/Microsoft/WindowsApps/unity.exe`.
- CLI recognizes the existing `6000.3.10f1` Editor.
- Unity MCP tools are exposed to Codex, but `mcpforunity://instances` returned zero connected instances during inspection. Live Editor capabilities are unavailable until a connection is established.
- User installed Unity Pipeline `0.8.0-exp.1`. Official Unity CLI connection was verified at `127.0.0.1:7800` against this exact project; scene, Console, script compilation, tests, and Play Mode can be controlled through CLI.
- Unity MCP remains disconnected; use the working CLI/Pipeline connection rather than adding another bridge.

## Constraints and open decisions

- Current implemented scope: search, walking animals, bow focus/arrow, fall or escape, collection/carrying/cart loading, foldable side/bottom management UI based on supplied layout references.
- Frame animation, final environment/cart/bow/UI art, sound, additional recipes/unlocks and OS transparent/always-on-top windows remain future work. Jerky/smoked recipes with independently persisted active/selected recipe, bow/facility/scout upgrades, weighted species encounters, five milestones, stock-only offline production, v1/v2→v3 persistence and collapsed management shortcuts are implemented. Cycle9–13 and 74 tests are documented in `Docs/AI/Cycles9-13.md`; Cycle4–8 history is in `Docs/AI/AdditionalFiveCycles.md`.
- Korean UI uses the installed Windows Malgun Gothic font unless a Font asset is assigned. Other-platform distribution needs a suitable licensed font asset.
- Preserve the existing artwork and serialized Unity assets.
- Use the actual project root above when running project commands; the parent workspace is not itself a Unity project.
- Package changes require user authorization. This feature added no packages. Build Settings now start with HuntingPrototype; other player/graphics/input settings and original artwork were preserved.

## Evidence inspected

`ProjectSettings/ProjectVersion.txt`, `ProjectSettings/QualitySettings.asset`, `ProjectSettings/GraphicsSettings.asset`, `ProjectSettings/ProjectSettings.asset`, `ProjectSettings/EditorBuildSettings.asset`, `Packages/manifest.json`, `Packages/packages-lock.json`, `Assets/Scenes/SampleScene.unity`, `Assets/Settings/UniversalRP.asset` and its meta, `Assets/Settings/Renderer2D.asset` and its meta, hunter/rabbit PNG import settings, `Assets/Art/Sprites/Animals/animals_32x32.json`, and `Assets/Art/Sprites/Animals/fox_source.md`.

<!-- unity-onboarding:generated:end -->

## Cycle14 presentation update

Hunter display height is 42 reference units (42/68 of its previous size). AnimalDefinition.heightRelativeToHunter controls presentation only: rabbit .85, boar1.8, deer2, magpie .9, tiger2.6, fox .9. Original sprite aspect ratios, background, overlay stage bounds, game probabilities and save v3 are preserved. Reticles, arrow trajectories and fall/carry placement follow actor dimensions. See Docs/AI/Cycle14.md for actual visible-height ratios, 74 passing tests, 12 hunt scenarios, 42 UI combinations and Windows-Cycle14 build/runtime evidence.

## Cycle15–19 presentation update

Optional `HuntArtSet` referenced by HuntConfiguration provides forest/prop/menu Sprite slots with per-slot built-in fallback. The setup tool creates/reuses `Assets/Data/Hunting/HuntArtSet.asset`; its initial slots are intentionally empty until artwork is made. `HuntBestiaryCards`, `HuntCampView` and `HuntMilestoneCards` are cached presentation objects owned by HUD/stage, with no independent update loop or save format. HUD refreshes them from the existing model and disposes its event/input subscriptions on destruction. Six species cards, actual recipe/production camp status, named/highlighted menus, first-discovery/scout notices, and five individually claimable milestone cards are implemented. Recent history remains scrollable. Base sprites, Cycle14 scaling, overlays and save v3 remain intact; frame animation and finished art remain future work. See `Docs/AI/Cycles15-19.md` and current validation artifacts; earlier onboarding evidence above is historical.

User screen editor: `HuntingScreenEditor` (Editor-only, Hunting Idle/화면 편집 창) provides an actual UI-rendered preview and editing of 11 UI group offsets/scales, Undo/redo/reset, asset save, and links to art/game settings. `HuntScreenLayout.asset` is optionally referenced by configuration; missing/empty overrides preserve defaults. HUD caches targets and restores base transforms before applying offsets to avoid drift. Edit previews never use the user's save or gameplay shortcuts and are disposed before Play; rendering cleanup uses DestroyImmediate in Edit Mode and Destroy in Play. Scene assets are not modified. Details: `Docs/AI/ScreenEditor.md`.
