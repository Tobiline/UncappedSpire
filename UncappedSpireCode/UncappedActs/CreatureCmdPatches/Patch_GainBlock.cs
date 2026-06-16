using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.ValueProps;

namespace UncappedSpire.UncappedSpireCode.UncappedActs.CreatureCmdPatches;

[HarmonyPatch(typeof(CreatureCmd), nameof(CreatureCmd.GainBlock), [
    typeof(Creature), typeof(decimal), typeof(ValueProp),
    typeof(CardPlay), typeof(bool)
])]
public class Patch_GainBlock
{
    [HarmonyPrefix]
    public static void Prefix(Creature creature, ref decimal amount, ValueProp props, CardPlay? cardPlay, bool fast)
    {
        if (creature.IsMonster)
        {
            amount *= (decimal)ChapterManager.Current_ScalingHp;
        }
    }
}