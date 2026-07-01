using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace UncappedSpire.UncappedSpireCode.UncappedUI.RelicInventoryContainer;

public partial class NRelicInventoryContainer : Control
{
	private static readonly string _scenePath = "res://UncappedSpireCode/UncappedUI/RelicInventoryContainer/relic_inventory_container.tscn";

	public static AddedNode<NGlobalUi, NRelicInventoryContainer>? Node = new(_scenePath,
		(parent, node) =>
		{
			var relicInventory = parent.GetNode<Control>("%RelicInventory");
			var scrollContainer = node.GetNode<ScrollContainer>("RelicInventoryScrollContainer");

			var index = relicInventory.GetIndex();
			
			parent.RemoveChild(relicInventory);
			scrollContainer.AddChild(relicInventory);

			relicInventory.SizeFlagsHorizontal = SizeFlags.ExpandFill;
			relicInventory.SizeFlagsVertical = SizeFlags.ExpandFill;
			
			parent.AddChild(node);
			parent.MoveChild(node, index);
		});
}
