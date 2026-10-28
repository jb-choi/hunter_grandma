# HunterVoxel v001 시각 검수

검수일: 2026-10-13. 범위: `idle_breath.aseprite` Frame 1을 바탕으로 만든 저장 Blender 모델의 10개 부위.

전체 근거: [사선](../Previews/three_quarter.png), [정면](../Previews/front.png), [반대편](../Previews/opposite.png), [두께](../Previews/depth.png).
원본 근거: [Frame 1](../Source/frame_01.png), [원본 5레이어 단독](source_layers.png).
저장 파일을 다시 열고 실제 `hide_render` 값을 전환한 [상태 기록](part_visibility_states.json)을 남겼다. 검수 복사 세션에서만 전환하고 편집 정본을 덮어쓰지 않았다.

| 부위 | 단독·숨김 근거 | 확인 결과 | 판정 |
| --- | --- | --- | --- |
| 깃털 | [단독](solo_00_Feather.png) / [숨김](hidden_00_Feather.png) | 흰색·주황색의 원본 형상과 모자 부착 위치 유지. 얇은 3칸 깊이. | 통과 |
| 모자 | [단독](solo_01_Hat.png) / [숨김](hidden_01_Hat.png) | 초록 모자, 갈색 띠, 넓은 챙이 둥근 계단형 볼륨으로 읽힘. 원본 윤곽의 검은 표면 큐브 유지. | 통과 |
| 머리·얼굴 | [단독](solo_02_Head_Hair_Face.png) / [숨김](hidden_02_Head_Hair_Face.png) | 머리카락·귀·검은 눈·피부색·턱이 원본 방향에서 읽힘. 반대편은 원본 색을 확장한 추정 면. | 통과 |
| 몸통·벨트 | [단독](solo_03_Torso_Belt.png) / [숨김](hidden_03_Torso_Belt.png) | 초록 상의, 갈색 대각선 끈, 벨트·금색 버클 유지. 다른 부위와 겹치는 부분은 메시별로 나눈 구성. | 통과 |
| 두 팔·손 | [원본 앞팔](solo_05_NearArm_Hand.png) / [장비 쪽 팔](solo_04_FarArm_Hand.png) / [전체 숨김 비교](hidden_contact.png) | 좌우 손의 피부와 커프 색이 유지되고 새총 그립이 손 앞에 연결됨. 팔은 원본 단일 포즈 기준 부위 분할. | 통과 |
| 두 다리·부츠 | [왼쪽](solo_06_LeftLeg_Boot.png) / [오른쪽](solo_07_RightLeg_Boot.png) / [숨김 비교](hidden_contact.png) | 초록 하의·갈색 부츠와 양발 간격 유지. 깊이 방향으로 앞뒤 위치 차이를 부여하고 바닥 접지 확인. | 통과 |
| 가방 | [단독](solo_08_Bag.png) / [숨김](hidden_08_Bag.png) | 원본 왼쪽 아래 초록 가방 실루엣 유지, 9칸 깊이로 확장. | 통과 |
| 새총 | [단독](solo_09_Slingshot.png) / [숨김](hidden_09_Slingshot.png) | Y자 형태, 주황색 줄·초록 프레임·검은 윤곽 유지. 3칸 깊이. | 통과 |

| 검수 종류 | 판정 | 근거·범위 |
| --- | --- | --- |
| 저장 메시 재개봉 | 통과 | [saved_blend_validation.json](saved_blend_validation.json): 11,472개 큐브, 변 길이 0.0986m, 불일치 큐브 0 |
| 원본 색·실루엣 대응 | 통과 | 저장 메시의 1,082열을 팔레트로 재구성한 [투영 PNG](saved_mesh_projection.png): 원본 대비 차이 0픽셀. 조명 렌더의 색 일치를 뜻하지 않음 |
| 전체 부위 단독·한 부위씩 숨김 | 통과 | [단독 10부위](solo_contact.png), [숨김 10상태](hidden_contact.png), 실제 전환 상태 JSON |
| 전신과 볼륨 | 통과 | 정면·사선·반대편·깊이의 900×900 렌더를 확인. 머리/모자와 몸/사지의 두께가 구분됨 |
| 실제 원본 표시 크기 | 통과 | 원본 64×64 F1을 확인하고 저장 메시 투영 색·알파 대응 검증 |
| 원본 보존 | 통과 | 원본과 Source 보존본 SHA-256 동일 |
| Blender에서 열기·MCP 연결 | 통과 | [live_mcp_validation.json](live_mcp_validation.json): 실행 중인 Blender가 저장 모델을 열고, MCP로 모델 루트와 10개 메시를 조회함 |
| 호흡 애니메이션·다른 프레임 | 해당 없음 | 사용자 지정은 Frame 1 정적 모델 |
| Unity 적용·게임 표시 | 해당 없음 | Blender 구현 요청 범위 |

의도한 예외: 원본의 검정 윤곽은 전면 표면에 남기고 내부를 인접 재질색으로 채웠다. 큐브 사이 0.0014m 간격은 블록 경계 표현을 위한 것이다. 부위를 숨기면 모자·머리·장비 부착점이나 분할 접합면이 보이는 것은 정상이다.

최종 판정: Frame 1 블록 모델 시안 제작·저장·검수 통과. 뒷면과 깊이는 단일 2D 입력에서 추정했으며, 별도 다방향 디자인 검증이나 리깅·최적화를 수행한 모델은 아니다.
