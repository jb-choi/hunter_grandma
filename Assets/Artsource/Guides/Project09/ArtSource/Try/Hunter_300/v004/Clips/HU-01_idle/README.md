# HU-01 / idle — 300×300 잔색·색 경계 정리

v004 · 태그 `idle` · 2프레임 · 각 500ms · 1초 반복

[편집 파일](idle.aseprite) · [1번 PNG](Frames/idle_01.png) / [2번 PNG](Frames/idle_02.png) · [시트](Export/idle_sheet.png) / [메타](Export/idle.json) · [GIF](../../Previews/idle.gif) · [시험 전체 안내](../../README.md)

300×300 Indexed 캔버스와 v003의 공통 팔레트 17칸(불투명 16색 + 투명 1칸)을 유지한다. 두 프레임 모두 실제 불투명 16색을 사용하며 알파는 0 또는 255만 사용한다. 투명 여백을 제외한 캐릭터 높이는 234px다. [GPL](Export/hunter_idle.gpl), [팔레트 JSON](Export/palette.json), [색표](../../Previews/palette.png)를 함께 보관한다.

귀의 밝은 면을 `#FFF1D2`로 통일하고, 피부·목·깃털·손의 작은 색 조각과 가는 띠를 주변 대표색으로 합쳤다. 같은 재질 안의 불필요한 1픽셀 색 경계를 정리하며 큰 명암 면, 외곽 실루엣, 눈과 볼의 디테일은 유지한다. [전후 비교](../../Previews/idle_before_after.png)는 위쪽 v003, 아래쪽 v004다. 귀의 [1번 확대 비교](../../Previews/ear_before_after.png)와 [2번 확대 비교](../../Previews/ear_before_after_02.png)는 왼쪽 v003, 오른쪽 v004다.

변경 픽셀은 1번 383개, 2번 398개다. 투명 영역·보호 디테일·기존 검정 윤곽의 불일치는 모두 0개다.

새 이미지 생성 없이 기존 Aseprite 픽셀을 직접 수정했다. 블러·안티앨리어싱·디더링을 사용하지 않았다. 몸과 장비는 한 레이어의 시험 합성본이며, 프레임 간 형태·포즈 차이는 남아 있다. Unity에는 적용하지 않았다.

수정은 이 폴더의 `idle.aseprite`에 진행한다. [Source](Source/README.md)는 수정 전 원본 비교용이다. 수정 후 PNG·GIF·시트도 다시 내보낸다. [정리 명세](../../Prompts/pixel_cleanup_spec.md) · [파일 검수 기록](../../QA/file_validation.json)
