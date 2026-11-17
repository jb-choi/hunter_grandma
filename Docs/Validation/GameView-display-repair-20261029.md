# Game View display repair

2026-10-29 KST · Unity 6000.3.10f1 / Web / DX12

Observed in the user's running Editor: the native Game View stretched the entire frame horizontally, including both HUD text and world sprites. The selected size was `HuntPlan 960x540`; the distorted frame occupied a wider area than its normal centered fixed-resolution presentation.

Selecting `16:9 Aspect` immediately restored normal text and character proportions. Returning to `HuntPlan 960x540` also rendered correctly at its centered 960x540 size. Restoring and maximizing the Editor window retained the correct proportions. The final selected view is `16:9 Aspect`, which fills the available pane while preserving the frame ratio.

Confirmed: reapplying the Game View size repaired the visible state. Likely mechanism: stale Game View zoom/render-area sizing. The underlying Unity internal cause and original trigger were not reproduced; the custom 960x540 definition itself is not proven faulty because it worked after reapplying it.

Validation was direct native Game View observation during Play Mode: HUD, grandma, background and result panel displayed normally; both aspect modes and a restore/maximize cycle were inspected. This is a local Editor display repair, with no gameplay, camera, sprite, scene or save-schema edits. The user's existing Play session was retained. No additional compilation, player build or automated gameplay suite was needed for changing the Editor view selection. Earlier offscreen image captures did not establish native Game View correctness.
