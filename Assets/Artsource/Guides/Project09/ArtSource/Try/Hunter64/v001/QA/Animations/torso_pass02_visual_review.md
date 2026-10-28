# 상의의 사각 빈칸과 가려진 면 복원 · Pass02

2026-10-11 · Hunter64/v001 · 기존 Aseprite 직접 픽셀 수정 · 8동작 39프레임

**지난 Pass01 검수는 실제 상의의 빈칸을 놓쳤다.** [당시 기록](torso_visual_review.md)은 오른쪽 옆면의 좁은 14개 기준점 복원과 해당 마스크의 미채움 0을 근거로 해결을 선언했다. 그러나 팔 분리로 빠진 반대쪽 상의 면과 옷 안쪽의 투명 픽셀까지 검사하지 않았다. 그 기록의 “문제 해결”, “새 틈 없음”은 전체 상의에 대한 판정으로 사용할 수 없다. 이 문서가 해당 누락을 다시 확인한 후속 검수다.

## 확인한 원인과 수정

팔에 가려진 상의까지 팔 레이어에 들어간 상태에서 팔이 이동하면 Body의 일부가 직각으로 잘려 빈칸이 드러났다. 남은 투명 픽셀은 색 수나 보간 문제가 아니었다. 실제 옷 면과 원래 비어 있어야 하는 공간을 구분해 수정했다.

- HU-02~08의 Body에서 기준 좌표 x25~28/y35~48의 가려진 면을 복원했다. 어깨에서 허리로 폭을 좁히고 기존 초록 기본색·그림자, 벨트색과 윤곽색을 사용했다. 각 동작의 몸통 이동·기울기·숙임에 맞춰 적용했다.
- 상의 안쪽의 기준 좌표 (37,43), (38,44)에 윤곽을 보완했다. focus F3/F4 및 shoot F1의 (37,34), walk F4의 (27,49)도 각 자세에서 남은 접점의 빈칸을 정리했다. 좌표는 64px 캔버스 좌상단 원점이다.
- `idle_breath.aseprite`에는 상체 레이어의 같은 두 윤곽 픽셀을 전5F에 추가했다. 합성 화면에서 차이가 드러나는 프레임은 F1/F5이며, 내려간 F2~4에서는 기존 겹침 아래에 가려진다. 사용자가 수정한 1F `idle.aseprite`의 파일 바이트는 보존했다.

머리·모자·깃털·머리카락·얼굴·눈·귀, 팔·손, 장비·다리·시연 소품의 픽셀과 움직임, 기존 팔레트·태그·duration은 유지했다. 새 이미지 생성, 블러, 안티앨리어싱, 디더링을 사용하지 않았다.

[walk 전후](walk_pass02_before_after.png)와 [focus 전후](focus_pass02_before_after.png)는 왼쪽 Pass02 이전, 오른쪽 이번 수정 후다. [편집 가능한 면 복원 기준](../../Reference/TorsoOcclusion/garment_planes_pass02.aseprite)에 비교를 보관했고, 각 클립의 `Masks/torso_pass02.aseprite`에 실제 포즈별 수정 범위를 기록했다. 마스크와 비교 레이어는 캐릭터 export에서 제외한다. 수정 전 파일은 [Pass02/Before](../../Source/TorsoOcclusion/Pass02/Before/)에 별도로 보존했으며 [보존 해시](torso_pass02_backup.json)로 구분한다.

## 전신·전 프레임 확대 확인

최신 밝은 배경 전신 contact의 신규7동작 34F와 idle 5F를 확인했다. 전체 그림을 함께 보면서 옷 면의 빈칸, 목·어깨·허리 접점과 팔이 빠져나간 자리를 살폈다. 머리와 장비의 형태를 옷으로 덮거나 팔·활 사이를 함께 채우지 않았는지도 확인했다.

