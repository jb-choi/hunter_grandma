# 순덕 장전 철컥 효과음

2026-10-30 사용자 요청. 컴퓨터 사용 기능 없이 구현·검증했다.

## 효과음과 연결

- SpringySpringo의 [Gun reload sounds](https://opengameart.org/content/gun-reload-sounds) 중 `shotguncock.wav`. 원본 게시물은 제작자의 에어소프트 총 녹음이며 CC0로 표시한다. 출처와 라이선스는 `Assets/HunterGrandma/Audio/LICENSE.txt`에 보관했다.
- 실제 게임 파일: `Assets/HunterGrandma/Audio/sundeok_reload_clack.wav`. 44.1 kHz/16-bit/모노, 0.436초. 원본의 앞 37.5ms를 덜고 모노 합성·DC 제거·피크 .80 정규화·시작/끝 짧은 페이드를 적용했다. WAV 자체의 피치/템포는 바꾸지 않았다.
- 수정 전 다운로드는 `Docs/References/ReloadAudio-20261030/shotguncock-original.wav`, SHA-256과 처리 내역은 같은 폴더 `processing.json`에 보존했다.
- `SundeokAnimationPlayer`가 Reload 2번째 프레임(원본 .15초 경계)에 도달할 때 한 번 요청한다. 이동 중 상체 합성과 좌우 방향도 같은 타이밍을 쓴다. 프레임을 건너뛰어도 Reload를 표시하는 시점에 경계를 통과했다면 한 번만 재생한다.
- `GrandmaShooter`의 별도 지속 AudioSource가 클립을 재생한다. 매번 새 음원을 생성하지 않는다. 기본 볼륨 .13, 2D, 비반복. 사격 음원의 믹서/음소거/볼륨을 따르며 기존 총소리 피치는 건드리지 않는다.
- 기본 발사 간격에서는 피치 1, 빠른 강화에서는 최대 3으로 짧게 재생한다. 새 사격·전투 중지·숨김·이동 연출·초기화·비활성화는 기존 장전음을 중단한다. 일시정지/연출 동결 시 재생 중인 소스는 Pause/UnPause하므로 다시 처음부터 재생하지 않는다. 누락 클립은 조용히 생략한다.

## 임포트·씬

`GrandmaStageSetup`이 PCM/DecompressOnLoad/샘플레이트 유지/미리 로드로 임포트하고 씬의 reloadSound에 연결한다. Unity 6.3의 `AudioImporter.defaultSampleSettings.preloadAudioData`를 사용한다. 최초 구 API 컴파일 오류는 `Docs/Validation/ReloadAudio-compile-attempt1.md`에 보존했다.

Unity가 생성한 오디오/검사 스크립트 메타데이터를 사용한다. 씬 변경은 `GrandmaShooter.reloadSound` 참조와 `reloadVolume: 0.13` 두 줄이다. 사용자 원화·기존 순덕 프레임·전투/성장/저장 규칙은 수정하지 않았다.

## 실행 검증

Unity 6000.3.10f1, 2026-10-30 16:26 KST, 격리 사본의 batchmode/nographics: **28개 PASS**. 실제 애니메이션/AudioSource의 연결과 프레임 경계, 긴 프레임/이동/좌우/빠른 연사·중복 방지·일시정지·음소거·클립 누락·중단·재도전을 확인했다. AudioSource가 활성 재생을 보고했고 Pause 중 정지도 확인했다. 어쿠스틱 청음·실제 스피커 출력 품질·플레이어 빌드의 검증은 수행하지 않았다.

전투 제어기 Update를 끈 격리 프로필에서 표현만 실행했고, 속도 강화 검사용 합성 재화는 해당 Library 프로필에서만 사용했다. 보통 저장·ProjectSettings·Build Settings·manifest·순덕 동작 데이터 5개 해시가 유지됐다. 검증한 코드/씬/클립/메타데이터 8개는 실제 작업 파일과 바이트 일치한다.

보고서: `Docs/Validation/ReloadAudio-validation.md`, 보존 대조: `ReloadAudio-preservation.json`. 게임플레이 오류/예외/assertion 0건. 이전 배치와 동일한 Unity Editor Search 시작 인덱스 예외 1건은 별도 기록했다. 메뉴 `Tools/Hunter Grandma/Run Reload Audio Validation`, 명령 `validate-reload`, CLI `HunterGrandma.Editor.GrandmaReloadAudioValidation.RunBatch`로 다시 검사할 수 있다.

Notion [기획서_02](https://app.notion.com/p/4067313398e08130965aebafbb62a76d)의 2.11 후속 기록과 [기획서_04](https://app.notion.com/p/4067313398e081b5b0d1fe284d521bc3)의 E13/작업 이력에도 적용 범위·출처·검증을 기록했다. 동시 진행된 보스 취소선/단일 Stage 마커 기획과 기존 상태표는 유지했다.
