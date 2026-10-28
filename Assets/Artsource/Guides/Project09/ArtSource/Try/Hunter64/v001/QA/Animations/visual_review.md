## 현재 focus·shoot·collect 작화검수 · SlingshotActions/p001

2026-10-13. 기준은 [동결 idle_breath F1](../../Source/SlingshotActions/p001/idle_breath.aseprite)이다. 세 클립은 기존 Indexed 픽셀·17칸 팔레트로 직접 구성했고 이미지 재생성·블러·안티앨리어싱·디더링을 사용하지 않았다. 아래→위 `Bow&Quiver` / `Slingshot&bag` / `LowerBody` / `UpperBody_Arms_Equipment` / `Head_Hat_Hair_Face`의 5개 레이어를 유지하며 `Bow&Quiver`만 숨겼다.

| 편집 정본 | 타이밍·동작 | 전 프레임 64px / 전후 비교 |
|---|---|---|
| [focus](../../Clips/HU-03_focus/focus.aseprite) | 4F × 150ms = 600ms. 새총 올리기 → 고무줄 당기기 → 볼 옆 조준 유지 | [64px](../SlingshotActions/p001/SourceReview/focus_native_contact_1x.png) · [전후](../SlingshotActions/p001/focus_before_after_8x.png) |
| [shoot](../../Clips/HU-04_shoot/shoot.aseprite) | 4F × 100ms = 400ms. F1 놓기 → 반동 → 내리기 → 기준 복귀 | [64px](../SlingshotActions/p001/SourceReview/shoot_native_contact_1x.png) · [전후](../SlingshotActions/p001/shoot_before_after_8x.png) |
| [collect](../../Clips/HU-05_collect/collect.aseprite) | 4F × 150ms = 600ms. 숙이기 → F3 반대손 수집 → 자루 입구 손 회복 | [64px](../SlingshotActions/p001/SourceReview/collect_native_contact_1x.png) · [전후](../SlingshotActions/p001/collect_before_after_8x.png) |

**독립 정적 작화검수 통과:** 전 12프레임을 native에서 다시 열고 전체 64px·8배로 확인했다. [focus·shoot 상세](../SlingshotActions/p001/SourceReview/focus_shoot_review.md) · [collect 상세](../SlingshotActions/p001/SourceReview/collect_review.md) · [정적 native 대응 검사](../SlingshotActions/p001/SourceReview/native_static_review.json).

| 부위 | 판정과 확인 범위 |
|---|---|
| 머리 전체·눈 | 원형 유지. focus 고정, shoot F2 -1x, collect F2 (1,3)/F3 (3,6) 이동. 머리 이동 비교 불일치 0이며 조준 손이 눈을 덮지 않음. |
| 목·상의·팔 가림 면·허리 | 팔 뒤 셔츠 면과 양쪽 어깨·허리 접점이 연속함. 네모 투명 구멍 없음. |
| 양팔·새총·고무줄 | 손과 손잡이가 연결되고 고무줄 당김·놓기 구분됨. collect의 앞손은 새총 보유, 반대손은 (34,54) 수집 후 자루 입구 (25,39)로 복귀함. |
| 자루·무릎·발 | 자루 최저점은 F3 y56으로 바닥을 넘지 않음. 굽힌 무릎과 옷자락은 연결됨. 기존 불투명 신발 픽셀은 전12프레임 변경 0. collect F3의 기존 빈 하체 y51~53에 관절·옷 연결 9점이 추가됐으므로 하체 전체 행 보존과 구분함. |

**통합 수치검수·브라우저 재생 검수: 통과.** [이번 작업 안내](../SlingshotActions/p001/README.md), [통합 native 검사](../SlingshotActions/p001/native_validation.json), [재생기](../../Previews/Animations/index.html)의 별도 결과를 따른다. 작화 판정과 별도로 전체8동작·42프레임 재생기 검사, 새총 그립과 발사·수집 이벤트 표시 검사를 통과했다.

