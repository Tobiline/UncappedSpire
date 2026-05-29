using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Cards;

namespace UncappedSpire.UncappedSpireCode.UncappedUpgrades.CardGridPatches;

[HarmonyPatch(typeof(NCardGrid), "AllocateCardHolders")]
public class Patch_AllocateCardHolders
{
    [HarmonyPrefix]
    public static void Prefix(NCardGrid __instance)
    {
        UpgradeContext.EnableMultiplier();
    }
    
    [HarmonyFinalizer]
    public static void Finalizer(NCardGrid __instance)
    {
        UpgradeContext.DisableMultiplier();
    }
}