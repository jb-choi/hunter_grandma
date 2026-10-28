# Focus8 · rework2 단색 실루엣 시안 검수

2026-10-13. 대상은 [focus_rework2.aseprite](../../../../focus_rework2.aseprite)의8F·8레이어다. [기준1F 보존본](../../../../Source/Rework2/Focus8/p001/focus_rework2_base.aseprite)을 입력으로 사용했다. 이번 판정 범위는 **8부위 단색으로 새총 들기→잡기→당기기→조준 유지가 읽히는지**다.

[전체8F](../../../../Previews/Rework2/Focus8/focus_contact_4x.png) · [실제64px](../../../../Previews/Rework2/Focus8/actual_size_64.png) · [재생기](../../../../Previews/Rework2/Focus8/index.html).

## 제작 기준과 보정

팔을 일정 굵기 선으로 새로 그리지 않았다. source의 팔 실루엣을 상완·전완 두 영역으로 나누고 어깨·팔꿈치 기준으로 회전했다. 두 구간 길이는 왼팔6.32/5.10px, 오른팔5.39/6.08px로 고정했다. 소수 관절 좌표를 최근접으로 정수 픽셀에 옮겼으며 평균색/AA/블러를 사용하지 않았다. 어깨·팔꿈치·손 좌표와 회전값은 [focus.json](../../../../Export/Rework2/Focus8/focus.json)에 기록한다.

처음 궤적은 F3→F4에서 왼쪽 팔꿈치가 크게 옆으로 이동했다. 손을 몸 아래에서 앞으로 올리는 완만한 곡선으로 조정했다. 이전 후보는 [candidate_01_before_hand_arc.aseprite](Attempts/candidate_01_before_hand_arc.aseprite)에 보존했다.

새총을 분리할 때 상세 원본의 고무줄 색을 모두 삭제하면 왼쪽 갈래 자체가 얇아졌다. source 갈래의 실루엣은 유지하고 기존의 바깥 고무줄·주머니 꼬리만 제거한 뒤 새 당김 선을 구성했다. 새총·고무줄·자루는 요청대로 같은 회청색 레이어에 둔다.

## 전 레이어·전 프레임 단독/숨김

저장 파일을 다시 열어 단독8개와 한 레이어씩 숨긴8개 상태를 실제 visible로 설정·저장·재개봉했다. 각 상태의 전8F, 총 단독64F+숨김64F를 PNG로 렌더했다. [native_validation.json](native_validation.json)의 `states`에 파일·표시 목록·이미지 경로를 기록했다. 정본의 전8레이어 visible은 유지한다.

| 부위 / F1~8 | 단독 근거 | 숨김 근거 | 판정 범위 |
| --- | --- | --- | --- |
| 새총+자루 | [단독](solo_1_light_4x.png) | [숨김](hidden_1_light_4x.png) | 자루 고정, source 갈래 유지, 무기 들림과 당김 선 연결. 현재 실루엣 시안 범위에서 확인. |
| 왼다리 | [단독](solo_2_light_4x.png) | [숨김](hidden_2_light_4x.png) | 허벅지·부츠의 source 픽셀·위치 고정. |
| 오른다리 | [단독](solo_3_light_4x.png) | [숨김](hidden_3_light_4x.png) | 허벅지·부츠의 source 픽셀·위치 고정. |
| 하반신 몸통 | [단독](solo_4_light_4x.png) | [숨김](hidden_4_light_4x.png) | 골반·자락 고정, 발/자락 접점 유지. |
| 상반신 몸통 | [단독](solo_5_light_4x.png) | [숨김](hidden_5_light_4x.png) | 팔이 움직여도 복원한 상의 면이 이어짐. 새 사각 누락 없음. |
| 왼팔 | [단독](solo_6_light_4x.png) | [숨김](hidden_6_light_4x.png) | source 형태 회전, F2~5 접근, F6 고무줄 잡기, F7~8 당김. 팔 길이 확대 없이 한 연결된 부위로 유지. |
| 오른팔 | [단독](solo_7_light_4x.png) | [숨김](hidden_7_light_4x.png) | 새총을 몸 가까이 들고 마지막에 앞으로 뻗음. 어깨·팔꿈치·그립 연결과 source 형태 확인. |
| 머리·모자 | [단독](solo_8_light_4x.png) | [숨김](hidden_8_light_4x.png) | source 머리 전체 고정. 회청색 당김 선과 분홍 손이 전면에서 읽히는지 합성으로 확인. |

