# Hunter64·산 원경 Unity 적용

사용자 요청: `ArtSource/Try/Hunter64`를 기준으로 사냥꾼 애니메이션을 교체하고, 나무 뒤 상단 배경을 원경 이미지로 변경한다.
Notion 작업/결과: https://app.notion.com/p/3faf3731816f814180e1d85f0a6cac16

## 적용

- 기존 코드 생성 uGUI 구조에 `HuntArtSet.hunterAnimations`를 연결했다. PNG 41개는 전부 64×64, RGBA, Point, 무압축, MipMap off이며 프레임 여백을 자르지 않았다.
- idle_breath 5F, walk_retry/walk 태그 8F, focus 4F, shoot 4F, collect 4F, carry_small 6F, drag_large 6F, load 4F. 원래 duration을 사용하고 단발 동작은 마지막 자세를 유지한다. 발사·들기·내려놓기·적재는 해당 게임 상태의 진행률에 맞춘다.
- 탐색/접근/기존 수거 이동=walk, 동물 등장/기절 대기/방사=idle, 조준=focus, 발사=shoot, 들어 올리기/깨우기/툭툭=collect, 탈탈/기존 운반=carry_small 또는 drag_large, 기존 적재=load.
- 시뮬레이션 상태 시간을 사용하므로 일시정지 때 프레임도 멈춘다. 이동 반전은 기존 방향을 따른다. 시뮬레이션 보상·세이브 형식은 이번 변경에서 건드리지 않았다.
- 캐릭터의 발 아래 빈 7행을 보정하고, 서 있는 50px 작화를 기존 표시 높이 42에 맞췄다. 동물별 크기 비율을 유지한다. 그림에 포함된 새총과 중복되던 코드 무기는 숨기고 돌 발사 위치를 새 그림에 맞췄다.
- `MountainSkyLoop/v004/Export/bg_mountain_sky.png`를 그대로 전달해 `HuntArtSet.farLandscape`에 연결했다. 앞쪽 나무·지면은 유지하며 원경은 비율을 보존하고 화면 밖을 잘라 채운다. 메뉴는 계속 overlay 방식이다.

## 입력 보존·제한

디자인을 새로 생성하거나 원본 픽셀을 수정하지 않았다. `idle_breath_v2`의 별도 다리 디자인, 미완료 rework, 색으로 부위를 나눈 focus_rework2, 빈 중간 프레임이 있는 기존 walk는 연결하지 않았다. walk_retry의 cycle_close F9도 반복에서 제외한다.

운반/견인/적재는 예전 자세다. 임시 export 복사본에서만 Bow·Quiver를 숨기고 DemoProps를 제외했다. 이 동작들에는 최신 새총·가방 작화가 없으며, 탈탈 털기·깨우기·툭툭 전용 프레임도 없어 제공된 동작과 기존 흔들기/회전을 재사용한다. 동물은 이번 요청의 Hunter64 범위에 따라 기존 base sprite 연출을 유지한다.

## QA와 해결한 문제

- EditMode 92/92 통과. 비균일 duration, 반복/마지막 유지, 일시정지, 상태별 동작과 큰 동물 분기를 확인했다.
- 41개 출력 프레임 모두 독립 Aseprite 기본 sprite-sheet 출력의 해당 프레임과 RGBA가 정확히 일치한다. 팔레트 5색 초과·알파 0/255·64×64를 확인했다. 원본 아트 124파일 해시가 같고 QA 중 사용자 세이브도 바이트 단위로 보존됐다.
- 640×360, 960×540, 1280×720 렌더, 걷기/조준/발사/들기/털기/방사, 멧돼지와 작은 동물, 메뉴 접힘/지도 overlay를 검사했다. 표시 높이42, 중복 무기 숨김, 원경 연결, 메뉴 전환 전후 stage rect 동일을 확인했다.
- 실패 원인: MCP의 개별 PNG export가 Indexed 팔레트를 잃어 검정 실루엣을 만들었다. 실제 화면 검수에서 발견하고 Aseprite 기본 CLI의 RGB export로 전41F를 교체했다. 색이 있는 native 시트와 별도 픽셀 비교로 재검증했다.
- 설정 적용과 빌드는 Pipeline의 5초 응답 제한보다 오래 걸려 응답 timeout이 날 수 있다. 실제 생성 asset/완료 로그·빌드 결과 파일을 확인하고 중복 실행하지 않았다.
- QA 시 Game View 크기 변경 직후 한 캡처가 이전 임시 화면 크기를 사용했다. 갱신 후 실제 Screen.width/height가 640×360인 것을 확인해 해당 스크린샷을 다시 만들었다.

검증 출력은 로컬 전용 `Docs/AI/Validation/Hunter64-*`에 보관한다. 재연결 메뉴는 `Hunting Idle > Apply Hunter64 and Mountain Panorama`이다. 원본 재출력과 이 메뉴의 import/연결은 별도 단계다.

Windows 개발 빌드 `Builds/Windows-Cycle21/HuntingIdle.exe` 생성 성공. 첫 빌드26.3초/증분6.4초, 보고의 errors1은 빌드 도중 CLI main-thread timeout을 Unity가 기록한 항목이다. 진단에 C# 컴파일 오류는 없다. 기존 Pipeline RuntimeConfig 미설정과 Unity Cloud 네이티브 심볼 업로드 생략 경고2가 있다. 별도 Player 검증은 `Hunter64-player.json`에서 passed/logicPassed/offscreenVerified/resourcesRestored 모두 true, errors0, 60FPS 목표를 확인했다. 숨긴 OS 창의 backbuffer는 검증하지 못했고 실제 UI를 소유 RenderTexture로 렌더해 검증했다. 기존 2D Animation 종료 ComputeBuffer 경고는 남아 있다. 배포 폴더에 기존 다운로드 이미지의 출처/라이선스 파일을 동봉했다.

다음 아트 작업: 새총·가방이 이어지는 운반/견인 자세, 실제 동물 접점을 고려한 털기·깨우기·툭툭 전용 프레임, 종별 동물 애니메이션.
