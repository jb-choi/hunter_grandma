# walk 팔 흔들림·몸 높낮이 · p002

2026-10-12. [walk.aseprite](../../../Clips/HU-02_walk/walk.aseprite)의 F3·5·7에 팔의 반대 흔들림과 몸의 높낮이를 추가했다. 9프레임 구조와 4개 레이어는 유지한다. [수정 직전 walk](../../../Clips/HU-02_walk/Source/WalkKeyposes/p002/walk_before.aseprite), [기준 F1](../../../Clips/HU-02_walk/Source/WalkKeyposes/p002/frame01_keypose.aseprite), [입력 idle](../../../Clips/HU-02_walk/Source/WalkKeyposes/p002/idle_breath_source.aseprite), [해시](source_hash.json)를 보존한다.

| 프레임 | 몸통·머리·자락 y | 뒤손 x | 앞손·활 x | 발 |
|---|---:|---:|---:|---|
| 1 | 0 | 0 | 0 | 기존 idle 기준 자세 |
| 2 | 빈 프레임 | — | — | 셀 없음 |
| 3 | -1px | -1px | +1px | 왼쪽 발 통과 · 접지 y56 |
| 4 | 빈 프레임 | — | — | 셀 없음 |
| 5 | +1px | -2px | +2px | 반대발 착지 · 접지 y56 |
| 6 | 빈 프레임 | — | — | 셀 없음 |
| 7 | -1px | +2px | -2px | 오른쪽 발 통과 · 접지 y56 |
| 8 | 빈 프레임 | — | — | 셀 없음 |
| 9 | 0 | 0 | 0 | 1프레임 그대로 |

화살통은 몸통과 같은 y 이동만 적용했다. 활은 잡고 있는 앞손과 같은 x·y 이동을 적용했다. 팔의 어깨는 몸에 붙이고 손에 가까워질수록 좌우 이동을 늘렸으며, 원래 팔에 가려져 있던 상의 면을 같은 팔레트로 복원했다. 올라가거나 내려간 자락과 골반을 기존 다리 키포즈에 연결했다. 접지 부츠의 y51~56 픽셀은 이전 키포즈와 정확히 같다. 머리는 모자·깃털·머리카락·얼굴·눈·귀의 색과 형태를 그대로 정수 이동했다.

임시 분할한 몸통·뒤팔·앞팔은 `UpperBody_Arms_Equipment`, 활·화살통은 `Bow&Quiver`에 합쳤다. `LowerBody`, `Head_Hat_Hair_Face`를 포함한 기존 4개 레이어의 이름·순서·표시·불투명도·블렌드를 유지한다. 새 native 레이어를 추가하지 않았다. [부위별 작업 마스크](movement_masks.aseprite) · [실제 이동·상의 보완 명세](fullbody_keyposes.json) · [키포즈 편집 중간본](fullbody_keyposes.aseprite).

[수정 전 9프레임](before_contact_8x.png) · [수정 후 9프레임](after_contact_8x.png) · [키포즈 F1/3/5/7/9](keyposes_8x.png) · [실제 64px](keyposes_1x.png) · [재생기](../../../Previews/Animations/index.html)

| 파일 검수 | 결과 |
|---|---|
| 64×64 / 9프레임 / 4레이어 / Indexed 17칸 | 유지 |
| F1·9의 셀 이미지·크기·좌표·픽셀 | 변경 0 |
| F2·4·6·8 | 셀 0 / 불투명 픽셀 0 |
| 머리의 정수 y 이동, 활·화살통의 지정 이동 | 불일치 0 |
| 부츠 y51~56 | 변경 0 / 접지 y56 |
| native / PNG / 시트 / GIF / 8배 GIF / 마스크 | 불일치 0 |
| 태그·프레임 시간 | walk 1~9 / 120ms×9=1080ms 유지 |
| 반투명 픽셀 | 0 |
| 입력 idle와 나머지 애니메이션 native | 8개 파일 해시 유지 |

[native 재개봉 검수](native_validation.json) · [브라우저 검수](browser_validation.json) · [부위별 독립 검수](fullbody_keyposes_validation.json) · [전신·상의 면 시각 검수](fullbody_keyposes_visual_review.md). 이미지 재생성·블러·안티앨리어싱·디더링·색 보간은 사용하지 않았다.

| 시각 검수 범위 | 확인 내용 | 판정 |
|---|---|---|
| 머리·얼굴·모자·깃털 / 모든 키포즈 | 디자인 유지, 몸과 같은 높낮이 | 확인 |
| 상의·스카프·벨트·자락 / 모든 키포즈 | 상하 이동, 팔 아래 드러나는 상의 면, 허리 접점 | 확인 |
| 양팔·소매·손 / F3·5·7 | 어깨 연결과 반대 흔들림 | 확인 |
| 활·활줄·화살통 / 모든 키포즈 | 손 그립과 등 접점 | 확인 |
| 바지·부츠 / 모든 키포즈 | 골반 연결, 들린 발과 접지 발 구분, 바닥 고정 | 확인 |
| 실제 64px·8배 확대·빈 프레임 포함 재생 | 높낮이·팔 반전·프레임 구조 | 확인 |

중간 프레임 2·4·6·8은 사용자 요청대로 비워둔 키포즈 단계다. [p001 다리 키포즈 검수](../p001/README.md)는 상체가 고정된 이전 단계의 기록이며 현재 팔과 몸 높낮이 판정은 이 기록을 사용한다.
