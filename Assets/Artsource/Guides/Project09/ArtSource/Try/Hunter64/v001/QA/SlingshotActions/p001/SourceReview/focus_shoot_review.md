# HU-03 focus / HU-04 shoot independent visual review

2026-10-13. The saved `Clips/HU-03_focus/focus.aseprite` and `Clips/HU-04_shoot/shoot.aseprite` were reopened read-only. The eight frames were independently rendered and inspected head to feet at native 64 px and 8×. No clip pixels were changed by this review.

[Focus at 64 px](focus_native_contact_1x.png) · [Focus at 8×](focus_native_contact_8x.png) · [Shoot at 64 px](shoot_native_contact_1x.png) · [Shoot at 8×](shoot_native_contact_8x.png) · [Static native checks](native_static_review.json)

**Static drawing verdict: pass for both clips. No required pixel correction identified.**

| Region or action | Observation |
|---|---|
| Whole head | The source face, ear, hair, feather, and hat shapes remain together. Focus keeps their position fixed; shoot F2 uses a single -1x recoil translation. Source-head comparison has zero pixel differences in every frame. |
| Shirt and waistband | Restored shirt planes remain behind the raised arms. Both shoulder connections are closed and no rectangular shirt hole appears. Buckle, waist, hem, trousers, and the fixed boots remain continuous. |
| Front gripping arm | It rises and extends toward the weapon. The pale fingers cover the green handle naturally because gear remains below UpperBody. Grip follows the hand at focus (47,38), (49,36), (50,36). |
| Drawing arm | The opposite arm bends toward the pouch and cheek. Focus F3/F4 read as increasing pull; the hand beside the jaw meets the pouch. Pale fingertips near the face do not replace the eye or change the source face shape. |
| Slingshot fork and bands | The source green fork and long handle remain recognizable. Orange bands are thin, hard-edged pixel paths meeting the drawn pouch. In F4 the pouch is beside the cheek at (41,30), with fork points (48,24)/(53,24). Some band pixels are intentionally occluded by fingers or fork surfaces. |
| Release and recovery | Shoot F1 brings the pouch forward to (50,27), showing slackened bands while keeping the weapon at its aim height. F2 moves the body/head slightly back, F3 lowers the arms, F4 returns to the exact current idle appearance. |
| Bag and feet | Bag remains attached at the left hip; recoil moves it -1x with the torso. All source LowerBody rows y51 onward are exact in all eight frames and the visible ground stays y56. |
| Native size reading | At 64 px the raising, draw, release, and recovery stages are distinguishable; the final fork reads as a slingshot. Band details are small at this intended size, but their stretched-versus-released silhouette changes are visible. |

[Focus torso and feet close-up](focus_torso_foot_contact_8x.png) · [Shoot torso and feet close-up](shoot_torso_foot_contact_8x.png)

Native-to-PNG pixel mismatches are zero in all eight frames. Both files retain the five source layer names and their order, with `Bow&Quiver` hidden and the four current body/gear layers visible. The action art uses existing indexed colors with crisp edges. This review covers static drawing and source/native correspondence; playback timing and live player behavior are validated separately by the root task.
