// using System.Reflection;
// using HarmonyLib;
// using MegaCrit.Sts2.Core.Runs;
//
// namespace UncappedSpire.UncappedSpireCode;
//
// [HarmonyPatch(typeof(RunManager), "DebugOnlyGetState")]
// public class Debug
// {
//     private static MethodInfo Method_get_State = AccessTools.PropertyGetter(typeof(RunManager), "State");
//     
//     [HarmonyPrefix]
//     public static void Prefix(RunManager __instance)
//     {
//         var state = Method_get_State.Invoke(__instance, null);
//         MainFile.Logger.Info(state?.GetHashCode().ToString() ?? "");
//     }
// }