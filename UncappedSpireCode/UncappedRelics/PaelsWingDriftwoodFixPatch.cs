using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.CardRewardAlternatives;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Rewards;
using UncappedSpire.UncappedSpireCode.Util;

namespace UncappedSpire.UncappedSpireCode.UncappedRelics;

[HarmonyPatch]
public class PaelsWingDriftwoodFixPatch
{
    static MethodBase TargetMethod()
    {
        return AccessTools.Method(typeof(CardReward), "OnSelect").GetAsyncInnerMethodIfExists();
    }
    
    [HarmonyTranspiler]
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var code = new List<CodeInstruction>(instructions);

        var method1ToFind = AccessTools.Method(typeof(CardRewardAlternative), nameof(CardRewardAlternative.Generate));
        var method2ToFind = AccessTools.Method(typeof(PlayerChoiceSynchronizer), nameof(PlayerChoiceSynchronizer.ReserveChoiceId));
        
        var codeToCopy = new List<CodeInstruction>();
        for (var i = 0; i < code.Count; i++)
        {
            var instruction = code[i];
            if (instruction.opcode == OpCodes.Call && instruction.operand is MethodInfo method1 && method1 == method1ToFind)
            {
                codeToCopy = code.GetRange(i - 2, 4);
            }
            else if (instruction.opcode == OpCodes.Callvirt && instruction.operand is MethodInfo method2 && method2 == method2ToFind)
            {
                code.InsertRange(i - 5, codeToCopy);
                break;
            }
        }

        return code;
    }
}