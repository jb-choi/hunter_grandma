using UnityEngine;

namespace HunterGrandma
{
    public sealed class DefenseRegion
    {
        public string Name { get; }
        public string Description { get; }
        public Vector2 MapPosition { get; }

        public DefenseRegion(string name, string description, Vector2 mapPosition)
        {
            Name = name;
            Description = description;
            MapPosition = mapPosition;
        }
    }

    public static class DefenseRegions
    {
        public const int Count = DefenseStages.Count - 1;
        private static readonly DefenseRegion[] regions =
        {
            new DefenseRegion("북서 주거지", "도시 북서쪽에 남은 주택가.\n좁은 골목에서 다가오는 좀비를 막아냅니다.", new Vector2(0.30f, 0.79f)),
            new DefenseRegion("북동 상가", "도로 너머 버려진 상업 구역.\n좀비가 끊임없이 밀려오는 길목입니다.", new Vector2(0.76f, 0.79f)),
            new DefenseRegion("서부 창고", "도시 서쪽의 창고와 주차장.\n순덕 할매가 방어선을 지킵니다.", new Vector2(0.24f, 0.58f)),
            new DefenseRegion("중앙 광장", "도시 한가운데의 넓은 광장.\n사방의 길이 모이는 중심 구역입니다.", new Vector2(0.64f, 0.50f)),
            new DefenseRegion("남서 주거지", "외곽 도로에 맞닿은 남쪽 주택가.\n폐허 사이에서 나타나는 좀비를 상대합니다.", new Vector2(0.30f, 0.21f)),
            new DefenseRegion("남동 시설", "도시 남동쪽에 남은 대형 시설.\n이곳에서도 할매의 자동 사격이 이어집니다.", new Vector2(0.76f, 0.21f)),
            new DefenseRegion("동부 외곽", "도시 동쪽 외곽의 마지막 방어 구역.\n일반·감염체·큰 좀비 무리가 몰려옵니다.", new Vector2(0.76f, 0.63f))
        };

        public static bool IsValid(int index) => index >= 0 && index < Count;
        public static DefenseRegion Get(int index) => regions[Mathf.Clamp(index, 0, Count - 1)];
    }
}
