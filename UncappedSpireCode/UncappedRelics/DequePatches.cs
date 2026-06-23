using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
using UncappedSpire.UncappedSpireCode.Config;

namespace UncappedSpire.UncappedSpireCode.UncappedRelics;

[HarmonyPatch]
public static class DequePatches
{
    static IEnumerable<MethodBase> TargetMethods()
    {
        return
        [
            AccessTools.Method(typeof(RelicGrabBag), nameof(RelicGrabBag.PullFromFront),
                [typeof(RelicRarity), typeof(Func<RelicModel, bool>), typeof(IRunState)]),
            AccessTools.Method(typeof(RelicGrabBag), nameof(RelicGrabBag.PullFromBack),
                [typeof(RelicRarity), typeof(Func<RelicModel, bool>), typeof(IRunState)]),
        ];
    }

    [HarmonyTranspiler]
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var code = new List<CodeInstruction>(instructions);
        
        var methodToFind = AccessTools.Method(typeof(List<RelicModel>), "RemoveAt");
        var methodToReplace = AccessTools.Method(typeof(DequePatches), nameof(ContextDependantRemoveAt));
        
        for (var i = 0; i < code.Count; i++)
        {
            var instruction = code[i];
            if (instruction.opcode == OpCodes.Callvirt && instruction.operand is MethodInfo method && method == methodToFind)
            {
                code[i] = new CodeInstruction(OpCodes.Call, methodToReplace);
                break;
            }
        }

        return code;
    }

    public static void ContextDependantRemoveAt(List<RelicModel> availableDeque, int i)
    {
        if (!ContextManager.UncappedRelicsEnabled)
        {
            availableDeque.RemoveAt(i);
        }
    }
}