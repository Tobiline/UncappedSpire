using BaseLib.Utils;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace UncappedSpire.UncappedSpireCode.UncappedUpgrades.DynamicVarPatches;

public class SpireField_InitialValue
{
    public static readonly SpireField<DynamicVar, decimal> _initialValue = new(() => 0);
}