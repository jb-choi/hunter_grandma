## 현재 focus·shoot·collect 제작 명세 · SlingshotActions/p001

2026-10-13. 이번 세 클립의 입력은 [동결 idle_breath.aseprite](../../Source/SlingshotActions/p001/idle_breath.aseprite)의 F1이다. 같은 v001에서 현재 새총·자루 디자인을 사용한다. 기존 Indexed 픽셀·17칸 팔레트를 직접 수정하며 이미지 재생성·블러·안티앨리어싱·디더링은 금지한다. 팔 뒤에 가려진 양쪽 상의 면을 복원한 다음 팔을 움직인다. 머리는 모자·깃털·머리카락·얼굴·귀·눈 전체를 정수 이동한다.

최종 파일의 레이어 순서는 아래→위 `Bow&Quiver`, `Slingshot&bag`, `LowerBody`, `UpperBody_Arms_Equipment`, `Head_Hat_Hair_Face`인 5개 그대로다. `Bow&Quiver`만 숨기고 나머지 4개는 표시한다. 새총·자루와 양팔의 작업용 분할은 최종 원래 레이어에 다시 합친다. 별도 지시·부위 마스크는 캐릭터 PNG에 넣지 않는다.

| 편집 정본 | 시간·포즈 | 종료·접점·이벤트 | 전체 64px / 전후 |
|---|---|---|---|
| [HU-03 focus](../../Clips/HU-03_focus/focus.aseprite) | 4F × 150ms = 600ms. F1 기준 → F2 새총 올림 → F3 고무줄 당김 → F4 볼 옆 조준 | 마지막 유지. F4 손잡이 (50,36), 당김 손 (40,31), 주머니 (41,30) | [64px](../../QA/SlingshotActions/p001/SourceReview/focus_native_contact_1x.png) · [전후](../../QA/SlingshotActions/p001/focus_before_after_8x.png) |
| [HU-04 shoot](../../Clips/HU-04_shoot/shoot.aseprite) | 4F × 100ms = 400ms. F1 놓기 → F2 반동 → F3 팔 내림 → F4 기준 복귀 | 단발. F1 시작 `release`, 위치 (54,27). F2 머리·몸 -1x. F4 현재 입력 F1과 동일 | [64px](../../QA/SlingshotActions/p001/SourceReview/shoot_native_contact_1x.png) · [전후](../../QA/SlingshotActions/p001/shoot_before_after_8x.png) |
| [HU-05 collect](../../Clips/HU-05_collect/collect.aseprite) | 4F × 150ms = 600ms. F1 기준 → F2 몸 숙임 → F3 무릎 굽혀 수집 → F4 일어서 자루 입구로 손 | 단발. F3 시작 `pickup`, 반대손 (34,54). 앞손 새총 보유 유지. F4 자루 입구 접점 (25,39) | [64px](../../QA/SlingshotActions/p001/SourceReview/collect_native_contact_1x.png) · [전후](../../QA/SlingshotActions/p001/collect_before_after_8x.png) |

좌표는 PNG 좌상단 원점, 0부터 시작하며 +y가 아래다. collect 머리 이동은 F2 (1,3), F3 (3,6), 자루 이동은 F2 (1,2), F3 (3,3)이다. 접지 y56과 기존 불투명 신발 픽셀을 유지한다. F3의 기존 빈 하체 영역 y51~53에 관절·옷 연결 9픽셀을 추가했으므로 전체 하체 행 동일 규칙을 적용하지 않는다. 수집 대상은 캐릭터 sprite에 그리지 않았으며 이벤트는 제작 메타로, Unity 런타임 연결은 이번 범위에 포함하지 않는다.

**독립 정적 작화검수 통과:** [focus·shoot](../../QA/SlingshotActions/p001/SourceReview/focus_shoot_review.md), [collect](../../QA/SlingshotActions/p001/SourceReview/collect_review.md). 전12프레임의 머리·상의·팔·장비·무릎·발을 64px/8배로 확인했다. **통합 수치검수·브라우저 재생 검수도 통과**이며 [이번 QA 안내](../../QA/SlingshotActions/p001/README.md), [native 검사](../../QA/SlingshotActions/p001/native_validation.json)의 실제 결과를 별도로 따른다.

