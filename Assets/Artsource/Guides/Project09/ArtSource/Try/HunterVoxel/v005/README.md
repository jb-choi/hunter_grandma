# HunterVoxel v005 — 정면 도안 픽셀 배치를 고정한 무테 3D

2026-10-13. v004가 도안의 픽셀 배치보다 입체 형태 해석을 앞세웠다는 사용자 정정에 따라, 승인된 2D 시트의 FRONT를 직접 셀 좌표로 옮기고 **그 무테 배치가 실제 3D 정사영에서도 유지되도록** 새 모델을 만들었다. 기존 v001~v004와 원본은 보존했다.

- 편집 정본: [Work/hunter_pixel_layout.blend](Work/hunter_pixel_layout.blend)
- 교환 파일: [Export/hunter_pixel_layout.glb](Export/hunter_pixel_layout.glb)
- 핵심 검수: [무테 도안 / 실제 3D 정면 렌더 / 차이](Previews/front_pixel_comparison.png)
- [정면 렌더 8배](Previews/front_flat_8x.png) · [입체 렌더](Previews/hero.png) · [회전](Previews/turntable.gif)
- [검수 기록](QA/visual_review.md) · [도안 시점 간 모순 분석](QA/projection_feasibility.md)

## 기준·무테 처리

주 입력은 보존된 [승인 시트](Source/approved_turnaround.png). 이는 1254×1254의 생성 이미지이며 정확한 정규 픽셀 격자 파일은 아니다. 원본의 주요 계단·눈·부츠 경계에서 약 9.8px 도트 간격을 읽고 FRONT를 **64×64 작업 캔버스에 정규화**했다. 실제 캐릭터는 이 캔버스 일부를 차지한다. 1254×1254 원시 이미지의 모든 픽셀과 일치한다는 의미가 아니다.

FRONT 셀 샘플 위치는 전역 시작점 `(40.5,-16.85)`, 간격 `(9.775,9.725)`px이다. 3×3 내부 샘플의 중간값을 셀 대표색으로 사용했다. [실제 입력·샘플 규칙](Work/projection_targets.json)에 ROI와 보존 눈/버클 좌표를 기록했다.

검정 윤곽 색을 주변의 기존 부위 색으로 채워 **도안의 실루엣과 셀 위치는 유지**했다. 눈과 버클 구멍은 따로 보존했다. 새 그림을 생성하거나 원본 파일을 수정하지 않았다. [무테 전후 기준](Reference/cleanup_reference.aseprite)은 왼쪽 원래 샘플 / 가운데 무테 / 오른쪽 마스크의 192×64 참조 파일이다. 마스크의 분홍은 제거한 윤곽 색, 청록은 보존 의미 디테일이다. [부위 마스크](Masks/material_regions.aseprite)와 [범례](Masks/region_legend.json)는 실제 볼륨 분할과 대응한다. 두 Aseprite 가이드는 게임 export에서 제외한다.

원본 `idle_breath.aseprite`는 계보 자료로 [보존](Source/idle_breath.aseprite)했고 SHA-256 `9063c92fba58858babf7ed4a94b59071a3376f355e4b756591884c6be1164151`이다. 승인 시트 SHA-256은 `6f0f81fd1bd6d6414b93e38aa7cf9cc9d0d0fee9bf41c5a17cef9c514bbcf618`이다. 작업 시작 당시 Blender v004 세션도 [별도 복사](Source/session_before_v005.blend)했다.

## 3D 구조·검수 방법

전방 `-Y`, 왼쪽 `+X`, 위 `+Z`. FRONT의 모든 불투명 셀을 하드 제약으로 유지하면서 LEFT의 해당 부위 깊이, TOP 모자 발자국을 사용해 전체 볼륨을 만들었다. 얼굴·몸의 가로 단면 곡률은 이 제약 안에서 보완하며, 정면 셀을 둥근 머리 근사로 옮기지 않았다. 작은 깊이 분리 요소는 기존 FRONT 셀 안의 지지 큐브로 이어 주었다.

정육면체 10,967개, 한 변 0.1 작업 단위, semantic 메시 11개다. 부위는 깃털 / 크라운 / 챙 / 머리 / 몸통·벨트·끈 / 양팔 / 양다리·부츠 / 가방 / 새총이다. 단위는 디자인 비율용이고 인체 실측 크기는 정하지 않았다.

색은 실제 큐브 면의 vertex color다. 전면·후면·측면에 고정된 색을 가진 **동일한 하나의 3D 모델**이다. 이미지 평면, 이미지 텍스처, 카메라 노드, 시점별 대체 메시를 사용하지 않았다. 서로 다른 시점에 독립 눈을 그려 세 눈처럼 보이지 않도록, 실제 전면 눈 큐브의 인접 면으로 눈을 연결했다.

저장 기본값은 `Camera_FRONT`, 정사영 6.4, 64×64, Workbench FLAT / VERTEX / AA OFF다. 윤곽·그림자·Cavity를 끈 실제 3D 렌더가 [무테 FRONT 셀](Reference/front_borderless.png)과 RGBA 차이 **0셀**이다. 정본을 다시 열어 렌더한 결과도 0셀이다. 조명이 있는 입체 렌더까지 같은 색이라는 뜻은 아니다.

## 시점 간 차이와 현재 상태

승인한 네 도안은 하나의 정사영 물체로 완전히 호환되지 않는다. 원시 FRONT/BACK 반전 실루엣도 약 4.34% 다르고, LEFT 높이와 TOP 크기도 다르다. 따라서 FRONT를 우선하고, 후면의 기존 공통 셀 색과 측면 깊이를 최대한 맞췄다.

현재 정규화 참조와의 실제 렌더 차이:

| 시점 | 실루엣 차이 | 전체 RGBA 차이 | 상태 |
| --- | ---: | ---: | --- |
| FRONT | 0셀 | 0셀 | 요청한 정면 픽셀 배치 검수 통과 |
| BACK | 47셀 | 47셀 | 반대 투영 실루엣 모순 남음 |
| LEFT | 26셀 | 32셀 | 깊이 조정 및 물리 눈 연결에 따른 차이 남음 |
| TOP | 327셀 | 951셀 | 원시 작업 캔버스 기준. 모자 평면을 앞·옆 축에 등록했고 윗면 일부 색은 3D 재질로 해석하여 완전 일치하지 않음 |

TOP까지 도안 일치 완료라고 보고하지 않는다. [각 시점의 실제 비교 수치](QA/projection_pixel_comparison.json)와 차이 이미지를 함께 남겼다. 현재 결과는 **정면 배치 수정이 검증된 정적 3D 실험**이다. 자유 회전 시 면 색의 이음과 챙·얼굴의 계단은 추가 입체 작화 정리가 필요하다. 리깅·애니메이션·게임용 최적화·Unity 적용은 수행하지 않았다.

## 재현

`Scripts/prepare_targets.py` → `prepare_voxels.py` → `build_v005.py` → `compare_projections.py` → `qa_saved.py` → `package_qa.py`. `qa_glb.py`는 교환 파일 재가져오기 검사다. 제작 스크립트는 현재 v005를 다시 생성하므로 수작업 수정 후에는 새 버전에서 실행한다.
