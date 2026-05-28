using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace UncappedSpire.UncappedSpireCode.LimitlessUpgrades.CardEnergyCostPatches;

[HarmonyPatch(typeof(CardEnergyCost), "UpgradeBy")]
public class Patch_UpgradeBy
{
    [HarmonyPrefix]
    static void Prefix(CardEnergyCost __instance, ref int addend)
    {
        addend *= UpgradeContext.GetMultiplier();
    }
}