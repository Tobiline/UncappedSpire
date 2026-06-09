using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using UncappedSpire.UncappedSpireCode.UncappedEnchantments.EnchantmentModelPatches;

namespace UncappedSpire.UncappedSpireCode.UncappedEnchantments;

/// <summary>
/// This is not to be used as an actual Enchantment!
/// </summary>
public class MultiEnchantment : CustomEnchantmentModel
{
    public override bool ShouldGlowGold => SpireField_Enchantments.Enchantments[this]!.Any(e => e.ShouldGlowGold);
    public override bool ShouldGlowRed => SpireField_Enchantments.Enchantments[this]!.Any(e => e.ShouldGlowRed);

    protected override string CustomIconPath => "res://UncappedSpire/images/enchantments/uncappedspire-multi_enchantment.png";
    
    // TODO: Find a way to remove the primary hover tip
    protected override IEnumerable<IHoverTip> ExtraHoverTips => SpireField_Enchantments.Enchantments[this]!.SelectMany(e => e.HoverTips);
    
    public override bool CanEnchant(CardModel card)
    {
        return true;
    }

    public override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay? cardPlay)
    {
        foreach (var enchantment in SpireField_Enchantments.Enchantments[this]!)
        {
            await enchantment.OnPlay(choiceContext, cardPlay);
        }
    }

    public override void RecalculateValues()
    {
        foreach (var enchantment in SpireField_Enchantments.Enchantments[this]!)
        {
            enchantment.RecalculateValues();
        }
    }

    protected override void OnEnchant()
    {
        foreach (var enchantment in SpireField_Enchantments.Enchantments[this]!)
        {
            EnchantmentModelAccessUtil.Method_OnEnchant.Invoke(enchantment, null);
        }
    }
    
    public override decimal EnchantBlockAdditive(decimal originalBlock, ValueProp props)
    {
        var sum = base.EnchantBlockAdditive(originalBlock, props);
        foreach (var enchantment in SpireField_Enchantments.Enchantments[this]!)
        {
            sum += enchantment.EnchantBlockAdditive(originalBlock, props);
        }
        return sum;
    }
    
    public override decimal EnchantBlockMultiplicative(decimal originalBlock, ValueProp props)
    {
        var product = base.EnchantBlockMultiplicative(originalBlock, props);
        foreach (var enchantment in SpireField_Enchantments.Enchantments[this]!)
        {
            product *= enchantment.EnchantBlockMultiplicative(originalBlock, props);
        }
        return product;
    }

    public override decimal EnchantDamageAdditive(decimal originalDamage, ValueProp props)
    {
        var sum = base.EnchantDamageAdditive(originalDamage, props);
        foreach (var enchantment in SpireField_Enchantments.Enchantments[this]!)
        {
            sum += enchantment.EnchantDamageAdditive(originalDamage, props);
        }
        return sum;
    }
    
    public override decimal EnchantDamageMultiplicative(decimal originalDamage, ValueProp props)
    {
        var product = base.EnchantDamageMultiplicative(originalDamage, props);
        foreach (var enchantment in SpireField_Enchantments.Enchantments[this]!)
        {
            product *= enchantment.EnchantDamageMultiplicative(originalDamage, props);
        }
        return product;
    }
    
    public override int EnchantPlayCount(int originalPlayCount)
    {
        var sum = base.EnchantPlayCount(originalPlayCount);
        foreach (var enchantment in SpireField_Enchantments.Enchantments[this]!)
        {
            sum += enchantment.EnchantPlayCount(originalPlayCount) - originalPlayCount;
        }
        return sum;
    }
    
    // Abstract Model Overrides
    // TODO: Probably best to add all at some point, or even make a base class to handle pipelined AbstractModel methods
    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        foreach (var enchantment in SpireField_Enchantments.Enchantments[this]!)
        {
            await enchantment.AfterCardPlayed(context, cardPlay);
        }
    }

    public override async Task BeforePlayPhaseStart(PlayerChoiceContext choiceContext, Player player)
    {
        foreach (var enchantment in SpireField_Enchantments.Enchantments[this]!)
        {
            await enchantment.BeforePlayPhaseStart(choiceContext, player);
        }
    }

    public override void ModifyShuffleOrder(Player player, List<CardModel> cards, bool isInitialShuffle)
    {
        foreach (var enchantment in SpireField_Enchantments.Enchantments[this]!)
        {
            enchantment.ModifyShuffleOrder(player, cards, isInitialShuffle);
        }
    }

    public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        foreach (var enchantment in SpireField_Enchantments.Enchantments[this]!)
        {
            await enchantment.AfterCardDrawn(choiceContext, card, fromHandDraw);
        }
    }
}