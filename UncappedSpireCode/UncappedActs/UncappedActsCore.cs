using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using UncappedSpire.UncappedSpireCode.Config;
using UncappedSpire.UncappedSpireCode.Util;

namespace UncappedSpire.UncappedSpireCode.UncappedActs;

public static class UncappedActsCore
{
    private static readonly FieldInfo Field__rooms = AccessTools.Field(typeof(ActModel), "_rooms");

    private static HashSet<Type> RelicsToRemove = [
        typeof(FurCoat),
        typeof(GoldenCompass)
    ];
    
    public static void AddFinalBossRewards(RewardsSet rewardsSet)
    {
        var rooms = (RoomSet)Field__rooms.GetValue(rewardsSet.Player.RunState.Act)!;
        if (ContextManager.UncappedActsEnabled && ContextManager.ActThreeBossRewardsEnabled && rooms.NextBossEncounter.Id == rewardsSet.Player.RunState.CurrentRoom!.ModelId)
        {
            var ancientRelics = ModelDb.AllRelics.Where(r => r.Rarity == RelicRarity.Ancient);
            var ancientRelicsList = ancientRelics.ToList();
            ancientRelicsList.RemoveAll(r => RelicsToRemove.Contains(r.GetType()));
            var shuffledAncientRelics = ancientRelicsList.UnstableShuffle(rewardsSet.Player.PlayerRng.Rewards);
        
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

    public static async Task EnterPostActThreeBossRoom()
    {
        var runManager = RunManager.Instance;
        if (ContextManager.UncappedActsEnabled)
        {
            await runManager.EnterRoom(new EventRoom(ModelDb.Event<ClosingTheChapter>()));
        }
        else
        {
            await runManager.EnterRoom(new EventRoom(ModelDb.Event<TheArchitect>()));
        }
    }
}