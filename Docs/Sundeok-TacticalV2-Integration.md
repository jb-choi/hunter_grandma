# 순덕 걷기·조준·발사 Tactical v2 연결

2026-11-02 사용자 요청. 편집 정본 `Assets/Artsource/Drafts/GrandmaShooter120/v002/grandma_01_sundeok_120.aseprite`의 현재 43프레임/9태그에서 게임용 동작을 선택했다.

| 게임 동작 | master 프레임 / 태그 | 게임 재생 |
| --- | --- | --- |
| Walk | 28–33 / walk_v2_tactical | 6×140ms, 반복 |
| Aim | 40 / fire_v2_tactical의 발사 준비 | 420ms 한 포즈 유지 |
| Fire | 40–43 / fire_v2_tactical | 100/70/100/150ms, 단발 |

원본에는 별도 aim_v2_tactical 태그가 없다. 새 조준 작화를 만들거나 기존 Aim을 전술 태그로 이름만 바꾸지 않고, 실제 v2 발사 직전 준비 자세를 조준에 연결했다. 총구 섬광은 Fire 두 번째 프레임(master 41)에만 있다. Idle/Reload/Collect는 이번 교체 대상이 아니며 기존 동작을 사용한다. 원본의 reload_v2_tactical은 후속 선택이 가능하도록 정본에 남아 있다.

## 게임용 데이터

`Assets/HunterGrandma/Art/Sundeok120/TacticalV2/`에 선택한 11포즈의 Composite/Body/Upper를 각각 전체 120×120 캔버스로 추출했다(33PNG). 원본 레이어 표시 상태와 픽셀을 보존해 읽었으며 native는 저장하지 않았다. Body는 무기/팔 없는 복원 몸체, Upper는 BackArm/Weapon/FrontArm/Ammo/Pickup/FX를 원래 순서로 합성한다. 원본의 검증된 합성 PNG 및 Body+Upper 재합성과 픽셀 차이 0을 확인했다. 근거: `Docs/Validation/TacticalV2-export-validation.json`.

현재 선택은 기존 `Art/Sundeok120/pose_metadata.json`에 기록한다. 해당 행만 `folder=TacticalV2/`를 사용하고, Editor importer는 그 경로를 선택한다. 기존 v1 PNG는 보존한다. `Sundeok120AnimationSet.asset`의 GUID와 씬 참조를 유지하고 데이터 내용만 바꾼다. 6개 게임 동작의 활성 포즈 슬롯은 25개다. 조준 한 포즈를 여러 개로 복제해 신규 작화 수를 늘려 세지 않는다.

32 PPU, 바닥 피벗(60,118) / Unity 픽셀 피벗(60,2), Point 필터, 비압축을 유지한다. 머리/몸체 비례가 다른 v2에서 이전 픽셀 색상 탐색만으로 상체를 정렬하면 보행 상하 이동이 1px 이상 어긋날 수 있어 `attachment_pixels`를 사용한다. 원본 v1 Aim3의 런타임 부착 기준(47,49)에 v2 제작 메타데이터의 준비 자세 이동을 더한다. 독립적인 발사 반동(0/−3/−1/0px, 세로 0/−1/0/0px)은 부착 기준에서 제외해 이동 사격 중에도 사라지지 않도록 한다. 각 보행 프레임에 조준 상체를 얹으면 원본 전술 Walk의 손/총구 위치와 일치한다.

`SundeokAnimationPlayer` 및 실제 사격/피해/보상/이동 로직은 수정하지 않는다. 장전 두 번째 프레임의 철컥 큐와 일시정지/재도전 취소 경로도 유지한다.

## 검증과 보존

이번 작업 시작 시 원본 master·파츠 7개·게임 실행 코드·씬·장전음·좀비 동작 데이터·일반 사용자 저장·프로젝트 설정의 새 SHA-256 기준을 만들었다. 수정 전 데이터와 Editor 코드는 무시되는 `Docs/Backups/TacticalV2Integration-20261102/`에 보존했다. 이전 작업의 보호 기준으로 사용자 저장을 덮어쓰지 않는다.

기존 순덕 애니메이션 검사를 현재 선택 동작과 부착 좌표 기준에 맞게 갱신했다. 별도 Library 프로필과 숨김 Unity batchmode로 컴파일/이동/조준/발사/반전/장전/수거/일시정지/재도전을 검증했다. 자동 검사에서만 AudioListener 출력 음량을 0으로 설정해 게임 음원이 실제 재생 상태를 가지면서 스피커로 출력되지 않도록 한다. 대화형 Editor의 음량이나 오디오 프로젝트 설정은 변경하지 않는다.

- 2026-11-02 01:52 KST: Unity 6000.3.10f1, batchmode / Direct3D11, **애니메이션 42개 PASS**. 25포즈의 합성/피벗/시간, 원본과 일치하는 6개 보행 조준 상체 위치/총구, 실제 자연 사격과 이동, 좌우 발사, 장전/수거/재도전/정지 동작을 검사했다. `Docs/Validation/TacticalV2-animation-validation.md`.
- 이어서 같은 데이터로 batchmode / nographics **장전음 28개 PASS**. 철컥 타이밍/연사/일시정지/음소거/재도전을 검사했다. `Docs/Validation/TacticalV2-reload-audio-validation.md`. AudioSource는 재생을 보고했고 출력은 음소거했으므로 청음/하드웨어 품질 검사로 표기하지 않는다.
- 기존 Unity Editor Search 시작 인덱스 예외는 별도 분류했다. 게임 오류/예외/assertion은 0건이다.
- Unity RenderTexture의 [걷기](Validation/TacticalV2-walk.png), [걷기+조준](Validation/TacticalV2-walk-aim.png), [걷기+발사](Validation/TacticalV2-walk-fire.png), [왼쪽 발사](Validation/TacticalV2-left-fire.png)를 캡처했다. 새 좁은 앞꿈치 보행과 상체/샷건 접점, 별도 섬광을 확인했다. 이는 현재 사용자의 native Game View를 조작/검증한 결과가 아니다.
- Idle/Reload/Collect의 기존 SO 직렬화 블록은 수정 전과 동일하다. 원본/사용자 저장/씬/게임 실행 코드/오디오/프로젝트 설정 보호 파일18개는 해시가 유지됐다. 최종 사본 대조는 `Docs/Validation/TacticalV2-preservation.json`에 있다. 전체 스테이지 검사를 이번에 다시 실행했다고 표기하지 않는다.

컴퓨터 사용 기능, 원본 재생성/덮어쓰기, 패키지 변경과 플레이어 빌드는 수행하지 않았다.
