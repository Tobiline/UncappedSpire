// using System.Reflection;
// using HarmonyLib;
// using MegaCrit.Sts2.Core.AutoSlay.Helpers;
// using MegaCrit.Sts2.Core.Multiplayer.Game;
// using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
// using MegaCrit.Sts2.Core.Nodes.Rooms;
// using MegaCrit.Sts2.Core.Runs;
// using UncappedSpire.UncappedSpireCode.Util;
//
// namespace UncappedSpire.UncappedSpireCode;
//
// [HarmonyPatch]
// public class Debug
// {
//     private static MethodInfo Method_get_State = AccessTools.PropertyGetter(typeof(RunManager), "State");
//     
//     [HarmonyPatch(typeof(UiHelper), nameof(UiHelper.Click))]
//     [HarmonyPrefix]
//     public static void ClickPrefix(NClickableControl button, ref int delayMs)
//     {
//         delayMs = 500;
//         
//         var player = RunUtil.GetLocalPlayer();
//         MainFile.Logger.Info($"Player: {player?.NetId} clicked {button.Name}");
//     }
//     
//     [HarmonyPatch(typeof(EventSynchronizer), nameof(EventSynchronizer.ChooseLocalOption))]
//     [HarmonyPrefix]
//     public static void ChooseLocalOptionPrefix(int index)
//     {
//         var player = RunUtil.GetLocalPlayer();
//         MainFile.Logger.Info($"Player: {player?.NetId} chose option {index}");
//     }
// }