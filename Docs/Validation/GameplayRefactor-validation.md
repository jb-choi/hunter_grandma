# Gameplay refactor validation

2026-10-31 · Unity 6000.3.10f1 · PASS with limitations

## Scope

`GrandmaDefense` delegates movement to its directly owned `GrandmaMovement`. Scenery, phase progression and shooting have separate methods; serialized fields, phase order, input rules and gameplay values are preserved. See `Docs/Gameplay-Refactoring.md`.

The starting working tree already included the new zombie roster and Stage 7. The comparison baseline includes those uncommitted changes; it is not Git HEAD. Backup and hashes: `Docs/Backups/GameplayRefactor-20261031`.

## Results

| Verification | Result |
| --- | --- |
| Before refactoring: existing stage suite | 222 PASS, 01:09 KST |
| After refactoring: same stage suite | 222 PASS, 01:11 KST; identical check descriptions |
| Existing reload audio suite | 28 PASS, 01:11 KST |
| Runtime + Editor compilation against the original project's full package configuration | Both exit 0; existing CS0108 warning only |
| Serialized fields in GrandmaDefense | All 12 declarations unchanged |
| Protected existing files | 597 checked; only the intended GrandmaDefense.cs changed; 596 unchanged, none missing |
| Tested runtime source/metadata and scene | Byte-identical to workspace after validation |

The 222-check suite exercises actual Play Mode movement, Tab/manual control, automatic hunting, cover/depth/range rules, real shots, map/input/pause restoration, ordinary enemy animation, natural Stage 0–7 advancement, final CLEAR, replay, failure/retreat, upgrades and save migration/reload. All eight stages clear without bosses/captives, and Stage 0 replay produces clear counts `[2,1,1,1,1,1,1,1]`. No gameplay errors, exceptions or assertions were recorded.

The 28-check suite exercises the actual animation/reload-source integration and its independent presentation clock. It disables encounter Update for controlled audio cases; the stage suite above supplies the encounter integration coverage.

## Execution evidence

The original Editor remains open. CLI runs use the existing isolated `Library/BossFreeStageBatch` project, its minimal manifest pointing at the original project's package cache, and dedicated Library profiles. No ordinary player save is reset. The original full package graph is additionally checked with Unity's bundled Roslyn and current Editor response files; isolated compiler outputs never replace ScriptAssemblies.

Commands use `E:/Unity hub/6000.3.10f1/Editor/Unity.exe` with `-batchmode -nographics -projectPath <isolated-project>`:

- Baseline and final stage runs: `-executeMethod HunterGrandma.Editor.GrandmaStageValidation.RunBatch`.
- Reload run: `-executeMethod HunterGrandma.Editor.GrandmaReloadAudioValidation.RunBatch`.

Evidence:

- Baseline: `Docs/Backups/GameplayRefactor-20261031/baseline-stage-validation.md`.
- Final stage checks: `Docs/Validation/GameplayRefactor-stage-validation.md`.
- Reload checks: `Docs/Validation/GameplayRefactor-reload-validation.md`.
- Preservation: `Docs/Validation/GameplayRefactor-preservation.json`.
- CLI logs: `Library/BossFreeStageBatch/refactor-baseline.log`, `refactor-stage.log`, `refactor-reload.log`.
- Original-project compilation: `Library/GameplayRefactorCompilation/runtime.rsp`, `editor.rsp`, `runtime.log`, `editor.log`.

## Existing issues and limits

`GrandmaShooter.animation` still produces its existing CS0108 member-hiding warning. Unity Editor Search startup raises the same indexing ArgumentOutOfRangeException seen in the baseline; validators record it separately from gameplay. The deliberate corrupt-save recovery warning is expected. These were not suppressed or attributed to the refactor.

Unity MCP had no connected Editor instance, so validation used local Editor compilation and the isolated CLI project. Null graphics runs do not establish native Game View appearance or acoustic/hardware quality. No player build, performance measurement or new multi-grandma AI is claimed. No manual scene or Inspector setup is required.
