# UI font correction — 2026-10-28

User report: broken font rendering in the Unity game UI. The exact visual symptom and failing player were not captured. The previous BeltScroll map screenshot shows readable Korean, so it does not reproduce the reported failure.

Before: DefenseHud created an OS font from Malgun Gothic, Arial and DejaVu Sans. The bundled NotoSansCJKkr-Regular.otf was not referenced by the HUD. OS-font availability is a plausible cause; the original failure is not confirmed.

Change: the entry scene now explicitly references the bundled Noto Sans CJK KR font. HUD labels, opening captions, upgrades and the map share that font. The scene creation tool assigns the same asset. The asset is no longer destroyed as a runtime-owned font. A missing scene assignment produces an explicit error and uses the built-in font to keep the UI running.

Validation:
- `dotnet build Assembly-CSharp-Editor.csproj --nologo -v quiet`: passed, zero warnings and zero errors. This checks generated project compilation, not a Unity player build.
- Parsed the actual OTF cmap: all 257 distinct Hangul syllables occurring in runtime source have glyphs.
- Checked the scene font GUID against the asset metadata and confirmed includeFontData is enabled.
- Reviewed the three-file diff against the backup: only font selection, ownership, setup and one scene reference changed.
- Unity MCP reported no connected Editor instances; runtime appearance and the original symptom remain unverified.

Manual verification: reload the saved GrandmaDefense scene outside Play mode, then enter Play. Check opening captions, HUD, upgrades and map at the affected resolution. If the failure was in an exported player/WebGL page, rebuild that player and reload the newly built page. Confirm Korean text renders and no missing-font error appears.

Original files: Docs/Backups/BeforeUIFontFix-20261028/.
