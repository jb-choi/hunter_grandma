## 2026-10-14 · 별도 다리 단순화 · idle_breath_v2

이번 요청의 편집 결과는 [idle_breath_v2.aseprite](idle_breath_v2.aseprite)다. 기존 [idle_breath.aseprite](idle_breath.aseprite)의 다리·부츠 y49~56을 전5프레임에서 단순화한 별도 시안이다. 바지의 작은 녹색 명암 조각과 부츠 금색·가로 띠를 줄였으며, 옷자락·다른4레이어의 셀·호흡 시간·발 접지를 유지했다.

[상세 작업 안내](idle_breath_v2.md) · [전후·재생](../../Previews/IdleBreathV2/index.html) · [이번 범위 QA](../../QA/IdleBreathV2/p001/visual_review.md) · [입력 동결본](Source/IdleBreathV2/p001/idle_breath_before.aseprite). 기존 idle_breath는 보존한 입력이자 기존 호흡 정본이며 v2와 다른 파일이다. 아래의 이전 ‘현재 정본’ 설명은 기존 호흡 파일의 이력이다.

## 현재 정본 · 새총·가방 전파 · EquipmentPropagation/p001

2026-10-12. 현재 호흡 편집 정본은 [idle_breath.aseprite](idle_breath.aseprite), 64×64 Indexed 17칸·5프레임·5레이어다. 사용자가 walk F1에 그린 `Slingshot&bag`을 전5프레임에 옮겼다. 가방132px과 새총96px은 상체의 y 이동0/1/1/1/0을 함께 따르며 머리만 추가로 내려가는 F3에서도 손과 장비 접점을 유지한다. 기존 duration360/180/300/180/180ms와 1200ms 호흡을 유지했다.

아래→위는 `Bow&Quiver`, `Slingshot&bag`, `LowerBody`, `UpperBody_Arms_Equipment`, `Head_Hat_Hair_Face`다. native와 캐릭터 출력 모두 옛 `Bow&Quiver`를 숨기고 새 장비와 몸3레이어를 표시한다. 옛 활 셀과 기존 몸 셀은 삭제하거나 수정하지 않았다. [작업 직전 보존본](Source/EquipmentPropagation/p001/idle_breath_before.aseprite) · [해시 기록](../../QA/EquipmentPropagation/p001/input_hashes.json).

수치 검수는 [native_validation.json](../../QA/EquipmentPropagation/p001/native_validation.json)의 기존 셀 변경0, 장비·PNG·마스크·8배 GIF 불일치0, 부분 알파0으로 확인했다. 작화 검수는 전5프레임의 실제64px·확대에서 손이 새총 그립 앞에 겹치고 가방이 상체 높이를 따르며 새 상의 구멍이나 발 이동이 없음을 확인했다. [검수 안내](../../QA/EquipmentPropagation/p001/README.md) · [장비 원본](../../QA/EquipmentPropagation/p001/gear_before_8x.png) · [전신5프레임](../../QA/EquipmentPropagation/p001/idle_contact_8x.png) · [실제 크기](../../QA/EquipmentPropagation/p001/idle_contact_1x.png).

사용자의 평면1프레임 [idle.aseprite](idle.aseprite)는 보존했다. HU-03~08도 이번 작업에서 변경하지 않았다. 아래의 Bow 4레이어·3레이어 호흡과 초기 축소 설명은 이전 이력이다.

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

# HU-01_idle · 64×64 디자인과 호흡 시험본

2026-10-11 · Hunter64/v001

현재 디자인 편집 원본은 사용자가 직접 수정한 **1프레임** [idle.aseprite](idle.aseprite)다. 이 키포즈를 기준으로 같은 v001에 별도 [idle_breath.aseprite](idle_breath.aseprite)를 추가한다. 디자인 원본을 덮어쓰지 않는다.

[호흡 동작 상세](idle_breath.md) · [새 재생기](../../Previews/IdleBreath/index.html) · [사용자 키포즈 보존](Source/IdleBreath/README.md) · [새 파일 검사](../../QA/IdleBreath/native_validation.json) · [새 시각 검사](../../QA/IdleBreath/visual_review.md)

머리 범위는 모자·깃털·머리카락·얼굴 전체다. 상체와 머리를 함께 1px 내린 뒤 머리만 추가 1px 내리고, 머리·상체 순서로 올라와 반복한다. 하체·발은 고정한다. 구성은 5프레임·1,200ms이며 duration은 360 / 180 / 300 / 180 / 180ms다. 저장 native의 픽셀·timing·발 고정·원본 보존 검사와 전 5프레임 정적 시각 검사를 통과했다. 실시간 브라우저 반복 재생과 레이아웃 검수도 통과했다.

