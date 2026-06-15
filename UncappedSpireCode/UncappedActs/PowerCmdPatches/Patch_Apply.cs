using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;

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
        if (target.IsMonster && applier != null && applier.IsMonster)
        {
            var powerType = power.GetType();
            if (ChapterManager.MonsterScalingPowers.TryGetValue(powerType, out var scalingType))
            {
                amount *= (decimal)ChapterManager.GetScaling(scalingType);
            }
        }
    }
}