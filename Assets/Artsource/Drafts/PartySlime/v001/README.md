# Party Slime import v001

2026-11-04 · 사용자 제공 `HunterGrandma_PartySlime_Aseprite_Import_Bundle.zip`에서 가져온 10종.

- 보존 원본: `Originals/`의 PNG 10개와 README. ZIP SHA-256: `cd275b87e08664590c25bf79dd043d1a402ddfae231f9a1678737973667f8bb0`.
- 편집 정본: `Masters/Z01.aseprite`~`Z10.aseprite`, 각 120×120 / 16프레임 / RGBA. 제공 PNG는 평면 합성이므로 원본에서 없던 신체·장비 분리 레이어를 만들지 않았다. `Imported artwork (flattened source)` 한 레이어가 기본 표시/출력된다.
- 대응 PNG: `Assets/HunterGrandma/Art/PartySlime/Z01`~`Z10`의 `frame_01.png`~`frame_16.png`.
- 프레임 태그(1-based): Idle 1–4, Walk 5–8, Attack 9–12, Dead 13–16. Run은 Unity에서 Walk를 .08초 간격으로 재생한다. Idle .20초, Walk .13초, Attack .15초, Dead .09/.12/.16/.35초. 원본 시트의 추천 행 구성을 사용한다.
- 1254px는 4로 나누어떨어지지 않으므로 경계를 `floor(n*1254/4)`로 계산하고, 셀의 픽셀 중심을 최근접 샘플하여 120px로 옮겼다. 원본 알파 1~127의 배경 추출 노이즈를 제거하고 128 이상은 불투명하게 했다. 신규 작화나 생성형 재작화는 하지 않았다.
- Unity: Point / no mipmaps / uncompressed / FullRect / PPU32. 발 기준 pivot `(60,6)` (좌하단 기준), 별도 표시 배율 .8. 접점은 원본 바닥 그림자 기준이며 손잡이·총구 좌표는 이 캐릭터의 게임 기능에 사용하지 않는다.
- Z01 풍선 / Z02 파티 나팔만 실제 웨이브에 연결한다. Stage 0 유지, Stage 1부터 Z01, Stage 2부터 Z01+Z02. Z03~Z10은 추가 배치용 아트이며 스테이지에 자동으로 등장하지 않는다.
- 재설정 메뉴: `Tools → Hunter Grandma → Import Party Slime Bundle`. 모든 160 PNG의 Unity 임포트 설정과 현재 애니메이션 에셋을 구성한다.
- 재현: 프로젝트 루트에서 `Aseprite -b --script Assets/Artsource/Drafts/PartySlime/v001/Scripts/import_bundle.lua`. 기존 정본은 덮지 않고 재검증한다. 원본 수정 후 새 파생본을 만들 때는 새 버전을 사용한다.

검수: `QA/visual_review.md`, `QA/source-validation.json`, `Docs/Validation/PartySlime-validation.md` 참고. 제공 원본 자체의 스타일·실루엣 변형은 유지했다. 그림의 이용 조건은 사용자 제공 자료에 따르며 pixelfx의 MIT와 별개다.