| 동작 / 프레임 | 현재 전신 이미지 / 수정 마스크 | 확인한 옷 면과 판정 |
|---|---|---|
| idle / F1~5 | [전신](../../Previews/IdleBreath/contact_6x.png) · [마스크](../../Clips/HU-01_idle/Masks/torso_pass02.aseprite) | F1/F5의 허리 옆 투명 두 칸이 윤곽으로 닫힘. 하강·복귀 동작과 하체 접지 유지. |
| walk / F1~6 | [전신](../../Previews/Animations/walk_contact_6x_on_light.png) · [마스크](../../Clips/HU-02_walk/Masks/torso_pass02.aseprite) | 팔 아래 상의가 사각형으로 잘려 보이던 면 복원. F4의 허리·다리 접점 빈칸 보완. |
| focus / F1~4 | [전신](../../Previews/Animations/focus_contact_6x_on_light.png) · [마스크](../../Clips/HU-03_focus/Masks/torso_pass02.aseprite) | 팔을 들어도 상의 옆면 유지. F3/F4 목·어깨의 작은 투명 틈 보완. 활줄 안쪽과 팔 아래 외부 공간 유지. |
| shoot / F1~4 | [전신](../../Previews/Animations/shoot_contact_6x_on_light.png) · [마스크](../../Clips/HU-04_shoot/Masks/torso_pass02.aseprite) | F1 어깨 틈과 각 자세의 옷 면 누락 보완. 발사·반동·회복의 팔·활 움직임 유지. |
| collect / F1~4 | [전신](../../Previews/Animations/collect_contact_6x_on_light.png) · [마스크](../../Clips/HU-05_collect/Masks/torso_pass02.aseprite) | 숙인 몸통에도 복원면이 이어짐. 최저 자세 F3의 (26,47)은 장비·다리 사이 공간으로 유지. |
| carry_small / F1~6 | [전신](../../Previews/Animations/carry_small_contact_6x_on_light.png) · [마스크](../../Clips/HU-06_carry_small/Masks/torso_pass02.aseprite) | 팔을 앞에 둔 상태에서 드러난 상의 면이 허리까지 연결됨. 운반 보행 중 같은 면 유지. |
| drag_large / F1~6 | [전신](../../Previews/Animations/drag_large_contact_6x_on_light.png) · [마스크](../../Clips/HU-07_drag_large/Masks/torso_pass02.aseprite) | 기울어진 몸통의 옆면을 복원하고 기존 팔·밧줄·등의 활 공간 유지. |
| load / F1~4 | [전신](../../Previews/Animations/load_contact_6x_on_light.png) · [마스크](../../Clips/HU-08_load/Masks/torso_pass02.aseprite) | 들어 올림·앞으로 뻗음·복귀 중 옷 면의 누락 보완. 손과 상의 밖의 공간 유지. |

확인된 상의 내부 빈칸과 팔 분리로 잘린 옷 면은 보완됐다. 활줄 안쪽, 팔과 활 사이, 다리 사이의 투명 공간은 정상적인 외부 공간으로 남겼다. 알파 후보를 모두 옷색으로 채우는 방식으로 처리하지 않았다. [조준](focus_keypose.png), [수집](collect_keypose.png), [적재](load_keypose.png)의 최신 브라우저 확대 이미지에서도 이 구분을 확인할 수 있다.

## 파일 검사와 실제 표시·재생 확인

[Pass02 독립 검사](torso_pass02_validation.json)는 39F를 재개봉해 검사했고 `passed=true`, 확인된 상의 빈칸의 미채움 0을 기록했다. 신규7동작 Body의 변경은 974픽셀, 다른 부위 변경은 0이다. idle 상체에는 [수정 기록](idle_alpha_pass02_repair.json)에 있는 프레임당 2개, 총10개의 윤곽 픽셀이 추가됐다. 이 수치는 변경 범위 검증이며 전신 작화 품질의 점수가 아니다. 검사에 남은 고립 알파 후보는 위치와 실제 형태를 대조하여 장비·다리 사이 공간과 상의 구멍을 구분했다.

남긴 후보 (31,53)은 다리·부츠 사이, drag의 (22,53)과 collect F2의 (24,53)은 등 뒤 활·부츠 사이, shoot F1의 (41,32)와 F3의 (45,33)은 활·손·얼굴 사이 공간이다. collect F3의 (26,47)은 장비 사이에 해당한다. 독립 검수자도 walk F4의 (27,49)와 idle F1/F5의 두 윤곽 빈칸이 닫힌 것을 확인했다.

[신규34F native/export 검사](native_validation.json)와 [idle5F native/export 검사](../IdleBreath/native_validation.json)는 저장 파일·PNG·시트·GIF·팔레트·알파·태그·duration의 일치를 통과했다. idle 검사는 F1/F5가 사용자 키포즈와 두 개의 기록된 알파 수리만 다르다는 점, 정수 이동·발 고정·반복 경계와 원본 SHA-256 보존도 확인했다. 사용자 원본의 SHA-256은 `9736801160266D4EDF2C0E4444FC3D3D3827CDDFD5CC539CC3E042BAB382130C`로 동일하다.

Pass02 수정 이후 갱신된 [브라우저 검사](browser_validation.json)는 8동작39F 선택, 실제 64×64 표시와 512×512 Point 확대, 반복·단발·조준 마지막 프레임 유지, 소품 표시 전환을 통과했다. [실제64px 이미지](actual_size.png)와 최신 확대 keypose를 확인했고, 루트 작업자는 브라우저의 walk F2에서도 상의 면을 직접 확인했다. 링크·스크립트 오류는 0이며 모바일390px에서 가로 넘침은 없었다. [재생기](../../Previews/Animations/index.html)에서 현재 저장 결과를 확인할 수 있다.

판정 범위는 **상의의 확인된 투명 구멍과 팔에 가려졌던 옷 면의 복원**이다. 기존 v001의 팔·손 관절 세부 작화, 시연 동물·수레의 완성도와 실제 Unity 연결은 이번 검수의 완료 범위에 포함하지 않는다. 전체 캐릭터의 모든 경계 정리나 최종 작화 완성을 선언하지 않는다.
