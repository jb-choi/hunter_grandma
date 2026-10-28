# idle 생성 출처와 v002 색 정리 규칙

[HU-01 idle 생성 프롬프트](HU-01_idle.md)는 `Hunter_1254/v001`에서 사용한 문서를 보존한 출처 기록이다. 기준 그림은 [hunter_base_1254.png](../Reference/hunter_base_1254.png)이며, 기존 생성은 내장 imagegen 편집 모드·투명 배경으로 진행했다.

**이번 v002 색 정리에서는 imagegen을 새로 호출하지 않았다.** v001의 300×300 idle에 공통 불투명 26색 팔레트를 적용하고 알파를 투명/완전 불투명으로 정리했다. 출력 Aseprite 파일은 Indexed 모드, 팔레트 27칸(투명 1 + 불투명 26)이며 반투명 픽셀은 0개다. 새 중간색 패턴을 만드는 디더링은 사용하지 않았다.

[pixel_cleanup_spec.md](pixel_cleanup_spec.md)는 이번 색 정리의 처리 규칙을 기록한 문서다. 이미지 생성 프롬프트와 구분해 보관한다. 최종 출력의 색·알파 수치는 [파일 검수 기록](../QA/file_validation.json)에서 확인한다.

[색 정리 전 원본 안내](../Clips/HU-01_idle/Source/README.md) · [시험 전체 안내](../README.md)
