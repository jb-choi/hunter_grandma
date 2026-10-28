# 순덕 할매 — 첫 전투 프로토타입

`Scenes/GrandmaDefense.unity`를 열고 **Play**를 누르면 전투가 바로 시작됩니다.

- `grandma_04_sundeok`이 사거리 안의 가장 가까운 좀비를 자동으로 조준합니다.
- 오른쪽에서 좀비가 계속 생성되어 왼쪽으로 걸어옵니다. 탄환에 맞으면 뒤로 넘어지고 사라집니다.
- 할머니 발사 이미지·반동, 탄환 궤적, 간단한 합성 총성, 처치 수와 경과 시간을 표시합니다.
- **Space / 일시정지 버튼**: 정지·재개. **R / 다시 시작 버튼**: 처치 수와 좀비를 초기화합니다.
- **지도 버튼 / M**: 첨부한 폐허 도시 지도를 중앙 창으로 엽니다. 6개 구역 핀을 눌러 정보를 보고 **이 구역에서 전투 시작**으로 확정합니다. **닫기 / Esc / 창 바깥**으로 취소합니다.

지도 창에서는 전투가 멈춥니다. 닫을 때 원래 일시정지 상태를 복원하며, 창을 연 동안 Space/R 전투 단축키는 차단합니다. 다른 구역을 확정하면 현재 구역 표시와 전투를 초기화하고, 같은 구역에서 계속하면 기존 진행을 유지합니다. 일반 재시작은 현재 구역을 유지합니다. 지역 선택은 이번 실행 동안 유지되며 별도 저장은 하지 않습니다.

이번 맵 작업은 지역 선택 UI와 기존 전투 연결입니다. 6개 구역은 현재 같은 횡스크롤 전투 배경과 밸런스를 사용합니다. 구역마다 다른 전투 씬/배경은 아직 제작하지 않았습니다.

`Endless defense` 오브젝트의 `GrandmaDefense`에서 생성 간격, 발사 간격, 좀비 속도, 사거리, 최대 좀비 수를 조절합니다. 기본 생성 간격은 1.7초, 발사 간격은 0.72초, 좀비 속도는 초당 1.4유닛, 사거리는 7.5유닛입니다. 할머니 `GrandmaShooter`의 Shot Volume으로 소리를 조절합니다.

좀비와 탄환은 고정 크기 풀을 재사용합니다. 총알이 날아가는 중인 좀비에는 중복 발사하지 않습니다. 사망 중인 좀비는 표적에서 제외됩니다. 기본 밸런스에서는 할머니에게 도달하기 전에 처치됩니다. 실험 설정에서 방어선을 통과한 좀비는 처치 수를 올리지 않고 회수되며 다음 좀비가 계속 나옵니다. 체력·게임 오버·성장·저장 기능은 이 첫 전투 범위에 포함하지 않았습니다.

## 구성과 확인

- `Runtime/GrandmaDefense.cs`: 생성·표적 선택·재시작·풀 관리.
- `Runtime/GrandmaShooter.cs`: 발사 이미지·반동·총성.
- `Runtime/ZombieActor.cs`: 걷기·명중·쓰러짐·페이드.
- `Runtime/ShotTracer.cs`: 실제 비행 후 명중 처리.
- `Runtime/DefenseHud.cs`: 한국어 UI·Input System 키보드/버튼 입력. Windows의 맑은 고딕 등 시스템 글꼴 사용.
- `Runtime/DefenseMapView.cs`, `DefenseRegions.cs`: 중앙 지도 창·6개 구역 핀·지역 정보·확정/취소. project09의 `HuntHud.BuildDrawer`와 `HuntWorldMapView`를 읽고 하단 진입 버튼, 중앙 창, 지도 핀, 정보 패널 흐름을 참고했습니다. project09 파일은 수정하지 않았습니다.
- `Prefabs`: 재사용할 좀비와 탄환. 각 씬 참조는 Inspector에 직렬화됩니다.
- `Editor/GrandmaProjectSetup.cs`: 프로젝트의 Unity Sprite Data Provider API로 스프라이트를 잘라 씬/프리팹을 만듭니다. 기존 생성 씬은 덮어쓰지 않습니다.
- `Tools > Hunter Grandma > Run Combat Smoke Test`: 실제 Play Mode에서 60초 전투, 정지·재개·초기화를 검사합니다. 결과는 프로젝트 루트 `Docs/Validation/CombatSmokeTest.md`에 저장합니다.
- `Tools > Hunter Grandma > Run Map UI Smoke Test`: 지도 열기/선택/확정/취소, 정지 상태 보존, 반복 사용, M/Esc 및 입력 차단을 검사합니다. 결과는 `Docs/Validation/MapSelectionSmokeTest.md`, 화면은 `MapSelection-1280x720.png`와 `MapSelection-1024x768.png`입니다.

새 씬을 Build Settings의 첫 번째로 추가하고 기존 SampleScene은 보존했습니다. 추가 패키지는 설치하지 않았습니다. 기존 `Assets/Artsource` 정본은 수정하지 않고 런타임용 PNG만 복사했습니다.

## 아트 출처와 생성 기록

할머니: `Assets/Artsource/Drafts/GrandmaShooter64/v002/Export`의 64×64 발사/효과 없음 정본. 걷는 좀비: 사용자가 첨부한 이미지를 시각 참고로 **built-in ImageGen**에서 생성. 원본은 `Art/Source/zombie_walk_generated.png`에 보존했습니다. Unity 편집기에서 알파 경계를 검사해 Point 방식으로 3장의 64×64 프레임에 옮겼으며 각 프레임의 발바닥을 맞췄습니다. 런타임은 `Art/zombie_walk_64.png`를 사용합니다. 배경은 편집기 코드로 생성한 픽셀 풍경입니다.

지도: 사용자가 2026-10-25에 첨부한 PNG를 `Art/Maps/ruined_city_map.png`로 그대로 복사했습니다. 원본 픽셀을 수정하지 않았으며 Unity에서 Sprite/Point/압축 없음으로 불러옵니다. `GrandmaMapSetup`이 `DefenseHud.cityMap`의 직렬화 참조를 연결합니다.

최종 생성 프롬프트:

> Use case: stylized-concept. Asset type: pixel art sprite sheet for a 2D side-view Unity game. The attached image is a reference for the zombie identity, colors and coarse pixel style only. Generate one transparent RGBA spritesheet, exactly three equal-width cells in one horizontal row. Each cell contains the SAME entire full body zombie, matching green square head and green skin, ragged dark brown sleeveless jacket, torn purple trousers, thin bony feet. Side view facing LEFT in all three cells, arms extended toward the left. Three successive WALK cycle poses: left foot forward, passing pose, right foot forward. Same scale, head size, proportions and baseline in all cells. Simple chunky low resolution crisp pixel art, approximately a 32 by 48 pixel character represented as large square blocks, flat limited muted colors and hard pixel edges, NO antialiasing. Keep 15 percent transparent padding on all sides of each cell, especially between figures, no overlap. No background, no paper, no floor lines, no shadow, no typography, no labels, no grid, no weapons. All characters upright and visibly walking, not front facing. Preserve resemblance to attached zombie. This is a production gameplay sprite asset, not a scene illustration.

API 확인: 설치된 2D Sprite 패키지의 `Documentation~/DataProvider.md`와 [Unity Sprite.Create 문서](https://docs.unity.com/en-us/engine/6000.5/script-reference/unityengine/sprite/create)를 참고했습니다.
