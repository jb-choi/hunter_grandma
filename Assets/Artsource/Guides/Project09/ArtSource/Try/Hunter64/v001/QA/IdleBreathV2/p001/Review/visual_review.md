# idle_breath_v2 독립 작화·레이어 검수

검수일: 2026-10-14. 입력은 동결한 `Clips/HU-01_idle/Source/IdleBreathV2/p001/idle_breath_before.aseprite`이고, 검수 후보는 저장 후 다시 연 `QA/IdleBreathV2/p001/candidate.aseprite`다. 납품 파일 [idle_breath_v2.aseprite](../../../../Clips/HU-01_idle/idle_breath_v2.aseprite)는 후보와 SHA-256이 같은 복사본임을 확인했다: `E21BDFF2468336DC3C4F307F3EBAA2F5233D518DBC5CF6A0B194EE124D27D110`.

**정적 작화·단독/숨김 검수 통과. 이번 다리 단순화 범위에서 추가 필수 픽셀 보정은 발견하지 못했다.** 전신 경계 전체를 새로 정리했다는 판정은 아니다. 머리·상의·팔·장비·자락은 기존 디자인과 분할 구조를 보존했으며, 이 검수는 native를 수정하지 않았다.

## 실제 확인 방법과 범위

새 [ArtworkGuide](../../../../../../../../Docs/AI/Art/02_Production/ArtworkGuide.md)의 단독/숨김 검수를 적용했다. root가 actual `isVisible`을 바꿔 저장한 [solo/omit 상태 목록](../layer_review_states.json)의 10개 `.aseprite`를 독립적으로 다시 열고, 전5프레임을 실제 렌더했다. 후보의 기본 조합도 독립 재개봉했다. 기본5 + 단독25 + 숨김25, 총55개 렌더를 검사했고 10개 상태의 실제 visible 값과 PNG 대응에 불일치가 없었다. UI 단축키를 실행한 것은 아니며 Lua에서 저장된 표시 상태를 읽었다.

기본 및 모든 단독/숨김 상태의 64px 실물, 밝은 배경8배, 어두운·체크 배경의 전5프레임8배를 실제 이미지로 열어 확인했다. 넓은 contact가 자동 축소되지 않도록 밝은 배경은 F1~3/F4~5로 나눴고, 다른 배경은 부위별 동일 위치 crop을 사용했다. 확대는 Point/Nearest이며 검수 배경은 캐릭터 출력에 넣지 않았다.

[독립 재개봉 수치 확인](reopened_state_checks.json) · [재현 스크립트](reopen_states.lua) · [64px 단독 전체](solo_light_overview_1x.png) · [64px 숨김 전체](omit_light_overview_1x.png) · [단독 어두운 배경](solo_dark_overview_1x.png) · [단독 체크](solo_checker_overview_1x.png) · [숨김 어두운 배경](omit_dark_overview_1x.png) · [숨김 체크](omit_checker_overview_1x.png)

## 단순화와 전신 판정

[Lower 전후8배](../lower_before_after_8x.png) · [전신 전후8배](../../../../Previews/IdleBreathV2/before_after_8x.png) · [기본 F1~3](default_light_group1_8x.png) · [기본 F4~5](default_light_group2_8x.png)

| 부위 / 전5프레임 | 확인한 변화·보존 | 판정 | 남은 문제 |
|---|---|---|---|
| 바지 y49..50 | 다리별 녹색 면이 index2로 통일되고 작은 index3 조각이 없어졌다. 다리 사이 투명 틈과 위 자락 연결은 유지됐다. | 단순화됨 | 없음 |
| 부츠 y51..56 | index7의 금색 발끝 장식과 가로 갈색 띠가 없어졌다. index6 기본색·index5 세로 그림자·index1 윤곽으로 면과 외곽이 정돈됐다. | 단순화됨 | 없음 |
| 자락·허리 y43..48 | 원래 밝은 녹색 테두리·명암·중앙 패임을 유지한다. 다리의 새 수직 면과 닿는 곳에 새 사각 구멍이나 끊김이 없다. | 보존·접점 정상 | 기존 자락 명암은 이번 단순화 대상이 아님 |
| 머리·모자·깃털·머리카락·얼굴·눈·귀 | 기존 모양과 호흡 높이 변화 유지. 내려간 F3에서도 목·스카프가 이어지고 얼굴 인상이 변하지 않는다. | 보존 | 전신 색·경계 추가 정리는 이번 범위 밖 |
| 상의·팔·손·버클 | 기존 셔츠와 손 모양을 유지한다. 손–새총 그립, 허리–자락 겹침이 전5프레임에서 정상이다. | 보존·접점 정상 | 단독 상태의 아래 열린 여백은 Lower와 합치는 분할 구조임 |
| 새총·자루 | 기존 형태와 상체 이동을 유지한다. 단순화한 다리로 인해 장비가 잘리거나 새 픽셀이 섞이지 않았다. | 보존·접점 정상 | 장비 자체 색·경계 재작화는 이번 범위 밖 |
| 발 접지·원래 표시 크기 | 전5프레임의 가장 낮은 불투명 픽셀은 y56. 실제64px에서 다리·발 방향이 읽히고 밝은 장식이 없어져 부츠 면이 단순하게 보인다. | 정상 | 없음 |

