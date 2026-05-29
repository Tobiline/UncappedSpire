using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Screens;

namespace UncappedSpire.UncappedSpireCode.UncappedUpgrades.CardsViewScreenPatches;

[HarmonyPatch(typeof(NCardsViewScreen), "ToggleShowUpgrades")]
public class Patch_ToggleShowUpgrades
{
    [HarmonyPrefix]
    public static void Prefix(NCardsViewScreen __instance)
    {
        UpgradeContext.EnableMultiplier();
    }
    
    [HarmonyFinalizer]
    public static void Finalizer(NCardsViewScreen __instance)
    {
        UpgradeContext.DisableMultiplier();
    }
}