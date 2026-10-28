using UnityEngine;

namespace HunterGrandma
{
    public enum DefensePhase { Waves, WaveBreak, BossArrival, BossFight, Subdued, LoadingCart, Returning, Clear, Failed, Retreated, Opening }
    public enum BossPattern { BarkDash, BoxCharge, CartRush, GuardSlam, SpinDance, Leap, HealSummon }

    public sealed class DefenseStage
    {
        public readonly int Id;
        public readonly string Name, BossType, BossName, WaveDescription;
        public readonly int WaveCount, EnemiesPerWave, ZombieHealth, BossHealth, GoldPerKill;
        public readonly float SpawnInterval, ZombieSpeed;
        public readonly BossPattern Pattern;
        public DefenseStage(int id, string name, string type, string boss, string waves, int count, int enemies, int hp, int bossHp, float interval, float speed, BossPattern pattern)
        {
            Id = id; Name = name; BossType = type; BossName = boss; WaveDescription = waves;
            WaveCount = count; EnemiesPerWave = enemies; ZombieHealth = hp; BossHealth = bossHp;
            SpawnInterval = interval; ZombieSpeed = speed; Pattern = pattern; GoldPerKill = 3 + id;
        }
    }

    public static class DefenseStages
    {
        public const int Count = 7;
        private static readonly DefenseStage[] stages =
        {
            new DefenseStage(0, "할머니 집", "neighborhood_dog", "동네 강아지 · 왕왕이형", "짧은 단방향 습격", 2, 7, 10, 85, 1.7f, 1.2f, BossPattern.BarkDash),
            new DefenseStage(1, "북서 주거지", "delivery", "택배 기사 · 박스맨형", "느린 다수형 군중", 3, 10, 20, 150, 1.35f, 1.05f, BossPattern.BoxCharge),
            new DefenseStage(2, "북동 상가", "salesperson", "마트 점원 · 카트맨형", "빠른 골목 무리", 3, 10, 22, 190, 1.25f, 1.7f, BossPattern.CartRush),
            new DefenseStage(3, "서부 창고", "worker", "현장 작업자 · 안전모형", "거구와 소형의 혼합", 4, 9, 26, 240, 1.4f, 1.25f, BossPattern.GuardSlam),
            new DefenseStage(4, "중앙 광장", "dancer", "거리 댄서 · 브레이커형", "좌우에서 다가오는 군집", 4, 9, 26, 260, 1.35f, 1.35f, BossPattern.SpinDance),
            new DefenseStage(5, "남서 주거지", "trainer", "헬스 트레이너 · 벌크업형", "밀집 무리 뒤 고속형", 4, 11, 32, 300, 1.2f, 1.4f, BossPattern.Leap),
            new DefenseStage(6, "남동 병원", "doctor", "의사 · 닥터형", "여러 유형의 복합 웨이브", 4, 12, 34, 340, 1.25f, 1.45f, BossPattern.HealSummon)
        };
        public static bool IsValid(int id) => id >= 0 && id < Count;
        public static DefenseStage Get(int id) => stages[Mathf.Clamp(id, 0, Count - 1)];
    }
}
