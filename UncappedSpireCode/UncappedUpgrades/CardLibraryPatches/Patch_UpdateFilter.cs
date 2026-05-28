using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary;
using UncappedSpire.UncappedSpireCode.UncappedUpgrades.CardModelPatches;

namespace UncappedSpire.UncappedSpireCode.UncappedUpgrades.CardLibraryPatches;

[HarmonyPatch(typeof(NCardLibrary), "UpdateFilter")]
public class Patch_UpdateFilter
{
    [HarmonyTranspiler]
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var code = new List<CodeInstruction>(instructions);
        
        var methodToFind = AccessTools.Method(typeof(CardModel), "UpgradeInternal");
        var methodToCall =
            AccessTools.Method(typeof(CardModelExtensions), nameof(CardModelExtensions.UpgradeToInternal));
        
        var getMultiplierMethod = AccessTools.Method(typeof(UpgradeContext), nameof(UpgradeContext.GetMultiplier));
        
        for (var i = 0; i < code.Count; i++)
        {
            if (code[i].opcode == OpCodes.Callvirt && code[i].operand is MethodInfo method && method == methodToFind)
            {
                code.RemoveAt(i);
                code.InsertRange(i,[
                    new CodeInstruction(OpCodes.Call, getMultiplierMethod),
                    new CodeInstruction(OpCodes.Call, methodToCall)
                ]);
                break;
            }
        }

        return code;
    }
}