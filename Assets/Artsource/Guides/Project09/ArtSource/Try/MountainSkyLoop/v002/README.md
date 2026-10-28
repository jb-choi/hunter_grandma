# BG-01+02 · 하늘과 산 원경 · v002

상태: **레이어 수정 완료 · Try 시안 · Unity 미적용** · 2026-10-12.

후속 해상도 작업: [v003 · 320×180 도트 원본](../v003/README.md). 이 v002는 기존 640×360 원본으로 보존한다.

v001 단독 검수에서 발견한 구름의 산 조각, 먼 능선의 지형 혼입과 미완성 밑칠, 주봉·산림의 투명 경계 누락을 기존 Aseprite에서 직접 수정한다. 640×360, 기존 주봉 구도·암벽 면·산림 색·팔레트·좌우 반복을 유지한다.

입력 원본: `Source/bg_mountain_sky_v001.aseprite`.
입력 SHA256: `7cc18773437f3225643b4b06c44ea8973aceee66431dd2a260abb8d589e7a220`.

픽셀 정리 가이드의 기준 예시·형태 마스크·전후 비교·visibility 전환 검수를 적용한다. 이번 작업은 레이어의 형태와 숨겨진 영역 수정이며, 색 수 축소나 필터만으로 완료를 판단하지 않는다.

- 수정 원본: `bg_mountain_sky.aseprite`.
- 합성 PNG: `Export/bg_mountain_sky.png`.
- 분리 PNG: `Layers/`.
- 실제 수정에 사용하는 영역: `Masks/material_regions.aseprite`.
- 수정 기준 예시: `Reference/cleanup_reference.aseprite`.
- 각 레이어 visibility 상태와 검수 이미지: `QA/LayerReview/`.
- 파일 검증·작화 판정: `QA/`.

v001 원본은 유지했다. Unity 적용은 이번 수정 범위에 포함되지 않는다.

## 실제 수정 결과

- 하늘: v001과 전체 픽셀 동일.
- 구름: 산에 속한 조각과 lower sky의 잔여 점 75픽셀 제거. 큰 구름과 긴 수평 구름 형태는 유지.
- 먼 능선: 주봉·산림 색 혼입을 없애고 가려진 구간을 연속된 능선과 3톤 명암 면으로 작화. 수평 밑칠 행과 세로 잔여물을 제거.
- 주봉·산림: 누락된 알파 영역 3,491픽셀 복원. 골짜기·주봉 측면의 불연속 경계를 수정하고, 수정 전 분리되지 않은 원본 픽셀에서 암벽·식생 색을 되찾아 기존 팔레트로 배치.
- 합성 화면: 8,901픽셀(약 3.86%) 변경. 주봉의 위치·큰 암벽 면·숲 구도 유지. 사용 RGB 37색, 원본의 49항목 팔레트 유지.

색 개수만 줄이는 처리나 새로운 이미지 생성은 사용하지 않았다. Aseprite에서 검토한 외곽 마스크와 원본 픽셀을 사용했고, 가려진 먼 능선을 직접 그렸다. 블러·안티앨리어싱·디더링 없음.

## 검수와 열어볼 파일

- [편집 원본](bg_mountain_sky.aseprite), [합성 PNG](Export/bg_mountain_sky.png), [분리 PNG](Layers/).
- [레이어 단독 비교](QA/LayerReview/layers_contact.png): 위 왼쪽 하늘 / 위 오른쪽 구름 / 아래 왼쪽 먼 능선 / 아래 오른쪽 주봉·산림.
- [구름 전후](Previews/before_after_02_clouds.png), [먼 능선 전후](Previews/before_after_03_distant_ridges.png), [주봉·산림 전후](Previews/before_after_04_granite_woodland.png). 각 이미지의 왼쪽이 v001, 오른쪽이 v002.
- [골짜기 확대 전후](Previews/before_after_mountain_valley_gaps.png), [주봉 확대 전후](Previews/before_after_mountain_peak_gaps.png).
- [합성 전후](Previews/before_after.png), [이동 GIF](Previews/scroll_preview.gif), [조작 HTML](Previews/scroll_preview.html).
- [작화 검수](QA/visual_review.md), [native_validation.json](QA/native_validation.json), [independent_audit.json](QA/independent_audit.json).

실제 Aseprite visibility를 바꾼 단독 표시 4개 + 하나만 숨긴 상태 4개를 저장 후 다시 열어 확인했다. Shift+X 키 입력은 사용하지 않았고 동일한 표시 속성을 직접 변경했다. 각 단독 레이어를 1배·2배·결함 영역 4배·가로 반복으로 확인했다.

원본과 PNG 230,400픽셀 일치, 레이어 PNG와 native cel 일치, 주봉·먼 능선의 형태 마스크 내부 투명 누락 0, 반투명 경계 0, 이동 GIF 80프레임과 PNG 순환 이동의 픽셀 차이 0을 확인했다.

원경의 새로 복원한 가려진 구간은 큰 3톤 면으로 표현했다. 시간대 변형, 게임에서의 별도 레이어 이동 속도와 Unity 적용은 추가 작업이다. HTML의 실제 앱 내 브라우저 조작 검수는 이전에 확인한 `file:` URL 보안 제한 때문에 수행하지 않았다.
