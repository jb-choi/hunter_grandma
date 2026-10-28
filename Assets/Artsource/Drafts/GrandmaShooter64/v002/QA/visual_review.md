# HG-BASE64 v002 정적 시안 검수

2026-10-21 · 후보 04–08, 각각 저장 native 1프레임·4레이어.

검수 정본과 입력의 해시는 [delivery_hashes.json](delivery_hashes.json)을 따른다. 실제 생성 원화, 축소 중간본, 최종 64px 후보를 구별했다. 디자인의 매력에 대한 사용자 승인은 아직 없다.

## 전체와 실제 크기

- [실제 64px 후보 5개](../Previews/candidates_04-08_native.png)
- [최근접 5배 비교](../Previews/candidates_04-08_64px_5x.png)
- [생성 원화 방향](../Previews/concept_directions.png): 고해상도 원화 비교용이며 최종 64px가 아니다.

## 레이어 시트와 배경 검수 근거

시트 배치: 위 왼쪽 01 윤곽, 위 오른쪽 02 통합 캐릭터, 아래 왼쪽 03 표정, 아래 오른쪽 04 총구 효과. `solo`는 한 레이어만 표시, `hidden`은 한 레이어만 숨긴 상태다.

| 후보 F1 | 전 레이어 단독 | 한 레이어씩 숨김 | 전체 밝음·어두움 |
| --- | --- | --- | --- |
| 04 순덕 | [시트](LayerReview/grandma_04_sundeok_solo_4x.png) | [시트](LayerReview/grandma_04_sundeok_hidden_4x.png) | [밝음](../Previews/grandma_04_sundeok_8x.png) · [어두움](../Previews/grandma_04_sundeok_dark_8x.png) |
| 05 숲지기 | [시트](LayerReview/grandma_05_redhood_solo_4x.png) | [시트](LayerReview/grandma_05_redhood_hidden_4x.png) | [밝음](../Previews/grandma_05_redhood_8x.png) · [어두움](../Previews/grandma_05_redhood_dark_8x.png) |
| 06 찻집 | [시트](LayerReview/grandma_06_teatime_solo_4x.png) | [시트](LayerReview/grandma_06_teatime_hidden_4x.png) | [밝음](../Previews/grandma_06_teatime_8x.png) · [어두움](../Previews/grandma_06_teatime_dark_8x.png) |
| 07 마녀 | [시트](LayerReview/grandma_07_mosswitch_solo_4x.png) | [시트](LayerReview/grandma_07_mosswitch_hidden_4x.png) | [밝음](../Previews/grandma_07_mosswitch_8x.png) · [어두움](../Previews/grandma_07_mosswitch_dark_8x.png) |
| 08 서부 | [시트](LayerReview/grandma_08_wildrose_solo_4x.png) | [시트](LayerReview/grandma_08_wildrose_hidden_4x.png) | [밝음](../Previews/grandma_08_wildrose_8x.png) · [어두움](../Previews/grandma_08_wildrose_dark_8x.png) |

모든 상태는 Aseprite Lua에서 실제 `isVisible`을 전환해 상태별 native를 저장한 뒤 다시 열었다. 실제 표시 값과 수행 범위는 [검사 JSON](native_validation.json)과 [재현 코드](../Scripts/validate_and_preview.lua)에 기록했다.

## 부위·역할 판정

| 역할 / 전 후보 F1 | 확인 사항 | 판정 | 한계·후속 |
| --- | --- | --- | --- |
| 윤곽 단독·숨김 | 캐릭터/총 둘레만 포함, 배경 픽셀 혼입 없음, 총구 FX 제외 | 역할 검수 통과 | 윤곽만 보였을 때 내부가 비는 것은 정상 |
| 통합 캐릭터 단독·숨김 | 머리부터 발·총·손까지 하나의 정적 자세, PNG 경계 잘림 없음 | 정적 베이스 시안 통과 | 팔·총·몸통의 독립 분리와 숨은 면 제작은 후속 |
| 표정 단독·숨김 | 눈·안경·입의 소량 보정 픽셀, 머리 전체나 다른 몸 부위 혼입 없음 | 역할 검수 통과 | 단독은 완전한 얼굴이 아니며 베이스 얼굴 위에 겹치는 보정 |
| 총구 FX 단독·숨김 | 작은 금색 효과만 포함, 끄면 캐릭터 자세 유지 | 역할 검수 통과 | 연속 발사 FX는 미제작 |
| 얼굴·머리 | 파마/두건/올림머리/마녀모자/카우보이 모자가 구별됨, 웃음·윙크·흰 머리 확인 | 방향 탐색용 검수 통과 | 05는 얼굴이 작아 세부 나이 표현과 눈동자가 간략함 |
| 몸통·의상·팔·손·총 | 넓게 버틴 자세·두손 장총/한손 권총 구분, 손이 총에 닿고 무기 방향 읽힘 | 정적 베이스 시안 통과 | 손가락·반동·관절은 연속 동작에서 재작화 필요 |
| 하체·발 | 후보별 치마/몸빼/바지와 신발 구분, y60 바닥 접점, 실루엣 잘림 없음 | 정적 베이스 시안 통과 | 보행 키포즈와 중간 프레임 없음 |
| 실제64px·확대 | 색 면과 의상·머리 실루엣으로 다섯 방향 구분 | 후보 비교 가능 | 원화의 장식·주름을 모두 보존한 결과는 아님 |
| 어두운 배경 | 전체 대비와 주요 얼굴·총 확인 | 검토 완료 | 04 바지·07 모자끝·08 코트끝 대비는 실제 배경 선정 후 재조정 권장 |

## 파일 검사와 완료 범위

| 검사 | 결과 | 근거 |
| --- | --- | --- |
| native/PNG·효과 없음 PNG | 통과 | 10개 비교 모두 차이 0 |
| 규격·팔레트·알파 | 통과 | 64×64, 1F, 4L, 33슬롯, 사용색 26/27/27/28/27, 부분 알파 0, 팔레트 밖 색 0 |
| 실제 레이어 단독·숨김 | 통과 | 40개 상태 native 저장·재개봉, 10개 시트 전부 시각 검수 |
| 기본 표시·합성 복원 | 통과 | 정본은 QA 중 미저장, 복원 합성 차이 0, 기본 4레이어 표시 |
| 이전 후보 보존 | 통과 | v001 루트 원본의 사전/사후 SHA-256 일치 |
| 시각적 선호도·캐릭터 최종 채택 | 사용자 선택 전 | 후보 제작 완료와 디자인 승인을 구분 |
| 애니메이션·Unity 실화면 | 해당 없음 | 정적 베이스 후보 추가 작업 |

최종 범위는 새로운 베이스 후보 5개·조사 기록·원화·64px 편집 파일·PNG·검수 자료다. 어두운 배경 대비와 상세 파츠 정리는 채택 후보의 후속 제작 범위로 남긴다.
