# WalkRetry p001

새 비교 시도: [walk_retry.aseprite](../../../Clips/HU-02_walk/walk_retry.aseprite), [동작 기록](../../../Clips/HU-02_walk/walk_retry.md), [재생기](../../../Previews/WalkRetry/index.html). 기존 정본 walk와 수동 walk_another를 보존한 별도 파일이다.

source는 [walk_another F1 보존본](../../../Clips/HU-02_walk/Source/WalkRetry/p001/walk_another.aseprite)이다. [입력 비교](Review/source_review.md)에서 최근43점은 바짓단7점·부츠36점임을 확인했으며 coat/몸/머리/새 장비는 기존과 같다. 앞부츠를 x>=31로 나누어 최신73px 부츠 디자인을 유지했다.

8개 연속 포즈·120ms·960ms loop, F9는 raw F1 복사 참조. 접지 밑창 y56, 교차의 들린 발 y53, 반대 팔 흔들림과 몸 y0/1/0/-1, 자루 ±1px 지연, 국소 자락 굽힘을 구성했다. Guide/background/old bow는 캐릭터 출력에서 제외했다.

수치: [native_validation.json](native_validation.json) passed. PNG/시트/8F GIF/8배 GIF/마스크 일치0, source head/gear 정수 이동0, 보이는 source boot 색 변화0, F1/F9 raw 셀 변화0, source17색 팔레트 유지·부분알파0·접지56. [source_hash.json](source_hash.json)에서 기존10개 native 파일 보존을 확인했다.

작화: [Review/visual_review.md](Review/visual_review.md) 및 [9F 전체 확대](contact_8x.png) / [실제64px](contact_1x.png). 머리부터 신발과 장비까지 전9F을 검수한다. 브라우저 재생 결과는 browser_validation.json에 별도로 기록한다.

작화 검수: [최종 판정](visual_review.json)과 [독립 전신 검수](Review/visual_review.md) 통과. 전9F의 머리·얼굴·상의·양팔/손·새총·자루·허리/자락·무릎·부츠를64px/8배, 교차 발을16배로 확인했다. 네모 상의 구멍, 손과 그립 단절, 눈 형태 손실과 접지 파손은 확인되지 않았다. 부츠의 일부가 근접 다리·자락에 가려지는 부분은 의도한 깊이 표현으로 기록했다.

재생 검수: [browser_validation.json](browser_validation.json) passed. 그린8프레임의 선택/로드,960ms loop2회복귀, F9제외 및 별도참조,64px와800% Point, 배경전환,390px 모바일,7개납품링크와오류0을 확인했다. [재생기](../../../Previews/WalkRetry/index.html) · [데스크톱](player_desktop.png) · [실제 크기](actual_size.png).
