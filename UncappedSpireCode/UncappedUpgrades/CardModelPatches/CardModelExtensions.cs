using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;

namespace UncappedSpire.UncappedSpireCode.UncappedUpgrades.CardModelPatches;

public static class CardModelExtensions
{
    private static readonly MethodInfo GetCurrentUpgradeLevel = AccessTools.PropertyGetter(typeof(CardModel), "CurrentUpgradeLevel");
    private static readonly MethodInfo SetCurrentUpgradeLevel = AccessTools.PropertySetter(typeof(CardModel), "CurrentUpgradeLevel");
    private static readonly MethodInfo OnUpgrade = AccessTools.Method(typeof(CardModel), "OnUpgrade");
    private static readonly FieldInfo Upgraded = AccessTools.Field(typeof(CardModel), "Upgraded");
    
    public static void UpgradeInternal(this CardModel card, int levelsToUpgrade)
    {
        MainFile.Logger.Info("Upgrading " + card.Title + " to " + levelsToUpgrade);
        card.AssertMutable();
        SetCurrentUpgradeLevel.Invoke(card, [(int)GetCurrentUpgradeLevel.Invoke(card, [])! + levelsToUpgrade]);
        OnUpgrade.Invoke(card, []);
        card.DynamicVars.RecalculateForUpgradeOrEnchant();
        ((Delegate?)Upgraded.GetValue(card))?.DynamicInvoke();
    }
}