# [일지] Hunter Grandma — D1부터 개발 진척

Notion 정본: https://app.notion.com/p/40b7313398e081678b53f560a2faa106

<callout icon="📒" color="blue_bg">
확인 가능한 Hunter Grandma의 최초 직접 기록인 <date start="2026-10-18" />을 <b>D1</b>로 삼았다. <b>D번호는 작업한 날의 순번이 아니라 시작일부터의 달력 일수</b>다. <date start="2026-11-01" />은 D15이며, 날짜는 모두 Asia/Seoul 기준이다.
</callout>

GPT Chat의 <b>CJB_게임만들기</b> 프로젝트 대화와 Codex의 <b>Project10_hunter_grandma</b> 작업 대화, Notion 기획서·스터디, 로컬 구현/아트/검증 문서와 Git 이력을 교차 확인한 회고 일지다. 기획·연구 완료, 아트 제작 완료, 코드 구현, 실행 검증을 구분했다. 테스트 수는 각 실행 묶음의 수이며 중복 회귀 검사를 날짜 간 합산하지 않는다.

기록 미확인은 작업이 없었다는 뜻이 아니다. 파일 수정 시각이나 대화 생성일만으로 성과 날짜를 결정하지 않고, 본문의 작성일·사용자 요청·완료 응답·실행 보고서의 날짜를 우선했다. 자정을 넘긴 작업은 완료 확인일로 기록했다.

