# AD-01 팔레트·원본 색 대조

검증일: 2026-10-11 · 방법: GPL RGB 행 대조, PNG 비투명 픽셀 RGB/알파와 경계 실측.

**기존 32색 GPL을 확인했다. AD-01의 18색 중 4색이 같고 14색은 추가 후보다.** 기존 GPL과 실제 캐릭터 사용색도 완전히 일치하지 않으므로, GPL만 기준으로 기존 그림을 재색칠하지 않는다.

## 1. 대조한 파일

- 원본 기준: `Assets/Art/Sprites/Animals/woodland_animals.gpl` — 32행, 고유 RGB 32개.
- 기존 시안: [hunting_idle_AD-01_reference_palette.gpl](hunting_idle_AD-01_reference_palette.gpl) — 18행, 고유 RGB 18개.
- 실제 그림: `Assets/Art/Sprites/Hunters/hunter.png`, `Assets/Art/Sprites/Animals/{rabbit,boar,deer,magpie,tiger,fox}.png`, `animals_32x32.png`.

경로는 Unity 프로젝트 루트 `Hunting_idle/` 기준이다. 파일 변경 없이 대조했으며, 「기존」은 원본 보존 기준이고 「후보」는 아직 채택하지 않은 색이다.

## 2. 기존 GPL 32색

| 번호 | 원본 이름 | HEX |
| ---: | --- | --- |
| 1 | Outline | `#18141F` |
| 2 | Deep Shadow | `#271F2B` |
| 3 | Warm Shadow | `#392B30` |
| 4 | Brown Shadow | `#4E352E` |
| 5 | Brown Dark | `#643E2D` |
| 6 | Brown Mid | `#7E4930` |
| 7 | Brown Light | `#9D5B34` |
| 8 | Tawny | `#BE7443` |
| 9 | Warm Highlight | `#D78B4E` |
| 10 | Tan Highlight | `#EBA863` |
| 11 | Pale Tan | `#FAC780` |
| 12 | Cream | `#FFDEA4` |
| 13 | Ivory | `#F7EACA` |
| 14 | Soft White | `#FFF7E0` |
| 15 | Tiger Shadow | `#C4501E` |
| 16 | Tiger Orange | `#E76518` |
| 17 | Tiger Light | `#F78922` |
| 18 | Tiger Highlight | `#FFB14A` |
| 19 | Nose Dark | `#6F2B28` |
| 20 | Nose Mid | `#9D4034` |
| 21 | Ear Pink | `#CD664E` |
| 22 | Ear Light | `#EA9170` |
| 23 | Blue Black | `#2A3141` |
| 24 | Feather Dark | `#1C273B` |
| 25 | Feather Blue Shadow | `#1E3D5B` |
| 26 | Feather Blue | `#185277` |
| 27 | Feather Teal | `#116974` |
| 28 | Feather Teal Light | `#2D777E` |
| 29 | Cool Gray | `#6B7F93` |
| 30 | Light Gray | `#A4B1BB` |
| 31 | Feather White | `#DCE2E0` |
| 32 | Eye Highlight | `#FFFFFF` |

## 3. AD-01 18색의 출처와 채택 상태

| 시안 이름 | HEX | 출처 | 원본 GPL 포함 | 현재 상태 |
| --- | --- | --- | --- | --- |
| OUTLINE | `#18141F` | 기존색 | 있음 | 원본 기준 |
| IVORY | `#F7EACA` | 기존색 | 있음 | 원본 기준 |
| TIGER | `#E76518` | 기존색 | 있음 | 원본 기준 |
| MAGPIE | `#116974` | 기존색 | 있음 | 원본 기준 |
| FOREST | `#3F614B` | Notion 숲 참고색 | 없음 | 신규 숲 후보 |
| LEAF | `#78915B` | Notion 잎 참고색 | 없음 | 신규 잎 후보 |
| WOOD | `#AD7E58` | Notion 나무 참고색 | 없음 | 신규 나무/UI 후보 |
| PAPER | `#EFE5C8` | Notion 종이 참고색 | 없음 | 신규 패널 후보 |
| GOLD | `#ECC96D` | Notion 보상 참고색 | 없음 | 신규 보상 후보 |
| forest_shadow | `#2B4438` | AD-01 제안 | 없음 | 숲 명암 후보 |
| leaf_highlight | `#AABF7A` | AD-01 제안 | 없음 | 잎 명암 후보 |
| wood_shadow | `#76543E` | AD-01 제안 | 없음 | 나무 명암 후보 |
| wood_light | `#D2A47A` | AD-01 제안 | 없음 | 나무 명암 후보 |
| ivory_shadow | `#CEC1A0` | AD-01 제안 | 없음 | 기존 밝은색과 우선 비교 |
| tiger_shadow | `#AC421A` | AD-01 제안 | 없음 | 기존 `#C4501E` 먼저 검토 |
| tiger_light | `#F4A957` | AD-01 제안 | 없음 | 기존 `#F78922` / `#FFB14A` 먼저 검토 |
| magpie_shadow | `#0C4553` | AD-01 제안 | 없음 | 기존 `#1E3D5B` 등 먼저 검토 |
| magpie_light | `#4C9EA6` | AD-01 제안 | 없음 | 기존 `#2D777E` 먼저 검토 |

