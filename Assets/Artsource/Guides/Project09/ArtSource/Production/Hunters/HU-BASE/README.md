# HU-BASE · hunter 32×32 리디자인

v002 · 2026-10-11 · 상태: 피드백 반영 수정본 출력 / Unity 미적용

v001은 생성 시안의 얼굴·비율·의상 특징을 지나치게 단순화했다. v002는 처음 시안의 구도를 기준으로 챙이 내려오는 모자, 금갈색 머리와 귀, 목깃, 사선 가죽 끈, 화살통, 튜닉 자락과 부츠를 다시 살렸다. 기술적 파일 검수와 사용자의 디자인 승인은 구분한다.

## 현재 편집 정본과 출력

- [hunter_base.aseprite](hunter_base.aseprite): 실제 32×32, 1프레임, `idle_base` 태그, Body/Bow 2레이어. 두 레이어가 켜진 완성 포즈로 열린다.
- [몸 PNG](Export/hunter_base.png): 별도 런타임 활과 함께 사용할 몸.
- [활 포함 PNG](Export/hunter_base_with_bow.png): 현재 포즈 합성본.
- [활 PNG](Export/bow_base.png): 몸과 같은 32×32 좌표.
- [제작 메타](Export/hunter_base.json), [14색 팔레트](Export/hunter_base_palette.gpl), [레이어별 픽셀 격자](hunter_base_grid.json).

100ms는 정적 프레임의 duration이다. 애니메이션을 제작한 것은 아니다. Body와 Bow는 같은 캔버스를 유지하며 별도 자동 크롭을 하지 않는다.

## 픽셀과 접점

좌상단 원점, x 오른쪽/y 아래쪽, 단위는 원본 픽셀.

| 항목 | 값 |
| --- | --- |
| 캔버스 | 32×32 |
| 몸 비투명 경계 | x7, y2, 폭18, 높이28 |
| 합성본 비투명 경계 | x7, y2, 폭19, 높이28 |
| 발 마지막 픽셀 / 바닥 경계 | y29 / y30 |
| 뒤/앞 발 기준 | (14,29) / (19,29) |
| 손–활 접점 | (24,21) |
| 권장 바닥 피벗 | 경계 (16,30), Unity 정규화 (.5,.0625), 적용 전 시작안 |

몸/합성본은 14색, 활은 4색이며 모든 출력의 반투명 픽셀은 0개다. 팔레트는 v002 자산용 색 선택이다. 기존 Woodland GPL이나 AD-01 공통 팔레트를 자동 변경하지 않는다.

## 미리보기와 기록

- [현재 합성본 12배](../../../Previews/HU-BASE/v002/hunter_base_with_bow_12x.png)
- [기존 / v001 / v002 비교](../../../Previews/HU-BASE/v002/hunter_comparison_6x.png)
- [v002 시안·제작 과정](../../../Drafts/HU-BASE/v002/README.md)
- [v001 보존본](../../../Backups/HU-BASE/2026-10-11_v001/README.md)
- [작업 기록](../../../../Docs/AI/Art/02_Production/HU-BASE_HunterRedesign/README.md)

내장 image_gen으로 참고 시안을 만들었다. 최종 픽셀은 첫 시안의 구도를 Aseprite에서 19×29로 축소해 참고한 뒤, 알파를 이진화하고 14색으로 정리하여 32×32 캔버스에서 눈·부츠·활을 수정했다. 고해상도 생성 시안 자체를 32×32 납품본으로 부르지 않는다.

원본 hunter PNG/Aseprite와 두 Unity 메타는 유지했다. 실제 적용 시 Sprite slice, 표시 높이, 새 활 접점, 6종 비율을 확인해야 한다. 현재 파일은 정적 수정본이며 Unity 적용·게임 실행 검수는 수행하지 않았다.
