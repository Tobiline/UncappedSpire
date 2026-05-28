using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Screens;

namespace UncappedSpire.UncappedSpireCode.UncappedUpgrades.NInspectCardScreenPatches;

[HarmonyPatch(typeof(NInspectCardScreen), "ToggleShowUpgrade")]
public class Patch_ToggleShowUpgrade
{
    [HarmonyPrefix]
    public static void Prefix(NInspectCardScreen __instance)
    {
        var uncappedCardInput = UI.InspectCardScreen.SpireField_UncappedCardInput.UncappedCardInput.Get(__instance);
        UpgradeContext.AddOrUpdateMultiplier((int)uncappedCardInput.GetValue());
    }
    
    [HarmonyFinalizer]
    public static void Finalizer(NInspectCardScreen __instance)
    {
        UpgradeContext.RemoveMultiplier();
    }
}