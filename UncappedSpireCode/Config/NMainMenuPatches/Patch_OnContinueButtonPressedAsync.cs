using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using UncappedSpire.UncappedSpireCode.UncappedActs;

namespace UncappedSpire.UncappedSpireCode.Config.NMainMenuPatches;

[HarmonyPatch(typeof(NMainMenu), "OnContinueButtonPressed")]
public class Patch_OnContinueButtonPressedAsync
{
    [HarmonyPrefix]
    public static void Prefix()
    {
        ContextManager.IsInitializing = true;
    }

    [HarmonyFinalizer]
    public static void Finalizer()
    {
        ContextManager.IsInitializing = false;
    }
}