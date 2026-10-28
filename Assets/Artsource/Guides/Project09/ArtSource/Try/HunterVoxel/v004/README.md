# HunterVoxel v004 — 승인한 2D 다방향 시트의 블록 3D

2026-10-13. 사용자가 승인한 `HunterTurnaround2D/v001` 앞·뒤·캐릭터 왼쪽·위 시트를 기준으로 새 정적 3D 모델을 만들었다. 기존 v001~v003은 보존했다.

- 편집 정본: [Work/hunter_turnaround_voxel.blend](Work/hunter_turnaround_voxel.blend)
- 교환 파일: [Export/hunter_turnaround_voxel.glb](Export/hunter_turnaround_voxel.glb)
- [대표 렌더](Previews/hero.png) · [다방향 렌더](Previews/views_contact.png) · [360도 회전](Previews/turntable.gif)
- [승인 2D / 실제 3D 비교](Previews/approved_2d_vs_3d.png) · [검수 기록](QA/visual_review.md)

## 기준과 보존

주 입력은 [승인 시트 보존본](Source/approved_turnaround.png)이다. 원본 `idle_breath.aseprite`는 [Source/](Source/idle_breath.aseprite)에 그대로 보존했으며 계보 참고 자료다. 원본 SHA-256은 `9063c92fba58858babf7ed4a94b59071a3376f355e4b756591884c6be1164151`이다. 입력·현재 Blender 세션의 보존본 해시는 [package_validation.json](QA/package_validation.json)에 기록했다. 제작 시작 당시 Blender의 v003 파일은 [session_before_v004.blend](Source/session_before_v004.blend)에 복사 보존했다.

이번 단계는 승인된 추가 시점의 디자인을 입체로 해석하는 작업이다. 원본 Frame 1과 RGBA 픽셀 차이 0을 만드는 요구의 후속 방식으로, 승인 시트의 앞·뒤·옆·위를 한 모델의 구조에 반영했다. 원본 파일이나 2D 시트는 수정하지 않았다.

## 구조와 배치

Blender 좌표: 전방 `-Y`, 캐릭터 왼쪽 `+X`, 위 `+Z`. 모자·머리·몸통·팔다리·부츠·가방은 타원체, 캡슐, 둥근 상자, 테이퍼 크라운 등의 전체 부피를 먼저 정의하고 동일한 0.09 단위 격자에서 샘플링했다. 원본 알파 실루엣으로 깊이 열을 제한하지 않았다.

- 실제 정육면체 7,606개, 편집 메시 29개. 큐브 한 변 0.08874 단위, 격자 사이에는 블록 표면을 읽기 위한 1.4% 간격이 있다. 작업 단위는 디자인 비율용이며 사람의 실측 크기는 정하지 않았다.
- 왼쪽 어깨·팔꿈치는 몸통 옆에 있고, 아래팔만 앞으로 굽혀 왼손이 새총 손잡이를 잡는다. 오른팔은 옆으로 내렸다.
- 두 다리는 골반 좌우에서 내려오고, 두 부츠는 같은 전방을 향한다. 가방은 오른쪽 뒤 골반에 연결했다.
- 모자의 챙과 크라운, 둘레를 도는 갈색 띠, 오른쪽 깃털, 뒤·옆 머리카락, 목, 옷깃, 벨트와 앞뒤 대각 끈이 실제 부피를 가진다.
- 눈은 얼굴 앞면에 붙은 얇은 검정 블록이다. 새총의 Y자 틈과 고무줄은 실제 메시이며 손 안에서 고정된 35도 방향으로 정면과 왼쪽에서 읽히도록 배치했다.
- 원본의 픽셀 윤곽·그림자를 앞쪽에 투사하지 않고 의미별 단색 재질과 조명으로 표현했다. 초록·피부·머리카락·깃털 계열은 원본 팔레트를 참고했고 가죽 중간색 `#713c27`을 추가했다.

이미지 평면이나 카메라에 따라 바뀌는 별도 모델·셰이더는 없다. Blender 안의 승인 시트는 참고 이미지 자료로만 패킹했다. 모든 렌더는 같은 모델의 카메라 방향을 바꾼 결과다.

## 검수와 현재 범위

저장한 `.blend`를 다시 열어 실제 메시와 큐브 크기를 검사하고, 앞·뒤·양옆·위, 회색 형태 검수, 29개 부위 단독/숨김, 24방향 회전을 확인했다. GLB를 별도 빈 Blender 세션에 다시 가져와 메시 29개와 삼각형 91,272개를 확인했다. 라이브 Blender에는 v004 편집 정본을 열어 두었다.

현재는 정적 디자인 모델이다. 관절 Empty는 숨긴 제작 가이드이며 스키닝 리그·동작은 만들지 않았다. 큐브 내부 면 제거, 게임용 최적화, 교체 부품의 가려진 접합 면 복원, Unity 적용은 이번 범위에 포함하지 않았다. 부위 분할은 현재 자세의 재질·볼륨 소유권에 따른 것이므로, 머리카락이나 팔을 따로 움직이는 단계에서는 접합 면을 보완해야 한다.

## 재현

`Scripts/prepare_v004.py` → `Scripts/build_v004.py` → `Scripts/qa_v004.py` → `Scripts/package_previews.py` 순서다. `qa_glb.py`는 교환 파일 재가져오기 검사다. Blender Store 버전은 `blender-launcher.exe --background --python <절대 스크립트 경로>`로 실행했다. 제작 스크립트를 다시 실행하면 현재 v004가 재생성되므로 수작업 수정 후에는 별도 새 버전으로 실행한다.
