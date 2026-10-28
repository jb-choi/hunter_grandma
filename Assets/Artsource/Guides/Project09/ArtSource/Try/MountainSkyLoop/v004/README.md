# BG-01+02 · 아기자기한 160×90 원경 · v004

상태: **시안 제작·레이어 검수 완료** · 2026-10-12.

사용자 요청: 기존 320×180도 수려하고 사실적인 인상이 강하므로 아기자기한 게임 컨셉에 맞춰 160×90으로 작업한다. 산·산림의 형태를 새로 제작하고 하늘·구름·먼 능선을 별도 레이어로 구성했다. 낮고 넓은 바위산, 크림색 큰 면, 덩어리 숲, 작은 구름을 중심으로 새 스타일을 잡았다.

- 편집 정본: [bg_mountain_sky.aseprite](bg_mountain_sky.aseprite) · **160×90**, RGB, 4레이어, 1프레임, 100ms, 태그 없음.
- 원본 PNG: [Export/bg_mountain_sky_160.png](Export/bg_mountain_sky_160.png).
- 최근접 4배 출력: [Export/bg_mountain_sky.png](Export/bg_mountain_sky.png) · 640×360. 각 논리 도트가 4×4로 표시된다.
- 레이어: 하늘 / 구름 / 먼 능선 / 주봉·산림.
- 단독 PNG: `Layers/Native160/`, 4배 PNG: `Layers/`.
- 원본 v003 보존: `Source/bg_mountain_sky_v003.aseprite`, SHA256 `26badcf6753d1abc028e5f8e87aef80a61106103cd3b7e5e6b3fdf1bb59d90b9`.
- 스타일 시안: **내장 ImageGen**으로 투명 산·산림 레이어를 생성했다. [Source/style_prompt.txt](Source/style_prompt.txt)에 프롬프트, [Source/imagegen_cozy_mountain.png](Source/imagegen_cozy_mountain.png)에 1672×941 생성 원본을 보존했다. 실제 최종 편집 규격은 160×90이다.
- **Aseprite 1.3.18.6**에서 하늘·작은 구름·낮은 능선을 그렸다. 산 시안을 논리 도트로 옮겨 새 색 면을 적용하고 좌우 연결의 세 열에서 14px를 정리했다. [Scripts/](Scripts/)에 제작·출력·visibility 검수와 독립 파일 검사 스크립트를 보존했다.
- 새 팔레트는 투명 1개와 스타일 색 14개, 최종 합성에서 실제 사용 13색이다. 레이어 알파는 0/255, 합성 PNG는 불투명이다. 캔버스를 trim하지 않았고 좌상단 원점·픽셀 좌표를 쓴다.

[전후 비교](Previews/style_before_after.png)는 왼쪽 v003, 오른쪽 v004를 같은 640×360 크기로 보여 준다. [3회 반복](Previews/repeat_3x.png), [이동 GIF](Previews/scroll_preview.gif), [HTML 재생기](Previews/scroll_preview.html)도 함께 보존했다. GIF는 80프레임·160ms·논리 2px 이동이며 12.8초로 반복된다.

[QA/visual_review.md](QA/visual_review.md)에 하늘·구름·능선·주요 산을 각각 단독 표시한 4개와 하나씩 숨긴 4개의 실제 Aseprite 상태, 확대·전후·반복 검수를 기록했다. Shift+X 키 입력을 실행한 것으로 보고하지 않는다. 파일 검수와 작화 판정을 구분해 남겼다. 정본·PNG·4배 출력·레이어·마스크·원본 보존과 GIF 80프레임 비교는 통과했다. HTML은 문법·파일 경로 검수이며 브라우저 실행 확인을 포함하지 않는다.

[Reference/cleanup_reference.aseprite](Reference/cleanup_reference.aseprite)는 주요 산의 반복 경계 정리 전후다. [Masks/material_regions.aseprite](Masks/material_regions.aseprite)는 실제 네 레이어의 알파 점유 영역이며 검수에서 대응을 확인했다. [제작 메타](Export/bg_mountain_sky.json)에 규격·레이어·색·출처 해시를 기록했다.

정본 SHA256 `a021e4a69b7f9a7094e4576c57be82a04a8d5ac30fbd2aaa1943b9d54ec0761b`.

최초 소재는 사용자가 제공한 한국 바위산 사진이며 [v001 Reference](../v001/Reference/mountain_photo.png)에 있다. 이번 생성에서는 [Reference/v003_display.png](Reference/v003_display.png)를 구성 참고로 사용했다. BG-01+02 작업 ID는 [사용자 Notion 페이지](https://app.notion.com/p/UI-3f5f3731816f81c09cd2c86fa306ef5f?source=copy_link)를 따른다.

사용자 지정 `ArtSource/Try` 안의 비교 실험이다. 레퍼런스의 세밀한 균열·나뭇잎·높고 날카로운 산 형태는 새 아기자기한 방향에 맞춰 줄였다. 사진의 전경 소나무·바위·꽃, 캐릭터·길·UI는 범위에 넣지 않았다. 이전 파일과 Unity 자산은 보존했다. 아직 Unity에 연결하지 않았으며 적용 시 BG-01 `HuntArtSet.sky`, BG-02 `distantForest`로 분리한 합성과 기존 참조를 확인해야 한다. 권장 import 시작값은 Sprite Single / Point / 무압축 / MipMap off이며 PPU·pivot은 화면 적용 단계에서 결정한다.
