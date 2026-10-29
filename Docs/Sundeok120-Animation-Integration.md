# 순덕 120px v002 · 사냥씬 애니메이션 통합

2026-10-29 사용자 지정 원본: `Assets/Artsource/Drafts/GrandmaShooter120/v002/`.

## 연결 범위

| 동작 | 원본 프레임/기간 | 런타임 진입 |
| --- | --- | --- |
| Idle | 4 / .88초 | 멈춰 있고 전투 동작이 없을 때 반복 |
| Walk | 6 / .66초 | 실제 발 이동에 따라 반복. 수동/자동 탐색·수레 접근/귀환에서 사용 |
| Aim | 3 / .42초 | 현재 샷건 판정으로 공격할 수 있는 표적 조준. 다음 발사까지의 준비 시간에 맞춰 재생 후 끝 자세 유지 |
| Fire | 4 / .43초 | 첫 프레임은 예정 발사 직전 준비, 실제 발사에서는 두 번째 총구 섬광 프레임으로 진입. 후퇴/회복은 원본 프레임 |
| Reload | 6 / .90초 | Fire 회복 이후. Fire/Reload 배속은 기존 발사 간격에 맞추며 탄약 수/새 발사 대기 규칙은 추가하지 않음 |
| Collect | 4 / .68초 | 처치 후 요청을 한 건만 보관했다가 이동·조준·사격/장전이 없는 순간 재생. 수동 이동·새 조준은 중단 가능 |

기존 자동 사냥은 임시 단일 순덕 로직이다. 다른 할매 전투/AI, 새 탄약 경제, 물리적인 아이템 찾아가기/줍기, Hit/Victory, 새 차 마시기 원화는 추가하지 않는다. 기존 64px 차 마시는 도입은 유지한다. 이번 120px 폴더에 있는 캐릭터는 순덕 한 명이다.

## 표현과 게임 규칙

`SundeokAnimationSet`은 6클립·27프레임의 합성/몸체/상체 이미지, 프레임 시간, 총구/정렬 좌표를 보관한다. `SundeokAnimationPlayer`는 이미지 선택만 담당하고 `GrandmaShooter`가 수명·오디오·좌우 방향·실제 이동 정보를 연결한다. 기존 SpriteRenderer/BeltActorVisual 구조를 확장했으며 Animator/root motion을 새로 도입하지 않았다.

이동하며 Aim/Fire/Reload 중이면 Walk의 무기 없는 몸체 위에 해당 상체를 합성한다. 원본 Lua 시연과 같은 머리 중심 X/어깨 높이 정렬을 사용한다. 전신 대기/이동/줍기는 원본 합성 PNG를 사용한다. 새 프레임을 쓰는 동안 기존 절차적 반동/상하 흔들림을 중복 적용하지 않는다.

120×120 전체 캔버스를 보존하고 PPU=32, 피벗=(60,2)px/정규화(.5,2/120), 표시 배율 .8을 사용한다. 원본 바닥 경계 y118이 논리 발 위치에 맞는다. 실제 총구는 프레임별 원본 좌표와 이동 상체 보정에서 계산하며 왼쪽도 함께 반사한다. 발사 프레임을 먼저 적용한 뒤 기존 투사체를 해당 총구에서 출발시킨다. X/Y 사격 판정·사거리·피해·발사 간격 공식은 기존 Defense/BeltSpace에 남는다.

Fire와 Reload의 전체 원본 시간은 1.33초다. 현재 발사 간격으로 나눈 배속을 사용해 기존 강화 수치를 유지한다. 낮은 프레임레이트/매우 빠른 발사 간격에서는 표시 샘플이 일부 프레임을 건너뛸 수 있지만 실제 발사 콜백에서는 섬광 프레임을 최소 한 렌더 프레임 유지한다.

Collect는 기존 보상이 이미 한 번 지급된 뒤의 표현이다. 애니메이션에서 재화를 더하거나 실제 저장을 다시 정산하지 않는다. 표적이 없고 자동 이동 중이면 .68초 줍기 동안만 자동 탐색을 멈춘다. 수동 입력은 즉시 중단하고 이동하며 새 표적 조준도 중단한다. 일시정지/도입/hit stop 동안은 클립 시간과 자세가 함께 멈춘다. 재도전/후퇴/실패/수레 이동에서 전투 동작을 정리한다.

## 출력·보존

게임용 경로: `Assets/HunterGrandma/Art/Sundeok120/`.

- Composite 27장은 원본 `Export/Frames/`의 바이트 동일 복사본이다.
- Body 27장은 원본 독립 Body Aseprite를 읽기 전용으로 출력했다.
- Upper 27장은 master를 Library 임시 복사본으로 만든 뒤 Body 표시만 끄고 Aseprite MCP로 출력했다.
- 원본 Aseprite/PNG/메타데이터는 덮지 않는다. 81개 게임 PNG는 Point/FullRect/무압축/no mipmaps로 Unity에서 임포트하고, ScriptableObject와 씬 참조도 Editor 도구가 생성한다.
- `SundeokAnimationSetup`이 게임용 좌표 JSON의 action/local_frame으로 프레임을 묶는다. 게임 UI의 기존 5명 초상 참조는 그대로다.

