# LH-01 오두막 소파·확장 작업본 v003

2026-10-12 (Asia/Seoul). 사용자 요청: 조금 더 넓게, 왼쪽에 소파 추가.

**현재 편집 작업본:** [Work/loghouse_640x320_v003.aseprite](Work/loghouse_640x320_v003.aseprite)

[새 컨셉](Concept/hunting_idle_LH-01_loghouse_sofa_concept_v003.png) · [전체 생성 프롬프트](prompt.txt) · [시각 검토](QA/visual_review.md)

## 변경과 규격

- 576×320 → **640×320**, 폭 64px(약 11.1%) 추가. 64×64 격자 **10×5칸**.
- 왼쪽 실내에 녹색 2인 소파를 배치했다. 도감 장은 소파 오른쪽으로 이동해 열린 책이 보이게 했다. 중앙 바닥 여유와 기존 무기·동물 액자·탐색 책상·우측 사냥개·외부 개집/수레를 유지하는 컨셉 수정이다.
- 내장 image_gen으로 기존 컨셉을 수정한 뒤 Aseprite 1.3.18.6에서 Nearest로 정규화했다. 새 생성 컨셉 1774×887은 정확히 2:1이므로 640×320 변환에 크롭·비율 왜곡 없음.
- 96색 상한, 실제 **92색**. RGB 편집 모드, 디더링/블러 없음. 정적 1프레임 100ms, `idle_layout` 태그.
- **15레이어:** 배경, 숨긴 참조, 소품 11종, 빈 리터치 레이어, 숨긴 64격자. 소파는 `12 Living - sofa`에 따로 분리했다.
- 소파 출력: `Export/prop_sofa_128x64.png`. 실제 영역 104×54, 128×64 셀 내부에서 가로 중앙/바닥 정렬.
- 소품 11종 출력은 전부 64배수 크기. 작은 장식/도감/도구 64×64, 활 64×128, 개집·사냥개/잠자리·소파 128×64, 수레 128×128. 개집은 새 컨셉의 실제 높이에 맞춰 이전 128×128 대신 128×64 셀을 사용한다.
- 장면 내 위치와 독립 출력 셀 오프셋/피벗은 `Export/loghouse_layout.json`에 기록한다. 소품 위치를 전부 64격자 모서리로 이동한 것은 아니다.
- Point/Nearest 표시 권장: 1× 640×320 / 2× 1280×640 / 3× 1920×960.

## 저장물

- `Work/loghouse_640x320_v003.aseprite`: 이 버전 편집 작업본.
- `Export/loghouse_640x320.png`: 기본 표시 레이어 합성본.
- `Export/loghouse_background_repaired_640x320.png`: 가려졌던 면을 주변 픽셀로 추정한 배경 준비본.
- `Export/prop_*.png`: 주요 소품 11종 투명 PNG.
- `Export/loghouse_palette.gpl`, `Export/loghouse_layout.json`: 팔레트와 위치/셀/피벗.
- `Previews/loghouse_2x.png`, `loghouse_grid64.png`, `props_64_aligned.png`: 확대·격자·소품 접촉판.
- `Scripts/normalize.lua`, `objects.lua`, `build_workfile.lua`, `verify_workfile.lua`: 정규화/영역 정의/제작/재읽기 검사. `--script-param root=<v003 폴더>`를 전달한다.
- `QA/file_verification.json`, `props_verification.json`, `source_hashes.json`: 검증 결과와 입력 해시.

## 보존과 완료 범위

v001 컨셉과 v002 Aseprite 원본은 수정하지 않았고 이번 `Source/`에도 보존했다. 새 컨셉은 기존 컨셉의 픽셀을 그대로 가로 복사한 것이 아니라 이미지 생성 편집을 거친 것으로, 작은 배경·가구 디테일은 달라질 수 있다.

현재 파일은 **레이어를 나눈 편집 준비본**이다. 주요 소품의 다각형 영역에 원래 배경 픽셀이 일부 포함된다. 원래 가려졌던 배경은 추정 복원해 복제 질감과 접합이 남으며, 소파/소품을 크게 이동할 때 배경과 경계를 추가 리터치해야 한다. 모든 가시 레이어를 합친 기본 화면은 새 640×320 정규화 컨셉과 0픽셀 차이다.

Unity 자산과 `.meta`는 변경하지 않았다. 실제 게임 표시 검수, 장비 단계 외형, 사냥개 애니메이션은 별도 후속 작업이다. Notion 아트워크 목록 6-1절에 최신 이미지·규격·작업 묶음을 반영한다.
