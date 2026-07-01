using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
using UncappedSpire.UncappedSpireCode.Config;
using UncappedSpire.UncappedSpireCode.Util;

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

    private static FieldInfo Field__deques = AccessTools.Field(typeof(RelicGrabBag), "_deques");
    
    [HarmonyPostfix]
    public static void Postfix(RelicGrabBag __instance)
    {
        if (!ContextManager.UncappedRelicsEnabled)
            return;

        var runRngSet = RunUtil.GetLocalPlayer()!.RunState.Rng;
        var rng = runRngSet.UpFront;
        
        var deques = (Dictionary<RelicRarity, List<RelicModel>>)Field__deques.GetValue(__instance)!;
        foreach (var value2 in deques.Values)
        {
            value2.UnstableShuffle(rng);
        }
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