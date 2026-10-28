## 2026-10-13 rework2 · Frame1 부위 실루엣

현재 [focus_rework2.aseprite](focus_rework2.aseprite)는 이 부위 구성을 사용한 **8F focus 실루엣 시안**이다. F1은 보존하고 들기→잡기→당김→조준 유지,총1210ms로 구성했다. [8F 재생기](Previews/Rework2/Focus8/index.html) · [최신 시안 QA](QA/Rework2/Focus8/p001/visual_review.md) · [기준1F 보존본](Source/Rework2/Focus8/p001/focus_rework2_base.aseprite). 아래1F 설명과 출력은 분리 당시 이력이다.

[focus_rework2.aseprite](focus_rework2.aseprite)는 rework F1 기준의 64×64·1F·8레이어 단색 실루엣이다. 머리/상반신/양팔/하반신/양다리/새총+자루를 서로 다른 색으로 분리했다. [색·레이어 안내](focus_rework2.md) · [미리보기](Export/Rework2/focus_rework2_preview.png) · [단독/숨김 QA](QA/Rework2/p001/visual_review.md). 이번 단계는 기준 Frame1 분리이며 새 조준 동작은 아직 만들지 않았다.

## 2026-10-13 별도 rework

[focus_rework.aseprite](focus_rework.aseprite)를 최신 idle F1의 5개 레이어로 별도 제작했다. 기존 focus는 유지한다. 4F·600ms·마지막 조준 유지. 소매·보호대·손, 팔꿈치와 손목 높이, 당김 손–고무줄 접점을 조정했다. [비교·레이어 재생기](../../Previews/ReworkActions/index.html) · [QA](../../QA/ReworkActions/p001/visual_review.md) · [작업·출력 안내](../../QA/ReworkActions/p001/README.md). 아래는 기존 focus 제작 이력이다.

## 현재 정본 · idle 기반 새총 동작 · SlingshotActions/p001

2026-10-13. [focus.aseprite](focus.aseprite)는 현재 [idle_breath F1 보존본](Source/SlingshotActions/p001/idle_breath_source.aseprite)에서 만든 64×64 Indexed 동작이다. 기준과 같은 5개 레이어 이름·순서·표시 설정을 유지했다. Bow&Quiver는 숨김이고 새총·자루와 몸3레이어를 표시한다.

동작: 새총 올리기 → 고무줄 당기기 → 조준 자세 유지. 4프레임·600ms. 머리·모자·머리카락·얼굴은 source 픽셀의 정수 이동이며, 팔과 가려진 상의 면은 같은 팔레트로 직접 수정했다. 추가 네이티브 레이어·재생성·블러·안티앨리어싱·디더링 없이 작업했다.

[기존 파일 보존본](Source/SlingshotActions/p001/focus_before.aseprite) · [최신 작업·검수](../../QA/SlingshotActions/p001/README.md) · [수치 검수](../../QA/SlingshotActions/p001/native_validation.json) · [현재 전신 확대](../../QA/SlingshotActions/p001/focus_contact_8x.png) · [전후 비교](../../QA/SlingshotActions/p001/focus_before_after_8x.png) · [재생기](../../Previews/Animations/index.html). 아래는 이전 활 동작 제작 이력이다.

# HU-03 / focus · 64×64

사용자가 직접 수정한 [기준 원본](../HU-01_idle/idle.aseprite)의 1프레임을 사용한 픽셀 포즈 시험본입니다.

활 들기 → 손 정렬 → 시위 당기기 → 조준 유지.

편집 파일: [focus.aseprite](focus.aseprite) · [전체 재생기](../../Previews/Animations/index.html) · [6배 프레임 나열](../../Previews/Animations/focus_contact_6x.png)

4프레임 / 600ms / 마지막 포즈 유지 / Indexed 17칸(투명0+16색), 신규 색 없음. 머리·모자·머리카락·얼굴은 기존 픽셀을 정수 이동하며 팔·활·관절은 같은 팔레트로 동작에 맞춰 구성했습니다.

| 프레임 | 타이밍 | 출력 |
|---|---|---|
| 1 | 150ms | [PNG](Frames/focus_01.png) |
| 2 | 150ms | [PNG](Frames/focus_02.png) |
| 3 | 150ms | [PNG](Frames/focus_03.png) |
| 4 | 150ms | [PNG](Frames/focus_04.png) |

[원본 보관본](Source/hunter_user_keypose.aseprite) · [시트](Export/focus_sheet.png) · [타이밍·접점·이벤트](Export/focus.json) · [부위 마스크](Masks/material_regions.aseprite) · [파트 정의](Masks/parts.json)

9개의 편집 레이어로 머리·몸통·양팔·양다리·활·화살통·시연 소품을 구분합니다. 운반/수집/견인/적재에서 활은 등에 보관하는 위치로 이동합니다. DemoProps는 시연용 소품 레이어이며 기본 숨김입니다. 캐릭터 PNG/시트/GIF에는 포함하지 않고 재생기의 '시연 소품'을 켜면 비교 합성본을 보여줍니다. 토끼·견인 짐·수레는 접점 시연을 위한 단순 소품이며 실제 게임 동물/수레 원본은 교체하지 않았습니다.

[파일 검수](../../QA/Animations/native_validation.json) · [시각 검수](../../QA/Animations/visual_review.md). 이번 파일은 v001 포즈·타이밍 시험본이며 Unity 연결·이벤트 소비와 최종 작화 확정은 별도입니다.


## 상의 빈 면·투명 구멍 · Pass02

이전 오른쪽14점 수정에서 누락된 왼쪽 숨은 상의 면과 상의 안의 작은 투명 구멍을 추가로 수정했습니다. 변경은 Body 레이어에만 있으며 팔·머리·다리·장비·소품·duration은 유지했습니다. [실제 수정 마스크](Masks/torso_pass02.aseprite) · [최신 전후·검수](../../QA/Animations/torso_pass02_visual_review.md) · [수정 직전 native](../../Source/TorsoOcclusion/Pass02/Before/Clips/HU-03_focus/focus.aseprite)
