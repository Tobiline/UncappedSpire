using System.Reflection;
using MegaCrit.Sts2.Core.Models;

namespace UncappedSpire.UncappedSpireCode.LimitlessUpgrades.CardModelPatches;

public static class CardModelExtensions
{
    private static readonly MethodInfo CurrentUpgradeLevelSetter = HarmonyLib.AccessTools.PropertySetter(typeof(CardModel), "CurrentUpgradeLevel");
    private static readonly MethodInfo OnUpgrade = HarmonyLib.AccessTools.Method(typeof(CardModel), "OnUpgrade");
    private static readonly FieldInfo Upgraded = HarmonyLib.AccessTools.Field(typeof(CardModel), "Upgraded");
    
    public static void UpgradeToInternal(this CardModel card, int level)
    {
        card.AssertMutable();
        CurrentUpgradeLevelSetter.Invoke(card, [level]);
        OnUpgrade.Invoke(card, []);
        card.DynamicVars.RecalculateForUpgradeOrEnchant();
        var upgraded = Upgraded.GetValue(card) as Action;
        if (upgraded == null)
            return;
        upgraded();
    }
}