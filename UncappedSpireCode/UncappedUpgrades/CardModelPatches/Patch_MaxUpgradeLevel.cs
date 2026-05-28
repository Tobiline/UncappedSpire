using HarmonyLib;
using MegaCrit.Sts2.Core.Models;

namespace UncappedSpire.UncappedSpireCode.UncappedUpgrades.CardModelPatches;

[HarmonyPatch(typeof(CardModel), nameof(CardModel.MaxUpgradeLevel), MethodType.Getter)]
public static class Patch_MaxUpgradeLevel
{
    public static void Postfix(CardModel __instance, ref int __result)
    {
        __result = int.MaxValue;
    }
}