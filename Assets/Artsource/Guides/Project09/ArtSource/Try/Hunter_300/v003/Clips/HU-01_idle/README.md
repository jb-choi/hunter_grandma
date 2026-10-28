# HU-01 / idle — 300×300 면 색 정리

v003 · 태그 `idle` · 2프레임 · 각 500ms · 1초 반복

[편집 파일](idle.aseprite) · [1번 PNG](Frames/idle_01.png) / [2번 PNG](Frames/idle_02.png) · [시트](Export/idle_sheet.png) / [메타](Export/idle.json) · [GIF](../../Previews/idle.gif) · [시험 전체 안내](../../README.md)

캔버스는 300×300, 캐릭터 높이는 두 프레임 모두 234px다. Indexed 팔레트는 **17칸(불투명 16 + 투명 1)**이며 각 프레임에서 실제 사용하는 불투명 색은 16색이다. 알파는 0 또는 255만 사용한다. [GPL](Export/hunter_idle.gpl), [팔레트 JSON](Export/palette.json), [색표](../../Previews/palette.png)를 함께 보관한다.

v002의 같은 물체 안에 남은 작은 명암 조각을 합치고 녹색 면과 모자 띠의 명암 위치를 지정했다. 녹색의 기본색·그림자·밝은색은 각각 `#487040`·`#355B32`·`#AEC95C`다. v002와 투명 영역·실루엣은 동일하며 색 경계만 정리했다. [전후 비교](../../Previews/idle_before_after.png)에서 위쪽 v002와 아래쪽 v003을 확인할 수 있다.

기존 픽셀을 직접 수정했다. 새 원화·키포즈를 생성하지 않았으며 프레임 간 얼굴·모자 형태 변화는 남아 있다. 몸과 장비가 한 레이어에 들어 있는 시험 합성본이며 Unity에는 적용하지 않았다.

편집은 이 폴더의 `idle.aseprite`에 진행한다. [Source](Source/README.md)는 원본 비교용이다. 수정 후 PNG·GIF·시트도 다시 내보내야 한다. 좌표 없이 영역을 지시하는 방법과 색 통일 규칙은 [정리 명세](../../Prompts/pixel_cleanup_spec.md)에 기록했다.
