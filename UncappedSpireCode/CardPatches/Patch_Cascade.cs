using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models.Cards;

namespace UncappedSpire.UncappedSpireCode.CardPatches;

[HarmonyPatch]
public class Patch_Cascade
{
    static MethodBase TargetMethod()
    {
        var m = AccessTools.Method(typeof(Cascade), "OnPlay");
        var attr = m.GetCustomAttribute<AsyncStateMachineAttribute>();
        return attr.StateMachineType.GetMethod("MoveNext", BindingFlags.NonPublic | BindingFlags.Instance);
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