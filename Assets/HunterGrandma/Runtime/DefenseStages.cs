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
        public readonly int InfectedSlots, LargeSlots;
        public readonly BossPattern Pattern;
        public DefenseStage(int id, string name, string type, string boss, string waves, int count, int enemies, int hp, int bossHp, float interval, float speed, BossPattern pattern, int infectedSlots = 0, int largeSlots = 0)
        {
            Id = id; Name = name; BossType = type; BossName = boss; WaveDescription = waves;
            WaveCount = count; EnemiesPerWave = enemies; ZombieHealth = hp; BossHealth = bossHp;
            SpawnInterval = interval; ZombieSpeed = speed; Pattern = pattern; GoldPerKill = 3 + id;
            InfectedSlots = infectedSlots; LargeSlots = largeSlots;
        }
        public ZombieKind EnemyKind(int spawnIndex, int wave)
        {
            int slot = (spawnIndex * 7 + (wave - 1) * 3) % (Id == 0 ? 20 : 30);
            if (slot >= 20) return Id >= 2 && slot >= 25 ? ZombieKind.PartyHorn : ZombieKind.PartyBalloon;
            return slot < LargeSlots ? ZombieKind.Large : slot < LargeSlots + InfectedSlots ? ZombieKind.Infected : ZombieKind.Normal;
        }
    }

    public static class DefenseStages
    {
        public const int Count = 8;
        private static readonly DefenseStage[] stages =
        {
            new DefenseStage(0, "할머니 집", "neighborhood_dog", "동네 강아지 · 왕왕이형", "울타리를 두드리는 감염체", 2, 7, 10, 85, 1.7f, 1.2f, BossPattern.BarkDash, 20),
            new DefenseStage(1, "북서 주거지", "delivery", "택배 기사 · 박스맨형", "일반·감염체 + Z1 풍선 파티 좀비", 3, 12, 20, 150, 1.05f, 1.05f, BossPattern.BoxCharge, 6),
            new DefenseStage(2, "북동 상가", "salesperson", "마트 점원 · 카트맨형", "일반·감염체 + Z1·Z2 파티 좀비", 3, 14, 22, 190, .95f, 1.7f, BossPattern.CartRush, 7),
            new DefenseStage(3, "서부 창고", "worker", "현장 작업자 · 안전모형", "밀집한 일반·감염체·파티 무리", 4, 15, 26, 240, .90f, 1.25f, BossPattern.GuardSlam, 8),
            new DefenseStage(4, "중앙 광장", "dancer", "거리 댄서 · 브레이커형", "양쪽에서 몰려오는 좀비·파티 무리", 4, 16, 26, 260, .85f, 1.35f, BossPattern.SpinDance, 10),
            new DefenseStage(5, "남서 주거지", "trainer", "헬스 트레이너 · 벌크업형", "일반·감염체·파티 + 큰 좀비", 4, 18, 32, 300, .80f, 1.4f, BossPattern.Leap, 8, 4),
            new DefenseStage(6, "남동 병원", "doctor", "의사 · 닥터형", "큰 좀비와 파티 좀비의 복합 웨이브", 4, 20, 34, 340, .75f, 1.45f, BossPattern.HealSummon, 9, 5),
            new DefenseStage(7, "동부 외곽", "outskirts", "", "다섯 유형이 몰려오는 마지막 구역", 4, 22, 38, 0, .70f, 1.5f, BossPattern.GuardSlam, 10, 6)
        };
        public static bool IsValid(int id) => id >= 0 && id < Count;
        public static DefenseStage Get(int id) => stages[Mathf.Clamp(id, 0, Count - 1)];
    }
}
