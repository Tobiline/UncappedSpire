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
        if (((power.Target != null && power.Target.IsMonster) || (power.Owner != null && power.Owner.IsMonster))
            && (applier == null || applier.IsMonster)
            && power.TryGetScaling(ScalingImplementationType.DataModify, out var scaling))
        {
            offset *= (decimal)scaling;
        }
        else if (applier != null && applier.IsMonster 
            && power.TryGetScaling(ScalingImplementationType.NonSelfAppliedDataModify, out var scaling2))
        {
            offset *= (decimal)scaling2;
        }
    }
}