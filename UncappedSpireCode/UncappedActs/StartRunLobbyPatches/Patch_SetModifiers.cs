// using HarmonyLib;
// using MegaCrit.Sts2.Core.Models;
// using MegaCrit.Sts2.Core.Multiplayer.Game.Lobby;
//
// namespace UncappedSpire.UncappedSpireCode.UncappedActs.StartRunLobbyPatches;
//
// [HarmonyPatch(typeof(StartRunLobby), nameof(StartRunLobby.SetModifiers))]
// public class Patch_SetModifiers
// {
//     [HarmonyPrefix]
//     public static void Prefix(StartRunLobby __instance, ref List<ModifierModel> modifiers)
//     {
//         var uncappedActs = ModelDb.Modifier<UncappedActs>().ToMutable();
//         modifiers.Insert(0, uncappedActs);
//     }
// }