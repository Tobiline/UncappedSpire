using System.Reflection;
using BaseLib.Abstracts;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Enchantments;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using UncappedSpire.UncappedSpireCode.UncappedEnchantments.EnchantmentModelPatches;

namespace UncappedSpire.UncappedSpireCode.UncappedEnchantments;

/// <summary>
/// This is not to be used as an actual Enchantment!
/// </summary>
public class MultiEnchantment : CustomEnchantmentModel
{
    [SavedProperty] 
    private List<SerializableCard> SerializableEnchantmentsOnCards { get; set; } = [];

    private List<CardModel>? _enchantmentsOnCards;
    public List<CardModel> EnchantmentsOnCards
    {
        get
        {
            if (_enchantmentsOnCards == null)
            {
                var temp_enchantmentsOnCards = SerializableEnchantmentsOnCards.Select(CardModel.FromSerializable).ToList();
                temp_enchantmentsOnCards.ForEach(c => EnchantmentModelAccessUtil.Field__card.SetValue(c.Enchantment, Card));
                _enchantmentsOnCards = temp_enchantmentsOnCards;
            }
            
            return _enchantmentsOnCards;
        }
    }

    public void SetSerializableCards(List<CardModel> cards)
    {
        SerializableEnchantmentsOnCards = cards.Select(card => card.ToSerializable()).ToList();
    }
    
    private static MethodInfo Method_set_Enchantment = AccessTools.PropertySetter(typeof(CardModel), "Enchantment");
    public void AddEnchantment(EnchantmentModel enchantment)
    {
        AssertMutable();
        var enchantmentType = enchantment.GetType();
        var matchingEnchantmentIndex = EnchantmentsOnCards.FindIndex(c => c.Enchantment!.GetType() == enchantmentType);
        var matchingEnchantmentCard = matchingEnchantmentIndex == -1 ? null : EnchantmentsOnCards[matchingEnchantmentIndex];
        var matchingEnchantment = matchingEnchantmentCard?.Enchantment;
        
        if (matchingEnchantment != null && matchingEnchantment.ShowAmount)
        {
            matchingEnchantment.Amount += enchantment.Amount;
            EnchantmentsOnCards[matchingEnchantmentIndex] = matchingEnchantmentCard!;
        }
        else
        {
            var card = ModelDb.Card<EnchantmentStorageCard>().ToMutable();
            Method_set_Enchantment.Invoke(card, [enchantment]);
            EnchantmentsOnCards.Add(card);
        }
        SetSerializableCards(EnchantmentsOnCards);
    }

    protected override void DeepCloneFields()
    {
        base.DeepCloneFields();
        _enchantmentsOnCards = null;
        SerializableEnchantmentsOnCards = [..SerializableEnchantmentsOnCards.Select(c =>
        {
            var card = CardModel.FromSerializable(c);
            var cloned = (CardModel)card.MutableClone();
            return cloned.ToSerializable();
        })];
    }
    
    public override bool ShouldGlowGold => EnchantmentsOnCards.Select(c => c.Enchantment!).Any(e => e.ShouldGlowGold);
    public override bool ShouldGlowRed => EnchantmentsOnCards.Select(c => c.Enchantment!).Any(e => e.ShouldGlowRed);

    protected override string CustomIconPath => "res://UncappedSpire/images/enchantments/uncappedspire-multi_enchantment.png";
    
    // TODO: Find a way to remove the primary hover tip
    protected override IEnumerable<IHoverTip> ExtraHoverTips => EnchantmentsOnCards.Select(c => c.Enchantment!).SelectMany(e => e.HoverTips);
    
    public override bool CanEnchant(CardModel card)
    {
        return true;
    }

