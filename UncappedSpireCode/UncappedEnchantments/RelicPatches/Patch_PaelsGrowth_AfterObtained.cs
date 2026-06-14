using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models.Relics;

namespace UncappedSpire.UncappedSpireCode.UncappedEnchantments.RelicPatches;

[HarmonyPatch]
public class Patch_PaelsGrowth_AfterObtained
{
    static MethodBase TargetMethod()
    {
        var m = AccessTools.Method(typeof(PaelsGrowth), "AfterObtained");
        var attr = m.GetCustomAttribute<AsyncStateMachineAttribute>();
        if (attr == null)
        {
            throw new NullReferenceException("OnPlay AsyncStateMachineAttribute attribute not found");
        }
        var moveNextMethod = attr.StateMachineType.GetMethod("MoveNext", BindingFlags.NonPublic | BindingFlags.Instance);
        if (moveNextMethod == null)
        {
            throw new NullReferenceException("AsyncStateMachineAttribute state machine method not found");
        }
        return moveNextMethod;
    }
    
    [HarmonyTranspiler]
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var code = new List<CodeInstruction>(instructions);
        
        for (var i = 0; i < code.Count; i++)
        {
            var instruction = code[i];
            if (instruction.opcode == OpCodes.Ldc_I4_4)
            {
                instruction.opcode = OpCodes.Ldc_I4_1;
                break;
            }
        }

        return code;
    }
}