[focus 이전 native](../../Clips/HU-03_focus/Source/SlingshotActions/p001/focus_before.aseprite) · [shoot 이전 native](../../Clips/HU-04_shoot/Source/SlingshotActions/p001/shoot_before.aseprite) · [collect 이전 native](../../Clips/HU-05_collect/Source/SlingshotActions/p001/collect_before.aseprite) · [입력 해시](../../QA/SlingshotActions/p001/input_hashes.json). idle·walk·carry_small·drag_large·load 및 Unity Assets는 이번 작업에서 변경하지 않았다.

## 이전 제작 명세 이력

아래의 활·화살과 9레이어 제작 명세는 이전 동작 단계의 기록이다. 현재 focus·shoot·collect에는 위 SlingshotActions/p001 명세를 적용한다.

# Hunter64 v001 · 사냥꾼 동작 제작 명세

2026-10-11. 사용자가 수정한 [64px 1프레임 디자인](../../Clips/HU-01_idle/idle.aseprite)을 기준으로 HU-01 이후의 사냥꾼 동작을 같은 v001에 추가한다. 기존 [사냥꾼 애니메이션 목록](../../../../../../Docs/AI/Art/02_Production/HU-Animations1254/Specification.md)의 ID·프레임 수·시간·이벤트를 64px 픽셀 동작에 적용했다.

## 입력과 제작 방식

- 시작 원본은 각 클립의 `Source/hunter_user_keypose.aseprite`에 그대로 보존한다. [원본 해시](../../QA/Animations/source_hash.json)로 입력과 보관본의 일치를 확인한다.
- 64×64 Indexed, 팔레트 17칸(투명 0 + 기존 불투명 16색)을 유지한다. 재생성·블러·안티앨리어싱·디더링을 사용하지 않는다.
- 머리 전체(모자·깃털·머리카락·얼굴·귀)는 기존 527픽셀을 정수 이동한다. 팔·활·다리 접점은 기존 팔레트로 직접 구성한다.
- `Head / Body / RearArm / FrontArm / RearLeg / FrontLeg / Bow / Quiver / DemoProps` 9개 레이어로 저장한다. 수집·운반·견인·적재의 활은 몸통·다리 뒤에 둔다.
- 실제 동물과 수레는 별도 자산이다. 접점 확인용 토끼·견인 짐·수레를 `DemoProps`에 그렸으며 편집 파일에서는 기본 숨김, 캐릭터 PNG·시트·GIF에는 제외한다. 재생기에서 합성본을 켜고 끌 수 있다.

## 동작과 타이밍

| ID / 편집 파일 | 프레임·길이 | 포즈 진행 | 종료·이벤트 |
|---|---|---|---|
| [HU-01 / idle_breath](../../Clips/HU-01_idle/idle_breath.aseprite) | 5F · 1200ms | 기준 → 상체·머리 +1y → 머리 추가 +1y → 머리 상승 → 상체 상승 | 반복. 하체·발 고정 |
| [HU-02 / walk](../../Clips/HU-02_walk/walk.aseprite) | 6F × 120ms = 720ms | 양발의 앞뒤 이동·들림 교대, 몸통 0/1px 호흡, 팔·활 반대 움직임 | 반복 |
| [HU-03 / focus](../../Clips/HU-03_focus/focus.aseprite) | 4F × 150ms = 600ms | 활 들기 → 손 정렬 → 시위 당기기 → 조준 자세 | 마지막 자세 유지 |
| [HU-04 / shoot](../../Clips/HU-04_shoot/shoot.aseprite) | 4F × 100ms = 400ms | 놓기 → 반동 → 활 내리기 → 기준 자세 | 단발. F1 시작 `release` |
| [HU-05 / collect](../../Clips/HU-05_collect/collect.aseprite) | 4F × 150ms = 600ms | 기준 → 몸 숙이기 → 무릎 굽혀 손 내리기 → 들어 올리기 | 단발. F3 시작 `pickup` |
| [HU-06 / carry_small](../../Clips/HU-06_carry_small/carry_small.aseprite) | 6F × 140ms = 840ms | 양팔로 짐 받치기 + 보행 교대 | 반복 |
| [HU-07 / drag_large](../../Clips/HU-07_drag_large/drag_large.aseprite) | 6F × 160ms = 960ms | 몸 앞으로 낮추기 + 뒤손 견인줄 유지 + 보행 교대 | 반복 |
| [HU-08 / load](../../Clips/HU-08_load/load.aseprite) | 4F × 250ms = 1000ms | 운반 자세 → 들어 올림 → 수레에 내림 → 손 놓고 복귀 | 단발. F3 시작 `detach`, F4 종료 `load_complete` |

