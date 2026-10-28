## 별도 walk 재시도 · WalkRetry/p001

2026-10-13. [walk_retry.aseprite](walk_retry.aseprite)는 기존 walk와 walk_another를 보존한 **별도 재시도**다. 기존 `walk.aseprite` 정본을 대체하지 않는다. [보존된 walk_another F1](Source/WalkRetry/p001/walk_another.aseprite)의 최신 LowerBody43점 수정(바짓단7·부츠36)을 반영하고, 몸·머리·새총·자루는 같은 source 디자인을 사용했다.

레이어 순서는 아래→위 `backgroud_temp`, `Bow&Quiver`, `Slingshot&bag`, `LowerBody`, `UpperBody_Arms_Equipment`, `Head_Hat_Hair_Face` 6개 그대로다. 배경·옛 활은 숨기고 새 장비·몸3개를 표시한다. `walk` F1~8은 전부 작화된 8F × 120ms = 960ms 반복이고, 별도 `cycle_close` F9는 source F1의 raw 셀·좌표와 같은 닫힘 참고로 반복에 넣지 않았다.

무릎 굽힘·반대 팔 흔들림·몸 높낮이·자루 지연·국소 자락 변형을 추가했다. F3/F7 들린 밑창 y53, 지지 밑창 y56이며 교차 발과 자락의 부분 가림은 의도한 겹침이다. source 부츠 삭제가 아니다. 기존 indexed 픽셀을 직접 사용했고 재생성·블러·안티앨리어싱·디더링은 사용하지 않았다.

[동작 안내](walk_retry.md) · [별도 재생기](../../Previews/WalkRetry/index.html) · [작업·검수](../../QA/WalkRetry/p001/README.md). [native 검사](../../QA/WalkRetry/p001/native_validation.json), [브라우저 재생 검사](../../QA/WalkRetry/p001/browser_validation.json), [독립 작화 판정](../../QA/WalkRetry/p001/Review/visual_review.md)를 각각 통과했고 [기존 10파일 해시](../../QA/WalkRetry/p001/source_hash.json)는 보존됐다. 아래는 기존 walk 제작 기록으로 유지한다.

## 현재 정본 · 새총·가방 전파 · EquipmentPropagation/p001

2026-10-12. 현재 편집 정본은 [walk.aseprite](walk.aseprite), 64×64 Indexed 17칸·11프레임·6레이어다. 사용자가 추가한 `Slingshot&bag` F1 셀의 이미지·좌표·픽셀을 보존하고 F3·5·7·9·10·11에 전파했다. 가방132px은 몸 높이를, 새총96px은 앞손과 몸 높이를 따른다. `walk` 태그1~9와 별도 `knee_drape` 태그10~11, 각120ms를 유지하며 F2·4·6·8은 모든 레이어에 셀이 없는 빈 프레임이다.

native의 아래→위 레이어는 `backgroud_temp`, `Bow&Quiver`, `Slingshot&bag`, `LowerBody`, `UpperBody_Arms_Equipment`, `Head_Hat_Hair_Face`다. 사용자 편집 격리 상태를 보존해 native는 새 장비만 표시하고 몸3레이어·옛 활·임시 배경은 숨김이다. 전체 캐릭터 PNG·시트·GIF·미리보기는 명시한 출력 정책으로 `Slingshot&bag`과 몸3레이어를 표시하고 `Bow&Quiver`·`backgroud_temp`를 제외한다.

[작업 직전 사용자 파일](Source/EquipmentPropagation/p001/walk_before.aseprite)과 [해시 기록](../../QA/EquipmentPropagation/p001/input_hashes.json)을 보존했다. 기존 캐릭터·장비·배경 셀은 픽셀·좌표 변경0이다. 평면 `idle.aseprite`와 HU-03~08은 이번 전파 범위에 포함하지 않았다.

수치 검수는 [native_validation.json](../../QA/EquipmentPropagation/p001/native_validation.json)의 장비·PNG·마스크·8배 GIF 불일치0, 부분 알파0, 원본 셀 보존으로 확인했다. 작화 검수는 점유7프레임을 실제64px와 확대에서 보고 새총–손 접점, 몸을 따르는 가방, 상의·자락·발 경계에 새 파손이 없음을 확인했다. [검수 안내](../../QA/EquipmentPropagation/p001/README.md) · [전파 전 장비](../../QA/EquipmentPropagation/p001/gear_before_8x.png) · [전신11프레임](../../QA/EquipmentPropagation/p001/walk_contact_8x.png) · [실제 크기](../../QA/EquipmentPropagation/p001/walk_contact_1x.png).

아래의 Bow 4레이어 설명과 단계별 검수는 장비 전파 이전 이력이다.

## 이전 무릎 굽힘·자락 별도 비교 프레임

