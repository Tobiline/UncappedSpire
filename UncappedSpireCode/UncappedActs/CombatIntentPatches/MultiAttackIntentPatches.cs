using HarmonyLib;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;

namespace UncappedSpire.UncappedSpireCode.UncappedActs.CombatIntentPatches;

[HarmonyPatch(typeof(MultiAttackIntent))]
public class MultiAttackIntentPatches
{
    [HarmonyPatch(MethodType.Constructor, [typeof(int), typeof(int)])]
    [HarmonyPrefix]
    public static void Prefix1(MultiAttackIntent __instance, ref int damage, int repeat)
    {
        damage = (int)(damage * ChapterManager.Current_ScalingDmg);
    }
    
    [HarmonyPatch(MethodType.Constructor, [typeof(int), typeof(Func<int>)])]
    [HarmonyPrefix]
    public static void Prefix2(MultiAttackIntent __instance, ref int damage, Func<int> repeatCalc)
    {
        damage = (int)(damage * ChapterManager.Current_ScalingDmg);
    }
}