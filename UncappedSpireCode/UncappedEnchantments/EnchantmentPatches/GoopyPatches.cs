using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;
using UncappedSpire.UncappedSpireCode.Config;

namespace UncappedSpire.UncappedSpireCode.UncappedEnchantments.EnchantmentPatches;

// TODO: Find a way to get around having to do this.
// Probably via contextual getters in MultiEnchant, would need to test performance
[HarmonyPatch(typeof(Goopy), nameof(Goopy.AfterCardPlayed))]
public class GoopyPatches
{
    [HarmonyTranspiler]
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var code = new List<CodeInstruction>(instructions);

        var methodToFind = AccessTools.PropertySetter(typeof(EnchantmentModel), nameof(EnchantmentModel.Amount));
        var Method_AddAmountToGoopyFromMulti = AccessTools.Method(typeof(GoopyPatches), nameof(AddAmountToGoopyFromMulti));
        
        for (var i = 0; i < code.Count; i++)
        {
            var instruction = code[i];
            if (instruction.opcode == OpCodes.Callvirt && instruction.operand is MethodInfo method && method == methodToFind)
            {
                code.RemoveRange(i - 6, 7);
                code.InsertRange(i - 6, [
                    new CodeInstruction(OpCodes.Call, Method_AddAmountToGoopyFromMulti)
                ]);
                break;
            }
        }
    
        return code;
    }

    public static void AddAmountToGoopyFromMulti(EnchantmentModel enchantmentModel)
    {
        if (ContextManager.UncappedEnchantmentsEnabled)
        {
            var multiEnchantment = (MultiEnchantment)enchantmentModel;
            var goopyEnchantment = multiEnchantment.EnchantmentsOnCards.Find(c => c.Enchantment is Goopy)!.Enchantment;
            goopyEnchantment!.Amount++;
        }
        else
        {
            enchantmentModel.Amount++;
        }
    }
}