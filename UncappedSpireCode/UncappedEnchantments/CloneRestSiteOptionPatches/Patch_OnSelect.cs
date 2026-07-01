using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using UncappedSpire.UncappedSpireCode.Config;

namespace UncappedSpire.UncappedSpireCode.UncappedEnchantments.CloneRestSiteOptionPatches;

[HarmonyPatch(typeof(CloneRestSiteOption), nameof(CloneRestSiteOption.OnSelect))]
public class Patch_OnSelect
{
    public static MethodInfo Method_get_Owner = AccessTools.PropertyGetter(typeof(RestSiteOption), "Owner");
    
    [HarmonyPrefix]
    public static bool Prefix(CloneRestSiteOption __instance, ref Task<bool> __result)
    {
        if (ContextManager.UncappedEnchantmentsEnabled)
        {
            __result = Replacement(__instance);
            return false;
        }

        return true;
    }

    public static async Task<bool> Replacement(CloneRestSiteOption __instance)
    {
        IEnumerable<CardModel> enumerable = ((Player)Method_get_Owner.Invoke(__instance, null)!).Deck.Cards
            .Where(c => c.Enchantment != null && ((MultiEnchantment)c.Enchantment!).EnchantmentsOnCards
                .Any(ec => ec.Enchantment is Clone)).ToList();
        var results = new List<CardPileAddResult>();
        foreach (var item in enumerable)
        {
            var cloneAmount = ((MultiEnchantment)item.Enchantment!).EnchantmentsOnCards.Count(c => c.Enchantment is Clone);

            for (var i = 0; i < cloneAmount; i++)
            {
                var card = ((Player)Method_get_Owner.Invoke(__instance, null)!).RunState.CloneCard(item);
                var list = results;
                list.Add(await CardPileCmd.Add(card, PileType.Deck));
            }
        }
        CardCmd.PreviewCardPileAdd(results, 1.2f, CardPreviewStyle.MessyLayout);
        return true;
    }
}