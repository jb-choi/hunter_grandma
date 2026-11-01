# Sundeok reload audio validation

Unity 6000.3.10f1 · 2026-10-31 01:11 KST

Status: PASS

Isolated profile: Library/ReloadAudio-validation.json. Encounter Update is disabled after skipping the opening; audio and actual animation presentation continue. One synthetic balance funds fire-rate upgrades only in this fixture. Timeline tests use an independent presentation clone and public timeline methods via reflection; no computer-use/native UI input is used.

- PASS: The actual scene assigns the licensed reload WAV to Sundeok
- PASS: Imported audio preserves the prepared 44.1kHz mono 0.436-second recording
- PASS: Short reload audio is preloaded PCM and does not wait on background decoding
- PASS: One persistent reload source supplements the existing gunshot source
- PASS: Idle and Collect never request a reload sound
- PASS: The committed firing frame does not prematurely play a reload sound
- PASS: Crossing reload frame 2 in one long frame requests exactly one sound
- PASS: Moving upper-body reload uses the same sound cue
- PASS: Pause and repeated sampling of the reload frame do not duplicate the cue
- PASS: Cancelling an active reload never schedules a delayed cue
- PASS: Fast left-facing reload requests one cue despite skipping intermediate frames
- PASS: Real Sundeok does not start the reload sound during the firing pose
- PASS: Paused presentation neither advances into reload nor emits a cue
- PASS: Actual animation triggers one clip at the loading-hand frame
- PASS: The dedicated persistent 2D source plays the correct non-looping clip
- PASS: Reload respects the existing source mixer, mute and master volume
- PASS: Normal cadence preserves the recording's pitch and leaves gunshots unchanged
- PASS: Pausing an active reload preserves its one-shot event and frozen pose
- PASS: Pause suspends the active AudioSource without restarting it
- PASS: The completed reload does not repeat its sound in idle
- PASS: Muted weapon audio produces no audible reload request
- PASS: Rapid-fire upgrades shorten reload audio with bounded pitch on its own source
- PASS: A new gunshot stops the previous reload tail instead of stacking voices
- PASS: Stopping combat cancels pending/repeated reload playback
- PASS: A missing optional clip does not break animation or emit a sound
- PASS: Audio and animation-only calls never generate gameplay shots or kills
- PASS: Retry clears playback while preserving the borrowed clip asset
- PASS: No gameplay errors, exceptions or assertions in reload audio validation

AudioSource reported active playback: True. This is routing/timing validation, not acoustic listening or a hardware-output measurement.


Separate Unity Editor Search startup errors:
ArgumentOutOfRangeException: Index was out of range. Must be non-negative and less than the size of the collection.
Parameter name: index
System.Collections.Generic.List`1[T].get_Item (System.Int32 index) (at <41d29b352f6a475ab1bf7c6628b82790>:0)
UnityEditor.Search.SearchDatabase+<EnumerateAll>d__80.MoveNext () (at <e5e60b34d51d47fbbbfacfb81405fd89>:0)
System.Linq.Enumerable.TryGetFirst[TSource] (System.Collections.Generic.IEnumerable`1[T] source, System.Boolean& found) (at <80564d689e48448fa2a7101993ee7201>:0)
System.Linq.Enumerable.First[TSource] (System.Collections.Generic.IEnumerable`1[T] source) (at <80564d689e48448fa2a7101993ee7201>:0)
UnityEditor.Search.SearchDatabase.GetDefaultSearchDatabase () (at <e5e60b34d51d47fbbbfacfb81405fd89>:0)
UnityEditor.Search.SearchInit.IndexationOnStartup () (at <e5e60b34d51d47fbbbfacfb81405fd89>:0)
UnityEditor.EditorApplication.Internal_CallDelayFunctions () (at <49fcb209645d4ccfb3e373da32345d37>:0)
