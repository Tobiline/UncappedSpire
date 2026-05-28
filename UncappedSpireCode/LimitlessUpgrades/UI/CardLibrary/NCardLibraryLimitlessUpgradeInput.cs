using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary;

namespace UncappedSpire.UncappedSpireCode.LimitlessUpgrades.UI.CardLibrary;

public partial class NCardLibraryLimitlessUpgradeInput : SpinBox
{
	private static readonly string _scenePath = "res://UncappedSpireCode/LimitlessUpgrades/UI/CardLibrary/card_library_limitless_upgrade_input.tscn";
	
	public static AddedNode<NCardLibrary, NCardLibraryLimitlessUpgradeInput>? Node = new(_scenePath,
		(parent, node) =>
		{
			var updateFilter = HarmonyLib.AccessTools.Method(typeof(NCardLibrary), "UpdateFilter");
			node.ValueChanged += value =>
			{
				UpgradeContext.AddOrUpdateMultiplier((int)value);
				updateFilter.Invoke(parent, [false]);
				UpgradeContext.RemoveMultiplier();
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
			
			SpireField_LimitlessCardInput.LimitlessCardInput.Set(parent, node);
		});
}
