# focus_rework2 · 8프레임 focus 실루엣

## 현재 · 8프레임 focus 실루엣 시안

2026-10-13. 같은 [focus_rework2.aseprite](focus_rework2.aseprite)에 기준→새총 들기→고무줄 잡기→당기기→조준 유지의8F를 추가했다.64×64·8레이어·부위8색은 유지하며 F1의 셀 이미지·좌표도 그대로다. 태그`focus`1~8,총1210ms,마지막 조준 자세 유지다.

[재생기](Previews/Rework2/Focus8/index.html) · [전체8F](Previews/Rework2/Focus8/focus_contact_4x.png) · [제작·검수](QA/Rework2/Focus8/p001/README.md) · [시안 판정](QA/Rework2/Focus8/p001/visual_review.md) · [기준1F 보존본](Source/Rework2/Focus8/p001/focus_rework2_base.aseprite).

아래는 부위 분리 당시의1F 기록이다. 현재 native의8F와 당시 정적 출력/QA를 구분하며, 당시 원본은 위 보존본에서 확인한다.

## 이전 · Frame1 분리 기록

2026-10-13 · 현재 단계: 기준 Frame1의 8부위 분리. [focus_rework.aseprite](focus_rework.aseprite)의 F1을 참조해 같은 폴더에 [focus_rework2.aseprite](focus_rework2.aseprite)를 별도 저장했다.

64×64 Indexed, **1프레임·8레이어**, 팔레트9칸(투명0+부위8색). F1 duration150ms, `focus_base` 태그1~1은 정적 기준 자세이며 조준 동작을 추가한 기록은 아니다.

![부위별 색과 Frame1](Export/Rework2/focus_rework2_preview.png)

| 부위 / 레이어 | 단색 | 포함 범위 |
| --- | --- | --- |
| 머리_모자포함 | `#F4C65E` 노랑 | 모자·깃털·머리카락·얼굴·귀를 합친 머리 실루엣 |
| 상반신_몸통 | `#60AB72` 초록 | 목·상의·벨트, 팔에 가려진 상의 면 |
| 왼팔 | `#EB7581` 분홍 | 화면 왼쪽의 아래로 내려온 팔·소매·손 |
| 오른팔 | `#54C6DE` 하늘색 | 화면 오른쪽의 새총을 잡은 팔·소매·손 |
| 하반신_몸통 | `#A275D4` 보라 | 골반과 튜닉 자락 |
| 오른다리 | `#E99248` 주황 | 화면 오른쪽의 다리·부츠, 자락 아래 허벅지 |
| 왼다리 | `#5E83E1` 파랑 | 화면 왼쪽의 다리·부츠, 자락 아래 허벅지 |
| 새총+자루 | `#687C8B` 회청색 | 새총·고무줄·자루. 서로 떨어진 두 물체를 한 레이어/색으로 관리 |

좌우는 화면 기준이다. 모든 부위에서 기존 눈·무늬·명암·윤곽색을 해당 부위의 한 색으로 바꿨다. 바깥 실루엣과 새총 내부·발 사이 같은 구조적 투명 공간은 유지한다. 원본의 숨긴 Bow&Quiver는 이 8부위 작업본에 넣지 않고 입력 보존본에 남겼다.

레이어는 아래→위 `새총+자루 / 왼다리 / 오른다리 / 하반신_몸통 / 상반신_몸통 / 왼팔 / 오른팔 / 머리_모자포함` 순서다. 모두 visible, opacity255, normal이다. 기본 합성에서 팔은 몸통 앞, 자락은 다리 앞에 놓인다.

단독 표시에서도 연결을 확인할 수 있게 팔 뒤 상의37px과 가려진 허벅지의 포즈 기반 실루엣을 보완했다. 허벅지는 원본에서 보이던 그림을 복구했다고 주장하지 않는다. 앞의 팔·자락에 가려지므로 합성의 불투명 범위는 원본 F1과 정확히 같다(1082px, 알파 차이0).

- [원본 보존본](Source/Rework2/p001/focus_rework_reference.aseprite), [입력 해시](QA/Rework2/p001/source_hash.json)
- [64px 투명 PNG](Export/Rework2/focus_rework2_frame1.png), [8배 표시](Export/Rework2/focus_rework2_frame1_8x.png), [색·레이어·좌표 메타](Export/Rework2/focus_rework2.json)
- [8부위 단독](QA/Rework2/p001/solo_contact_4x.png), [한 부위씩 숨김](QA/Rework2/p001/hidden_contact_4x.png), [원본과 비교](QA/Rework2/p001/reference_vs_rework2_8x.png)
- [QA 판정](QA/Rework2/p001/visual_review.md), [native 검사](QA/Rework2/p001/native_validation.json), [PNG·원본 보존 검사](QA/Rework2/p001/independent_validation.json)

이번 결과는 부위·색 분리 기준이다. Focus의 들기·당기기·유지 포즈와 애니메이션은 다음 작업 단계에서 구성한다.
