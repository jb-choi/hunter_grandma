# 2026-10-14 hunting_idle 컨셉북

Notion: https://app.notion.com/p/3f8f3731816f810a8c86e895cae831ab

주변 사람에게 현재 방향을 보여주기 위한 기존 아트 모음이다. 기존 아트 정본과 Unity Assets는 수정하지 않았다.

- `scene_01_hunting_concept.gif`: MountainSkyLoop v004, Hunter64 idle, 동물 3종의 위치 이동·좌우 반전, 실제 Unity UI 버튼을 합성했다. 단색 잔디이며 동물의 신규 보행 작화는 아니다.
- `scene_02_cabin_concept.png`: 동일한 원경과 헌터, Loghouse v004의 오두막·표시 소품, 실제 Unity UI 버튼을 합성한 정지 시안이다. 숨긴 참조와 가이드 레이어는 제외했다.
- `scene_03_current_unity.png`: 당일 Unity 6000.3.10f1에서 실행한 HuntingPrototype Game view를 윈도 캡처에서 크롭했다. 원본은 `unity_editor_2026-10-14.png`다.
- `01_idle.gif`–`08_load.gif`: 기존 64×64 동작의 표시용 카드. 원본 프레임과 시간을 유지하며, 단발·유지 동작은 소개용으로 종료 지연을 추가해 반복한다. `hunter_8_actions.gif`는 8개를 동시에 보여준다.
- `blender_diagonal_try_v006.png`: 최신 v006 대각선 렌더를 그대로 보존했다. 연결·면 깨짐을 수정하거나 완성품으로 소개하지 않았다.
- `animals_sprite_gallery.png`: 현행 32×32 동물 6종의 최근접 확대 시트다.

재현: 번들 Python으로 `build_conceptbook.py` 실행. Pillow는 기존 파일의 합성·위치 이동·최근접 확대와 GIF 포장에만 사용했다. GIF 공유 팔레트, 디더 없음, 원본 작화 변경 없음. Unity 캡처는 재현 스크립트에 포함되지 않는다.

출처 SHA-256, 원본 프레임 수·시간, 출력 규격은 `manifest.json`에 있다. SaveBeforeCapture는 캡처 전 사용자 진행 파일 2개의 보존본이며 Notion에 업로드하지 않았다.
