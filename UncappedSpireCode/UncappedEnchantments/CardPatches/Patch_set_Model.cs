// using System.Reflection;
// using System.Reflection.Emit;
// using HarmonyLib;
// using MegaCrit.Sts2.Core.Nodes.Cards;
//
// namespace UncappedSpire.UncappedSpireCode.UncappedEnchantments.CardPatches;
//
// [HarmonyPatch(typeof(NCard), "set_" + nameof(NCard.Model))]
// public class Patch_set_Model
// {
//     private static readonly MethodInfo ToFind_Method_UnsubscribeFromModel = AccessTools.Method(typeof(NCard), "UnsubscribeFromModel");
//     private static readonly MethodInfo ToFind_Method_SubscribeToModel = AccessTools.Method(typeof(NCard), "SubscribeToModel");
//     
//     private static readonly MethodInfo ToReplace_Method_UnsubscribeFromModel = AccessTools.Method(typeof(CardExtensions), nameof(CardExtensions.UnsubscribeFromModel));
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
//             if (instruction.opcode == OpCodes.Call && instruction.operand is MethodInfo method)
//             {
//                 if (method == ToFind_Method_UnsubscribeFromModel)
//                 {
//                     instruction.operand = ToReplace_Method_UnsubscribeFromModel;
//                 }
//                 else if (method == ToFind_Method_SubscribeToModel)
//                 {
//                     instruction.operand = ToReplace_Method_SubscribeToModel;
//                 }
//             }
//         }
//
//         return code;
//     }
// }