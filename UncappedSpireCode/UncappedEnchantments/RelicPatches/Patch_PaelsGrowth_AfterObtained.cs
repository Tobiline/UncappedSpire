using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models.Relics;
using UncappedSpire.UncappedSpireCode.Config;

namespace UncappedSpire.UncappedSpireCode.UncappedEnchantments.RelicPatches;

[HarmonyPatch(typeof(PaelsGrowth), "AfterObtained", MethodType.Async)]
public static class Patch_PaelsGrowth_AfterObtained
{
    [HarmonyTranspiler]
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var code = new List<CodeInstruction>(instructions);
        
        var methodToCall = AccessTools.Method(typeof(Patch_PaelsGrowth_AfterObtained), nameof(GetPaelsGrowthAmount));
        
        for (var i = 0; i < code.Count; i++)
        {
            var instruction = code[i];
            if (instruction.opcode == OpCodes.Ldc_I4_4)
            {
                instruction.opcode = OpCodes.Call;
                instruction.operand = methodToCall;
                break;
            }
        }

        return code;
    }

    public static int GetPaelsGrowthAmount()
    {
        if (ContextManager.UncappedEnchantmentsEnabled)
        {
            return 1;
        }

        return 4;
    }
}