# v002 할매 5명 · 업그레이드 목록용 복사본

원본: `Assets/Artsource/Drafts/GrandmaShooter64/v002/Export/`.

| 슬롯 | 역할 | 컬러 원본 | 잠금 원본 |
| --- | --- | --- | --- |
| 0 | 순덕 / 샷건 | grandma_04_sundeok_no_fx.png | 현재 사용하지 않음 |
| 1 | 화방 | grandma_05_flamethrower_no_fx.png | grandma_05_flamethrower_silhouette.png |
| 2 | 기관총 | grandma_06_machinegun_no_fx.png | grandma_06_machinegun_silhouette.png |
| 3 | 저격 | grandma_07_sniper_no_fx.png | grandma_07_sniper_silhouette.png |
| 4 | 오토바이 | grandma_08_motorcycle_no_fx.png | grandma_08_motorcycle_silhouette.png |

원본과 바이트가 같은 PNG를 `GrandmaStageSetup`에서 복사한다. 컬러 복사본은 기본 이름, 잠금 복사본은 `_locked.png`로 저장한다. 전부 64×64이며 Point/Uncompressed/no mipmaps/Sprite Single로 임포트한다. Aseprite 원본·원본 PNG·원본 메타데이터는 수정하지 않는다. 게임용 메타데이터/GUID는 Unity가 생성한다.

HUD의 `upgradePortraits`와 `upgradeLockedPortraits`는 이 5슬롯 순서다. 순덕은 컬러, 다른 4명은 각자의 잠금 실루엣을 표시한다. 네 캐릭터의 해금/성장/전투 기능을 추가했다는 뜻은 아니다. 현재 픽셀은 정적 1프레임이다.
