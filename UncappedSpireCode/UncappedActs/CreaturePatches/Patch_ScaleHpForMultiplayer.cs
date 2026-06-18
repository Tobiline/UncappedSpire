using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using UncappedSpire.UncappedSpireCode.Config;

namespace UncappedSpire.UncappedSpireCode.UncappedActs.CreaturePatches;

[HarmonyPatch(typeof(Creature), nameof(Creature.ScaleHpForMultiplayer))]
public class Patch_ScaleHpForMultiplayer
{
    private static MethodInfo Method_set_MaxHp = AccessTools.PropertySetter(typeof(Creature), nameof(Creature.MaxHp));
    
    [HarmonyPrefix]
    public static void Prefix(Creature __instance, ref decimal hp, EncounterModel? encounter, int playerCount, int actIndex)
    {
        hp *= (decimal)ContextManager.Current_ScalingHp;
    }
}