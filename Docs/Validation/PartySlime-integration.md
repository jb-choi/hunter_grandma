# Party Slime 통합 결과

2026-11-04 · Unity 6000.3.10f1 · baseline `70c1114` + 기존 사용자 작업 유지.

추가 요청 반영: `ZombiePopVisual`의 표시 배율을 **.8 → 2.4**로 바꿔 팝 효과의 가로·세로를 이전보다 **3배** 키웠다. 같은 프레임·원점·시간을 사용하므로 파티클의 화면상 크기와 퍼지는 범위가 함께 커진다. 확대 후 D3D11 Play Mode 기능 검사 **230개를 다시 통과**했고, Z1/Z2와 반대 방향의 실제 렌더를 확인했다. 결과는 [확대 효과 검사](PartySlime-large-validation.md), [Z1 확대](PartySlime-large-pop.png), [Z2 확대](PartySlime-large-horn-pop.png), [반대 방향](PartySlime-large-right-pop.png)에 있다. 아래 222개 전체 스테이지 검사는 확대 전 결과이며, 확대 요청은 표시 크기만 변경했다.

## 게임 동작

- Stage 0의 도입부와 감염체 구성 유지.
- Stage 1: 기존 일반·감염체에 Z01 풍선 파티 좀비 추가.
- Stage 2~4: 위 구성에 Z02 나팔 파티 좀비 추가.
- Stage 5~7: 기존 큰 좀비까지 유지하고 Z01/Z02를 함께 등장시킨다.
- 파티 종류는 `ZombieKind` 뒤에 추가했으며 기존 Normal/Infected/Large 값은 보존했다. 기존 20개 분포 슬롯에 파티 10개 슬롯을 더한 30개 순환을 사용한다. Stage 1은 파티 슬롯 전체 Z1, Stage 2부터 파티 슬롯을 Z1/Z2가 반씩 쓴다. 적 수·HP·속도·보상·공격 판정 시점은 기존 값을 유지한다.

실제 자연 진행에서 Stage 0은 Infected만, Stage 1은 Normal/Infected/PartyBalloon, Stage 2~4는 여기에 PartyHorn, Stage 5~7은 Large까지 포함되는 것을 검증했다.

## 에셋과 효과

사용자 ZIP의 11개 파일을 원문 바이트 그대로 `Assets/Artsource/Drafts/PartySlime/v001/Originals`에 보존했다. 4×4 시트를 120px / 16프레임 / 단일 합성 레이어의 편집 가능한 Aseprite 정본 10개와 PNG 160개로 정리했다. Idle/Walk/Attack/Dead 태그, 프레임 시간, 원본 해시, 픽셀 변환 방법은 해당 README와 QA에 있다. Z03~Z10은 준비용 아트이며 실제 스테이지에는 배치하지 않았다.

pixelfx 스킬의 고정 소스 검증 → 커스텀 효과 작성 → 시드 고정 export → 밝고 어두운 배경 검수 → Unity 연결 순서를 사용했다. `party/slime-pop` / seed21 / 30fps / 112×112 / 22프레임이다. 현재 출력은 `Assets/HunterGrandma/Art/VFX/PartyPop/v002`이며 `.png`, Aseprite식 `.json`, `.pixelfx.json`, 개별 프레임, 레시피와 MIT 고지를 포함한다. 마지막 프레임은 투명하다. Patch Pixel 폰트는 사용하지 않았다.

기존 `Zombie120AnimationSet.asset`의 GUID를 유지하면서 두 종류와 효과를 연결했다. 팝 효과는 치명타 후 .09 combat seconds에 시작해 원본 사망 동작과 함께 재생된다. `ZombiePopVisual`은 좀비 풀 객체당 한 개의 렌더러를 재사용하고 같은 깊이 정렬 그룹을 따른다. 일시정지·히트스톱·스킬 정지 시 `CombatDeltaTime`과 함께 멈추며 완료/재사용/Despawn/Disable/재도전 때 초기화한다. 보상이나 피해를 발생시키지 않는다.

기존 씬의 좀비에는 애니메이션 연결이 있었지만 생성용 `Zombie.prefab`에는 없었다. 초기 런타임 검사에서 이 문제를 발견해 에디터 구성 단계가 생성용 프리팹에도 같은 에셋을 연결하도록 고쳤다. 프리팹의 GUID와 파일 ID는 유지했다.

