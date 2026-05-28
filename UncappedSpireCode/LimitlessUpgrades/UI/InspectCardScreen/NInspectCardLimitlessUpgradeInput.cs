using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Nodes.Screens;
using MegaCrit.Sts2.Core.Nodes.Screens.InspectScreens;

namespace UncappedSpire.UncappedSpireCode.LimitlessUpgrades.UI.InspectCardScreen;

public partial class NInspectCardLimitlessUpgradeInput : SpinBox
{
	private static readonly string _scenePath = "res://UncappedSpireCode/LimitlessUpgrades/UI/InspectCardScreen/inspect_card_limitless_upgrade_input.tscn";
	
	public static AddedNode<NInspectCardScreen, NInspectCardLimitlessUpgradeInput>? Node = new(_scenePath,
		(parent, node) =>
		{
			var updateCardDisplay = HarmonyLib.AccessTools.Method(typeof(NInspectCardScreen), "UpdateCardDisplay");
			node.ValueChanged += value =>
			{
				UpgradeContext.AddOrUpdateMultiplier((int)value);
				updateCardDisplay.Invoke(parent, []);
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
				.FindChild("Upgrade");
			
			hBoxContainer.AddChild(node);
			hBoxContainer.MoveChild(node, 0);

			SpireField_LimitlessCardInput.LimitlessCardInput.Set(parent, node);
		});
}
