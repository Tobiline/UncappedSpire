using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Screens;

namespace UncappedSpire.UncappedSpireCode.UncappedUpgrades.UI.InspectCardScreen;

public partial class NInspectCardUncappedUpgradeInput : SpinBox
{
	private static readonly string _scenePath = "res://UncappedSpireCode/UncappedUpgrades/UI/InspectCardScreen/inspect_card_uncapped_upgrade_input.tscn";
	private static bool isInternalUpdate;
	private static Action<int>? MultiplierChanged;
	
	public static AddedNode<NInspectCardScreen, NInspectCardUncappedUpgradeInput>? Node = new(_scenePath,
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
			
			var updateCardDisplay = HarmonyLib.AccessTools.Method(typeof(NInspectCardScreen), "UpdateCardDisplay");
			var _viewUpgradesField = HarmonyLib.AccessTools.Field(typeof(NInspectCardScreen), "_upgradeTickbox");
			node.ValueChanged += value =>
			{
				if (!isInternalUpdate)
				{
					UpgradeContext.UpdateMultiplier((int)value);
				}
				
				if (_viewUpgradesField.GetValue(parent) is NTickbox _viewUpgrades && _viewUpgrades.IsTicked)
				{
					UpgradeContext.EnableMultiplier();
					updateCardDisplay.Invoke(parent, []);
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
				.FindChild("Upgrade");
			
			hBoxContainer.AddChild(node);
			hBoxContainer.MoveChild(node, 0);

			SpireField_UncappedCardInput.UncappedCardInput.Set(parent, node);
		});
}
