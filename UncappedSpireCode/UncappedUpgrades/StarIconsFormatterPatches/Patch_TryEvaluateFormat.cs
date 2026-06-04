using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using MegaCrit.Sts2.Core.Localization.Formatters;
using SmartFormat.Core.Extensions;

namespace UncappedSpire.UncappedSpireCode.UncappedUpgrades.StarIconsFormatterPatches;

[HarmonyPatch(typeof(StarIconsFormatter), nameof(StarIconsFormatter.TryEvaluateFormat))]
public class Patch_TryEvaluateFormat
{
    [HarmonyTranspiler]
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var code = new List<CodeInstruction>(instructions);
        
        var methodToFind = AccessTools.Method(typeof(IFormattingInfo), nameof(IFormattingInfo.Write), [typeof(string)]);
        var methodToCall = AccessTools.Method(typeof(Utilities), nameof(Utilities.GetFormattedStarIcons));
        
        for (var i = 0; i < code.Count; i++)
        {
            var instruction = code[i];
            if (instruction.opcode == OpCodes.Callvirt && instruction.operand is MethodInfo method && method == methodToFind)
            {
                code.RemoveAt(i - 8);
                i--;
                code.RemoveRange(i - 6, 4);
                i -= 4;
                code.InsertRange(i - 2, [
                    new CodeInstruction(OpCodes.Ldarg_1),
                    new CodeInstruction(OpCodes.Call, methodToCall),
                    new CodeInstruction(OpCodes.Stloc_1),
                ]);
                break;
            }
        }

        return code;
    }
}