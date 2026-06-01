// using HarmonyLib;
// using MegaCrit.Sts2.Core.Models;
//
// namespace UncappedSpire.UncappedSpireCode.UncappedUpgrades.CardModelPatches;
//
// [HarmonyPatch(typeof(CardModel), "get_HoverTips")]
// public class Patch_HoverTips
// {
//     private static int cachedMultiplier = UpgradeContext.GetMultiplierRaw();
//     
//     [HarmonyPrefix]
//     public static void Prefix(CardModel __instance)
//     {
//         cachedMultiplier = UpgradeContext.GetMultiplierRaw();
//         UpgradeContext.UpdateMultiplier(__instance.CurrentUpgradeLevel, false);
//         UpgradeContext.EnableMultiplier();
//     }
//     
//     [HarmonyFinalizer]
//     public static void Finalizer(CardModel __instance)
//     {
//         UpgradeContext.DisableMultiplier();
//         UpgradeContext.UpdateMultiplier(cachedMultiplier, false);
//     }
// }