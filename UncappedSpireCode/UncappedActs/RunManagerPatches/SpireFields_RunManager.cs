using BaseLib.Utils;
using MegaCrit.Sts2.Core.Runs;
using UncappedSpire.UncappedSpireCode.Config;

namespace UncappedSpire.UncappedSpireCode.UncappedActs.RunManagerPatches;

public class SpireFields_RunManager
{
    public static SpireField<RunManager, UncappedSpireModifierSynchronizer> UncappedSpireModifierSynchronizer = new(() => null);
    public static SpireField<RunManager, ChapterChangeSynchronizer> ChapterChangeSynchronizer = new(() => null);
}