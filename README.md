# Hunter Grandma

Unity 6000.3.10f1 기반 2D 벨트스크롤 사냥 프로토타입입니다.

## 실행

1. Unity Hub에서 이 저장소 폴더를 프로젝트로 추가합니다.
2. Unity 6000.3.10f1로 열고 패키지와 에셋 임포트가 끝날 때까지 기다립니다.
3. `Assets/HunterGrandma/Scenes/GrandmaDefense.unity`를 열고 Play를 누릅니다.

WASD/방향키 이동, Tab 자동 탐색 전환, U 강화, M 지도, Escape 패널 닫기, Space 일시정지, R 재도전. Stage 0 도입은 Enter로 건너뛸 수 있습니다.

## 저장소 구성

- `Assets/`: 런타임 코드, 에디터 도구, 씬, 프리팹, 필수 에셋과 `.meta` 파일. `Assets/Artsource/`에는 편집 원본과 제작 기록도 포함합니다.
- `Packages/`: 패키지 목록과 잠금 파일.
- `ProjectSettings/`: 프로젝트 설정과 Unity 버전.
- `Docs/`: 기획 자료, 구현 기록과 검증 결과.

Unity 캐시, 로컬 설정, 빌드 출력물, 임시 복구 씬, `Docs/Backups/`는 추적하지 않습니다. `Library/`와 IDE 프로젝트 파일은 Unity가 다시 생성합니다.

## UI 폰트와 검증

HUD와 지도는 포함된 `NotoSansCJKkr-Regular.otf`를 공유합니다. `DefenseHud.font`의 씬 참조를 유지해야 합니다. `DefenseUiBuilder`가 패널·버튼·텍스트 생성 기본값을 담당합니다.

UI 리팩터링 검증: 컴파일 오류·경고 0개, UI Play Mode 검사 71개와 도입/공격 검사 48개 통과. 자세한 결과는 [UI 리팩터링 기록](Docs/Validation/UI-refactor-20261028.md)을 참고하세요. 해당 리팩터링 이후 WebGL/standalone 재빌드는 별도로 필요합니다.

전체 구조와 작업 절차는 [프로젝트 컨텍스트](Docs/AI/UnityProjectContext.md)에 정리돼 있습니다. 에디터 검증 도구는 별도 `Library/` 프로필을 사용하므로 일반 플레이 저장 데이터를 초기화하지 않습니다.
