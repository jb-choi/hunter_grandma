# HU-02 · walk_retry

2026-10-13. 새 편집 파일은 [walk_retry.aseprite](walk_retry.aseprite)다. [보존된 walk_another F1](Source/WalkRetry/p001/walk_another.aseprite)의 최신 부츠·바짓단 디자인과 기존 머리/얼굴/상의/새총/자루를 사용했다. 기존 walk와 walk_another는 덮어쓰지 않았다.

8프레임·각120ms·960ms 반복이며 모든 프레임에 작화가 있다. Native F9는 F1 셀 이미지·좌표의 정확한 복사본이고 cycle_close 태그에서 확인한다. walk 태그는1–8로 F9를 반복에 중복 넣지 않았다.

| 프레임 | 동작 |
|---|---|
| 1 | 기준 앞발 착지 |
| 2 | 몸 낮춤·뒤발 들기 |
| 3 | 뒤 무릎 굽힘·들린 밑창 y53 |
| 4 | 몸 상승·뒤발 전방 |
| 5 | 반대발 착지 |
| 6 | 몸 낮춤·앞발 들기 |
| 7 | 앞 무릎 굽힘·들린 밑창 y53 |
| 8 | 몸 상승·앞발 전방 |
| 9 | F1 그대로·닫힘 비교용 |

6개 레이어 이름·순서는 원본과 같다. backgroud_temp/Bow&Quiver는 숨기고 Slingshot&bag/LowerBody/UpperBody_Arms_Equipment/Head_Hat_Hair_Face를 표시했다. 팔 뒤 상의 면을 복원한 뒤 원래 손·팔 픽셀을 정수 행 이동으로 흔들었다. 머리는 통째로 y0/1/0/-1을 두 번 반복하고, 새총은 앞손, 자루는 몸 높이와 ±1px 지연 움직임을 따른다.

부츠는 최근 source rear31px/front42px 디자인을 사용했다. 앞부츠 x31을 포함해 원래 색/형상을 정수 이동하고, 무릎→커프는 동일 팔레트로 연결했다. 교차 시 올라온 다리 위의 자락 끝만1px 올렸으며 중앙 자락 패임을 보존했다. 실제 교차에서는 가까운 다리/자락이 먼 부츠 일부를 가리는 의도된 겹침이 있다.

[별도 재생기](../../Previews/WalkRetry/index.html) · [8프레임 GIF](../../Previews/WalkRetry/walk_retry_8x.gif) · [9프레임 전체](../../QA/WalkRetry/p001/contact_8x.png) · [출력/모션 메타](Export/WalkRetry/walk_retry.json) · [부위 마스크](Masks/WalkRetry/material_regions.aseprite) · [수치 검수](../../QA/WalkRetry/p001/native_validation.json) · [입력 비교](../../QA/WalkRetry/p001/Review/source_review.md).

저장·픽셀·팔레트·알파·기준 셀·발/장비 이동 검수를 통과했다. 작화와 브라우저 재생 판정은 [최신 검수 기록](../../QA/WalkRetry/p001/README.md)을 따른다. 이미지 재생성·블러·안티앨리어싱·디더링 없이 작업했고 Unity Assets는 변경하지 않았다.
