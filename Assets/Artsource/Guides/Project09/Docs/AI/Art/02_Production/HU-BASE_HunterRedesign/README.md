# HU-BASE · hunter base 32×32 리디자인 기록

2026-10-11 · 요청: `hunter base 32by32 리디자인`

현재 버전은 **v002 피드백 반영 수정본**이다. `HU-BASE`는 이번 작업의 로컬 ID이며 Notion HU-01~08 애니메이션 완료를 뜻하지 않는다.

v001은 파일 규격을 충족했지만 좋은 생성 시안의 머리·얼굴·의상 특징을 잃었다. 사용자의 부정적 피드백 이후 첫 시안의 좁은 비율과 따뜻한 색감을 기준으로 다시 제작했다. 머리카락/귀, 목깃, 사선 끈, 화살통, 옷자락과 부츠를 유지하고 눈·밑창·활의 줄과 곡선을 다듬었다.

![기존 / v001 / v002](../../../../../ArtSource/Previews/HU-BASE/v002/hunter_comparison_6x.png)

왼쪽은 기존 총 사냥꾼, 가운데는 이전 v001, 오른쪽은 수정 v002다. 같은 32×32 셀을 6배로 확대한 비교다.

## 현재 산출물

- [편집 정본](../../../../../ArtSource/Production/Hunters/HU-BASE/hunter_base.aseprite): 32×32, 1프레임, Body/Bow 2레이어.
- [몸 PNG](../../../../../ArtSource/Production/Hunters/HU-BASE/Export/hunter_base.png), [활 포함 PNG](../../../../../ArtSource/Production/Hunters/HU-BASE/Export/hunter_base_with_bow.png), [분리 활](../../../../../ArtSource/Production/Hunters/HU-BASE/Export/bow_base.png).
- [14색·접점·파일 규격 안내](../../../../../ArtSource/Production/Hunters/HU-BASE/README.md).
- [시안과 제작 과정](../../../../../ArtSource/Drafts/HU-BASE/v002/README.md), [v001 보존본](../../../../../ArtSource/Backups/HU-BASE/2026-10-11_v001/README.md).

## 확인 범위

PNG 3개가 실제 32×32이며 반투명 픽셀은 0개다. 몸/합성본 14색, 활 4색. 합성본 경계는 x7/y2/폭19/높이28, 발은 y29, 손–활 접점은 (24,21)이다. 원본 시안과 비교하여 의상 특징, 눈, 밑창과 활 내부 틈을 시각적으로 확인했다.

정적 수정본 출력과 파일 검수까지 진행했다. 사용자 디자인 승인을 완료 상태로 대신 기록하지 않는다. Unity Sprite 참조·메타·코드와 기존 게임 자산은 유지했으며, 애니메이션·실제 게임 적용과 실행 검수는 아직 수행하지 않았다.

[아트 문서 입구](../../README.md) · [전체 방향](../../01_ArtDirection/ArtDirection.md) · [저장 규칙](../ProductionGuide.md)
