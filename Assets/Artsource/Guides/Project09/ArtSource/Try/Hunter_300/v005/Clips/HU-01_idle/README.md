# HU-01 / idle — 300×300 윤곽·재질 경계 정리

v005 · 태그 `idle` · 2프레임 · 각 500ms · 1초 반복

[편집 파일](idle.aseprite) · [1번 PNG](Frames/idle_01.png) / [2번 PNG](Frames/idle_02.png) · [시트](Export/idle_sheet.png) / [메타](Export/idle.json) · [GIF](../../Previews/idle.gif) · [시험 전체 안내](../../README.md)

300×300 Indexed 캔버스와 v004의 공통 팔레트 17칸(불투명 16색 + 투명 1칸)을 유지한다. 두 프레임 모두 실제 불투명 16색을 사용하고 알파는 0 또는 255만 사용한다. 캐릭터 높이는 234px다. [GPL](Export/hunter_idle.gpl), [팔레트 JSON](Export/palette.json), [색표](../../Previews/palette.png)를 함께 보관한다.

눈을 단색 검정 6×13px 직사각형으로 정돈하고, 얼굴·모자·머리카락 경계의 얇은 다른 재질 색 띠와 잔점, 다리의 얇은 그림자 띠를 정리했다. 얼굴 위쪽의 갈색·녹색 잔점 2개는 검정 윤곽으로 합쳤다. 필요한 검정 윤곽과 외곽 픽셀도 수정 대상으로 포함했다. 귀의 의도한 음영, 버클과 활줄은 보호했다. 볼터치는 1번 프레임의 경계 돌출 1픽셀만 기본 피부색으로 합쳤다.

[전후 비교](../../Previews/idle_before_after.png)는 위쪽 v004, 아래쪽 v005다. 얼굴의 [1번 확대 비교](../../Previews/face_before_after.png)와 [2번 확대 비교](../../Previews/face_before_after_02.png)는 왼쪽 v004, 오른쪽 v005다.

변경 픽셀은 1번 105개, 2번 149개다. 투명/불투명 경계가 바뀐 위치는 각각 3개와 6개, 검정 윤곽이 바뀐 위치는 48개와 60개다. 볼터치 경계 1픽셀을 제외한 지정 보호 디테일의 불일치는 0개다.

새 이미지 생성 없이 기존 Aseprite 픽셀을 직접 수정했다. 블러·안티앨리어싱·디더링을 사용하지 않았다. 몸과 장비는 한 레이어의 시험 합성본이며, 프레임 간 형태·포즈 차이와 원본의 불규칙한 도트 간격은 남아 있다. Unity에는 적용하지 않았다.

수정은 이 폴더의 `idle.aseprite`에 진행한다. [Source](Source/README.md)는 수정 전 원본 비교용이다. 수정 후 PNG·GIF·시트도 다시 내보낸다. [정리 명세](../../Prompts/contour_cleanup_spec.md) · [파일 검수 기록](../../QA/file_validation.json)
