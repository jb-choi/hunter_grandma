# LH-01 오두막 Aseprite 작업본 v002

작성: 2026-10-12 (Asia/Seoul). 사용자 요청에 따라 기존 컨셉 이미지를 Aseprite 편집 준비본으로 변환했다. 이미지 재생성 없이 Aseprite 1.3.18.6의 CLI/Lua로 작업했다.

**현재 편집 작업본:** [Work/loghouse_576x320_v002.aseprite](Work/loghouse_576x320_v002.aseprite)

## 규격

- 전체 캔버스 **576×320px = 64×64 격자 9×5칸**, RGB, 정적 1프레임, 100ms, `idle_layout` 태그.
- 기본 격자 원점 (0,0), 간격 64×64. 화면 구도는 유지하므로 장식의 실제 위치를 억지로 격자 모서리에 맞추지는 않았다. 소품 출력 셀의 너비·높이는 64의 배수다.
- 512×320 / 576×320 / 640×384를 비교했다. 512와 640 후보는 상하 여백이 필요하고, 576×320은 원본 가로 구도를 거의 그대로 유지하면서 사냥개와 도구를 읽을 수 있어 선택했다.
- 원본 컨셉 1672×941 → Nearest로 576×324 → 위/아래 각 2px만 잘라 576×320. 캐릭터·건물·수레를 잘라내지 않았다.
- 96색 상한으로 만든 팔레트의 실제 색은 **94색**. 디더링·블러·안티앨리어싱 없음. RGB 편집 모드로 되돌려 자유롭게 리터치 가능.
- 표시할 때 Point/Nearest로 **1× 576×320 / 2× 1152×640 / 3× 1728×960** 권장. 64×64 자산을 실제 이미지보다 임의로 크게 늘린다는 뜻은 아니다.

## 레이어와 소품 셀

총 14개 레이어: 배경 1 + 숨긴 정규화 참조 1 + 주요 소품 10 + 리터치 빈 레이어 1 + 숨긴 가이드 1. 참조와 가이드는 기본 숨김/잠금 상태다.

| 소품 | 출력 셀 | 레이어 |
| --- | --- | --- |
| 활·화살통 | 64×128 | 02 Weapon |
| 사슴 액자 | 64×64 | 03 Collection |
| 여우 액자 | 64×64 | 04 Collection |
| 조각 기념품 | 64×64 | 05 Collection |
| 펼친 도감 | 64×64 | 06 Collection |
| 돋보기 | 64×64 | 07 Scout |
| 쌍안경 | 64×64 | 08 Scout |
| 사냥개+잠자리 | 128×64 | 09 Companion |
| 외부 개집 | 128×128 | 10 Outdoor |
| 주차 수레+등불 | 128×128 | 11 Outdoor |

각 소품은 투명 RGBA PNG로 따로 출력했다. 실제 실루엣은 셀보다 작다. 셀 내부는 가로 중앙/바닥 정렬, 바닥 피벗은 `Export/loghouse_layout.json`에 기록했다. 소품마다 장면 내 위치와 출력 셀 오프셋을 구분한다.

## 파일

- `Export/loghouse_576x320.png`: 가이드/참조를 제외한 합성본.
- `Export/loghouse_background_repaired_576x320.png`: 소품을 제외하고 가려진 배경을 주변 픽셀로 추정 복원한 준비본.
- `Export/prop_*.png`: 10종 소품. 64배수 크기.
- `Export/loghouse_layout.json`: 캔버스·격자·소품 셀·좌상단 기준 위치·오프셋·피벗.
- `Export/loghouse_palette.gpl`: 실제 94색 팔레트.
- `Previews/loghouse_2x.png`: 정수 2배 확대.
- `Previews/loghouse_grid64.png`: 64×64 배치 격자 확인용. 게임 출력에 사용하지 않는다.
- `Previews/props_64_aligned.png`: 소품 접촉판, 위 5종/아래 5종은 표 순서.
- `QA/visual_review.md`, `QA/file_verification.json`, `QA/props_verification.json`: 시각 검토와 파일 재읽기 확인.
- `Scripts/build_workfile.lua`, `Scripts/verify_workfile.lua`, `Scripts/compare_sizes.lua`: 작업/재검수 절차. `--script-param root=<이 버전 폴더>`를 전달한다.

## 작업본의 범위

현재는 생성 컨셉에서 실제 픽셀을 분리한 **편집 준비본**이다. 완전한 수작업 도트 재작화나 정식 Unity 적용 검수 완료본은 아니다.

다각형으로 주요 소품 영역을 분리했으며, 소품 가장자리에는 원본의 벽·잔디·바닥 픽셀이 일부 포함되어 있다. 개집은 주변 풀, 수레는 등불, 사냥개는 잠자리를 함께 유지했다. 소품 뒤 배경은 원래 가려져 있던 면을 복구한 것이 아니라 주변 픽셀에서 추정한 것으로, 복제 질감/접합이 남는다. 큰 이동·다른 모양 장비로 교체할 때는 경계와 배경을 추가 리터치해야 한다.

작업본의 모든 기본 표시 레이어를 합친 결과는 선택한 576×320 정규화 시안과 픽셀 단위로 일치한다. 분리 과정에서 기본 구도가 바뀌거나 소품이 두 번 그려지는 문제는 없다.

아이템 변경 버튼/가격/레벨은 배경에 합치지 않았다. 새 장비 단계나 사냥개 애니메이션은 이번 작업 범위에 포함되지 않는다. Unity 자산·GUID·`.meta`는 변경하지 않았다.

## 원본 보존과 문서

`Source/loghouse_concept_v001.png`는 직전 생성 컨셉 보존본, `Source/loghouse_sketch_source.kra`는 사용자 러프 보존본이다. v001과 원본 KRA는 덮어쓰지 않았다. 해시는 `QA/source_hashes.json`에 기록한다.

Notion: [아트워크 제작 목록](https://app.notion.com/p/3f5f3731816f81c09cd2c86fa306ef5f)의 6-1절에 원본 컨셉과 이 작업본 규격·미리보기·다운로드 묶음을 반영한다.

사용한 Aseprite 공식 API: [SpriteSize](https://aseprite.org/api/command/SpriteSize), [Sprite와 gridBounds](https://aseprite.org/api/sprite), [ColorQuantization](https://aseprite.org/api/command/ColorQuantization).
