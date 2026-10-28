# BG-01+02 · 320×180 도트 작업 · v003

상태: **320×180 도트 작업 완료 · Try 시안 · Unity 미적용** · 2026-10-12.

사용자가 지정한 **320×180 작업 해상도**로 v002의 하늘·구름·먼 능선·주봉과 산림 4개 레이어를 변환하고 도트 면·경계를 정리한다. 최종 640×360 PNG는 320×180 정본의 최근접 2배 확대다.

- 편집 정본: `bg_mountain_sky.aseprite` · 320×180.
- 네이티브 출력: `Export/bg_mountain_sky_320.png`.
- 2배 출력: `Export/bg_mountain_sky.png` · 640×360.
- 레이어 PNG: `Layers/Native320/` · 320×180, `Layers/` · 640×360.
- 입력 v002: `Source/bg_mountain_sky_v002.aseprite`.
- 입력 SHA256: `e4014d8d6cc3419fc595258a33eb8a100eb78ad75431142c1ebf5245ba18cd43`.
- 수정 기준 예시: `Reference/cleanup_reference.aseprite`.
- 실제 수정 범위: `Masks/material_regions.aseprite`.
- 단독/숨김 visibility 검수와 판정: `QA/LayerReview/`, `QA/visual_review.md`.

이전 버전은 보존한다. 새 이미지 생성·블러·안티앨리어싱·디더링은 사용하지 않는다. 작은 색 조각·띠·경계를 위치와 재질에 따라 정리하며, 해상도나 색 수만으로 작화 완료를 판단하지 않는다.

## 실제 결과

4개 레이어를 Aseprite 최근접 방식으로 640×360에서 320×180으로 줄였다. 같은 640×360 표시 크기에서 한 도트가 **2×2픽셀**을 차지한다.

축소 뒤 숲의 가까운 명암색을 네 값 그룹으로 묶고, 기존 수관 위치를 따라 10개 면을 형태별로 다시 그렸다. 주봉과 바위 디테일의 보호 영역은 축소 직후 원본과 픽셀 차이 0이다. 구름의 작은 잔여 조각 13픽셀, 두 내리막 경계의 4픽셀을 정리했다. 축소 후 정리로 변경된 합성 도트는 6,783개(11.78%)이며, 영역·색 그룹 정리와 수관 면 작화를 포함한 값이다. 원본 49항목 팔레트를 유지했고 실제 합성 사용 RGB는 37색이다.

가까운 숲의 잔무늬를 줄이면서 화강암 주봉의 큰 면·주요 균열·구도는 유지했다. 모든 식생을 단색 도형으로 바꾼 결과는 아니며, 세밀한 픽셀 일러스트의 기본 방향은 남아 있다.

## 파일과 비교

- [320×180 Aseprite](bg_mountain_sky.aseprite).
- [320×180 PNG](Export/bg_mountain_sky_320.png), [2배 640×360 PNG](Export/bg_mountain_sky.png).
- [v002와 해상도 비교](Previews/resolution_before_after.png): 왼쪽 기존 640×360, 오른쪽 320×180에서 작업한 2배 출력.
- [축소 후 정리 전후](Previews/cleanup_before_after.png): 왼쪽 최근접 축소만 한 상태, 오른쪽 면·경계 정리 후. 두 이미지는 같은 640×360 표시 크기다.
- [레이어별 단독 보기](QA/LayerReview/layers_contact.png): 위 왼쪽 하늘 / 위 오른쪽 구름 / 아래 왼쪽 먼 능선 / 아래 오른쪽 주봉·산림.
- [구름 비교](Previews/layer_before_after_02_clouds.png), [먼 능선 비교](Previews/layer_before_after_03_distant_ridges.png), [주봉·산림 비교](Previews/layer_before_after_04_granite_woodland.png).
- [이동 GIF](Previews/scroll_preview.gif), [조작 HTML](Previews/scroll_preview.html).
- [작화 검수](QA/visual_review.md), [native_validation.json](QA/native_validation.json), [independent_audit.json](QA/independent_audit.json).

## 검수와 범위

정본의 실제 Aseprite visibility를 바꾼 단독 4개·하나만 숨긴 4개 상태를 저장하고 다시 열어 검수했다. Shift+X 키 입력은 사용하지 않았으며 표시 속성을 직접 전환했다.

- 320×180 원본과 PNG 픽셀 일치.
- 640×360 합성과 각 레이어가 네이티브 원본의 정확한 최근접 2배 확대.
- 4개 레이어, 1프레임, duration·팔레트 보존.
- 주봉·먼 능선의 내부 투명 누락 없음, 반투명 알파 없음.
- 이동 GIF 80프레임과 640×360 PNG 순환 이동이 픽셀 단위로 일치.
- v002 원본 SHA256 그대로 유지.

HTML은 정적 코드 문법과 자산 경로를 확인했다. 앱 내 브라우저의 `file:` 정책 때문에 UI 실행 검수는 하지 않았다. Unity 적용과 게임 화면 합성 검수는 이번 범위에 포함하지 않는다.
