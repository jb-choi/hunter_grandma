# 새총·자루 레이어 전파 p001

사용자가 walk 1프레임에 그린 Slingshot&bag 레이어의 기존 indexed 픽셀을 직접 복사·이동했다. 이미지 재생성, 블러, 안티앨리어싱, 디더링 없이 작업했다.

- 정본: [walk.aseprite](../../../Clips/HU-02_walk/walk.aseprite), [idle_breath.aseprite](../../../Clips/HU-01_idle/idle_breath.aseprite).
- walk: F1/3/5/7/9/10/11에 장비228픽셀(자루132·새총96) 전파. F2/4/6/8은 빈 상태 유지. walk 태그1–9와 knee_drape 태그10–11 유지.
- idle: 5프레임 모두 전파. Slingshot&bag visible, Bow&Quiver invisible. 기존 몸과 호흡 타이밍 유지.
- 새총은 기존 앞손/활의 정수 이동, 자루는 기존 몸통/화살통의 정수 이동을 정확히 따른다. 상세 좌표는 [propagation.json](propagation.json).
- walk native는 사용자의 장비 단독 편집 상태를 보존했다. 몸3레이어·Bow&Quiver·backgroud_temp 숨김, Slingshot&bag 보임. 전체 캐릭터 미리보기에서는 복제본에 몸3레이어와 새 장비만 켜는 명시적 export 정책을 적용했다. native 기본 표시 PNG와 캐릭터 PNG는 각각 따로 검증했다.
- [equipment_parts.aseprite](equipment_parts.aseprite)는 자루/새총 이동 구분에 사용한 마스크다. 캐릭터 export에 포함하지 않는다.

수치 검수: [native_validation.json](native_validation.json) passed. 작성된 walk F1 장비 raw/geometry 변경0, walk와 idle의 기존 cel raw/geometry 변경0, gear·PNG·GIF·mask mismatch0, partial alpha0. [source_hash.json](source_hash.json)에서 입력 백업과 기존 idle.aseprite 및 HU03–HU08 정본7개 보존을 확인했다.

작화 검수: [visual_review.json](visual_review.json). 머리·상의·팔/그립·자루·하의/무릎·신발을 전12개 점유프레임의 확대·원크기로 검토했다. 새총과 손의 연결, 자루의 높낮이, 몸의 가림을 확인했으며 새 상의 구멍이 보이지 않는다. walk의 비어있는 중간4프레임은 사용자 지정 상태로 남아 있다.

비교: [walk 전체](walk_contact_8x.png), [walk 원크기](walk_contact_1x.png), [idle 전체](idle_contact_8x.png), [idle 원크기](idle_contact_1x.png), [원본 장비](source_1_8x.png).

최신 재생기: [사냥꾼 애니메이션](../../../Previews/Animations/index.html), [idle](../../../Previews/IdleBreath/index.html), [무릎·옷자락 비교](../../../Previews/WalkKneeDrape/index.html). 기존 수치·브라우저 보고서 및 메타데이터는 Previous/에 보존했다. 이번 작업은 Unity Assets에 적용하지 않았다.

브라우저 검수: [browser_validation.json](browser_validation.json) passed. idle 5프레임·1200ms 반복, 전체 8동작·42프레임, 무릎 비교 2쌍의 표시·파일 링크·64px 원크기·Point 확대·모바일 레이아웃을 확인했다. [보이는 색 수](color_counts.json)는 투명색 제외 15색으로 유지됐다.
