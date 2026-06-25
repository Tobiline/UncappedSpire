// using HarmonyLib;
// using MegaCrit.Sts2.Core.Nodes;
//
// namespace UncappedSpire.UncappedSpireCode.DebugTools;
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