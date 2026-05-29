using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Screens;

namespace UncappedSpire.UncappedSpireCode.UncappedUpgrades.UI.DeckViewScreen;

public partial class NDeckViewUncappedUpgradeInput : SpinBox
{
	private static readonly string _scenePath = "res://UncappedSpireCode/UncappedUpgrades/UI/DeckViewScreen/deck_view_uncapped_upgrade_input.tscn";
	private static bool isInternalUpdate;
	private static Action<int> MultiplierChanged;
	
	public static AddedNode<NDeckViewScreen, NDeckViewUncappedUpgradeInput>? Node = new(_scenePath,
		(parent, node) =>
		{
			node.Value = UpgradeContext.GetMultiplierRaw();
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
					UpgradeContext.EnableMultiplier();
					if (_grid.IsShowingUpgrades)
					{
						_grid.IsShowingUpgrades = false;
						_grid.IsShowingUpgrades = true;
					}

					UpgradeContext.DisableMultiplier();
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
