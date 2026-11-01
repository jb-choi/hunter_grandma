# 순덕 v002 검수 · 2026-10-29

2026-11-02 현재 정본은 **43프레임/9태그**다. 좁은 앞꿈치 walk_v2_tactical(28–33)과 reload_v2_tactical(34–39)/fire_v2_tactical(40–43)의 최신 검수는 [TacticalV2/QA/review.md](../TacticalV2/QA/review.md), [기계 검사](../TacticalV2/QA/validation.json)에 있다. 기존 첫27프레임의 픽셀·시간을 보존했고 새16포즈·42개 저장/재오픈 표시 상태·301개 전체 파츠 라이브러리 슬롯을 확인했다. 기존 Reload/Fire는 reload_v1_normal/fire_v1_normal로 이름을 정리했다. 아래는 최초27프레임 구성의 검수 기록이다.
## 파일 검사

- 저장한 편집 정본을 다시 열어 120×120 / 27프레임 / 6태그 / 7레이어를 확인했다.
- 투명 제외 전 프레임 RGB 합집합 26, 지정 팔레트 밖 픽셀 0, 부분 알파 0. 모든 프레임 바닥 마지막 픽셀 y117, 좌우 캔버스 끝에 닿는 픽셀 없음.
- PNG 재로드, 잘린 116개 파츠 재합성, 시트 셀 추출, 좌우 반전 후 복원: 모두 픽셀 차이 0.
- 6개 독립 동작 클립의 27프레임 및 7개 파츠 라이브러리의 189프레임 슬롯이 master 대응 이미지와 일치했다. 189는 고유 작화 수가 아니다.
- UI 초상/잠금 초상의 native와 PNG 2쌍 일치. 독립 샷건은 78×17이며 native/PNG 일치.
- 12프레임 Walk+Aim/Fire 시연의 Body는 대응 Walk Body와 정확히 같다.
- 저장/재오픈한 solo/hidden 상태 54개를 검증했다. Fire 대표는 FX가 있는 두 번째 프레임이다. 정본의 기본 표시 상태는 모두 켜져 있다.
- 출처 5개 SHA-256 보존 일치. v001 및 기존 64px 편집 없음.

[validation.json](validation.json) · [package_validation.json](package_validation.json) · [gif_validation.json](gif_validation.json) · [part_inventory.json](part_inventory.json) · [base_hashes_after.json](base_hashes_after.json)

## 작화 확인

실제 120px 전체 포즈, 2/3/4배 최근접 미리보기, 밝고 어두운 배경, 동작별 solo/hidden 시트를 시각 확인했다. 회색 곱슬머리·안경·깔깔이 누빔·꽃무늬 바지·와인색 신발이 이어지고, 조준/큰 반동/몸을 숙여 줍는 동작이 구별되는 것을 확인했다.

초기 자동 마스크에서 발견한 잘못된 손 선택, 걷기 총열 잘림, 원화 무기 잔여 픽셀, 총에 가렸던 소매 단절을 수정했다. 총에 가린 소매와 Reload 일부 손은 native 픽셀로 복원했다. 걷기는 최초 생성 원화를 보완한 6포즈를 사용하며 초기 다리 반전 보정은 제거했다. 캐릭터 위치는 걷기 머리/몸 기준으로 정렬하되 왼쪽 발 여백을 확보했다.

확인 이미지:
[전체 실제 크기](../Previews/all_poses_native.png) · [전체 2배](../Previews/all_poses_2x.png) · [밝음/어두움](../Previews/sundeok_light_dark_3x.png) · [걷기/조준/발사](../Previews/Walk_Aim_Fire_strip_2x.png).
동작마다 이 폴더의 `<Action>_solo_2x.png`, `<Action>_hidden_2x.png`와 `LayerStates/`에 실제 native 표시 상태를 남겼다.

이는 아트 시안/프레임 합성 검수이며 Unity 화면이나 Animator 전환을 테스트한 것은 아니다. 최종 게임 표시 배율·PPU·걷기 속도와 이벤트 시점은 통합 시 맞춰야 한다. UI 초상과 월드 전신은 별도 파일로 관리한다.
