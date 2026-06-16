using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;

namespace UncappedSpire.UncappedSpireCode.UncappedActs.CreaturePatches;

[HarmonyPatch(typeof(Creature), nameof(Creature.ScaleMonsterHpForMultiplayer))]
public class Patch_ScaleHpForMultiplayer
{
    private static MethodInfo Method_set_MaxHp = AccessTools.PropertySetter(typeof(Creature), nameof(Creature.MaxHp));
    
    [HarmonyPrefix]
    public static void Prefix(Creature __instance, EncounterModel? encounter, int playerCount, int actIndex)
    {
        var scaledHp = (int)(__instance.MaxHp * ChapterManager.Current_ScalingHp);
        Method_set_MaxHp.Invoke(__instance, [scaledHp]);
        __instance.SetCurrentHpInternal(__instance.MaxHp);
    }
}