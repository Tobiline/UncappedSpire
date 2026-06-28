using System.Reflection.Emit;
using HarmonyLib;

namespace UncappedSpire.UncappedSpireCode.DebugTools;

public static class DebugUtil
{
    public static void PrintDebug(this IEnumerable<CodeInstruction> instructions)
    {
        foreach (var instruction in instructions)
        {
            MainFile.Logger.Info($"{instruction.labels.FirstOrDefault().Id}: {instruction.opcode} {(instruction.operand is Label label ? label.Id : instruction.operand)}");
        }
    }
}