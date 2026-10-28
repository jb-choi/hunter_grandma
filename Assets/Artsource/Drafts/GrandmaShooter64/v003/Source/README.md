# 05·07 나이 인상 수정 입력

- `Before/`: v002의05·07 Aseprite·64px PNG·생성 원화를 바이트 그대로 보존했다.
- `Generated/`: 기존 원화에 노년 특징과 체형 수정을 지시한 built-in ImageGen 편집 결과. 각1254×1254, 투명 배경. 새 원본은 생성 기본 폴더와 현재 프로젝트 양쪽에 남겼다.
- `generation_prompts.json`: 현재 사용한 입력 이미지 경로·최종 프롬프트·생성 결과 경로. `edit_prompts.json`은 생성 전 편집 지시 보관본이다.
- `*_sampled.*`: 64×64 최근접 샘플 중간본.
- `*_curated.aseprite / curated_palette.json`: 명암·재질32색으로 맞춘 중간본.

현재 편집 정본은 상위 루트의 `grandma_05_redhood.aseprite`, `grandma_07_mosswitch.aseprite`다. 보존본과 중간본을 최신 정본으로 사용하지 않는다. 이전04·06·08은 v002에서 계속 관리한다.
