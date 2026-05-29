using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary;

namespace UncappedSpire.UncappedSpireCode.UncappedUpgrades.CardLibraryGridPatches;

[HarmonyPatch(typeof(NCardLibraryGrid), "DisplayCards")]
public class Patch_DisplayCards
{
    [HarmonyPrefix]
    public static void Prefix(NCardLibraryGrid __instance)
    {
        UpgradeContext.EnableMultiplier();
    }
    
    [HarmonyFinalizer]
    public static void Finalizer(NCardLibraryGrid __instance)
    {
        UpgradeContext.DisableMultiplier();
    }
}