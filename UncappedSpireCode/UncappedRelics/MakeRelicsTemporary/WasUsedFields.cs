using BaseLib.Utils;
using MegaCrit.Sts2.Core.Models.Relics;

namespace UncappedSpire.UncappedSpireCode.UncappedRelics.MakeRelicsTemporary;

public static class FurCoatFields
{
    public static SavedSpireField<FurCoat, bool> WasUsedUp = new(() => false, "FurCoatFields-WasUsedUp");
}

public static class GoldenCompassFields
{
    public static SavedSpireField<GoldenCompass, bool> WasUsedUp = new(() => false, "GoldenCompassFields-WasUsedUp");
}