## 이전 300→64 초기 축소 기록

아래의 두 프레임, 각 500ms, 15색, bounds와 기존 PNG·시트·GIF·검사 JSON은 **사용자 디자인 수정 이전의 초기 축소 기록**이다. 현재 `idle.aseprite`의 상태 또는 새 `idle_breath.aseprite`의 출력으로 해석하지 않는다. 새 작업의 PNG·메타·재생·QA는 각각 `IdleBreath/` 하위에서 관리한다.

[보존한 초기 입력 원본](Source/idle_300_v006.aseprite)은 Hunter_300 v006의 300×300 idle이다. 최초 납품은 동일 구도의 64×64 최근접 축소본, 2프레임·각 500ms·정방향 `idle` 태그였다.

프레임을 개별 crop·trim·이동하지 않고 같은 중심 최근접 샘플링 좌표로 축소한다. PNG 좌상단을 좌표 원점으로 사용한다. 발 위치와 활을 쥔 손의 접점도 같은 전체 캔버스 좌표 변환을 따른다. 캐릭터 bounds는 1번 `(17, 7, 32, 50)`, 2번 `(17, 7, 33, 50)`이고 발 바닥은 두 프레임 모두 y56이다. [변환 측정](../../QA/conversion.json)과 [파일 검사](../../QA/native_validation.json)를 함께 보관한다.

| 용도 | 파일 |
|---|---|
| 현재 사용자 수정 디자인 | [idle.aseprite](idle.aseprite), 현재 1프레임으로 수정됨 |
| 프레임 | [idle_01.png](Frames/idle_01.png), [idle_02.png](Frames/idle_02.png) |
| 128×64 시트 | [idle_sheet.png](Export/idle_sheet.png) |
| 타이밍·좌표 메타 | [idle.json](Export/idle.json) |
| 축소 전 원본 | [Source 안내](Source/README.md) |
| 부위 마스크 | [Masks 안내](Masks/README.md) |
| 비교·재생 | [재생기](../../Previews/index.html), [전후 PNG](../../Previews/idle_before_after.png), [GIF](../../Previews/idle.gif) |
| 검사 | [native_validation.json](../../QA/native_validation.json), [visual_review.md](../../QA/visual_review.md) |

원본의 Indexed 17칸 팔레트는 투명 인덱스 0과 불투명 16색으로 구성된다. 축소된 결과에서는 두 프레임 모두 불투명 15색을 사용한다. 금색 그림자 인덱스 15는 샘플링에서 선택되지 않았으며 팔레트에는 유지한다. 색 평균 보간·블러·안티앨리어싱·디더링을 사용하지 않는다.

추가 픽셀 수정은 0개다. 눈은 1번 1×2px / 2번 1×3px로 샘플링되며, 이 프레임 차이를 새로 작화해 맞추지는 않았다.

기본 오른쪽 방향과 비대칭 장비를 원본대로 유지한다. 이번 시험에 왼쪽 반전 원본이나 다른 동작은 포함하지 않는다. 입력 원본의 두 프레임 사이에 있던 손·귀·머리 등의 형태 차이는 축소본에서도 남을 수 있다. 재생 중 변화와 작은 디테일의 가독성은 시각 검수 기록을 기준으로 확인한다.

Unity 출력 경로와 importer·피벗 연결은 아직 정하지 않았다. 게임용 자산 폴더를 변경하지 않은 비교 시험본이다.


## 상의 투명 구멍 후속 수정 · Pass02

사용자 디자인 원본은 그대로 보존하고 생성 `idle_breath.aseprite`의 UpperBody 레이어에 기존 윤곽색1을 `(37,43)`, `(38,44)` 두 점 추가했습니다. 부위 이동을 따라 각각 내려가며 F1/F5에서 보이던 네모 구멍을 닫습니다. F2~4에서는 기존 LowerBody와 같은 윤곽 픽셀이 겹쳐 합성 결과는 같습니다. 머리·하체·모션·timing·팔레트는 유지했습니다. 기존 이동부위 마스크는 원본1048픽셀 분류용으로 보존하고, [수정 마스크](Masks/torso_pass02.aseprite)를 별도로 저장했습니다. [최신 검수](../../QA/Animations/torso_pass02_visual_review.md)
