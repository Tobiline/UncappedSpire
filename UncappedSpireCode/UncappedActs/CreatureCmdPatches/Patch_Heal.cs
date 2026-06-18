using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using UncappedSpire.UncappedSpireCode.Config;

namespace UncappedSpire.UncappedSpireCode.UncappedActs.CreatureCmdPatches;

[HarmonyPatch(typeof(CreatureCmd), nameof(CreatureCmd.Heal))]
public class Patch_Heal
{
    [HarmonyPrefix]
    public static void Prefix(Creature creature, ref decimal amount, bool playAnim)
    {
        if (creature.IsMonster)
        {
            amount *= (decimal)ContextManager.Current_ScalingHp;
        }
    }
}