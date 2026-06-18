using HarmonyLib;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using UncappedSpire.UncappedSpireCode.Config;

namespace UncappedSpire.UncappedSpireCode.UncappedActs.CombatIntentPatches;

[HarmonyPatch(typeof(SingleAttackIntent))]
public class SingleAttackIntentPatches
{
    [HarmonyPatch(MethodType.Constructor, [typeof(int)])]
    [HarmonyPrefix]
    public static void Prefix1(SingleAttackIntent __instance, ref int damage)
    {
        damage = (int)(damage * ContextManager.Current_ScalingDmg);
    }
    
    [HarmonyPatch(MethodType.Constructor, [typeof(Func<decimal>)])]
    [HarmonyPrefix]
    public static void Prefix2(SingleAttackIntent __instance, ref Func<decimal> damageCalc)
    {
        var originalCalc = damageCalc;
        damageCalc = () => originalCalc() * (decimal)ContextManager.Current_ScalingDmg;
    }
}