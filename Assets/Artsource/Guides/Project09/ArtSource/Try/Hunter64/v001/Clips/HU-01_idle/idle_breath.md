## 현재 호흡 정본 · 새총·가방 전파 · EquipmentPropagation/p001

2026-10-12. [idle_breath.aseprite](idle_breath.aseprite)는 현재5프레임·5레이어다. walk F1의 사용자 `Slingshot&bag`228픽셀을 전5프레임에 옮겨 장비가 상체 y 이동0/1/1/1/0을 따른다. native와 출력은 새 장비+몸3레이어를 표시하고 옛 `Bow&Quiver`는 숨긴다. 기존 몸·옛 활 셀과 호흡시간360/180/300/180/180ms는 보존했으며 평면 `idle.aseprite`는 변경하지 않았다.

[최신 작업·검수](../../QA/EquipmentPropagation/p001/README.md) · [저장/전파 수치 검사](../../QA/EquipmentPropagation/p001/native_validation.json) · [전5프레임 확대](../../QA/EquipmentPropagation/p001/idle_contact_8x.png) · [실제64px](../../QA/EquipmentPropagation/p001/idle_contact_1x.png). 수치 검수는 불일치0으로, 작화 검수는 손–새총·가방–몸 접점과 옷·발 경계의 전신 확인으로 각각 통과했다. 아래의 Bow4레이어와 초기3레이어 설명은 이전 제작 이력이다.

## 이전 5프레임 레이어·디자인 전파 · p001

2026-10-12. 현재 호흡 애니메이션의 디자인 기준은 사용자가 수정한 `idle_breath.aseprite` F5다. F5의 4개 레이어 구분과 픽셀을 F1~4에 전파했다. F5 셀 이미지·좌표·픽셀은 그대로 보존했다. 활·화살통은 상체와 함께 움직이고 하체는 고정한다. 머리에는 모자·깃털·머리카락·얼굴이 포함된다. 프레임 시간은 기존 360/180/300/180/180ms, 합계 1200ms다.

| 레이어 (아래→위) | F1/F2/F3/F4/F5 y 이동 | F5 불투명 픽셀 |
|---|---|---:|
| Bow&Quiver | 0/1/1/1/0 | 209 |
| LowerBody | 0/0/0/0/0 | 191 |
| UpperBody_Arms_Equipment | 0/1/1/1/0 | 243 |
| Head_Hat_Hair_Face | 0/1/2/1/0 | 527 |

[최신 검수](../../QA/IdleBreath/Frame05Propagation/p001/README.md) · [기준 5프레임 보존본](Source/IdleBreath/Frame05Propagation/p001/frame05_keypose.aseprite) · [작업 직전 native](Source/IdleBreath/Frame05Propagation/p001/idle_breath_before.aseprite) · [현재 파트 마스크](Masks/idle_breath_current_parts.aseprite)

## 이전 1프레임 디자인 수정 전파 · p001

2026-10-12. 사용자가 수정한 `idle_breath.aseprite` F1을 최신 호흡 디자인 기준으로 사용한다. F1의 각 셀 이미지·좌표·픽셀은 그대로 보존하고 F2~5에 모자·머리카락·얼굴 색, 상의·손 경계, 신발 끝 삭제를 전파했다. 총24개의 보이는 수정점이 각 후속 프레임에 반영됐다. Head 레이어에 그린 몸통·손9점은 후속 프레임에서 UpperBody의1px 움직임을 따른다. 머리는1/2/1/0px, 상체는1/1/1/0px 이동하며 하체는 고정된다. 프레임5는 프레임1과, 프레임2는 프레임4와 합성 픽셀이 정확히 같다. duration360/180/300/180/180ms, 태그·팔레트는 유지했다.

[최신 검수](../../QA/IdleBreath/Frame01Propagation/p001/README.md) · [수정된 기준1F](Source/IdleBreath/Frame01Propagation/p001/frame01_keypose.aseprite) · [작업 직전 native 보존본](Source/IdleBreath/Frame01Propagation/p001/idle_breath_before.aseprite) · [현재 파트 마스크](Masks/idle_breath_current_parts.aseprite)

# HU-01_idle · 사용자 키포즈 기반 호흡

2026-10-11 · Hunter64/v001 · 같은 버전 안의 추가 애니메이션

호흡 애니메이션의 최신 디자인 기준은 사용자가 수정한 `idle_breath.aseprite`의5프레임이다. 이전 사용자 디자인 원본 [idle.aseprite](idle.aseprite)는 보존한다. 사용자가 이 파일을 1프레임으로 직접 수정한 상태를 기준으로, 별도 파일 [idle_breath.aseprite](idle_breath.aseprite)에 호흡 동작을 추가한다. `idle.aseprite`를 덮어쓰거나 기존 300→64 초기 축소 프레임을 다시 사용하지 않는다. 작업 시작 시점의 사용자 원본은 [idle_user_keypose.aseprite](Source/IdleBreath/idle_user_keypose.aseprite)에 보존한다.

## 동작 기준

사용자가 확정한 머리 범위는 모자·깃털·머리카락·얼굴 전체다. 처음에는 상체와 머리를 함께 1px 내리고, 다음에는 상체 높이를 유지한 채 머리 전체만 추가로 1px 내린다. 이후 머리부터 한 단계씩 원래 높이로 올라와 반복한다. 좌표는 캔버스 좌상단 원점이며 아래 방향이 `+y`다.

