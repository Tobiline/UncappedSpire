using System.Reflection;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using UncappedSpire.UncappedSpireCode.UncappedUpgrades.CardModelPatches;

namespace UncappedSpire.UncappedSpireCode.UncappedUpgrades.GridCardHolderPatches;

public static class GridCardHolderExtensions
{
    private static readonly FieldInfo Field__upgradedCard = HarmonyLib.AccessTools.Field(typeof(NGridCardHolder), "_upgradedCard");
    
    public static void UpdateUpgradedCard(this NGridCardHolder nGridCardHolder)
    {
        var _upgradedCard = Field__upgradedCard.GetValue(nGridCardHolder) as CardModel;
        if (_upgradedCard == null)
        {
            return;
        }
        _upgradedCard.UpgradeToInternal(UpgradeContext.GetMultiplier());
    }
}