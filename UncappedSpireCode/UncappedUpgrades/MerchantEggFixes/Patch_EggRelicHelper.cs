using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Helpers.Models;
using MegaCrit.Sts2.Core.Models;

namespace UncappedSpire.UncappedSpireCode.UncappedUpgrades.MerchantEggFixes;

[HarmonyPatch(nameof(EggRelicHelper), nameof(EggRelicHelper.UpgradeValidCards))]
public class Patch_EggRelicHelper
{
    [HarmonyPrefix]
    public static void Prefix(ref List<CardCreationResult> cards, CardType cardType, RelicModel eggRelic)
    {
        var cardsThatHaveAlreadyBeenModified = cards.Where(card => !card.ModifyingRelics.Contains(eggRelic)).ToList();
        cards = cardsThatHaveAlreadyBeenModified;
    }
}