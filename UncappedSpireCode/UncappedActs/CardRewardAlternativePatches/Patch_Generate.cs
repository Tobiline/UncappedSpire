using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.CardRewardAlternatives;

namespace UncappedSpire.UncappedSpireCode.UncappedActs.CardRewardAlternativePatches;

// TODO: REMOVE AND COMBINE PAELS WING
[HarmonyPatch(typeof(CardRewardAlternative), nameof(CardRewardAlternative.Generate))]
public class Patch_Generate
{
    [HarmonyTranspiler]
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var code = new List<CodeInstruction>(instructions);
        
        for (var i = 0; i < code.Count; i++)
        {
            var instruction = code[i];
            if (instruction.opcode == OpCodes.Throw)
            {
                code.RemoveRange(i - 2, 3);
                code.Insert(i - 2, new CodeInstruction(OpCodes.Nop));
                break;
            }
        }

        return code;
    }
}