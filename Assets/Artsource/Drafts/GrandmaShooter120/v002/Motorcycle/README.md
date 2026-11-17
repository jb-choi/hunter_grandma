# 오토바이 할머니 · 120px · v002

2026-10-30. 기존 v002 오토바이 할머니의 은색 땋은 머리·빨간 스카프·청록 코트·버건디 오토바이를 바탕으로 추가했다. 사용자의 최신 요청에 따라 카우보이 모자를 **둥근 올리브색 1레벨 군용 반모 느낌의 오토바이 헬멧**으로 바꾸고, Collect를 **질주 중 한 손으로 줍고 계속 달리는 동작**으로 만들었다. 표식·로고·총은 없다.

## 바로 열기

- **[편집 정본 · 120×120 / 18프레임](../grandma_05_motorcycle_120.aseprite)**
- **[주행 → 수집 → 계속 주행 · 24프레임 GIF](Previews/drive_by_pickup_world_2x.gif)** · [투명 배경 편집 원본](motorcycle_drive_by_pickup.aseprite)
- **[독립 오토바이](Vehicle/motorcycle.aseprite)** · [PNG](Vehicle/motorcycle.png)
- **[인물만 · 몸/양팔](motorcycle_rider_120.aseprite)** · [8개 파츠 라이브러리](Parts/Native)
- [실제 120px 포즈 시트](Previews/all_poses_native.png) · [2배](Previews/all_poses_2x.png) · [에셋 목록](asset_manifest.json) · [검수](QA/review.md)

## 동작

| 동작 | 프레임/원본 시간 | Aseprite | 확대 GIF | PNG 시트 |
| --- | --- | --- | --- | --- |
| Idle · 탑승 대기 | 4 / 180ms | [원본](Clips/motorcycle_idle.aseprite) | [GIF](Previews/Idle_3x.gif) | [PNG](Export/Sheets/motorcycle_idle.png) |
| Ride · 질주 루프 | 6 / 80ms | [원본](Clips/motorcycle_ride.aseprite) | [GIF](Previews/Ride_3x.gif) | [PNG](Export/Sheets/motorcycle_ride.png) |
| Collect · 달리며 줍기 | 8 / 85ms | [원본](Clips/motorcycle_collect.aseprite) | [GIF](Previews/Collect_3x.gif) | [PNG](Export/Sheets/motorcycle_collect.png) |

기획의 Walk 슬롯은 이 인물의 **Ride**로 표현한다. 내리거나 도보로 줍는 프레임은 넣지 않았다. Collect는 ① 질주 접근 ② 한 손 내리기 ③ 지면으로 뻗기 ④ 주머니 잡기 ⑤ 끌어올리기 ⑥ 몸 일으키기 ⑦ 뒤 가방에 넣기 ⑧ 핸들 복귀다. 바퀴 회전과 전진 효과가 수집 중에도 유지된다. Collect를 8프레임으로 늘려 총 18프레임이며, 기획의 15프레임 가안보다 사용자의 구체적인 수집 연기를 우선했다.

24프레임 시연은 주행 8 + 수집 8 + 주행 8이며 전부 80ms다. 시연의 줍기 이벤트는 12번, 수납은 15번 프레임이다. [데모 좌표/진행량](Source/demo_metadata.json). 도로가 흐르는 GIF는 움직임 확인용 미리보기이고 도로는 투명 캐릭터 원본에 포함하지 않는다.

## 분리 구조와 좌표

아래→위 레이어는 `MotionFX / RearWheel / FrontWheel / Vehicle / BackArm / Body / FrontArm / Pickup`. 기본 전부 표시, Normal, opacity 255. 헬멧·얼굴·땋은 머리·스카프·몸통·탑승 하체는 Body, 팔은 앞뒤로 분리했다. Body에는 팔/오토바이에 가렸던 옷과 다리를 복원했다. 핸들을 잡는 뒤팔과 수집하는 앞팔, 손 접점도 native에서 보완했다.

