using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Screens;

namespace UncappedSpire.UncappedSpireCode.UncappedUpgrades.DeckViewScreenPatches;

[HarmonyPatch(nameof(NDeckViewScreen), "DisplayCards")]
public class Patch_DisplayCards
{
    [HarmonyPrefix]
    public static void Prefix(NDeckViewScreen __instance)
    {
        UpgradeContext.EnableMultiplier();
    }
    
    [HarmonyFinalizer]
    public static void Finalizer(NDeckViewScreen __instance)
    {
        UpgradeContext.DisableMultiplier();
    }
}