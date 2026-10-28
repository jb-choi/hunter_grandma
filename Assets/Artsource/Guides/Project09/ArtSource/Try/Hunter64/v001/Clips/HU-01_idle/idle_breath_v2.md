# HU-01 · idle_breath_v2 · 다리 디자인 단순화

2026-10-14 · Hunter64/v001 · 별도 시험본

편집 파일은 [idle_breath_v2.aseprite](idle_breath_v2.aseprite)다. 사용자가 지정한 [idle_breath.aseprite](idle_breath.aseprite)의 전 5프레임을 기준으로 다리와 부츠를 단순화했다. 기존 호흡 파일은 그대로 보존한다.

## 변경한 디자인

`LowerBody`의 y49~56만 기존 Indexed 픽셀에서 직접 수정했다. 좌표는 64×64 캔버스 좌상단 기준, 0부터 시작하며 양 끝을 포함한다.

- y49~50 바지는 작은 녹색 명암 조각을 기본 녹색 index2로 합쳤다. 윤곽색 index1은 유지한다.
- y51~56 부츠는 금색 발끝 장식과 가로 띠를 제거하고, 갈색 index6·한쪽 세로 그림자 index5·윤곽 index1로 정리했다.
- 부츠의 짧은 폭 변화를 단순한 발목과 발끝 윤곽으로 바꿨다. 두 발 사이의 틈과 밑창 y56을 유지했다.

레이어당 48개 캔버스 좌표가 각 프레임에서 바뀌었다. 하체의 불투명 픽셀 수는 191→192다. 픽셀 수 감소를 품질 기준으로 삼지 않고, 색 면과 윤곽을 단순하게 정리했다.

옷자락 y43~48은 그대로다. 머리·얼굴·상의·팔·손·새총·가방·숨긴 옛 활의 원래 셀 이미지와 좌표는 전부 보존했다. 다른 동작으로 이 디자인을 전파하지 않았다.

## 레이어와 호흡

64×64 Indexed, 기존 17칸 팔레트와 투명 index0을 유지했다. 새 색·블러·안티앨리어싱·디더링·이미지 재생성을 사용하지 않았다.

| 아래→위 레이어 | 기본 표시 | 역할 |
| --- | --- | --- |
| Bow&Quiver | 숨김 | 이전 장비 보존; 캐릭터 출력에서 제외 |
| Slingshot&bag | 표시 | 현재 새총·가방; 기존 상체 호흡을 따름 |
| LowerBody | 표시 | 옷자락·단순화한 바지와 부츠; 전 프레임 고정 |
| UpperBody_Arms_Equipment | 표시 | 기존 상체·팔·손 |
| Head_Hat_Hair_Face | 표시 | 기존 모자·깃털·머리카락·얼굴 |

`idle` 태그 F1~5, 정방향 반복, 360 / 180 / 300 / 180 / 180ms, 총 1200ms다. 상체·장비의 y 이동 0/1/1/1/0, 머리의 0/1/2/1/0, 발 고정을 유지한다.

## 확인과 파일 위치

[전후·호흡 재생기](../../Previews/IdleBreathV2/index.html) · [전후 8배 비교](../../Previews/IdleBreathV2/before_after_8x.png) · [하체 단독 전후](../../QA/IdleBreathV2/p001/lower_before_after_8x.png) · [전 5프레임 실제 64px](../../QA/IdleBreathV2/p001/after_contact_1x.png)

출력은 [Frames/IdleBreathV2](Frames/IdleBreathV2), [시트](Export/IdleBreathV2/idle_breath_v2_sheet.png), [제작 메타](Export/IdleBreathV2/idle_breath_v2.json), [호흡 GIF](../../Previews/IdleBreathV2/idle_breath_v2.gif)에 분리했다. 마스크·검수 배경은 캐릭터 출력에 포함하지 않는다.

입력은 [Source/IdleBreathV2/p001](Source/IdleBreathV2/p001/idle_breath_before.aseprite)에 동결했다. 이번 디자인의 기준 예시는 [전후 2프레임 참고 파일](../../QA/IdleBreathV2/p001/design_reference.aseprite)이며, [부위 마스크](Masks/IdleBreathV2/leg_design_scope.aseprite)의 F1은 기존, F2는 v2다. 마스크 index1=바지, 2=부츠, 3=보존 옷자락이다. 실제 수정 경계와 검수 비교에 사용한 안내 파일이며 편집 정본의 레이어가 아니다.

[수치 검수](../../QA/IdleBreathV2/p001/native_validation.json) · [작화·레이어 검수](../../QA/IdleBreathV2/p001/visual_review.md) · [원본 15파일 보존](../../QA/IdleBreathV2/p001/source_preservation.json) · [실제 브라우저 검수](../../QA/IdleBreathV2/p001/browser_validation.json)

이번 결과는 idle 다리 디자인 시험본이다. 다른 부위의 전체 경계 재정리나 Unity 적용 검수로 확대해서 해석하지 않는다.
