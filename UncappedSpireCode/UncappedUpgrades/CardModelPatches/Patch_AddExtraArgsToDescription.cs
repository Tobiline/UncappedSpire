using HarmonyLib;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

namespace UncappedSpire.UncappedSpireCode.UncappedUpgrades.CardModelPatches;

[HarmonyPatch(typeof(CardModel), "AddExtraArgsToDescription")]
public class Patch_AddExtraArgsToDescription
{
    [HarmonyPrefix]
    public static void Prefix(CardModel __instance, LocString description)
    {
        // TODO: Find way to add within the formatter
        description.Add("UpgradeLevelPlusTwo", __instance.CurrentUpgradeLevel + 2);
        description.Add("UpgradeLevelPlusOne", __instance.CurrentUpgradeLevel + 1);
        description.Add("UpgradeLevel", __instance.CurrentUpgradeLevel);
    }
}