LowerBody의 y49..56은 각 프레임48점 변경, 그 외 Lower 영역과 다른4개 레이어의 캔버스 픽셀 변화는 0이다. 합성 변화 수는 F1/F5에서47점, F2~4에서48점으로 가림에 따라 다르므로 레이어 변경량과 구분한다. 부츠 사용색은 윤곽 포함4→3색, 바지는3→2색이지만 전체17칸 팔레트는 유지됐으며 전체 캐릭터 색 수 감소를 뜻하지 않는다.

## 전 레이어 단독·숨김 판정

아래→위 순서는 `Bow&Quiver`, `Slingshot&bag`, `LowerBody`, `UpperBody_Arms_Equipment`, `Head_Hat_Hair_Face`다. 기본 상태는 옛 활만 숨김, 나머지4개 표시이며 모든 레이어의 전5프레임을 확인했다.

| 레이어 / 역할·기본 표시 | 단독5프레임 판정·근거 | 기본 조합에서 숨김5프레임 판정·근거 | 남은 문제 / 구조적 예외 |
|---|---|---|---|
| Bow&Quiver / 보존된 옛 장비 / 숨김 | [F1~3](solo_01_light_group1_8x.png) · [F4~5](solo_01_light_group2_8x.png) · [어두운](solo_01_dark_crop_8x.png) · [체크](solo_01_checker_crop_8x.png). 이전 활·화살통과 원래 이동을 확인했다. | [F1~3](omit_01_light_group1_8x.png) · [F4~5](omit_01_light_group2_8x.png). 원래 숨김이므로 기본 합성과 같다. 출력 제외 정상. | 보존 역할 검수 완료. 현재 게임용 장비 작화 통과로 계산하지 않음. |
| Slingshot&bag / 현재 장비 / 표시 | [F1~3](solo_02_light_group1_8x.png) · [F4~5](solo_02_light_group2_8x.png) · [어두운](solo_02_dark_crop_8x.png) · [체크](solo_02_checker_crop_8x.png). source 새총·자루에 다리 픽셀 혼입이나 새 단절 없음. | [F1~3](omit_02_light_group1_8x.png) · [F4~5](omit_02_light_group2_8x.png). 장비를 빼도 몸·손·새 다리의 면이 정상적으로 남는다. | 두 장비 사이 빈 공간은 같은 레이어의 별도 물체 구조. |
| LowerBody / 자락·바지·부츠 / 표시 | [F1~3](solo_03_light_group1_8x.png) · [F4~5](solo_03_light_group2_8x.png) · [어두운](solo_03_dark_crop_8x.png) · [체크](solo_03_checker_crop_8x.png). 동일한 새 다리5개, 단순 면·발끝·밑창·다리 사이 틈 확인. | [F1~3](omit_03_light_group1_8x.png) · [F4~5](omit_03_light_group2_8x.png). 해당 자락·다리·부츠만 없어지고 머리·상체·장비는 정상적으로 남는다. | Lower를 숨긴 아래 빈 영역은 의도한 제거 결과. |
| UpperBody_Arms_Equipment / 상체·양팔·손·벨트 / 표시 | [F1~3](solo_04_light_group1_8x.png) · [F4~5](solo_04_light_group2_8x.png) · [어두운](solo_04_dark_crop_8x.png) · [체크](solo_04_checker_crop_8x.png). 기존 상의·팔·손·버클과 호흡 이동 보존. 부츠 혼입 없음. | [F1~3](omit_04_light_group1_8x.png) · [F4~5](omit_04_light_group2_8x.png). 장비와 Lower 윗면을 드러내도 다리의 새 사각 패치나 잘림 없음. | 단독 아래 열린 여백은 자락·하체와 합치는 구조. Lower 윗부분의 기존 허리/자락 경계색은 변경하지 않음. |
| Head_Hat_Hair_Face / 머리 전체 / 표시 | [F1~3](solo_05_light_group1_8x.png) · [F4~5](solo_05_light_group2_8x.png) · [어두운](solo_05_dark_crop_8x.png) · [체크](solo_05_checker_crop_8x.png). 얼굴·눈·귀·머리카락·모자·깃털을 전5프레임 확인. 하체 혼입 없음. | [F1~3](omit_05_light_group1_8x.png) · [F4~5](omit_05_light_group2_8x.png). 머리 아래 목·스카프·상의와 하체가 source 구조대로 남는다. | 남는 짧은 목 색은 Upper의 연결부이며 머리 잔여 혼입으로 판단하지 않음. |

다리 사이·팔과 몸/장비 사이의 투명 공간, 숨긴 부위의 제거 결과, 합성을 전제로 나눈 레이어의 열린 경계는 정상 구조로 구분했다. 전체 합성의 새 사각 구멍·다리 색 혼입·외곽 잘림·장비 접점 단절은 발견하지 못했다. source의 다른 부위 색·작화까지 새로 정리했다는 의미로 이 판정을 확대하지 않는다.

## 검수 경계와 표시 복원

후보와 10개 상태 파일은 읽기 전용으로 열었고 후보의 기본 표시를 바꾸거나 저장하지 않았다. 독립 수치 확인에서는 기본/단독/숨김55개 PNG 대응 불일치0, actual visible 불일치0을 확인했다. [root의 통합 native 검사](../native_validation.json)는 별도 파일 구조·raw 셀·시트/GIF·마스크 대응 근거다.

실제 브라우저 재생·반복 경계·모바일 검수는 이 정적 검수와 구분하여 root의 [브라우저 검사](../browser_validation.json)를 따른다. 이 검수자가 브라우저 재생을 직접 수행했다고 기록하지 않는다. Unity 게임 연결 검수는 이번 범위 밖이다.
