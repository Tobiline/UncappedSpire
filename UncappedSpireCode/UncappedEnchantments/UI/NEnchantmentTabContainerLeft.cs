// using BaseLib.Utils;
// using Godot;
// using MegaCrit.Sts2.Core.Nodes.Cards;
//
// namespace UncappedSpire.UncappedSpireCode.UncappedEnchantments.UI;
//
// public partial class NEnchantmentTabContainerLeft : VBoxContainer
// {
// 	private static readonly string _scenePath = "res://UncappedSpireCode/UncappedEnchantments/UI/enchantment_tab_container_left.tscn";
// 	
// 	public static AddedNode<NCard, NEnchantmentTabContainerLeft>? Node = new(_scenePath,
// 		(parent, node) =>
// 		{
// 			var cardContainer = parent.GetNode<Control>("%CardContainer");
// 			cardContainer.AddChild(node);
// 			
// 			var enchantment = parent.GetNode<TextureRect>("%Enchantment");
// 			enchantment.CustomMinimumSize = new Vector2(
// 				enchantment.OffsetRight - enchantment.OffsetLeft,
// 				enchantment.OffsetBottom - enchantment.OffsetTop);
// 			cardContainer.RemoveChild(enchantment);
// 			
// 			SpireField__enchantmentTabContainer._enchantmentTabContainerLeft.Set(parent, node);
// 		});
// }
