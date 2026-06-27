// using System.Reflection;
// using System.Reflection.Emit;
// using System.Runtime.CompilerServices;
// using HarmonyLib;
// using MegaCrit.Sts2.Core.AutoSlay;
// using MegaCrit.Sts2.Core.Runs;
// using UncappedSpire.UncappedSpireCode.Config;
// using UncappedSpire.UncappedSpireCode.Util;
//
// namespace UncappedSpire.UncappedSpireCode.AutoMultiSlayer;
//
// [HarmonyPatch]
// public class MultiSlayerPlayPatch
// {
//     static MethodBase TargetMethod()
//     {
//         return AccessTools.Method(typeof(AutoSlayer), "PlayRunAsync").GetAsyncInnerMethodIfExists();
//     }
//     
//     [HarmonyTranspiler]
//     static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
//     {
//         var code = new List<CodeInstruction>(instructions);
//         
//         var methodToFind = AccessTools.PropertyGetter(typeof(ExtraRunFields), nameof(ExtraRunFields.StartedWithNeow));
//         var methodToCall = AccessTools.PropertyGetter(typeof(ContextManager), nameof(ContextManager.Current_Chapter));
//         
//         for (var i = 0; i < code.Count; i++)
//         {
//             if (code[i].opcode == OpCodes.Callvirt && code[i].operand is MethodInfo methodInfo && methodInfo == methodToFind)
//             {
//                 var jumpLabel = code[i + 1].operand;
//                 
//                 code.InsertRange(i + 2, [
//                     new CodeInstruction(OpCodes.Call, methodToCall),
//                     new CodeInstruction(OpCodes.Ldc_I4_1),
//                     new CodeInstruction(OpCodes.Cgt),
//                     new CodeInstruction(OpCodes.Brtrue, jumpLabel)
//                 ]);
//                 break;
//             }
//         }
//
//         return code;
//     }
// }