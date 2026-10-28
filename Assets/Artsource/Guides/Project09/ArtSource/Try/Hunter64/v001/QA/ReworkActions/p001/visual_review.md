# ReworkActions p001 · 전 레이어·전 프레임 시각 검수

## 2026-10-13 재검토 · 작화 판정 정정

현재 작화 판정은 **추가 수정 필요**다. 이전 검수는 레이어 분리·누락·픽셀 보존·그립 겹침을 확인했지만 원본 팔/손의 형태·관절 비율, 수집 동작의 회복 단계를 충분히 평가하지 못했다. 파일·표시 상태·브라우저 기능 검사의 통과는 유효하며 최종 작화/동작의 통과를 의미하지 않는다.

- `build.lua`의 `arm()`과 `hand()`는 팔을 일정 굵기의 선과 작은 손 블록으로 새로 그렸다. 조준에서 팔이 가죽 띠처럼 몸을 가로지르고, 팔꿈치·보호대·손의 원래 형태와 부피가 충분히 유지되지 않았다. 원본 부위의 형태 기준과 관절별 포즈를 먼저 정해야 한다.
- collect F3의 손 `(38,53)`은 앞 부츠 가까이에 겹쳐 수집 접점이 뚜렷하지 않다. [F3 관절 확대](LayerReview/Crops/collect_joints_f03_12x.png)에서 손과 신발의 구분을 다시 평가했다.
- collect F3→F4에서 자유손은 `(38,53)`→`(25,40)`, 약18.38px 이동한다. 머리도 `(+2,+5)`에서 기준 위치로 바로 돌아온다. 낮은 자세→일어남→자루로 옮김의 중간 단계가 생략되어 동작이 급하게 바뀐다.
- shoot의 고무줄 복귀는 확인되지만 놓는 손의 형태 변화와 팔의 반동·회복을 핵심 포즈로 다시 평가해야 한다. release 이벤트와 그립 픽셀 겹침만으로 발사 동작의 판독을 통과 처리하지 않는다.

앞서 “추가 필수 보정 없음”으로 보고한 판단을 정정한다. 이 재검토에서는 `.aseprite`나 PNG를 수정하지 않았다. 아래의 파일·레이어 근거와 이전 확인 범위는 유지하며, 작화 완료 판단은 이 정정과 수정 필요 항목을 우선한다.

2026-10-13. 대상은 `focus_rework`, `shoot_rework`, `collect_rework`이며 각4F·5레이어다. 작업자가 저장 native를 재개봉한 단독60F, 기본 조합에서 한 레이어씩 숨긴48F, 전체 합성12F를 확인했다. 비교 대상은 각 클립의 `Source/ReworkActions/p001/*_before.aseprite`다.

[전체12F 4배](../../../Previews/ReworkActions/rework_contact_4x.png) · [실제64px](../../../Previews/ReworkActions/actual_size_64.png) · [재생기](../../../Previews/ReworkActions/index.html).

## 실제 표시 전환과 판정

`validate.lua`에서 QA 복사본의 `isVisible`을 바꿔 저장하고, 다시 열어 표시 설정과 전4F를 읽었다. 단독은 한 레이어만 표시, 숨김은 옛 활을 숨긴 기본 조합에서 가시 레이어 하나씩만 숨겼다. 실제 상태·이미지 경로는 [native_validation.json](native_validation.json)의 `states`에 있다. 정본의 원래 표시 상태는 변경하지 않았다.

아래 각 링크는 해당 동작의 전체 레이어와 F1~4를 함께 보여 준다. 표의 판정 범위는 세 동작 전12F다.

| 레이어 / 전12F | 단독 근거 | 숨김·전체 근거 | 확인과 판정 |
| --- | --- | --- | --- |
| Bow&Quiver | [focus](LayerReview/focus_solo_checker_4x.png) / [shoot](LayerReview/shoot_solo_checker_4x.png) / [collect](LayerReview/collect_solo_checker_4x.png), 각 첫 행 | 기본 합성에서 항상 제외 | source F1의 옛 활·화살통 셀이 그대로다. 현재 새총 동작용 그림의 완성 판정과 구분한다. 보존·숨김 확인 통과. |
| Slingshot&bag | 위 단독 보드 각 둘째 행 | [focus 숨김](LayerReview/focus_hidden_light_4x.png) / [shoot 숨김](LayerReview/shoot_hidden_light_4x.png) / [collect 숨김](LayerReview/collect_hidden_light_4x.png), 각 첫 행 | 자루 실루엣 유지, 조준 고무줄 당김·발사 복귀, 손잡이·그립 연결 확인. 배경·옷 픽셀 혼입 없음. 통과. |
| LowerBody | 위 단독 보드 각 셋째 행 | 위 숨김 보드 각 둘째 행 | focus/shoot의 source 하체 유지. collect는 이동 자락·굽힌 무릎과 바짓단이 부츠까지 연결됨. 원래 부츠의 불투명 픽셀·접지 보존. 통과. |
| UpperBody_Arms_Equipment | 위 단독 보드 각 넷째 행 | 위 숨김 보드 각 셋째 행 | 상의 누락·접점 연속 확인은 유지. 팔/손의 형태·부피·관절·원본 스타일을 다시 정리해야 한다. **작화 수정 필요.** |
| Head_Hat_Hair_Face | 위 단독 보드 각 다섯째 행 | 위 숨김 보드 각 넷째 행 | 머리 전체 source 형태 유지. 얼굴·눈·귀·챙·깃털이 손/새총에 의해 훼손되지 않음. collect 하강, shoot 작은 반동은 정수 이동이다. 통과. |

