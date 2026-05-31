using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;

namespace UncappedSpire.UncappedSpireCode.UncappedUpgrades.CardPatches;

[HarmonyPatch]
public class Patch_Spinner
{
    static MethodBase TargetMethod()
    {
        var m = AccessTools.Method(typeof(Spinner), "OnPlay");
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
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        var code = new List<CodeInstruction>(instructions);
        
        var currentUpgradeGetter = AccessTools.PropertyGetter(typeof(CardModel), "CurrentUpgradeLevel");
        
        var loopStart = generator.DefineLabel();
        var loopCheck = generator.DefineLabel();
        
        var loopCounter = generator.DeclareLocal(typeof(int));
        var condition = generator.DeclareLocal(typeof(bool));

        var loopStartInstruction = new CodeInstruction(OpCodes.Nop);
        loopStartInstruction.labels.Add(loopStart);

        var loopCheckInstruction = new CodeInstruction(OpCodes.Ldloc_S, loopCounter);
        loopCheckInstruction.labels.Add(loopCheck);
            
        for (var i = 0; i < code.Count; i++)
        {
            if (code[i].opcode == OpCodes.Brfalse_S)
            {
                code.InsertRange(i + 1, [
                    new CodeInstruction(OpCodes.Ldc_I4_0),
                    new CodeInstruction(OpCodes.Stloc_S, loopCounter),
                    new CodeInstruction(OpCodes.Br, loopCheck),
                    loopStartInstruction,
                ]);
                
                code.InsertRange(i + 41, [
                    new CodeInstruction(OpCodes.Ldloc_S, loopCounter),
                    new CodeInstruction(OpCodes.Ldc_I4_1),
                    new CodeInstruction(OpCodes.Add),
                    new CodeInstruction(OpCodes.Stloc_S, loopCounter),
                    loopCheckInstruction,
                    new CodeInstruction(OpCodes.Ldloc_1),
                    new CodeInstruction(OpCodes.Callvirt, currentUpgradeGetter),
                    new CodeInstruction(OpCodes.Clt),
                    new CodeInstruction(OpCodes.Stloc_S, condition),
                    new CodeInstruction(OpCodes.Ldloc_S, condition),
                    new CodeInstruction(OpCodes.Brtrue_S, loopStart),
                ]);
                break;
            }
        }

        return code;
    }
}