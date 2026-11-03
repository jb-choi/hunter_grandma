# Zombie asset reference validation

Unity 6000.3.10f1 · Status: PASS

- PASS: Original and party zombie variants are assigned with every required action
- PASS: Normal: 28 authored poses preserve full canvas, foot pivot, point filtering and 32 PPU
- PASS: Normal: authored attack follows windup, existing damage beat and recovery
- PASS: Normal: one-shot death holds the final authored pose
- PASS: Infected: 28 authored poses preserve full canvas, foot pivot, point filtering and 32 PPU
- PASS: Infected: authored attack follows windup, existing damage beat and recovery
- PASS: Infected: one-shot death holds the final authored pose
- PASS: Large: 28 authored poses preserve full canvas, foot pivot, point filtering and 32 PPU
- PASS: Large: authored attack follows windup, existing damage beat and recovery
- PASS: Large: one-shot death holds the final authored pose
- PASS: All 84 exported poses are linked without substituting legacy zombie art
- PASS: Stage 0: roster matches the requested early/late-stage enemy types
- PASS: Stage 1: roster matches the requested early/late-stage enemy types
- PASS: Stage 1: timed enemies arrive more frequently than the previous version
- PASS: Stage 2: roster matches the requested early/late-stage enemy types
- PASS: Stage 2: timed enemies arrive more frequently than the previous version
- PASS: Stage 3: roster matches the requested early/late-stage enemy types
- PASS: Stage 3: timed enemies arrive more frequently than the previous version
- PASS: Stage 4: roster matches the requested early/late-stage enemy types
- PASS: Stage 4: timed enemies arrive more frequently than the previous version
- PASS: Stage 5: roster matches the requested early/late-stage enemy types
- PASS: Stage 5: timed enemies arrive more frequently than the previous version
- PASS: Stage 6: roster matches the requested early/late-stage enemy types
- PASS: Stage 6: timed enemies arrive more frequently than the previous version
- PASS: Stage 7: roster matches the requested early/late-stage enemy types
- PASS: Existing seven-entry v1 save migrates without losing gold, upgrades, preference or clear history
- PASS: Stage 6 clear unlocks appended Stage 7; nonexistent Stage 8 is rejected
- PASS: Uncleared Stage 6 keeps new Stage 7 locked after old-save migration
- PASS: Malformed six-entry saves remain rejected rather than silently resetting progress
- PASS: Stage 7 has its own selectable city-map district
- PASS: All three scene zombie instances resolve the imported animation set and source sprite after GUID reconciliation
