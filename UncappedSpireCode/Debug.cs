// using System.Reflection;
// using HarmonyLib;
// using MegaCrit.Sts2.Core.Hooks;
// using MegaCrit.Sts2.Core.Multiplayer.Game;
// using MegaCrit.Sts2.Core.Nodes;
// using MegaCrit.Sts2.Core.Nodes.Screens.Map;
// using MegaCrit.Sts2.Core.Rooms;
// using MegaCrit.Sts2.Core.Runs;
// using MegaCrit.Sts2.Core.TestSupport;
// using UncappedSpire.UncappedSpireCode.Config;
//
// namespace UncappedSpire.UncappedSpireCode;
//
// [HarmonyPatch(typeof(RunManager), "EnterAct")]
// public class Debug
// {
//     public static readonly MethodInfo Method_get_State = AccessTools.PropertyGetter(typeof(RunManager), "State");
//     public static readonly MethodInfo Method_ClearScreens = AccessTools.Method(typeof(RunManager), "ClearScreens");
//     public static readonly MethodInfo Method_ExitCurrentRooms = AccessTools.Method(typeof(RunManager), "ExitCurrentRooms");
//     public static readonly MethodInfo Method_EnterRoomInternal = AccessTools.Method(typeof(RunManager), "EnterRoomInternal");
//     public static readonly MethodInfo Method_FadeIn = AccessTools.Method(typeof(RunManager), "FadeIn");
//     public static readonly FieldInfo Field_ActEntered = AccessTools.Field(typeof(RunManager), "ActEntered");
//
//     [HarmonyPrefix]
//     public static bool Prefix(RunManager __instance, int currentActIndex, bool doTransition, ref Task __result)
//     {
//         __result = EnterAct(__instance, currentActIndex, doTransition);
//         return false;
//     }
//
//     public static async Task EnterAct(RunManager __instance, int currentActIndex, bool doTransition)
//     {
//         var i = 0;
//         
//         var State = (RunState?)Method_get_State.Invoke(RunManager.Instance, null);
//         
//         MainFile.Logger.Info("37");
//         if (State == null)
//         {
//             return;
//         }
//         if (TestMode.IsOff)
//         {
//             await NGame.Instance.Transition.RoomFadeOut();
//         }
//         MainFile.Logger.Info("46");
//         using (new NetLoadingHandle(__instance.NetService))
//         {
//             MainFile.Logger.Info("49");
//             Method_ClearScreens.Invoke(__instance, null);
//             MainFile.Logger.Info("51");
//             await (Task)Method_ExitCurrentRooms.Invoke(__instance, null)!;
//             MainFile.Logger.Info("53");
//             await __instance.SetActInternal(currentActIndex);
//             MainFile.Logger.Info("55");
//             if (currentActIndex == 0 && State.ExtraFields.StartedWithNeow && ContextManager.Current_Chapter <= 1)
//             {
//                 if (NRun.Instance != null)
//                 {
//                     NMapScreen.Instance?.InitMarker(State.Map.StartingMapPoint.coord);
//                 }
//                 await __instance.EnterMapCoord(State.Map.StartingMapPoint.coord);
//                 NMapScreen.Instance?.RefreshAllMapPointVotes();
//             }
//             else
//             {
//                 MainFile.Logger.Info("67");
//                 await (Task)Method_EnterRoomInternal.Invoke(__instance, [new MapRoom()])!;
//                 MainFile.Logger.Info("69");
//                 ((Action?)Field_ActEntered.GetValue(__instance))?.Invoke();
//                 MainFile.Logger.Info("71");
//                 await (Task)Method_FadeIn.Invoke(__instance, [doTransition])!;
//                 MainFile.Logger.Info("73");
//             }
//             MainFile.Logger.Info("75");
//             await Hook.AfterActEntered(State);
//             MainFile.Logger.Info("77");
//         }
//     }
//     //
//     // [HarmonyPostfix]
//     // public static void Postfix()
//     // {
//     //     var state = (RunState?)Method_get_State.Invoke(RunManager.Instance, null);
//     //     
//     //     var message = $"$[POSTFIX] {state.CurrentRoom?.GetType()}";
//     //     
//     //     MainFile.Logger.Info(message);
//     //     MainFile.Logger.Info(message);
//     //     MainFile.Logger.Info(message);
//     //     MainFile.Logger.Info(message);
//     //     MainFile.Logger.Info(message);
//     // }
// }