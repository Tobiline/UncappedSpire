using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Nodes.Cards;

namespace UncappedSpire.UncappedSpireCode.UncappedEnchantments.UI;

public partial class NEnchantmentTabContainer : HBoxContainer
{
	private static readonly string _scenePath = "res://UncappedSpireCode/UncappedEnchantments/UI/enchantment_tab_container.tscn";
	
	public static AddedNode<NCard, NEnchantmentTabContainer>? Node = new(_scenePath,
		(parent, node) =>
		{
			var cardContainer = parent.GetNode<Control>("%CardContainer");
			cardContainer.AddChild(node);
			
			var enchantment = parent.GetNode<TextureRect>("%Enchantment");
			enchantment.CustomMinimumSize = new Vector2(
				enchantment.OffsetRight - enchantment.OffsetLeft,
				enchantment.OffsetBottom - enchantment.OffsetTop);
			cardContainer.RemoveChild(enchantment);

			var containerLeft = (VBoxContainer)node.FindChild("LeftContainer");
			var containerRight = (VBoxContainer)node.FindChild("RightContainer");
			
			SpireField__enchantmentTabContainer._enchantmentTabContainer.Set(parent, node);
			SpireField__enchantmentTabContainer._enchantmentTabContainerLeft.Set(parent, containerLeft);
			SpireField__enchantmentTabContainer._enchantmentTabContainerRight.Set(parent, containerRight);
		});
}
