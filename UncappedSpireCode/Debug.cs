// using System.Reflection;
// using HarmonyLib;
// using MegaCrit.Sts2.Core.Entities.Relics;
// using MegaCrit.Sts2.Core.Models;
// using MegaCrit.Sts2.Core.Runs;
//
// namespace UncappedSpire.UncappedSpireCode;
//
// [HarmonyPatch(typeof(RelicGrabBag), "GetAvailableDeque")]
// public class Debug
// {
//     public static MethodInfo Method_GetDeque = AccessTools.Method(typeof(RelicGrabBag), "GetDeque");
//     
//     [HarmonyPrefix]
//     public static void Prefix(RelicGrabBag __instance, RelicRarity rarity)
//     {
//         var list = (List<RelicModel>)Method_GetDeque.Invoke(__instance, [rarity])!;
//         MainFile.Logger.Info($"Rarity: {rarity}");
//         foreach (var relic in list)
//         {
//             MainFile.Logger.Info($"Relic: {relic.Title}");
//         }
//     }
// }