2026-10-12. 편집 파일 뒤에 F10·11을 추가했다. F10은 기존 F3의 왼쪽 발 통과, F11은 기존 F7의 오른쪽 발 통과를 기준으로 무릎을 앞으로 내고 정강이를 뒤로 접었다. 올라온 허벅지 위의 자락 끝만 1px 올리고 기존 녹색 명암으로 접힘을 추가했다. 중앙 자락 notch와 반대쪽 패널을 보존했다. 기존 F1~9와 walk 태그1~9는 픽셀·셀·시간까지 보존했다. 새 포즈는 knee_drape 태그10~11에서 확인한다.

[전후 비교](../../Previews/WalkKneeDrape/index.html) · [최신 별도 프레임 검수](../../QA/WalkKneeDrape/p001/README.md) · [변경 마스크](Masks/knee_drape_changes.aseprite) · [수정 직전 9프레임](Source/WalkKneeDrape/p001/walk_before.aseprite)

# HU-02 / walk · 64×64 · 키포즈 작업본

2026-10-12. 현재 편집 정본은 [walk.aseprite](walk.aseprite)다. 사용자가 수정한 [idle_breath.aseprite](../HU-01_idle/idle_breath.aseprite) 1프레임을 기준으로 요청한 9프레임 구조를 만들었다. 중간 프레임 작화 전 단계이며 F2·4·6·8은 모든 레이어의 셀이 비어 있다.

[전체 재생기](../../Previews/Animations/index.html) · [9프레임 나열](../../Previews/Animations/walk_contact_6x.png) · [키포즈만 비교](../../QA/WalkKeyposes/p002/keyposes_8x.png) · [검수 기록](../../QA/WalkKeyposes/p002/README.md)

| 프레임 | 내용 | 타이밍 | 출력 |
|---|---|---|---|
| 1 | idle_breath F1 그대로 | 120ms | [PNG](Frames/walk_01.png) |
| 2 | 빈 프레임 | 120ms | [PNG](Frames/walk_02.png) |
| 3 | 발 교차 · 왼쪽 발 통과 · 몸 상승 · 팔 흔들림 | 120ms | [PNG](Frames/walk_03.png) |
| 4 | 빈 프레임 | 120ms | [PNG](Frames/walk_04.png) |
| 5 | 반대발 착지 · 몸 하강 · 팔 반대 움직임 | 120ms | [PNG](Frames/walk_05.png) |
| 6 | 빈 프레임 | 120ms | [PNG](Frames/walk_06.png) |
| 7 | 발 교차 · 오른쪽 발 통과 · 몸 상승 · 팔 반전 | 120ms | [PNG](Frames/walk_07.png) |
| 8 | 빈 프레임 | 120ms | [PNG](Frames/walk_08.png) |
| 9 | 1프레임 그대로 | 120ms | [PNG](Frames/walk_09.png) |

`idle_breath`의 4개 레이어 이름·순서·표시·불투명도·블렌드 설정을 유지했다. 아래→위: `Bow&Quiver`, `LowerBody`, `UpperBody_Arms_Equipment`, `Head_Hat_Hair_Face`. F1·9의 셀 이미지·좌표·픽셀은 기준과 정확히 같다. F3·7은 몸통·머리·자락을 1px 올리고, 착지 F5는 1px 내렸다. 팔은 다리와 반대로 흔들고 활은 앞손, 화살통은 몸통을 따른다. 발바닥과 부츠의 접지 픽셀은 p001 위치를 유지하고 골반 연결을 몸 높이에 맞췄다. 움직이는 팔 아래에 드러나는 상의 면을 보완했다. 임시 부위 분할은 기존 4개 레이어에 다시 합쳤다.

기존 walk의 120ms 프레임 시간을 사용해 전체 1080ms다. 태그 `walk` 1~9, Indexed 17칸 팔레트를 사용한다. 이미지를 재생성하거나 보간하지 않았다. 미리보기는 키포즈 검토를 위해 일시정지로 시작하며 재생하면 빈 프레임도 그대로 표시한다.

[기준 1프레임 보존본](Source/WalkKeyposes/p001/idle_frame01_keypose.aseprite) · [입력 idle 전체](Source/WalkKeyposes/p001/idle_breath_source.aseprite) · [기존 walk 보존본](Source/WalkKeyposes/p001/walk_before.aseprite) · [시트](Export/walk_sheet.png) · [프레임 명세](Export/walk.json) · [현재 파트 마스크](Masks/material_regions.aseprite) · [파트 정의](Masks/parts.json)

기존 6프레임·9레이어 walk와 상의 Pass02 검수는 이전 작업 기록이다. [이전 README](Source/WalkKeyposes/p001/README_before.md) · [이전 마스크](Source/WalkKeyposes/p001/material_regions_before.aseprite) · [Pass02 기록](../../QA/Animations/torso_pass02_visual_review.md). 현재 walk의 판정은 위 키포즈 검수를 사용한다.

현재 팔·높낮이 검수는 p002다. [수정 직전 9프레임](Source/WalkKeyposes/p002/walk_before.aseprite) · [p001 다리 키포즈 기록](../../QA/WalkKeyposes/p001/README.md).
