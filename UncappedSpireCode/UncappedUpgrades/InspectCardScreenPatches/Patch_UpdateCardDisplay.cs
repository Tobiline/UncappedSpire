using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Screens;

namespace UncappedSpire.UncappedSpireCode.UncappedUpgrades.InspectCardScreenPatches;

[HarmonyPatch(typeof(NInspectCardScreen), "UpdateCardDisplay")]
public class Patch_UpdateCardDisplay
{
    [HarmonyTranspiler]
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var code = new List<CodeInstruction>(instructions);
        
        var methodToFind =
            AccessTools.PropertyGetter(typeof(CardModel), nameof(CardModel.IsUpgraded));
        
        for (var i = 0; i < code.Count; i++)
        {
            if (code[i].opcode == OpCodes.Callvirt 
                && code[i].operand is MethodInfo methodInfo 
                && methodInfo == methodToFind 
                && code[i-1].opcode == OpCodes.Ldloc_0)
            {
                code.RemoveRange(i - 1, 3);
                break;
            }
        }
    
        return code;
    }
}