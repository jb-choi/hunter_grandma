# Grandma Defense combat smoke test

Unity 6000.3.10f1

Status: FAIL

Scene: `Assets/HunterGrandma/Scenes/GrandmaDefense.unity`

Sustained run: 38 spawned, 32 defeated in 60 simulated seconds.

- PASS: Zombie pool initialized to 20 actors
- PASS: Scene starts with three approaching zombies
- PASS: Zombie advances left toward grandma
- PASS: Automatic gunfire and projectile hits defeat the opening zombies
- PASS: Spawning and kills continue for 60 simulated seconds
- PASS: Default balance prevents zombies reaching grandma
- PASS: Endless combat keeps a bounded actor pool
- PASS: Defeated zombie remains visible during its fall and fade
- PASS: Pause freezes timers and zombie motion
- PASS: Resume restores the previous game speed
- PASS: Restart clears scores and resets the opening wave
- PASS: Restart cancels in-flight projectiles
- PASS: Automatic combat resumes after restart

Failure: Cannot send delta state events against bitfield controls: Key:/Combat validation keyboard/space
