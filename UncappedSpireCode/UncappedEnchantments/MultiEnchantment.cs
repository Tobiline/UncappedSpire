using System.Reflection;
using BaseLib.Abstracts;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace UncappedSpire.UncappedSpireCode.UncappedEnchantments;

/// <summary>
/// This is not to be used as an actual Enchantment!
/// </summary>
public class MultiEnchantment : CustomEnchantmentModel
{
    [SavedProperty] 
    public List<SerializableCard> SerializableEnchantmentOnCards { get; } = [];
    
    public List<EnchantmentModel> GetEnchantments()
    {
        return SerializableEnchantmentOnCards.Select(c => CardModel.FromSerializable(c).Enchantment).ToList()!;
    }
    
    private static MethodInfo Method_set_Enchantment = AccessTools.PropertySetter(typeof(CardModel), "Enchantment");
    public void AddEnchantment(EnchantmentModel enchantment)
    {
        AssertMutable();
        var enchantmentType = enchantment.GetType();
        var matchingEnchantmentIndex = SerializableEnchantmentOnCards.ToList().FindIndex(c => c.Enchantment!.GetType() == enchantmentType);
        var matchingEnchantmentCard = matchingEnchantmentIndex == -1 ? null : CardModel.FromSerializable(SerializableEnchantmentOnCards[matchingEnchantmentIndex]);
        var matchingEnchantment = matchingEnchantmentCard?.Enchantment;
        if (matchingEnchantment != null && matchingEnchantment.ShowAmount)
        {
            matchingEnchantment.Amount += enchantment.Amount;
            SerializableEnchantmentOnCards[matchingEnchantmentIndex] = matchingEnchantmentCard!.ToSerializable();
        }
        else
        {
            var card = ModelDb.Card<EnchantmentStorageCard>().ToMutable();
            Method_set_Enchantment.Invoke(card, [enchantment]);
            SerializableEnchantmentOnCards.Add(card.ToSerializable());
        }
    }
    
    public override bool ShouldGlowGold => GetEnchantments().Any(e => e.ShouldGlowGold);
    public override bool ShouldGlowRed => GetEnchantments().Any(e => e.ShouldGlowRed);

    protected override string CustomIconPath => "res://UncappedSpire/images/enchantments/uncappedspire-multi_enchantment.png";
    
    // TODO: Find a way to remove the primary hover tip
    protected override IEnumerable<IHoverTip> ExtraHoverTips => GetEnchantments().SelectMany(e => e.HoverTips);
    
    public override bool CanEnchant(CardModel card)
    {
        return true;
    }

    public override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay? cardPlay)
    {
        foreach (var enchantment in GetEnchantments())
        {
            await enchantment.OnPlay(choiceContext, cardPlay);
        }
    }

    public override void RecalculateValues()
    {
        foreach (var enchantment in GetEnchantments())
        {
            enchantment.RecalculateValues();
        }
    }

    protected override void OnEnchant()
    {
        foreach (var enchantment in GetEnchantments())
        {
            EnchantmentModelAccessUtil.Method_OnEnchant.Invoke(enchantment, null);
        }
    }
    
    public override decimal EnchantBlockAdditive(decimal originalBlock, ValueProp props)
    {
        var sum = base.EnchantBlockAdditive(originalBlock, props);
        foreach (var enchantment in GetEnchantments())
        {
            sum += enchantment.EnchantBlockAdditive(originalBlock, props);
        }
        return sum;
    }
    
    public override decimal EnchantBlockMultiplicative(decimal originalBlock, ValueProp props)
    {
        var product = base.EnchantBlockMultiplicative(originalBlock, props);
        foreach (var enchantment in GetEnchantments())
        {
            product *= enchantment.EnchantBlockMultiplicative(originalBlock, props);
        }
        return product;
    }

    public override decimal EnchantDamageAdditive(decimal originalDamage, ValueProp props)
    {
        var sum = base.EnchantDamageAdditive(originalDamage, props);
        foreach (var enchantment in GetEnchantments())
        {
            sum += enchantment.EnchantDamageAdditive(originalDamage, props);
        }
        return sum;
    }
    
    public override decimal EnchantDamageMultiplicative(decimal originalDamage, ValueProp props)
    {
        var product = base.EnchantDamageMultiplicative(originalDamage, props);
        foreach (var enchantment in GetEnchantments())
        {
            product *= enchantment.EnchantDamageMultiplicative(originalDamage, props);
        }
        return product;
    }
    
    public override int EnchantPlayCount(int originalPlayCount)
    {
        var sum = base.EnchantPlayCount(originalPlayCount);
        foreach (var enchantment in GetEnchantments())
        {
            sum += enchantment.EnchantPlayCount(originalPlayCount) - originalPlayCount;
        }
        return sum;
    }
    
    // Abstract Model Overrides
    // TODO: Probably best to add all at some point, or even make a base class to handle pipelined AbstractModel methods
    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        foreach (var enchantment in GetEnchantments())
        {
            await enchantment.AfterCardPlayed(context, cardPlay);
        }
    }

    public override async Task BeforePlayPhaseStart(PlayerChoiceContext choiceContext, Player player)
    {
        foreach (var enchantment in GetEnchantments())
        {
            await enchantment.BeforePlayPhaseStart(choiceContext, player);
        }
    }

    public override void ModifyShuffleOrder(Player player, List<CardModel> cards, bool isInitialShuffle)
    {
        foreach (var enchantment in GetEnchantments())
        {
            enchantment.ModifyShuffleOrder(player, cards, isInitialShuffle);
        }
    }

    public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        foreach (var enchantment in GetEnchantments())
        {
            await enchantment.AfterCardDrawn(choiceContext, card, fromHandDraw);
        }
    }
}