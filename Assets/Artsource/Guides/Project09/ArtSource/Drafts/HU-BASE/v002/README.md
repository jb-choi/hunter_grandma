# HU-BASE v002 · 피드백 반영 작업

v001의 과도한 단순화를 수정한 작업이다. 현재 정본은 [Production 안내](../../../Production/Hunters/HU-BASE/README.md)에서 찾는다.

- [추가 image_gen 시안](hunter_concept_imagegen.png): 고해상도 참고 이미지, 게임용 32×32 출력이 아님.
- [image_gen 프롬프트](imagegen_prompt.md): 내장 image_gen 편집, 투명 배경.
- [첫 시안 크롭](concept_crop.aseprite), [19×29 참고 축소](concept_29px.aseprite).
- [32×32 픽셀 수정 작업본](hunter_native_work.aseprite).
- [최종 확대](../../../Previews/HU-BASE/v002/hunter_base_with_bow_12x.png).

최종 v002 구도는 [첫 생성 시안](../v001/hunter_concept_imagegen.png)을 기준으로 유지했다. Aseprite에서 명시적 영역을 크롭해 19×29로 줄이고, 읽은 픽셀을 불투명 14색으로 정리하여 32×32 중앙에 배치했다. 눈을 1×2로 정리하고 밑창을 추가했다. 활 내부에 실제 투명 틈을 남기고 가는 황갈색 줄과 나무 곡선을 구분했다. Body와 Bow를 분리해 같은 좌표의 PNG를 출력했다.

팔레트 축소만으로 시안의 인상이 유지되는 것은 아니므로, v001의 파일 규격 통과를 디자인 완성으로 판단했던 기록을 수정했다.