밝은 배경뿐 아니라 단독 어두운 배경 보드도 보관했다: [focus](LayerReview/focus_solo_dark_4x.png), [shoot](LayerReview/shoot_solo_dark_4x.png), [collect](LayerReview/collect_solo_dark_4x.png). 인벤토리에 의도한 빈 레이어·그룹은 없다. 가이드와 마스크는 별도 파일에 있다.

## 동작과 작은 크기 판독

| 동작 / F1~4 | 전체 전후 | 주요 접점·표현 | 판정 |
| --- | --- | --- | --- |
| focus | [전후](Composite/focus_before_after_4x.png) | 무기 들기→당기기→볼 옆 조준. [F4 관절12배](LayerReview/Crops/focus_joints_f04_12x.png). 팔·보호대·손의 형태와 몸 앞 가림을 재작화해야 한다. | 작화 수정 필요 |
| shoot | [전후](Composite/shoot_before_after_4x.png) | F1 고무줄 복귀·F4 exact idle 유지. [F1 관절12배](LayerReview/Crops/shoot_joints_f01_12x.png). 놓는 손·반동·회복의 형태와 진행을 재평가해야 한다. | 작화 수정 필요 |
| collect | [전후](Composite/collect_before_after_4x.png) | 부츠·접지 보존은 유지. [F3 관절12배](LayerReview/Crops/collect_joints_f03_12x.png). 손–부츠의 구분과 최저 자세→일어남→자루 이동의 중간 단계가 필요하다. | 작화·동작 수정 필요 |

몸통·팔은 함께 조합하는 부위이므로 단독에서 완전한 캐릭터가 될 필요는 없다. 보존한 옛 장비가 정지 상태인 것도 현재 출력에 섞이지 않는 보존 정책이다. source 새총의 내부 공간, 당긴 고무줄/머리/팔 사이의 삼각형 공간, 양발 사이의 빈 공간은 정상 투명 영역으로 남겼다.

수치 검사에서 enclosed alpha 후보를 모두 기록했지만 이를 일괄 채우지 않았다. 전체 합성과 단독·숨김에서 옷 내부의 누락과 물체 사이 공간을 구분했다. 원본 소매/손/새총을 포함한 기준 자세는 focus F1, shoot F4, collect F1에서 원본 셀 이미지·좌표까지 정확히 보존한다.

## 검수 종류별 결과

| 검수 | 판정 | 근거·범위 |
| --- | --- | --- |
| 저장 파일·레이어 설정·출력 | 통과 | [native_validation.json](native_validation.json), 3native·12F, 285개 구조·픽셀·표시 상태 검사 |
| 실제 단독 표시 | 통과 | 15상태·60F, 기본 숨김 Bow 포함, 체크/밝은/어두운 배경 보드 |
| 한 레이어씩 숨김 | 통과 | 12상태·48F, 전체 상의와 장비/목/허리 접점 확인 |
| 전신·전 프레임·실제 크기 | 수정 필요 | 위 이미지의 누락/접점 확인과 별개로 원본 팔·손 형태, 수집 접점 및 회복 단계 보완 필요 |
| GIF 출력·보존 | 통과 | [independent_validation.json](independent_validation.json), GIF12F 픽셀/알파/시간, 기존native11파일 해시 |
| 실제 브라우저·재생·전환 | 통과 | [browser_validation.json](browser_validation.json), 합성/단독/숨김120선택,64px/512px Point, 마지막 유지·단발·조준→발사·이벤트·검수2주기·390px 화면·링크6개 |
| 실제 동물/수레·Unity·게임 실화면 | 미검수 | 이번 작업은 별도 세 동작의 편집본 제작이며 runtime 연결은 수행하지 않음 |

[실제 브라우저64px](browser_native64.png), [focus 마지막 유지](browser_focus_end.png), [shoot 종료](browser_shoot_end.png), [collect 종료](browser_collect_end.png), [390px 화면](browser_mobile390.png)도 확인했다. 모바일 요청800%는 화면에 맞는 정수500%·320px로 표시하며 실제 크기를 안내한다. 이미지 표시·프레임/종료/전환 검사와 정적 작화 판정을 함께 확인했다. 콘솔 오류와 누락 이미지 오류는0이며 상태 전환으로 supersede된 이미지 요청 취소는 별도 기록한다.

현재 작화 완료 판정은 수정 필요다. 팔·손의 형태 기준과 각 동작의 핵심 포즈를 먼저 수정하고, 같은 단독/숨김/전체/실제 크기·재생 조건에서 다시 평가해야 한다. 실제 게임의 사냥감 크기·pickup 위치·상태 전환은 후속 적용 단계에서 확인한다.
