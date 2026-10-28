# idle 기반 새총·자루 동작 p001

현재 idle_breath.aseprite F1의 디자인과 5개 레이어 구성을 그대로 사용해 focus/shoot/collect를 교체했다. 기존 파일과 입력 source는 Source/SlingshotActions/p001/ 아래에 byte 단위로 보존했다. 최신 source·출력 해시는 [source_hash.json](source_hash.json).

- 조준: 4F·600ms. 새총 올리기·고무줄 당기기·조준 유지.
- 발사: 4F·400ms. F1 release·반동·회복·기준 idle 복귀.
- 수집: 4F·600ms. 몸/무릎 낮추기·F3 pickup·자루 쪽 손 회복.

native의 아래→위 레이어는 Bow&Quiver(숨김), Slingshot&bag, LowerBody, UpperBody_Arms_Equipment, Head_Hat_Hair_Face다. 기준 이름·순서·표시·불투명도·blend와 팔레트를 유지했다. 별도 부위 마스크는 각 클립 Masks/에 있으며 캐릭터 export에 넣지 않았다.

[focus](../../../Clips/HU-03_focus/focus.aseprite) · [shoot](../../../Clips/HU-04_shoot/shoot.aseprite) · [collect](../../../Clips/HU-05_collect/collect.aseprite) · [재생기](../../../Previews/Animations/index.html).

수치 검수: [native_validation.json](native_validation.json). native를 다시 열어5layer 설정/태그/duration/17칸palette, PNG·시트·GIF 일치, head 정수 이동, 기준 cel raw 보존, 부분알파0와 접지 y56을 독립 검사했다. 작화/브라우저 판정은 별도 최신 기록을 따른다.

[focus 전후](focus_before_after_8x.png) · [shoot 전후](shoot_before_after_8x.png) · [collect 전후](collect_before_after_8x.png). idle/walk/carry_small/drag_large/load 및 Unity Assets는 이번 작업에서 수정하지 않았다.

수치 검수와 작화·재생 검수는 각각 통과했다. [작화 판정](visual_review.json) · [collect 독립검수](SourceReview/collect_review.md) · [focus/shoot 독립검수](SourceReview/focus_shoot_review.md) · [브라우저 검사](browser_validation.json) · [조준→발사 접점](transition_validation.json).

머리·모자·깃털·얼굴, 상의 숨은 면, 양팔·새총 그립/고무줄, 자루, 무릎·자락, 신발을 전12프레임에서 확인했다. 상의 내부 네모 구멍·손/새총 단절·자루의 바닥 관통은 보이지 않는다. collect F3는 원래 불투명 신발 픽셀을 보존하면서 기존 하체 빈 곳에 연결9픽셀을 추가했다.

재생 검수: 전체8동작·42프레임의 로드/선택/원크기64px/800% Point 표시, 반복4종·단발/유지4종, focus 마지막 자세 유지, shoot F1 발사와collect F3 줍기 이벤트, 소품 표시, 모바일390px와 납품 링크56개를 확인했다. [실제 크기](actual_size.png) · [조준](focus_keypose.png) · [발사](shoot_keypose.png) · [수집](collect_keypose.png).
