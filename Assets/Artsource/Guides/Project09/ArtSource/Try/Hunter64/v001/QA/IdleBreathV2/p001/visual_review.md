# idle_breath_v2 · 다리 디자인 단순화 검수

검수일: 2026-10-14. 대상은 [저장된 idle_breath_v2.aseprite](../../../Clips/HU-01_idle/idle_breath_v2.aseprite)이며, [원본과 최종 해시](source_preservation.json)로 이번 결과를 식별한다. 입력은 [작업 직전 동결 파일](../../../Clips/HU-01_idle/Source/IdleBreathV2/p001/idle_breath_before.aseprite)이다.

검수 범위는 전체 5레이어·전체 5프레임의 단독/한 레이어씩 숨김/기본 합성, 실제 64px와 Point 8배, 1200ms 반복이다. 디자인 수정은 LowerBody y49~56에 한정한다. 이번 판정은 다리 단순화와 기존 부위 보존에 관한 것이며 캐릭터 전체 경계 재작화 판정이 아니다.

## 수치 검수 · 통과

[native_validation.json](native_validation.json)의 범위 밖 픽셀 변경0, 다른 네 레이어 전20셀 raw 이미지·좌표·설정 변경0, 팔레트 불일치0을 확인했다. 바지·부츠는 프레임마다 같은48좌표를 수정했고, 옷자락 y43~48은 그대로다. 전5프레임 하체 고정 불일치0, 발 접지 y56, 부분 알파0이다.

64×64 Indexed·17칸·투명0·5레이어, `idle` F1~5 정방향 태그와 360/180/300/180/180ms 총1200ms를 유지했다. PNG·시트·GIF·8배 GIF·부위 마스크 불일치0이다. 기존15개 native와 동결 원본의 SHA-256은 그대로이며 candidate와 요청한 최종 파일의 해시가 같다.

바지 y49~50은 윤곽1+녹색2, 부츠 y51~56은 윤곽1+기본갈색6+세로 그림자5로 단순화했다. 부츠의 금색7과 바지의 녹색3 조각은 없어졌다. 하체 불투명191→192로 바뀌었으며 단순화는 색 면과 윤곽에 대한 평가다.

## 실제 표시 상태와 작화

기본 native와 출력은 Bow&Quiver만 숨기고, Slingshot&bag·LowerBody·UpperBody_Arms_Equipment·Head_Hat_Hair_Face를 표시한다. Lua에서 실제 `isVisible`을 전환해 [10개 native 상태](layer_review_states.json)를 저장하고 **각 상태를 독립 재개봉**했다. 단독5상태×5F, 하나씩 숨김5상태×5F를 실제 sprite에서 다시 렌더했다. 검수 복사본의 표시 전환을 최종 native에 저장하지 않았다. 실행하지 않은 UI 단축키 조작을 수행한 것으로 기록하지 않는다.

[전체 전후 8배](../../../Previews/IdleBreathV2/before_after_8x.png) · [하체 단독 전후](lower_before_after_8x.png) · [전체5F 실제64px](after_contact_1x.png) · [전체5F 밝은8배](after_contact_8x.png) · [전체5F 어두운8배](after_contact_dark_8x.png) · [독립 재개봉 수치 확인](Review/reopened_state_checks.json)