[focus 보존본](../../Clips/HU-03_focus/Source/SlingshotActions/p001/focus_before.aseprite) · [shoot 보존본](../../Clips/HU-04_shoot/Source/SlingshotActions/p001/shoot_before.aseprite) · [collect 보존본](../../Clips/HU-05_collect/Source/SlingshotActions/p001/collect_before.aseprite) · [입력 해시](../SlingshotActions/p001/input_hashes.json). idle·walk·carry_small·drag_large·load와 Unity Assets는 이번 작업에서 변경하지 않았다.

## 이전 제작 이력

아래의 활·화살 기반 9레이어와 최초 동작 검수는 제작 이력으로 보존한다. 이번 세 클립의 최신 장비·레이어·포즈 판정은 위 SlingshotActions/p001이 기준이다.

> 별도 무릎·자락 비교 포즈 F10·11의 최신 검수는 [WalkKneeDrape/p001](../WalkKneeDrape/p001/README.md)이다. 기존 walk 태그1~9의 p002 검수는 그대로 유지된다.

> 현재 walk의 팔·높낮이 최신 검수는 [WalkKeyposes/p002](../WalkKeyposes/p002/README.md)다. 아래 p001의 상체 고정 판정은 이전 단계의 기록이다.

> 현재 walk는 9프레임·4레이어 키포즈 작업본입니다. 최신 기록: [WalkKeyposes/p001](../WalkKeyposes/p001/README.md). F2·4·6·8은 의도적으로 비워뒀으며 아래 기존 6프레임 walk 검수는 이전 작업의 기록입니다.

> 최신 상의 판정은 [Pass02 검수](torso_pass02_visual_review.md)입니다. 아래 이전 검수는 좁은14점 복원에 한정되어 실제 투명 구멍·왼쪽 누락면을 놓친 기록이며 현재 완료 판정으로 사용하지 않습니다.

# Hunter64 v001 · HU-02~08 시각 검수

2026-10-11. 사용자 수정 1프레임 디자인을 기준으로 만든 7동작·34프레임의 전신 포즈를 확인했다. [제작 명세](../../Prompts/Animations/motion_spec.md), [기준 디자인](../../Reference/Animations/user_keypose.png), [재생기](../../Previews/Animations/index.html).

## 수치 검사

[저장 파일 독립 재개봉 검사](native_validation.json) 통과. 64×64 Indexed, 17칸 팔레트 동일, 레이어 9개, 태그·프레임·duration·마스크 범위 일치. 34개 프레임 모두 캐릭터 PNG·시트·GIF 및 소품 합성 PNG·GIF와 native 렌더의 픽셀 불일치 0, 반투명 픽셀 0. 각 동작의 모든 캐릭터 프레임은 서로 다른 포즈다.

전 프레임에서 527픽셀의 머리 레이어는 입력 머리 전체를 지정한 정수 offset만큼 이동한 결과와 일치한다. 따라서 모자·깃털·머리카락·귀·눈·얼굴 내부를 동작마다 새로 그리지 않았다. 발 바닥은 y56이며 들리는 반대 발은 1px 올라간다. 수치 일치는 작화 완성도와 구분한다.

## 전 프레임 시각 판정

각 행의 contact는 전 프레임의 최근접 6배 합성본이다. 캐릭터 단독 64px는 각 클립 Frames에, 단독 8배 이미지는 Previews/Animations의 `<동작>_01_8x.png`부터 마지막 프레임까지 저장했다. 소품은 팔의 접점 확인을 위한 시험용 그림이다.

최종 분할 보정 뒤에는 밝은 배경의 캐릭터 단독 전체 프레임도 확인했다: [걷기](../../Previews/Animations/walk_contact_6x_on_light.png), [조준](../../Previews/Animations/focus_contact_6x_on_light.png), [발사](../../Previews/Animations/shoot_contact_6x_on_light.png), [수집](../../Previews/Animations/collect_contact_6x_on_light.png), [운반](../../Previews/Animations/carry_small_contact_6x_on_light.png), [견인](../../Previews/Animations/drag_large_contact_6x_on_light.png), [적재](../../Previews/Animations/load_contact_6x_on_light.png). 검정 윤곽과 빈 공간을 확인하기 위한 QA 이미지이며 캐릭터 export에는 배경을 추가하지 않았다.

