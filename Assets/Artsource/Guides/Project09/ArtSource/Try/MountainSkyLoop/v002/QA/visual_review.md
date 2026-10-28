# v002 · 레이어 수정 후 작화 검수

2026-10-12 · 원본 640×360 · 정적 1프레임 · BG-01+02.

## 실제 검수 방법

검수 복사본의 Aseprite `isVisible`을 직접 전환했다. 대상 하나만 보이는 상태 4개, 대상 하나만 숨긴 상태 4개를 저장하고 다시 열어 렌더했다. Shift+X 키를 직접 누른 것은 아니다. [실제 visibility 상태](LayerReview/visibility_audit.json).

단독 상태의 1배·2배, 이전 결함 위치 4배, 가로 3회 반복, 다른 레이어를 숨긴 화면을 각각 검토했다. [4개 단독 비교](LayerReview/layers_contact.png).

## 레이어별 수정·판정

| 영역 | 전후 근거 | 확인 내용 | 판정 | 남은 조건 |
| --- | --- | --- | --- | --- |
| 하늘 | [전후](../Previews/before_after_01_sky.png) | v001과 동일한 불투명 하늘 면. 누락·색 경계 없음. | 통과 | 현재 단색 낮 하늘 기준. |
| 구름 | [전후](../Previews/before_after_02_clouds.png), [산 흔적 확대](../Previews/before_after_clouds_peak_trace.png) | 대각 산 조각·수직 점·짧은 잔여물이 제거됐다. 큰 구름과 수평 위스프가 유지된다. | 확인된 결함 해결 | 하늘의 작은 위스프 일부는 의도적으로 남김. |
| 먼 능선 | [전후](../Previews/before_after_03_distant_ridges.png), [확대](../Previews/before_after_distant_ridge_vertical_artifacts.png) | 산림·암벽의 색 혼입과 세로 돌출 없음. 가려진 능선이 연결되며 세 명암 면이 연속된다. | 확인된 결함 해결 | 복원한 숨은 부분은 큰 면으로 표현. |
| 주봉·산림 골짜기 | [4배 전후](../Previews/before_after_mountain_valley_gaps.png) | 수직으로 비던 구간을 복원했고 주봉·산림 자체의 사면으로 이어진다. 하늘색 점도 원래 산 색으로 되돌렸다. | 확인된 결함 해결 | 암벽·식생의 원래 픽셀 디테일 유지. |
| 주봉 오른쪽 측면 | [4배 전후](../Previews/before_after_mountain_peak_gaps.png) | 긴 투명 기둥과 잘린 면이 없어졌고 밝은 암벽·어두운 사면이 이어진다. | 확인된 결함 해결 | 구조를 설명하는 암벽 균열은 유지. |
| 단독 가로 반복 | [구름](LayerReview/Repeats/02_clouds_3x.png), [먼 능선](LayerReview/Repeats/03_distant_ridges_3x.png), [주봉](LayerReview/Repeats/04_granite_woodland_3x.png) | 각각 연결되며 수직 빈 띠·잘린 내부 영역이 보이지 않는다. | 통과 | 주봉은 한 타일마다 반복되는 확정 규격. |
| 한 레이어 숨김 | [주봉 숨김](LayerReview/Hidden/04_granite_woodland_hidden.png), [먼 능선 숨김](LayerReview/Hidden/03_distant_ridges_hidden.png) | 주봉을 숨겨도 산 조각이 원경에 남지 않는다. 먼 능선을 숨겨도 주봉이 갈라지지 않는다. | 통과 | 실제 게임 속도로 분리 이동하는 적용 검수는 별도. |

## 수정 방식·실제 입력

- `Masks/material_regions.aseprite`의 주봉 연속 외곽, 복원된 먼 능선, 구름 영역을 저장하고 다시 읽어 실제 출력 영역에 적용했다.
- `Reference/cleanup_reference.aseprite`에는 v001 Before와 v002 After를 같은 캔버스의 레이어로 보존했다.
- 주봉의 색 누락은 `Source/stitched_640_v001.png`의 분리 전 픽셀을 읽어 기존 산림·암벽 팔레트로 복원했다. 기존 Aseprite의 원래 주봉 픽셀 대부분은 그대로 유지했다.
- 원경의 가려진 외곽은 검토한 위치 기준점으로 다시 그렸고, 이미 보이던 원경의 픽셀 명암 덩어리를 가능한 범위에서 유지했다. 원경 팔레트는 3개 푸른 톤으로 일관되게 정리했다.
- 이웃 다수결 필터나 색 수 제한만으로 정리 완료를 판단하지 않았다. 수직 누락과 세로 조각을 형태 영역 단위로 수정했다.

## 파일 검증과 실제 변경

- v001 원본 SHA256 보존: `7cc18773437f3225643b4b06c44ea8973aceee66431dd2a260abb8d589e7a220`.
- 원본을 다시 열어 PNG와 전체 픽셀 일치. 레이어 PNG와 cel 일치. 49항목 팔레트·640×360·1프레임 유지.
- 주봉 누락 영역 3,491픽셀 복원, 구름 75픽셀 제거. 합성은 8,901픽셀(3.86%) 변경.
- 먼 능선·주봉의 연속 형태 내부에 투명 구멍 0. 모든 레이어 알파 0/255.
- 이동 GIF는 80프레임·160ms/프레임·8px 이동. 전 프레임이 합성 PNG의 가로 순환 이동과 픽셀 단위로 일치했다.
- [원본 재조회 검증](native_validation.json), [독립 감사](independent_audit.json), [수정 영역 지표](repair_metrics.json).

**현재 판정:** v001 단독 검수에서 확인한 세 레이어의 결함을 수정하고 재검수했다. 이 판정은 이미지/레이어 제작 기준이며 Unity 적용 완료를 뜻하지 않는다.
