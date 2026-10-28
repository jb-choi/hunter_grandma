# HunterVoxel v003 — 실제 3D 구조와 원본 Frame 1 투영

2026-10-13. 사용자의 요청에 따라 v001/v002를 보존하고, 실제 캐릭터의 방향·좌우·관절을 기준으로 v003을 새로 구성했다. 2D 기준은 사용자가 확인한 **원본 `idle_breath.aseprite` Frame 1**이다.

편집 파일: [Work/hunter_idle_frame01_voxel.blend](Work/hunter_idle_frame01_voxel.blend).
[회전 보기](Previews/turntable.gif) · [여러 방향](Previews/views_contact.png) · [원본과 실제 2D 렌더 비교](Previews/original_vs_3d_projection.png) · [GLB](Export/hunter_idle_frame01_voxel.glb).

## Blender에서 보기

- 기본 장면 `Hunter v003 • Anatomical 3D / Frame 1 projection`: 입체 모델, 조명, 7개 카메라. 기본 카메라는 `Camera_01_ThreeQuarter`다.
- 장면 `VIEW_2D • Original Frame 1 (same model)`: 원본 방향의 정사영 카메라, 64×64, Flat 재질 표시, 조명·안티앨리어싱·디더링 제외, 투명 배경. 이 장면을 선택해 렌더하면 원본 F1과 같은 PNG가 나온다.
- 두 장면은 **동일한 10개 메시 오브젝트와 컬렉션을 공유**한다. 어느 장면에서 메시를 편집해도 다른 장면에 적용된다.
- `GUIDES • Anatomical joints (hidden)`에는 어깨·팔꿈치·손·골반·무릎·발목의 위치 안내 Empty가 있다. 완성 리깅이나 애니메이션이 아니다.

## 3D 구성

캐릭터의 전방은 `(0.8, -0.6, 0)`, 왼쪽은 `(0.6, 0.8, 0)`이다. 원본을 보는 카메라는 세계 좌표 -Y 방향에 있어, 캐릭터의 3D 좌우 배치가 원본 이미지의 좌우 위치로 투영된다.

- 머리·귀·코는 입체 덩어리, 모자는 원뿔형 크라운과 기울어진 타원형 챙으로 구성했다. 모자 띠와 뒷머리 재질도 뒤로 이어진다.
- 몸통·골반을 분리된 입체 중심으로 잡고, 양팔은 각각 어깨→팔꿈치→손으로 이어지는 볼륨으로 구성했다.
- 두 다리를 골반의 실제 좌우에 배치하고 무릎·발목·부츠로 연결했다. 양발은 캐릭터의 공통 전방을 향한다. v002의 화면 도식 배치보다 이번의 실제 3D 구성 요구를 우선한 새 해석이다.
- 새총은 캐릭터 좌우축을 따르는 실제 두께를 가진 Y자 장비이며 왼손과 연결된다. 가방은 후방에 놓고 부착 끈을 연결했다.
- 메시 10개, 정육면체 10,906개. 격자 간격 0.1m, 한 변 0.0986m. 큐브 면을 메시로 묶은 편집용 볼륨 모델이다. 내부 면을 유지하므로 게임용 최적화 메시가 아니다.

입체는 타원체·관절을 잇는 캡슐·방향이 있는 부츠 등의 공간 형태를 격자에 샘플링했다. 원본 정사영 실루엣의 범위 안에서 형태를 제한하고, 외곽선·머리카락·깃털 등의 경계 디테일 102픽셀은 가장 가까운 입체 부위에 실제 큐브로 연결했다.

원본의 색은 기준 카메라에서 보이는 **실제 큐브의 -Y 면**에 팔레트 재질로 기록했다. 옆면과 뒷면은 머리·머리카락·의상·끈·장비의 재질을 사용한다. 이미지 평면, 빌보드, 카메라에 따라 바뀌는 셰이더, 별도의 숨겨진 2D 모델은 없다.

## 원본 대응과 검수

저장된 Blender 파일을 독립 재개봉하고 `VIEW_2D` 장면을 다시 렌더했다. [실제 출력](QA/frame1_reopened_render.png)은 원본 64×64 RGBA와 **차이 0픽셀**이다. 이것은 수학적 배치 자료만 비교한 값이 아니라 실제 저장 메시를 Blender에서 렌더한 결과다.

3D 조명 렌더는 명암·그림자가 있으므로 원본 PNG의 색과 픽셀 단위로 같지는 않다. 원본 일치는 지정한 정사영·Flat 표시 조건의 2D 장면에 적용한다. 다른 방향의 모습과 깊이는 한 장의 2D 입력에서 추정한 디자인이다.

[저장 파일 검사](QA/saved_blend_validation.json) · [시각 검수](QA/visual_review.md) · [단독 10부위](QA/solo_contact.png) · [한 부위씩 숨김](QA/hidden_contact.png) · [24방향 회전 확인](QA/turntable_contact.png).

실제 저장 큐브의 동일 치수, 전체 격자 연결, 팔다리의 해부학적 좌우 위치, 두 장면의 동일 메시 공유를 확인했다. 다리·부츠의 전방 중심은 발목보다 각각 약 0.074m / 0.169m 앞에 있다. v001/v002 폴더의 [보존 해시](QA/prior_version_hashes.json)는 모두 동일했다.

## 입력·이전 버전 보존

- [Source/idle_breath.aseprite](Source/idle_breath.aseprite), [Frame 1 PNG](Source/frame_01.png), [원본 팔레트·레이어 픽셀](Source/frame01_pixels.json).
- 입력 SHA-256: `9063c92fba58858babf7ed4a94b59071a3376f355e4b756591884c6be1164151`.
- [v002 파일 복사본](Source/hunter_v002.blend), [작업 시작 당시 Blender 세션](Source/hunter_current_session.blend). 시작 당시 unsaved 변경은 없었다.
- v001/v002 원본, Aseprite 원본, Unity Assets는 변경하지 않았다.

재현: [prepare_anatomy.py](Scripts/prepare_anatomy.py) → [build_v003.py](Scripts/build_v003.py) → [add_reference_scene.py](Scripts/add_reference_scene.py). [verify_saved_v003.py](Scripts/verify_saved_v003.py)는 실제 저장 파일을 재개봉해 메시와 2D 렌더를 검사한다.

완료 범위: 실제 부위·관절 위치를 갖춘 정적 3D 블록 시안, 원본 F1과 일치하는 공유 모델 2D 장면, 교환 파일과 회전·시각 검수. 리깅·호흡 애니메이션·게임 메시 최적화·Unity 적용은 포함하지 않았다.
