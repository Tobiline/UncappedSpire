using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Nodes.Screens;

namespace UncappedSpire.UncappedSpireCode.UI;

public partial class CardViewLimitlessUpgradeInput : Control
{
    public static AddedNode<NSimpleCardsViewScreen, CardViewLimitlessUpgradeInput> Node = new((screen) =>
    {
        var control = new CardViewLimitlessUpgradeInput();

        
        
        return control;
    });
}