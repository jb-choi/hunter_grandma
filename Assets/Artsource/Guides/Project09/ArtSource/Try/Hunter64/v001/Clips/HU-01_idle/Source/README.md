# 축소 전 입력 원본

[idle_300_v006.aseprite](idle_300_v006.aseprite)는 [Hunter_300 v006 idle](../../../../../Hunter_300/v006/Clips/HU-01_idle/idle.aseprite)의 보관본이다. 300×300 Indexed·2프레임·각 500ms이며, 기존 17칸 팔레트를 포함한다. 64×64 변환은 이 입력 파일을 기준으로 재현한다.

이 파일은 300×300 입력 기록이다. 64×64 편집 정본은 [상위 idle.aseprite](../idle.aseprite)이며, 원본 보관본을 직접 고치지 않는다. 원래 Hunter_300 v006 파일·보관본의 SHA-256 비교는 [source_hash.json](../../../QA/source_hash.json)에 남긴다.

| 보관 자료 | 크기와 역할 |
|---|---|
| [idle_300_v006.aseprite](idle_300_v006.aseprite) | 300×300 캐릭터·idle 입력 |
| [material_regions_300.aseprite](material_regions_300.aseprite) | 300×300 원본 부위 마스크 |
| [regions_300.json](regions_300.json) | 원본의 재질 ID·보존 범위 좌표 정의 |
| [cleanup_reference_300.aseprite](../../../Reference/Source/cleanup_reference_300.aseprite) | Reference/Source에 보관한 300×300 기존 v005/v006 정리 기준 |

64×64 대응 마스크는 [Masks/material_regions.aseprite](../Masks/material_regions.aseprite), 축소한 기준 예시는 [Reference/cleanup_reference.aseprite](../../../Reference/cleanup_reference.aseprite)다. 원본 자료와 축소 자료를 같은 크기의 편집 정본으로 혼용하지 않는다.

출처는 프로젝트 내부의 Hunter_1254 → Hunter_300 v001~v006 제작 이력이다. 이번 작업에서 외부 이미지를 새로 가져오거나 생성하지 않았다.
