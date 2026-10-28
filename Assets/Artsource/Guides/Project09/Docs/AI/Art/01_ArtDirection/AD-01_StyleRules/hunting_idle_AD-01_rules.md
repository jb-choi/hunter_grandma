# AD-01 — hunting_idle 아트 규칙 (v0.1)

출처: 2026-10-10 Notion 아트워크 제작 목록. 이 시안은 기존 동물 원본을 수정하지 않는다.

## 1. 팔레트의 출처
- **기존 에셋에서 확인된 4색:** OUTLINE `#18141F`, IVORY `#F7EACA`, TIGER `#E76518`, MAGPIE `#116974`.
- **기획서상 숲/UI 기준 5색:** FOREST `#3F614B`, LEAVES `#78915B`, WOOD `#AD7E58`, PAPER `#EFE5C8`, GOLD `#ECC96D`.
- **제안 추가 색 9색:** FOREST_SHADOW `#2B4438`, LEAF_HIGHLIGHT `#AABF7A`, WOOD_SHADOW `#76543E`, WOOD_LIGHT `#D2A47A`, IVORY_SHADOW `#CEC1A0`, TIGER_SHADOW `#AC421A`, TIGER_LIGHT `#F4A957`, MAGPIE_SHADOW `#0C4553`, MAGPIE_LIGHT `#4C9EA6`.

원본 `Assets/Art/Sprites/Animals/woodland_animals.gpl`의 32색 전부는 미확인이다. 배포한 `.gpl`은 **기준/확장 후보를 따로 모은 팔레트**이므로 원본 32색을 덮어쓰지 말아야 한다. 제안 색을 정식 채택하기 전 기존 32색과 중복되는지 검사한다.

## 2. 형태와 외곽선
- 기본 예제는 32×32 원본 픽셀 격자를 상정한다. 외곽선은 기본 1px, 주요 외곽색 `#18141F`.
- 눈/입/관절을 제외한 내부 검은 선을 줄인다. 작은 종은 귀·꼬리·얼굴을 최우선으로 판독한다.
- 나무·수풀은 캐릭터보다 명암과 채도 대비를 낮춰 장면을 방해하지 않는다.
- 비율/애니메이션 확대는 픽셀 정수 배율 + Point/Nearest로 한다. 안티앨리어싱/블러는 사용하지 않는다.

## 3. 명암 규칙
- 광원을 좌상단으로 가정. SHADOW–BASE–LIGHT 3톤을 기본으로 사용한다.
- 넓은 덩어리로 명암을 묶고 1px 무작위 점, 부드러운 그라디언트, 체커보드 디더링을 제한한다.
- FOREST: `#2B4438`(NEW) → `#3F614B`(REFERENCE) → `#78915B`(REFERENCE)
- WOOD: `#76543E`(NEW) → `#AD7E58`(REFERENCE) → `#D2A47A`(NEW)
- TIGER: `#AC421A`(NEW) → `#E76518`(EXISTING) → `#F4A957`(NEW)

## 4. 작업 전 검수
1. 사냥꾼/동물을 게임 내 실제 표시 크기로 놓아 실루엣이 1초 안에 식별되는가?
2. 1px 외곽선이 정수 배율에서도 매끈한 도트로 보이는가?
3. 배경/숲이 캐릭터의 얼굴·무기·화살보다 튀지 않는가?
4. 신규 색이 기존 32색 팔레트와 중복되는지 비교했는가?
5. 신규 스프라이트를 원본 `hunter/animals`와 나란히 놓아 이질감을 검수했는가?
