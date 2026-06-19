using HarmonyLib;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Managers;

namespace UncappedSpire.UncappedSpireCode;

[HarmonyPatch(typeof(RunState), "AppendToMapPointHistory")]
public class Debug
{
    [HarmonyPrefix]
    public static void Prefix(RunState __instance)
    {
        MainFile.Logger.Info("Appended To Map Point History");
    }
}