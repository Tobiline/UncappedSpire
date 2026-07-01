// using System.Reflection;
// using System.Reflection.Emit;
// using BaseLib.Utils.Patching;
// using HarmonyLib;
// using MegaCrit.Sts2.Core.AutoSlay.Handlers.Rooms;
// using MegaCrit.Sts2.Core.Commands;
//
// namespace UncappedSpire.UncappedSpireCode.AutoMultiSlayer;
//
// [HarmonyPatch(typeof(CombatRoomHandler), nameof(CombatRoomHandler.HandleAsync), MethodType.Async)]
// public class AutoSlayerCombatPatch
// {
//     [HarmonyTranspiler]
//     static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator, MethodBase original)
//     {
//         var code = new List<CodeInstruction>(instructions);
//         
//         var methodToFind = AccessTools.Method(typeof(PlayerCmd), nameof(PlayerCmd.EndTurn), [typeof(string)]);
//         var methodToCall = AccessTools.Method(typeof(AutoSlayerCombatPatch), nameof(EndTurnProperly));
//         
//         for (var i = 0; i < code.Count; i++)
//         {
//             var instruction = code[i];
//             if (instruction.opcode == OpCodes.Call && instruction.operand is MethodInfo method && method == methodToFind)
//             {
//                 var asyncMethodCall = AsyncMethodCall.Create(
//                     generator, code, original, methodToCall,
//                     AccessTools.Method(typeof(AutoSlayerCombatPatch), nameof(EndTurnProperly)));
//                 
//                 code.RemoveAt(i - 8);
//                 i--;
//                 code.RemoveRange(i - 6, 4);
//                 i -= 4;
//                 code.InsertRange(i - 2, [
//                     new CodeInstruction(OpCodes.Ldarg_1),
//                     new CodeInstruction(OpCodes.Call, methodToCall),
//                     new CodeInstruction(OpCodes.Stloc_1),
//                 ]);
//                 break;
//             }
//         }
//
//         return code;
//     }
//
//     public async Task EndTurnProperly()
//     {
//         
//     }
// }