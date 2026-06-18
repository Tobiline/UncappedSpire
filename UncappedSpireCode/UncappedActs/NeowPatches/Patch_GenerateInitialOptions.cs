using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models.Events;
using UncappedSpire.UncappedSpireCode.Config;

namespace UncappedSpire.UncappedSpireCode.UncappedActs.NeowPatches;

[HarmonyPatch(typeof(Neow), "GenerateInitialOptions")]
public class Patch_GenerateInitialOptions
{
    public static readonly MethodInfo Method_get_Current_Chapter = AccessTools.PropertyGetter(typeof(ContextManager), nameof(ContextManager.Current_Chapter));
    
    [HarmonyTranspiler]
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        var code = new List<CodeInstruction>(instructions);
        
        for (var i = 0; i < code.Count; i++)
        {
            if (code[i].opcode == OpCodes.Ldc_I4_0)
            {
                code[i] = new CodeInstruction(OpCodes.Ldc_I4_1);
                code[i + 1].opcode = OpCodes.Ble;
                
                var ifFalseLabel = code[i + 1].operand;
                var nextStatementLabel = generator.DefineLabel();
                var resultLabel = generator.DefineLabel();
                var resultLocal = generator.DeclareLocal(typeof(bool));
                
                code[i + 1].operand = nextStatementLabel;
                
                var resultFalseInstruction = new CodeInstruction(OpCodes.Ldc_I4_1);
                resultFalseInstruction.labels.Add(nextStatementLabel);
                var setResultLocal = new CodeInstruction(OpCodes.Stloc_S, resultLocal);
                setResultLocal.labels.Add(resultLabel);
                
                code.InsertRange(i + 2, [
                    new CodeInstruction(OpCodes.Call, Method_get_Current_Chapter),
                    new CodeInstruction(OpCodes.Ldc_I4_1),
                    new CodeInstruction(OpCodes.Cgt),
                    new CodeInstruction(OpCodes.Br_S, resultLabel),
                    resultFalseInstruction,
                    setResultLocal,
                    new CodeInstruction(OpCodes.Ldloc_S, resultLocal),
                    new CodeInstruction(OpCodes.Brfalse_S, ifFalseLabel)
                ]);
                
                break;
            }
        }

        return code;
    }
}