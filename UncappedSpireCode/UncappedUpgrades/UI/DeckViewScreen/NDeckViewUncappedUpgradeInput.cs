using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Screens;
using UncappedSpire.UncappedSpireCode.Config;
using UncappedSpire.UncappedSpireCode.UncappedActs;

namespace UncappedSpire.UncappedSpireCode.UncappedUpgrades.UI.DeckViewScreen;

public partial class NDeckViewUncappedUpgradeInput : SpinBox
{
	private static readonly string _scenePath = "res://UncappedSpireCode/UncappedUpgrades/UI/DeckViewScreen/deck_view_uncapped_upgrade_input.tscn";
	private static bool isInternalUpdate;
	private static Action<int>? MultiplierChanged;
	public static Action? EnabledInConfig;
	
	public static AddedNode<NDeckViewScreen, NDeckViewUncappedUpgradeInput>? Node = new(_scenePath,
		(parent, node) =>
		{
			EnabledInConfig = () =>
			{
				node.Visible = ContextManager.UncappedUpgradesEnabled;
			};
			UpgradeContext.EnabledInConfig += EnabledInConfig;
			
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
				UpgradeContext.EnabledInConfig -= EnabledInConfig;
				UpgradeContext.MultiplierChanged -= MultiplierChanged;
			};
			
			// Update related components when changed
			var _gridField = HarmonyLib.AccessTools.Field(typeof(NCardsViewScreen), "_grid");
			var _grid = (NCardGrid)_gridField.GetValue(parent)!;
			var _viewUpgradesField = HarmonyLib.AccessTools.Field(typeof(NDeckViewScreen), "_showUpgrades");
			node.ValueChanged += value =>
			{
				if (!isInternalUpdate)
				{
					UpgradeContext.UpdateMultiplier((int)value);
				}
				
				if (_viewUpgradesField.GetValue(parent) is NTickbox _viewUpgrades && _viewUpgrades.IsTicked)
				{
					if (_grid.IsShowingUpgrades)
					{
						_grid.IsShowingUpgrades = false;
						_grid.IsShowingUpgrades = true;
					}
				}
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
			
			SpireField_UncappedCardInput.UncappedCardInput.Set(parent, node);
		});
}
