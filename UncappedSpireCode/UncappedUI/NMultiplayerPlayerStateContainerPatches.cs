using System.Reflection.Emit;
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Multiplayer;

namespace UncappedSpire.UncappedSpireCode.UncappedUI;

[HarmonyPatch(typeof(NMultiplayerPlayerStateContainer), "GetTargetPosition")]
public class NMultiplayerPlayerStateContainerPatches
{
    [HarmonyTranspiler]
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var code = new List<CodeInstruction>(instructions);
        
        for (var i = 0; i < code.Count; i++)
        {
            if (code[i].opcode == OpCodes.Add && code[i + 1].opcode == OpCodes.Mul)
            {
                code[i - 4].opcode = OpCodes.Ldc_I4_2;
                break;
            }
        }

        return code;
    }
}