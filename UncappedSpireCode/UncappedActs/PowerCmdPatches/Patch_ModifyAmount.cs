using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;

namespace UncappedSpire.UncappedSpireCode.UncappedActs.PowerCmdPatches;

[HarmonyPatch(typeof(PowerCmd), nameof(PowerCmd.ModifyAmount))]
public class Patch_ModifyAmount
{
    [HarmonyPrefix]
    static void Prefix(PowerModel power, ref decimal offset, Creature? applier, CardModel? cardSource, bool silent)
    {
        if (power.Owner.IsMonster && applier != null && applier.IsMonster)
        {
            var powerType = power.GetType();
            if (ChapterManager.MonsterScalingPowers.TryGetValue(powerType, out var scalingType))
            {
                offset *= (decimal)ChapterManager.GetScaling(scalingType);
            }
        }
    }
}