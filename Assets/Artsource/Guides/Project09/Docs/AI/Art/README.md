# Hunting Idle 아트워크 문서

작성일: 2026-10-11 · 기준: Notion 아트워크 목록, Cycle14 비율, Cycle15–19 아트 연결 코드, 기존 AD-01 v0.1.

**작업 시작: [범용 아트워크 제작·QA 가이드](02_Production/ArtworkGuide.md) · [전체 테마와 제작 방향](01_ArtDirection/ArtDirection.md).**

이 폴더는 게임의 아트 기준, 작업별 제작 지시, 검수용 참고 자료를 관리한다. 이번 작업은 Notion의 **「1. 전체 테마와 제작 방향」 문서화**다. AD-02~05의 제작 지시를 작성했으며, 해당 그림의 제작·게임 적용은 별도 작업이다.

후속 작업으로 2026-10-11에 사냥꾼 32×32 정적 베이스 리디자인을 제작했다. 기존 원본은 유지하고 새 편집 정본/출력은 `ArtSource/Production/Hunters/HU-BASE`에서 관리한다.

## 문서 찾기

| 문서 | 용도 | 현재 상태 |
| --- | --- | --- |
| [범용 아트워크 제작·QA 가이드](02_Production/ArtworkGuide.md) | 입력·보존·레이어 분리·단독/숨김·전 프레임 QA·기록 양식 | 2026-10-13 기존 제작·실패·검수 기록 종합 |
| [전체 테마와 제작 방향](01_ArtDirection/ArtDirection.md) | 테마·화면·캐릭터·배경·UI·제작 순서의 공통 기준 | v0.2 제작 기준 문서 작성 |
| [AD-01 규칙 시트 안내](01_ArtDirection/AD-01_StyleRules/README.md) | 기존 규칙 시트와 팔레트의 해석 | v0.1 원본 보존, 대조 결과 추가 |
| [AD-01 팔레트 대조](01_ArtDirection/AD-01_StyleRules/PaletteAudit.md) | 실제 32색·원본 사용색·후보 18색 비교 | 로컬 파일 검증 완료 |
| [AD-02 크기 비교 시트](01_ArtDirection/AD-02_SizeComparison/README.md) | 사냥꾼+6종의 비율·접점 제작 지시 | 문서 작성, 정식 시트 제작 대기 |
| [AD-03 숲 키아트](01_ArtDirection/AD-03_ForestKeyArt/README.md) | 숲+길+수레의 구도·레이어 제작 지시 | 문서 작성, 키아트 제작 대기 |
| [AD-04 UI 펼침/접힘](01_ArtDirection/AD-04_UIStates/README.md) | 같은 본 화면 위의 두 UI 상태 제작 지시 | 문서 작성, 시안 제작 대기 |
| [AD-05 공통 관리 패널](01_ArtDirection/AD-05_CommonPanels/README.md) | 6개 관리 기능이 공유하는 스킨 제작 지시 | 문서 작성, 시안 제작 대기 |
| [제작물 저장·납품 규칙](02_Production/ProductionGuide.md) | 폴더·파일명·버전·메타데이터·Unity 전달 | 신규 작업부터 적용할 기준 |
| [픽셀 면·경계 정리 기준](02_Production/PixelCleanupGuide.md) | 기준 예시·재질 마스크·경계 편집·전신 검수 | Hunter_300 v006부터 적용 |
| [HU-BASE 사냥꾼 32×32 리디자인](02_Production/HU-BASE_HunterRedesign/README.md) | 새 정적 베이스·분리 활·편집 원본 | v002 피드백 반영 수정본 출력, Unity 미적용 |
| [HU-01~08 고해상도 애니메이션 시험](../../../ArtSource/Try/Hunter_1254/v001/README.md) | 1254×1254 8동작/36프레임·비교 재생기 | v001 동작 시안/타이밍 시연, 정식 채택·Unity 적용 전 |
| [참고 자료](03_References/README.md) | Notion 출처와 보존한 현재 구현 화면 | 기존 QA 이미지 3장 보존 |

## 폴더 역할

```text
Hunting_idle/
├─ Docs/AI/Art/                       # 제작 기준·브리프·검수용 시안
│  ├─ README.md                      # 문서 입구와 진행 상태
│  ├─ 01_ArtDirection/
│  │  ├─ ArtDirection.md             # 전체 테마와 제작 방향
│  │  ├─ AD-01_StyleRules/            # 기존 v0.1 시트 + 팔레트 대조
│  │  ├─ AD-02_SizeComparison/
│  │  ├─ AD-03_ForestKeyArt/
│  │  ├─ AD-04_UIStates/
│  │  └─ AD-05_CommonPanels/
│  ├─ 02_Production/                 # 저장·납품·상태 관리 규칙
│  └─ 03_References/                 # 출처·현재 화면, 제작물과 구별
├─ ArtSource/                        # 새 작업의 편집 원본·작업 중 export
└─ Assets/Art/Sprites/                # Unity에서 사용하는 아트
```

AD-01의 기존 세 파일은 `Art/` 바로 아래에서 `01_ArtDirection/AD-01_StyleRules/`로 이동했다. **파일명과 파일 내용은 그대로다.** 기존 v0.1 이미지의 「32색 미확인」 문구는 작성 당시 상태이며, 현재 대조 결과는 `PaletteAudit.md`가 우선한다.

기존 `Assets/Art/Sprites/Hunters`, `Animals`, `개인작업`과 `ArtSource/Backups`, `Previews`는 현재 경로를 유지한다. 앞으로 새 작업만 제작 규칙의 경로를 사용한다. 예정 폴더는 해당 작업을 시작할 때 만들며 빈 디렉터리를 미리 늘리지 않는다.

## 업데이트 원칙

- 전체 감성·색·비율을 바꾸면 `ArtDirection.md`를 먼저 갱신한다.
- AD별 시안은 해당 ID 폴더에 저장하고 그 폴더의 README에 버전·상태·검수 결과를 기록한다.
- 작업 상태는 `문서 준비 → 제작 중 → 시안 검수 → 제작 완료 → Unity 적용 → 적용 검수`로 나눠 기록한다.
- 그림 파일이 존재한다는 이유만으로 「Unity 적용」이나 「적용 검수 완료」로 표시하지 않는다.
- 제작 원본과 게임용 export의 경로는 [제작 가이드](02_Production/ProductionGuide.md)를 따른다.
