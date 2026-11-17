# 05–08 역할 변경 · 화방/기관총/저격/오토바이

2026-10-28. 새 편집 정본 네 개는 v002 루트의 `grandma_05_flamethrower.aseprite`, `grandma_06_machinegun.aseprite`, `grandma_07_sniper.aseprite`, `grandma_08_motorcycle.aseprite`다. 기존 05–08 정본과 순덕/차 마시기 파일은 보존했다.

## 요청과 근거

[기획서_02](https://app.notion.com/p/4067313398e08130965aebafbb62a76d)에서 화방의 전방 범위 화염, 기관총의 지속 제압/탄막, 저격의 긴 사거리/단발, 운송의 지원 역할을 읽었다. 기획서 수레 대신 오토바이를 사용한 것은 이번 사용자의 명시적 요청이다. 게임 수치·해금 순서·추가 전투 로직을 이 아트 작업에서 결정하지 않았다. [읽은 페이지 스냅샷](notion_source_snapshot.md).

## 제작 범위

기존 native의 머리·색상·의상 픽셀을 재사용하고 Aseprite Lua 이미지/레이어 API로 무기, 연결 팔과 손, 효과를 직접 작화했다. 08은 카우보이 모자/얼굴/땋은 머리를 유지하고 몸통·굽힌 하체·오토바이를 새로 그렸다. 이미지 생성/고해상도 축소 방식은 사용하지 않았다. 신규 파일은 구별되는 역할 이름을 사용한다. 작업 중간본은 `InitialPass`에 있고 편집 정본이 아니다.

## 파일 검사

| 항목 | 결과 |
| --- | --- |
| 규격 | 네 파일 모두 64×64 RGB, 1프레임/100ms, 8레이어 |
| 팔레트 | 각 33슬롯, 합성 사용색 26/29/30/28개 |
| 알파/팔레트 | 부분 알파 0, 팔레트 밖 불투명 픽셀 0 |
| 발/바퀴 | 전부 마지막 불투명 행 y60 |
| PNG 대응 | 기본 4개 및 효과 없는 PNG 4개, 저장 native 합성과 차이 0 |
| 레이어 표시 | 4명 × 8레이어 × 단독/숨김 = 64개 실제 상태 native 저장·재개봉 및 visible 값 검사 |
| 기본 표시 복원 | 네 편집 정본 재개봉 시 모든 레이어 표시 및 최초 합성 차이 0 |
| 기존 원본 | 사전/사후 SHA-256은 `source_preservation.json` 참조 |

[검사 JSON](validation.json) · [납품 해시](delivery_hashes.json). 실루엣 PNG는 효과 없는 합성을 검정으로 변환했으며 캐릭터와 장비만 포함한다.

## 작화 검수

전체 확대, 실제 64px 비교 시트, 밝고 어두운 배경을 확인했다. 05의 연료통/호스/화염, 06의 큰 총 본체/드럼 탄창/급탄 벨트, 07의 얇고 긴 총열/분리된 조준경, 08의 바퀴/핸들/연료탱크/운송 가방으로 역할을 구분했다. 이전 무기의 떠 있는 잔여 픽셀을 제거했다. 손–총/핸들 접점, 발과 바퀴의 바닥 경계, 캔버스 가장자리 여백을 확인했다.

| 캐릭터 | 밝음/어두움 | 단독 표시 | 하나씩 숨김 |
| --- | --- | --- | --- |
| 05 화방 | [비교](grandma_05_flamethrower/light_dark_6x.png) | [시트](grandma_05_flamethrower/solo_3x.png) | [시트](grandma_05_flamethrower/hidden_3x.png) |
| 06 기관총 | [비교](grandma_06_machinegun/light_dark_6x.png) | [시트](grandma_06_machinegun/solo_3x.png) | [시트](grandma_06_machinegun/hidden_3x.png) |
| 07 저격 | [비교](grandma_07_sniper/light_dark_6x.png) | [시트](grandma_07_sniper/solo_3x.png) | [시트](grandma_07_sniper/hidden_3x.png) |
| 08 오토바이 | [비교](grandma_08_motorcycle/light_dark_6x.png) | [시트](grandma_08_motorcycle/solo_3x.png) | [시트](grandma_08_motorcycle/hidden_3x.png) |

시트는 가로 4칸, 왼쪽 위부터 뒤 장비/차량, 몸통·하체, 머리, 먼 팔 / 무기·차량, 가까운 팔, 손, 효과 순서다. 신규 무기·팔·손은 분리했으며 몸통은 팔 뒤까지 그렸다. 재사용한 원본 의상에는 망토·팔다리의 통합 픽셀이 일부 남아 있어 회전/연속 동작용 완성 리그로 판정하지 않는다. 특히 05의 망토와 08의 바퀴는 어두운 배경에서 외곽 대비가 낮다. 현재 정적 시안의 형태와 접점을 확인한 것이며 Unity 실제 전투 화면 대비를 검증한 것은 아니다.

정적 native/PNG 4종과 검수 자료 완료. 연속 화염·연사·조준/발사·바퀴 회전 애니메이션, 잠금 해제 UI, 게임 연결은 수행하지 않았다.
