using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Runs;

namespace UncappedSpire.UncappedSpireCode.UncappedActs.RunManagerPatches;

[HarmonyPatch]
public class Patch_EnterNextAct
{
    static MethodBase TargetMethod()
    {
        var m = AccessTools.Method(typeof(RunManager), nameof(RunManager.EnterNextAct));
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
    
    public static readonly MethodInfo MethodToFind = AccessTools.Method(typeof(ModelDb), nameof(ModelDb.Event), null, [typeof(TheArchitect)]);
    public static readonly MethodInfo MethodToReplace = AccessTools.Method(typeof(UncappedActsCore), nameof(UncappedActsCore.EnterPostActThreeBossRoom));
    
    [HarmonyTranspiler]
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var code = new List<CodeInstruction>(instructions);
        
        for (var i = 0; i < code.Count; i++)
        {
            var instruction = code[i];
            if (instruction.opcode == OpCodes.Call && instruction.operand is MethodInfo methodInfo && methodInfo == MethodToFind)
            {
                code.RemoveRange(i - 1, 4);
                code.InsertRange(i - 1, [
                    new CodeInstruction(OpCodes.Call, MethodToReplace)
                ]);
                break;
            }
        }

        return code;
    }
}