# 아트 제작물 저장·납품 규칙

v0.2 · 2026-10-11 · 신규 작업부터 적용할 기준

**문서/검수용 시안은 `Docs/AI/Art`, 새 편집 원본은 `ArtSource`, 게임용 출력은 `Assets/Art/Sprites`에 저장한다.** 현재 Unity에서 사용하는 원본과 `.meta`는 기존 위치에 유지한다.

공통 작업 순서·입력 보존·레이어 단독/숨김·전 프레임 QA와 README/검수 양식은 [ArtworkGuide.md](ArtworkGuide.md)를 함께 적용한다.

## 1. 저장 위치를 선택하는 기준

| 파일 종류 | 경로 | 규칙 |
| --- | --- | --- |
| 전체 방향·제작 지시·검수 기록 | `Docs/AI/Art/01_ArtDirection/` | 공통 기준과 AD별 README |
| AD-01~05 문서용 시트/합성본 | `Docs/AI/Art/01_ArtDirection/<AD-ID_작업명>/` | 버전 있는 PNG, 문서 캡션 허용 |
| 현재 구현/외부 참고 | `Docs/AI/Art/03_References/` | 출처·날짜·용도, 완성 자산과 구별 |
| 선택 전 편집 원본·생성 후보 | `ArtSource/Drafts/<작업ID>/v001/` | 후보 버전마다 분리, README에 선택 결과 |
| 검수 후 신규 편집 정본 | `ArtSource/Production/<Category>/<자산ID>/` | 해당 자산의 편집 정본 1개를 명시 |
| 게임 전달 전 export | 위 정본 폴더의 `Export/` | PNG/JSON은 재생성 가능한 출력 |
| 반복 비교/확대 미리보기 | `ArtSource/Previews/<작업ID>/` | 기존 Previews 재사용 |
| 원본 변경 전 보존본 | `ArtSource/Backups/<작업ID>/<날짜_버전>/` | 기존 Backups 재사용, 정본과 구별 |
| 사용자 요청 비교 실험 | `ArtSource/Try/<실험명>/v001/` | 원본·프레임·시트·재생·프롬프트·QA를 한 루트에 보존, 정식 해상도 변경과 구별 |
| Unity용 출력 | `Assets/Art/Sprites/<Category>/` | 연결에 필요한 PNG/시트/메타데이터만 |

`Category`는 `Hunters / Animals / Environment / Props / UI / Icons / VFX`를 사용한다. 애니메이션 Unity 출력은 `Animations/Hunters` 또는 `Animations/Animals/<species>`로 분리한다. 작업 ID는 기존 Notion의 `AD / HU / AN / PR / BG / UI / IC / FX`를 그대로 사용한다.

기존 `Assets/Art/Sprites/Hunters/*.aseprite`, `Animals/*.aseprite`는 현재 원본 위치다. 파일을 복사해서 새 정본으로 선언하지 않는다. 앞으로 특정 자산을 새로 제작하면 **어느 파일이 편집 정본인지** 해당 작업 README에 지정한다. 실제 원본 이관이 필요하면 참조와 importer를 확인하는 별도 통합 작업으로 진행한다.

## 2. 실제 저장 예시

```text
Docs/AI/Art/01_ArtDirection/AD-03_ForestKeyArt/
  README.md
  hunting_idle_AD-03_forest_keyart_v001.png    # 검수용 전체 화면

ArtSource/Drafts/AD-03/v001/
  README.md                                 # 후보/도구/선택 결과
  forest_keyart.aseprite                     # 선택 전 편집 원본 예시

ArtSource/Production/Environment/BG-01/
  README.md                                 # 정본/버전/출처/연결 대상
  bg_forest_sky.aseprite                     # 신규 편집 정본
  Export/
    bg_forest_sky.png
    bg_forest_sky.json                       # 제작 메타, runtime 소비는 별도

Assets/Art/Sprites/Environment/Forest/
  bg_forest_sky.png                          # Unity 참조용 안정된 이름
  bg_forest_sky.png.meta                     # Unity가 생성/관리
```

위 파일은 경로 예시이며 이번 문서 작업에서 만든 신규 그림이 아니다. 편집 도구에 따라 `.aseprite / .psd / .kra / .svg` 등 적절한 원본 형식을 사용하되, 픽셀 작업은 레이어·태그를 보존하는 Aseprite를 우선한다.

## 3. Unity 출력과 연결 대상

