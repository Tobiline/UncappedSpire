using System.Reflection.Emit;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models.Events;

namespace UncappedSpire.UncappedSpireCode.UncappedActs.NeowPatches;

[HarmonyPatch(typeof(Neow), "GenerateInitialOptions")]
public class Patch_GenerateInitialOptions
{
    [HarmonyTranspiler]
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var code = new List<CodeInstruction>(instructions);
        
        for (var i = 0; i < code.Count; i++)
        {
            if (code[i].opcode == OpCodes.Ldc_I4_0)
            {
                code[i] = new CodeInstruction(OpCodes.Ldc_I4_1);
                break;
            }
        }

        return code;
    }
}