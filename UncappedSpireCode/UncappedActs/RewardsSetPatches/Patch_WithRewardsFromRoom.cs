using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using MegaCrit.Sts2.Core.Rewards;

namespace UncappedSpire.UncappedSpireCode.UncappedActs.RewardsSetPatches;

[HarmonyPatch(typeof(RewardsSet), nameof(RewardsSet.WithRewardsFromRoom))]
public class Patch_WithRewardsFromRoom
{
    public static MethodInfo Method_AddFinalBossRewards = AccessTools.Method(typeof(UncappedActsCore), nameof(UncappedActsCore.AddFinalBossRewards));
    
    [HarmonyTranspiler]
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var code = new List<CodeInstruction>(instructions);
        
        for (var i = 0; i < code.Count; i++)
        {
            var instruction = code[i];
            if (instruction.opcode == OpCodes.Ret)
            {
                var ldRewardSet = code[i - 1].opcode;
                
                code.RemoveRange(i - 1, 2);
                code.InsertRange(i - 1, [
                    new CodeInstruction(ldRewardSet),
                    new CodeInstruction(OpCodes.Call, Method_AddFinalBossRewards)
                ]);
                break;
            }
        }

        return code;
    }
}