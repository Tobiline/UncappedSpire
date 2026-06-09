using BaseLib.Utils;
using MegaCrit.Sts2.Core.Models;
using UncappedSpire.UncappedSpireCode.Util;

namespace UncappedSpire.UncappedSpireCode.UncappedEnchantments.CardModelPatches;

public class SpireField__multiEnchantment
{
    public static readonly SpireField<CardModel, MultiEnchantment> _multiEnchantment = new(() => null);
    public static readonly SpireField<CardModel, EventContainer> EnchantmentsChanged = new(() => new());
}