위 표는 **동작 구조 시안**에 대한 확인이다. source의 원래 옷·손가락 디테일을 되살린 최종 작화 판정이 아니다. 팔의 픽셀 면적은 회전·접힘·두 원본 조각의 겹침으로 달라지며 면적 변화만으로 길이가 변했다고 판단하지 않는다. 정확한 프레임별 값은 native 검사에서 확인한다.

## 핵심 접점과 남은 범위

- F1은 source8개 셀 이미지·좌표·opacity가 그대로다. 머리·상/하반신·양다리는 전8F에서 raw source 셀과 같다.
- 새총의 source 그립 `(45,42)`은 최종 `(48,33)`까지 이동한다. F6부터 자유손은 `(40,34)`→`(40,33)`→`(39,33)`이며 주머니 접점을 따른다.
- [F6 잡기 확대](grip_f06_12x.png), [F7 당김 확대](grip_f07_12x.png), [F8 조준 확대](grip_f08_12x.png)에서 손/그립과 손/주머니의 연결을 확인했다. 겹침 수치와 별도로 전체 실루엣을 보았다.
- 왼쪽 어깨는 source `(26,35)`에서 몸 안쪽 `(29,35)`으로 접근한다. 이3px 이동은 부위 시안의 포즈 선택이며 실제3D 회전이나 최종 해부학적 작화의 증명이 아니다.
- 얼굴·손가락·소매 명암·새총 재질은 요청한 단색 상태로 유지한다. 당김 세기와 최종 조준 높이의 선호, 원화 디테일 복원은 후속 수정에서 결정할 수 있다.

## 검수 결과

| 종류 | 결과 | 근거 |
| --- | --- | --- |
| native 구조·팔레트·시간·출력 | 통과 | [native_validation.json](native_validation.json),304개 검사.8F/8layer/9palette,태그focus1~8,1210ms |
| 원본 F1·고정 부위·자루·발 | 통과 | raw source 비교,접지y56,원본 셀/자루 차이0 |
| 단독/숨김·64px·접점 확대 | 시안 범위 확인 | 위 전128상태 프레임·전체8F·접점3개 확대 |
| GIF 픽셀·알파·시간/보존본 | 통과 | [independent_validation.json](independent_validation.json),GIF8F 및 다른native14파일 보존 |
| 실제 브라우저 재생·마지막 유지 | 통과 | [browser_validation.json](browser_validation.json),전체/단독/숨김136선택,64px/512px Point,단발F8 유지,검수2주기,배경3종,390px 화면 |
| 최종 디테일 작화·shoot 연결·Unity | 미수행 | 이번 범위는 focus8F 단색 실루엣 시안 |

[브라우저 조준 유지](browser_aim_hold.png)와 [390px 표시](browser_mobile390.png)를 함께 확인했다. 마지막F8에서 정지 후 자세가 유지되고 작은 화면에서는 정수5배·320px로 축소 표시한다. [브라우저64px](browser_native64.png)와 정상/2배 속도,검수 반복2주기도 확인했다.

현재 확인 범위에서 빈 프레임·분리된 팔·손 접점 단절·상체 구멍·발 이동은 찾지 못했다. 이번 완료 범위는 8F 실루엣 동작 시안이며 실제 게임과 연결한 최종 조준/발사 판단은 이 검사와 구분한다.
