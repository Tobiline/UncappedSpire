using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary;

namespace UncappedSpire.UncappedSpireCode.UncappedUpgrades.CardLibraryPatches;

[HarmonyPatch(typeof(NCardLibrary), "ToggleShowUpgrades")]
public class Patch_ToggleShowUpgrades
{
    [HarmonyPrefix]
    public static void Prefix(NCardLibrary __instance)
    {
        UpgradeContext.EnableMultiplier();
    }
    
    [HarmonyFinalizer]
    public static void Finalizer(NCardLibrary __instance)
    {
        UpgradeContext.DisableMultiplier();
    }
}