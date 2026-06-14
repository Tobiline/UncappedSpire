using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;

namespace UncappedSpire.UncappedSpireCode.UncappedEnchantments.CardModelPatches;

[HarmonyPatch(typeof(CardModel), nameof(CardModel.EnchantInternal))]
public class Patch_EnchantInternal
{
    //private static readonly MethodInfo ToFind_Method_ApplyInternal = AccessTools.Method(typeof(EnchantmentModel), nameof(EnchantmentModel.ApplyInternal));
    private static readonly MethodInfo ToFind_Method_set_Enchantment = AccessTools.PropertySetter(typeof(CardModel), nameof(CardModel.Enchantment));
    private static FieldInfo Field_EnchantmentChanged = AccessTools.Field(typeof(CardModel), nameof(CardModel.EnchantmentChanged));
    
    // As Enchantment setter points towards the underlying list, ApplyInternal needs to be used on the parameter and not the property.
    // [HarmonyTranspiler]
    // static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    // {
    //     var code = new List<CodeInstruction>(instructions);
    //
    //     var enchantmentArg = new CodeInstruction(OpCodes.Ldarg_1);
    //     for (var i = 0; i < code.Count; i++)
    //     {
    //         var instruction = code[i];
    //         if (instruction.opcode == OpCodes.Call && instruction.operand is MethodInfo method1 &&
    //             method1 == ToFind_Method_set_Enchantment)
    //         {
    //             enchantmentArg = code[i - 1];
    //         }
    //         else if (instruction.opcode == OpCodes.Callvirt && instruction.operand is MethodInfo method2 && method2 == ToFind_Method_ApplyInternal)
    //         {
    //             code.RemoveRange(i - 4, 2);
    //             code.Insert(i - 4, enchantmentArg);
    //         }
    //     }
    //
    //     return code;
    // }
    
    [HarmonyPrefix]
    public static bool Prefix(CardModel __instance, EnchantmentModel enchantment, decimal amount)
    {
        enchantment.Amount = (int)amount;
        __instance.AssertMutable();
        enchantment.AssertMutable();
        ToFind_Method_set_Enchantment.Invoke(__instance, [enchantment]);
        enchantment.ApplyInternal(__instance, amount);
        var del = (Action?)Field_EnchantmentChanged.GetValue(__instance);
        del?.Invoke();

        return false;
    }
}