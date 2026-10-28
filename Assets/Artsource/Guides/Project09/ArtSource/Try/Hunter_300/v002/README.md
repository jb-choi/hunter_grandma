# 사냥꾼 300×300 idle 색 정리 시험본

v002 · 2026-10-11 · **idle 1동작 / 2프레임 / 300×300 캔버스**

[v001 축소본](../v001/README.md)을 보존하고, 같은 idle의 색과 경계를 정리한 비교본을 별도 폴더에 만들었다. 두 프레임에 **불투명 26색 + 투명 1칸의 공통 팔레트**를 적용했으며, 반투명 픽셀은 남기지 않았다. 새로운 원화·키포즈를 생성하지 않았으며 Unity에는 적용하지 않았다.

[비교 재생기](Previews/index.html) · [로컬 실행 중 재생기](http://127.0.0.1:53156/Previews/index.html) · [idle GIF](Previews/idle.gif)

## 먼저 볼 파일

- [idle.aseprite](Clips/HU-01_idle/idle.aseprite): 색 정리한 300×300 Indexed 편집 파일. 2프레임, 각 500ms, 1초 반복.
- [1번 프레임](Clips/HU-01_idle/Frames/idle_01.png) / [2번 프레임](Clips/HU-01_idle/Frames/idle_02.png): 실제 300×300 투명 PNG.
- [프레임 나란히 보기](Previews/idle_contact.png): 두 프레임을 원래 크기로 배치한 600×300 이미지.
- [v001 1번 프레임](Reference/idle_v001_01.png) / [v001 2번 프레임](Reference/idle_v001_02.png): 색 정리 전 비교 원본.
- [v001 편집 원본 보관본](Clips/HU-01_idle/Source/idle_300_v001.aseprite): 이번 정리에 사용한 파일을 원형 그대로 보관했다.
- [색 정리 전후 비교](Previews/idle_before_after.png): 600×600 이미지. 위쪽은 v001 두 프레임, 아래쪽은 v002 두 프레임이다.
- [팔레트 색표](Previews/palette.png) · [Aseprite용 GPL](Clips/HU-01_idle/Export/hunter_idle.gpl) / [팔레트 JSON](Clips/HU-01_idle/Export/palette.json).

**300은 캔버스의 가로·세로 크기다.** 투명 여백을 제외한 캐릭터 높이는 두 프레임 모두 234px다. 캐릭터 자체의 높이를 300px로 맞춘 파일은 아니다.

## 제작 목록

| ID / 태그 | 캔버스 | 프레임 / 타이밍 | 편집 원본 | 시트 / 메타 |
| --- | --- | --- | --- | --- |
| HU-01 / idle | 300×300 | 2장 / 각 500ms / 반복 | [Aseprite](Clips/HU-01_idle/idle.aseprite) | [600×300 PNG](Clips/HU-01_idle/Export/idle_sheet.png) / [JSON](Clips/HU-01_idle/Export/idle.json) |

걷기·조준·발사 등 다른 동작은 이 시험 폴더에 포함하지 않는다.

## 폴더 체계

```text
Reference/
  hunter_base_1254.png             # 선택한 기준 그림, 원형 보관
  hunter_base_300.png              # 기존 300 기준 그림, 출처 보관
  idle_v001_01.png                 # 색 정리 전 300×300 비교 프레임
  idle_v001_02.png
Clips/HU-01_idle/
  Source/idle_300_v001.aseprite     # 색 정리에 사용한 v001 보관본
  Source/idle_1254.aseprite         # 기존 1254 편집 원본 참고 보관
  Source/idle_poses_imagegen.png   # 기존 생성 시트 참고 보관
  Frames/idle_01.png               # 색 정리한 300×300 개별 프레임
  Frames/idle_02.png
  idle.aseprite                    # v002 편집·재생 파일
  Export/idle_sheet.png             # 가로 2장, 600×300
  Export/idle.json                  # 프레임·타이밍·출처 기록
  Export/hunter_idle.gpl            # 공통 팔레트, Aseprite에서 불러오기
  Export/palette.json               # 팔레트 수치 기록
Previews/                           # 비교 재생기·300 GIF·접촉판·색표
Prompts/                            # 기존 생성 프롬프트·이번 색 정리 규칙
QA/                                 # 색 정리 스크립트·파일 검수 기록
```

## 색 정리 방식과 비교 범위

두 프레임에 같은 팔레트를 적용했다. 편집 파일은 **Indexed 모드, 팔레트 27칸(투명 1 + 불투명 26)**이며 각 프레임에서 실제 사용한 불투명 색은 26색이다. 알파는 0 또는 255만 사용하며 반투명 픽셀은 0개다.

알파값 128 미만은 투명으로, 128 이상은 완전 불투명으로 처리했다. 이때 1번·2번 프레임에서 각각 257·268개의 희미한 픽셀을 제거했다. 색 대응에는 디더링을 사용하지 않았다. 녹색 계열과 머리카락 계열 내부의 고립된 명암 픽셀만 각각 22·28개 정리했으며 눈·피부·외곽·활은 이 내부 보정에서 제외했다. 캐릭터 높이 234px는 유지되었고, 폭은 1번 152→151px, 2번 156px로 기록되었다.

최종 규칙은 [색 정리 명세](Prompts/pixel_cleanup_spec.md)에, 실제 결과와 검수 수치는 [파일 검수 기록](QA/file_validation.json)에 남긴다.

이번 작업에서는 imagegen을 새로 호출하지 않았다. 기존 그림의 미세한 색 변화와 반투명 경계를 줄여 300 출력의 인상을 비교하는 작업이다. 원본의 도트 형태를 일정한 저해상도 격자로 다시 그리거나, idle의 모자·얼굴·호흡 포즈를 새로 설계한 작업은 아니다. 알파와 색 대응 때문에 가장자리의 일부 픽셀은 v001과 달라질 수 있다.

키포즈 수정은 v002의 `idle.aseprite`에서 진행한다. 프레임 간 모자·얼굴 형태 변화와 동작 연결은 별도의 애니메이션 보정 대상이다. 메타 파일은 이 시험의 기록이며 게임의 런타임 연결 코드는 포함하지 않는다.

[재구성 스크립트](QA/cleanup_300.lua) · [Aseprite 검수](QA/native_validation.json) · [재생기 검수](QA/browser_validation.json) · [생성 출처와 정리 규칙](Prompts/README.md) · [idle 상세 기록](Clips/HU-01_idle/README.md)
