# idle 기반 focus·shoot·collect 별도 rework

2026-10-13 · [ArtworkGuide.md](../../../../../../../Docs/AI/Art/02_Production/ArtworkGuide.md) 적용.

**현재 상태: 작화·동작 추가 수정 필요.** 원본 팔/손 형태·관절 비율과 collect의 수집 접점·회복 단계에 대한 이전 통과 판정을 [재검토 기록](visual_review.md)에서 정정했다. 파일·레이어 표시 상태·브라우저 기능 검사 결과는 유지한다.

최신 [동결 idle_breath.aseprite](../../../Source/ReworkActions/p001/idle_breath.aseprite)의 F1을 사용했다. 기존 세 동작과 idle을 수정하지 않고 아래 파일로 저장했다. 이전 SlingshotActions/p001 결과와 별도인 rework_p001이다.

| 동작 | 새 편집 파일 | 규격·진행 | 전후 비교 |
| --- | --- | --- | --- |
| focus | [focus_rework.aseprite](../../../Clips/HU-03_focus/focus_rework.aseprite) | 4F × 150ms, 600ms. 무기 들기→당기기→조준 유지 | [4배 비교](Composite/focus_before_after_4x.png) |
| shoot | [shoot_rework.aseprite](../../../Clips/HU-04_shoot/shoot_rework.aseprite) | 4F × 100ms, 400ms. F1 release→반동→내림→idle 복귀 | [4배 비교](Composite/shoot_before_after_4x.png) |
| collect | [collect_rework.aseprite](../../../Clips/HU-05_collect/collect_rework.aseprite) | 4F × 150ms, 600ms. 낮춤→F3 pickup→F4 자루 쪽 손 회복 | [4배 비교](Composite/collect_before_after_4x.png) |

[비교·단독·숨김 재생기](../../../Previews/ReworkActions/index.html) · [전체 12F](../../../Previews/ReworkActions/rework_contact_4x.png) · [실제 64px](../../../Previews/ReworkActions/actual_size_64.png) · [시각 판정](visual_review.md).

## 입력·레이어·편집 방법

- 캔버스 64×64, Indexed, 투명 인덱스0, 기존 팔레트17칸. 전12F의 실제 불투명 사용색은15색.
- 아래→위: `Bow&Quiver`(숨김), `Slingshot&bag`, `LowerBody`, `UpperBody_Arms_Equipment`, `Head_Hat_Hair_Face`. 이름·순서·visible·opacity·blend를 idle과 같게 유지했다.
- 기존 Indexed 픽셀을 직접 사용했다. 팔·새총·자루는 임시 의미 영역으로 나누고 최종 결과는 원래 5개 레이어에 저장했다. 이미지 재생성·AA·블러·디더링·색 평균 보간을 사용하지 않았다.
- 머리·모자·깃털·얼굴·눈은 source 그대로 정수 이동한다. focus 전F는 고정, shoot F2는 -1x, collect F2/F3는 (+1,+3)/(+2,+5).
- 팔 아래 숨은 상의 양쪽을 복원하고, 소매→가죽 보호대→짧은 손 끝이 구분되게 팔을 그렸다. 조준의 앞팔은 팔꿈치와 손목의 높이 차를 둔다. 새총 고무줄은 조준 시 당김 손을 따르고 release에서 짧아진다.
- collect는 source 부츠의 모든 불투명 픽셀을 유지하며 골반·무릎·바짓단을 연결했다. 손과 자루 입구의 회복 접점을 별도로 잡았다. 최저 불투명 y는 전F에서56이다.
- 좌표는 64px 캔버스 좌상단, 0 기반, +x 오른쪽/+y 아래다. 이벤트·접점·시간은 각 `Export/Rework/<동작>_rework.json`에 기록했다. 접점 확인용 사냥감·발사체를 캐릭터에 합치지 않았다.

## 보존과 파일 역할

[기존 native 11파일 해시](original_hashes.json), [원본 보존·GIF 독립 검사](independent_validation.json)를 함께 보관한다. 입력 SHA-256은 `9063c92fba58858babf7ed4a94b59071a3376f355e4b756591884c6be1164151`이다.

클립별 `Source/ReworkActions/p001/<동작>_before.aseprite`에 기존 파일을 보존했다. 새 PNG는 `Frames/Rework/`, 시트·GIF·메타는 `Export/Rework/`, 프레임별 마스크는 `Masks/Rework/`에 둔다. 기존 Frames·Export·Masks·Animations 재생기는 그대로 유지한다.

[source_parts.aseprite](source_parts.aseprite)는 입력의 팔·몸통·새총·자루 분할 예시다. [cleanup_reference.aseprite](cleanup_reference.aseprite)는 원본 상체와 팔을 제외하고 복원한 상의 면을 별도 레이어로 보관한다. 이 참고·마스크는 캐릭터 export에 포함하지 않는다.

## 실제 QA

- [native_validation.json](native_validation.json): 저장 native의 독립 재개봉, 레이어 설정·팔레트 RGBA·태그·시간·원본 셀·머리·자루 이동·부츠·그립·PNG/시트·마스크·알파를 검사한다.
- `LayerReview/States/`: 실제 Aseprite visible을 설정해 저장·재개봉한 단독15개와 숨김12개, 총27개 상태. 단독60F와 숨김48F를 각각 렌더했다. 키보드 단축키를 실행한 것으로 보고하지 않는다.
- `LayerReview/Solo`, `Hidden`: 모든 상태의 개별64px PNG와 4F contact. 밝은/어두운/체크 보드, 관절12배 확대도 함께 만들었다.
- [visual_review.md](visual_review.md): 전 레이어·전 프레임을 확인한 작업자 작화 판단. 수치 검수와 구분한다.
- [browser_validation.json](browser_validation.json): 실제 Edge 실행 통과. 전체 합성/단독/숨김120개 선택, 64px/8배 Point, 배경3종, focus 마지막 유지·shoot/collect 단발, 조준→발사·release F1·pickup F3·stow F4, 검수 반복2주기와390px 화면을 확인했다. 콘솔·누락 이미지 오류0. 빠른 상태 전환으로 취소된 이미지 요청은 실제 로드 실패와 구분해 별도 기록했다.

세 동작의 별도 파일 저장과 파일·레이어 표시·브라우저 기능 검사를 수행했다. 작화·동작 완료 판정은 수정 필요로 정정했다. 실제 동물/수레 연결, Unity 적용과 게임 실화면 검수는 수행하지 않았다.

## 제작·재검수 코드

[inspect.lua](inspect.lua) · [build.lua](build.lua) · [validate.lua](validate.lua) · [prepare_previews.py](prepare_previews.py) · [validate_player.cjs](validate_player.cjs).

재생기는 데이터가 HTML에 포함되어 있어 `index.html`을 브라우저에서 직접 열 수 있다. 기본 focus는 마지막 유지, shoot·collect는 단발 종료이며 `검수용 반복`을 켰을 때만 재시작한다. 모바일에서는 요청 배율 중 화면에 들어오는 정수 배율로 표시하고 실제 px를 표시한다.
