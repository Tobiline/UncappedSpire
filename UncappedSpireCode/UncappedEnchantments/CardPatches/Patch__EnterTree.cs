// using System.Reflection;
// using System.Reflection.Emit;
// using HarmonyLib;
// using MegaCrit.Sts2.Core.Nodes.Cards;
//
// namespace UncappedSpire.UncappedSpireCode.UncappedEnchantments.CardPatches;
//
// [HarmonyPatch(typeof(NCard), nameof(NCard._EnterTree))]
// public class Patch__EnterTree
// {
//     private static readonly MethodInfo ToFind_Method_SubscribeToModel = AccessTools.Method(typeof(NCard), "SubscribeToModel");
//     private static readonly MethodInfo ToReplace_Method_SubscribeToModel = AccessTools.Method(typeof(CardExtensions), nameof(CardExtensions.SubscribeToModel));
//     
//     [HarmonyTranspiler]
//     static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
//     {
//         var code = new List<CodeInstruction>(instructions);
//         
//         for (var i = 0; i < code.Count; i++)
//         {
//             var instruction = code[i];
//             if (instruction.opcode == OpCodes.Call && instruction.operand is MethodInfo method && method == ToFind_Method_SubscribeToModel)
//             {
//                 instruction.operand = ToReplace_Method_SubscribeToModel;
//             }
//         }
//
//         return code;
//     }
// }