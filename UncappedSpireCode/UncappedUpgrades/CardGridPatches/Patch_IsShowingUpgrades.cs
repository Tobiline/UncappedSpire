using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;

namespace UncappedSpire.UncappedSpireCode.UncappedUpgrades.CardGridPatches;

[HarmonyPatch(typeof(NCardGrid), "set_" + nameof(NCardGrid.IsShowingUpgrades))]
public class Patch_IsShowingUpgrades
{
    private static readonly MethodInfo methodToFind =
        AccessTools.Method(typeof(NGridCardHolder), "SetIsPreviewingUpgrade");
    private static readonly MethodInfo methodToCall = AccessTools.Method(typeof(NGridCardHolder), "UpdateCardModel");
    
    [HarmonyTranspiler]
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var code = new List<CodeInstruction>(instructions);
        
        for (var i = 0; i < code.Count; i++)
        {
            if (code[i].opcode == OpCodes.Callvirt && code[i].operand is MethodInfo method && method == methodToFind)
            {
                code.InsertRange(i - 3, [
                    new CodeInstruction(OpCodes.Ldloc_3),
                    new CodeInstruction(OpCodes.Callvirt, methodToCall)
                ]);
                break;
            }
        }

        return code;
    }
}