원점은 캔버스 좌상단, x 오른쪽·y 아래, 0부터 시작하는 픽셀 좌표다. 모든 포즈에서 접지된 발 바닥은 y56이다. 운반 손·활 손잡이·견인 접점은 각 클립 `Export/<동작>.json`의 프레임별 `anchors`에 기록했다. `events`는 연결을 위한 제작 메타데이터이며 Unity의 런타임 이벤트를 새로 적용하지 않았다.

## 저장·검수

각 클립에 편집 원본, Source, Frames, Export, Masks와 README를 함께 둔다. 부위 마스크는 모든 포즈의 실제 레이어를 바탕으로 작성하며 export에는 포함하지 않는다. `Masks/parts.json`은 레이어·마스크 라벨 순서를 설명한다.

[전체 재생기](../../Previews/Animations/index.html)는 8동작·39프레임을 선택하며 Point 100/400/800%, 캐릭터 단독/소품 합성, 반복/단발/조준 유지, 프레임 이동과 속도 조절을 지원한다. GIF는 프레임·속도 확인용 반복 파일이다. 단발·hold 동작의 실제 종료 방식은 재생기와 메타데이터를 기준으로 확인한다.

[native 검수](../../QA/Animations/native_validation.json), [시각 검수](../../QA/Animations/visual_review.md), [재생기 검사](../../QA/Animations/browser_validation.json)를 구분해 보관한다. 이번 결과는 사용자 디자인에 맞춘 v001 동작 시험본이다. 게임 장면의 이동속도·동물 크기·수레 높이와 연결한 최종 작화 판정은 이번 파일 검사와 별도다.


## 팔 가림 면

flat 키포즈에서 팔에 가려진 몸통 면은 팔을 움직이기 전에 별도로 Body에 그린다. 범위는 기존 어깨·옆구리·벨트 윤곽에 맞추며, 앞팔 픽셀을 단순히 잘라 몸통에서 빼기만 하지 않는다. 이번 v001은 [native 수정 기준](../../QA/Animations/torso_underpaint.lua)의 x37~39/y36~43을 기존 초록2/3·벨트6·윤곽1로 복원했다. 몸통의 offset·scale·shear를 따라 이동하고 기준 자세를 재구성하는 shootF4/collectF1/loadF4는 원래 Body를 유지한다. [편집 기준 예시](../../Reference/TorsoOcclusion/torso_underpaint.aseprite) · [검수](../../QA/Animations/torso_visual_review.md).


## 숨은 상의 면·alpha 검사 · Pass02

파트를 잘라 움직이기 전에 팔에 가려진 양쪽 상의 면을 완성한다. [Pass02 면 기준](../../QA/Animations/torso_pass02_spec.lua)은 어깨에서 넓고 허리에서 좁아지는 왼쪽 면과 원본 투명구멍2점을 명시한다. 팔·몸통 합성의 네 방향 enclosed alpha를 전체 프레임에서 검사하고, 옷 안의 구멍과 활줄 내부·팔과 활 사이·다리 사이 공간을 의미로 구분한다. shootF4/collectF1/loadF4도 원본 구멍2점은 보완하므로 이전의 기준자세 완전동일 규칙을 대체한다. [최신 검수](../../QA/Animations/torso_pass02_visual_review.md).
