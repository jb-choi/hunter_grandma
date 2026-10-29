# 순덕 120px v002 · 사냥씬 런타임 데이터

출처: `Assets/Artsource/Drafts/GrandmaShooter120/v002/README.md`, `QA/review.md`.

Composite 27장은 원본 Export/Frames PNG 복사본이다. Body 27장은 독립 Body 라이브러리의 전체 캔버스 출력이다. Upper 27장은 Library의 master 복사본에서 Body만 숨겨 출력했다. 원본 native/PNG를 편집하지 않았다. 새 픽셀 작화나 이미지 생성은 이번 연결 작업에 사용하지 않았다.

파일명/프레임 시간/총구 좌표는 `pose_metadata.json`에 대응한다. 머리/어깨 정렬은 Body 픽셀에서 원본 데모와 같은 기준으로 계산한다. `SundeokAnimationSetup`이 PPU32, FullRect, 피벗(.5,2/120), Point, 무압축/no mipmaps로 임포트하고 `Sundeok120AnimationSet.asset`을 생성한다. 게임 자산의 GUID는 Unity가 생성한다.

전신/몸체/상체는 모두 120×120이다. 전신 프레임을 따로 자르거나 중복 절차적 반동으로 변형하지 않는다. 런타임 합성은 Walk 몸체에 Aim/Fire/Reload 상체를 정렬하는 데 사용한다. 자세/발사 시점 연결 및 검증은 `Docs/Sundeok120-Animation-Integration.md` 참조.
