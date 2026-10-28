## 2026-10-13 현재 수정

오두막을 별도 레이어로 분리해 현재 16레이어다. [오두막 분리 검수](cabin_split_review.md)와 [분리 전후 수치](cabin_split_verification.json)를 우선 확인한다. 아래 표는 2026-10-12의 15레이어 검수 이력이며 이미지 링크는 당시 보존본으로 연결한다.

# LH-01 v004 — 각 레이어 단독 visible 검수

2026-10-12. v003 15레이어를 한 개씩 단독 visible로 확인한 뒤 수정했다. v004에서도 한 개씩 단독 표시한 PNG/체크 배경/3배 경계 확대를 다시 확인했다. 모든 프레임은 정적 1프레임이며 애니메이션 재생 검수는 해당 없음.

![강아지 없는 최신 합성](../Previews/loghouse_no_dog_2x.png)

![소품 10개 단독 접촉판](../Previews/props_after_checker.png)

소품 접촉판 순서: 윗줄 활·사슴 액자·여우 액자·기념품·도감, 아랫줄 돋보기·쌍안경·개집·수레·소파.

## 전 레이어 결과

| 레이어 / 프레임 1 | 전 / 후 이미지 | 확인한 경계·내용 | 판정 | 남은 확인 |
| --- | --- | --- | --- | --- |
| 01 배경 | [전](../Backups/20261013_before_cabin_split/Previews/Before/layer_01_solo.png) / [후](../Backups/20261013_before_cabin_split/Previews/After/layer_01_solo.png) | 소품 제거 자국과 반복 띠 제거. 강아지/잠자리 없음. 벽·바닥·벽난로 기단과 외부 빈 마당이 연결됨 | 단독 표시 정상 | 배경 일부 디테일은 보완 생성으로 변경됨. 실제 Unity 렌더 미검수 |
| 00 숨긴 현재 참조 | [전](../Backups/20261013_before_cabin_split/Previews/Before/layer_02_solo.png) / [후](../Backups/20261013_before_cabin_split/Previews/After/layer_02_solo.png) | 이전 참조의 강아지를 현재 합성 참조로 교체. 기본 숨김/잠금 | 단독 표시 정상 | 작업용 참조, export 제외 |
| 02 활·화살통 | [전](../Backups/20261013_before_cabin_split/Previews/Before/layer_03_detail3x.png) / [후](../Backups/20261013_before_cabin_split/Previews/After/layer_03_detail3x.png) | 외부 벽 여백 정리, 활줄 연속과 화살통 유지 | 단독 표시 정상 | 서로 다른 무기 단계는 미제작 |
| 03 사슴 액자 | [전](../Backups/20261013_before_cabin_split/Previews/Before/layer_04_detail3x.png) / [후](../Backups/20261013_before_cabin_split/Previews/After/layer_04_detail3x.png) | 액자 외부 목재 여백 정리, 프레임/사슴 형태 유지 | 단독 표시 정상 | — |
| 04 여우 액자 | [전](../Backups/20261013_before_cabin_split/Previews/Before/layer_05_detail3x.png) / [후](../Backups/20261013_before_cabin_split/Previews/After/layer_05_detail3x.png) | 프레임 외부 여백 정리, 여우 얼굴 유지 | 단독 표시 정상 | — |
| 05 기념품 | [전](../Backups/20261013_before_cabin_split/Previews/Before/layer_06_detail3x.png) / [후](../Backups/20261013_before_cabin_split/Previews/After/layer_06_detail3x.png) | 조각/받침 주변 벽 픽셀 정리 | 단독 표시 정상 | — |
| 06 열린 도감 | [전](../Backups/20261013_before_cabin_split/Previews/Before/layer_07_detail3x.png) / [후](../Backups/20261013_before_cabin_split/Previews/After/layer_07_detail3x.png) | 책 모서리와 하단 여백 정리, 동물 그림 유지 | 단독 표시 정상 | 실제 문구는 별도 UI 필요 |
| 07 돋보기 | [전](../Backups/20261013_before_cabin_split/Previews/Before/layer_08_detail3x.png) / [후](../Backups/20261013_before_cabin_split/Previews/After/layer_08_detail3x.png) | 렌즈·받침 주변 벽/책상 여백 정리 | 단독 표시 정상 | 원 컨셉의 거치형 형태 유지 |
| 08 쌍안경 | [전](../Backups/20261013_before_cabin_split/Previews/Before/layer_09_detail3x.png) / [후](../Backups/20261013_before_cabin_split/Previews/After/layer_09_detail3x.png) | 두 경통과 밝은 렌즈, 외부 여백 정리 | 단독 표시 정상 | 실제 게임에서 더 작게 표시할 때 가독성 확인 필요 |
| 09 사냥개 대기 | [전](../Backups/20261013_before_cabin_split/Previews/Before/layer_10_detail3x.png) / [후](../Backups/20261013_before_cabin_split/Previews/After/layer_10_detail3x.png) | 강아지/잠자리 그림과 셀 제거. 기본 숨김 | 의도한 빈 레이어 | 해금 시 별도 아트 추가 |
| 10 개집 | [전](../Backups/20261013_before_cabin_split/Previews/Before/layer_11_detail3x.png) / [후](../Backups/20261013_before_cabin_split/Previews/After/layer_11_detail3x.png) | 지붕 외부 잎/벽 여백 정리. 입구/집 형태 유지 | 단독 표시 정상 | 붙어 있는 하부 풀/돌은 개집 장식으로 의도적 유지 |
| 11 수레 | [전](../Backups/20261013_before_cabin_split/Previews/Before/layer_12_detail3x.png) / [후](../Backups/20261013_before_cabin_split/Previews/After/layer_12_detail3x.png) | 바퀴 구멍의 배경·외부 잔디·아래 흙 자국 정리, 손잡이 실루엣과 등불 받침 연결 확인 | 단독 표시 정상 | 대형 성장 수레 외형은 미제작 |
| 12 소파 | [전](../Backups/20261013_before_cabin_split/Previews/Before/layer_13_detail3x.png) / [후](../Backups/20261013_before_cabin_split/Previews/After/layer_13_detail3x.png) | 왼쪽 위 벽 픽셀 제거. 둥근 팔걸이·쿠션·목재 발 유지 | 단독 표시 정상 | — |
| 13 리터치 공간 | [전](../Backups/20261013_before_cabin_split/Previews/Before/layer_14_detail3x.png) / [후](../Backups/20261013_before_cabin_split/Previews/After/layer_14_detail3x.png) | 의도한 빈 작업 공간, 기본 숨김 | 의도한 빈 레이어 | 미작화 공간을 완성 소품으로 취급하지 않음 |
| 14 가이드 | [전](../Backups/20261013_before_cabin_split/Previews/Before/layer_15_solo.png) / [후](../Backups/20261013_before_cabin_split/Previews/After/layer_15_solo.png) | 64×64 원점 격자, 단독 표시에서 식별됨. 기본 숨김/잠금 | 작업용 가이드 정상 | export 제외 |

