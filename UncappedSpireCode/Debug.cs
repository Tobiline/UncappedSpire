// using HarmonyLib;
// using MegaCrit.Sts2.Core.Entities.CardRewardAlternatives;
// using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;
// using MegaCrit.Sts2.Core.Rewards;
//
// namespace UncappedSpire.UncappedSpireCode;
//
// [HarmonyPatch(typeof(NCardRewardSelectionScreen), "ShowScreen")]
// public class Debug
// {
//     [HarmonyPrefix]
//     public static void Prefix(IReadOnlyList<CardRewardAlternative> extraOptions)
//     {
//         foreach (var option in extraOptions)
//         {
//             MainFile.Logger.Info($"Option Title: {option.Title}");
//         }
//     }
// }