using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Models.Monsters;
using UncappedSpire.UncappedSpireCode.Config;

namespace UncappedSpire.UncappedSpireCode.UncappedActs.SpecificMonsterPatches;

[HarmonyPatch(typeof(TestSubject), "Revive")]
public class TestSubjectPatches
{
    [HarmonyPrefix]
    public static bool Prefix(TestSubject __instance, int baseRespawnHp, ref Task __result)
    {
        __result = Replacement(__instance, baseRespawnHp);
        return false;
    }
    
    public static async Task Replacement(TestSubject __instance, int baseRespawnHp)
    {
        __instance.AssertMutable();
        var scaledHp = baseRespawnHp * __instance.Creature.CombatState!.Players.Count;
        await CreatureCmd.SetMaxHp(__instance.Creature, (int)(scaledHp * ContextManager.Current_ScalingHp));
        await CreatureCmd.Heal(__instance.Creature, scaledHp);
    }
}