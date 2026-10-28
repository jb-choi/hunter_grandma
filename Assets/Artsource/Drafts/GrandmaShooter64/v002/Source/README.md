# 입력·중간본·보존본

현재 편집 정본은 한 단계 위 루트의 `grandma_04~08_*.aseprite` 다섯 개다.

- `Generated/`: 후보별 built-in ImageGen 생성 원본. 각 1254×1254 투명 PNG. 원본은 사용자 기본 생성 폴더에도 남겨 두었다.
- `generation_prompts.json`: 공통 프롬프트, 각 후보의 개별 프롬프트, 최초 생성 경로와 이름.
- `*_sampled.aseprite / .png`: 원본 bounds를 64×64 캔버스 안으로 최근접 샘플한 초기 입력. 현재 결과와 구분한다.
- `*_curated.aseprite`: 재질별 공통 32색으로 맞춘 보정 전 중간본.
- `curated_palette.json`: 중간본 팔레트. 최종 피부색 보정이 반영된 팔레트는 `../Export/grandma_v002_palette.gpl`이다.
- `First64Pass/`: 얼굴색·안경 대비 보정 전 첫 64px 파일. 현재 편집 정본이 아니다.
- `Rejected/*_palette32.aseprite`: 내장 색상 축소 도구가 밝은 색을 잘못 처리한 실패 출력. 제작 입력과 최종 출력에서 제외했다. 최종 결과는 재질 팔레트 대응 후 전체 픽셀의 알파·색 포함 여부를 다시 검사했다.

외부 레퍼런스는 [조사 기록](../Reference/Research.md)의 링크로 관리한다. 외부 그림을 복제해 원화 입력으로 사용하지 않았다. 생성 원화와 변환·정리한 픽셀 베이스는 역할이 다르다.