원본 45개 및 기존 코드/씬·일반 저장/설정 해시는 `Docs/Backups/BeforeSundeok120Integration-20261029/`에 보존했다. 별도로 만든 게임 데이터/메타데이터를 제외하고 원본은 변경하지 않는다.

## 검증

전용 명령: `validate-animation` / `Tools/Hunter Grandma/Run Sundeok 120 Animation Validation`.

별도 `Library/Sundeok120-validation.json`을 사용한다. 자연 사냥 1초 이후 기존 투사체를 취소하고, 죽은 풀 슬롯을 active/disabled로 유지해 추가 스폰을 막는다. 숨긴 예약 표적 하나로 추가 실제 발사 없이 조준을 유지하되 조우·키보드 이동 코드는 계속 실행한다. 합성 키보드로 실제 이동을 조작하고 표현 API를 검사한다. 합성 보상 +7 한 번으로 Collect가 재화를 중복 지급하지 않음을 확인한다. 직접적인 Fire 표현 호출은 게임 발사/피해를 만들지 않는다. 전체 자연 전투/포획/저장 흐름은 기존 벨트스크롤 검사로 별도 회귀 확인한다.

첫 검사에서는 Editor update 사이에 이동 API를 직접 호출해 게임 프레임의 실제 이동 시각과 맞지 않았다. 원본 재합성·데모 정렬·도입/실제 발사·대기는 통과했지만 걷기 검사에서 멈췄으며 `Sundeok120-animation-attempt1.md`에 보존했다. 제품 걷기 판정을 완화하지 않고 검사 쪽을 실제 키보드/플레이어 Update 이동으로 수정했다.

검사 범위: 원본 시간/프레임/피벗/PPU·27장 Body+Upper 재합성 픽셀 일치·12개 원본 데모 정렬값, 실제 사냥과 6동작·이동 중 상체 합성·좌우 총구·일시정지·줍기 중단/보상 1회·재도전 초기화. 보고서: `Docs/Validation/Sundeok120-animation-validation.md`, 화면: `Docs/Validation/Sundeok120-*.png`.

### 실제 실행 결과 · 2026-10-29

Unity 6000.3.10f1 Editor에서 실제 재컴파일·임포트 후 다음 검사를 실행했다. 각 검사는 별도 Library 프로필을 사용하며 일반 저장을 초기화하지 않았다.

| 검사 | 실행 시각 KST | 결과 | 보고서 |
| --- | --- | --- | --- |
| 순덕 120px 애니메이션 | 13:40 | 45개 PASS | `Docs/Validation/Sundeok120-animation-validation.md` |
| 전투·포획·저장 및 Stage 0 반복 | 13:44 | 156개 PASS | `Docs/Validation/BeltScroll-validation.md` |
| Stage 0 도입·울타리·좀비 공격 | 13:46 | 48개 PASS | `Docs/Validation/Stage0-opening-validation.md` |
| 업그레이드 연결 UI | 13:49 | 103개 PASS | `Docs/Validation/UpgradeConnection-validation.md` |

합계 352개 PASS, 각 suite 런타임 오류/예외/assertion 0건. 전투 검사의 의도적인 손상 저장 복구 경고 1건은 예상 결과다. 13:49의 마지막 Editor 코드 변경은 애니메이션 데이터 저장을 `SaveAssetIfDirty`로 한정한 것이며 런타임은 앞선 세 검사와 동일하다. 이후 UI 검사를 완료했다.

원본 45개와 일반 저장·ProjectSettings·EditorBuildSettings·manifest 4개, 총 49개 SHA-256이 모두 작업 시작 시점과 일치한다. 최종 비교는 `Docs/Validation/Sundeok120-preservation.json`에 기록했다. 이 작업 시작 씬과 비교하면 `GrandmaShooter.animationSet` 참조 한 줄만 추가했다. 기존 폰트·초상·도입 참조를 유지했다.

1280×720 게임 캡처에서 걷기/이동 조준/이동 발사/이동 장전/줍기/왼쪽 발사를 확인했다. 대표 화면은 `Docs/Validation/Sundeok120-walk-fire.png`, `Sundeok120-collect.png`, `Sundeok120-left-fire.png`다. 이 캡처에는 검사 fixture의 격리된 배우/입력도 포함되며 전체 자연 진행은 156개 검사로 별도 확인했다. UI 검사는 자동 캡처·실제 UI 이벤트/레이캐스트 방식이며 별도 네이티브 Game View 지도 관찰은 포함하지 않는다. 플레이어 빌드·장시간 성능 검사는 미실행이다.

### 기획 문서 연결

- Notion [기획서_02 재화·성장·동료 해금](https://app.notion.com/p/4067313398e08130965aebafbb62a76d): v1.8, 2.11에 순덕 1명 실제 통합·동작/시간·재생 데이터·검증 범위를 기록했다. 다른 네 할매와 다인 AI의 미완료 상태는 유지했다.
- Notion [기획서_04 화면·UI·작업 상태](https://app.notion.com/p/4067313398e081b5b0d1fe284d521bc3): v2.3, 8.2 E11 및 변경 이력에 연결된 월드 애니메이션·최신 회귀 결과를 기록했다. UI ID는 기존 15개이며 KRA PNG 첨부를 유지했다. 게임 캡처는 로컬 증빙 경로로 기록했고 Notion에 새 첨부하지 않았다.
