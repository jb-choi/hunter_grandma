# 사냥꾼 300×300 idle — 면·경계 정리 시험

v006 · 2026-10-11 · idle 2프레임 · 각 500ms · 1초 반복

[v005](../v005/README.md)의 눈·국소 경계 수정 뒤에도 깃털, 모자, 장비 등에 남은 작은 색 띠와 불규칙한 돌출을 정리하는 시험이다. 기존 Aseprite 픽셀을 직접 수정하며 전체 디자인·포즈·캐릭터 인상을 유지한다. 새 이미지 생성, 블러, 안티앨리어싱, 디더링은 사용하지 않는다.

300×300 Indexed 캔버스와 기존 팔레트 17칸(불투명 16색 + 투명 1칸), 두 프레임의 타이밍을 작업 기준으로 유지한다. 300px 캔버스 전체를 일률적인 격자로 다시 그리는 작업은 아니다. 경계의 불필요한 작은 턱과 가는 띠는 검정 윤곽이나 투명 영역에 걸쳐 있어도 수정 대상으로 포함한다.

[편집 파일](Clips/HU-01_idle/idle.aseprite) · [비교 재생기](Previews/index.html) · [전신 부위별 비교](Previews/review.html) · [로컬 재생기](http://127.0.0.1:53160/Previews/index.html) · [idle 상세](Clips/HU-01_idle/README.md)

## 작업·확인 기준

| 항목 | 파일 | 확인 목적 |
|---|---|---|
| 정리 기준 예시 | [cleanup_reference.aseprite](Reference/cleanup_reference.aseprite) | 깃털·모자의 수정 전후에서 남길 형태와 정돈할 경계 확인 |
| 부위별 면 구분 | [material_regions.aseprite](Clips/HU-01_idle/Masks/material_regions.aseprite), [regions.json](Clips/HU-01_idle/Masks/regions.json) | 같은 색을 쓰는 서로 다른 부위의 수정 범위 구분 |
| 경계 수정 규칙 | [PixelCleanupGuide.md](../../../../Docs/AI/Art/02_Production/PixelCleanupGuide.md), [이번 버전 명세](Prompts/cleanup_spec.md) | 선 두께·계단 진행·보존 디테일 기준 확인 |
| 전신 시각 검수 | [visual_review.md](QA/visual_review.md) | 두 프레임의 부위별 확대·표시 크기·반복 재생 판정 확인 |

작업 순서는 기준 예시와 마스크 확인 → 부위별 면·경계 정리 → 두 프레임의 전신 시각 검수다. 파일 수치 검사와 시각 검수는 별도로 기록한다. 색 수와 반투명 검사만 통과한 상태를 전체 경계 정리 완료로 기록하지 않는다. 실제 판정과 남은 문제는 `QA/visual_review.md`를 기준으로 확인한다.

## 이번 시험의 실제 처리

13개 부위를 위치와 재질에 따라 구분했다. 각 색 면의 경계에 3×3 정사각형 열기·닫기 처리를 적용해 얇은 돌출과 패임의 후보를 정리하고, 해당 부위에서 허용하는 가까운 색 면으로 픽셀을 합쳤다. 색을 평균 내거나 새 색을 만들지 않았다. 이후 보수적인 모서리 정리 두 차례와 발끝 하이라이트·허리 경계의 개별 수정을 적용했다. 눈·귀 디테일·버클·활줄의 지정 범위는 보존했다. 실제 부위 정의와 수정 위치는 [regions.json](Clips/HU-01_idle/Masks/regions.json), [cleanup_v6.lua](QA/cleanup_v6.lua), [pixel_edits.json](QA/pixel_edits.json)에서 확인한다.

원본은 큰 4~5px 색 블록에 작은 경계 칸이 섞인 그림이다. 이번에는 전체 격자 정렬을 강제하지 않고 기존 큰 블록의 흐름에 맞춰 경계를 정리했다. `QA/Candidates/`의 3·4·5px 격자 후보는 비교용으로 남긴 미채택 시험이며 `idle.aseprite`의 결과가 아니다.

| 파일 수치 | 1번 프레임 | 2번 프레임 |
|---|---:|---:|
| 변경 픽셀 | 694 | 748 |
| 투명/불투명 경계 변경 위치 | 32 | 51 |
| 검정색 마스크 변경 위치 | 346 | 376 |
| 지정 보존 디테일의 불일치 | 0 | 0 |
| 실제 불투명 색 수 | 16 | 16 |
| 반투명 픽셀 | 0 | 0 |

총 1,442픽셀을 수정했다. [파일 검수 기록](QA/file_validation.json), [편집 파일 재검수](QA/native_validation.json), [원본 해시](QA/source_hash.json), [재생기 검수](QA/browser_validation.json)를 함께 보관한다. 변경 수치는 품질 통과 판정이 아니며, 전신 부위별 결과는 시각 검수 기록에 따른다.

저장된 편집 파일·마스크·기준 예시·PNG·GIF를 독립적으로 다시 읽어 파일 검수를 통과했다. 두 프레임의 PNG 픽셀 불일치와 지정 보호 마스크 불일치는 모두 0개다. 원본 보관본의 바이트 일치, 300×300 Indexed·기존 17칸 팔레트, `idle` 태그·2프레임·각 500ms, GIF의 같은 타이밍을 확인했다. 전신 11개 영역의 두 프레임에 대한 작업자 시각 판정은 [visual_review.md](QA/visual_review.md)에 별도로 기록했다.

## 비교할 파일

- [깃털·모자 기준 예시 PNG](Reference/cleanup_reference_before_after.png): 기존 v005와 v006의 같은 영역 비교.
- [전신 전후 PNG](Previews/idle_before_after.png) · [1번 4배 확대](Previews/idle_zoom_01.png) / [2번 4배 확대](Previews/idle_zoom_02.png).
- [11개 부위별 비교](Previews/review.html): 각 이미지에서 왼쪽 v005, 오른쪽 v006, 위쪽 1번 프레임, 아래쪽 2번 프레임. [영역·배율 정의](Previews/Review/regions.json).
- [1번 마스크 미리보기](Previews/material_regions_01.png) / [2번 마스크 미리보기](Previews/material_regions_02.png): 실제 캐릭터 색과 별개의 부위 식별 색.
- [idle GIF](Previews/idle.gif) · [프레임 나란히 보기](Previews/idle_contact.png) · [팔레트 색표](Previews/palette.png).

기준 예시 Aseprite는 `Before_v005`와 `After_v006` 두 레이어로 구성되며 두 프레임을 보관한다. 기본 표시는 `After_v006`이고, 레이어를 전환해 같은 위치의 수정 전후를 확인한다. 마스크 파일은 재질·보존 디테일·수정 경계를 별도 레이어로 구분한다.

## 폴더 체계

```text
Reference/cleanup_reference.aseprite        # 깃털·모자 기준 예시
Clips/HU-01_idle/
  Source/idle_300_v005.aseprite             # 수정 전 원본 보관본
  Masks/material_regions.aseprite          # 부위별 작업 마스크
  Masks/regions.json                       # 마스크 부위 정의
  idle.aseprite                            # v006 편집 파일
  Frames/                                 # 개별 300×300 PNG
  Export/                                 # 시트·타이밍·팔레트
Previews/                                 # 전후 비교·확대·재생기
Prompts/                                  # 적용 지시·버전별 정리 명세
QA/                                       # 수정·검수 기록
```

수정 전 원본은 [Source](Clips/HU-01_idle/Source/README.md)에 보관한다. 마스크의 색은 부위 구분용이며 그림의 팔레트로 사용하지 않는다. [마스크 안내](Clips/HU-01_idle/Masks/README.md) · [지시 안내](Prompts/README.md)

이 버전은 `Try`의 시험본이며 Unity에 적용한 제작 확정본이 아니다.

기존 생성본의 두 프레임 사이에 있던 형태·포즈 차이는 남아 있다. 이번 작업은 그 차이를 새 애니메이션으로 다시 그리는 작업이 아니다.
