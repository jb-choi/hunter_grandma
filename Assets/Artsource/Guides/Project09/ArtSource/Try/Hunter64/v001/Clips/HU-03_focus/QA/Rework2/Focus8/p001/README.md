# Focus8 · rework2 작업 안내

2026-10-13. 사용자가 요청한 **8부위 단색 실루엣의 focus8프레임**을 같은 [focus_rework2.aseprite](../../../../focus_rework2.aseprite)에 추가했다. 이전1F 파일은 [Source 보존본](../../../../Source/Rework2/Focus8/p001/focus_rework2_base.aseprite)에 바이트 그대로 남겼다. 작업 범위는 focus뿐이다.

[8프레임 PNG](../../../../Export/Rework2/Focus8/focus_sheet.png) · [재생기](../../../../Previews/Rework2/Focus8/index.html) · [시안 판정](visual_review.md) · [팔·시간 명세](../../../../Export/Rework2/Focus8/focus.json).

| F | 내용 | 시간 |
| --- | --- | --- |
| 1 | 원래 기준 자세 | 150ms |
| 2 | 몸 가까이 새총 들기 | 120ms |
| 3 | 새총 상승·자유손 상승 | 120ms |
| 4 | 무기 정렬·주머니 접근 | 140ms |
| 5 | 손을 주머니 쪽으로 올림 | 160ms |
| 6 | 고무줄 주머니 잡기 | 140ms |
| 7 | 무기를 앞으로 뻗으며 당김 | 140ms |
| 8 | 완전 조준·마지막 자세 유지 | 240ms / 이후 유지 |

총1210ms·8F,태그`focus`1~8.64×64 Indexed,8레이어·부위8색·투명0을 유지한다. 팔 두 구간은 원본 실루엣을 어깨/팔꿈치에서 회전하고, 머리·몸통·자락·다리·자루는 고정한다. 프레임별 뼈대의 길이·좌표·회전값은 JSON에 있다.

`States/`, `Solo/`, `Hidden/`은 실제 native 표시 상태16개와 전128프레임이다. 검수 배경과 안내는 결과 native에 합치지 않는다. [입력/다른 파일 해시](source_hashes.json), [저장 파일 검사](native_validation.json), [독립 GIF·보존 검사](independent_validation.json), [현재 파일 공개 기록](publication.json)을 보관한다.

[실제 브라우저 검사](browser_validation.json)도 통과했다.8F×전체/단독/숨김17조합=136선택,단발 후F8 유지,정상/2배 속도,검수 반복2주기,64px/8배 Point,배경3종,390px 화면을 확인했다. 빠른 표시 전환으로 취소된 이전 이미지 요청은 별도 기록하며 실제 로드 실패와 구분한다.

이 결과는 움직임과 부위 연결을 비교하는 **실루엣 시안**이다. 원래 캐릭터 디테일 작화·shoot 재시도·Unity 적용은 이번 작업 범위에 포함하지 않는다. GIF는 반복 확인용이고 재생기는 단발 후 F8을 유지한다. 검수용 반복을 켜면 F1로 다시 시작한다.

코드: [build.lua](build.lua), [validate.lua](validate.lua), [prepare_preview.py](prepare_preview.py), [export_zoom.lua](export_zoom.lua), [validate_player.cjs](validate_player.cjs).
