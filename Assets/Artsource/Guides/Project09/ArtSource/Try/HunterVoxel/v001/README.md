# HunterVoxel v001 — idle_breath Frame 1

2026-10-13. `Hunter64/v001/Clips/HU-01_idle/idle_breath.aseprite`의 **첫 프레임**을 동일 크기의 정육면체를 쌓은 Blender 모델로 구현했다.

편집 정본: [Work/hunter_idle_frame01_voxel.blend](Work/hunter_idle_frame01_voxel.blend).
교환용 출력: [Export/hunter_idle_frame01_voxel.glb](Export/hunter_idle_frame01_voxel.glb).
전체 렌더: [사선](Previews/three_quarter.png), [원본 투영 방향](Previews/front.png), [반대편](Previews/opposite.png), [두께](Previews/depth.png).

## 입력과 보존

- 원본은 [Source/idle_breath.aseprite](Source/idle_breath.aseprite)에 바이트 그대로 보존했다. SHA-256: `9063c92fba58858babf7ed4a94b59071a3376f355e4b756591884c6be1164151`.
- 64×64 Indexed, 5프레임 중 F1만 사용했다. 호흡 애니메이션과 다른 프레임은 이번 모델 제작 범위가 아니다.
- 표시한 원본 레이어: `Slingshot&bag`, `LowerBody`, `UpperBody_Arms_Equipment`, `Head_Hat_Hair_Face`. 숨겨진 이전 장비 `Bow&Quiver`는 모델에 넣지 않았다.
- [첫 프레임 PNG](Source/frame_01.png), [팔레트·레이어 픽셀](Source/frame01_pixels.json), [원본 레이어 단독 비교](QA/source_layers.png)를 보존했다. Aseprite 원본과 Unity Assets는 수정하지 않았다.

## 블록 구성과 좌표

11,472개 정육면체를 깃털 / 모자 / 머리·얼굴 / 몸통·벨트 / 양팔·손 / 양다리·부츠 / 가방 / 새총의 10개 메시로 구분했다. 각 메시의 연결되지 않은 면 섬 하나가 정육면체 하나이므로 Edit Mode에서 큐브 단위로 선택·편집할 수 있다.

격자 간격은 0.1m, 큐브 한 변은 0.0986m다. 작은 틈으로 블록의 경계를 읽을 수 있게 했다. 91,776 vertices / 68,832 quad faces이며, 개별 큐브의 안쪽 면도 유지한 편집용 시안이다.

- Aseprite x → Blender X, Aseprite 아래 방향 y → Blender -Z, 깊이 → Blender Y.
- 발바닥은 Z≈0, 모델 높이는 약 5m다. 게임 적용 크기가 아니라 편집·검수용 단위다.
- 머리 최대 17칸, 모자 21칸, 몸통 13칸, 팔 7칸, 다리 8칸, 가방 9칸, 깃털·새총 3칸의 깊이를 사용했다. 테두리로 갈수록 계단 형태로 얇아지게 했다.
- 원본 방향은 Y 음의 방향에서 바라보는 `Camera_02_OriginalProjection`이다. `Camera_01_ThreeQuarter`가 기본 카메라다.
- 원본 1,082개 불투명 픽셀의 실루엣과 전면 팔레트 인덱스를 보존했다. 저장 메시를 독립 재개봉해 복원한 정면 투영의 색·알파 차이는 0픽셀이다. 렌더 이미지는 3D 조명·그림자·큐브 틈 때문에 PNG의 색과 픽셀 단위로 같지는 않다.
- 한 장의 2D 입력에서 알 수 없는 **깊이와 뒷면은 추정 구성**이다. 내부·옆면 윤곽색은 같은 부위의 인접 재질색으로 채웠다. 원본에서 안 보이는 다른 시점의 정본 디자인이 있다는 의미는 아니다.

## 재현과 검수

[Scripts/extract_frame01.lua](Scripts/extract_frame01.lua)로 Indexed 셀을 원본 팔레트와 함께 읽고, [prepare_voxels.py](Scripts/prepare_voxels.py)로 블록 배치를 생성했다. [build_blend.py](Scripts/build_blend.py)는 Blender 모델과 조명·카메라·4방향 렌더를 만든다. [verify_saved_blend.py](Scripts/verify_saved_blend.py)는 저장 파일을 다시 열어 실제 메시 큐브 치수를 확인하고 GLB와 10개 부위 단독/숨김 이미지를 출력한다. [finish_qa.py](Scripts/finish_qa.py)는 저장 메시의 정면 투영과 원본 PNG를 비교한다.

[최신 시각 검수](QA/visual_review.md) · [저장 파일 검사](QA/saved_blend_validation.json) · [부위 단독](QA/solo_contact.png) · [한 부위씩 숨김](QA/hidden_contact.png).

완료 범위: Frame 1 정적 Blender 시안, 편집 파일, GLB, 렌더 및 재개봉 검수. 리깅·호흡 애니메이션·게임 메시 최적화·Unity 연결은 이번 요청에 포함하지 않았다.
