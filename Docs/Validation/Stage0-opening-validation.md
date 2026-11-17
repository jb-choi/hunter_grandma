# Stage 0 opening and zombie attack validation

Unity 6000.3.10f1 · 2026-10-29 16:19 KST

Status: PASS

Isolated profile: Library/Stage0-opening-validation.json. Includes 100 synthetic test currency for the intro purchase guard. Zombie attack cases temporarily disable only the encounter driver and cancel its old projectiles; the live zombie, visual, collision and damage code continue to run.

- PASS: Stage 0 starts with the tea opening before combat
- PASS: Opening uses the fresh export of the user-specified seated tea sprite
- PASS: Window is black before the exterior reveal
- PASS: Outside remains concealed while grandma drinks tea
- PASS: Intro has no live attackers, shots or combat-clock progress
- PASS: Only the seated opening actor is shown during the intro
- PASS: Map travel, movement and retreat cannot interrupt the intro
- PASS: Opening cannot receive combat damage
- PASS: Upgrades cannot spend even affordable test funds during the intro
- PASS: Pause freezes the intro timeline
- PASS: Movement input during paused intro cannot move or fire the live grandma
- PASS: Exterior reveal has a growl and repeated staggered fence strikes
- PASS: Fence runs from back-left to front-right as requested
- PASS: Diagonal fence placement does not tilt the upright posts
- PASS: Darkness lifts to reveal the zombies at the fence
- PASS: Storyboard zombies remain presentation only and grant no rewards
- PASS: Grandma leaves her chair and moves toward the open door
- PASS: The exported chair layer stays behind after grandma stands
- PASS: Combat still waits until the exit shot finishes
- PASS: The opening releases live fence zombies and automatic shotgun fire
- PASS: The first Stage 0 wave keeps banging the fence after the opening
- PASS: Live belt actor and normal controls resume after the opening
- PASS: Gameplay begins outside the doorway instead of returning grandma to the chair
- PASS: Opening actor is hidden without duplicating the live grandma
- PASS: Normal retry skips the already-seen intro in this session
- PASS: Component context-menu replay cleanly resets the opening
- PASS: Enter skips directly into the same Stage 0 combat
- PASS: Skipping grants no currency, captive or clear reward
- PASS: Repeated skip callbacks cannot start extra waves
- PASS: Fence zombie draws back before striking without pursuing grandma
- PASS: Fence strike extends the body while keeping feet at the diagonal fence
- PASS: Hitting the fence does not deal damage to grandma
- PASS: Pause freezes the live fence attack pose
- PASS: Killing a fence zombie cancels its repeated strike
- PASS: Pool reuse clears the fence assignment for normal pursuit
- PASS: Pooled zombie starts without a stale attack state
- PASS: Left-facing zombie pulls its feet backward before attacking
- PASS: Windup visibly draws the body back as well as the feet
- PASS: Windup does not deal contact damage
- PASS: Zombie lunges forward with an extended body pose
- PASS: Forward strike deals one hit at the strike timing
- PASS: Strike is followed by a recovery pose
- PASS: Recovery does not repeat the strike damage
- PASS: Moving to a different belt depth avoids the committed strike
- PASS: Killing a winding-up zombie cancels the pending attack
- PASS: A dead zombie cannot deal a delayed ghost hit
- PASS: Pool reuse resets attack timer and pose
- PASS: No runtime errors, exceptions or assertions in opening/attack validation