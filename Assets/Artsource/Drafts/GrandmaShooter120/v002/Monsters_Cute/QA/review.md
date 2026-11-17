# 얼굴 절반 코팅·피격 후 누운 Dead 검수
2026-10-30. 기존 v002/Monsters_Cute의 인간형 정본을 갱신했다. [편집 정본](../zombie_03_rescue_human_120.aseprite) · [전체 포즈](../z03_human/Previews/all_actions_2x.png) · [수정 전 보존](../QA/BeforeHalfFaceProne_20261030_205628).

## 파일 검사
최종 선택 빌드와 export_validate_monsters, qa_overviews, check_restored_human, check_half_face_prone을 실행했다. [원시 검사](validation.json) · [실행 로그](export_validate_monsters_stdout.txt).

| 종 | 프레임/클립 | 불투명 RGB | 비어 있지 않은 파츠 PNG | 저장/재오픈 표시 상태 |
| --- | --- | ---: | ---: | ---: |
| z01_normal | 28/5 | 26 | 56 | 20 |
| z02_large | 28/5 | 26 | 57 | 22 |
| z03_human | 28/5 | 26 | 63 | 22 |
| 합계 | 84/15 | 종별26 | 176 | 64 |

120×120, 알파0/255, x1..118/y1..117 여백, 종별 전 동작26색을 통과했다. native/개별 PNG/시트/좌우 반전/파츠 재합성/15클립/9파츠 라이브러리 비교는 픽셀 차이0이다. 라이브러리는 각28프레임, 총252슬롯이며 슬롯은 고유 작화 수가 아니다.

인간형28포즈의 HumanBody가 복원 샘플 PNG와 일치하고 점액 없는 별도 native도 일치한다. [복원 검사](restored_human_validation.json). 대표 Idle1/Walk2/Run3/Attack4/Dead6의 실제 solo/hidden 저장/재오픈은 인간형22건·전체64건이다. [인간형 상태 시트](../z03_human/QA/material_states_overview_2x.png). 기본 전 레이어 표시를 복원했다. GIF31개의 규격·프레임·시간 일치는 [gif_validation.json](gif_validation.json)에 기록한다.

## 새 작화 기준과 시각 확인
얼굴의 이미지 왼쪽 눈·볼·턱까지 청록 외피가 내려와 실제 얼굴 약절반을 가린다. 반대쪽 한 눈/볼과 작은 코·입은 읽히고, 귀여운 SD 비율과 전신 코팅을 유지한다. 전체28포즈와 밝고 어두운 배경에서 얼굴·코팅·손발·옷을 확인했다.

Dead6은 총에 맞은 반동 → 무릎 풀림 → 뒤/옆으로 넘어짐 → 옆/등으로 착지 → 누워 안정 → 누운 자세 유지다. 마지막에는 대부분의 슬라임이 몸에 남고 한쪽 어깨/가슴 일부만 벗겨진다. 다시 무릎 꿇거나 도움 손을 들지 않는다. 인간 몸체는 완전한 옷 입은 인물로 남으며 점액 웅덩이로 사라지지 않는다. 피격은 작은 점액 반동으로 표현했다.

누운 자세에서도 몸체·코팅·분리 방울을 편집할 수 있다. HumanBody만 켜면 누운 옷 입은 사람 전체가 보인다. SlimeCoat에는 원래 합성의 노출면 색/외곽선을 맞추는 보정 픽셀도 있어 점액만의 순수 마스크는 아니다. Dead 가로 정렬은 몸체 경계 중심을 사용해 마지막 발 위치 때문에 앞 프레임 체격이 과도하게 작아지지 않도록 했다.

[추가 동작 검사](half_face_prone_validation.json)는 Idle에서 복원된 따뜻한 얼굴색 위에 청록색이 보이는 비율이5.2%→38.6%로 늘었음을 확인했다. 이는 3차원 얼굴 표면적이 아닌 색 분류 보조 지표다. 최종 복원 몸체 경계는97×50px, 서 있는 피격 첫 몸체는59×93px이고, 마지막 몸체3131px로 첫4036px의 절반 이상이 남는다. 마지막 몸체 영역의 청록 픽셀은약53.7%로 코팅이 충분히 남아 있다. 최종 너비/높이와 몸체 픽셀 수 기준을 통과했다.

## 제작·보존·범위
Built-in ImageGen 총9회 중 이번 수정은 코팅/Dead 보드1회와 누운 사람 아래층 복원1회다. [프롬프트](../Source/generation_prompts.json)와 원화를 보관했다. 상단24셀의 기존 동작은 유지하고 얼굴 코팅과 하단6셀 Dead를 수정했다. Aseprite에서30셀 중Idle4+다른 동작 각6셀을 선택해28포즈로 만들고 팔레트·알파·정렬·파츠를 정리했다.

수정 전 인간형·두 원화 보드·출력·스크립트·문서를 해시와 함께 보존했다. [수정 범위](half_face_prone_revision.json). 일반/큰 좀비 native2개는 변경하지 않았다. 이전 Monsters·순덕·오토바이 원본 보존은 [기존 소스 검사](source_preservation.json)에 있다.

Dead JSON 의미는 shot_hit_fall_prone_partial_slime_peel, 단발·마지막 프레임 유지다. GIF는 반복 검토용이다. Unity Animator·프리팹·씬/피격 로직 연결이나 게임 실화면 테스트는 수행하지 않았다.
