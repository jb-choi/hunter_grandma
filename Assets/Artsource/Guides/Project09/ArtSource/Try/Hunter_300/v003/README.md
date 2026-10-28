# 사냥꾼 300×300 idle 면 색 정리 시험본

v003 · 2026-10-11 · idle 2프레임 · 각 500ms · 1초 반복

[v002](../v002/README.md)는 전체 색 수를 줄였지만 같은 면 안에 작은 명암 조각이 남아 있었다. v003은 모자·옷·머리카락·가죽 장비를 구분하여 잔색을 합치고, 녹색 면의 그림자를 직접 지정한 비교본이다. **300×300 Indexed, 불투명 16색 + 투명 1칸**을 사용한다. 두 프레임 모두 실제 불투명 16색이며 반투명 픽셀은 0개다.

[편집 파일](Clips/HU-01_idle/idle.aseprite) · [비교 재생기](Previews/index.html) · [로컬 재생기](http://127.0.0.1:53157/Previews/index.html) · [idle GIF](Previews/idle.gif)

## 확인할 파일

- [색 정리 전후](Previews/idle_before_after.png): 600×600. 위쪽은 v002 두 프레임, 아래쪽은 v003 두 프레임이다.
- [1번 프레임](Clips/HU-01_idle/Frames/idle_01.png) / [2번 프레임](Clips/HU-01_idle/Frames/idle_02.png): 실제 300×300 투명 PNG.
- [1번 4배 확대](Previews/idle_zoom_01.png) / [2번 4배 확대](Previews/idle_zoom_02.png): 보간 없이 확대한 1200×1200 검토용 PNG.
- [프레임 나란히 보기](Previews/idle_contact.png): 600×300.
- [팔레트 색표](Previews/palette.png) · [GPL](Clips/HU-01_idle/Export/hunter_idle.gpl) · [팔레트 JSON](Clips/HU-01_idle/Export/palette.json).

300은 캔버스 크기다. 투명 여백을 제외한 캐릭터 높이는 두 프레임 모두 234px다. v002의 투명 영역과 실루엣을 그대로 유지했으며, 색 경계는 이번 면 정리에 따라 달라졌다.

## 정리 방식과 남은 작업

녹색은 기본색 `#487040`, 그림자 `#355B32`, 밝은색 `#AEC95C`로 고정했다. 기존의 작은 명암 조각을 그대로 따라가지 않고 모자·챙·목도리·몸통·소매·바지·다리에 필요한 면을 지정했다. 같은 물체 안의 고립된 잔색을 합쳤으며 디더링·색 평균·새 반투명 색을 만들지 않았다. [정리 명세](Prompts/pixel_cleanup_spec.md), [재구성 스크립트](QA/clean_planes.lua), [파일 검수 기록](QA/file_validation.json)에 규칙과 수치를 남겼다.

기존 300 그림의 색 면을 정리한 시험이다. 일정한 저해상도 도트 격자로 다시 그린 완성본은 아니며, 프레임 간 모자·얼굴 형태 변화와 포즈 차이는 남아 있다. 새 이미지 생성은 사용하지 않았고 Unity에는 적용하지 않았다.

## 폴더 체계

```text
Reference/                         # 기준 그림 및 v002 비교 PNG
Clips/HU-01_idle/
  Source/idle_300_v002.aseprite     # 직접 사용한 v002 원본 보관본
  Frames/idle_01.png               # v003 개별 프레임
  Frames/idle_02.png
  idle.aseprite                    # v003 편집 파일
  Export/idle_sheet.png            # 가로 2장, 600×300
  Export/idle.json                 # 프레임·타이밍·정리 규칙
  Export/hunter_idle.gpl
  Export/palette.json
Previews/                          # 전후 비교·4배 확대·재생기·GIF·색표
Prompts/                           # 생성 출처 안내와 직접 픽셀 수정 규칙
QA/                                # 재구성 스크립트·검수 기록
```

키포즈 수정은 [v003의 idle.aseprite](Clips/HU-01_idle/idle.aseprite)에 진행한다. 좌표 없이 영역을 지정하려면 별도 `색정리_지시` 레이어에 원하는 면을 눈에 띄는 색으로 표시하고 저장한다. 작업자가 이 레이어를 수정 범위로 읽고 실제 그림에서는 제외할 수 있다. 자세한 지시 예시는 [정리 명세](Prompts/pixel_cleanup_spec.md)에 있다.

[idle 상세](Clips/HU-01_idle/README.md) · [원본 출처](Clips/HU-01_idle/Source/README.md) · [출처와 수정 규칙](Prompts/README.md)
