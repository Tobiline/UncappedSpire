using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Models;

namespace UncappedSpire.UncappedSpireCode.UncappedActs.PowerCmdPatches;

[HarmonyPatch(typeof(PowerCmd), nameof(PowerCmd.Remove), [typeof(PowerModel)])]
public class Patch_Remove
{
    [HarmonyPrefix]
    public static void Prefix(PowerModel? power)
    {
        if (power != null)
        {
            ChapterManager.JustRemovedPower = power;
        }
    }
}