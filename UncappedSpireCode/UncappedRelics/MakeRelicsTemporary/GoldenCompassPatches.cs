using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Runs;

namespace UncappedSpire.UncappedSpireCode.UncappedRelics.MakeRelicsTemporary;

[HarmonyPatch]
public static class GoldenCompassPatches
{
    [HarmonyPatch(typeof(GoldenCompass), "ModifyGeneratedMap")]
    [HarmonyPrefix]
    public static bool Prefix(GoldenCompass __instance, IRunState runState, ActMap map, int actIndex, ref ActMap __result)
    {
        if (GoldenCompassFields.WasUsedUp.Get(__instance))
        {
            __instance.Status = RelicStatus.Disabled;
            __result = map;
            return false;
        }
        if (__instance.GoldenPathAct == actIndex)
        {
            GoldenCompassFields.WasUsedUp.Set(__instance, true);
        }

        return true;
    }
}