| 자산 | 신규 출력 경로 | 현재 연결 |
| --- | --- | --- |
| BG-01 하늘 | `Environment/Forest/bg_forest_sky.png` | `HuntArtSet.sky` |
| BG-02 원경 | `Environment/Forest/bg_forest_far.png` | `distantForest` |
| BG-03 중경 | `Environment/Forest/bg_forest_mid.png` | `middleForest` |
| BG-04 나무 3종 | `Environment/Forest/bg_forest_tree_01.png` 등 | `trees[]` |
| BG-05 길 | `Environment/Forest/bg_forest_trail.png` | `trail`, 현재 stretch 표시 |
| PR-01 활 | `Props/Weapons/prop_bow.png` | `bow`, 정적 표시 슬롯 |
| PR-02 화살 | `Props/Weapons/prop_arrow.png` | `arrow` |
| PR-03 수레 | `Props/Cart/prop_cart.png` | `cart` |
| PR-07 건조대 | `Props/Camp/prop_drying_rack.png` | `dryingRack` |
| PR-08 훈제 시설 | `Props/Camp/prop_smoker.png` | `smoker` |
| IC-M01~06 메뉴 | `Icons/Menu/icon_storage.png` 등 | `menuIcons[0..5]` |
| UI 공통 프레임 | `UI/Frames/ui_panel_frame.png` 등 | 스킨 연결 개발 필요 |
| HU 사냥꾼 시트 | `Animations/Hunters/hunter_animations.png` | 프레임/상태 연결 개발 필요 |
| AN 종별 시트 | `Animations/Animals/rabbit/rabbit_animations.png` 등 | 프레임/상태 연결 개발 필요 |
| FX 효과 | `VFX/fx_hit.png` 등 | 이벤트/프레임 연결 개발 필요 |

표의 경로는 모두 `Assets/Art/Sprites/` 아래다. 메뉴 순서는 `icon_storage / icon_bow / icon_bestiary / icon_region / icon_goals / icon_settings`로 대응한다. 상품/상태 아이콘은 `Icons/Items`, `Icons/Status`로 나눈다.

PR-04 수레 전면, BG-06 하단 경계, 추가 장식에는 현재 전용 슬롯이 없다. 레이어별 파일은 납품할 수 있으나 개발 연결 완료로 표시하지 않는다. 활의 3포즈 역시 정적 `bow` 슬롯만으로 자동 재생되지 않는다.

## 4. 파일명과 버전

- 작업 ID는 Notion과 같은 형태를 유지한다: `AD-03`, `BG-01`, `IC-M01`.
- 실제 자산명은 소문자 snake_case: `hunter_walk`, `rabbit_flee`, `bg_forest_far`, `prop_cart`, `ui_panel_frame`, `icon_bow`, `fx_hit`.
- 종 ID는 `rabbit / boar / deer / magpie / tiger / fox`를 유지한다.
- 문서 시안은 `hunting_idle_<ID>_<설명>_v001.png`, 후보 폴더는 `v001 / v002` 순서다. `final / final2 / new` 이름은 사용하지 않는다.
- 제작 정본과 Unity 출력 이름은 안정적으로 유지하고 README에 현재 버전과 대응 export를 기록한다. 덮어쓰기 전 필요한 보존본은 Backups에 남긴다.
- Unity 적용 파일 이름/경로 변경은 GUID·Sprite 참조·설정 도구의 경로를 확인한 뒤 진행한다. `.meta`를 임의 재생성하지 않는다.

## 5. 자산별 README에 기록할 내용

```text
작업/자산 ID:
기능과 그림 이름:
현재 상태: 문서 준비 / 제작 중 / 시안 검수 / 제작 완료 / Unity 적용 / 적용 검수
편집 정본 경로와 현재 버전:
사용한 기존 원본과 참고 출처/라이선스:
문서 합성본 / export / Unity 출력 경로:
캔버스·셀 크기 / 프레임 수 / 태그·duration:
피벗·발·손·장비·운반 접점 및 좌표 기준:
팔레트와 신규 색 상태:
연결 슬롯 또는 필요한 코드 변경:
검수 해상도·상태 / 결과 / 남은 문제:
```

문서에서 사용하는 이미지의 출처도 기록한다. 외부 참고나 생성 후보가 편집 정본/게임 납품본으로 섞이지 않게 상태를 지정한다. 파일 개수만으로 작화량이나 완료 상태를 계산하지 않는다.

## 6. PNG·시트·메타데이터