## 파일 검사와 기준 자료

- [재읽기 검증](file_verification.json): 640×320, 15레이어, 1프레임 100ms, 10종 소품. 합성/PNG/현재 참조와 대응 소품 출력 픽셀 일치.
- [수정 전 레이어 인벤토리](layer_audit_Before.json), [수정 후 인벤토리](layer_audit_After.json): 단독 표시 이미지 경로, 기본 visible, 비투명 픽셀 수, 실제 범위.
- [소파 경계 전후 기준 파일](../Reference/cleanup_reference.aseprite), [정리 부위 마스크](../Masks/material_regions.aseprite).
- 배경 첫 직접 복원 시도는 사각형 패치가 보여 **실패** 판정. 해당 시도 스크립트는 `Scripts/repair_layers_attempt01.lua`로 보존했다. 최종 작업본은 깨끗한 빈 배경판을 사용한다.
- 최종 배경만 내장 image_gen 보완/기존 팔레트 매핑, 소품은 기존 Aseprite 픽셀에서 직접 경계 수정. 이 둘을 같은 방법의 수작업으로 보고하지 않는다.

검수 범위는 현재 정적 장면과 모든 레이어의 단독 표시다. 애니메이션·실제 Unity 렌더·전 성장 단계 검수는 미수행이며 완료로 표시하지 않는다.
