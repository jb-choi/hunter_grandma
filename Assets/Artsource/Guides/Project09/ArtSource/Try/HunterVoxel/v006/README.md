# HunterVoxel v006 — FRONT + 원본 RIGHT 무테 픽셀 배치

2026-10-14 검수 완료(작업 시작 2026-10-13). 사용자가 추가한 **원본 캐릭터의 오른쪽 면** 기준을 반영했다. v005의 FRONT를 유지하고, 원본 `idle_breath.aseprite` Frame 1을 RIGHT 제작 기준으로 추가하여 동일 3D 모델에서 두 정사영을 동시에 맞췄다. 기존 v001~v005는 보존했다.

- 편집 정본: [Work/hunter_front_right.blend](Work/hunter_front_right.blend)
- 교환 파일: [Export/hunter_front_right.glb](Export/hunter_front_right.glb)
- [원본 / 무테 / 실제 오른쪽 렌더 비교](Previews/original_right_vs_3d.png)
- [FRONT 실제 비교](Previews/front_pixel_comparison.png) · [RIGHT 실제 비교](Previews/right_pixel_comparison.png)
- 오른쪽 2D 편집 참조: [원본 좌표의 무테 Aseprite](Reference/right_native_borderless.aseprite), [PNG](Reference/right_native_borderless.png)
- [입체 렌더](Previews/hero.png) · [24방향 회전](Previews/turntable.gif) · [검수 기록](QA/visual_review.md)

## 원본 RIGHT를 사용하는 방법

입력은 [원본 Aseprite 보존본](Source/idle_breath.aseprite)과 그 Frame 1을 네이티브 Aseprite CLI에서 export한 [64×64 PNG](Source/frame_01.png)다. PNG는 이전에 검증한 원본 PNG와 RGBA 차이 0이다. 원본 Aseprite SHA-256은 `9063c92fba58858babf7ed4a94b59071a3376f355e4b756591884c6be1164151`이다.

원본은 오른쪽을 바라보지만, 정면 버클과 두 부츠·양팔이 보이므로 실제 작화 카메라가 순수 90° 측면이었다고 단정하지 않는다. 사용자가 지정한 RIGHT 제작 도안으로 채택했다. 원본을 다른 그림으로 재생성하지 않았다.

원본 캐릭터 bbox `(15,7)–(51,57)`는 36×50셀이고 현재 FRONT는 53셀 높이다. 두 방향의 머리 상단과 접지를 맞추기 위해 원본 bbox를 **NEAREST 38×53**, 작업 캔버스 `(13,10)`에 등록했다. [등록 기록](QA/right_reference_registration.md), [실제 규칙과 부위별 셀](Work/right_reference.json). 이는 약 1.06배의 좌표 등록이며 원본 네이티브 카메라 출력과 직접 동일하다는 의미는 아니다.

원본 윤곽 색 `#110612`은 같은 부위의 기존 색으로 채우고, 검정 눈 3셀과 갈색 버클 구멍은 유지했다. 원본 알파·비윤곽 색·원본 파일은 보존했다. [기준 예시](Reference/cleanup_reference.aseprite)는 등록 원본 / 무테 / 윤곽·의미 디테일 마스크의 3패널 참조다. [부위 마스크](Masks/material_regions.aseprite)는 원본 visible 레이어와 좌표를 이용한 11개 구분이며 게임 export에서 제외한다.

첫 MCP PNG export는 Indexed 팔레트가 검정으로 나와 사용하지 않았다. 네이티브 `Aseprite.exe -b ... --frame-range 0,0 --save-as ...`로 다시 export했고 원본 색 일치를 확인했다. 실패 출력은 진단용 `Source/frame_01_mcp_export_black.png`로만 보존했으며 모델 입력에 사용하지 않았다.

## 동일한 3D 모델에서 두 배치 고정