| 동작·전체 이미지 | 프레임별 확인 | 전신·접점 판정 |
|---|---|---|
| [walk 6F](../../Previews/Animations/walk_contact_6x.png) · [8배 기준](../../Previews/Animations/walk_01_8x.png) | F1/F4 양발 앞뒤 교대, F2/F5 체중 이동과 상체 하강, F3/F6 반대 발 들림. 팔과 활은 다리와 반대로 작게 이동. | 모자·귀·얼굴·버클 유지. 허벅지–정강이 연결을 기존 초록 ramp로 보완. 한 발 접지 유지. 6→1은 다음 보행 단계로 이어짐. 보폭은 2px로 작고 게임 이동속도 대응은 미검수. |
| [focus 4F](../../Previews/Animations/focus_contact_6x.png) · [최종 8배](../../Previews/Animations/focus_04_8x.png) | F1 아래쪽에서 활 준비, F2 활을 얼굴 아래 높이로 올림, F3/F4 시위 손이 안쪽으로 당겨짐. | 양팔의 어깨–팔꿈치–손 연결과 손–활 접점 유지. 손과 전완이 화살 뒤쪽 일부를 의도적으로 가린다. 화살 앞쪽은 활부터 끝까지 이어짐. 얼굴 눈은 보이고 모자·깃털 형태 유지. 팔은 포즈 확인을 위한 단순한 3색 형태이며 소매 디테일은 후속 작화 대상. |
| [shoot 4F](../../Previews/Animations/shoot_contact_6x.png) · [반동 8배](../../Previews/Animations/shoot_02_8x.png) | F1 화살 제거·시위 복귀, F2 머리 1px 뒤로 반동·손 뒤로, F3 활 하강, F4 기준 자세. | 발 고정, 머리 디테일 유지. F4가 원래 디자인으로 돌아와 idle로 연결 가능. 조준→발사의 전완 방향 변화는 빠른 놓기 동작이며 최종 손 관절 작화는 다듬을 수 있음. 화살 비행은 별도 자산·게임 이벤트 영역. |
| [collect 4F](../../Previews/Animations/collect_contact_6x.png) · [최저 8배](../../Previews/Animations/collect_03_8x.png) | F1 기준, F2 머리 +3x/+3y 숙이기, F3 +5x/+7y 낮추고 손 접촉, F4 서서 두 팔로 받침. | 머리·얼굴 인상 유지, 몸통은 정수 최근접으로 짧아지고 무릎/부츠는 직접 연결. 활은 등에 둬 팔 공간 확보. 최저 자세에서도 팔·손·부츠가 분리되지 않음. F3→F4는 4F 규격의 빠른 일어서기이며 중간 포즈 추가 가능. |
| [carry_small 6F](../../Previews/Animations/carry_small_contact_6x.png) · [단독 8배](../../Previews/Animations/carry_small_04_8x.png) | 짐 아래 양손 위치를 몸통에 상대적으로 고정. walk와 같은 양발 교대에 0/1px 상체 움직임. | 머리·벨트·등 장비 유지. 소품 끄면 두 손이 열린 받침 형태로 남음. 활은 다리 뒤에 그려 부츠 위로 관통하지 않음. 토끼 시연 그림은 단순 형태로 최종 동물 작화 판정에 포함하지 않음. |
| [drag_large 6F](../../Previews/Animations/drag_large_contact_6x.png) · [단독 8배](../../Previews/Animations/drag_large_04_8x.png) | 머리 앞으로 +3x, 몸통 낮춤, 뒤손 x18에서 바닥 가까운 줄 접점 x6/y53 유지. 보행 교대에 손 높이 0/1px 움직임. | 어깨–팔–줄 연결이 끊기지 않고 바닥 접점 유지. 활은 뒤쪽 수납. 실루엣은 walk보다 앞으로 낮음. 큰 사냥감은 직사각형 시험 짐이므로 실제 동물의 크기·당김 각도는 아직 판단하지 않음. |
| [load 4F](../../Previews/Animations/load_contact_6x.png) · [들기 8배](../../Previews/Animations/load_02_8x.png) | F1 운반, F2 들어 올림, F3 앞으로 팔 뻗어 놓기, F4 짐이 수레에 남고 팔·활 기준 자세 복귀. | 손–짐–수레 높이의 진행을 합성본으로 확인. 팔꿈치–손 연결 유지. F3에는 겹치는 두 팔이 한 덩어리로 읽히므로 최종 작화에서 전후 팔 구분을 보완할 수 있음. 수레 오른쪽은 64px 끝에 가깝고 실제 수레 배치와의 검수는 별도. |

