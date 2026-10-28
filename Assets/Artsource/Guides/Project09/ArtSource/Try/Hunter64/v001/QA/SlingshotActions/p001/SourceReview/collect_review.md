# HU-05 collect independent visual review

2026-10-13. Current four-frame `QA/SlingshotActions/p001/Collect/candidate.aseprite` was reopened read-only, rendered independently, and compared with its candidate PNGs. All four frames were inspected at native 64 px and 8×, including the whole head, shirt, both arms, bag, slingshot, bent knees, and boots. No candidate pixels were changed by this review.

[Native 64 px contact](collect_native_contact_1x.png) · [Native 8× contact](collect_native_contact_8x.png) · [Torso, hands, and feet close-up](collect_torso_foot_contact_8x.png) · [Static native checks](native_static_review.json)

**Static drawing verdict: pass. No required pixel correction identified.**

| Region or action | Observation |
|---|---|
| Source design | F1 matches the current slingshot-and-bag idle appearance. Hat, feather, hair, face, and eye retain their source shapes. |
| Head and neck | Whole head shifts (1,3) in F2 and (3,6) in F3. Neck and scarf remain joined to the compressed torso. Head translation comparison has zero pixel differences in every frame. |
| Shirt behind arms | Moving the free arm exposes continuous green shirt and a closed waist. No rectangular transparent cutout appears in F2/F3/F4. |
| Pickup hand | The opposite hand lowers to (30,49) then (34,54), near the feet. The slingshot stays in the front hand at (47,44) then (49,45). The earlier proposed contact (51,54) and hand transfer are not part of the approved candidate. |
| Knees and boots | The crouch has outward bent knees and connected trouser-to-boot shapes. All original opaque boot pixels remain exact, and the ground stays y56. F3 adds nine joint/clothing pixels in formerly empty areas of LowerBody y51..53; these are intentional crouch connections, not changed source boot colors. |
| Bag | It follows the body at (1,2), then (3,3). Its lowest point reaches y56 in the crouch and does not extend beneath the feet. The large bag face and its source colors remain coherent. |
| Return hand | F4 rises and brings the free hand to the bag mouth at (25,39); the hand and bag tie meet while the front hand retains the slingshot. |
| Native size reading | At 64 px, the dip and low reach are visible, followed by a raised free hand at the bag. The object being collected is not drawn into the character sprite. |

Native-to-candidate PNG mismatch count is zero in all four frames. The five source layer names and order are retained; `Bow&Quiver` is hidden and the four current body/gear layers are visible. Static contact inspection does not certify playback timing or Unity behavior; the root task performs player validation separately. The four 150 ms frames describe the requested collect action, with the pickup event at F3.