    public override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay? cardPlay)
    {
        foreach (var enchantmentCard in EnchantmentsOnCards)
        {
            await enchantmentCard.Enchantment!.OnPlay(choiceContext, cardPlay);
        }
        SetSerializableCards(EnchantmentsOnCards);
    }

    public override void RecalculateValues()
    {
        foreach (var enchantmentCard in EnchantmentsOnCards)
        {
            enchantmentCard.Enchantment!.RecalculateValues();
        }
        SetSerializableCards(EnchantmentsOnCards);
    }

    protected override void OnEnchant()
    {
        foreach (var enchantmentCard in EnchantmentsOnCards)
        {
            EnchantmentModelAccessUtil.Method_OnEnchant.Invoke(enchantmentCard.Enchantment!, null);
        }
        SetSerializableCards(EnchantmentsOnCards);
    }
    
    public override decimal EnchantBlockAdditive(decimal originalBlock, ValueProp props)
    {
        var sum = base.EnchantBlockAdditive(originalBlock, props);
        foreach (var enchantmentCard in EnchantmentsOnCards)
        {
            sum += enchantmentCard.Enchantment!.EnchantBlockAdditive(originalBlock, props);
        }
        SetSerializableCards(EnchantmentsOnCards);
        return sum;
    }
    
    public override decimal EnchantBlockMultiplicative(decimal originalBlock, ValueProp props)
    {
        var product = base.EnchantBlockMultiplicative(originalBlock, props);
        foreach (var enchantmentCard in EnchantmentsOnCards)
        {
            product *= enchantmentCard.Enchantment!.EnchantBlockMultiplicative(originalBlock, props);
        }
        SetSerializableCards(EnchantmentsOnCards);
        return product;
    }

    public override decimal EnchantDamageAdditive(decimal originalDamage, ValueProp props)
    {
        var sum = base.EnchantDamageAdditive(originalDamage, props);
        foreach (var enchantmentCard in EnchantmentsOnCards)
        {
            sum += enchantmentCard.Enchantment!.EnchantDamageAdditive(originalDamage, props);
        }
        SetSerializableCards(EnchantmentsOnCards);
        return sum;
    }
    
    public override decimal EnchantDamageMultiplicative(decimal originalDamage, ValueProp props)
    {
        var product = base.EnchantDamageMultiplicative(originalDamage, props);
        foreach (var enchantmentCard in EnchantmentsOnCards)
        {
            product *= enchantmentCard.Enchantment!.EnchantDamageMultiplicative(originalDamage, props);
        }
        SetSerializableCards(EnchantmentsOnCards);
        return product;
    }
    
    public override int EnchantPlayCount(int originalPlayCount)
    {
        var sum = base.EnchantPlayCount(originalPlayCount);
        foreach (var enchantmentCard in EnchantmentsOnCards)
        {
            sum += enchantmentCard.Enchantment!.EnchantPlayCount(originalPlayCount) - originalPlayCount;
        }
        SetSerializableCards(EnchantmentsOnCards);
        return sum;
    }
    
    // Abstract Model Overrides
    // TODO: Probably best to add all at some point, or even make a base class to handle pipelined AbstractModel methods
    public static MethodInfo Method_set_Card = AccessTools.PropertySetter(typeof(CardPlay), "Card");
    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        foreach (var enchantmentCard in EnchantmentsOnCards)
        {
            //var original = cardPlay.Card;
            //Method_set_Card.Invoke(cardPlay, [enchantmentCard]);
            await enchantmentCard.Enchantment!.AfterCardPlayed(context, cardPlay);
            //Method_set_Card.Invoke(cardPlay, [original]);
        }
        
        foreach (var enchantmentCard in EnchantmentsOnCards)
        {
            MainFile.Logger.Info($"[BEFORE] Enchantment {enchantmentCard.Enchantment!.Title.GetRawText()} is {(enchantmentCard.Enchantment!.Status == EnchantmentStatus.Disabled ? "DISABLED" : "ENABLED")}");
        }

        SetSerializableCards(EnchantmentsOnCards);
    }

    public override async Task BeforePlayPhaseStart(PlayerChoiceContext choiceContext, Player player)
    {
        foreach (var enchantmentCard in EnchantmentsOnCards)
        {
            await enchantmentCard.Enchantment!.BeforePlayPhaseStart(choiceContext, player);
        }
        SetSerializableCards(EnchantmentsOnCards);
    }

    public override void ModifyShuffleOrder(Player player, List<CardModel> cards, bool isInitialShuffle)
    {
        foreach (var enchantmentCard in EnchantmentsOnCards)
        {
            enchantmentCard.Enchantment!.ModifyShuffleOrder(player, cards, isInitialShuffle);
        }
        SetSerializableCards(EnchantmentsOnCards);
    }

    public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        foreach (var enchantmentCard in EnchantmentsOnCards)
        {
            await enchantmentCard.Enchantment!.AfterCardDrawn(choiceContext, card, fromHandDraw);
        }
        SetSerializableCards(EnchantmentsOnCards);
    }
}