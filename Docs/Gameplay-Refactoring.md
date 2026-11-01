# 전투 흐름과 이동 책임 정리

2026-10-31

## 변경 범위

`GrandmaDefense`에 함께 있던 입력·자동 탐색을 `GrandmaMovement`로 분리했다. 기존 `GrandmaShadowSkill`과 같은 방식으로 encounter가 직접 소유하는 일반 C# 객체이며, 별도 GameObject나 Inspector 연결은 필요하지 않다.

- `GrandmaMovement`: WASD/방향키, Tab 전환, 수동 조작 후 1.1초 대기, 가까운 적 선택, 사격 거리 유지, 깊이 정렬, 장애물 우회, 이동 중 수집 모션 취소를 담당한다. 스테이지 시작 시 이동 타이머를 초기화한다.
- `GrandmaDefense`: 진행 상태, 적·탄환 풀, 저장과 보상을 계속 소유한다. 프레임 처리를 배경 갱신, 이동, 상태 진행, 사격 순서로 읽을 수 있게 정리했다. 상태별 동작은 `AdvanceWave`, `AdvanceWaveBreak`, `BeginLoadingCart`로 나눴다.
- `CanShoot`는 두 클래스가 동일한 판정을 쓰도록 assembly 내부에서만 공유한다. 외부 public API와 serialized 필드 12개는 유지한다.

## 유지한 동작

수동/자동 이동 속도 3.6/2.8, 수동 입력 대기 시간, 목표 점수와 동점 우선순위, 거리·깊이·장애물 판정, 수집의 이른 반환을 유지했다. 한 프레임에 상태 분기를 한 번만 실행하고, 전환 후의 상태로 조준·사격한다. 도입부, pause, hit stop, 스킬의 마지막 프레임, CLEAR의 이른 반환 순서도 유지했다.

최근 작업인 좀비 3종·Stage 7·장전음과 기존 휴면 보스/포획 경로를 보존했다. 씬, 에셋, 설정, 패키지, 저장 형식과 밸런스를 변경하지 않았다.

## 검증과 작업 기준

작업 시작 시점의 미커밋 변경까지 포함한 백업은 `Docs/Backups/GameplayRefactor-20261031`에 있다. Git HEAD로 되돌리면 최근 좀비 작업이 유실될 수 있으므로 해당 백업을 비교 기준으로 삼는다.

실행 결과와 제한은 `Docs/Validation/GameplayRefactor-validation.md`에 기록한다. 원본 Editor를 닫지 않고 기존 `Library/BossFreeStageBatch` 복사본에서 Play Mode 회귀 검사를 실행한다. 해당 복사본은 동일한 로컬 패키지 캐시를 참조하는 최소 manifest를 사용하므로, 원본 프로젝트의 전체 패키지 구성으로 runtime/editor assembly 컴파일도 별도로 수행한다.
