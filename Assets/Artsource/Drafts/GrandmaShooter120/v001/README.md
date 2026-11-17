# 순덕 120×120 · 재제작본

2026-10-29. [기획서_02 v1.7](https://app.notion.com/p/4067313398e08130965aebafbb62a76d), 사용자의 “순덕할매만 우선 수행”, “무기도 분리”, “output 다시” 요청을 반영했다.

## 편집 정본과 독립 파츠

| 결과물 | 편집 정본 | PNG/메타 |
| --- | --- | --- |
| 순덕 6동작 원본 | [grandma_01_sundeok_120.aseprite](grandma_01_sundeok_120.aseprite) | [기본](Export/sundeok_idle.png) · [발사](Export/sundeok_fire.png) · [잠금 실루엣](Export/sundeok_locked.png) |
| **샷건 독립 원본** | **[sundeok_shotgun.aseprite](Weapons/Shotgun/sundeok_shotgun.aseprite)** | **[샷건 PNG](Weapons/Shotgun/sundeok_shotgun.png)** · [피벗/그립/총구](Weapons/Shotgun/sundeok_shotgun.json) |
| 팔·총 없는 몸체 | [sundeok_body_base.aseprite](Parts/Body/sundeok_body_base.aseprite) | [PNG](Parts/Body/sundeok_body_base.png) |
| 앞팔 | [sundeok_frontarm_base.aseprite](Parts/Arms/sundeok_frontarm_base.aseprite) | [PNG](Parts/Arms/sundeok_frontarm_base.png) |
| 뒤팔 | [sundeok_backarm_base.aseprite](Parts/Arms/sundeok_backarm_base.aseprite) | [PNG](Parts/Arms/sundeok_backarm_base.png) |
| 걷기 + 조준/발사 합성 시험 | [sundeok_walk_aim_fire_demo.aseprite](sundeok_walk_aim_fire_demo.aseprite) | [프레임 시트](Previews/Walk_Aim_Fire_strip_2x.png) |

캐릭터 합성 기준은 **120×120px**, 샷건 독립 이미지는 **78×17px**다. 샷건 독립 원본에는 캐릭터·손이 없으며 손에 가렸던 개머리판·방아쇠/가드·펌프·총열을 복원했다. 전체를 확대하는 기존 도형 시안을 대신해 새 생성 시안을 픽셀 원본으로 정리했다.

## 원본 구조와 출력 정책

아래→위 레이어는 `Body`, `BackArm`, `Weapon`, `FrontArm`, `Ammo`, `Pickup`, `FX`다. 기본 전부 표시/Normal/opacity 255. **Idle을 포함해 무기는 모든 프레임에서 별도 레이어**다. 기획의 Idle 통그림은 합성 PNG/GIF로 제공하되, 최신 사용자의 무기 분리 요청에 따라 편집 원본의 Body에는 팔과 무기를 합치지 않았다.

- [전체 27프레임](Previews/all_poses_2x.png) · [6동작 대표 포즈](Previews/six_actions_3x.png) · [밝음/어두움](Previews/sundeok_light_dark_3x.png)
- `Export/Frames/<동작>/`: 정면 기준 오른쪽을 보는 120×120 합성 PNG.
- `Export/Left/<동작>/`: 수평 반전한 왼쪽 보기 검수 PNG. 방향별 별도 신규 시점은 아니다.
- `Export/Sheets/`: 동작별 120px 셀 시트와 프레임/시간/피벗 JSON.
- `Parts/<동작>/`: 필요한 불투명 bounds로 자른 파츠 PNG. **[sundeok_parts.json](Export/sundeok_parts.json)**의 `origin_xy`에 놓으면 합성 원본과 정확히 같은 픽셀 결과를 만든다.
- `Weapons/Shotgun/`: 완전한 별도 샷건 원본. `Parts/<동작>/Weapon_*.png`는 포즈에 맞춰 회전/반동을 적용한 인스턴스다.
- 색상은 [26색 GPL](Export/sundeok_26.gpl)을 전 동작에서 공유한다. 투명 제외 RGB 합집합 상한 26, 알파 0/255. 생성 고해상도 원본의 색상 수와 혼동하지 않는다.
- 전체 120px 좌표 원점은 좌상단, x 오른쪽/y 아래. 바닥 경계 y118, 기본 캐릭터 피벗 `(60,118)`(Unity 정규화 `(0.5, 0.016667)`). 파츠 원점·어깨·손·무기 피벗·총구는 [pose_metadata.json](Source/pose_metadata.json)에 있다. 회전/반동 시 실제 좌표가 달라지므로 모든 프레임에 같은 총구값을 고정하지 않는다.

## 동작 시안

| 동작 | 프레임 | 편집 원본 | GIF |
| --- | --- | --- | --- |
| Idle | 4 | [Idle](Clips/sundeok_idle.aseprite) | [미리보기](Export/Gifs/sundeok_idle.gif) |
| Walk | 6 | [Walk](Clips/sundeok_walk.aseprite) | [미리보기](Export/Gifs/sundeok_walk.gif) |
| Reload | 6 | [Reload](Clips/sundeok_reload.aseprite) | [미리보기](Export/Gifs/sundeok_reload.gif) |
| Aim | 3 | [Aim](Clips/sundeok_aim.aseprite) | [미리보기](Export/Gifs/sundeok_aim.gif) |
| Fire | 4 | [Fire](Clips/sundeok_fire.aseprite) | [미리보기](Export/Gifs/sundeok_fire.gif) |
| Collect | 4 | [Collect](Clips/sundeok_collect.aseprite) | [미리보기](Export/Gifs/sundeok_collect.gif) |

이는 **27개 합성 프레임 + 별도 파츠**로 구성한 아트/합성 시험이다. 걷기·탄 삽입·반동·집기는 파츠 변형으로 만든 1차 모션 시안이며, 최종 손가락·팔꿈치 중간 작화와 게임 내 애니메이션 품질 승인까지 완료했다는 뜻은 아니다. 수거 소품은 종류 미정의 임시 주머니이며 재화 이름/보상을 확정하지 않는다. 이번에 Unity Clip·Animator·씬 연결은 변경하지 않았다. 차 마시기·피격·승리 V·추가 시점과 다른 할매는 이번 범위에 추가하지 않았다.

## 제작 방법과 보존

**Built-in ImageGen**으로 기존 64px 순덕을 참고한 전신 시안, 팔/무기를 지워 가려진 옷을 복원한 몸체, 손 없이 완전한 샷건의 세 이미지를 만들었다. [생성 입력과 프롬프트](Source/imagegen_prompts.md), [생성 원본](Source/Generated), [축소 기록](Source/redesign_sampling.json). Aseprite에서 120px 최근접 샘플링·26색 재질 팔레트 매핑 후 팔 마스크/관절 변형·발 위치·파츠 접점을 정리했다. 고해상도 생성물을 처음부터 120px 수작업 작화한 것으로 표기하지 않는다.

기존 64px 원본과 게임 사용 PNG는 유지했다. 거부된 이전 120px 원본·스크립트·파츠는 [BeforeRedesign 보존 폴더](QA/BeforeRedesign-20261029-103643/hashes.json)에 있다. 이전 `build_sundeok.lua`는 보존용 제작 코드이며 현재 원본 생성에 사용하지 않는다.

현재 재현 순서: `sample_redesign.lua` → `build_redesign.lua` → `export_validate.lua`. 각 스크립트의 `root`는 이 v001 폴더다. 이미 편집한 정본에 재실행하기 전 별도 보존본과 해시를 만든다.

[검수 기록](QA/review.md) · [기계 검사](QA/validation.json) · [납품 해시](QA/delivery_hashes.json)
