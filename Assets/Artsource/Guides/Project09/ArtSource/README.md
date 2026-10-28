# Hunting Idle 편집 원본

이 폴더는 신규 아트의 편집 원본과 작업 중 출력, 미리보기, 보존본을 관리한다. 작업 순서와 레이어 단독·숨김·전 프레임 검수는 [범용 아트워크 가이드](../Docs/AI/Art/02_Production/ArtworkGuide.md), 제작 방향은 [아트 문서](../Docs/AI/Art/README.md), 상세 경로/납품 규칙은 [제작 가이드](../Docs/AI/Art/02_Production/ProductionGuide.md)를 따른다.

| 폴더 | 용도 | 생성 시점 |
| --- | --- | --- |
| `Drafts/<ID>/v001/` | 선택 전 시안/편집 원본/후보 | 해당 작업 시작 시 |
| `Production/<Category>/<ID>/` | 검수 후 신규 자산의 편집 정본과 `Export/` | 자산 정본 확정 시 |
| `Previews/<ID>/` | 반복 비교·확대·프레임 검수 출력 | 검수본 생성 시 |
| `Backups/<ID>/<날짜_버전>/` | 변경 전 보존본 | 필요할 때 |
| `Try/Hunter_1254/v001/` | 고해상도 사냥꾼 8동작 시험본·비교 재생기 | 사용자 요청 실험, 해상도 결정 전 |
| `Try/Hunter_300/v001/` | 기존 사냥꾼 idle의 300×300 축소본·1254 비교 재생기 | 300 캔버스 편집·표시 비교 |
| `Try/Hunter_300/v002/` | idle 공통 26색·불투명 경계 정리본과 v001 비교 | 픽셀 색 정리 비교 |
| `Try/Hunter_300/v003/` | idle 재질별 단색 면·16색 정리본과 v002 비교 | 면 내부 얼룩·잔색 정리 |
| `Try/Hunter_300/v004/` | idle 귀·손·깃털 잔색과 내부 경계 정리본·v003 비교 | 디테일 보존하며 작은 색 조각·가는 띠 정리 |
| `Try/Hunter_300/v005/` | idle 눈·얼굴·모자·머리카락 경계 정리본·v004 비교 | 경계의 이물색·가는 띠·돌출과 패임 직접 수정 |
| `Try/Hunter_300/v006/` | idle 재질 마스크·기준 예시·전신 경계 정리본·v005 비교 | 11개 부위·두 프레임 확대 검수와 기록 |
| `Try/Hunter64/v001/` | 64×64 초기 축소 기록·사용자 수정 1프레임 디자인·별도 idle 호흡 애니메이션 | 64 캔버스 편집·표시·호흡 시험 |

기존 `Backups/rabbit_before_pixel_cleanup`와 `Previews/rabbit_cleanup_comparison.png`는 기존 경로를 유지한다. 사냥꾼/6종의 현재 Aseprite 원본은 `Assets/Art/Sprites/Hunters`, `Animals`에 있다. 새 위치에 복사하여 중복 정본으로 관리하지 않는다.

각 작업 README에 편집 정본 1개와 대응 PNG, 버전, 상태, 출처, 팔레트, 접점/프레임, Unity 연결 대상을 기록한다. 게임용 출력은 `Assets/Art/Sprites`로 전달하며 문서용 비교 시트는 `Docs/AI/Art`에 둔다.

후속 작업으로 [HU-BASE 사냥꾼 32×32 수정본](Production/Hunters/HU-BASE/README.md)을 제작했다. 현재 버전은 v002이며, 시안은 Drafts, 확대/비교는 Previews, 수정 전 v001은 Backups에서 보존한다. Unity 자산 적용은 별도 단계다.

[사냥꾼 1254×1254 애니메이션 시험본](Try/Hunter_1254/v001/README.md)은 8동작·36프레임 시안과 재생 비교를 한 폴더에서 관리한다. 정식 Production 해상도를 변경한 기록은 아니다.

[사냥꾼 300×300 idle 시험본](Try/Hunter_300/v001/README.md)은 동일한 폴더 체계로 대기 2프레임만 관리한다. 기존 1254 편집 원본의 최근접 축소본이며, 새 작화나 추가 안티앨리어싱 정리를 진행한 결과는 아니다.

[300 idle v002](Try/Hunter_300/v002/README.md)는 v001을 보존하고 공통 26색과 투명 인덱스 1개로 색을 정리한 후속 편집본이다. 새 포즈 생성 없이 반투명 경계와 일부 면 내부 잔색을 정리했다.

[300 idle v003](Try/Hunter_300/v003/README.md)는 v002에 남았던 같은 면 안의 얼룩을 합친 후속 비교본이다. 녹색과 가죽의 명암 면을 지정하고 공통 16색을 사용한다. 두 프레임과 외곽 실루엣은 유지하며, 기존 포즈 차이는 남아 있다.

[300 idle v004](Try/Hunter_300/v004/README.md)는 v003의 기존 팔레트와 포즈를 유지하고 귀·손·깃털에 남은 작은 색 조각과 가는 띠, 내부 색 경계 돌출을 정리한 후속 편집본이다. 새 이미지 생성 없이 Aseprite 픽셀을 직접 수정했다.

[300 idle v005](Try/Hunter_300/v005/README.md)는 v004에서 남았던 눈의 이물색과 얼굴·모자·머리카락의 가는 색 띠를 정리한 후속 편집본이다. 16색과 포즈를 유지하면서 검정 경계와 외곽 일부도 직접 수정했다. 얼굴 확대 비교와 변경 좌표는 해당 버전의 Previews와 QA에서 확인한다.

[300 idle v006](Try/Hunter_300/v006/README.md)는 실제 정리 기준 예시와 13개 재질 마스크를 만들고, 깃털부터 신발·장비까지 두 프레임의 경계를 정리한 시험본이다. 전신 11부위 비교와 판정은 해당 버전의 Previews/Review와 QA/visual_review.md에 보관한다. 이후 색·경계 정리에는 [공통 픽셀 정리 가이드](../Docs/AI/Art/02_Production/PixelCleanupGuide.md)와 [ArtSource 작업 지침](AGENTS.md)을 적용한다.

[64×64 idle 시험본](Try/Hunter64/v001/README.md)은 최초 Hunter_300 v006의 두 프레임을 같은 캔버스 구도로 최근접 축소했다. 이후 사용자가 같은 v001의 `idle.aseprite`를 1프레임 디자인으로 수정했고, 이 키포즈를 입력으로 별도 [idle_breath.aseprite](Try/Hunter64/v001/Clips/HU-01_idle/idle_breath.aseprite)에 호흡 동작을 추가한다. 300×300 초기 입력·2프레임 축소 출력·과거 검수는 보존하고, 최신 애니메이션 출력·검수는 `IdleBreath/` 하위로 구분한다. [동작 상세](Try/Hunter64/v001/Clips/HU-01_idle/idle_breath.md)에서 최신 상태를 확인한다. 정식 제작 해상도와 Unity 자산은 변경하지 않았다.
