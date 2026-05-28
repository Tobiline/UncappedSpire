using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Nodes.Screens;
using MegaCrit.Sts2.Core.Nodes.Screens.InspectScreens;
using UncappedSpire.UncappedSpireCode.CardModelPatches;

namespace UncappedSpire.UncappedSpireCode.UI;

public partial class NInspectCardLimitlessUpgradeInput : SpinBox
{
	private static readonly string _scenePath = "res://UncappedSpireCode/inspect_card_limitless_upgrade_input.tscn";
	
	public static AddedNode<NUpgradePreviewTickbox, NInspectCardLimitlessUpgradeInput>? Node = new(_scenePath,
		(parent, node) =>
		{
			var inspectCardScreen = (NInspectCardScreen)parent.FindParent("InspectCardScreen");
			
			var updateCardDisplay = HarmonyLib.AccessTools.Method(typeof(NInspectCardScreen), "UpdateCardDisplay");
			
			node.ValueChanged += value =>
			{
				UpgradeContext.Multiplier = (int)value;
				updateCardDisplay.Invoke(inspectCardScreen, []);
			};

			var lineEdit = node.GetLineEdit();

			lineEdit.TextChanged += text =>
			{
				if (double.TryParse(text, out var value))
				{
					node.SetValue(value);
				}
			};
			
			parent.AddChild(node);
			parent.MoveChild(node, 0);

			SpireField_LimitlessCardInput.LimitlessCardInput.Set(inspectCardScreen, node);
		});
}
