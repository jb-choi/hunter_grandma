# v002 시각 검수

2026-10-13. v001 보존 후 왼팔·새총 위치와 양다리·부츠 수정 파일을 독립 재개봉해 검수했다.

| 부위·상태 | 근거 | 결과 |
| --- | --- | --- |
| 왼팔·손·새총 | [전후 전체](../Previews/before_after.png), [팔 단독](solo_04_FarArm_Hand.png), [새총 단독](solo_09_Slingshot.png) | 몸 앞쪽 돌출을 줄이고 +Y 옆으로 이동. 손과 새총의 이동량이 같아 그립 유지. 사선·반대편에서 몸과 연결됨. 통과 |
| 다리·발목·부츠 | [단독 왼쪽](solo_06_LeftLeg_Boot.png), [단독 오른쪽](solo_07_RightLeg_Boot.png), [사선 하체](../Previews/legs_three_quarter.png) | 하의 → 커프 → 발목 → 부츠 연결. 단일 격자 연결 컴포넌트이며 누락된 발목 큐브 없음. 통과 |
| 도식 위에서 보기 | [발만 위에서](boots_plan.png), [다리 위에서](../Previews/feet_top.png), [전후](../Previews/legs_before_after.png) | 왼쪽 세로 3칸 / 오른쪽 위 가로 3칸 배치 확인. 발목과 바짓단은 위에서 볼 때 발의 뒤쪽 일부를 가리는 정상 구성. 통과 |
| 다른 6개 부위 | [전체 단독](solo_contact.png), [각 부위 숨김](hidden_contact.png), [실제 상태](part_visibility_states.json) | 머리·모자·깃털·몸통·반대팔·가방의 실제 메시·재질·위치는 v001과 동일. 통과 |
| 전체 | [사선](../Previews/three_quarter.png), [정면](../Previews/front.png), [반대편](../Previews/opposite.png), [두께](../Previews/depth.png) | 팔·새총 접점과 신발의 서로 다른 방향, 양발 접지 확인. 통과 |

[saved_blend_validation.json](saved_blend_validation.json): 저장 모델 11,224개 큐브의 한 변 0.0986m, 치수 불일치 0개, 다리의 격자 연결 누락 0개, 실제 저장 발 큐브가 지정한 배치와 일치. v001 파일 변경 0건.

시각 검수의 단독·숨김은 저장 파일을 다시 연 별도 세션에서 실제 hide_render 값을 전환해 수행했고 정본을 덮어쓰지 않았다. 임시 부츠 전용 검수 메시도 저장된 메시의 실제 큐브 면을 복사해 발목 위를 제외한 것으로, 새로운 도식용 모형을 대신 찍은 것이 아니다.

의도한 예외: 다리의 도식은 위에서 본 발 방향으로 적용한 수정 시안이다. 원본 Aseprite F1과 똑같은 정면 실루엣은 유지 조건에서 제외했다. 원본 보존 요구는 v001과 Source 스냅샷으로 충족했다. 2D 입력에서 보이지 않던 뒷면·깊이는 여전히 시안 해석이다.

최종 판정: 지정한 팔·새총 배치와 다리·부츠 구조의 v002 수정 시안 검수 통과. 리깅·호흡 애니메이션·Unity 적용은 해당 없음.
