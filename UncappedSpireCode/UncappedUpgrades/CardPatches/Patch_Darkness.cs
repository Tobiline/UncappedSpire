using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;

namespace UncappedSpire.UncappedSpireCode.UncappedUpgrades.CardPatches;

[HarmonyPatch]
public class Patch_Darkness
{
    static MethodBase TargetMethod()
    {
        var m = AccessTools.Method(typeof(Darkness), "OnPlay");
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
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, MethodBase original, ILGenerator generator)
    {
        var code = new List<CodeInstruction>(instructions);
        
        var methodToFind = AccessTools.PropertyGetter(typeof(CardModel), "IsUpgraded");
        var methodToCall = AccessTools.PropertyGetter(typeof(CardModel), "CurrentUpgradeLevel");
        
        for (var i = 0; i < code.Count; i++)
        {
            if (code[i].opcode == OpCodes.Call && code[i].operand is MethodInfo methodInfo && methodInfo == methodToFind)
            {
                code.RemoveRange(i - 2, 7);
                code.InsertRange(i - 2, [
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldloc_1),
                    new CodeInstruction(OpCodes.Call, methodToCall),
                    new CodeInstruction(OpCodes.Ldc_I4_1),
                    new CodeInstruction(OpCodes.Add)
                ]);
                break;
            }
        }

        return code;
    }
}