<table fit-page-width="true" header-row="true">
<colgroup>
<col width="60" />
<col width="125" />
<col width="550" />
<col width="380" />
<col width="140" />
</colgroup>
<tr><th>Day</th><th>날짜 · KST</th><th>진척시킨 사항</th><th>결과·상태·남은 점</th><th>근거</th></tr>
<tr><td><b>D1</b></td><td><date start="2026-10-18" /></td><td>[조사·기획] Hunter Grandma의 샷건 외형·실루엣·재질과 탄환 발전사 레퍼런스를 문서화. 2인 기획·아트 협업을 위한 Notion 운영 방식과 대안을 검토했다.</td><td>확인 가능한 최초 Project10 기록 기준. 샷건 8종·탄환 자료는 아트·성장 아이디어이며 게임 구현 완료가 아니다.</td><td>N1 · N2 · C1</td></tr>
<tr><td><b>D2</b></td><td><date start="2026-10-19" /></td><td>검토한 기록에서 이 날짜의 직접적인 완료 산출물·작업 내역을 확인하지 못했다.</td><td>기록 미확인. 작업하지 않았다는 뜻은 아니며, 전날·다음날의 성과를 임의로 배정하지 않았다.</td><td>—</td></tr>
<tr><td><b>D3</b></td><td><date start="2026-10-20" /></td><td>[기획] Desktop Companion 기획서 작성. 빨간 공격좀비 사살·보상, 초록 안정좀비 포획·수레 운반, 치료 후 동료·상점 합류 루프를 정리했다. 샷건·탄환의 외형 교체와 업그레이드 보드 방식을 비교했다. 총기 업그레이드 방식을 9개 게임 사례와 함께 별도 스터디로 분류했다.</td><td>기획 문서 완료. 치료·상점·바탕화면 상주 기능은 구현한 상태가 아니다. 이후 변경되는 초기안으로 보존한다.</td><td>N0 · C2 · R1</td></tr>
<tr><td><b>D4</b></td><td><date start="2026-10-21" /></td><td>[아트] project09 Aseprite 가이드를 모아 제작 입구를 정리하고, 64×64 할머니 베이스 후보 1~3 및 4~8을 제작했다. 5·7번의 주름·노년 체형을 보강했다.<br />[조사] 픽셀 좀비 사격 레퍼런스와 보는 맛 중심 자료를 추가했다.</td><td>Aseprite·투명 PNG·레이어와 픽셀 일치 검수 완료. 이 시점의 후보는 정적 베이스이며 완성 애니메이션이 아니다.</td><td>C2 · C3 · L1</td></tr>
<tr><td><b>D5</b></td><td><date start="2026-10-22" /></td><td>검토한 기록에서 이 날짜의 직접적인 완료 산출물·작업 내역을 확인하지 못했다.</td><td>기록 미확인. 추가 자료가 확보되면 해당 날짜에 보완한다.</td><td>—</td></tr>
<tr><td><b>D6</b></td><td><date start="2026-10-23" /></td><td>검토한 기록에서 이 날짜의 직접적인 완료 산출물·작업 내역을 확인하지 못했다.</td><td>기록 미확인. 추가 자료가 확보되면 해당 날짜에 보완한다.</td><td>—</td></tr>
<tr><td><b>D7</b></td><td><date start="2026-10-24" /></td><td>[문서] Project10_Hunter_grandma와 하위 문서 6개·첨부 이미지를 현재 작업 공간에 복사했다. 원본은 유지하고 일부 사본 이미지를 용량에 맞게 압축했다. [조사] Cat Gunner v1.1.64의 성장 궤도·재미요소를 분석하고 로컬 근거를 보관했다.</td><td>현재 Notion 프로젝트 ‘Project10_Hunter_grandma · 복사본’이 작업 기준 위치로 마련됐다. 게임 기능 추가와는 구분한다.</td><td>C2 · N3 · R2</td></tr>
<tr><td><b>D8</b></td><td><date start="2026-10-25" /></td><td>[개발] 순덕 자동 사격, 계속 접근하는 좀비, 탄환 명중·쓰러짐·소멸, 반동·총성·처치 수를 구현했다. 지도 버튼/M, 6개 구역 선택, 지도 중 일시정지와 Esc 닫기를 연결했다.<br />[기획] Stage 0~6·반복 포획·보스 7종을 정리하고 아트 원본을 수집했다. 밤에 Stage v2 구현을 시작했다.</td><td>기본 전투 60초에 38마리 생성·32마리 처치, 전투 검사 16 PASS. 지도 검사 26 PASS.<br />밤의 Stage v2 코드는 추가했으나 당일 실행 검증은 미완료; 다음날 재시도로 확정했다.</td><td>C4 · C5 · L2 · L3</td></tr>
<tr><td><b>D9</b></td><td><date start="2026-10-26" /></td><td>[개발] Stage 0~6, 보스 7종, 제압→수레 적재→마당 귀환, 지도 순차 해금·반복 포획·강화·진행 저장을 통합했다.<br />[기획·조사] 스탯의 14개 분류축, 파워 그릇·재화·업그레이드·상호작용을 정리했다. 축적형 성장 후보와 Duelist Idle Expedition의 성장·스테이지 진행도 분석했다.</td><td>Stage v2 Play Mode 검사 115 PASS, 런타임 오류 0건. 이후 보스·포획 진행은 10/30에 현행 루프에서 제외된다. 성장 연구는 후보 체계이며 최종 스탯·재화 확정이 아니다.</td><td>C4 · C6 · C7 · L4 · R3 · R4</td></tr>
<tr><td><b>D10</b></td><td><date start="2026-10-27" /></td><td>[개발] 고정 횡방향 전투를 2D 벨트스크롤로 변경: WASD/방향키, Tab 자동 탐색, 카메라 추적, 발 기준 깊이 정렬, 깊이·장애물을 반영한 샷건 판정. 정지 씬의 미리보기도 갱신했다. 접이식 강화/U, 현재→다음 값·구매 상태, HUD 재화 획득 효과를 구현했다.<br />[기획] UI 우선순위·작업 ID·Chat/Codex 상태를 분리하고 ‘(미정)재화’ 표기를 채택했다. 승리 포즈·CLEAR·자동 재사냥의 엔딩 연출 사례를 스터디로 남겼다.</td><td>벨트스크롤 156 PASS, UI 71 PASS. 여러 할머니의 자동 이동·사격은 후속 항목이고 현재는 순덕 1명의 임시 자동 탐색이다. V 포즈·최종 성장 체계는 미완료다.</td><td>C4 · C8 · L5 · L6 · N7 · R7</td></tr>
<tr><td><b>D11</b></td><td><date start="2026-10-28" /></td><td>[아트·개발] 차 마시는 순덕, 좀비 발견·문 열고 나가기, 대각선 울타리와 반복 두드리기, 좀비 뒤로 빼기→앞치기→복귀 공격을 연결했다. 한글 Noto 폰트를 씬에 명시하고 공통 UI 생성 코드를 DefenseUiBuilder로 정리했다. 화방·기관총·저격·오토바이 역할별 아트를 추가했다.<br />[문서·개발] 기획서 01~04 역할을 정리하고 KRA 기반 5칸 할매 목록→강화 상세 연결창을 제작했다. 저장소를 초기 커밋·푸시했다. [조사] Duelist의 해상도별 화면/UI 조정 로직을 설치본에서 분석했다. 생활 코미디·자동 치료·누적 성장의 ‘좀비 치료 대작전’을 별도 아이디어 초안으로 남겼다.</td><td>도입/공격 48 PASS, 관련 재실행 묶음 275 PASS; UI 리팩터링 119 PASS. 최초 Git 커밋 c4d0cb4.<br />연결창 초기 검증은 부분 완료였고 103개 검사 통과와 초상 연결 확정은 10/29다. 위 검사 수는 중복 실행을 포함하므로 합산하지 않는다. 치료 대작전은 기획 초안이며 실제 치료 시스템 구현은 아니다.</td><td>C4 · C9 · C10 · L7 · L8 · L9 · G1 · R8 · N8</td></tr>
<tr><td><b>D12</b></td><td><date start="2026-10-29" /></td><td>[아트·개발] 순덕 120×120·26색·6행동/27프레임과 독립 샷건·몸체/팔/무기 파츠를 제작·통합했다. 이동 중 조준·발사·장전, 프레임별 총구, 수집 모션을 연결했다. 5명 카드·개별 잠금 실루엣의 UI 연결을 마쳤다.<br />[스킬] ‘할매의 밤마실’ ON/OFF, 전투 10초 자동 발동, 시간 정지·암전·후방 순간이동 5연사·복귀를 구현했다. Game View 가로 늘어짐을 16:9 재적용으로 복구했다. [조사] 데이브 더 다이버의 초기 사냥 에셋·스프라이트·색상 수 실측을 120px 제작량·26색 시험의 근거로 정리했다.</td><td>애니메이션 통합 352 PASS; 스킬 추가 후 회귀 묶음 385 PASS(스킬 자체 33). 별개 누적 합계로 더하지 않는다.<br />일반 프로필 스킬 기본 OFF, 표적이 먼저 전멸하면 조기 복귀. 전체 업그레이드 체계·다인 AI·다른 4명의 전투는 미구현이다.</td><td>C4 · C11 · C12 · L10 · L11 · L12 · R9</td></tr>
<tr><td><b>D13</b></td><td><date start="2026-10-30" /></td><td>[개발] 모든 보스 등장을 제외하고 일반 웨이브 소탕→CLEAR 1.5초→다음 Stage로 자동 이동하게 변경했다. 당시 마지막 Stage 6은 전체 CLEAR에서 정지. 장전 2번째 프레임에 CC0 철컥 소리를 연결했다. 스킬 시연이 짧은 원인이 OFF 설정·높은 공격력·적 조기 전멸임을 조사하고 격리 시연 메뉴를 추가했다.<br />[기획·아트] 소탕전/구조전 구분을 단일 Stage·선택적 동료 조우/영입으로 개정. 오토바이 18프레임과 귀여운 슬라임 좀비 3종/84프레임을 제작·수정했다. 관련 코드 커밋을 푸시했다. ‘The Art of Screenshake’ 전사본의 타격감 기법 30개를 문서화했다.</td><td>보스 제외·자동 진행 169 PASS, 장전음 28 PASS. 커밋 2b23f67·3fbebee·c824084 원격 반영.<br />보스 코드는 삭제하지 않고 비활성 경로로 보존. 동료 영입 이벤트·지도 마커는 기획만 완료. 귀여운 좀비 아트의 실제 게임 연결은 10/31이다.</td><td>C4 · C10 · C11 · C12 · N4~N7 · L13 · L14 · L15 · G2~G4 · R10 · C15</td></tr>
<tr><td><b>D14</b></td><td><date start="2026-10-31" /></td><td>[개발] 일반·감염체·큰 좀비 3종과 84포즈를 게임에 연결했다. Stage 1~4 출현 수·빈도를 늘리고 Stage 5~7에 큰 좀비를 추가했다. Stage 0을 유지한 채 Stage 7·지도 핀·자동 진행·구 저장 호환을 확장했다.<br />[정리·조사] GrandmaMovement로 입력·자동 탐색을 분리하고 전투 상태 진행을 함수별로 정리했다. GPT Chat에서 Tools 메뉴 15개를 읽기 전용 리뷰했다. Unity 시작 소리의 원인을 조사했다.</td><td>좀비·8개 Stage 검사 222 PASS. 리팩터링 전후 같은 222개 검사 PASS + 장전음 28 PASS = 최종 250개. 보호 파일 597개 중 의도한 GrandmaDefense.cs만 변경, 12개 serialized 선언 유지.<br />시작 소리는 재현되지 않아 원인 미확정. 기존 CS0108 경고·배치 Editor Search 오류는 별도 기록. 보스 코드 삭제는 수행하지 않았다.</td><td>C4 · C13 · C14 · L16 · L17 · L18</td></tr>
<tr><td><b>D15</b></td><td><date start="2026-11-01" /></td><td>[일지] GPT Chat의 CJB_게임만들기 프로젝트 대화, Codex 작업 대화·구현/아트/검증 문서와 Git 이력을 교차 확인해 이 일지를 작성했다.</td><td>새 게임 기능 구현일로 집계하지 않는다. 시작일·날짜별 진척·검증 상태·기획 변경·기록 공백을 하나의 문서로 정리했다.</td><td>이 문서 · 출처 목록</td></tr>
</table>

