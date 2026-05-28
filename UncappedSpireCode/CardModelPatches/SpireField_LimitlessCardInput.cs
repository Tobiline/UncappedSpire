using BaseLib.Utils;
using MegaCrit.Sts2.Core.Nodes.Screens;
using UncappedSpire.UncappedSpireCode.UI;

namespace UncappedSpire.UncappedSpireCode.CardModelPatches;

public class SpireField_LimitlessCardInput
{
    public static readonly SpireField<NInspectCardScreen, NInspectCardLimitlessUpgradeInput> LimitlessCardInput = new(() => null);
}