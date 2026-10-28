# 사냥꾼 1254×1254 애니메이션 시험본

v001 · 2026-10-11 · **8동작 / 36프레임 시안·재생 시험 / 정식 해상도 결정 전**

사용자가 선택한 고해상도 사냥꾼을 바탕으로 Notion 「3. 사냥꾼 애니메이션 제작 목록」을 시험 제작했다. 현재 32×32 베이스와 기존 게임 자산은 별도로 유지한다.

[재생 플레이어](Previews/index.html) · [로컬 실행 중 플레이어](http://127.0.0.1:53154/Previews/index.html) · [제작 명세](../../../../Docs/AI/Art/02_Production/HU-Animations1254/Specification.md)

## 먼저 볼 파일

- [선택한 1254×1254 원본](Reference/hunter_base_1254.png): 사용자 첨부를 바이트 그대로 복사한 기준 그림.
- [걷기 GIF](Previews/walk.gif), [조준 GIF](Previews/focus.gif), [발사 GIF](Previews/shoot.gif).
- 플레이어에서 8동작을 선택하고, 320px 작화 확인과 42/68/96px 축소 표시를 비교한다. 32×32 후보를 같은 보이는 높이로 나란히 보여준다.
- GIF는 384×384 축소 타이밍 미리보기이며 반복 표시한다. PNG 플레이어는 실제 1254 프레임과 클립의 루프/단발/마지막 포즈 유지 규칙을 사용한다.

로컬 플레이어 주소가 열리지 않으면 PowerShell에서 아래 서버를 실행한 뒤 같은 주소로 접속한다. 서버는 이 시험 폴더만 `127.0.0.1:53154`에 제공한다. 이미 열리는 경우 추가 실행할 필요가 없다.

```powershell
& 'C:\Users\Junbeom_Choi\.cache\codex-runtimes\codex-primary-runtime\dependencies\node\bin\node.exe' 'E:\unity_project\project09_hunting_idle\Hunting_idle\ArtSource\Try\Hunter_1254\v001\QA\serve_preview.cjs'
```

## 제작 목록

| ID / 태그 | 프레임 | 재생 | 편집 원본 | 시트 / 메타 | 미리보기 |
| --- | ---: | --- | --- | --- | --- |
| HU-01 / idle | 2 | 루프 | [Aseprite](Clips/HU-01_idle/idle.aseprite) | [PNG](Clips/HU-01_idle/Export/idle_sheet.png) / [JSON](Clips/HU-01_idle/Export/idle.json) | [GIF](Previews/idle.gif) |
| HU-02 / walk | 6 | 루프 | [Aseprite](Clips/HU-02_walk/walk.aseprite) | [PNG](Clips/HU-02_walk/Export/walk_sheet.png) / [JSON](Clips/HU-02_walk/Export/walk.json) | [GIF](Previews/walk.gif) |
| HU-03 / focus | 4 | 마지막 유지 | [Aseprite](Clips/HU-03_focus/focus.aseprite) | [PNG](Clips/HU-03_focus/Export/focus_sheet.png) / [JSON](Clips/HU-03_focus/Export/focus.json) | [GIF](Previews/focus.gif) |
| HU-04 / shoot | 4 | 단발 | [Aseprite](Clips/HU-04_shoot/shoot.aseprite) | [PNG](Clips/HU-04_shoot/Export/shoot_sheet.png) / [JSON](Clips/HU-04_shoot/Export/shoot.json) | [GIF](Previews/shoot.gif) |
| HU-05 / collect | 4 | 단발 | [Aseprite](Clips/HU-05_collect/collect.aseprite) | [PNG](Clips/HU-05_collect/Export/collect_sheet.png) / [JSON](Clips/HU-05_collect/Export/collect.json) | [GIF](Previews/collect.gif) |
| HU-06 / carry_small | 6 | 루프 | [Aseprite](Clips/HU-06_carry_small/carry_small.aseprite) | [PNG](Clips/HU-06_carry_small/Export/carry_small_sheet.png) / [JSON](Clips/HU-06_carry_small/Export/carry_small.json) | [GIF](Previews/carry_small.gif) |
| HU-07 / drag_large | 6 | 루프 | [Aseprite](Clips/HU-07_drag_large/drag_large.aseprite) | [PNG](Clips/HU-07_drag_large/Export/drag_large_sheet.png) / [JSON](Clips/HU-07_drag_large/Export/drag_large.json) | [GIF](Previews/drag_large.gif) |
| HU-08 / load | 4 | 단발 | [Aseprite](Clips/HU-08_load/load.aseprite) | [PNG](Clips/HU-08_load/Export/load_sheet.png) / [JSON](Clips/HU-08_load/Export/load.json) | [GIF](Previews/load.gif) |

개별 투명 PNG는 각 `Clips/<ID_태그>/Frames/`에 있다. **36개 모두 캔버스가 1254×1254**다. 상태별 프레임 수는 2/6/4/4/4/6/6/4다. 왼쪽은 반전 재사용 검토 대상으로 별도 36장을 만든 것으로 계산하지 않는다.

## 원본·출력·저장 방식

```text
Reference/                         # 선택 원본과 32×32 비교본
Clips/HU-01_idle/                   # HU-02~08도 같은 체계
  Source/idle_poses_imagegen.png    # 생성 원본 시트, 원형 보존
  Frames/idle_01.png                # 1254×1254 개별 프레임
  idle.aseprite                    # 타이밍·태그가 있는 시험 편집 원본
  Export/idle_sheet.png             # 규격 시트
  Export/idle.json                  # duration·이벤트·좌표·생성 출처
Previews/                          # 플레이어·384 GIF·접촉판
Prompts/                           # 8개 생성 프롬프트
QA/                                # 재구성 스크립트·검수 기록
```

내장 **image_gen 편집 모드**로 각 동작의 새로운 포즈 시트를 생성했다. [프롬프트 모음](Prompts/README.md)을 보존했다. 생성 시트는 원본마다 크기가 다르다. 개별 포즈를 Aseprite에서 추출해 같은 1254 캔버스에 배치했으며, **36장을 각각 1254 단독 원화로 생성한 작업은 아니다.** 원본의 도트 모양은 균일한 32×32 격자를 의미하지 않는다.

정규화는 원본 시트의 빈 영역을 분할선으로 찾아 모자/깃털/손/화살을 보존했다. 각 클립에서 하나의 배율을 사용해 웅크린 자세를 늘리지 않았다. 최대 보이는 높이 980px와 바닥 경계 y1115를 시작값으로 썼으며, 견인 줄까지 들어오도록 동작별 머리 기준 x를 메타에 기록했다. 실제 픽셀 반올림으로 바닥에는 최대 1px 차이가 있다. 알파 32 미만의 희미한 외부 노이즈를 제거했고 나머지 생성 알파는 유지했다.

시트는 2프레임 2508×1254, 4프레임 2508×2508, 6프레임 3762×2508이다. 런타임 읽기 코드를 만든 메타는 아니다.

## 확인과 보정 항목

[파일 검수](QA/file_validation.json) · [전투/수집 시각 검토](QA/combat_visual_review.md) · [재생 플레이어 검토](QA/player_review.md)

새로운 동작 포즈·캔버스·프레임 수·타이밍 시연을 제작했다. 생성 시안에는 다음 보정 항목이 남아 있어 정식 애니메이션 완료나 Unity 적용으로 기록하지 않는다.

- 프레임 간 모자·얼굴·활의 크기/형태 변화, 걷기/운반의 반대 발 접지와 루프 연결.
- 조준 손·시위 접점, 발사 첫 프레임에 그려진 화살과 별도 투사체의 분리.
- 수집/견인 시 활 보관 외형, 동물·수레를 연결할 손과 운반/적재 접점.
- 장비는 시험 합성본에 포함되어 있다. 정식 채택 시 몸/활을 분리하고 클립별 기준점을 같은 게임 좌표에 연결해야 한다.

1254 출력은 세부 모습을 보존하지만, 실제 작은 표시에서는 디테일이 줄어든다. RGBA32 36프레임의 순수 텍스처 데이터는 mip 없이 약 **215.95 MiB**다. 이는 PNG 디스크 용량이나 측정된 Unity 메모리가 아니다. 이 시험본을 보고 작화 방향과 게임 출력 크기를 결정한다.
