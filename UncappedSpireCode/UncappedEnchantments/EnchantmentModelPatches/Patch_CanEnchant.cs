using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;

namespace UncappedSpire.UncappedSpireCode.UncappedEnchantments.EnchantmentModelPatches;

[HarmonyPatch(typeof(EnchantmentModel), nameof(EnchantmentModel.CanEnchant))]
public class Patch_CanEnchant
{
    private static readonly MethodInfo methodToFind = AccessTools.Method(typeof(object), nameof(GetType));
    private static readonly MethodInfo Method_CanEnchantCardType = AccessTools.Method(typeof(EnchantmentModel), nameof(EnchantmentModel.CanEnchantCardType));
    private static readonly MethodInfo Method_HasUselessSameEnchantments = AccessTools.Method(typeof(Patch_CanEnchant), nameof(HasUselessSameEnchantments));
    
    [HarmonyTranspiler]
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        var code = new List<CodeInstruction>(instructions);
        
        for (var i = 0; i < code.Count; i++)
        {
            var instruction = code[i];
            if (instruction.opcode == OpCodes.Callvirt && instruction.operand is MethodInfo method1 &&
                method1 == Method_CanEnchantCardType)
            {
                var ldEnchantment = code[i - 3];
                var ldCard = code[i - 2];

                var thisConditionLabel = generator.DefineLabel();
                var nextConditionLabels = code[i + 1].operand;

                code[i + 1].operand = thisConditionLabel;

                code.InsertRange(i + 4, [
                    new CodeInstruction(ldEnchantment)
                    {
                        labels = [thisConditionLabel]
                    },
                    new CodeInstruction(ldCard),
                    new CodeInstruction(OpCodes.Call, Method_HasUselessSameEnchantments),
                    new CodeInstruction(OpCodes.Brfalse_S, nextConditionLabels),
                    new CodeInstruction(OpCodes.Ldc_I4_0),
                    new CodeInstruction(OpCodes.Ret)
                ]);
                i += 6;
            }
            else if (instruction.opcode == OpCodes.Call && instruction.operand is MethodInfo method3 && method3 == methodToFind)
            {
                code[i + 3].opcode = OpCodes.Ldc_I4_1;
                break;
            }
        }

        return code;
    }

    public static HashSet<Type> EnchantmentsToNotDuplicate =
    [
        typeof(Imbued),
        typeof(PerfectFit),
        typeof(RoyallyApproved),
        typeof(Slither),
        typeof(SoulsPower),
        typeof(Steady),
        typeof(TezcatarasEmber)
    ];
    
    public static bool HasUselessSameEnchantments(EnchantmentModel enchantment, CardModel card)
    {
        var enchantmentType = enchantment.GetType();
        if (card.Enchantment == null || !EnchantmentsToNotDuplicate.Contains(enchantmentType))
            return false;

        var multiEnchantment = card.Enchantment as MultiEnchantment;
        
        return multiEnchantment!.EnchantmentsOnCards.Any(c => c.Enchantment!.GetType() == enchantmentType);
    }
}