// using System.Reflection;
// using System.Reflection.Emit;
// using HarmonyLib;
// using MegaCrit.Sts2.Core.AutoSlay;
//
// namespace UncappedSpire.UncappedSpireCode.AutoMultiSlayer;
//
// [HarmonyPatch(typeof(AutoSlayer), "PlayRunAsync", MethodType.Async)]
// public class MultiSlayerPlayPatch
// {
//     private static MethodInfo OriginalPlayMainMenuAsync = AccessTools.Method(typeof(AutoSlayer), "PlayMainMenuAsync");
//     
//     [HarmonyTranspiler]
//     static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
//     {
//         var code = new List<CodeInstruction>(instructions);
//         
//         var methodToCall = AccessTools.Method(typeof(MultiSlayerPlayPatch), nameof(PlayMainMenuAsync));
//         
//         for (var i = 0; i < code.Count; i++)
//         {
//             if (code[i].opcode == OpCodes.Call && code[i].operand is MethodInfo methodInfo && methodInfo == OriginalPlayMainMenuAsync)
//             {
//                 code[i].operand = methodToCall;
//                 break;
//             }
//         }
//
//         return code;
//     }
//
//     public static async Task PlayMainMenuAsync(AutoSlayer autoSlayer, CancellationToken ct)
//     {
//         if (AutoSlayerFields.IsMultiplayer.Get(autoSlayer))
//         {
//             await autoSlayer.PlayMultiplayerMenuAsync(ct);
//         }
//         else
//         {
//             await (Task)OriginalPlayMainMenuAsync.Invoke(autoSlayer, [ct])!;
//         }
//     }
// }