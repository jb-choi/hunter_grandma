# HunterVoxel v002 — 왼팔 위치와 다리 배치 수정

2026-10-13. 사용자가 v001을 보존한 다음 파일에서 왼팔의 앞쪽 쏠림과 다리 구성을 정리하도록 요청했다. 아래 도식은 사용자가 **위에서 본 배치**라고 확인했다.

```text
ㅁ  ㅁㅁㅁ
ㅁ
ㅁ
```

현재 편집 파일: [Work/hunter_idle_frame01_voxel.blend](Work/hunter_idle_frame01_voxel.blend).
[전체 전후 비교](Previews/before_after.png) · [다리 위에서 본 전후](Previews/legs_before_after.png) · [실제 발 모양](QA/boots_plan.png) · [GLB](Export/hunter_idle_frame01_voxel.glb).

## 보존

- v001 폴더 전체는 변경하지 않았다. [해시 목록](QA/v001_preserved_hashes.json)과 [재검사](QA/saved_blend_validation.json)의 변경 파일 0건으로 확인했다.
- v001 편집 파일을 [Source/hunter_idle_frame01_voxel_v001.blend](Source/hunter_idle_frame01_voxel_v001.blend)에 별도로 복사했다.
- 작업 시작 당시 실행 중인 Blender 세션도 [Source/hunter_current_session.blend](Source/hunter_current_session.blend)로 보존했다. 당시 원본 세션의 unsaved 상태는 false였다.
- Aseprite 원본과 Unity Assets는 변경하지 않았다.

## 수정

캐릭터 진행 방향은 +X, 캐릭터 기준 왼쪽은 +Y로 해석했다. `04_FarArm_Hand`와 손에 든 `09_Slingshot`을 함께 X -0.4m / Y +0.7m 이동했다. 몸 앞에서 팔이 뻗어나오는 양을 4도트 줄이고 몸의 왼쪽 옆으로 붙였으며, 새총 그립과 손의 상대 위치는 유지했다. 이 두 부위의 실제 메시와 재질은 이전 파일과 동일하고 배치만 바꿨다.

다리·부츠 두 메시를 새로 쌓았다. 위에서 본 도식의 왼쪽 발은 -Y 방향 세로 3칸, 오른쪽 발은 +X 방향 가로 3칸으로 구성했다. 도식 한 칸은 4×4×3개의 기존 정육면체 묶음이다. 양발의 뒷꿈치에 발목·커프·종아리·하체가 연결된다. 좌우는 이 문단에서 **도식 화면 기준**이며, 기존 메시 이름의 Left/Right가 해부학적으로 재정의된 의미는 아니다.

정육면체 한 변 0.0986m와 0.1m 격자는 유지했다. 모델은 총 11,224개 큐브, 10개 부위로 구성된다. 다리 각 440개, 발 부분 각 144개다. 다리 수정은 원본 2D 투영 일치보다 사용자가 지정한 3D 배치를 우선했으므로, v001의 정면 0픽셀 차이 규칙을 v002에는 적용하지 않는다.

깃털, 모자, 얼굴·머리, 몸통·벨트, 반대 팔, 가방의 6개 부위는 메시·재질·위치를 모두 보존했다. [검사 기록](QA/saved_blend_validation.json)에 각각의 실제 저장 데이터 비교 결과가 있다.

## 검수와 범위

[사선 렌더](Previews/three_quarter.png), [정면](Previews/front.png), [반대편](Previews/opposite.png), [두께](Previews/depth.png), [다리만 사선](Previews/legs_three_quarter.png), [다리 위에서 보기](Previews/feet_top.png)를 확인했다. 저장 모델을 독립 재개봉하고 전 10부위를 단독/숨김 상태로 렌더했다.

[시각 검수](QA/visual_review.md) · [저장 메시 검사](QA/saved_blend_validation.json) · [팔 이동](QA/arm_correction.json) · [다리 구성](QA/leg_rebuild.json).

완료 범위: 사용자가 우선 지정한 왼팔·새총 배치와 다리·부츠의 수정 시안. 전체 캐릭터의 다른 부위 디자인 정리, 리깅·애니메이션·게임 적용은 이번 수정에 포함하지 않았다.