## 검증

| 실행 | 결과 | 근거 |
|---|---|---|
| Aseprite 저장/재오픈·PNG 픽셀 일치 | 10종 / 160프레임 PASS | source QA의 `aseprite-frames.txt` |
| 단일 레이어 기본·단독·숨김 복사본 저장/재오픈 | 10종 PASS | `aseprite-layers.txt`, 숨김 렌더 전체 투명 확인 |
| 알파 규격·원본 보존 | 160 PNG binary alpha, ZIP 11파일 바이트 일치 | `source-validation.json` |
| pixelfx 회귀 검사 | 8 tests PASS | `node --test Tools/PixelFX/tests/export.test.mjs` |
| 현재 레시피 재생성 | sheet/manifest/22프레임 총 24파일 바이트 일치 | `Library/PartySlime/reproduction-v002` |
| Unity 에셋·Play Mode 효과 검사 | **230 checks PASS** | [PartySlime-validation.md](PartySlime-validation.md) |
| 실제 웨이브/이동/클리어/지도/저장 회귀 | **222 checks PASS** | [PartySlime-stage-regression.md](PartySlime-stage-regression.md) |
| 최종 에셋·씬 참조 재검사 | **31 checks PASS** | [PartySlime-asset-references.md](PartySlime-asset-references.md) |
| 검증 복사본과 최종 적용 파일 비교 | 201개 코드/에셋 동일, Unity 생성 metadata 265개 반영 | [PartySlime-preservation.json](PartySlime-preservation.json) |

Play Mode는 원본을 복사한 `Library/BossFreeStageBatch` 프로젝트에서 같은 Unity 버전으로 실행했다. 별도의 검증 저장 파일을 사용했다. 230개 검사에는 두 방향 표시, 중복 처치 보상 방지, 깊이 정렬, pause/resume, Z2 효과, 재도전 정리, 일반 종류로 재사용, 20회 연속 재사용, Disable/Despawn, 사망 후 풀 복귀가 포함된다. 전체 진행의 최종 클리어 횟수는 `[2,1,1,1,1,1,1,1]`, 포획 수는 0이다.

직접 확인한 D3D11 렌더: [두 종류 배치](PartySlime-lineup.png), [Z1 팝](PartySlime-pop.png), [Z2 팝](PartySlime-horn-pop.png), [반대 방향 팝](PartySlime-right-pop.png). 원본 캐릭터와 크기가 어울리고 발이 바닥에 놓이며 팝이 타깃 위치에서 재생됨을 확인했다. 이는 실제 씬의 오프스크린 렌더이며 네이티브 Game View를 직접 조작한 검수는 아니다.

첫 두 기능 검사 실패는 `Library/PartySlime/attempt1.md`, `attempt2.md`에 보존했다. 생성용 프리팹의 누락 참조를 수정한 뒤 마지막 실행이 통과했다. Unity Editor Search 시작 시의 기존 `SearchInit.IndexationOnStartup` 예외는 게임 런타임 오류와 분리해 각 보고서에 기록했다. 기능 검사와 전체 진행에서 게임 런타임 오류/예외/assertion은 0이다.

ProjectSettings, 씬, 기존 할머니 Aseprite 원본과 기존 GUID metadata를 포함한 보호 파일 **690개**의 해시가 유지됐다. 기존 사용자 변경은 되돌리지 않았다. Web/모바일/독립 실행 파일 빌드와 장시간 성능 측정은 이번 검증에 포함하지 않았다.

## 다시 실행

Unity 에셋 구성: `Tools → Hunter Grandma → Import Party Slime Bundle`. 스테이지 구성은 기존 `Configure Stage v2` 메뉴를 사용한다. 에디터 코드/에셋이 같은 버전인 격리 복사본에서 `HunterGrandma.Editor.PartySlimeValidation.RunBatch`를 `-batchmode -noaudio`로 실행하면 기능 검사와 실제 렌더를 재현한다. `GrandmaStageValidation.RunBatch`는 전체 스테이지 자연 진행을 검사한다. 검사 진입점에 `-quit`를 함께 전달하지 않는다.
