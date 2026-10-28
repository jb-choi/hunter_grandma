# HU-01 / idle — 300×300 색 정리 시험

v002 · 2프레임 · 각 500ms · 1초 반복 · 태그 `idle`

[편집 파일](idle.aseprite) · [1번 PNG](Frames/idle_01.png) / [2번 PNG](Frames/idle_02.png) · [시트](Export/idle_sheet.png) / [메타](Export/idle.json) · [GIF](../../Previews/idle.gif) · [작업 입구](../../README.md)

기존 v001 idle에 공통 제한 팔레트를 적용하고 반투명 경계를 정리한 비교본이다. 개별 프레임과 편집 파일의 캔버스는 **300×300**, 캐릭터 높이는 두 프레임 모두 234px, 시트는 가로 2장으로 배치한 600×300이다. 두 프레임의 500ms 타이밍과 반복 태그를 유지한다.

편집 파일은 **Indexed 모드, 팔레트 27칸(투명 1 + 불투명 26)**이다. 두 프레임 모두 불투명 26색을 사용하며 반투명 픽셀은 0개다. [팔레트 GPL](Export/hunter_idle.gpl) / [JSON](Export/palette.json) / [색표](../../Previews/palette.png)를 함께 보관한다.

알파 128 미만은 투명, 128 이상은 완전 불투명으로 처리했으며 색 대응에는 디더링을 사용하지 않았다. 녹색·머리카락 계열 내부의 고립된 명암 픽셀만 소수 보정했다. 구체적인 정리 규칙과 최종 수치는 [색 정리 명세](../../Prompts/pixel_cleanup_spec.md)와 [검수 기록](../../QA/file_validation.json)을 기준으로 확인한다.

새 원화나 키포즈를 생성하지 않았다. 반투명 가장자리와 일부 색 픽셀은 달라질 수 있으며, 모자·얼굴의 프레임 간 형태 변화는 별도의 보정 대상이다. 몸과 장비가 함께 들어 있는 시험 합성본이며 Unity에는 적용하지 않았다.

키포즈를 수정할 파일은 이 폴더의 `idle.aseprite`다. [Source](Source/README.md)의 v001·1254 파일과 생성 시트는 출처 비교를 위한 보관본이다. 이후 키포즈를 바꾸면 PNG·GIF·시트를 다시 내보내야 한다.