32색과 18색의 합집합은 46색이다. 이는 파일 대조 결과이며 최종 팔레트 수량을 46색으로 정한 것은 아니다. 보조 버튼의 보라색은 기존 AD-01 GPL에 없고 HEX도 정해져 있지 않다.

## 4. PNG 실사용색과 투명 경계

| PNG | 원본 캔버스 | 비투명 경계 시작 (x,y) | 경계 크기 | 사용 RGB 수 | GPL 밖 색 |
| --- | --- | --- | --- | ---: | --- |
| hunter | 32×32 | (5,1) | 25×29 | 7 | `#536D45`, `#91AA63` |
| rabbit | 32×32 | (6,7) | 21×24 | 5 | `#A05D3A`, `#CF864D`, `#D77A5D`, `#FDE4B4` |
| boar | 32×32 | (4,10) | 25×15 | 9 | 없음 |
| deer | 32×32 | (8,4) | 20×24 | 15 | 없음 |
| magpie | 32×32 | (3,10) | 26×15 | 13 | 없음 |
| tiger | 32×32 | (3,8) | 26×18 | 18 | 없음 |
| fox | 32×32 | (2,14) | 28×16 | 6 | 없음 |
| animals_32x32 | 192×32 | (6,4) | 184×27 | 35 | 토끼와 동일한 4색 |

좌표는 PNG 좌상단 원점이며 x는 오른쪽, y는 아래쪽이다. 경계는 알파가 0보다 큰 픽셀의 포함 범위다. **Unity importer의 Sprite rect와 별도 값**이다. 원본 PNG 전체 크기, Unity slice 크기, 실제 비투명 높이를 같은 숫자로 취급하지 않는다.

대조한 8개 PNG 모두 `0 < alpha < 255`인 경계 픽셀이 0개다. 기존 캐릭터 외곽의 하드 에지를 신규 프레임에서도 유지한다.

## 5. 새 색을 추가할 때

1. 원본 PNG의 해당 재질과 기존 GPL의 가까운 색을 먼저 확인한다.
2. 후보색을 실제 사냥꾼·동물·배경/UI 합성본에서 비교한다.
3. 필요한 색만 `HEX / 이름 / 재질·용도 / 출처 / 검수 시안 버전 / 채택 상태`로 아래 표에 기록한다.
4. 최종 채택 목록은 별도 새 GPL로 내보낸다. 기존 `woodland_animals.gpl`과 v0.1 후보 GPL은 보존한다.

| HEX/이름 | 재질·용도 | 출처 | 검수 시안 | 채택 상태 |
| --- | --- | --- | --- | --- |
| `#AABF7A` / leaf_highlight | 사냥꾼 모자/옷 하이라이트 | AD-01 후보 | HU-BASE v001 | 해당 자산 사용, 공통 팔레트 미채택 |

2026-10-11 [사냥꾼 베이스 리디자인](../../02_Production/HU-BASE_HunterRedesign/README.md) v001에서 위 후보를 사용했다. v001은 피드백 이후 보존본으로 남겼다. 현재 v002는 생성 시안의 인상을 유지하기 위한 자산 전용 14색을 사용하며, 색 목록은 해당 Production 메타/GPL에 기록했다. 전체 공통 팔레트의 채택을 의미하지 않으며 기존 GPL은 변경하지 않았다.

## 6. Import와 현재 연결 상태

현재 `HuntConfiguration.asset`은 사냥꾼/동물 **PNG**를 참조한다. Aseprite 원본과 PNG의 Unity GUID는 서로 다르므로 파일/참조를 자동 교체하지 않는다.

기존 주요 PNG 메타는 `filterMode: 1`, `textureCompression: 1`, MipMap off, PPU100이다. 런타임 `HuntStage`와 `PixelUi`가 Point로 설정한다. 따라서 현재 PNG importer가 모두 Point/무압축이라고 기록하지 않는다. 신규 export의 Point·무압축·MipMap off는 제작/통합 시 적용할 제안 규칙이다.

본 문서는 이미지·메타·GPL을 수정하지 않은 대조 기록이다. v0.1 규칙 이미지의 문구를 읽을 때 이 결과와 [전체 제작 방향](../ArtDirection.md)을 함께 적용한다.
