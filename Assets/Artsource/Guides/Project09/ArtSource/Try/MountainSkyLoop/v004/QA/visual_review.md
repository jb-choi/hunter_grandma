# v004 · 아기자기한 160×90 원경 검수

2026-10-12 · 상태: 시안 제작과 파일·레이어 검수 완료.

사용자가 v003의 수려하고 사실적인 인상을 줄여 달라고 요청했다. 산·산림의 형태를 새로 만들고, 작은 구름과 낮은 능선을 별도로 그렸다. 넓어진 바위 면, 덩어리 숲, 부드러운 민트·세이지·크림색을 기준으로 평가했다.

## 비교와 표시 크기

- [전체 전후 비교](../Previews/style_before_after.png): 왼쪽 v003 320×180의 2배, 오른쪽 v004 160×90의 4배. 같은 640×360 표시 크기에서 비교한다.
- [160×90 원본](../Export/bg_mountain_sky_160.png), [640×360 4배 표시](../Export/bg_mountain_sky.png), [가로 3회 반복](../Previews/repeat_3x.png).
- [이동 GIF](../Previews/scroll_preview.gif): 80프레임, 프레임당 160ms, 논리 도트 2px 이동, 12.8초 루프. 모든 프레임을 원본 이동 위치와 대조했다.

## 실제 visibility 검수

마스터를 복사해 단독 표시 4개와 한 레이어를 숨긴 상태 4개를 저장했다. 각 `.aseprite`를 다시 열어 실제 `isVisible` 값과 출력 픽셀을 확인했다. 키보드 Shift+X를 누른 것으로 보고하지 않으며, 그 기능과 같은 가시성 상태를 파일에 직접 설정해 평가했다. 마스터는 네 레이어를 모두 표시한다.

[단독 레이어 보드](LayerReview/layers_contact.png) · [8개 상태 기록](LayerReview/visibility_audit.json) · [저장된 Aseprite 상태](LayerReview/States/).

| 레이어 | 이미지와 전후 비교 | 작화 판정 |
| --- | --- | --- |
| BG-01 Sky | [단독](LayerReview/Solo/01_sky_checker.png), [전후](LayerReview/1_before_after.png) | 밝은 민트색 한 면. 산·구름 조각이나 그라데이션 없이 전체 캔버스를 채운다. 조용한 배경으로 적합. |
| BG-01 Clouds | [단독](LayerReview/Solo/02_clouds_checker.png), [확대](LayerReview/Crops/clouds_6x.png), [전후](LayerReview/2_before_after.png), [반복](LayerReview/Repeats/02_clouds_3x.png) | 긴 사실적 구름 띠를 작은 계단형 구름으로 바꿨다. 크림색 본체와 짧은 밑면 그림자로 읽힌다. 바위·숲 잔여 조각 없음. 좌우 경계를 넘는 구름도 연결된다. |
| BG-02 Distant ridges | [단독](LayerReview/Solo/03_distant_ridges_checker.png), [확대](LayerReview/Crops/distant_ridge_6x.png), [전후](LayerReview/3_before_after.png) | 세 가지 낮은 색 면과 완만한 둔덕으로 정리했다. 주요 산을 숨겨도 능선 아래가 온전히 채워져 있다. 세밀한 절벽·숲 무늬 없이 원근을 유지한다. |
| BG-02 Cozy granite and woodland | [단독](LayerReview/Solo/04_granite_woodland_checker.png), [왼쪽 산](LayerReview/Crops/left_peak_6x.png), [오른쪽 산](LayerReview/Crops/right_peak_6x.png), [숲](LayerReview/Crops/woodland_planes_6x.png), [전후](LayerReview/4_before_after.png) | 오른쪽 큰 산과 왼쪽 작은 산의 배치를 유지하면서 낮고 넓은 형태로 새로 제작했다. 암벽은 크림색 큰 면으로, 숲은 큰 녹색 덩어리로 읽힌다. 사진식 균열·나뭇잎 밀도가 크게 줄었다. 바위와 산림은 같은 레이어이며 내부 투명 구멍 없음. |

[주요 산을 숨긴 결과](LayerReview/Hidden/04_granite_woodland_hidden.png)에서 먼 능선의 아래 면을 확인했고, [먼 능선을 숨긴 결과](LayerReview/Hidden/03_distant_ridges_hidden.png)에서 주요 산 외곽을 확인했다. 색 수만으로 작화를 판정하지 않고 단독 표시, 봉우리·숲 확대, 이전 버전과 같은 크기의 비교를 함께 확인했다.

## 좌우 반복

세 번 연결한 전체 그림과 구름 단독 반복에서 연결 위치를 확인했다. 주요 산은 끝의 세 열을 낮은 숲 경계에 맞춰 직접 정리했다. 변경은 14개의 논리 픽셀이다. 전체 이미지를 반전해 만드는 반복은 사용하지 않았다. 주요 산의 양끝 열 색·알파가 같고, 먼 능선의 양끝 높이도 같다. 반복 구도에는 큰 산 하나와 작은 산 하나가 한 주기로 나타난다.

## 파일 검수

[Aseprite 검수](native_validation.json), [독립 파일 검수](independent_audit.json) 모두 통과.

- 편집 정본 160×90, RGB, 4레이어, 1프레임, duration 100ms, 태그 0개.
- 팔레트 15항목: 투명 1개와 새 스타일 색 14개. 최종 합성에서 실제 사용 색은 13개. 이전 버전 팔레트 보존은 이번 스타일 변경의 요구사항이 아니다.
- 정본과 원본 PNG의 차이 0px. 640×360은 정확한 최근접 4배이며 각 논리 도트가 4×4로 표시된다.
- 네 레이어의 원본·4배 PNG 차이 0px, 부분 알파 0px, 실제 마스크 알파 차이 0px.
- 두 산 레이어의 실루엣 아래 투명 구멍 0px. 최종 합성 전체 알파 255.
- 이전 v003 보존본과 기존 마스터의 SHA256 일치. 새 ImageGen 원본도 별도로 보존.
- GIF 80개 프레임의 이동 위치 차이 0px. HTML의 자바스크립트 문법과 파일 연결은 확인했으며 브라우저 실행 검수로 보고하지 않는다.

이번 시안에서 확인된 파일·레이어 결함은 없다. 아기자기함의 선호와 캐릭터·중경을 합친 게임 화면의 조화는 사용자 판단 대상이며, Unity 적용 완료로 표시하지 않는다.

## 기준 파일의 역할

[정리 전후 참고](../Reference/cleanup_reference.aseprite)는 주요 산의 반복 경계 정리 전후를 두 레이어로 보존한다. [영역 마스크](../Masks/material_regions.aseprite)는 실제 160×90 네 레이어의 점유 영역이며, 출력 알파와 대응을 확인하는 데 사용했다. 내장 ImageGen의 투명 산 시안은 [생성 원본](../Source/imagegen_cozy_mountain.png), 프롬프트는 [Source/style_prompt.txt](../Source/style_prompt.txt)에 보존했다.
