# idle F1 수정 전파 · p001

2026-10-12. 현재 편집 파일은 [idle_breath.aseprite](../../../../Clips/HU-01_idle/idle_breath.aseprite)다. 저장된F1 수정24점을 F2~5에 전파했다. [직전 전체native](../../../../Clips/HU-01_idle/Source/IdleBreath/Frame01Propagation/p001/idle_breath_before.aseprite), [기준1F](../../../../Clips/HU-01_idle/Source/IdleBreath/Frame01Propagation/p001/frame01_keypose.aseprite), [해시](source_hash.json)를 보관한다.

F1의 원본3셀은 크기·좌표·픽셀을 그대로 유지한다. Head에 들어간 몸통·손9점을 뒤프레임에서는 상체에 반영해 머리의추가하향을 따라 잘못 움직이지 않게 했다. 모자·깃털·머리카락·얼굴·귀는 기존 머리 움직임, 하체는 고정이다. 옷·손·장비의 의미를 유지하며 이미지 생성·색 보간을 사용하지 않았다.

[수정 전 전체5F](before_contact_8x.png) · [수정 후 전체5F](after_contact_8x.png) · [24점/파트 이동 기록](propagation.json) · [native 검수](native_validation.json) · [재생기](../../../../Previews/IdleBreath/index.html)

| 확인 | 결과 |
|---|---|
| F1 셀 및 합성 보존 | 변경0 |
| F2~5 디자인 전파 | 각24개 visible픽셀 반영 |
| 기존 모션과 duration | 정수 부위이동·1200ms 유지 |
| F1=F5, F2=F4 | 불일치0 |
| PNG/시트/GIF/8배GIF | 불일치0 |
| 팔레트·알파·발 | 기존17칸·반투명0·발고정 |

전체5F 확대와 실제부위검수에서 모자/얼굴 색·상의/손 경계·신발 삭제가 반영됐고, 머리최저F3에서도상의와손이추가로내려가지않는다. 독립 검수에서도상의내부새네모구멍없음,발고정,복귀디자인튀는현상없음을확인했다. 원본 `idle.aseprite`와 HU-02~08은 변경하지 않았다. 기존 Pass02기록은 당시원본기준의검수이며 최신호흡원본판정은 이 기록을 사용한다.

실시간 재생과 실제64px/512px 표시·1200ms 반복 검수도 통과했다. [브라우저 검사](browser_validation.json), [최저자세 화면](../../pose_03_8x.png). 현재 통합 재생기도 새F1 기준 자세를 표시하며 idleF3에 멈춰 두었다.
