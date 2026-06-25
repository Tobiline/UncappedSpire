using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Nodes.Cards;

namespace UncappedSpire.UncappedSpireCode.UncappedEnchantments.UI;

public class SpireField__enchantmentTabContainer
{
    public static readonly SpireField<NCard, NEnchantmentTabContainer> _enchantmentTabContainer = new(() => new());
    public static readonly SpireField<NCard, VBoxContainer> _enchantmentTabContainerLeft = new(() => new());
    // TODO: Make using this default, but optional
    public static readonly SpireField<NCard, VBoxContainer> _enchantmentTabContainerRight = new(() => new());
}