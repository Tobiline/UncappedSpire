using HarmonyLib;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

namespace UncappedSpire.UncappedSpireCode.LimitlessUpgrades.CardModelPatches;

[HarmonyPatch(typeof(CardModel), "AddExtraArgsToDescription")]
public class Patch_AddExtraArgsToDescription
{
    [HarmonyPrefix]
    public static bool Prefix(CardModel __instance, LocString description)
    {
        description.Add("UpgradeLevel", __instance.CurrentUpgradeLevel);
        
        return false;
    }
}