using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Enchantments;

namespace UncappedSpire.UncappedSpireCode.UncappedEnchantments;

[HarmonyPatch(typeof(Glam), "AfterCardPlayed")]
public static class Debug
{
    private static readonly MethodInfo Method_get_UsedThisCombat = AccessTools.PropertyGetter(typeof(Glam), "UsedThisCombat");
    
    [HarmonyPrefix]
    public static void Prefix(Glam __instance, PlayerChoiceContext context, CardPlay cardPlay)
    {
        //MainFile.Logger.Info($"UsedThisCombat? {Method_get_UsedThisCombat.Invoke(__instance, null)}, cardPlayCard: {cardPlay.Card.Title}, GlamCard: {__instance.Card.Title}");
    }
}

[HarmonyPatch(typeof(Vigorous), "AfterCardPlayed")]
public static class Debug_Vig
{
    private static readonly MethodInfo Method_get_UsedThisCombat = AccessTools.PropertyGetter(typeof(Vigorous), "UsedThisCombat");
    
    [HarmonyPrefix]
    public static void Prefix(Vigorous __instance, PlayerChoiceContext context, CardPlay cardPlay)
    {
        //MainFile.Logger.Info($"UsedThisCombat? {Method_get_UsedThisCombat?.Invoke(__instance, null)}, cardPlayCard: {cardPlay?.Card.Title}, VigorousCard: {__instance?.Card.Title}");
    }
}