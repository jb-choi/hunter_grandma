# Hunter64 v001 · idle 호흡 동작 명세

2026-10-11 · HU-01_idle · 기존 Aseprite 픽셀 기반

입력은 사용자가 1프레임 디자인으로 수정한 [idle.aseprite](../../Clips/HU-01_idle/idle.aseprite)다. 보존한 시작 원본은 [idle_user_keypose.aseprite](../../Clips/HU-01_idle/Source/IdleBreath/idle_user_keypose.aseprite), 추가 편집 파일은 [idle_breath.aseprite](../../Clips/HU-01_idle/idle_breath.aseprite)다. Hunter64/v001 안에서 작업하며 원본을 덮어쓰지 않는다.

## 사용자가 지정한 동작

1. 기준 포즈에서 상체와 머리를 함께 아래로 1px 이동한다.
2. 상체의 높이는 유지하고 머리 전체를 추가로 아래로 1px 이동한다.
3. 머리를 1px 올린 뒤 상체와 머리를 1px 올려 기준 높이로 돌아온다.

머리는 **모자·깃털·머리카락·얼굴 전체**다. 얼굴 피부만 따로 이동하지 않는다. 눈·귀·머리카락 등 기존 디자인 디테일은 해당 머리 레이어와 함께 이동한다. 하체와 발 위치는 고정한다. 동작은 5프레임의 `(상체 y, 머리 y)` 이동 `(0,0) → (1,1) → (1,2) → (1,1) → (0,0)`으로 구성한다. x 이동은 0이다.

좌표는 64×64 캔버스의 좌상단 원점이며 아래가 양의 y다. 최초 키포즈에서 매 프레임을 직접 구성하여 누적 이동에 따른 픽셀 손실을 피한다. 새 이미지 생성이나 보간 없이 기존 팔레트와 정수 위치 이동을 사용한다.

## 타이밍과 연결

확정 duration은 F1 360ms, F2 180ms, F3 300ms, F4 180ms, F5 180ms이며 전체 1,200ms 반복이다. 끝 프레임은 시작 위치와 같은 포즈이므로 반복 경계에서 위치가 튀지 않도록 확인한다. 시작·끝 포즈가 같은 그림이어도 각각 duration을 갖는다. `idle` 태그는 F1–F5 정방향 반복이다. 저장 native·PNG 메타·GIF duration의 일치는 [native 검사](../../QA/IdleBreath/native_validation.json)에서 확인했다.

## 편집 레이어와 부위 범위

새 native 파일은 다음 3개 캐릭터 레이어로 나눈다. 같은 부위를 확인하는 작업 마스크는 [idle_breath_parts.aseprite](../../Clips/HU-01_idle/Masks/idle_breath_parts.aseprite)이며 1프레임 키포즈의 동일 분리를 사용한다. 아래 좌표 범위는 끝 값을 포함하고, 투명 픽셀은 부위 수에 포함하지 않는다.

| 레이어 | 입력 키포즈의 범위 | 원본 불투명 픽셀 | 동작 |
|---|---|---:|---|
| `LowerBody` | y44: x27–37 / y45–47: x27–38 / y48: x27–37 / y49 이상: x40 이하. 단 x40/y49–53의 활 윤곽 제외 | 162 | 아래 옷자락·하체·부츠 고정 |
| `UpperBody_Arms_Equipment` | 머리와 하체에 속하지 않는 모든 비투명 픽셀. x40/y51–52의 활 아래 윤곽 포함 | 359 | 목·스카프·몸통·벨트·양팔·손·활·화살통을 함께 0 또는 +1px 이동 |
| `Head_Hat_Hair_Face` | y29 이하 전체 / y30: x25–39 / y31: x26–38 | 527 | 모자·깃털·머리카락·얼굴 전체를 0 / +1 / +2px 이동 |

그리는 순서는 하체 → 상체·장비 → 머리다. 목 y32는 상체에 남기고 머리가 목·스카프 위에 겹친다. 상체가 1px 내려올 때 고정된 아래 옷자락 위로 겹쳐 허리 부분이 짧아진다. 손·활·화살통은 상체 레이어에서 함께 움직여 장비 접점을 유지하도록 구성했다. 개별 연결부를 보간하거나 새 색으로 덧칠하지 않았으며 접점 추가 작화는 0픽셀이다.

중첩으로 프레임별 비투명 픽셀 수가 바뀐다. 현재 생성 메타 [motion.json](../../QA/IdleBreath/motion.json)에 F1/F5 1,048개, F2/F4 1,037개, F3 1,025개로 기록한다. 이는 레이어가 겹쳐 가려지는 결과이며 매번 최초 키포즈를 참조해 원래 위치로 돌아온다. 하체·발 고정, 머리·목과 허리 중첩의 시각 판정은 별도 [visual_review.md](../../QA/IdleBreath/visual_review.md)로 확인한다. 이 구성 설명만으로 접점의 시각 검수 완료를 선언하지 않는다.

기존 Indexed 17칸 팔레트(투명 0 + 불투명 16색)를 유지하고, 현재 생성 프레임의 실제 불투명 사용색은 15색이다. 좌표 이동만 수행하며 새 팔레트 색은 없다.

## 출력과 검수

- 프레임: [Frames/IdleBreath](../../Clips/HU-01_idle/Frames/IdleBreath/idle_01.png), 64×64, 투명 PNG.
- 시트와 메타: [idle_sheet.png](../../Clips/HU-01_idle/Export/IdleBreath/idle_sheet.png), [idle.json](../../Clips/HU-01_idle/Export/IdleBreath/idle.json).
- 반복 재생과 정수 확대: [Previews/IdleBreath](../../Previews/IdleBreath/index.html).
- 저장 파일 검사: [native_validation.json](../../QA/IdleBreath/native_validation.json).
- 전신·접점·5프레임 시각 검수: [visual_review.md](../../QA/IdleBreath/visual_review.md).
- 원본 보존: [source_hash.json](../../QA/IdleBreath/source_hash.json).

기존 v001 루트의 2프레임 PNG·미리보기·QA는 300→64 초기 축소 기록이다. 이번 사용자 키포즈와 새 애니메이션은 `IdleBreath/` 하위 출력·검수로 구분한다. 마스크·참조·작업 안내 레이어를 최종 캐릭터 export에 포함하지 않는다.


생성 idle 상의의 `(37,43)`, `(38,44)` 투명 구멍은 Pass02에서 기존 윤곽색1로 보완했다. 사용자1F 원본은 보존했고 모션과 나머지 픽셀은 같다. [후속 검수](../../QA/Animations/torso_pass02_visual_review.md).
