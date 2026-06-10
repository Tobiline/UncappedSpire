using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace UncappedSpire.UncappedSpireCode.UncappedEnchantments;

/// <summary>
/// This is not to be used as an actual Card!
/// </summary>
[Pool(typeof(ColorlessCardPool))]
public class EnchantmentStorageCard() : CustomCardModel(1, CardType.Attack, CardRarity.Basic, TargetType.Self, false)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play) 
    { }

    protected override void OnUpgrade()
    { }
}