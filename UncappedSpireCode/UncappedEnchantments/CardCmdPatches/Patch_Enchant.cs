using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Models;

namespace UncappedSpire.UncappedSpireCode.UncappedEnchantments.CardCmdPatches;

[HarmonyPatch(typeof(CardCmd), nameof(CardCmd.Enchant), [typeof(EnchantmentModel), typeof(CardModel), typeof(decimal)])]
public class Patch_Enchant
{
    private static readonly MethodInfo ToFind_Method_EnchantInternal = AccessTools.Method(typeof(CardModel), "EnchantInternal");
    
    // Removes checks for single enchantments, always enchants if able
    [HarmonyTranspiler]
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var code = new List<CodeInstruction>(instructions);
        List<CodeInstruction> enchantInstructions = [];
        
        for (var i = 0; i < code.Count; i++)
        {
            var instruction = code[i];
            if (instruction.opcode == OpCodes.Callvirt && instruction.operand is MethodInfo method && method == ToFind_Method_EnchantInternal)
            {
                enchantInstructions = code.GetRange(i - 3, 6);
            }
            else if (instruction.opcode == OpCodes.Ldc_I4_S && instruction.operand is sbyte n && n == 58)
            {
                enchantInstructions[0].labels = [
                    ..code[i - 1].labels, 
                    ..code[i - 17].labels
                ];
                
                code.RemoveRange(i - 17, 49);
                code.InsertRange(i - 17, enchantInstructions);
                break;
            }
        }
    
        return code;
    }
}