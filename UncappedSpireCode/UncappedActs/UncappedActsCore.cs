using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using UncappedSpire.UncappedSpireCode.Config;
using UncappedSpire.UncappedSpireCode.UncappedActs.RunManagerPatches;

namespace UncappedSpire.UncappedSpireCode.UncappedActs;

public static class UncappedActsCore
{
    public static readonly MethodInfo Method_get_State = AccessTools.PropertyGetter(typeof(RunManager), "State");
    public static readonly FieldInfo Field__mapPointHistory = AccessTools.Field(typeof(RunState), "_mapPointHistory");
    public static readonly FieldInfo Field__rooms = AccessTools.Field(typeof(ActModel), "_rooms");
    
    public static async Task EnterNextChapter()
    {
        var state = (RunState?)Method_get_State.Invoke(RunManager.Instance, null);
        if (state == null)
        {
            return;
        }
        
        // TODO: Save the old run history somewhere to display???
        Field__mapPointHistory.SetValue(state, new List<List<MapPointHistoryEntry>>());
                
        if (RunManager.Instance.NetService.Type is NetGameType.Host or NetGameType.Singleplayer)
        {
            var chapterChangeSynchronizer = SpireFields_RunManager.ChapterChangeSynchronizer.Get(RunManager.Instance)!;
            _ = chapterChangeSynchronizer.DoLocalSeedChange(SeedHelper.GetRandomSeed());
        }
                
        var uncappedActsModifier = state.Modifiers.First(m => m is UncappedSpireModifier) as UncappedSpireModifier;
        uncappedActsModifier!.CurrentChapter++;

        TaskHelper.RunSafely(RunManager.Instance.EnterAct(0));
    }

    public static void AddFinalBossRewards(RewardsSet rewardsSet)
    {
        var rooms = (RoomSet)Field__rooms.GetValue(rewardsSet.Player.RunState.Act)!;
        if (ContextManager.UncappedActsEnabled && rooms.NextBossEncounter.Id == rewardsSet.Player.RunState.CurrentRoom!.ModelId)
        {
            var ancientRelics = ModelDb.AllRelics.Where(r => r.Rarity == RelicRarity.Ancient);
            var shuffledAncientRelics = ancientRelics.ToList().UnstableShuffle(rewardsSet.Player.RunState.Rng.UpFront);
        
            var rareCardRewardOptions = new CardCreationOptions(
                [rewardsSet.Player.Character.CardPool],
                CardCreationSource.Encounter,
                CardRarityOddsType.BossEncounter);
        
            rewardsSet.Rewards.AddRange([
                new GoldReward(100, 300, rewardsSet.Player),
                new GoldReward(100, 200, rewardsSet.Player),
                new RelicReward(shuffledAncientRelics[0].ToMutable(), rewardsSet.Player),
                new RelicReward(shuffledAncientRelics[1].ToMutable(), rewardsSet.Player),
                new CardReward(rareCardRewardOptions, 3, rewardsSet.Player),
                new CardReward(rareCardRewardOptions, 3, rewardsSet.Player),
            ]);
        }
    }
}