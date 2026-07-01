using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Cards;
using UncappedSpire.UncappedSpireCode.Config;

namespace UncappedSpire.UncappedSpireCode.UncappedEnchantments.CardPatches;

[HarmonyPatch(typeof(NCard), "UpdateVisuals")]
public class Patch_UpdateVisuals
{
    private static readonly MethodInfo ToFind_Method_SubscribeToModel = AccessTools.Method(typeof(NCard), "UpdateEnchantmentVisuals");
    private static readonly MethodInfo ToReplace_Method_SubscribeToModel = AccessTools.Method(typeof(Patch_UpdateVisuals), nameof(ToReplace));
    
    // Swaps out to use the multi-enchantment nodes
    [HarmonyTranspiler]
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var code = new List<CodeInstruction>(instructions);
        
        for (var i = 0; i < code.Count; i++)
        {
            var instruction = code[i];
            if (instruction.opcode == OpCodes.Call && instruction.operand is MethodInfo method && method == ToFind_Method_SubscribeToModel)
            {
                instruction.operand = ToReplace_Method_SubscribeToModel;
            }
        }

        return code;
    }

    public static void ToReplace(NCard nCard)
    {
        if (ContextManager.UncappedEnchantmentsEnabled)
        {
            nCard.UpdateEnchantmentVisuals();
        }
        else
        {
            ToFind_Method_SubscribeToModel.Invoke(nCard, null);
        }
    }
}