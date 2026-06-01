using BaseLib.Utils;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary;

namespace UncappedSpire.UncappedSpireCode.UncappedUpgrades.UI.CardLibrary;

public partial class NCardLibraryUncappedUpgradeInput : SpinBox
{
	private static readonly string _scenePath = "res://UncappedSpireCode/UncappedUpgrades/UI/CardLibrary/card_library_uncapped_upgrade_input.tscn";
	private static bool isInternalUpdate;
	private static Action<int>? MultiplierChanged;
	
	public static AddedNode<NCardLibrary, NCardLibraryUncappedUpgradeInput>? Node = new(_scenePath,
		(parent, node) =>
		{
			// Add sync with other inputs
			node.Value = UpgradeContext.GetMultiplier();
			MultiplierChanged = v =>
			{
				isInternalUpdate = true;
				node.SetValue(v);
				isInternalUpdate = false;
			};
			UpgradeContext.MultiplierChanged += MultiplierChanged;
			node.TreeExiting += () =>
			{
				UpgradeContext.MultiplierChanged -= MultiplierChanged;
			};
			
			// Update related components when changed
			var _gridField = AccessTools.Field(typeof(NCardLibrary), "_grid");
			var _grid = (NCardGrid)_gridField.GetValue(parent)!;
			
			var updateFilter = AccessTools.Method(typeof(NCardLibrary), "UpdateFilter");
			var _viewUpgradesField = AccessTools.Field(typeof(NCardLibrary), "_viewUpgrades");
			node.ValueChanged += value =>
			{
				if (!isInternalUpdate)
				{
					UpgradeContext.UpdateMultiplier((int)value);
				}
				
				if (_viewUpgradesField.GetValue(parent) is NLibraryStatTickbox _viewUpgrades && _viewUpgrades.IsTicked)
				{
					if (_grid.IsShowingUpgrades)
					{
						_grid.IsShowingUpgrades = false;
						_grid.IsShowingUpgrades = true;
					}
				}
			};
			
			var lineEdit = node.GetLineEdit();
			lineEdit.AddThemeFontSizeOverride("font_size", 18);
			lineEdit.TextChanged += text =>
			{
				if (double.TryParse(text, out var value))
				{
					node.SetValue(value);
				}
			};
			
			var bottomVBoxContainer = parent
				.FindChild("Sidebar")
				.FindChild("MarginContainer")
				.FindChild("BottomVBox");
			
			bottomVBoxContainer.AddChild(node);
			
			SpireField_UncappedCardInput.UncappedCardInput.Set(parent, node);
		});
}
