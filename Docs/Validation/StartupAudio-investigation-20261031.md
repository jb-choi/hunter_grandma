# Play 전 프로젝트 시작 효과음 조사

2026-10-31. 사용자는 Unity 프로젝트를 열기만 해도 총/장전/좀비/울타리와 같은 게임 효과음이 들린다고 설명했다. **원인은 아직 확정하지 못했다.** 시작 당시 소리를 재현하거나 재생 호출을 포착하지 못했으므로 게임 코드나 미리듣기 설정을 추정으로 수정하지 않았다.

## 확인한 증거

- 에디터 시작/임포트 경로: GrandmaStageSetup/GrandmaMapSetup은 자동 구성과 임포트를 수행한다. 정지 화면용 StageZeroOpeningView.Create에는 withAudio=false를 전달하며 AudioSource를 생성하지 않는다. Runtime 클래스에 ExecuteAlways/ExecuteInEditMode는 없다.
- 사격음은 GrandmaShooter.PlayOneShot, 장전음은 Reload 프레임 경계의 별도 AudioSource.Play, 울타리/좀비 연출음은 실제 opening/전투 Advance 경로에서 재생한다. 에디터를 열기만 하는 코드에서 이 호출로 들어가는 경로는 찾지 못했다.
- 씬에 저장된 AudioSource는 1개이고 clip 참조는 비어 있다. playOnAwake 직렬화 값은 1이지만 GrandmaShooter.Awake가 false로 설정한다. 이 값만으로 Play 전 효과음을 설명할 수 없다.
- EditorPrefs AutoPlayAudio=0, 저장된 Scene View m_PlayAudio=0/m_AudioPlay=0. 오디오 클립 자동 미리듣기 가설을 현재 설정은 지지하지 않는다. Unity 6.3 문서는 Preview가 Play 밖에서도 재생 가능함을 설명한다: https://docs.unity.com/en-us/engine/6000.3/manual/audio/reference/class-audio-clip
- 실제 project10 Editor PID 1540은 batchmode가 아닌 정상 Editor로 확인했다. PID 45296/45768은 AssetImportWorker이다. 별도 테스트 프로세스라고 오인하지 않았다. PID 1880의 명령 메타데이터는 조회되지 않았다.
- 기본 Windows 출력의 Core Audio 세션을 250ms 간격으로 8번 읽었을 때 재생 중인 Unity 세션/피크는 포착되지 않았다. 시작 순간의 다른 출력 장치나 과거 소리까지 검증한 것은 아니다.
- 최신 좀비 검증의 숨김 Unity는 00:37경 Play Mode에서 효과음을 재생할 수 있는 경로였지만 종료됐다. 정상 Editor는 01:06경 시작됐으므로 이 사실만으로 사용자 증상의 원인이라고 단정할 수 없다.
- Unity MCP에는 연결된 인스턴스가 없었다. 파일로 작성한 임시 읽기 전용 에디터 음원/미리듣기 진단은 현재 Editor가 스크립트를 임포트하지 않아 실행되지 않았다. 임시 코드와 실행 요청은 제거했다. Play 상태를 직접 관찰했다고 주장하지 않는다.

## 다음 확인

소리가 나는 순간의 재생 프로세스와 Unity Play/Preview 상태를 함께 확인해야 한다. 현재의 무음 상태에서의 점검은 증상 재현을 대신하지 않는다. 실제 게임/씬/오디오/전역 Editor 설정 변경과 컴퓨터 사용 기능은 수행하지 않았다.
