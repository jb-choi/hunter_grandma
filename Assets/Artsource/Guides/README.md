# Aseprite 제작 문서 관리

갱신일: 2026-10-21 · project10 Hunter Grandma

이 폴더를 Aseprite 작업 문서의 단일 입구로 사용한다.

새 캐릭터 탐색 자료: [레퍼런스 조사](../Drafts/GrandmaShooter64/v002/Reference/Research.md) · [현재 후보와 05·07 노년 인상 수정](../Drafts/GrandmaShooter64/v003/README.md) · [초기 추가 후보 04–08](../Drafts/GrandmaShooter64/v002/README.md)

## 먼저 읽기

1. [project10 작업 규칙](AsepriteWorkflow.md)
2. [범용 제작·QA 가이드](Project09/Docs/AI/Art/02_Production/ArtworkGuide.md)
3. [저장·납품 가이드](Project09/Docs/AI/Art/02_Production/ProductionGuide.md)
4. [픽셀 면·경계 정리 가이드](Project09/Docs/AI/Art/02_Production/PixelCleanupGuide.md)
5. [64px 캐릭터 작업 이력](Project09/ArtSource/Try/Hunter64/v001/README.md)

## 수집 범위와 원본 관리

- project09 `Hunting_idle/ArtSource`의 MD 전체, `Docs/AI/Art`의 MD 전체, Hunter64 통합 문서·게임 전달 README·동물 Aseprite 출처 문서와 Aseprite 작업 경로가 있는 프로젝트 문맥 문서: **205개**.
- [전체 색인](INDEX.md)과 [원본 경로·SHA-256 목록](project09_manifest.json)으로 관리한다.
- `Project09/`는 2026-10-21의 **참고용 스냅샷**이다. 원문 내용을 고치지 않으며 원본 project09 파일도 이동하거나 수정하지 않는다.
- 과거 `AGENTS.md`는 아카이브 안에서 `AGENTS.project09.md`로 보관했다. 과거 프로젝트 지침을 새 프로젝트의 활성 지침으로 적용하지 않기 위한 파일명 변경이며 내용과 해시는 같다.
- AD-01 스타일 가이드 PNG와 참고 팔레트 GPL도 같은 상대 경로에 보관했다. 이 두 부속 자료의 출처·해시는 [부속 자료 목록](project09_support_manifest.json)에 기록한다. 과거 참고 팔레트이며 project10의 채택 팔레트가 아니다.
- 문서 안의 상대 링크는 원래 폴더 구조를 유지한다. 위 두 부속 자료 외의 그림·영상·Aseprite·HTML 등 비문서 자료는 복사하지 않았다. 해당 자료는 manifest의 `source_absolute`를 기준으로 원래 project09 폴더에서 확인한다. 과거 `AGENTS.md` 링크는 `AGENTS.project09.md`를 사용한다.

## 갱신 방법

project10의 공통 규칙은 `AsepriteWorkflow.md`, 개별 작업 결정과 검수는 해당 버전의 `README.md`와 `QA/visual_review.md`에서 관리한다. project09의 새 문서를 추가 수집할 때는 기존 스냅샷을 덮지 않고 새 날짜 폴더와 별도 manifest를 만든다. 수집본이 해당 프로젝트의 현재 정본이라는 의미는 아니다.
