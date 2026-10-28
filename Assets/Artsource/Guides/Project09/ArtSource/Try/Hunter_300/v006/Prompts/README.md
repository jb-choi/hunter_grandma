# v006 적용 지시와 정리 명세

[cleanup_spec.md](cleanup_spec.md)에 사용자의 지시, 이번 시험의 수정 범위와 검수 기준을 기록한다. 기존 Aseprite 픽셀을 직접 수정하는 작업이며 이미지 생성 프롬프트로 재생성하지 않는다.

공통 규칙은 [PixelCleanupGuide.md](../../../../../Docs/AI/Art/02_Production/PixelCleanupGuide.md), 적용 예시는 [cleanup_reference.aseprite](../Reference/cleanup_reference.aseprite), 부위 구분은 [마스크 안내](../Clips/HU-01_idle/Masks/README.md)를 확인한다. 이번 버전에서 무엇이 수정되었는지와 남은 문제는 [전신 시각 검수](../QA/visual_review.md)를 확인한다.

실제 정리는 [cleanup_v6.lua](../QA/cleanup_v6.lua)의 부위별 색 면·경계 처리와 개별 픽셀 수정으로 진행했다. [pixel_edits.json](../QA/pixel_edits.json)에 수정 위치와 이유, [file_validation.json](../QA/file_validation.json)에 수치를 보관한다. 3·4·5px 전역 격자 정렬 후보는 미채택 비교 시험으로만 남겼다.
