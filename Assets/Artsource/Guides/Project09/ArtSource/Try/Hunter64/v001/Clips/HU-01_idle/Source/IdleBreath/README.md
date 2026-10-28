# idle 호흡 · 사용자 키포즈 보존

2026-10-11 · Hunter64/v001 · HU-01_idle

[idle_user_keypose.aseprite](idle_user_keypose.aseprite)는 사용자가 디자인을 직접 수정한 [idle.aseprite](../../idle.aseprite)의 작업 시작 시점 보관본이다. 입력은 64×64 Indexed, 1프레임, 500ms이며 팔레트는 투명 인덱스 0을 포함한 17칸이다. 이 보관본을 수정 원본으로 덮어쓰지 않는다.

현재 디자인 편집은 `Clips/HU-01_idle/idle.aseprite`, 별도 호흡 애니메이션 편집은 [idle_breath.aseprite](../../idle_breath.aseprite)에서 진행한다. 두 파일의 목적을 구분한다. 보관본의 SHA-256과 작업 후 디자인 원본의 미변경 여부는 [source_hash.json](../../../../QA/IdleBreath/source_hash.json)에 기록한다.

부모 Source의 [idle_300_v006.aseprite](../idle_300_v006.aseprite)는 더 이전의 300→64 초기 축소 입력이다. 이번 애니메이션은 그 300px 두 프레임을 다시 축소하거나 원래 포즈를 복원하지 않는다. 사용자가 수정한 64px 한 포즈를 기준으로 구성한다.

[동작 상세](../../idle_breath.md) · [동작 명세](../../../../Prompts/IdleBreath/motion_spec.md)
