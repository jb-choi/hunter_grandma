# 순덕 120px 재제작 · 파일/합성 검사

2026-10-29. 최신 사용자 피드백에 따라 외형을 built-in ImageGen 시안으로 다시 만들고, 샷건을 독립 native/PNG로 분리했다. 기존 도형 기반 초안은 `BeforeRedesign-20261029-103643`에 보존했고 현재 정본은 v001 루트의 `grandma_01_sundeok_120.aseprite`다.

## 요구사항 대응

| 근거 | 산출물·판정 |
| --- | --- |
| 순덕만 우선 | 다른 할매의 새 120px 원본은 제작하지 않음 |
| 깔깔이·짧은 샷건·120px | 파마/안경/꽃무늬 몸빼 정체성 유지, 누빔 외투를 새 시안으로 재제작. 월드 합성 120×120 |
| 무기도 분리 | **Weapons/Shotgun/sundeok_shotgun.aseprite + PNG**, 78×17. 원본은 손·인물 없이 개머리판·그립·펌프·총열 전체를 포함 |
| Body/앞뒤 팔 분리 | `Parts/Body`, `Parts/Arms`의 독립 원본. 생성 몸체에서 팔·총을 제거하고 가려진 옷 복원. 앞뒤 팔 마스크에 남은 금속 총기 픽셀 정리 |
| 전 동작 26색 | 27개 합성 프레임의 투명 제외 RGB 합집합 26색, native 팔레트 27슬롯(투명 포함) |
| 6동작 | Idle4 / Walk6 / Reload6 / Aim3 / Fire4 / Collect4. 여섯 native와 GIF |
| Idle 통그림과 편집성 | 합성된 통그림 PNG/GIF 제공. 최신 무기 분리 요청을 우선하여 편집 원본에서는 Idle도 Body·팔·무기를 나눔 |
| 걷기 중 조준/발사 | Walk 몸체 6장을 유지한 채 Aim/Fire 상체 파츠를 바꾸는 12프레임 native 시연 |

## 기계 검사

[validation.json](validation.json)에 실제 수치를 기록했다.

- 저장 master native 재개봉: 120×120, 27프레임, 6태그, 7레이어. 기본 전부 표시.
- 전 프레임 부분 알파 0, 팔레트 밖 불투명 픽셀 0. 전 동작 공유 26색.
- 합성 PNG 27장 차이 0. cropped part PNG 116개를 메타데이터 원점에 재합성해 차이 0.
- 왼쪽 반전→다시 반전 차이 0, 각 시트 셀과 원본 프레임 차이 0.
- 6개 동작 대표 프레임의 존재하는 레이어마다 실제 단독/숨김 상태를 native로 저장·재개봉: 54상태. 기본 합성 복원 차이 0.
- 걷기+조준/발사 시연의 Body가 Walk 원본과 12프레임 모두 픽셀 차이 0.
- 독립 샷건 native와 대응 PNG 차이 0, 실제 78×17.
- 전체 포즈 발바닥 마지막 행 y117, 바닥 경계 y118. 파츠 원점·어깨·그립·총구는 프레임별 JSON에 기록.
- 기존 64px 순덕·차 마시기 native/원본 PNG의 SHA-256은 `source_preservation.json` 참조.

## 시각 검수와 한계

[전체 포즈](../Previews/all_poses_native.png), [밝고 어두운 배경](../Previews/sundeok_light_dark_3x.png), 동작별 스트립과 단독/숨김 시트를 확인했다. 처음 시안의 기하학적인 비례를 새 얼굴·머리·자연스러운 옷 덩어리와 팔 형태로 바꿨다. 복원한 몸통에 손/무기가 남지 않으며, 완전한 무기와 팔을 각각 끌 수 있다.

단독/숨김 시트는 `Idle_solo_2x.png` / `Idle_hidden_2x.png` 등 6동작 이름을 따른다. 왼쪽 위부터 현재 존재하는 레이어 순서(Body, BackArm, Weapon, FrontArm, Ammo/Pickup/FX 해당 시)다. 원본 파일은 `LayerStates/`에 있고 정본이 아니다.

120px 변환 과정에서 일부 손가락·눈·꽃 장식은 간략화했다. Walk의 하체와 Reload/Collect 팔은 새 파츠를 변형한 **1차 모션 시안**이며 최종 중간 프레임 수작업 작화나 게임 플레이 애니메이션 승인을 뜻하지 않는다. 실제 원본 크기/확대 검토를 수행했으며 Desktop Companion 실제 창, Unity SpriteRenderer 가림/PPU/충돌·게임 재생은 수행하지 않았다. 순덕 외 캐릭터, 차·안락의자·승리 V 등은 이번 결과로 완료 처리하지 않았다.

## 재현과 출처

[ImageGen 프롬프트·입력·원본 경로](../Source/imagegen_prompts.md), [Notion 스냅샷](../Source/notion_02_snapshot.md), [색상/축소 기록](../Source/redesign_sampling.json), [프레임 접점](../Source/pose_metadata.json).

현재 제작은 `sample_redesign.lua` → `build_redesign.lua` → `export_validate.lua`다. 초기 `build_sundeok.lua`는 거부된 시안의 보존용 코드다. 최종 납품 해시는 [delivery_hashes.json](delivery_hashes.json)에 기록한다.
