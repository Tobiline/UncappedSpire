using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs.History;

namespace UncappedSpire.UncappedSpireCode.UncappedEnchantments.CardCmdPatches;

[HarmonyPatch(typeof(CardCmd), nameof(CardCmd.Enchant), [typeof(EnchantmentModel), typeof(CardModel), typeof(decimal)])]
public class Patch_Enchant
{
    private static readonly MethodInfo ToFind_Method_EnchantInternal = AccessTools.Method(typeof(CardModel), "EnchantInternal");
    
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
    
    [HarmonyPrefix]
    public static bool Prefix(EnchantmentModel enchantment, CardModel card, decimal amount, ref EnchantmentModel? __result)
    {
        enchantment.Amount = (int)amount;
        return true;
        // MainFile.Logger.Info("ENCHANTING");
        // enchantment.AssertMutable();
        // if (!enchantment.CanEnchant(card))
        // {
        //     throw new InvalidOperationException($"Cannot enchant {card.Id} with {enchantment.Id}.");
        // }
        // enchantment.Amount = (int)amount;
        // card.EnchantInternal(enchantment, amount);
        // enchantment.ModifyCard();
        // card.FinalizeUpgradeInternal();
        // var pile = card.Pile;
        // if (pile != null && pile.Type == PileType.Deck)
        // {
        //     card.Owner.RunState.CurrentMapPointHistoryEntry?.GetEntry(card.Owner.NetId).CardsEnchanted.Add(new CardEnchantmentHistoryEntry(card, enchantment.Id));
        // }
        // __result = card.Enchantment;
        //
        // return false;
    }
}