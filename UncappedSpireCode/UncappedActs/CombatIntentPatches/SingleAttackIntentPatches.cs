using HarmonyLib;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;

namespace UncappedSpire.UncappedSpireCode.UncappedActs.CombatIntentPatches;

[HarmonyPatch(typeof(SingleAttackIntent))]
public class SingleAttackIntentPatches
{
    [HarmonyPatch(MethodType.Constructor, [typeof(int)])]
    [HarmonyPrefix]
    public static void Prefix1(SingleAttackIntent __instance, ref int damage)
    {
        damage = (int)(damage * ChapterManager.Session_ScalingDmg);
    }
    
    [HarmonyPatch(MethodType.Constructor, [typeof(Func<decimal>)])]
    [HarmonyPrefix]
    public static void Prefix2(SingleAttackIntent __instance, Func<decimal> damageCalc)
    {
        damageCalc = () => damageCalc() * (decimal)ChapterManager.Session_ScalingDmg;
    }
}