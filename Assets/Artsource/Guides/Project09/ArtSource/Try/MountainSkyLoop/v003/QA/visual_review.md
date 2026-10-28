# 320×180 도트 작업 · 작화 검수

2026-10-12 · v003 · BG-01+02.

## 방법

작업 해상도 320×180, 표시/출력 640×360(최근접 2배). 원본의 visibility를 바꾼 단독 4개·하나만 숨긴 4개 상태를 저장 후 다시 열어 확인했다. 키보드 Shift+X를 직접 누른 것은 아니다. [실제 상태](LayerReview/visibility_audit.json).

각 레이어를 1배 원본, 2배 표시, 주요 영역 6배, 가로 3회 반복으로 확인했다. [전체 레이어 단독 비교](LayerReview/layers_contact.png).

## 영역별 확인

| 영역 | 이미지 | 확인한 경계·면 | 판정 | 남은 표현 |
| --- | --- | --- | --- | --- |
| 하늘 | [단독](LayerReview/Solo/01_sky_checker.png) | 단색 면과 양끝 색 연결 유지. | 통과 | 현재 낮 하늘 1색. |
| 구름 | [6배](LayerReview/Crops/clouds_6x.png), [반복](LayerReview/Repeats/02_clouds_3x.png) | 작은 잔여 점 13개 제거. 큰 구름과 긴 1도트 위스프 유지. 산 조각 혼입 없음. | 정리됨 | 의도한 얇은 위스프는 남김. |
| 먼 능선 | [6배](LayerReview/Crops/distant_ridge_6x.png), [반복](LayerReview/Repeats/03_distant_ridges_3x.png) | 가려진 부분까지 연속된 3톤 면. 축소 뒤에도 세로 찢김·내부 구멍 없음. | 통과 | 일부 원래의 능선 질감 유지. |
| 왼쪽 봉우리·골짜기 | [6배](LayerReview/Crops/left_peak_valley_6x.png) | 내리막의 고립 돌출/패임 정리, 주봉과 산림 면의 연결 유지. | 정리됨 | 실제 바위의 급경사는 유지. |
| 화강암 주봉 | [6배](LayerReview/Crops/granite_peak_6x.png) | 큰 암벽 면과 길게 이어진 주요 균열이 읽힘. 지정한 바위 디테일 보호 영역의 픽셀 차이 0. | 보존 확인 | 원래 세밀한 균열 일부 유지. |
| 왼쪽·중앙 산림 | [6배](LayerReview/Crops/woodland_planes_6x.png), [정리 전후](../Previews/cleanup_before_after.png) | 가까운 녹색 톤을 묶고 수관 위치에 넓은 면을 다시 그렸다. 잔무늬 대비가 낮아졌다. | 정리됨 | 전체 숲의 질감을 모두 제거한 것은 아님. |
| 오른쪽 산림 | [6배](LayerReview/Crops/right_woodland_planes_6x.png) | 암벽과 맞닿는 밝은 식생을 덩어리로 묶고 하부 잎 면을 연결했다. | 정리됨 | 바위와 나무 사이의 기존 명암 유지. |
| 단독 반복·숨김 | [주봉 반복](LayerReview/Repeats/04_granite_woodland_3x.png), [원경 숨김](LayerReview/Hidden/03_distant_ridges_hidden.png), [주봉 숨김](LayerReview/Hidden/04_granite_woodland_hidden.png) | 모든 레이어가 개별적으로 이어지며 v001의 누락/혼입이 재발하지 않음. | 통과 | 320px 한 타일 주기 반복. |

## 실제 수정과 근거

- Aseprite에서 실제 320×180 레이어와 마스크를 만들고 수정했다. 큰 640×360 그림에 표시 효과만 적용한 파일이 아니다.
- `Masks/material_regions.aseprite`는 주봉 외곽, 먼 능선, 구름, 수정 가능한 산림 면, 보존할 암벽 디테일의 5개 레이어다. 저장 후 다시 읽어 산림 면 정리에 사용했다. 마스크는 최종 4개 아트 레이어에 포함하지 않았다.
- `Reference/cleanup_reference.aseprite`는 최근접 축소만 한 Before와 320 면 정리 After의 같은 좌표 비교다.
- 수관 10개 영역의 위치/꼭짓점과 실제 변경 수: [cleanup_metrics.json](cleanup_metrics.json). 녹색 그룹 정리 외에 수관 면 작화와 경계 수정이 포함된다.
- 블러, 색 평균 보간, 안티앨리어싱, 디더링, 새 이미지 생성 없음. 기존 팔레트를 유지했다.

## 파일 검증

- 320×180 PNG와 저장 후 다시 연 Aseprite의 픽셀 차이 0.
- 640×360 PNG와 각 레이어가 네이티브의 **모든 도트를 2×2로 복제한 결과**와 일치.
- 보호된 화강암 영역 변경 픽셀 0, 네이티브 4레이어·1프레임·duration·팔레트 보존.
- 주봉/원경 내부 투명 구멍 0. 반투명 알파 0.
- 320 작업본을 2배 표시한 이동 GIF 80프레임 전체 일치.
- v002 정본 보존: SHA256 `e4014d8d6cc3419fc595258a33eb8a100eb78ad75431142c1ebf5245ba18cd43`.
- [네이티브 검증](native_validation.json), [독립 감사](independent_audit.json).

정본 320×180 기준의 도트 강조와 레이어 구조 검수를 완료했다. 실제 Unity 화면 적용, 다른 배경과의 합성, 시간대 변형은 검수하지 않았다.
