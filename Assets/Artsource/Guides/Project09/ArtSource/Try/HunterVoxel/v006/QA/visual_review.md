# v006 검수 — 원본 RIGHT 추가, FRONT 유지

2026-10-14. 실제 저장 파일을 재개봉해 검사했다. FRONT는 v005 셀 참조, RIGHT는 원본 Frame 1의 무테 등록 참조다.

| 항목 | 판정 | 실제 근거 |
| --- | --- | --- |
| FRONT 유지 | 통과 | [비교](../Previews/front_pixel_comparison.png), [재개봉 렌더](front_reopened_64.png). v005 참조와 RGBA 차이 0셀 |
| 원본 RIGHT 배치 | 통과 | [비교](../Previews/right_pixel_comparison.png), [재개봉 렌더](right_reopened_64.png). 등록된 원본 무테 참조와 RGBA 차이 0셀 |
| 원본 좌표 대응 | 통과 / 역등록 출력 | [원본 비교](../Previews/original_right_vs_3d.png), [역변환 기록](right_native_coordinate_validation.json). 실제 RIGHT 렌더를 원본 좌표로 되돌린 결과가 무테 원본과 0픽셀 차이. 네이티브 직접 카메라 렌더와 구분 |
| 검정 윤곽 제거·의미 디테일 | 확인 | 같은 부위의 기존 색으로 윤곽만 채움. [기준 예시](../Reference/cleanup_reference.aseprite), [마스크](../Masks/right_outline_and_details.png). 눈 3셀·버클 구멍 보존 |
| 모자·깃털·얼굴 | 2D 배치 통과 / 입체 정리 여지 | 두 기본 방향에서 계단·띠·피부·눈·머리카락 위치 확인. [회전](turntable_contact.png)에서 얇은 깃털·챙과 면색 연결에 추가 정리 여지 |
| 몸통·팔·손 | 2D 배치 통과 | 옷깃·끈·벨트·자유손·새총 손 위치 확인. 공통 눈/버클 코너를 넣어 앞뒤 다른 표면에 기능이 독립 중복되는 것을 줄임 |
| 두 다리·부츠 | 확인 | [위에서 본 양발](paired_boots_top.png), [부위 단독](solo_contact.png). 두 메시를 FRONT 좌우와 RIGHT 앞뒤 위치에 대응. 네 발을 만들지 않음 |
| 가방·새총 | 2D 배치 통과 / 입체 정리 여지 | 원본 RIGHT 위치와 F 셀을 동시에 보존. 회전에서 재질·얇은 장비 연결은 추가 작화 여지 |
| 전체 11부위 단독·숨김 | 확인 완료 | [단독](solo_contact.png), [숨김](hidden_contact.png), [가시성 기록](part_visibility_states.json). 부위 경계·가림 확인. 독립 교체용 접합 면 복원 완료를 주장하지 않음 |
| 실제 큐브·편집 파일 | 통과 | [검사](saved_file_validation.json). 12,988큐브·11메시, 한 변 0.1, 오류 0 |
| 격자 연결 | 통과 | [패키지 검사](package_validation.json). 전체 12,988셀이 하나의 26이웃 배치 요소. 용접·물리·리깅 검수가 아님 |
| 이미지 평면·시점 의존 모델 | 없음 확인 | 고정 vertex color → Principled BSDF. 이미지 텍스처·카메라 노드·대체 2D 메시 없음. 참고 PNG는 패킹 자료 |
| GLB | 통과 | [재가져오기](glb_roundtrip.json). 11메시·155,856삼각형, vertex color 보존, 오류 0 |
| Native Aseprite 참조 | 통과 | 예시·마스크 재export 차이 각각 0픽셀. 원본 좌표 무테 Aseprite도 PNG 재export 일치 확인 |
| 이전 원본 보존 | 통과 | [기존 검사](saved_file_validation.json) 467개 파일 중 변경 0개. 원본 해시 유지 |
| 라이브 Blender | 통과 | [열기 결과](live_open.json). v006, RIGHT 카메라, 전체 11메시 기본 표시 |
| BACK·LEFT·TOP | 차이 남음 | BACK 47 / LEFT 993 / TOP 1,146 RGBA 셀 차이. 원본 RIGHT와 생성 LEFT가 반대 실루엣 조건에 맞지 않음. 전체 5방향 일치로 보고하지 않음 |
| 리깅·동작·Unity 적용 | 미실행 | 정적 도안 대응 실험 범위 |

QA의 카메라·가시성 전환은 독립 재개봉 세션에서 수행하고 정본에는 저장하지 않았다. 판정은 **FRONT와 원본 RIGHT의 무테 셀 대응 검증 완료**이며, 자유 회전 입체 작화 완성은 별도다.

