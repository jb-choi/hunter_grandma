# Visibility 전환 · 레이어 단독 검수

2026-10-12 · 사용자가 요구한 Shift+X 레이어 표시 전환을 검수 기준으로 적용.

**판정: 하늘 통과, 구름·먼 능선·주봉과 산림 수정 필요. 현재 4개 레이어의 작화 완료 판정은 미통과다.**

## 실제 전환 방식과 증거

키보드 Shift+X를 직접 누른 것은 아니다. Aseprite에서 각 레이어의 `isVisible`을 직접 바꾼 검수 복사본을 저장하고, 다시 열어 표시 상태를 확인한 뒤 렌더했다. 레이어 PNG만 읽고 결론을 내리는 방식에서 확장했다.

- `States/only_*.aseprite`: 대상 한 레이어만 visible, 다른 세 레이어 invisible.
- `States/hidden_*.aseprite`: 대상 한 레이어만 invisible, 다른 세 레이어 visible.
- 총 8개 실제 visibility 상태. [표시 상태 감사](visibility_audit.json).
- 원본 Aseprite의 검수 전후 SHA256 동일: `7cc18773437f3225643b4b06c44ea8973aceee66431dd2a260abb8d589e7a220`.
- 단독 상태 렌더와 기존 Layers PNG는 4종 모두 픽셀 차이 0이다. 문제는 PNG 출력 오류가 아니라 **원본 레이어의 영역 분리 자체**에 있다.

[4개 레이어 비교 이미지](layers_contact.png): 위 왼쪽 하늘 / 위 오른쪽 구름 / 아래 왼쪽 먼 능선 / 아래 오른쪽 주봉과 산림. 체크무늬는 검수용 투명 배경이며 아트에 포함되지 않는다.

## 레이어별 평가

| 레이어 | 단독 표시 판정 | 확인 내용 | 필요한 수정 |
| --- | --- | --- | --- |
| BG-01 Sky | **통과** | 전체 640×360 불투명 단색 면, 누락 없음. 좌우 색이 동일하다. 단색 하늘이라는 현 시안 범위에서 판정했다. | 필수 수정 없음. |
| BG-01 Clouds | **수정 필요** | 큰 구름 덩어리는 읽히지만, 주봉 위치에 구름과 무관한 대각 점·세로 조각이 남는다. 산을 가리면 잔여 흔적이 드러난다. | 산 조각을 구름 영역에서 제거하고, 실제 구름만 남기는 형태 마스크로 재분리한다. |
| BG-02 Distant ridges | **수정 필요** | 녹색·암벽 픽셀이 섞여 있고 세로 줄이 튀어나오거나 내려간다. 주봉을 숨기면 y≈184 부근의 수평 밑칠 경계와 미복원 구간이 드러난다. | 가려진 능선을 연결해 독립된 실루엣으로 만들고, 주봉·산림에 속한 픽셀을 이동한다. |
| BG-02 Granite and woodland | **수정 필요** | 왼쪽 봉우리·골짜기, 오른쪽 주봉 측면에 기둥처럼 갈라진 투명 경계가 있다. 다른 레이어가 가리는 부분이라 합성본에서는 잘 드러나지 않았다. | 주봉·산림의 연속된 외곽 마스크를 다시 만들고, 다른 레이어에 넘어간 산 픽셀을 되돌린다. |

### 근거 이미지

- 구름의 산 흔적: [단독 이미지](Solo/02_clouds_checker.png), [4배 확대](Crops/clouds_peak_trace_4x.png), [구름만 보이는 Aseprite](States/only_02_clouds.aseprite).
- 먼 능선의 세로 잔여물: [단독 이미지](Solo/03_distant_ridges_checker.png), [4배 확대](Crops/distant_ridge_vertical_artifacts_4x.png), [주봉을 숨긴 상태](Hidden/04_granite_woodland_hidden.png), [원경만 보이는 Aseprite](States/only_03_distant_ridges.aseprite).
- 주봉·산림의 경계 누락: [단독 이미지](Solo/04_granite_woodland_checker.png), [골짜기 4배 확대](Crops/mountain_valley_gaps_4x.png), [주봉 4배 확대](Crops/mountain_peak_gaps_4x.png), [먼 능선을 숨긴 상태](Hidden/03_distant_ridges_hidden.png), [주봉만 보이는 Aseprite](States/only_04_granite_woodland.aseprite).
- 각각의 가로 반복: [하늘](Repeats/01_sky_3x.png), [구름](Repeats/02_clouds_3x.png), [먼 능선](Repeats/03_distant_ridges_3x.png), [주봉과 산림](Repeats/04_granite_woodland_3x.png).

## 원인과 판정 정정

`Scripts/build_art.lua`는 색 조건으로 열마다 처음 나타나는 지형 픽셀을 찾고, 그 아래를 주봉·산림으로 분류한다. 그 시작 높이가 실제 외곽과 어긋난 열에서는 암벽·식생 픽셀이 구름이나 먼 능선으로 넘어간다. 먼 능선 뒤의 기본색 밑칠은 가려진 능선 자체를 작화한 것이 아니다.

기존 파일 감사의 원본/PNG 일치, 알파, 크기, GIF 프레임 검증은 여전히 유효하다. 해당 통과가 레이어별 작화 완성을 입증하지는 않는다. 이전 제작 완료 판정은 **레이어 작화 수정 필요**로 정정한다.

이번 검토에서는 원본 픽셀을 수정하지 않았다. 다음 수정은 v001을 보존한 별도 버전에서 경계 마스크와 가려진 능선을 정리한 뒤, 같은 visibility 전환으로 재검수해야 한다.

## 이후 필수 검수 순서

1. 레이어 하나만 visible로 두고 1배·2배에서 전체 면과 투명 경계를 평가한다.
2. 각각 한 레이어만 invisible로 두고, 앞 레이어가 가린 잔여물·누락·밑칠을 확인한다.
3. 단독 레이어마다 가로 3회 연결과 순환 이동을 확인한다.
4. 레이어 단독 판정과 전체 합성 판정을 각각 기록한다. 단독 판정이 미통과인 레이어를 제작 완료로 기록하지 않는다.
