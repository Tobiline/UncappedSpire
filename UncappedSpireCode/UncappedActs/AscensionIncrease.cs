using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Runs;

namespace UncappedSpire.UncappedSpireCode.UncappedActs;

public static class AscensionIncrease
{
    public static void IncrementAscension(Player player)
    {
        var ascensionManager = RunManager.Instance.AscensionManager;
        var levelField = AccessTools.Field(typeof(AscensionManager), "_level");
        var level = (int)levelField.GetValue(ascensionManager)!;
        var maxAscensionAllowedField = AccessTools.Field(typeof(AscensionManager), "maxAscensionAllowed");
        var maxAscensionAllowed = (int)maxAscensionAllowedField.GetValue(ascensionManager)!;
        if (level >= maxAscensionAllowed)
        {
            return;
        }
        
        // Increment State
        var ascensionLevelSetter = AccessTools.PropertySetter(typeof(RunState), nameof(RunState.AscensionLevel));
        ascensionLevelSetter.Invoke(player.RunState, [player.RunState.AscensionLevel + 1]);

        var ascensionManagerSetter = AccessTools.PropertySetter(typeof(RunManager), nameof(RunManager.AscensionManager));
        ascensionManagerSetter.Invoke(RunManager.Instance, [new AscensionManager(player.RunState.AscensionLevel)]);
        
        // Apply Ascension Effects
        ascensionManager = RunManager.Instance.AscensionManager;
        level = (int)levelField.GetValue(ascensionManager)!;
        foreach (var player2 in player.RunState.Players)
        {
            if (level == (int)AscensionLevel.TightBelt)
            {
                player2.SubtractFromMaxPotionCount(1);
            }
            else if (level == (int)AscensionLevel.AscendersBane)
            {
                var ascendersBane = player.RunState.CreateCard<AscendersBane>(player2);
                ascendersBane.FloorAddedToDeck = 1;
                player2.Deck.AddInternal(ascendersBane);
            }
        }
    }
}