<toggle>
근거 자료와 기록 범위

<b>Notion 기획·스터디</b>

- <b>N0</b> · <mention url="https://app.notion.com/p/4037313398e081cf87e2f8f247f2a942">[기획서_00] f/o Desktop Companion 기획서</mention>
- <b>N1</b> · <mention url="https://app.notion.com/p/4037313398e0818d99eef7aca3563cf8">[아트레퍼런스_00] 샷건 8종 — 작성일 2026-10-18</mention>
- <b>N2</b> · <mention url="https://app.notion.com/p/4037313398e08151a1d1f38c7dea89bf">[아트레퍼런스_01] 탄환 발전사 — 작성일 2026-10-18</mention>
- <b>N3</b> · <mention url="https://app.notion.com/p/4037313398e081d0ba8addb38d803d6d">Project10_Hunter_grandma · 복사본</mention>
- <b>N4</b> · <mention url="https://app.notion.com/p/4047313398e0818db924d95c5c455419">[기획서_01] 게임 루프·전투·포획</mention>
- <b>N5</b> · <mention url="https://app.notion.com/p/4067313398e08130965aebafbb62a76d">[기획서_02] 재화·성장·동료 해금</mention>
- <b>N6</b> · <mention url="https://app.notion.com/p/4067313398e081fab3d4dedff019e11c">[기획서_03] 스테이지 진행·동료 영입·연출</mention>
- <b>N7</b> · <mention url="https://app.notion.com/p/4067313398e081b5b0d1fe284d521bc3">[기획서_04] 화면·UI·작업 상태</mention>
- <b>N8</b> · <mention url="https://app.notion.com/p/4077313398e0812484dcf9c0e690bdfe">[기획서_05] 좀비 치료 대작전 — 2026-10-28 아이디어 초안</mention>
- <b>R1</b> · <mention url="https://app.notion.com/p/4037313398e081fdbfb7e73a4ad8edf1">[스터디_01] 총기 업그레이드 체계 — 2026-10-20</mention>
- <b>R2</b> · <mention url="https://app.notion.com/p/4047313398e0808a8f24c573dbead63a">[스터디_02] Cat Gunner — 2026-10-24</mention>
- <b>R3</b> · <mention url="https://app.notion.com/p/4057313398e081cb8458c60da4a38f07">[스터디_03] 축적형 성장 — 2026-10-26</mention>
- <b>R4</b> · <mention url="https://app.notion.com/p/4057313398e0810e9d1bcd15cbbcdb5d">[스터디_04] Duelist 성장·진행 — 2026-10-26</mention>
- <b>R7</b> · <mention url="https://app.notion.com/p/4067313398e081fabe70ea585a7f04de">[스터디_07] 엔딩 연출 — 2026-10-27</mention>
- <b>R8</b> · <mention url="https://app.notion.com/p/4077313398e081df809bd64d3da1b26c">[스터디_08] Duelist 화면/UI — 분석 기준 2026-10-28</mention>
- <b>R9</b> · <mention url="https://app.notion.com/p/4077313398e08106aad7f6d7dc94e3c3">[스터디_09] 데이브 더 다이버 에셋 실측 — 2026-10-29</mention>
- <b>R10</b> · <mention url="https://app.notion.com/p/4087313398e08196b82fc91de582863a">[스터디_10] The Art of Screenshake — 기록일 2026-10-30</mention>

