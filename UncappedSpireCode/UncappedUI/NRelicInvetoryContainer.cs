using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace UncappedSpire.UncappedSpireCode.UncappedUI;

public partial class NRelicInvetoryContainer : Control
{
	private static readonly string _scenePath = "res://UncappedSpireCode/UncappedUI/relic_inventory_container.tscn";

	public static AddedNode<NGlobalUi, NRelicInvetoryContainer>? Node = new(_scenePath,
		(parent, node) =>
		{
			var relicInventory = parent.GetNode<Control>("%RelicInventory");
			var scrollContainer = node.GetNode<ScrollContainer>("RelicInventoryScrollContainer");
			
			parent.RemoveChild(relicInventory);
			scrollContainer.AddChild(relicInventory);

			relicInventory.SizeFlagsHorizontal = SizeFlags.ExpandFill;
			relicInventory.SizeFlagsVertical = SizeFlags.ExpandFill;
		});
}
