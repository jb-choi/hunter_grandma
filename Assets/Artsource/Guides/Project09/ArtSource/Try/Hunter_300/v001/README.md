# 사냥꾼 300×300 idle 비교 시험본

v001 · 2026-10-11 · **idle 1동작 / 2프레임 / 300×300 캔버스**

[1254 시험본](../../Hunter_1254/v001/README.md)과 같은 폴더 체계로, 기존 idle만 300×300으로 축소했다. 새로운 원화나 동작을 생성하지 않았으며, Unity에는 적용하지 않았다.

[비교 재생기](Previews/index.html) · [로컬 실행 중 재생기](http://127.0.0.1:53155/Previews/index.html) · [idle GIF](Previews/idle.gif)

## 먼저 볼 파일

- [idle.aseprite](Clips/HU-01_idle/idle.aseprite): 300×300, 2프레임, 각 500ms, 1초 반복. 이 파일에서 키포즈를 수정한다.
- [1번 프레임](Clips/HU-01_idle/Frames/idle_01.png) / [2번 프레임](Clips/HU-01_idle/Frames/idle_02.png): 실제 300×300 투명 PNG.
- [프레임 나란히 보기](Previews/idle_contact.png): 두 프레임을 원래 크기로 배치한 600×300 이미지.
- [1254 편집 원본 보관본](Clips/HU-01_idle/Source/idle_1254.aseprite): 축소에 사용한 파일을 바이트 그대로 복사해 보관했다.

**300은 캔버스의 가로·세로 크기다.** 투명 여백을 포함하므로 두 프레임 모두 캐릭터의 보이는 높이는 234px다. 캐릭터 자체를 300px 높이로 맞춘 파일은 아니다.

## 제작 목록

| ID / 태그 | 캔버스 | 프레임 / 타이밍 | 편집 원본 | 시트 / 메타 |
| --- | --- | --- | --- | --- |
| HU-01 / idle | 300×300 | 2장 / 각 500ms / 반복 | [Aseprite](Clips/HU-01_idle/idle.aseprite) | [600×300 PNG](Clips/HU-01_idle/Export/idle_sheet.png) / [JSON](Clips/HU-01_idle/Export/idle.json) |

걷기·조준·발사 등 다른 동작은 이 시험 폴더에 포함하지 않는다.

## 폴더 체계

```text
Reference/
  hunter_base_1254.png            # 선택한 기준 그림, 원형 보관
  hunter_base_300.png             # 기준 그림을 300×300으로 축소
  idle_1254_01.png                # 비교용 1254×1254 프레임
  idle_1254_02.png
Clips/HU-01_idle/
  Source/idle_1254.aseprite       # 축소에 사용한 편집 원본 보관본
  Source/idle_poses_imagegen.png  # 기존 1254 시험의 생성 시트 보관본
  Frames/idle_01.png             # 300×300 개별 프레임
  Frames/idle_02.png
  idle.aseprite                  # 300×300 편집·재생 파일
  Export/idle_sheet.png           # 가로 2장, 600×300
  Export/idle.json                # 프레임·타이밍·출처 기록
Previews/
  index.html                     # 300 / 1254 비교 재생기
  manifest.json
  idle.gif                       # 실제 300×300, 1초 반복
  idle_contact.png               # 600×300 프레임 접촉판
Prompts/                         # 기존 idle 생성 프롬프트 보관
QA/                              # 축소 스크립트·파일 검수 기록
```

## 축소 방식과 비교 범위

Aseprite에서 **Nearest Neighbor(최근접 이웃)** 방식으로 기존 1254×1254 idle을 300×300으로 축소했다. 두 프레임의 타이밍과 반복 태그를 유지하며, 캔버스와 캐릭터 위치를 같은 비율로 줄였다. [재구성 스크립트](QA/build_300.lua)와 [파일 검수 기록](QA/file_validation.json)을 함께 보관한다.

이번 작업에서는 imagegen을 새로 호출하거나 블러·안티앨리어싱·팔레트 정리를 추가하지 않았다. 최근접 축소는 새 중간색을 보간하지 않지만, 원본에 있던 중간색과 반투명 경계가 모두 단단한 픽셀 경계로 바뀌는 것은 아니다. 원본의 색 변화와 프레임 간 모자·얼굴 형태 차이는 남아 있다.

300 파일의 인상과 키포즈 편집 난이도를 1254 파일과 비교하는 시험이다. 300 격자에서 처음부터 다시 그린 원화나 정식 애니메이션 완료본으로 기록하지 않는다. 메타 파일은 이 시험의 기록이며 게임의 런타임 연결 코드는 포함하지 않는다.

[생성 출처와 프롬프트](Prompts/README.md) · [idle 상세 기록](Clips/HU-01_idle/README.md)