<b>GPT Chat·Codex 대화</b>

- <b>C1</b> · [ChatGPT 「막작업 선택 ToDos」](https://chatgpt.com/c/6aa00814-1284-83ee-907e-7a5a212e2081) — 10/18 협업 검토와 10/21 개발 순서 논의
- <b>C2</b> · Codex 「Desktop Companion 기획서 작성」 · 채팅 ID <code>01a15719-281b-79a3-b9ec-ef893ff5b77a</code> — 10/20 초기 기획, 10/21 레퍼런스, 10/24 프로젝트 복사
- <b>C3</b> · Codex 「Aseprite 할머니 스프라이트 제작」 · 채팅 ID <code>01a15c15-5109-7633-b8f0-db0d03cf28d8</code> — 10/21 베이스·후보 수정
- <b>C4</b> · Codex 「(개발)할머니가 좀비를 쏘게 만들기」 · 채팅 ID <code>01a17106-29ea-7f01-97f8-917e038e1587</code> — 10/25~10/31 전투·지도·진행·음원·좀비 통합; 3페이지 조회
- <b>C5</b> · [ChatGPT 「방치형 게임 시장 분석」](https://chatgpt.com/c/6adab2c6-e0d0-83e8-962e-5c3b291de975) — 10/25 Stage v2·보스·반복 포획 기획
- <b>C6</b> · [ChatGPT 「게임 시스템 레퍼런스」](https://chatgpt.com/c/6adf0fc2-3e4c-83ee-9e71-e8a5f90dc4ce) — 10/26 파워 그릇·재화·업그레이드 문서
- <b>C7</b> · [ChatGPT 「무기 업그레이드 방식 비교」](https://chatgpt.com/c/6ade91e3-7118-83e8-89f3-04ce8b7df479) — 10/26 스탯, 10/28 기믹과 데미지 충돌 검토
- <b>C8</b> · [ChatGPT 「기획서 작성」](https://chatgpt.com/c/6ad397de-79e0-83ec-992a-b05b07eddfe6) — 10/27 UI·(미정)재화·Chat/Codex 상태 관리
- <b>C9</b> · Codex 「Fix 잡다한 작업 UI 폰트 깨짐」 · 채팅 ID <code>01a17e1a-9dc1-71f3-b5c4-327bbe7de8ad</code> — 10/28 폰트·UI 리팩터링·초기 Git 푸시
- <b>C10</b> · Codex 「Notion 기획서 01~04 리팩터링」 · 채팅 ID <code>01a18293-44be-7ff3-ae79-6908642f3757</code> — 10/28 역할 정리, 10/30 보스 취소선 반영
- <b>C11</b> · Codex 「(개발) 업그레이드 기능」 · 채팅 ID <code>01a18672-49e8-7e51-b6e2-74846394bff6</code> — 10/29 스킬·Game View, 10/30 스킬 시연 조사
- <b>C12</b> · Codex 「주인공 할매 이미지 소스 확인」 · 채팅 ID <code>01a17ddd-b07b-7932-bc5f-24664771d242</code> — 10/30 귀여운 좀비·전신 슬라임·얼굴/Dead 수정. 큰 턴 읽기 제한으로 앞선 제작분은 L10/L15로 보완
- <b>C13</b> · [ChatGPT 「막작업2 작업 요청」](https://chatgpt.com/c/6ae16c42-25d0-83ee-9013-06129a89cdd3) — 10/31 GitHub 연결 및 Tools 15개 코드 리뷰
- <b>C14</b> · Codex 「(개발)할머니가 좀비를 쏘게 만들기 2」 · 채팅 ID <code>01a18bc0-0d2f-7c61-8d68-bdff39751e72</code> — 이 채팅의 10/31 전투 리팩터링과 보스 코드 보존 확인
- <b>C15</b> · [ChatGPT 「시점 비교 추천」](https://chatgpt.com/c/6adff096-d1ac-83ec-bff6-debed24afdf1) — 10/29 스킬·성장 제안, 10/30 단일 Stage·조건부 조우/영입 확정

<b>로컬 문서</b>

루트: <code>E:/unity_project/project10_hunter_grandma/Hunter_grandma/</code>. 아래 상대 경로는 이 루트 기준이다.

- <b>L1</b> · <code>Assets/Artsource/Guides/README.md · Assets/Artsource/Drafts/GrandmaShooter64/v001~v003/README.md</code> — 가이드와 64px 후보·노년 수정·역할별 아트
- <b>L2</b> · <code>Docs/Validation/CombatSmokeTest.md</code> — 최초 자동 전투 16개 검사, 60초 실행
- <b>L3</b> · <code>Docs/Validation/MapSelectionSmokeTest.md</code> — 2026-10-25 지도 UI 검사
- <b>L4</b> · <code>Docs/StageV2-Implementation.md · Docs/Validation/StageV2-validation.md</code> — 2026-10-26 Stage v2, 115 PASS
- <b>L5</b> · <code>Docs/BeltScroll-Implementation.md · Docs/Validation/BeltScroll-validation.md</code> — 벨트스크롤 구현. 검증 파일은 이후 재실행으로 갱신될 수 있어 당시 수치는 C4도 대조
- <b>L6</b> · <code>Docs/UI-P0-Implementation.md</code> — 2026-10-27 접이식 강화·임시 재화·HUD
- <b>L7</b> · <code>Docs/Stage0-Opening-Implementation.md · Docs/Validation/Stage0-opening-validation.md</code> — 차 도입·울타리·좀비 공격
- <b>L8</b> · <code>Docs/Validation/UI-font-fix-20261028.md · Docs/Validation/UI-refactor-20261028.md</code> — 한글 폰트·공통 UI, 119 PASS
- <b>L9</b> · <code>Docs/UpgradeConnection-Implementation.md · Docs/Validation/UpgradeConnection-validation.md</code> — 부분 검증에서 103 PASS로 이어진 연결창·5명 카드
- <b>L10</b> · <code>Assets/Artsource/Drafts/GrandmaShooter120/v001/README.md · v002/README.md · Docs/Sundeok120-Animation-Integration.md</code> — 순덕 120px 원본·재제작·6동작 게임 통합
- <b>L11</b> · <code>Docs/ShadowSkill-Implementation.md · Docs/Validation/ShadowSkill-summary.md</code> — 밤마실 스킬 및 385 PASS 구성
- <b>L12</b> · <code>Docs/Validation/GameView-display-repair-20261029.md</code> — 실제 Game View 비율 복구
- <b>L13</b> · <code>Docs/BossFree-StageProgression.md · Docs/Validation/BossFree-stage-progression-validation.md</code> — 보스 제외·다음 Stage 자동 진행 169 PASS
- <b>L14</b> · <code>Docs/Sundeok-Reload-Audio.md · Docs/Validation/ReloadAudio-validation.md · Docs/Validation/SkillInvestigation-20261030.md</code> — 장전음 28 PASS와 자연 전투/스킬 시연 조사
- <b>L15</b> · <code>Assets/Artsource/Drafts/GrandmaShooter120/v002/Motorcycle/README.md · Monsters_Cute/README.md</code> — 10/30 오토바이 18프레임·좀비 3종 84프레임
- <b>L16</b> · <code>Docs/ZombieCute-StageRoster.md · Docs/Validation/ZombieCute-stage-validation.md</code> — 10/31 8개 Stage·좀비 통합 222 PASS
- <b>L17</b> · <code>Docs/Gameplay-Refactoring.md · Docs/Validation/GameplayRefactor-validation.md</code> — 10/31 이동 분리·상태 정리 250 PASS
- <b>L18</b> · <code>Docs/Validation/StartupAudio-investigation-20261031.md</code> — 시작 소리 조사; 원인 미확정

<b>Git 이력</b>

- <b>G1</b> · [c4d0cb4](https://github.com/jb-choi/hunter_grandma/commit/c4d0cb4) — 2026-10-28 최초 프로젝트 커밋.
- <b>G2</b> · [2b23f67](https://github.com/jb-choi/hunter_grandma/commit/2b23f67) — 2026-10-30 순덕 애니메이션·강화·스킬 커밋. 실제 제작/검증은 앞선 날짜이며 커밋일과 구분한다.
- <b>G3</b> · [3fbebee](https://github.com/jb-choi/hunter_grandma/commit/3fbebee) — 2026-10-30 보스 없는 자동 진행.
- <b>G4</b> · [c824084](https://github.com/jb-choi/hunter_grandma/commit/c824084) — 2026-10-30 장전음 동기화.

<b>조사 범위의 한계:</b> 연결된 현재 Notion 복사본과 조회 가능한 ChatGPT/Codex 대화가 기준이다. 과거 원본 Notion 프로젝트는 현재 연결에서 접근되지 않아 직접 확인 자료로 사용하지 않았다. 이미지 소스 Codex 대화의 큰 턴 일부는 도구 읽기 제한이 있어 제작 README·QA 문서로 보완했다. project09의 선행 동물 사냥 연구는 Hunter Grandma 고유 진척과 섞지 않았다. 이 일지는 새 기능을 검증한 보고서가 아니라 당시 작업·검증 기록의 정리다.
</toggle>

다음 작업은 같은 문서의 표 마지막 행 아래에 이어 적는다. 2026-11-02는 D16이며, 완료되지 않은 계획은 결과 칸에 ‘기획/진행 중/미검증’으로 구분한다.