| 레이어 / 실제 확인 프레임 | 단독 근거 | 하나씩 숨김 근거 | 확인 항목 | 판정 | 의도한 예외 / 남은 문제 |
| --- | --- | --- | --- | --- | --- |
| Bow&Quiver / F1~5 | [solo01](LayerReview/Solo/solo_01_light_8x.png) | [omit01](LayerReview/Hidden/omit_01_light_8x.png) | 보존 활·화살통209px, source 동일, 기본 출력 제외 | 통과 | 숨긴 보존 장비의 확인이다. omit는 기본 합성과 같으며 현재 새총 장비 작화 통과와 구분한다. |
| Slingshot&bag / F1~5 | [solo02](LayerReview/Solo/solo_02_light_8x.png) | [omit02](LayerReview/Hidden/omit_02_light_8x.png) | 새총·자루, 손 접점·이동·다른 부위 혼입 | 통과 | 분리된 새총과 자루 사이의 투명 여백은 정상이다. 기존 장비 디자인 유지. |
| LowerBody / F1~5 | [solo03](LayerReview/Solo/solo_03_light_8x.png) | [omit03](LayerReview/Hidden/omit_03_light_8x.png) | 자락 보존, 단색 바지, 단순 부츠·발목·발끝·접지 | 통과 | 자락 중앙 패임과 두 다리 사이 틈은 형태를 설명하는 공간이다. |
| UpperBody_Arms_Equipment / F1~5 | [solo04](LayerReview/Solo/solo_04_light_8x.png) | [omit04](LayerReview/Hidden/omit_04_light_8x.png) | 상체·팔·손·허리, 가림과 하체 경계 | 통과 | 머리·하체와 결합하는 분할 레이어여서 단독 캐릭터를 만들지 않는다. 기존 셀 그대로. |
| Head_Hat_Hair_Face / F1~5 | [solo05](LayerReview/Solo/solo_05_light_8x.png) | [omit05](LayerReview/Hidden/omit_05_light_8x.png) | 모자·깃털·머리카락·얼굴·눈, 목 경계 | 통과 | 머리가 추가1px 내려가는 F3을 포함해 source 형태 유지. |

각 단독·숨김 폴더에는 동일한5F의 `_light_1x`, `_dark_1x`, `_checker_1x` 및 각각 `_8x`와 투명 개별F PNG가 있다. [독립 작화 검수](Review/visual_review.md)의 역할별 판정과 함께 확인한다. 수정하지 않은 부위의 보존 확인을 전체 픽셀 경계 정리 완료로 확대하지 않는다.

## 재생과 최종 상태

| 검수 종류 | 판정 | 근거 |
| --- | --- | --- |
| 저장 파일·출력 대응 | 통과 | native_validation.json, source_preservation.json |
| 전체 레이어 단독 표시 | 통과 | 저장 상태 독립 재개봉, 모든 단독25F를 64px·밝은/어두운/체크8배에서 작화 확인 |
| 하나씩 숨김·가려진 면 | 통과 | 모든 숨김25F 작화 확인, 실제 visible 및 기본5F 포함55개 PNG 대응 불일치0 |
| 전체 그림·전체5F | 통과 | 전후·하체·전체5F 이미지에서 다리 단순화와 기존 부위 보존 확인 |
| 실제64px | 통과 | after_contact_1x.png, preview_native64_dark.png |
| 반복·재생기 실제 브라우저 | 통과 | [실제 Edge 결과](browser_validation.json): 2주기·전5F·재개·1×/8×·3배경·375px 모바일·5링크, 오류0 |
| 원본 보존·표시 복원 | 통과 | 원본15파일 해시 유지, 최종 native 기본 표시 입력과 동일 |
| Unity 적용·실화면 | 해당 없음 | 이번 요청은 별도 Aseprite 시안 저장 |

필수 수정: 다리 단순화 영역에서 추가 필수 픽셀 보정은 발견하지 않았다. 레이어 분할에 따른 목·몸·옷자락 접점과 장비 사이 공간은 의도한 투명 영역이다. 수정 범위 밖의 기존 세부 윤곽과 다른 동작은 유지했으며 전체 재작화·다른 클립 전파는 수행하지 않았다.

최종 판정: 다리 단순화 시안의 수치·전체/단독/숨김 작화·64px·실제 브라우저 반복 검수 통과. [독립 검수](Review/visual_review.md)에서 추가 필수 픽셀 보정은 발견하지 못했다. 수정 범위 밖은 원래 디자인을 보존했으며 전체 캐릭터 재작화 완료나 다른 동작 전파·Unity 적용 완료를 의미하지 않는다.
