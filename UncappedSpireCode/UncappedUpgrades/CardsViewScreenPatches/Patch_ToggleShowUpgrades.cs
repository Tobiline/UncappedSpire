using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Screens;
using SpireField_UncappedCardInput = UncappedSpire.UncappedSpireCode.UncappedUpgrades.UI.DeckViewScreen.SpireField_UncappedCardInput;

namespace UncappedSpire.UncappedSpireCode.UncappedUpgrades.CardsViewScreenPatches;

[HarmonyPatch(typeof(NCardsViewScreen), "ToggleShowUpgrades")]
public class Patch_ToggleShowUpgrades
{
    [HarmonyPrefix]
    public static void Prefix(NCardsViewScreen __instance)
    {
        var uncappedCardInput = SpireField_UncappedCardInput.UncappedCardInput.Get((NDeckViewScreen)__instance);
        UpgradeContext.AddOrUpdateMultiplier((int)uncappedCardInput.GetValue());
    }
    
    [HarmonyFinalizer]
    public static void Finalizer(NCardsViewScreen __instance)
    {
        UpgradeContext.RemoveMultiplier();
    }
}