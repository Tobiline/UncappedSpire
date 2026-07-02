using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;

namespace UncappedSpire.UncappedSpireCode.UncappedActs.PotionContainerPatches;

[HarmonyPatch(typeof(Player), "SetMaxPotionCountInternal")]
public class Patch_SetMaxPotionCountInternal
{
    [HarmonyTranspiler]
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        var code = new List<CodeInstruction>(instructions);
        
        var methodToFind = AccessTools.Method(typeof(List<PotionModel>), nameof(List<PotionModel>.IndexOf), [typeof(PotionModel)]);
        
        for (var i = 0; i < code.Count; i++)
        {
            if (code[i].opcode == OpCodes.Callvirt && code[i].operand is MethodInfo methodInfo && methodInfo == methodToFind)
            {
                var label = generator.DefineLabel();
                var ldInd = code[i + 2].opcode;
                var stInd = code[i + 1].opcode;
                code.InsertRange(i + 2, [
                    new CodeInstruction(ldInd),
                    new CodeInstruction(OpCodes.Ldc_I4_M1),
                    new CodeInstruction(OpCodes.Ceq),
                    new CodeInstruction(OpCodes.Brfalse_S, label),
                    new CodeInstruction(OpCodes.Ldc_I4, int.MaxValue),
                    new CodeInstruction(stInd),
                    new CodeInstruction(OpCodes.Nop)
                    {
                        labels = [label]
                    }
                ]);
                break;
            }
        }

        return code;
    }
}