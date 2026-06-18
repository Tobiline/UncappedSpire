using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands.Builders;
using UncappedSpire.UncappedSpireCode.Config;

namespace UncappedSpire.UncappedSpireCode.UncappedActs.CombatCmdPatches;

[HarmonyPatch]
public class AttackCommandPatches
{
    public static readonly FieldInfo Field__damagePerHit = AccessTools.Field(typeof(AttackCommand), "_damagePerHit");
    
    [HarmonyPatch(typeof(AttackCommand), nameof(AttackCommand.FromMonster))]
    [HarmonyPrefix]
    public static void Prefix(AttackCommand __instance)
    {
        var damagePerHit = (decimal)Field__damagePerHit.GetValue(__instance)!;
        Field__damagePerHit.SetValue(__instance, damagePerHit * (decimal)ContextManager.Current_ScalingDmg);
    }
}