| 프레임 | 상체 y 이동 | 머리 y 이동 | 동작 | duration |
|---|---:|---:|---|---:|
| 1 | 0px | 0px | 사용자 기준 포즈 | 360ms |
| 2 | +1px | +1px | 상체·머리 함께 내려감 | 180ms |
| 3 | +1px | +2px | 머리만 추가로 내려감 | 300ms |
| 4 | +1px | +1px | 머리가 먼저 올라옴 | 180ms |
| 5 | 0px | 0px | 상체·머리가 기준 위치로 올라옴 | 180ms |

하체·발의 접지는 유지한다. 정수 픽셀 위치 이동으로 구성하며 새 이미지 생성·블러·안티앨리어싱·디더링은 사용하지 않는다. 3개 편집 레이어는 `LowerBody`, `UpperBody_Arms_Equipment`, `Head_Hat_Hair_Face`다. 머리·목과 허리에서 레이어가 겹쳐 압축되며 손·활·화살통은 상체와 함께 이동한다. 상의 투명 구멍 보완은 윤곽색2픽셀이다. 레이어 분리 위치와 접점 처리의 실제 범위는 [동작 명세](../../Prompts/IdleBreath/motion_spec.md)에 기록한다. 타이밍은 저장 native·메타·GIF에서 일치함을 확인한 5프레임·1,200ms 반복이다.

캔버스는 64×64 Indexed이며 기존 17칸 팔레트(투명 0 + 불투명 16색)를 유지한다. 현재 생성 프레임의 실제 불투명 사용색은 15색이다. 신규 색은 없다. 시트는 5셀을 가로로 배치한 320×64 PNG다. 발 바닥은 모든 프레임에서 y56에 둔다.

## 파일

| 용도 | 경로 |
|---|---|
| 디자인 편집 원본 | [idle.aseprite](idle.aseprite) |
| 별도 애니메이션 편집 원본 | [idle_breath.aseprite](idle_breath.aseprite) |
| 시작 원본 보존 | [Source/IdleBreath](Source/IdleBreath/README.md) |
| 이동 부위 마스크 | [idle_breath_parts.aseprite](Masks/idle_breath_parts.aseprite) |
| 64×64 프레임 PNG | [F1](Frames/IdleBreath/idle_01.png), [F2](Frames/IdleBreath/idle_02.png), [F3](Frames/IdleBreath/idle_03.png), [F4](Frames/IdleBreath/idle_04.png), [F5](Frames/IdleBreath/idle_05.png) |
| 시트·타이밍 | [idle_sheet.png](Export/IdleBreath/idle_sheet.png), [idle.json](Export/IdleBreath/idle.json) |
| 팔레트 | [palette.json](Export/IdleBreath/palette.json) |
| 재생 | [재생기](../../Previews/IdleBreath/index.html), [GIF](../../Previews/IdleBreath/idle.gif), [8배 GIF](../../Previews/IdleBreath/idle_8x.gif) |
| 프레임 나열 | [6배 contact](../../Previews/IdleBreath/contact_6x.png) |
| 검수 | [native_validation.json](../../QA/IdleBreath/native_validation.json), [시각 검수](../../QA/IdleBreath/visual_review.md), [원본 해시](../../QA/IdleBreath/source_hash.json) |

저장된 native를 독립적으로 다시 읽어 부위별 정수 이동, 5프레임 PNG·시트·GIF·8배 GIF 일치, 기존 팔레트·알파, 발 고정, 원본 보존을 [수치 검사](../../QA/IdleBreath/native_validation.json)에서 통과했다. 픽셀 불일치·새 색·반투명 픽셀은 모두 0개다. F1/F5는 상의 구멍2점을 제외하면 사용자 키포즈와 같고 F2/F4는 같은 하향 자세다. 이 동일 프레임들도 각각 duration을 가지며 반복 경계에서는 위치가 바뀌지 않는다.

[정적 시각 검사](../../QA/IdleBreath/visual_review.md)는 실제 64px와 8배 확대의 전 5프레임을 확인했다. 디자인·머리 디테일·목과 허리 접점·손–활 접점·발 접지 유지로 판정했다. F3의 목 압축과 상체–코트 중첩은 의도한 동작이며 추가 접점 작화 필요는 발견하지 못했다. 실시간 브라우저 반복 재생과 레이아웃 검수도 통과했다. Unity 적용은 이번 작업에 포함하지 않는다.

기존 `Frames/idle_01.png`, `Frames/idle_02.png`, `Export/idle_sheet.png`와 루트 `Previews/index.html`, `QA/native_validation.json`은 사용자 수정 이전의 **300→64 초기 축소 2프레임 기록**이다. 현재 디자인 원본 또는 이번 호흡 애니메이션의 출력·검수로 사용하지 않는다.


## 상의 투명 구멍 후속 수정 · Pass02

사용자 디자인 원본은 그대로 보존하고 생성 `idle_breath.aseprite`의 UpperBody 레이어에 기존 윤곽색1을 `(37,43)`, `(38,44)` 두 점 추가했습니다. 부위 이동을 따라 각각 내려가며 F1/F5에서 보이던 네모 구멍을 닫습니다. F2~4에서는 기존 LowerBody와 같은 윤곽 픽셀이 겹쳐 합성 결과는 같습니다. 머리·하체·모션·timing·팔레트는 유지했습니다. 기존 이동부위 마스크는 원본1048픽셀 분류용으로 보존하고, [수정 마스크](Masks/torso_pass02.aseprite)를 별도로 저장했습니다. [최신 검수](../../QA/Animations/torso_pass02_visual_review.md)
