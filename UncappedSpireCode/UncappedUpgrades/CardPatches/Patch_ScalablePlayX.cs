using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models.Cards;

namespace UncappedSpire.UncappedSpireCode.UncappedUpgrades.CardPatches;

[HarmonyPatch]
public class Patch_ScalablePlayX
{
    static readonly Type[] Targets =
    [
        typeof(Cascade),
        typeof(Malaise),
        typeof(MultiCast),
        typeof(Tempest)
    ];
    
    static IEnumerable<MethodBase> TargetMethods()
    {
        foreach (var target in Targets)
        {
            var m = AccessTools.Method(target, "OnPlay");
            var attr = m.GetCustomAttribute<AsyncStateMachineAttribute>();
            if (attr == null)
            {
                MainFile.Logger.Error($"OnPlay AsyncStateMachineAttribute attribute not found for {target.Name}");
                continue;
            }
            var moveNextMethod = attr.StateMachineType.GetMethod("MoveNext", BindingFlags.NonPublic | BindingFlags.Instance);
            if (moveNextMethod == null)
            {
                MainFile.Logger.Error($"AsyncStateMachineAttribute state machine method not found for {target.Name}");
                continue;
            }
            
            yield return moveNextMethod;
        }
    }
    
    [HarmonyTranspiler]
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
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