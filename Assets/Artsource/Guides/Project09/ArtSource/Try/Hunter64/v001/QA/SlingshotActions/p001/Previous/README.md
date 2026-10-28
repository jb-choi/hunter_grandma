## 현재 정본 · 새총·가방 전파 · EquipmentPropagation/p001

2026-10-12. 현재 편집 정본은 [walk.aseprite](Clips/HU-02_walk/walk.aseprite)와 [idle_breath.aseprite](Clips/HU-01_idle/idle_breath.aseprite)다. walk F1의 사용자 `Slingshot&bag` 셀을 보존하고 walk 점유7프레임·idle 전5프레임에 반영했다. 가방132px은 몸 높이, 새총96px은 손과 몸 높이를 따른다. walk는 native11프레임·6레이어(`walk`1~9, 별도 무릎10~11), idle은5프레임·5레이어다. 기존 시간·팔레트와 walk 빈 F2·4·6·8을 유지했다.

walk native의 사용자 장비 격리 상태를 보존해 새 장비만 표시하고 몸3레이어·옛 활·`backgroud_temp`는 숨겨져 있다. 전체 캐릭터 export·미리보기는 `Slingshot&bag`+몸3레이어 표시, `Bow&Quiver`+임시 배경 숨김 정책을 사용한다. idle native와 출력은 새 장비+몸3레이어 표시, 옛 활 숨김으로 같다.

[walk 입력 보존본](Clips/HU-02_walk/Source/EquipmentPropagation/p001/walk_before.aseprite) · [idle 입력 보존본](Clips/HU-01_idle/Source/EquipmentPropagation/p001/idle_breath_before.aseprite) · [입력 해시](QA/EquipmentPropagation/p001/input_hashes.json). 기존 작성 셀은 픽셀·좌표 변경0이며 사용자 평면1프레임 `idle.aseprite`와 HU-03~08은 변경하지 않았다.

[수치 검수](QA/EquipmentPropagation/p001/native_validation.json) 통과와 [작화 검수](QA/visual_review.md)를 구분한다. 저장/전파 불일치0을 확인했고 전12개 점유 프레임의 실제64px·확대에서 손–새총, 가방–몸 접점과 옷·발 경계를 확인했다. [작업 안내](QA/EquipmentPropagation/p001/README.md) · [장비 원본](QA/EquipmentPropagation/p001/gear_before_8x.png) · [walk 전신](QA/EquipmentPropagation/p001/walk_contact_8x.png) · [idle 전신](QA/EquipmentPropagation/p001/idle_contact_8x.png).

아래의 Bow 4레이어·이전 프레임 수·축소 설명은 해당 단계의 이력이다. 최신 장비 표시와 검수는 위 기록을 따른다.

## 이전 walk 무릎·자락 비교 프레임

2026-10-12. 기존 걷기9프레임과4개 레이어를 보존하고 비교용10·11프레임을 뒤에 추가했다. 무릎의 꺾임과 올라온 다리에 얹힌 자락의 국소 변형을 비교할 수 있다. [편집 파일](Clips/HU-02_walk/walk.aseprite) · [전후 비교](Previews/WalkKneeDrape/index.html) · [검수](QA/WalkKneeDrape/p001/README.md).

## 이전 walk 팔 흔들림·몸 높낮이

2026-10-12. 기존 9프레임 구조와 4개 레이어를 유지하고 F3·5·7에 팔의 반대 흔들림과 -1/+1/-1px 몸 높낮이를 반영했다. 부츠 접지, F1·9 기준 자세와 빈 중간 프레임은 유지했다. [편집 파일](Clips/HU-02_walk/walk.aseprite) · [최신 검수](QA/WalkKeyposes/p002/README.md) · [키포즈 비교](QA/WalkKeyposes/p002/keyposes_8x.png).

## 이전 walk 키포즈 구조 · p001

