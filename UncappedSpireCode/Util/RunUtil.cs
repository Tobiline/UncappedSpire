using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Runs;

namespace UncappedSpire.UncappedSpireCode.Util;

public static class RunUtil
{
    public static Player? GetLocalPlayer()
    {
        return LocalContext.GetMe(RunManager.Instance.DebugOnlyGetState());
    }
}