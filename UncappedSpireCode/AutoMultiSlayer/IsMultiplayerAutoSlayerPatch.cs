// using System.Reflection.Emit;
// using HarmonyLib;
// using MegaCrit.Sts2.Core.AutoSlay;
// using MegaCrit.Sts2.Core.Helpers;
//
// namespace UncappedSpire.UncappedSpireCode.AutoMultiSlayer;
//
// [HarmonyPatch(typeof(AutoSlayer), nameof(AutoSlayer.Start))]
// public class IsMultiplayerAutoSlayerPatch
// {
//     [HarmonyTranspiler]
//     static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
//     {
//         var code = new List<CodeInstruction>(instructions);
//         
//         var methodToCall = AccessTools.Method(typeof(IsMultiplayerAutoSlayerPatch), nameof(SetIsMultiplayer));
//         
//         for (var i = 0; i < code.Count; i++)
//         {
//             if (code[i].opcode == OpCodes.Newobj)
//             {
//                 var ldAutoSlayer = code[i - 1].opcode;
//                 
//                 code.InsertRange(i - 1, [
//                     new CodeInstruction(ldAutoSlayer),
//                     new CodeInstruction(OpCodes.Call, methodToCall)
//                 ]);
//                 
//                 break;
//             }
//         }
//
//         return code;
//     }
//
//     public static void SetIsMultiplayer(AutoSlayer autoSlayer)
//     {
//         if (CommandLineHelper.HasArg("fastmp"))
//         {
//             AutoSlayerFields.IsMultiplayer.Set(autoSlayer, true);
//         }
//     }
// }