# HU-BASE · hunter 32×32 리디자인

v001 · 2026-10-11 · 상태: 정적 베이스 제작·파일 검수 완료 / Unity 미적용

초록 모자와 깃털, 따뜻한 얼굴, 숲색 옷과 갈색 부츠를 이어가며 총을 제거했다. 오른쪽을 보는 베이스에 얼굴/코/눈, 벨트, 두 발을 명확히 나누고 활을 별도 레이어로 제작했다.

## 편집 정본과 출력

- **편집 정본:** [hunter_base.aseprite](hunter_base.aseprite) — 32×32, 1프레임, `idle_base` 태그.
- **몸 PNG:** [hunter_base.png](Export/hunter_base.png) — 게임에서 별도 활을 표시할 때 사용.
- **활 포함 PNG:** [hunter_base_with_bow.png](Export/hunter_base_with_bow.png) — 완성 포즈 확인용 합성본.
- **활만 PNG:** [bow_base.png](Export/bow_base.png) — 같은 32×32 좌표를 유지하는 레이어 출력.
- [제작 메타](Export/hunter_base.json), [11색 팔레트](Export/hunter_base_palette.gpl), [재작화 픽셀 격자](hunter_base_grid.json).

레이어는 `Body`(표시), `Bow`(숨김), `Guides`(숨김/잠금)이다. 원본을 열면 몸만 보인다. `Bow`를 켜면 활 포함 포즈, `Guides`를 켜면 바닥과 접점 표시를 확인할 수 있다. 100ms는 정적 프레임의 파일 duration이며 호흡/idle 애니메이션을 만든 것은 아니다.

## 픽셀과 접점

좌표는 PNG 좌상단 원점, x는 오른쪽, y는 아래쪽, 단위는 원본 픽셀이다.

| 항목 | 값 |
| --- | --- |
| 전체 캔버스 | 32×32 |
| 몸 비투명 경계 | x6, y2, 폭21, 높이28 |
| 활 포함 비투명 경계 | x6, y2, 폭24, 높이28 |
| 발바닥 마지막 픽셀 | y29 |
| 바닥 경계선 | y30 |
| 뒤/앞 발 기준 | (13,29) / (19,29) |
| 손–활 접점 | (26,23) |
| 권장 바닥 피벗 | 경계 좌표 (16,30), Unity 정규화 (.5,.0625) 시작안 |

위 권장 피벗은 적용 전 검토값이다. 기존 Unity importer 피벗을 변경한 기록이 아니다. 몸과 활은 같은 32×32 캔버스이므로 레이어 합성 시 서로 다른 자동 크롭을 적용하지 않는다.

몸/합성본은 불투명 11색, 활만 출력은 4색이다. 8색은 기존 Woodland GPL, 녹색 2색 `#536D45/#91AA63`은 기존 사냥꾼 PNG, 하이라이트 `#AABF7A`는 AD-01 후보에서 사용했다. 하이라이트는 이번 자산에 사용하는 색이며 공통 팔레트 전체의 채택을 의미하지 않는다.

## 검수와 적용 범위

- 3개 PNG 모두 실제 32×32, 반투명 경계 픽셀 0개 확인.
- 모자/깃털/얼굴/두 발, 몸과 활 접점의 확대 시각 검수 완료.
- 기존 hunter PNG/Aseprite와 두 `.meta`의 SHA256 동일 확인.
- 기존 그림의 비투명 높이29 대비 신규28이므로 적용 시 실제 표시 높이와 6종 비율을 다시 확인한다.
- 현재 Unity 활 위치는 신규 손 접점과 맞지 않으므로 별도 통합 시 위치를 맞춘다. 몸에 활 포함 PNG를 쓰면서 런타임 활을 함께 표시하지 않는다.
- 프레임 애니메이션·Unity 참조 교체·게임 실행 검수는 수행하지 않았다.

## 미리보기와 제작 기록

- [활 포함 10배 미리보기](../../../Previews/HU-BASE/hunter_base_with_bow_10x.png)
- [몸 10배 미리보기](../../../Previews/HU-BASE/hunter_base_10x.png)
- [기존 / 신규 몸 / 신규 활 포함 비교](../../../Previews/HU-BASE/hunter_comparison_6x.png)
- [imagegen 시안 프롬프트](../../../Drafts/HU-BASE/v001/imagegen_prompt.md), [고해상도 참고 시안](../../../Drafts/HU-BASE/v001/hunter_concept_imagegen.png)
- [작업 기록](../../../../Docs/AI/Art/02_Production/HU-BASE_HunterRedesign/README.md)

시안은 내장 image_gen으로 만들고, 최종 베이스는 Aseprite MCP의 `draw_grid`로 실제 32×32 격자에 재작화했다. 고해상도 시안과 확대 PNG는 게임용 32×32 납품본과 구별한다. 이후 편집은 이 폴더의 정본에서 진행한다.
