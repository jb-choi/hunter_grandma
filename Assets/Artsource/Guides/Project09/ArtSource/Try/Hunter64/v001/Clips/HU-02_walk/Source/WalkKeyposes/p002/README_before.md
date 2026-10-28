# HU-02 / walk · 64×64 · 키포즈 작업본

2026-10-12. 현재 편집 정본은 [walk.aseprite](walk.aseprite)다. 사용자가 수정한 [idle_breath.aseprite](../HU-01_idle/idle_breath.aseprite) 1프레임을 기준으로 요청한 9프레임 구조를 만들었다. 중간 프레임 작화 전 단계이며 F2·4·6·8은 모든 레이어의 셀이 비어 있다.

[전체 재생기](../../Previews/Animations/index.html) · [9프레임 나열](../../Previews/Animations/walk_contact_6x.png) · [키포즈만 비교](../../QA/WalkKeyposes/p001/keyposes_8x.png) · [검수 기록](../../QA/WalkKeyposes/p001/README.md)

| 프레임 | 내용 | 타이밍 | 출력 |
|---|---|---|---|
| 1 | idle_breath F1 그대로 | 120ms | [PNG](Frames/walk_01.png) |
| 2 | 빈 프레임 | 120ms | [PNG](Frames/walk_02.png) |
| 3 | 발 교차 · 왼쪽 발 통과 | 120ms | [PNG](Frames/walk_03.png) |
| 4 | 빈 프레임 | 120ms | [PNG](Frames/walk_04.png) |
| 5 | 반대발 착지 · 발 순서 교환 | 120ms | [PNG](Frames/walk_05.png) |
| 6 | 빈 프레임 | 120ms | [PNG](Frames/walk_06.png) |
| 7 | 발 교차 · 오른쪽 발 통과 | 120ms | [PNG](Frames/walk_07.png) |
| 8 | 빈 프레임 | 120ms | [PNG](Frames/walk_08.png) |
| 9 | 1프레임 그대로 | 120ms | [PNG](Frames/walk_09.png) |

`idle_breath`의 4개 레이어 이름·순서·표시·불투명도·블렌드 설정을 유지했다. 아래→위: `Bow&Quiver`, `LowerBody`, `UpperBody_Arms_Equipment`, `Head_Hat_Hair_Face`. F1·9의 셀 이미지·좌표·픽셀은 기준과 정확히 같다. F3·5·7은 `LowerBody`의 다리·부츠만 수정했고, 사용자 자락 y43~48과 다른 3개 레이어는 그대로다. 다리를 잠시 구분해 작업한 뒤 `LowerBody` 한 레이어에 합쳤다.

기존 walk의 120ms 프레임 시간을 사용해 전체 1080ms다. 태그 `walk` 1~9, Indexed 17칸 팔레트를 사용한다. 이미지를 재생성하거나 보간하지 않았다. 미리보기는 키포즈 검토를 위해 일시정지로 시작하며 재생하면 빈 프레임도 그대로 표시한다.

[기준 1프레임 보존본](Source/WalkKeyposes/p001/idle_frame01_keypose.aseprite) · [입력 idle 전체](Source/WalkKeyposes/p001/idle_breath_source.aseprite) · [기존 walk 보존본](Source/WalkKeyposes/p001/walk_before.aseprite) · [시트](Export/walk_sheet.png) · [프레임 명세](Export/walk.json) · [현재 파트 마스크](Masks/material_regions.aseprite) · [파트 정의](Masks/parts.json)

기존 6프레임·9레이어 walk와 상의 Pass02 검수는 이전 작업 기록이다. [이전 README](Source/WalkKeyposes/p001/README_before.md) · [이전 마스크](Source/WalkKeyposes/p001/material_regions_before.aseprite) · [Pass02 기록](../../QA/Animations/torso_pass02_visual_review.md). 현재 walk의 판정은 위 키포즈 검수를 사용한다.
