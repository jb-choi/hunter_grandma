# IdleBreathV2/p001 · 다리 디자인 단순화

2026-10-14. 현재 결과는 [idle_breath_v2.aseprite](../../../Clips/HU-01_idle/idle_breath_v2.aseprite)다. 기존 [idle_breath.aseprite](../../../Clips/HU-01_idle/idle_breath.aseprite)의 LowerBody y49~56만 변경한 별도 시험본이다.

[작업 안내](../../../Clips/HU-01_idle/idle_breath_v2.md) · [전후·재생](../../../Previews/IdleBreathV2/index.html) · [시각 검수](visual_review.md)

- [input_hashes.json](input_hashes.json): 동결 입력과 기존 native 15파일의 작업 전 SHA-256.
- [build.lua](build.lua): 기존 Indexed 셀의 다리 영역을 명시적 픽셀 행으로 수정; candidate 저장 후 독립 재개봉해 출력.
- [build_scope.lua](build_scope.lua): 원본 인덱스를 직접 읽어 만든 현재 바지·부츠·보존 자락 마스크. 서로 다른 팔레트 사이의 합성으로 범위가 빠지지 않도록 실제 인덱스를 읽는다.
- [validate.lua](validate.lua), [native_validation.json](native_validation.json): 요청한 파일을 독립 재개봉해 5프레임·시간·태그·팔레트·원래 4레이어 raw 셀·범위 밖·발 접지·PNG/시트/GIF/8배 출력·마스크 확인.
- [layer_review.lua](layer_review.lua), [layer_review_states.json](layer_review_states.json): 실제 `isVisible`로 전 5레이어를 단독 표시하거나 하나씩 숨겨 저장하고 다시 연 10개 native 상태. 전 50프레임을 투명·밝은·어두운·체크 배경과 실제 크기·8배로 출력.
- [source_preservation.json](source_preservation.json): 기존 15파일과 동결 입력 보존, candidate와 최종 파일 SHA-256 일치.
- [browser_check.cjs](browser_check.cjs), [browser_validation.json](browser_validation.json): 실제 Edge의 2주기 이상 반복, 전 5프레임 수동 이동, 재생 재개, 64px/512px, 배경 전환, 모바일과 파일 링크 확인.
- [독립 작화 검수](Review/visual_review.md): 전체·단독·하나씩 숨긴 전 프레임의 역할별 판정. 숨긴 옛 활은 현재 캐릭터 출력 통과와 구분한다.

정본의 기본 표시 상태는 입력과 같다. Bow&Quiver만 숨기고 나머지 네 레이어는 표시한다. 검수 상태는 LayerReview/States 하위 복사본에만 저장했다. 원본/최종 캐릭터에 검수용 visible 전환이나 배경·안내 레이어를 저장하지 않았다.

QA의 candidate는 최종 파일과 같은 픽셀·설정의 제작 보조 복사본이다. 편집과 후속 작업은 요청 위치의 idle_breath_v2.aseprite를 기준으로 한다. Unity 적용은 이번 요청에 포함하지 않는다.