2026-10-12. `idle_breath` F1의 4개 레이어를 유지해 walk를 9프레임 키포즈 작업본으로 변경했다. 1·9는 동일 기준 자세, 3·7은 발 교차, 5는 반대발 착지이며 2·4·6·8은 빈 프레임이다. [편집 파일](Clips/HU-02_walk/walk.aseprite) · [키포즈 검수](QA/WalkKeyposes/p001/README.md) · [재생기](Previews/Animations/index.html).

## 이전 idle 5프레임 레이어·디자인 전파

2026-10-12. 사용자가 F5에서 분리한 활·화살통 레이어와 수정한 픽셀을 F1~4에 반영했다. 전체 5프레임이 같은 4개 레이어 구성을 사용한다. F5 원본셀과 기존 호흡시간을 유지했다. [편집 파일](Clips/HU-01_idle/idle_breath.aseprite) · [검수](QA/IdleBreath/Frame05Propagation/p001/README.md) · [idle 재생기](Previews/IdleBreath/index.html).

## 이전 idle 1프레임 디자인 전파

2026-10-12. `idle_breath.aseprite` F1의 사용자 수정24점을 뒤4프레임에 반영했다. F1 원본셀과호흡시간표는 유지했다. [편집 파일](Clips/HU-01_idle/idle_breath.aseprite) · [검수](QA/IdleBreath/Frame01Propagation/p001/README.md) · [idle 재생기](Previews/IdleBreath/index.html).

## 상의 빈 면·네모 구멍 후속 수정 · Pass02

이전14점 보완은 오른쪽 옆구리만 검사해 상의 안의 투명 구멍과 왼쪽 절단면을 놓쳤습니다. 팔이 움직인 뒤 드러나는 왼쪽 상의–벨트–자락 면을 Body에 복원하고, 양쪽 상의 구멍·어깨·손과 자락 접점을 수정했습니다. 생성 idle의 상의 구멍2점도 보완했습니다. 전체8동작39프레임을 확인하고 native/PNG/시트/GIF·재생 검사를 통과했습니다. 사용자1프레임 디자인은 보존했습니다. [걷기 전후](QA/Animations/walk_pass02_before_after.png) · [조준 전후](QA/Animations/focus_pass02_before_after.png) · [최신 검수](QA/Animations/torso_pass02_visual_review.md) · [native 예시](Reference/TorsoOcclusion/garment_planes_pass02.aseprite)

## 기본 8동작 제작 이력

