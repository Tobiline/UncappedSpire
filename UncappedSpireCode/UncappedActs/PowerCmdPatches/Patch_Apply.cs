using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using UncappedSpire.UncappedSpireCode.Config;

namespace UncappedSpire.UncappedSpireCode.UncappedActs.PowerCmdPatches;

[HarmonyPatch(typeof(PowerCmd), nameof(PowerCmd.Apply), [
    typeof(PowerModel), typeof(Creature), typeof(decimal),
    typeof(Creature), typeof(CardModel), typeof(bool)
])]
public class Patch_Apply
{
    [HarmonyPrefix]
    public static void Prefix(PowerModel power, Creature target, ref decimal amount, Creature? applier, CardModel? cardSource, bool silent = false)
    {
        if (((target != null && target.IsMonster) || (power.Owner != null && power.Owner.IsMonster))
            && (applier == null || applier.IsMonster)
            && power.TryGetScaling(ScalingImplementationType.DataModify, out var scaling))
        {
            amount *= (decimal)scaling;
        }
        else if (applier != null && applier.IsMonster 
            && power.TryGetScaling(ScalingImplementationType.NonSelfAppliedDataModify, out var scaling2))
        {
            amount *= (decimal)scaling2;
        }
    }
}