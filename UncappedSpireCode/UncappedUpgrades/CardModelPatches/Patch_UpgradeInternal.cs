using System.Reflection.Emit;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;

namespace UncappedSpire.UncappedSpireCode.UncappedUpgrades.CardModelPatches;

[HarmonyPatch(typeof(CardModel), "UpgradeInternal")]
public class Patch_UpgradeInternal
{
    [HarmonyTranspiler]
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var code = new List<CodeInstruction>(instructions);
        
        var methodToCall =
            AccessTools.Method(typeof(UpgradeContext), nameof(UpgradeContext.GetMultiplier));
        
        for (var i = 0; i < code.Count; i++)
        {
            if (code[i].opcode == OpCodes.Ldc_I4_1)
            {
                code[i] = new CodeInstruction(OpCodes.Call, methodToCall);
                break;
            }
        }

        return code;
    }
}