좌표는 전방 `-Y`, 왼쪽 `+X`, 위 `+Z`. FRONT는 `-Y`에서, RIGHT는 캐릭터의 오른쪽 `-X`에서 본 정사영이다. FRONT의 `(x,z)` 점유와 RIGHT의 `(y,z)` 점유를 동시에 만족하는 부위별 큐브 볼륨을 구성했다. 어느 방향에서만 보이도록 바뀌는 대체 메시나 이미지 평면은 없다.

정육면체 12,988개, 한 변 0.1 작업 단위, semantic 메시 11개다. 큐브의 실제 면에는 고정된 vertex color가 있다. 이미지 텍스처·카메라 노드는 사용하지 않았다.

- FRONT의 셀·색·실루엣은 v005와 동일하다.
- 원본 RIGHT에서 두 발이 수평으로 분리되므로 양다리를 앞뒤 깊이에서 엇갈리게 배치했다. FRONT 좌우 발과 RIGHT 앞뒤 발을 각 다리로 대응시켜 네 개의 발이 생기지 않도록 했다.
- RIGHT의 자유손·새총 손·가방·모자·얼굴 위치가 원본 등록 셀을 따르도록 깊이를 정했다.
- 앞면과 오른쪽에 눈·버클이 독립적으로 중복되는 것을 줄이기 위해 공통 입체 코너를 만들었다. [코너·지지 셀 기록](QA/projection_registration.json).

Blender 기본 카메라는 `Camera_RIGHT`, 64×64 정사영, Workbench FLAT / VERTEX / AA OFF다. `Camera_FRONT`도 같은 정사영 설정을 사용한다. 조명·Cavity·윤곽선 없이 두 방향의 실제 재개봉 렌더가 등록된 무테 참조와 **각각 RGBA 차이 0셀**이다.

[원본 좌표 비교](Previews/original_right_vs_3d.png)의 마지막 패널은 실제 재개봉 RIGHT 렌더를 크롭한 뒤 NEAREST로 36×50에 되돌려 원본 `(15,7)`에 배치한 출력이다. 이 **등록 역변환 결과**와 원본 좌표의 무테 참조도 차이 0픽셀이다. [정확한 역변환·검사](QA/right_native_coordinate_validation.json). 원본 좌표의 카메라가 직접 이 PNG를 렌더했다는 뜻으로 확대해 보고하지 않는다.

## 검수 범위와 남은 부분

저장된 메시 재개봉, FRONT/RIGHT 재렌더, 11개 부위 단독·숨김, 24방향 회전, 양발 위 배치, GLB 재가져오기와 vertex color 보존을 확인했다. 원본 방향의 무테 Aseprite 참조도 재export 검사했다. 라이브 Blender에는 v006의 RIGHT 기본 뷰를 열어 두었다.

원본 RIGHT와 기존 생성 LEFT는 서로 반전되는 실루엣이 아니다. 이번에는 사용자가 추가한 FRONT+RIGHT를 우선했으며 LEFT·TOP 전체 일치는 완료하지 않았다. 현재 참조별 실제 RGBA 차이는 FRONT 0 / RIGHT 0 / BACK 47 / LEFT 993 / TOP 1,146셀이다. [전체 비교 수치](QA/projection_pixel_comparison.json).

현재는 정적 3D 도안 대응 실험이다. 자유 회전에서의 깃털·챙·새총의 면 연결과 재질 이음은 추가 입체 작화 정리가 필요하다. 2D 투영 검수 통과를 입체 디자인 완성·리깅·게임 성능 검수로 확대하지 않는다. 애니메이션·최적화·Unity 적용은 수행하지 않았다.

## 재현

`prepare_right_reference.py` → `prepare_views_and_volume.py` → `build_v006.py` → `compare_projections.py` → `qa_saved.py` → `package_qa.py` → `native_right_comparison.py`. `qa_glb.py`는 교환 파일 검사다. 제작 스크립트는 현재 v006를 다시 만들므로 수작업 편집 뒤에는 새 버전에서 실행한다.

