# Map selection UI smoke test

Unity 6000.3.10f1

Status: PASS

Validated 2026-10-25 in Unity Editor Play Mode. Runtime and editor scripts compiled. Final Console showed zero warnings and zero errors.

- PASS: Map starts closed and does not interrupt automatic combat
- PASS: Map button opens a modal and pauses combat
- PASS: User-supplied map sprite is connected
- PASS: Map pin 1 updates the selected region details
- PASS: Map pin 2 updates the selected region details
- PASS: Map pin 3 updates the selected region details
- PASS: Map pin 4 updates the selected region details
- PASS: Map pin 5 updates the selected region details
- PASS: Map pin 6 updates the selected region details
- PASS: Previewing pins does not change the active region
- PASS: Map pin receives pointer hits above the modal backdrop
- PASS: Combat timers and kills remain frozen while browsing the map
- PASS: Confirming a new region changes location, closes the map and resumes combat
- PASS: Region travel resets the encounter exactly once
- PASS: Map closing restores the previous game speed
- PASS: HUD displays the confirmed region
- PASS: Closing without travel preserves location and encounter progress
- PASS: Travel preserves an existing player pause
- PASS: Backdrop close also preserves an existing player pause
- PASS: Twenty open/close cycles preserve pause state and bounded pools
- PASS: Repeated map use reuses one overlay
- PASS: Invalid region selections are rejected
- PASS: M input opens the map and pauses combat
- PASS: Map blocks gameplay pause/restart shortcuts while browsing
- PASS: Escape closes the map and restores combat
- PASS: No runtime errors or exceptions during map validation

Manual pointer verification: clicked the footer 지도 button, continued in the current district without resetting progress, reopened the map, clicked the 중앙 광장 pin and confirmed travel. The modal closed, the HUD displayed 중앙 광장, and the new encounter started with zero kills. Original combat regression also passed all 16 checks after map integration (38 spawned / 32 defeated in 60 simulated seconds).

Visual inspection: `MapSelection-1280x720.png` and `MapSelection-1024x768.png` show the complete map, all six pins, details, close and confirm controls without clipping or overlap. The map PNG is byte-identical to the user attachment (SHA-256 `457AC543F794299F514C2F52D98A755161EAE03348E98D157D4E321F8FEBE2C3`).

Reference: read-only project09 `HuntHud.BuildDrawer` and `HuntWorldMapView`; project09 was not changed. The initial raycast check ran before the newly opened overlay had rendered. It was moved to the following rendered frame; the retained `MapSelectionSmokeTest-first-frame-raycast.md` records that resolved test-harness issue.

Scope: district selection, HUD location and restarting the existing encounter. Districts currently share the existing battle scenery and balance. Session persistence only; player packaging and other platforms were not tested.
