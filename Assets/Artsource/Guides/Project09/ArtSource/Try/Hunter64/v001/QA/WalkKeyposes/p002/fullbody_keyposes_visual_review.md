# Full-body walk keypose candidate review

Candidate: [fullbody_keyposes.aseprite](fullbody_keyposes.aseprite), 64×64 indexed17, the four current idle layer names and order, three frames mapping to walk F3/F5/F7. Input [idle source](../../../Clips/HU-02_walk/Source/WalkKeyposes/p002/idle_breath_source.aseprite) and [prior nine-frame walk](../../../Clips/HU-02_walk/Source/WalkKeyposes/p002/walk_before.aseprite) are backed up by the parent task.

[8× full-character contact](keypose_contact_8x.png) · [1× contact](keypose_contact_1x.png) · [movement masks](movement_masks.aseprite) · [pixel/offset specification](fullbody_keyposes.json) · [saved-native validation](fullbody_keyposes_validation.json).

| Walk keypose | Head/body/skirt bob | Rear hand | Front hand and bow | Foot behavior |
|---|---:|---:|---:|---|
| F3 | −1y | −1x | +1x | Prior lifted rear foot/planted front foot unchanged. |
| F5 | +1y | −2x | +2x | Prior opposite contact and both grounded soles unchanged. |
| F7 | −1y | +2x | −2x | Prior lifted front foot/planted rear foot unchanged. |

The shoulder pixels stay fixed relative to the bobbed torso. Forearm/hand movement increases by source row toward the distal hand; the row rule is recorded in JSON. Actual green-ramp sleeve boundaries supplement the front arm mask at source x36/37. Hidden shirt surfaces behind both sleeves prevent the moving arms from exposing the former rectangular cuts. During visual review a one-pixel right-waist gap in F3 was found and closed with the existing outline index at canonical (38,43), then all three native poses were reviewed again.

The combined Bow&Quiver layer is processed as two temporary parts: the quiver follows body height only, while the bow follows the front hand horizontally and body height vertically. They are merged back into their original single layer. Head/hat/feather/hair/face pixels are exact source translations. Source skirt pixels move vertically intact. Hip joints connect the translated hem to the same existing boot positions; the prior LowerBody rows y51..63 remain exact.

Final inspection covered all three full characters at 1× and 8×, then reopened the native torso/hand/hem/boot regions on a bright transparency background. The arm swing reverses between F3 and F7, contact F5 sits lower and both passing poses sit higher. Both shoulders remain attached, the bow grip follows its hand, and no rectangular shirt hole remains. The boots retain their established shapes, colors and ground contact. The existing single negative-space pixel at (31,53) in F5 lies between the boots, outside the clothing surface.

Saved-native validation passed with zero head/equipment translation mismatches, zero translated-skirt mismatches, zero prior lower-row y51..63 differences, zero PNG mismatches, zero partial-alpha pixels and no isolated body-alpha candidates. Each pose still has its lowest visible pixel at y56. This subtask edited only QA/candidate files; the parent task assembles and validates the final nine-frame walk and its exports.
