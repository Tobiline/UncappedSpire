using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;

namespace UncappedSpire.UncappedSpireCode.UncappedEnchantments.Deck;

public static class DeckExtensions
{
    public static IEnumerable<CardModel> GetCloneable(this CardPile deck)
    {
        return deck.Cards.Where(c => c.Enchantment != null && ((MultiEnchantment)c.Enchantment!).EnchantmentsOnCards
            .Any(ec => ec.Enchantment is Clone)).ToList();
    }
}