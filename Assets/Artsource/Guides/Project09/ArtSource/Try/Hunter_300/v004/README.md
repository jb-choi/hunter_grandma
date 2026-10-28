# 사냥꾼 300×300 idle 잔색·색 경계 정리

v004 · 2026-10-11 · idle 2프레임 · 각 500ms · 1초 반복

[v003](../v003/README.md)에 남은 귀·피부·목·깃털·손의 작은 색 조각과 가는 띠를 기존 픽셀에서 직접 정리했다. 전체 디자인·포즈·캐릭터 인상을 유지하고, 같은 면 안의 잔색을 주변 대표색으로 합치는 시험본이다. 300×300 Indexed 캔버스와 기존 팔레트 **17칸(불투명 16색 + 투명 1칸)**을 유지한다.

[편집 파일](Clips/HU-01_idle/idle.aseprite) · [비교 재생기](Previews/index.html) · [로컬 재생기](http://127.0.0.1:53158/Previews/index.html) · [idle GIF](Previews/idle.gif)

## 확인할 파일

- [색 정리 전후](Previews/idle_before_after.png): 600×600. 위쪽은 v003 두 프레임, 아래쪽은 v004 두 프레임이다.
- [1번 귀 부분 비교](Previews/ear_before_after.png) / [2번 귀 부분 비교](Previews/ear_before_after_02.png): 왼쪽 v003, 오른쪽 v004. 각 25×18 영역을 16배 확대해 나란히 놓은 800×288 PNG.
- [1번 프레임](Clips/HU-01_idle/Frames/idle_01.png) / [2번 프레임](Clips/HU-01_idle/Frames/idle_02.png): 실제 300×300 투명 PNG.
- [1번 4배 확대](Previews/idle_zoom_01.png) / [2번 4배 확대](Previews/idle_zoom_02.png): 보간 없이 확대한 1200×1200 검토용 PNG.
- [프레임 나란히 보기](Previews/idle_contact.png): 600×300.
- [팔레트 색표](Previews/palette.png) · [GPL](Clips/HU-01_idle/Export/hunter_idle.gpl) · [팔레트 JSON](Clips/HU-01_idle/Export/palette.json).

## 수정 범위와 확인 기준

귀의 밝은 면은 `#FFF1D2`로 통일하고 주황색 내부와 갈색 테두리는 유지한다. 피부·목·깃털·손 내부의 작은 색 조각을 같은 재질의 대표색으로 합친다. 녹색·머리카락의 큰 명암 면은 남기고, 같은 재질 안의 불필요한 1픽셀 돌출과 패임을 보수적으로 정리한다. 외곽 실루엣과 눈·볼의 디테일은 보호한다.

[사용자 지시와 정리 명세](Prompts/pixel_cleanup_spec.md), [직접 수정 스크립트](QA/cleanup_v4.lua), [파일 검수 기록](QA/file_validation.json)에 작업 범위와 수치를 남겼다. [원본 보관본](Clips/HU-01_idle/Source/idle_300_v003.aseprite)의 동일성은 [출처 해시 기록](QA/source_hash.json)에서 확인한다. [편집 파일 재검수](QA/native_validation.json)와 [재생기 검수](QA/browser_validation.json)를 함께 보관한다.

실제 변경은 1번 프레임 383픽셀, 2번 프레임 398픽셀이다. 두 프레임 모두 불투명 16색을 사용하며 반투명 픽셀은 0개다. 투명 영역·보호 디테일·기존 검정 윤곽의 불일치는 모두 0개로 확인했다. 투명 여백을 제외한 캐릭터 높이는 두 프레임 모두 234px다.

이미지 재생성·블러·안티앨리어싱·디더링은 사용하지 않았다. 기존 두 프레임의 형태·포즈 차이는 남아 있으며, 균일한 저해상도 격자로 다시 그린 완성본은 아니다. Unity에는 적용하지 않았다.

## 폴더 체계

```text
Reference/                         # 기준 그림 및 v003 비교 PNG
Clips/HU-01_idle/
  Source/idle_300_v003.aseprite     # 수정 전 v003 원본 보관본
  Frames/idle_01.png               # v004 개별 프레임
  Frames/idle_02.png
  idle.aseprite                    # v004 편집 파일
  Export/idle_sheet.png            # 가로 2장, 600×300
  Export/idle.json                 # 프레임·타이밍
  Export/hunter_idle.gpl
  Export/palette.json
Previews/                          # 전후 비교·4배 확대·재생기·GIF·색표
Prompts/                           # 사용자 지시와 직접 픽셀 수정 규칙
QA/                                # 수정 스크립트·검수 기록
```

[idle 상세](Clips/HU-01_idle/README.md) · [원본 출처](Clips/HU-01_idle/Source/README.md) · [수정 지시 안내](Prompts/README.md)
