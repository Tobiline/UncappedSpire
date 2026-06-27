using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using HarmonyLib;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using UncappedSpire.UncappedSpireCode.Config;

namespace UncappedSpire.UncappedSpireCode.UncappedActs.PowerModelPatches;

[HarmonyPatch(typeof(PowerModel), nameof(PowerModel.HoverTips), MethodType.Getter)]
public class Patch_HoverTips
{
    public static readonly MethodInfo MethodToFind = AccessTools.Method(typeof(PowerModel), "AddDumbVariablesToDescription");
    public static readonly MethodInfo Method_ModifyDisplayAmount = AccessTools.Method(typeof(Patch_HoverTips), nameof(ModifyDisplayAmount));
    
    [HarmonyTranspiler]
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var code = new List<CodeInstruction>(instructions);
        
        for (var i = 0; i < code.Count; i++)
        {
            var instruction = code[i];
            if (instruction.opcode == OpCodes.Call && instruction.operand is MethodInfo methodInfo && methodInfo == MethodToFind)
            {
                var ldLocString = new CodeInstruction(code[i + 3]);
                var ldPowerModel = new CodeInstruction(code[i + 1]);
                    
                code.InsertRange(i + 5, [
                    ldLocString,
                    ldPowerModel,
                    new CodeInstruction(OpCodes.Call, Method_ModifyDisplayAmount)
                ]);
                break;
            }
        }

        return code;
    }

    public static FieldInfo Field__variables = AccessTools.Field(typeof(LocString), "_variables");
    public static void ModifyDisplayAmount(LocString locString, PowerModel powerModel)
    {
        if (((powerModel.Target != null && powerModel.Target.IsEnemy) || (powerModel.Owner != null && powerModel.Owner.IsEnemy))
            && (powerModel.Applier == null || powerModel.Applier.IsEnemy)
            && powerModel.TryGetScaling(ScalingImplementationType.DisplayModify, out var scaling))
        {
            var variables = (Dictionary<string, object>)Field__variables.GetValue(locString)!;
            foreach (var key in ContextManager.LocStringVariablesToScale)
            {
                if (variables.TryGetValue(key, out var value))
                {
                    if (value is DynamicVar dynamicVar)
                    {
                        value = dynamicVar.BaseValue;
                    }
                    
                    variables[key] = (decimal)value * (decimal)scaling;
                }
            }
            
        }
    }
}