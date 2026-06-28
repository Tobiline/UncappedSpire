using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using UncappedSpire.UncappedSpireCode.DebugTools;
using UncappedSpire.UncappedSpireCode.UncappedUpgrades.CardModelPatches;

namespace UncappedSpire.UncappedSpireCode.UncappedUpgrades.CardPatches;

[HarmonyPatch(typeof(Claws), "CreateMaulFromOriginal")]
public class MaulTransformPatch
{
    [HarmonyTranspiler]
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var code = new List<CodeInstruction>(instructions);
        
        var methodToFind = AccessTools.Method(typeof(CardModel), "UpgradeInternal");
        var getCurrentUpgradeLevel = AccessTools.PropertyGetter(typeof(CardModel), nameof(CardModel.CurrentUpgradeLevel));
        var safelySetCurrentUpgradeLevel = AccessTools.Method(typeof(CardModelExtensions),
            nameof(CardModelExtensions.SafelySetCurrentUpgradeLevel));
        
        for (var i = 0; i < code.Count; i++)
        {
            var instruction = code[i];
            if (instruction.opcode == OpCodes.Callvirt && instruction.operand is MethodInfo methodInfo && methodInfo == methodToFind)
            {
                var ldOriginal = code[i - 9];
                var ldCardModel = code[i - 1].opcode;
                
                code.InsertRange(i + 3, [
                    new CodeInstruction(ldOriginal),
                    new CodeInstruction(OpCodes.Callvirt, getCurrentUpgradeLevel),
                    new CodeInstruction(OpCodes.Ldc_I4_1),
                    new CodeInstruction(OpCodes.Sub),
                    new CodeInstruction(OpCodes.Call, safelySetCurrentUpgradeLevel),
                    new CodeInstruction(ldCardModel)
                ]);
                
                code.InsertRange(i - 1, [
                    new CodeInstruction(ldCardModel),
                    new CodeInstruction(ldOriginal),
                    new CodeInstruction(OpCodes.Callvirt, getCurrentUpgradeLevel),
                    new CodeInstruction(OpCodes.Ldc_I4_1),
                    new CodeInstruction(OpCodes.Sub),
                    new CodeInstruction(OpCodes.Call, safelySetCurrentUpgradeLevel)
                ]);
                
                break;
            }
        }
        
        code.PrintDebug();
    
        return code;
    }
}