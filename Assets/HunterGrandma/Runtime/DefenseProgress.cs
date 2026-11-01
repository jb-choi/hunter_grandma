using System;
using System.Collections.Generic;
using UnityEngine;

namespace HunterGrandma
{
    [Serializable] public sealed class DistrictProgress
    {
        public int stageId, clearCount, capturedCount;
        public string firstClearAt;
    }
    [Serializable] public sealed class DefenseCaptive
    {
        public string captiveId, bossTypeId, sourceRunId, appearance, capturedAt;
        public int originStageId;
        public bool returnedToYard;
    }
    [Serializable] public sealed class DefenseSaveData
    {
        public int version = 1;
        public int gold, attackLevel, fireRateLevel;
        public bool shadowSkillEnabled;
        public List<DistrictProgress> stages = new List<DistrictProgress>();
        public List<DefenseCaptive> captives = new List<DefenseCaptive>();
    }
    public sealed class DefenseRun
    {
        public readonly string Id = Guid.NewGuid().ToString("N");
        public readonly int StageId;
        public int EarnedGold { get; private set; }
        public bool BossSubdued { get; private set; }
        public bool ReturnedToYard { get; private set; }
        public bool WavesCleared { get; private set; }
        public bool Abandoned { get; private set; }
        public bool Settled { get; private set; }
        public DefenseRun(int stageId) { StageId = stageId; }
        public void AddGold(int gold) { if (!Settled && !Abandoned) EarnedGold += Math.Max(0, gold); }
        public void SubdueBoss() { if (!Abandoned) BossSubdued = true; }
        public void ReturnToYard() { if (BossSubdued && !Abandoned) ReturnedToYard = true; }
        public void CompleteWaves() { if (!Settled && !Abandoned) WavesCleared = true; }
        public void Abandon() { if (!Settled) Abandoned = true; }
        internal void Settle() { Settled = true; }
    }

    // Persistent rewards belong here; clearing waves does not fabricate a captive.
    public sealed class DefenseProgress
    {
        private readonly DefenseSaveData data;
        public int Gold => data.gold;
        public int AttackLevel => data.attackLevel;
        public int FireRateLevel => data.fireRateLevel;
        public bool ShadowSkillEnabled => data.shadowSkillEnabled;
        public void SetShadowSkillEnabled(bool enabled) { data.shadowSkillEnabled = enabled; }
        public int Damage => DamageAtLevel(AttackLevel);
        public float FireInterval => IntervalAtLevel(FireRateLevel);
        public int NextDamage => DamageAtLevel(Math.Min(30, AttackLevel + 1));
        public float NextFireInterval => IntervalAtLevel(Math.Min(15, FireRateLevel + 1));
        private static int DamageAtLevel(int level) => 10 + level * 4;
        private static float IntervalAtLevel(int level) => Mathf.Max(0.18f, 0.72f / (1f + level * .14f));
        public int AttackCost => 18 + AttackLevel * 18;
        public int FireRateCost => 22 + FireRateLevel * 22;
        public int CaptiveCount => data.captives.Count;
        public IReadOnlyList<DefenseCaptive> Captives => data.captives;

        public DefenseProgress(DefenseSaveData source = null)
        {
            data = source ?? new DefenseSaveData();
            // Stage 7 extends the campaign without resetting an existing v1 profile.
            for (int i = data.stages.Count; i < DefenseStages.Count; i++) data.stages.Add(new DistrictProgress { stageId = i });
        }
        public DistrictProgress Stage(int id) => data.stages[id];
        public bool IsUnlocked(int id) => DefenseStages.IsValid(id) && (id == 0 || Stage(id - 1).clearCount > 0);
        public void AddGold(int amount) { data.gold = (int)Math.Min(int.MaxValue, (long)data.gold + Math.Max(0, amount)); }
        public bool TryUpgradeAttack()
        {
            if (AttackLevel >= 30 || Gold < AttackCost) return false;
            data.gold -= AttackCost; data.attackLevel++; return true;
        }
        public bool TryUpgradeFireRate()
        {
            if (FireRateLevel >= 15 || Gold < FireRateCost) return false;
            data.gold -= FireRateCost; data.fireRateLevel++; return true;
        }
        public bool TryCompleteWaves(DefenseRun run)
        {
            if (run == null || !IsUnlocked(run.StageId) || run.Abandoned || run.Settled || !run.WavesCleared) return false;
            RecordClear(run.StageId, DateTime.UtcNow.ToString("O"));
            run.Settle();
            return true;
        }
        private void RecordClear(int stageId, string clearedAt)
        {
            DistrictProgress district = Stage(stageId);
            if (district.clearCount == 0) district.firstClearAt = clearedAt;
            district.clearCount = (int)Math.Min(int.MaxValue, (long)district.clearCount + 1);
        }
        public bool TryComplete(DefenseRun run, string appearance, out DefenseCaptive captive)
        {
            captive = null;
            if (run == null || !IsUnlocked(run.StageId) || run.Abandoned || run.Settled || !run.BossSubdued || !run.ReturnedToYard) return false;
            foreach (DefenseCaptive existing in data.captives) if (existing.sourceRunId == run.Id) return false;
            DefenseStage definition = DefenseStages.Get(run.StageId);
            captive = new DefenseCaptive
            {
                captiveId = Guid.NewGuid().ToString("N"), bossTypeId = definition.BossType,
                originStageId = run.StageId, sourceRunId = run.Id, appearance = appearance,
                returnedToYard = true, capturedAt = DateTime.UtcNow.ToString("O")
            };
            data.captives.Add(captive);
            DistrictProgress district = Stage(run.StageId);
            RecordClear(run.StageId, captive.capturedAt);
            district.capturedCount++;
            run.Settle();
            return true;
        }
        public string ToJson() => JsonUtility.ToJson(data, true);
        public static DefenseProgress FromJson(string json)
        {
            DefenseSaveData loaded = JsonUtility.FromJson<DefenseSaveData>(json);
            if (loaded == null || loaded.version != 1 || loaded.gold < 0 || loaded.attackLevel < 0 || loaded.attackLevel > 30 || loaded.fireRateLevel < 0 || loaded.fireRateLevel > 15 || loaded.stages == null || (loaded.stages.Count != 7 && loaded.stages.Count != DefenseStages.Count) || loaded.captives == null)
                throw new FormatException("Invalid Hunter Grandma save data.");
            if (loaded.stages.Count == 7) loaded.stages.Add(new DistrictProgress { stageId = 7 });
            var ids = new HashSet<string>();
            var runs = new HashSet<string>();
            int[] counts = new int[DefenseStages.Count];
            foreach (DefenseCaptive captive in loaded.captives)
            {
                if (captive == null || !DefenseStages.IsValid(captive.originStageId) || !captive.returnedToYard || string.IsNullOrEmpty(captive.captiveId) || string.IsNullOrEmpty(captive.sourceRunId) || !ids.Add(captive.captiveId) || !runs.Add(captive.sourceRunId) || captive.bossTypeId != DefenseStages.Get(captive.originStageId).BossType)
                    throw new FormatException("Invalid captive record.");
                counts[captive.originStageId]++;
            }
            for (int i = 0; i < DefenseStages.Count; i++)
            {
                DistrictProgress stage = loaded.stages[i];
                if (stage == null || stage.stageId != i || stage.clearCount < counts[i] || stage.capturedCount != counts[i] || (i > 0 && stage.clearCount > 0 && loaded.stages[i - 1].clearCount == 0))
                    throw new FormatException("Invalid stage progression.");
            }
            return new DefenseProgress(loaded);
        }
    }
}