오토바이 원본에는 **완전한 바퀴 2개 + 인물 없는 차체**가 있다. 탑승자에 가렸던 안장·연료통·엔진·발판을 복원했다. 바퀴는 별도 레이어라 회전 위상을 독립적으로 바꿀 수 있다. 인물 원본에는 차량·바퀴·주머니·속도 FX가 없다.

전체 합성 좌표는 **120×120**이며 오토바이까지 포함한 캔버스다. 실제 실루엣은 측면 탈것의 비율을 유지한다. 전 동작 합집합은 **불투명 RGB 26색 / 공통 26색 상한**, 팔레트 27슬롯(투명 포함), 알파 0/255. [GPL](Export/motorcycle_26.gpl).

- 원점 좌상단, x 오른쪽/y 아래. 바퀴 바닥 마지막 픽셀 y117, 지면 경계 y118, 기본 루트 `(60,118)` / Unity 정규화 `(0.5,0.016667)`.
- 뒤/앞 바퀴 중심 `(25,104)/(99,104)`, 반지름 13px. 전진 시 프레임마다 10도 증가하는 스포크 위상을 사용했다.
- 핸들을 유지하는 손 기준 `(78,72)`; 수집 손/어깨/물건 상태는 [pose_metadata.json](Source/pose_metadata.json).
- `Parts/<동작>/`의 **129개 잘린 PNG 슬롯**은 [motorcycle_parts.json](Export/motorcycle_parts.json)의 `origin_xy`와 레이어 순서로 정확히 재합성된다. 129는 고유 작화 수가 아니다.
- `Export/Frames/`는 120px 투명 합성 PNG, `Export/Left/`는 좌우 반전 검수 PNG, `Export/Sheets/`는 동작별 시트/영역/시간 JSON. 추가 방향 작화는 아니다.
- [컬러 UI 초상](UI/motorcycle_portrait.aseprite) · [PNG](UI/motorcycle_portrait.png), [잠금 초상](UI/motorcycle_portrait_locked.aseprite) · [PNG](UI/motorcycle_portrait_locked.png).

## 제작과 검수

Built-in ImageGen으로 동작 시트 3종, 인물만 남긴 시트 3종, 팔 없는 복원 몸체 시트 3종, 차량 복원 시트 1종을 생성했다. [프롬프트 10건](Source/generation_prompts.json) · [고해상도 생성 원본](Source/Generated) · [현재 기획서 스냅샷](Source/notion_02_snapshot.md). Aseprite Lua에서 최근접 샘플링·팔레트 매핑·헬멧 올리브색 정리·탑승 위치/팔 접점 보정·가려진 팔 보완·바퀴/스포크 위상·작은 주머니/속도 FX를 작업했다. 고해상도 생성물을 모두 120px 수작업 작화로 표기하지 않는다.

재현: `sample_motorcycle.lua` → `build_motorcycle.lua` → `export_validate_motorcycle.lua`. `root`는 이 Motorcycle 폴더의 절대 경로. build는 상위의 편집 정본도 저장하므로 사용자 편집 후 재실행 전 보존본과 해시를 만든다. GIF는 저장된 native 파일에서 Aseprite로 출력했다. GIF 시간은 포맷의 10ms 단위로 양자화되므로 정확한 원본 시간은 Aseprite/시트 JSON을 따른다.

원본 18프레임, 3클립, 분리 인물, 독립 차량, 파츠 라이브러리 8개×18프레임 슬롯, UI 2쌍, PNG/시트/좌우 반전/129개 파츠 재합성과 실제 저장한 solo/hidden 42상태를 검증했다. 원본 비교에서 픽셀 차이 0, 부분 알파 0이다. [검사 JSON](QA/validation.json) · [시각 검수](QA/review.md).

기존 64px 원본과 순덕 편집 정본은 유지했다. 기존 README는 보존 후 오토바이 안내만 추가했다. [보존 비교](QA/source_preservation.json). 이 작업은 로컬 아트 에셋 추가이며 Unity 씬·Animator·게임 로직 연결은 별도다. 수레·전투 역할 D-03과 재화 종류를 새로 확정하지 않았다.
