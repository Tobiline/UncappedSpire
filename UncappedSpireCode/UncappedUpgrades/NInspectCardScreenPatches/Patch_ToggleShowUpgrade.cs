using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Screens;

namespace UncappedSpire.UncappedSpireCode.UncappedUpgrades.NInspectCardScreenPatches;

[HarmonyPatch(typeof(NInspectCardScreen), "ToggleShowUpgrade")]
public class Patch_ToggleShowUpgrade
{
    [HarmonyPrefix]
    public static void Prefix(NInspectCardScreen __instance)
    {
        UpgradeContext.EnableMultiplier();
    }
    
    [HarmonyFinalizer]
    public static void Finalizer(NInspectCardScreen __instance)
    {
        UpgradeContext.DisableMultiplier();
    }
}