전체 머리(깃털부터 얼굴)는 기존 디자인 유지, 몸통·어깨끈·버클은 자세별 위치 변화, 양팔·활은 작업에 맞춘 직접 포즈, 다리·부츠는 접지와 보행/무릎 구부림을 확인했다. 확대상 계단은 정수 픽셀이며 블러·AA·디더링은 없다. 이전 300px 그림의 경계 정리를 다시 수행한 결과로 보고하지 않는다.

정적 시각 판정: **34프레임의 동작 진행·머리 디자인 보존·팔/다리/장비 접점 확인 완료. 포즈 시험본으로 사용 가능. 최종 소매·손 관절·전후 팔의 세부 작화와 실제 동물/수레 연결은 남은 범위.**

## 재생·표시 크기

[브라우저 검사](browser_validation.json) 통과. 전체 8동작·39프레임을 각각 선택해 64px 원본 로드와 고정 기준 이미지를 확인했다. idle/walk/carry_small/drag_large의 반복 복귀, focus의 마지막 유지, shoot/collect/load의 단발 종료를 확인했다. 적재 완료 표시, 100% 64px와 800% 512px의 Point 표시, 소품 표시 전환, 숲 배경, 390px 화면에서 가로 넘침 없음, 납품 링크56개 응답 정상, 브라우저 오류0건을 확인했다.

[데스크톱](player_desktop.png), [모바일](player_mobile.png), [실제 크기](actual_size.png), [조준](focus_keypose.png), [수집](collect_keypose.png), [운반](carry_small_keypose.png), [견인](drag_large_keypose.png), [적재](load_keypose.png), [보행](walk_keypose.png), [발사](shoot_keypose.png)의 화면을 확인했다. 캐릭터와 기준 디자인이 겹치지 않고 머리·몸통·팔·발의 진행이 읽힌다. 100%에서는 모자·얼굴·의상·활의 구분을 유지한다. 보행/운반의 작은 보폭과 수집/적재의 빠른 자세 변화는 제작 목록의 시간·프레임 수를 적용한 시험 동작이며 게임 속도와의 최종 비교는 별도다. Unity 적용·실제 게임 화면은 이번 검수 범위에 포함하지 않았다.


## 활과 부츠 분할 보정

확대 배경 비교에서 조준·운반 시 부츠 오른쪽에 남아 있던 검정 픽셀2개를 발견했다. 원본 x40/y51–52는 활 아래 끝의 윤곽이므로 다리에서 활 레이어로 옮겼다. 부츠 x40/y55–56과 얼굴·의상은 유지했다. 다시 native를 저장해 34개 프레임·PNG·시트·GIF·마스크를 재검사하고, 활 이동 뒤의 불필요한 잔여 픽셀이 사라진 것을 화면에서 확인했다. 기준 자세를 재구성하는 shoot F4는 여전히 원본과 같다.


## 상의 가림 면 후속 수정

위 최초 포즈 검수 이후 사용자가 오른팔 이동 시 상의가 깨지는 문제를 지적했다. 팔에 포함되어 이동하던 상의 옆면에 별도의 Body 가림 면이 없는 것이 원인이었다. 7동작31프레임의 Body만 직접 복원하고 나머지 모든 파트 픽셀과3개 기준 자세는 유지했다. 전신·전 프레임·실제64px·재생과 native/export 검사를 갱신했다. [수정 전후 및 상세 검수](torso_visual_review.md), [픽셀 보존 검사](torso_validation.json), [입력 보존본](torso_backup.json), [수정 파일 해시](torso_delivery.json)를 최신 상의 판정으로 사용한다.
