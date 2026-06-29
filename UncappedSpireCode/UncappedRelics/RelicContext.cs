using MegaCrit.Sts2.Core.Models.Relics;

namespace UncappedSpire.UncappedSpireCode.UncappedRelics;

public static class RelicContext
{
    public static HashSet<Type> IsNotStackable =
    [
        typeof(JuzuBracelet),
        typeof(BeatingRemnant),
        typeof(IceCream),
        typeof(MiniatureTent),
        typeof(SneckoEye),
        typeof(LordsParasol),
        typeof(NutritiousSoup),
        typeof(PandorasBox),
        typeof(RunicPyramid),
        typeof(PaperKrane),
        typeof(PaperPhrog),
        typeof(WhisperingEarring),
        typeof(UnceasingTop)
    ];
}