using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using HarmonyLib;
using MegaCrit.Sts2.Core.Runs;
using UncappedSpire.UncappedSpireCode.Config;

namespace UncappedSpire.UncappedSpireCode.UncappedActs.RunManagerPatches;

[HarmonyPatch]
public class Patch_EnterAct
{
    static MethodBase TargetMethod()
    {
        var m = AccessTools.Method(typeof(RunManager), nameof(RunManager.EnterAct));
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
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var code = new List<CodeInstruction>(instructions);
        
        var methodToFind = AccessTools.PropertyGetter(typeof(ExtraRunFields), nameof(ExtraRunFields.StartedWithNeow));
        var methodToCall = AccessTools.PropertyGetter(typeof(ContextManager), nameof(ContextManager.Current_Chapter));
        
        for (var i = 0; i < code.Count; i++)
        {
            if (code[i].opcode == OpCodes.Callvirt && code[i].operand is MethodInfo methodInfo && methodInfo == methodToFind)
            {
                var jumpLabel = code[i + 1].operand;
                
                code.InsertRange(i + 2, [
                    new CodeInstruction(OpCodes.Call, methodToCall),
                    new CodeInstruction(OpCodes.Ldc_I4_1),
                    new CodeInstruction(OpCodes.Cgt),
                    new CodeInstruction(OpCodes.Brtrue, jumpLabel)
                ]);
                break;
            }
        }

        return code;
    }
}