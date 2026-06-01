using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;

namespace UncappedSpire.UncappedSpireCode.UncappedUpgrades.CardEnergyCostPatches;

[HarmonyPatch(typeof(CardEnergyCost), "UpgradeBy")]
public class Patch_UpgradeBy
{
    private static readonly FieldInfo getBase = AccessTools.Field(typeof(CardEnergyCost), "_base");
    private static readonly FieldInfo getOwner = AccessTools.Field(typeof(CardEnergyCost), "_card");
    
    [HarmonyPrefix]
    static void Prefix(CardEnergyCost __instance, ref int addend)
    {
        var card = (CardModel?)getOwner.GetValue(__instance);
        if (card != null)
        {
            getBase.SetValue(__instance, __instance.Canonical);
            addend *= card.CurrentUpgradeLevel;
        }
    }
}