// using System.Reflection;
// using System.Reflection.Emit;
// using HarmonyLib;
// using MegaCrit.Sts2.Core.Multiplayer.Game;
// using MegaCrit.Sts2.Core.Multiplayer.Game.Lobby;
// using MegaCrit.Sts2.Core.Runs;
//
// namespace UncappedSpire.UncappedSpireCode.UncappedActs.StartRunLobbyPatches;
//
// [HarmonyPatch(typeof(StartRunLobby), MethodType.Constructor, [
//     typeof(GameMode), typeof(INetGameService), typeof(IStartRunLobbyListener), typeof(int)
// ])]
// public class Patch_Constructor
// {
//     public static readonly MethodInfo ToFind_Method_get_NetService = AccessTools.PropertyGetter(typeof(StartRunLobby), nameof(StartRunLobby.NetService));
//     public static readonly MethodInfo Method_AddModifiers = AccessTools.Method(typeof(Patch_Constructor), nameof(AddModifiers));
//     
//     [HarmonyTranspiler]
//     static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
//     {
//         var code = new List<CodeInstruction>(instructions);
//         
//         for (var i = 0; i < code.Count; i++)
//         {
//             var instruction = code[i];
//             if (instruction.opcode == OpCodes.Call && instruction.operand is MethodInfo method && method == ToFind_Method_get_NetService)
//             {
//                 var ldInstance = new CodeInstruction(code[i - 1]);
//                 code.InsertRange(i + 5, [
//                     ldInstance,
//                     new CodeInstruction(OpCodes.Call, Method_AddModifiers)
//                 ]);
//                 break;
//             }
//         }
//
//         return code;
//     }
//
//     static void AddModifiers(StartRunLobby __instance)
//     {
//         __instance.SetModifiers([]);
//     }
// }