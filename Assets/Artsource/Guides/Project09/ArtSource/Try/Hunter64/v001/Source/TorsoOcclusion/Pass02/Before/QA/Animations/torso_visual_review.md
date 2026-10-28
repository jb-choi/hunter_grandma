# 오른팔 가림 아래 상의 복원

2026-10-11 · Hunter64/v001 · HU-02~08 기존 Aseprite 직접 수정

## 원인과 수정

원본 키포즈는 팔에 가려진 몸통 면을 따로 그리지 않은 한 장의 그림이다. 이를 `x>=38,y34..44`로 팔과 몸통을 분리하면서 상의 옆면의 일부까지 FrontArm에 들어갔다. 팔이 올라가거나 앞쪽으로 이동하면 Body의 오른쪽이 x37에서 수직으로 잘리고 벨트 끝에 투명한 홈이 드러났다. 이미지 보간 문제가 아니라 가려진 면이 없는 문제였다.

기존 Body cel에 어깨 외곽, 초록 상의 옆면과 좁아지는 벨트 끝을 직접 그려 복원했다. 기준 좌표는 x37~39/y36~43이며 각 자세의 몸통 이동·굽힘·기울기를 적용했다. 상의는 기존 기본 초록3/그림자2, 벨트6, 윤곽1을 사용했다. 팔이나 활 공간까지 폭을 넓히지 않았다. 이미지 재생성·AA·블러·디더링·신규 색은 없다.

[전후 비교](torso_focus_before_after.png)는 왼쪽 수정 전, 오른쪽 수정 후의 조준 F4다. [편집 가능한 기준 예시](../../Reference/TorsoOcclusion/torso_underpaint.aseprite)는 F1 Body 전/F2 Body 후/F3 조준 전/F4 조준 후로 구성한다. 각 클립 `Masks/torso_repair.aseprite`에 실제 포즈별 수정 범위를 보관했다. 이 마스크는 캐릭터 PNG에 포함하지 않는다.

## 전체 동작·프레임 검수

| 동작 / 전후·현재 이미지 | 범위 | 상의·허리 판정 |
|---|---|---|
| walk · [전](../../Source/TorsoOcclusion/Before/Previews/Animations/walk_contact_6x_on_light.png) / [후](../../Previews/Animations/walk_contact_6x_on_light.png) | F1~6 | 팔의 작은 상하·앞뒤 이동에도 옆구리와 벨트 끝이 연결됨. 몸통 0/1px 움직임을 따라감. |
| focus · [전](../../Source/TorsoOcclusion/Before/Previews/Animations/focus_contact_6x_on_light.png) / [후](../../Previews/Animations/focus_contact_6x_on_light.png) | F1~4 | 팔을 올리고 시위를 당긴 모든 자세에서 수직으로 잘린 옆면과 허리 홈이 사라짐. |
| shoot · [전](../../Source/TorsoOcclusion/Before/Previews/Animations/shoot_contact_6x_on_light.png) / [후](../../Previews/Animations/shoot_contact_6x_on_light.png) | F1~3, F4 보존 | 발사·반동·회복 중 상의 외곽 연결. F4는 원래 기준 자세 그대로. |
| collect · [전](../../Source/TorsoOcclusion/Before/Previews/Animations/collect_contact_6x_on_light.png) / [후](../../Previews/Animations/collect_contact_6x_on_light.png) | F2~4, F1 보존 | 숙임·최저 자세에서 보완면이 몸통의 압축·기울기를 따라감. 운반 자세 허리 끝 연결. |
| carry_small · [전](../../Source/TorsoOcclusion/Before/Previews/Animations/carry_small_contact_6x_on_light.png) / [후](../../Previews/Animations/carry_small_contact_6x_on_light.png) | F1~6 | 두 팔을 앞에 둔 상태에서 드러난 상의 옆면이 채워지고 보행 내내 동일 형태 유지. |
| drag_large · [전](../../Source/TorsoOcclusion/Before/Previews/Animations/drag_large_contact_6x_on_light.png) / [후](../../Previews/Animations/drag_large_contact_6x_on_light.png) | F1~6 | 몸통의 기울기를 따르는 옆면과 허리 외곽 유지. 새 돌출·틈 없음. |
| load · [전](../../Source/TorsoOcclusion/Before/Previews/Animations/load_contact_6x_on_light.png) / [후](../../Previews/Animations/load_contact_6x_on_light.png) | F1~3, F4 보존 | 팔을 들거나 앞으로 뻗을 때 상의가 끊기지 않음. F4는 원래 기준 자세 그대로. |

루트가 전34F의 밝은 배경 전신 이미지를 확인했고, 독립 검수자는 최신 저장 native 7개를 직접 렌더해 전34F의 상의·허리 경계를 재확인했다. 총31개 움직이는 팔 프레임의 Body만 수정했다. 나머지3개 기준 포즈의 합성 결과는 기존과 동일하다.

머리·모자·깃털·머리카락·얼굴·눈·귀, 양팔·손, 다리·부츠, 활·화살·화살통, 시연 소품은 수정 전 native와 픽셀이 동일하다. 어깨끈과 버클도 수정 범위 밖이며 유지했다. 전체 실루엣에서 추가 돌출이나 장비 접점 분리는 발견하지 못했다. 이번 판정은 상의의 가림 면 복원이며 기존 v001의 팔·손 관절 세부 작화와 동물·수레 연결을 완성한 것으로 보고하지 않는다.

## 파일·재생 확인

[수정 범위 독립 검사](torso_validation.json)는 보존본 대비 Body 변경417픽셀, 다른 레이어 변경0픽셀, 수정 범위 밖 변경0, 복원 마스크의 미채움0, 기준 포즈 합성 차이0을 확인했다. [저장 native 및 export 검사](native_validation.json)는 PNG·시트·GIF·마스크·팔레트·알파·duration·태그·머리·발 기준을 다시 통과했다. [재생기 검사](browser_validation.json)는 전체8동작의39F 선택·Point 실제 크기·반복/단발/조준 유지와 소품 전환을 다시 확인했다.

[현재 재생기](../../Previews/Animations/index.html), [실제64px](actual_size.png), [조준 확대](focus_keypose.png), [수집 확대](collect_keypose.png), [적재 확대](load_keypose.png)에서 상의·허리의 형태를 확인했다. 사용자 idle.aseprite와 idle_breath.aseprite는 이번 수정에서 변경하지 않았다. 원본 및 보존본 해시는 [torso_backup.json](torso_backup.json)에, 새 파일 해시는 [torso_delivery.json](torso_delivery.json)에 기록한다.

판정: **오른팔 이동 시 상의 옆면·허리선이 깨지는 문제 해결. 31F Body 수정 / 팔·포즈·캐릭터 인상 유지.**
