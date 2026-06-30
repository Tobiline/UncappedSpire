using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;

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
        var isTezcatarasEmber = card?.Rarity == CardRarity.Basic && card.Tags.Contains(CardTag.Strike);
        if (card != null && !isTezcatarasEmber)
        {
            getBase.SetValue(__instance, __instance.Canonical);
            addend *= card.CurrentUpgradeLevel;
        }
    }
}