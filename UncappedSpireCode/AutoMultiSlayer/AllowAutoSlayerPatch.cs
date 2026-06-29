// using HarmonyLib;
// using MegaCrit.Sts2.Core.Nodes;
//
// namespace UncappedSpire.UncappedSpireCode.AutoMultiSlayer;
//
// [HarmonyPatch(typeof(NGame), nameof(NGame.IsReleaseGame))]
// public class AllowAutoSlayerPatch
// {
//     [HarmonyPrefix]
//     private static bool Prefix(ref bool __result)
//     {
//         __result = false;
//         return false;
//     }
// }