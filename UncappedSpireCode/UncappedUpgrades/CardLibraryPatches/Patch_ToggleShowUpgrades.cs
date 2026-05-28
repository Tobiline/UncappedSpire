using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary;
using UncappedSpire.UncappedSpireCode.UncappedUpgrades.UI.CardLibrary;

namespace UncappedSpire.UncappedSpireCode.UncappedUpgrades.CardLibraryPatches;

[HarmonyPatch(typeof(NCardLibrary), "ToggleShowUpgrades")]
public class Patch_ToggleShowUpgrades
{
    [HarmonyPrefix]
    public static void Prefix(NCardLibrary __instance)
    {
        var uncappedCardInput = SpireField_UncappedCardInput.UncappedCardInput.Get(__instance);
        UpgradeContext.AddOrUpdateMultiplier((int)uncappedCardInput.GetValue());
    }
    
    [HarmonyFinalizer]
    public static void Finalizer(NCardLibrary __instance)
    {
        UpgradeContext.RemoveMultiplier();
    }
}