- 캐릭터/소품/UI/FX는 투명 RGBA PNG를 기본으로 한다. 하늘 등 불투명 레이어는 의도한 전체 면을 사용한다.
- 애니메이션은 셀 크기·발 접점·몸 중심을 고정한다. 태그와 프레임별 duration을 보존하며 자동 trim으로 프레임 기준점이 흔들리지 않게 한다.
- 태그는 Notion의 상태와 맞춘다: hunter `idle / walk / focus / shoot / collect / carry_small / drag_large / load`, animal `idle / walk / flee / fall / downed`.
- 기본 오른쪽 원본, 왼쪽 반전 정책과 좌우 비대칭 장비의 예외를 기록한다.
- 기존 PNG/Sprite rect와 달라지면 실제 표시 높이, 운반 바닥, 조준 범위를 다시 검수한다. 과도한 투명 여백을 넣지 않는다.
- 신규 Unity import 제안: Sprite, Point, 무압축, MipMap off, PPU100 시작안. 시트 Multiple/정적 Single, pivot과 border는 자산별로 명시한다.
- 9-slice는 border 값과 최소 표시 크기를 기록한다. 게임 글자/수치는 PNG에 넣지 않는다.

제작 메타 JSON에는 `schema_version`, `work_id`, `asset_name`, `source`, `canvas_px`, `cell_px`, `coordinate_system`, `pivot`, `anchors`, `palette`, `tags`, `frame_duration_ms`, `events`를 필요한 범위에서 기록한다. 접점 좌표는 PNG 좌상단 원점/픽셀 단위인지 Unity pivot 정규화 값인지 명시하고 혼용하지 않는다. 실제 제작 전에는 빈 메타 파일을 만들지 않는다.

**이 메타 형식은 제작 전달 기준이며 현재 런타임에 JSON 로더가 구현되었다는 의미가 아니다.** release/명중/수거/적재 이벤트의 프레임과 게임 로직 연결은 적용 단계에서 구현·검수한다.

## 7. 작업 단계

1. 해당 AD/자산의 지시와 기존 원본을 확인한다.
2. `Drafts/<ID>/v001`에 편집 원본/후보를 저장하고 문서용 검수본을 해당 AD 폴더에 올린다.
3. 시안 검수 결과와 색·크기·접점 결정을 README에 기록한다.
4. 신규 자산의 편집 정본을 `Production`에서 관리하고 분리 PNG/메타를 export한다.
5. 적용 단계에서 필요한 출력만 `Assets/Art/Sprites`에 넣고 Unity가 메타를 생성하게 한다. 기존 자산 교체는 참조 보존을 확인한다.
6. 실제 연결과 3해상도/사냥 상태/메뉴 검수가 끝나면 `적용 검수`로 기록한다.

기존 AD-01 파일은 역사 작업본으로 보존했다. 이번 저장 체계 정리는 문서 영역에만 적용했고 기존 Unity 자산이나 ArtSource의 백업/개인작업 파일을 이동하지 않았다.

## 8. 기존 픽셀 그림의 색·면·경계 정리

색 조각·가는 띠·경계 돌출을 정리하는 작업에는 [PixelCleanupGuide.md](PixelCleanupGuide.md)를 적용한다. 공통 규칙은 이 가이드에서 관리하고, 실제 기준 예시·부위 마스크·전후 이미지·검수 결과는 각 작업 버전에 함께 저장한다. [ArtSource/AGENTS.md](../../../../ArtSource/AGENTS.md)에 다음 작업에서도 같은 확인 순서를 적용하도록 명시한다.

1. 작업 전 가이드와 해당 버전 README를 읽고, 깃털·모자 등 대표 영역의 `Reference/cleanup_reference.aseprite`와 클립의 `Masks/material_regions.aseprite`를 확인한다.
2. 부위별 기본색·명암·보존 디테일을 구분하고, 작은 색 조각뿐 아니라 띠·선 두께·계단 진행을 기존 Aseprite 픽셀에서 직접 수정한다.
3. 작업 후 파일 검수와 별도로 전신·전 프레임의 확대 비교, 원래 표시 크기, 애니메이션 재생을 확인한다.
4. `QA/visual_review.md`에 모든 부위의 전후 이미지·판정·남은 문제를 기록한다. 색 수 검사 통과나 한 부위의 개선만으로 전신 정리 완료를 선언하지 않는다.

Hunter_300 v006의 기준 예시와 마스크는 시험 제작용이다. 이 버전의 전신 검수 결과와 실제 표시 크기를 확인한 뒤 후속 제작에 적용할 범위를 정한다.
