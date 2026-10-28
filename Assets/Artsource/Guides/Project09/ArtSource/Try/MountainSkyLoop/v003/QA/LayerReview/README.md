# 320×180 원본 visibility 검수

- `States/only_*.aseprite`: 320×180에서 한 레이어만 visible.
- `States/hidden_*.aseprite`: 320×180에서 한 레이어만 invisible.
- `Solo/*_native320.png`: 실제 320×180 단독 PNG.
- `Solo/*_checker.png`: 2배 표시 비교.
- `Solo/*_4x.png`, `Crops/*_6x.png`: 최근접 확대.
- `Repeats/*_3x.png`: 가로 3회 반복.
- [4개 레이어 보기](layers_contact.png), [작화 판정](../visual_review.md).

Aseprite 표시 속성을 실제로 전환하고 다시 열어 확인했다. Shift+X 키를 직접 누르지는 않았다. 네이티브 레이어와 단독 PNG가 동일하며, 640×360 출력은 정확한 2배 확대다.