[8동작 재생기](Previews/Animations/index.html) · [로컬 재생기](http://127.0.0.1:53161/Previews/Animations/index.html)

사용자가 수정한 64px 디자인의 idle 5프레임과 HU-02~08의 신규 34프레임을 같은 v001에 저장했습니다. 아래 기본 애니메이션 목록은 walk 태그1~9 기준의 8동작·42프레임이며, 별도 무릎 비교2프레임은 위 이력을 따릅니다. walk의 4개 중간 프레임은 의도적으로 비어 있습니다. 원본 idle.aseprite는 사용자 수정 1프레임으로 보존합니다.

| 동작 | 프레임 | 길이 | 방식 | 편집 파일 |
|---|---:|---|---|---|
| HU-01 / idle | 5 | 1200ms | 반복 | [Aseprite](Clips/HU-01_idle/idle_breath.aseprite) |
| HU-02 / walk | 9 | 1080ms | 키포즈·빈 중간 프레임 | [Aseprite](Clips/HU-02_walk/walk.aseprite) |
| HU-03 / focus | 4 | 600ms | 마지막 유지 | [Aseprite](Clips/HU-03_focus/focus.aseprite) |
| HU-04 / shoot | 4 | 400ms | 단발 | [Aseprite](Clips/HU-04_shoot/shoot.aseprite) |
| HU-05 / collect | 4 | 600ms | 단발 | [Aseprite](Clips/HU-05_collect/collect.aseprite) |
| HU-06 / carry_small | 6 | 840ms | 반복 | [Aseprite](Clips/HU-06_carry_small/carry_small.aseprite) |
| HU-07 / drag_large | 6 | 960ms | 반복 | [Aseprite](Clips/HU-07_drag_large/drag_large.aseprite) |
| HU-08 / load | 4 | 1000ms | 단발 | [Aseprite](Clips/HU-08_load/load.aseprite) |

신규 34프레임의 저장 파일·PNG·시트·GIF·부위 마스크 검사와 전체 8동작 재생기 검사를 통과했습니다. 팔·손·관절은 동작 확인용 v001 포즈 시험본이며 세부 작화와 실제 동물·수레 연결의 남은 범위는 시각 검수 기록에 적었습니다. 각 신규 클립은 Source/Frames/Export/Masks와 README를 갖습니다. [동작 제작 명세](Prompts/Animations/motion_spec.md), [파일 검수](QA/Animations/native_validation.json), [전신·접점 검수](QA/Animations/visual_review.md)를 함께 보관합니다.

# 사냥꾼 64×64 idle 시험본

v001 · 2026-10-11 · HU-01_idle · 사용자 디자인 수정과 별도 호흡 애니메이션

사용자가 [idle.aseprite](Clips/HU-01_idle/idle.aseprite)를 **1프레임 디자인**으로 직접 수정했다. 이 파일이 현재 디자인 편집 원본이다. 수정한 키포즈를 기준으로 같은 v001에 별도 [idle_breath.aseprite](Clips/HU-01_idle/idle_breath.aseprite)를 추가한다. 동작은 상체·머리 1px 하향 → 머리 전체 추가 1px 하향 → 머리·상체 순서로 기준 높이 복귀다. 모자·깃털·머리카락·얼굴을 머리 전체로 함께 움직이고 하체·발의 접지는 유지한다.

| 현재 작업 | 편집 파일 / 출력·검수 |
|---|---|
| 사용자 디자인 키포즈 | [idle.aseprite](Clips/HU-01_idle/idle.aseprite), 64×64, 1프레임 |
| 별도 호흡 애니메이션 | [idle_breath.aseprite](Clips/HU-01_idle/idle_breath.aseprite), [상세·동작표](Clips/HU-01_idle/idle_breath.md) |
| 시작 원본 보존 | [Source/IdleBreath](Clips/HU-01_idle/Source/IdleBreath/README.md) |
| 새 프레임·시트·재생 | [Frames/IdleBreath](Clips/HU-01_idle/Frames/IdleBreath/idle_01.png), [Export/IdleBreath](Clips/HU-01_idle/Export/IdleBreath/idle.json), [Previews/IdleBreath](Previews/IdleBreath/index.html) |
| 새 검사 기록 | [native_validation.json](QA/IdleBreath/native_validation.json), [visual_review.md](QA/IdleBreath/visual_review.md), [source_hash.json](QA/IdleBreath/source_hash.json) |

새 호흡 애니메이션은 **5프레임·1,200ms 반복**이며 각 duration은 360 / 180 / 300 / 180 / 180ms다. 팔레트는 기존 17칸, 실제 불투명 사용색은 15색이다. 저장 파일의 독립 재개봉과 픽셀·태그·duration·발 고정·원본 해시 검사를 통과했고, 실제 64px와 8배 확대의 전 5프레임 정적 시각 검사도 통과했다. 실시간 브라우저 반복 재생과 레이아웃 검수도 통과했다. 사용자의 디자인 파일, 이전 300 원본과 초기 축소 출력은 보존한다.

## 300→64 초기 축소 기록

**아래의 2프레임·15색·눈 높이·변환 수치와 기존 루트 PNG·시트·미리보기·QA는 사용자 수정 이전의 초기 축소 납품 기록이다. 현재 1프레임 디자인이나 새 호흡 애니메이션의 검사 결과가 아니다.** 초기 파일 검수의 `QA/native_validation.json`과 현재 `idle.aseprite`가 일치하지 않는 것은 이후 사용자 디자인 수정으로 입력이 바뀌었기 때문이다. 최신 검수는 `QA/IdleBreath/`에서 확인한다.

초기 납품: v001 · HU-01_idle · 2프레임 · 각 500ms · 1초 반복

[Hunter_300 v006](../../Hunter_300/v006/README.md)의 idle을 64×64 캔버스로 옮긴 비교 시험이다. 300×300 원본의 전체 캔버스 구도를 유지하고 기존 팔레트 인덱스를 최근접 중심 샘플링으로 축소했다. 디자인·포즈를 새로 생성하거나 색을 평균 내지 않았다.

초기 축소 입력 원본은 [Source/idle_300_v006.aseprite](Clips/HU-01_idle/Source/idle_300_v006.aseprite)에 보관한다. 최초 64 출력은 아래 PNG·시트·GIF로 보존한다. 최초 편집 파일 위치는 이후 사용자 디자인 편집에 사용되었으므로, 이전 2프레임을 현재 native 파일 상태로 혼동하지 않는다. 이번 버전은 idle만 포함한다.

[비교 재생기](Previews/index.html) · [로컬 재생기](http://127.0.0.1:53161/Previews/index.html) · [전후 비교 PNG](Previews/idle_before_after.png) · [idle 상세](Clips/HU-01_idle/README.md) · [시각 검수 기록](QA/visual_review.md)

## 파일과 크기

| 항목 | 내용 |
|---|---|
| 편집 캔버스 | 64×64 Indexed |
| 애니메이션 | `idle`, 정방향 2프레임, 각 500ms |
| 팔레트 | 원본의 17칸 유지: 투명 인덱스 0 + 불투명 16색 |
| 실제 사용색 | 두 프레임 모두 불투명 15색; 작은 금색 그림자 인덱스 15는 축소 샘플에서 선택되지 않음 |
| 좌표 | PNG 좌상단 원점, x 오른쪽·y 아래, 픽셀 단위 |
| 프레임 PNG | [idle_01.png](Clips/HU-01_idle/Frames/idle_01.png), [idle_02.png](Clips/HU-01_idle/Frames/idle_02.png), 각각 64×64 |
| 시트 | [idle_sheet.png](Clips/HU-01_idle/Export/idle_sheet.png), 128×64 |
| 메타데이터 | [idle.json](Clips/HU-01_idle/Export/idle.json) |
| 확대 미리보기 | [1번 프레임](Previews/idle_zoom_01.png), [2번 프레임](Previews/idle_zoom_02.png), 각각 512×512 최근접 8배 확대 |
| 반복 재생 | [idle.gif](Previews/idle.gif) |

팔레트에 남아 있는 색 수와 축소된 프레임에서 실제 사용하는 색 수는 구분한다. [변환 측정](QA/conversion.json)의 캐릭터 bounds는 다음과 같다. 64×64는 캐릭터를 감싸는 캔버스 크기이며, 실루엣은 두 프레임 모두 50px 높이다.

| 프레임 | bounds `(x, y, w, h)` | 불투명 픽셀 | 발 바닥 y |
|---|---|---:|---:|
| 1 | `(17, 7, 32, 50)` | 1,039 | 56 |
| 2 | `(17, 7, 33, 50)` | 1,066 | 56 |

원본 보존·PNG 일치와 저장된 native 파일의 측정은 [native_validation.json](QA/native_validation.json)에서 확인한다.

## 변환 기준

출력 좌표 `(x, y)`가 참조하는 입력 좌표는 `floor((x + 0.5) × 300 / 64)`, `floor((y + 0.5) × 300 / 64)`다. 두 프레임에 같은 좌표 변환을 적용하며 자동 trim이나 프레임별 재배치를 하지 않는다. 축소 시 선택되지 않는 작은 명암·가는 선은 줄거나 사라질 수 있으므로 전신 확대와 반복 재생에서 확인한다.

추가 픽셀 수정은 0개다. 눈은 최근접 샘플 결과에 따라 1번 프레임에서 1×2px, 2번에서 1×3px로 남아 있다. 이 차이를 다시 그리지 않고 우선 원본 축소 결과로 보관했다. 전신 가독성과 재생 판정은 [visual_review.md](QA/visual_review.md)에 기록한다. 이미지 재생성, 블러, 안티앨리어싱, 디더링은 사용하지 않는다.

전후 비교 PNG는 각 캔버스를 512×512로 표시한 1024×1024 이미지다. 왼쪽은 300px 원본, 오른쪽은 64px 결과이며 위쪽은 1번·아래쪽은 2번 프레임이다. [실제 크기 프레임 나열](Previews/idle_contact.png)은 128×64, [8배 프레임 나열](Previews/idle_contact_8x.png)은 1024×512다.

## 기준 예시와 마스크

[Reference/cleanup_reference.aseprite](Reference/cleanup_reference.aseprite)는 Hunter_300 v006에서 사용한 v005/v006 정리 기준을 같은 방식으로 64×64로 축소한 참조다. `Before_v005`는 기본 숨김, `After_v006`는 기본 표시다. 이 두 레이어는 Hunter64에서 새로 정리한 전후가 아니다. [300px 기준 예시 보관본](Reference/Source/cleanup_reference_300.aseprite)과 대응한다.

[material_regions.aseprite](Clips/HU-01_idle/Masks/material_regions.aseprite)는 같은 샘플링으로 축소한 64×64 부위 마스크다. 13개 재질 면, 보존 디테일, 수정 경계의 대응을 확인한다. 원본 마스크와 정의는 Source에 별도로 보관하며 [마스크 안내](Clips/HU-01_idle/Masks/README.md)에 명시한다.

## 폴더 체계

```text
Reference/                               # 300 원본 참조와 64 축소 참조
Clips/HU-01_idle/
  Source/idle_300_v006.aseprite            # 입력 300×300 원본 보관본
  Masks/                                 # 64×64 부위 마스크·대응 정의
  idle.aseprite                          # 64×64 편집 정본
  Frames/                                # 64×64 개별 PNG
  Export/                                # 시트·타이밍·팔레트
Previews/                                # 동일 표시 크기 비교·확대·재생기
Prompts/resize_spec.md                    # 변환 범위와 방법
QA/                                      # 원본 해시·파일 검사·시각 검수
```

원본/축소본의 구분은 [Source 안내](Clips/HU-01_idle/Source/README.md)와 [마스크 안내](Clips/HU-01_idle/Masks/README.md)를 따른다. 마스크와 비교 레이어는 캐릭터 export에 포함하지 않는다.

## 저장 파일 검수

저장된 native 편집 파일·PNG·시트·GIF·마스크·기준 예시를 독립적으로 다시 읽어 [파일 검수](QA/native_validation.json)를 통과했다. 두 프레임의 8,192개 출력 픽셀은 지정한 최근접 중심 샘플과 정확히 일치하며 PNG·시트·GIF 불일치는 모두 0개다. 반투명 픽셀도 0개다. 원본과 보관본의 SHA-256 일치 및 원본 미변경은 [source_hash.json](QA/source_hash.json)에서 확인했다.

이 검사는 변환과 저장의 무결성 판정이다. 작은 디테일의 손실·프레임 간 형태 변화·64px 작업성에 대한 판정은 [시각 검수 기록](QA/visual_review.md)을 따른다.

[재생기 검사](QA/browser_validation.json)도 통과했다. 100% 표시에서 64px 캔버스·50px 캐릭터, 800%에서 512px 캔버스·400px 캐릭터를 확인했고, 캐릭터 높이 42px/68px 설정과 프레임 진행이 정상 동작했다. 390px 모바일 화면의 가로 넘침과 재생기 링크 오류는 없었다. 재생 검사 통과가 눈·귀·버클 등 축소 디테일의 재작화 완료를 뜻하지는 않는다.

저장 기준은 [제작 가이드](../../../../Docs/AI/Art/02_Production/ProductionGuide.md), 이후 직접 픽셀 정리는 [픽셀 정리 가이드](../../../../Docs/AI/Art/02_Production/PixelCleanupGuide.md)를 따른다. 이 버전은 `Try`의 해상도 비교본이며 Unity 적용과 정식 제작 해상도 확정은 진행하지 않았다.
