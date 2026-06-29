using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using UncappedSpire.UncappedSpireCode.DebugTools;

namespace UncappedSpire.UncappedSpireCode.UncappedUpgrades.CardModelPatches;

[HarmonyPatch(typeof(CardModel), nameof(CardModel.DowngradeInternal))]
public class DowngradeInternalPatch
{
    [HarmonyTranspiler]
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var code = new List<CodeInstruction>(instructions);
        
        var currentUpgradeLevelSetter = AccessTools.PropertySetter(typeof(CardModel), nameof(CardModel.CurrentUpgradeLevel));
        var currentUpgradeLevelGetter = AccessTools.PropertyGetter(typeof(CardModel), nameof(CardModel.CurrentUpgradeLevel));
        var upgradeInternalExtension = AccessTools.Method(typeof(CardModelExtensions), nameof(CardModelExtensions.UpgradeInternal));
        var finalizeUpgradeInternal = AccessTools.Method(typeof(CardModel), "FinalizeUpgradeInternal");
        
        for (var i = 0; i < code.Count; i++)
        {
            if (code[i].opcode == OpCodes.Call && code[i].operand is MethodInfo method && method == currentUpgradeLevelSetter)
            {
                var ldThis = code[i + 6].opcode;
                var ldMutable = code[i + 7].opcode;
                
                code.InsertRange(i + 6, [
                    new CodeInstruction(ldMutable),
                    new CodeInstruction(ldThis),
                    new CodeInstruction(OpCodes.Call, currentUpgradeLevelGetter),
                    new CodeInstruction(OpCodes.Call, upgradeInternalExtension),
                    new CodeInstruction(ldMutable),
                    new CodeInstruction(OpCodes.Call, finalizeUpgradeInternal)
                ]);
                
                code.RemoveAt(i - 1);
                
                code.InsertRange(i - 1, [
                    new CodeInstruction(OpCodes.Dup),
                    new CodeInstruction(OpCodes.Call, currentUpgradeLevelGetter),
                    new CodeInstruction(OpCodes.Ldc_I4_1),
                    new CodeInstruction(OpCodes.Sub),
                ]);
                break;
            }
        }

        return code;
    }
}