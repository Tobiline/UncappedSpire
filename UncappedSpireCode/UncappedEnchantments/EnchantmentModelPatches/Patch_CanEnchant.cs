using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;

namespace UncappedSpire.UncappedSpireCode.UncappedEnchantments.EnchantmentModelPatches;

[HarmonyPatch(typeof(EnchantmentModel), nameof(EnchantmentModel.CanEnchant))]
public class Patch_CanEnchant
{
    private static readonly MethodInfo methodToFind = AccessTools.Method(typeof(object), nameof(GetType));
    
    [HarmonyTranspiler]
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var code = new List<CodeInstruction>(instructions);
        
        for (var i = 0; i < code.Count; i++)
        {
            if (code[i].opcode == OpCodes.Call && code[i].operand is MethodInfo method && method == methodToFind)
            {
                code[i + 3].opcode = OpCodes.Ldc_I4_1;
                break;
            }
        }

        return code;
    }
}