using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Screens;

namespace UncappedSpire.UncappedSpireCode.LimitlessUpgrades.UI.DeckViewScreen;

public partial class NDeckViewLimitlessUpgradeInput : SpinBox
{
	private static readonly string _scenePath = "res://UncappedSpireCode/LimitlessUpgrades/UI/DeckViewScreen/deck_view_limitless_upgrade_input.tscn";
	
	public static AddedNode<NDeckViewScreen, NDeckViewLimitlessUpgradeInput>? Node = new(_scenePath,
		(parent, node) =>
		{
			var _gridField = HarmonyLib.AccessTools.Field(typeof(NCardsViewScreen), "_grid");
			var _grid = (NCardGrid)_gridField.GetValue(parent)!;
			
			node.ValueChanged += value =>
			{
				UpgradeContext.AddOrUpdateMultiplier((int)value);
				if (_grid.IsShowingUpgrades)
				{
					_grid.IsShowingUpgrades = false;
					_grid.IsShowingUpgrades = true;
				}
				UpgradeContext.RemoveMultiplier();
			};
			
			var lineEdit = node.GetLineEdit();
			lineEdit.AddThemeFontSizeOverride("font_size", 24);
			lineEdit.TextChanged += text =>
			{
				if (double.TryParse(text, out var value))
				{
					node.SetValue(value);
				}
			};
			
			var hBoxContainer = parent
				.FindChild("ViewUpgrades")
				.FindChild("MarginContainer")
				.FindChild("Upgrades");
			
			hBoxContainer.AddChild(node);
			hBoxContainer.MoveChild(node, 0);
			
			SpireField_LimitlessCardInput.LimitlessCardInput.Set(parent, node);
		});
}
