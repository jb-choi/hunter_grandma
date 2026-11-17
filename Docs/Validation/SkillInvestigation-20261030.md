# Skill visibility investigation

2026-10-30 KST · Unity 6000.3.10f1 Editor

User report: the skill could not be demonstrated in normal gameplay.

## Evidence

- Ordinary save before investigation: `shadowSkillEnabled=false`, Attack Lv.7 and Fire Rate Lv.5. Damage is 38; ordinary stage zombie base HP ranges from 10 to 34. This profile therefore kills ordinary zombies with one normal primary hit.
- A separate Library profile was cloned from that current growth state and enabled the skill. The ordinary profile was never loaded as the active test store or changed.
- A natural Stage 0 run completed with no runtime errors and **one real skill activation**. Its first active sample was at encounter elapsed 12.564 seconds, with **2 live zombies but only 1 eligible target**. The sequence fired once and returned, with about 0.7 seconds of active presentation observed.
- The extra encounter elapsed time was a wave break: at elapsed 7.770 the phase was WaveBreak and cooldown was 2.322; at elapsed 10.317 Waves resumed with cooldown 2.248. It was not evidence of a 2.6-second wait for a target. The skill's ten-second timer excludes the wave break while encounter Elapsed includes it.
- A separate demonstration with five durable eligible zombies completed **two automatic activations, each with all five shots**, and recorded zero runtime errors. Natural source rules, the actual skill sequence and current 38-damage progression were reused. Normal automatic shots were reserved in this demonstration so they could not clear the targets first.

## Cause and scope

The skill execution is working. The normal saved setting is OFF, and when ON the sparse weak enemies at current growth values make the skill much shorter than the intended five-target showcase. Only eligible targets from the activation snapshot are visited; when they are all dead, the sequence correctly returns early. Current gameplay therefore does not reliably demonstrate a full five-shot sequence.

The current cooldown measures active Waves/BossFight time, not ten wall-clock seconds from scene entry. Opening, wave breaks, capture and results do not advance it. Boss-only combat cannot demonstrate this skill because bosses are excluded from its target list.

No production combat balance, cooldown, targeting, save defaults or enemy HP was changed by this investigation.

## Repeatable Editor reproduction

New Editor menus under `Tools → Hunter Grandma`:

- `Investigate Skill in Natural Combat`: clone the current growth profile into Library, switch the clone ON and log a natural Stage 0 run.
- `Demonstrate Shadow Skill (Isolated Profile)`: use five durable ordinary zombies and ON at normal game speed to demonstrate two complete automatic sequences. It uses a separate Library profile and exits Play Mode afterward.

The existing local command bridge also accepts `investigate-skill` and `demo-skill`. Reports are `SkillInvestigation-natural.json` and `SkillInvestigation-demo.json`; the temporary live sample is `Library/SkillInvestigation-current.json`. These tools are Editor-only and do not register gameplay shortcuts.

Validation evidence: actual Unity Editor compilation, natural-gameplay reproduction, two full five-shot demonstrations, zero recorded runtime errors. Ordinary profile, scene, manifest, Player Settings and Build Settings are compared against this investigation's fresh baseline; do not restore old profiles from earlier tasks.
