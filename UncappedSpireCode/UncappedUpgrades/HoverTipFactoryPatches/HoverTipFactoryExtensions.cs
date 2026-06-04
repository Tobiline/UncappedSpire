using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using UncappedSpire.UncappedSpireCode.UncappedUpgrades.CardModelPatches;

namespace UncappedSpire.UncappedSpireCode.UncappedUpgrades.HoverTipFactoryPatches;

public static class HoverTipFactoryExtensions
{
    public static IEnumerable<IHoverTip> FromCardWithCardHoverTips<T>(bool upgrade = false, int levelsToUpgrade = 1) where T : CardModel
    {
        return new IHoverTip[1] { FromCard<T>(upgrade, levelsToUpgrade) }.Concat(ModelDb.Card<T>().HoverTips);
    }

    public static IHoverTip FromCard<T>(bool upgrade = false, int levelsToUpgrade = 1) where T : CardModel
    {
        return FromCard(ModelDb.Card<T>(), upgrade, levelsToUpgrade);
    }

    public static IHoverTip FromCard(CardModel card, bool upgrade = false, int levelsToUpgrade = 1)
    {
        if (upgrade)
        {
            card = (CardModel)card.MutableClone();
            card.UpgradeInternal(levelsToUpgrade);
            card.FinalizeUpgradeInternal();
        }
        return new CardHoverTip(card);
    }
}