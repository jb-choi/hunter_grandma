# Reload audio import API correction

2026-10-30, Unity 6000.3.10f1 isolated batch project.

The first Editor compilation rejected AudioImporter.preloadAudioData with CS0619 in GrandmaStageSetup and GrandmaReloadAudioValidation. Unity's diagnostic states that this setting moved to AudioImporter.SampleSettings as a per-platform setting.

Corrected both sites to defaultSampleSettings.preloadAudioData before repeating compilation/validation. No gameplay run occurred during this failed compile. This attempt is not a passing validation result.
