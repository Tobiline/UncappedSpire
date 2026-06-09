// using BaseLib.Utils;
// using Godot;
// using MegaCrit.Sts2.Core.Nodes.Cards;
//
// namespace UncappedSpire.UncappedSpireCode.UncappedEnchantments.UI;
//
// public partial class NEnchantmentTabContainerRight : VBoxContainer
// {
// 	private static readonly string _scenePath = "res://UncappedSpireCode/UncappedEnchantments/UI/enchantment_tab_container_right.tscn";
// 	
// 	public static AddedNode<NCard, NEnchantmentTabContainerRight>? Node = new(_scenePath,
// 		(parent, node) =>
// 		{
// 			var cardContainer = parent.GetNode<Control>("%CardContainer");
// 			cardContainer.AddChild(node);
// 			
// 			Console.WriteLine("Added Right Node");
// 			Console.WriteLine("Added Right Node");
// 			Console.WriteLine("Added Right Node: " + cardContainer.Name);
// 			Console.WriteLine("Added Right Node");
// 			Console.WriteLine("Added Right Node");
// 			
// 			SpireField__enchantmentTabContainer._enchantmentTabContainerRight.Set(parent, node);
// 		});
// }
