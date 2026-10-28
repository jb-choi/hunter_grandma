# 사냥꾼 300×300 idle 윤곽·재질 경계 정리

v005 · 2026-10-11 · idle 2프레임 · 각 500ms · 1초 반복

[v004](../v004/README.md)에 남은 눈 가장자리의 잔색과 모자·머리카락·피부 경계의 가는 색 띠, 불필요한 픽셀 돌출을 기존 Aseprite 픽셀에서 직접 정리했다. v004에서 보호했던 검정 윤곽과 투명 영역도 필요한 위치에서는 수정한다. 전체 디자인·포즈·캐릭터 인상을 유지하면서 경계의 구체적인 형태를 다듬는 시험본이다.

300×300 Indexed 캔버스와 기존 팔레트 17칸(불투명 16색 + 투명 1칸), 두 프레임의 500ms 타이밍을 유지한다. 이미지 재생성·블러·안티앨리어싱·디더링은 사용하지 않았다.

[편집 파일](Clips/HU-01_idle/idle.aseprite) · [비교 재생기](Previews/index.html) · [로컬 재생기](http://127.0.0.1:53159/Previews/index.html) · [idle GIF](Previews/idle.gif)

## 확인할 파일

- [정리 전후](Previews/idle_before_after.png): 600×600. 위쪽 v004 두 프레임, 아래쪽 v005 두 프레임이다.
- [1번 얼굴 확대 비교](Previews/face_before_after.png) / [2번 얼굴 확대 비교](Previews/face_before_after_02.png): 왼쪽 v004, 오른쪽 v005. 64×48px 영역을 보간 없이 10배 확대해 나란히 놓은 1280×480 PNG다.
- [1번 프레임](Clips/HU-01_idle/Frames/idle_01.png) / [2번 프레임](Clips/HU-01_idle/Frames/idle_02.png): 실제 300×300 투명 PNG.
- [1번 4배 확대](Previews/idle_zoom_01.png) / [2번 4배 확대](Previews/idle_zoom_02.png): 보간 없이 확대한 1200×1200 PNG.
- [프레임 나란히 보기](Previews/idle_contact.png): 600×300.
- [팔레트 색표](Previews/palette.png) · [GPL](Clips/HU-01_idle/Export/hunter_idle.gpl) · [팔레트 JSON](Clips/HU-01_idle/Export/palette.json).

## 수정 범위

눈을 6×13px의 단색 검정 직사각형으로 정돈하고 눈 가장자리의 다른 재질 잔색을 제거했다. 얼굴의 얇은 피부 그림자 띠와 위쪽 경계의 갈색·녹색 잔점, 모자 챙의 녹색 띠, 머리카락에 끼어 있는 가죽색 띠와 다리의 얇은 그림자 띠를 정리했다. 같은 면의 작은 점과 불필요한 윤곽 돌출을 주변 면에 맞췄다. 귀의 의도한 음영, 버클과 활의 가는 줄은 보호했다. 볼터치는 1번 프레임의 경계 돌출 1픽셀만 기본 피부색으로 합쳤다.

[직접 수정 명세](Prompts/contour_cleanup_spec.md), [수정 스크립트](QA/cleanup_v5.lua), [파일 검수 기록](QA/file_validation.json)에 작업 범위와 결과를 남겼다. 수정 전 [v004 보관본](Clips/HU-01_idle/Source/idle_300_v004.aseprite)의 동일성은 [출처 해시](QA/source_hash.json)에서 확인한다. [편집 파일 재검수](QA/native_validation.json)와 [재생기 검수](QA/browser_validation.json)를 함께 보관한다.

변경 픽셀은 1번 프레임 105개, 2번 프레임 149개로 총 254개다. 투명/불투명 경계가 바뀐 위치는 각각 3개와 6개, 검정 윤곽이 바뀐 위치는 48개와 60개다. 볼터치 경계 1픽셀을 제외한 지정 보호 디테일의 불일치는 두 프레임 모두 0개다. 두 프레임 모두 불투명 16색을 사용하고 반투명 픽셀은 0개이며, 캐릭터 높이는 234px다.

기존 두 프레임의 형태·포즈 차이와 원본의 불규칙한 도트 간격은 남아 있다. 모든 경계를 새로 그려 정돈한 완성본은 아니며, Unity에는 적용하지 않았다.

## 폴더 체계

```text
Reference/                         # 기준 그림 및 v004 비교 PNG
Clips/HU-01_idle/
  Source/idle_300_v004.aseprite     # 수정 전 v004 원본 보관본
  Frames/idle_01.png               # v005 개별 프레임
  Frames/idle_02.png
  idle.aseprite                    # v005 편집 파일
  Export/idle_sheet.png            # 가로 2장, 600×300
  Export/idle.json                 # 프레임·타이밍
  Export/hunter_idle.gpl
  Export/palette.json
Previews/                          # 전후 비교·얼굴 확대·재생기·GIF·색표
Prompts/                           # 사용자 지시와 직접 픽셀 수정 규칙
QA/                                # 수정 스크립트·검수 기록
```

[idle 상세](Clips/HU-01_idle/README.md) · [원본 출처](Clips/HU-01_idle/Source/README.md) · [수정 지시 안내](Prompts/README.md)
