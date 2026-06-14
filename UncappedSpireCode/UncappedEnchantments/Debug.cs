using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Enchantments;

namespace UncappedSpire.UncappedSpireCode.UncappedEnchantments;

[HarmonyPatch(typeof(Goopy), "AfterCardPlayed")]
public static class Debug
{
    //private static readonly MethodInfo Method_get_UsedThisCombat = AccessTools.PropertyGetter(typeof(Goopy), "UsedThisCombat");
    
    [HarmonyPrefix]
    public static void Prefix(Goopy __instance, PlayerChoiceContext context, CardPlay cardPlay)
    {
        MainFile.Logger.Info($"Cards Equal?: {cardPlay.Card.Title == __instance.Card.Title}");
        // MainFile.Logger.Info("DeckVersion: " + (__instance.Card.DeckVersion != null ? "HAS IT!" : " Does not... :("));
        // if (__instance.Card.DeckVersion != null)
        // {
        //     MainFile.Logger.Info($"DeckVersion Card: {__instance.Card.DeckVersion.Title}");
        //     MainFile.Logger.Info($"DeckVersion Enchantment: {__instance.Card.DeckVersion.Enchantment!.Title.GetRawText()}");
        // }
    }
}