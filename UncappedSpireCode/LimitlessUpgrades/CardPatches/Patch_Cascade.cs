using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models.Cards;

namespace UncappedSpire.UncappedSpireCode.LimitlessUpgrades.CardPatches;

[HarmonyPatch]
public class Patch_Cascade
{
    static MethodBase TargetMethod()
    {
        var m = AccessTools.Method(typeof(Cascade), "OnPlay");
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
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, MethodBase original)
    {
        var code = new List<CodeInstruction>(instructions);
        var currentUpgradeLevelField = AccessTools.Field(typeof(Cascade), "_currentUpgradeLevel");
        
        for (var i = 0; i < code.Count; i++)
        {
            if (code[i].opcode == OpCodes.Ldc_I4_1)
            {
                code[i] = new CodeInstruction(OpCodes.Ldloc_1);
                code.Insert(i + 1, new CodeInstruction(OpCodes.Ldfld, currentUpgradeLevelField));
                break;
            }
        }

        return code;
    }
}