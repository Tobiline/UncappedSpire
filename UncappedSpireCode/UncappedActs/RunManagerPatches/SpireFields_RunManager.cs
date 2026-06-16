using BaseLib.Utils;
using MegaCrit.Sts2.Core.Runs;

namespace UncappedSpire.UncappedSpireCode.UncappedActs.RunManagerPatches;

public class SpireFields_RunManager
{
    public static SpireField<RunManager, SeedChangeSynchronizer> ChapterChangeSynchronizer = new(() => null);
}