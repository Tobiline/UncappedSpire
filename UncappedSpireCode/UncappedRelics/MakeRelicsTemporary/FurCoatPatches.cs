using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models.Relics;

namespace UncappedSpire.UncappedSpireCode.UncappedRelics.MakeRelicsTemporary;

[HarmonyPatch]
public static class FurCoatPatches
{
    [HarmonyPatch(typeof(FurCoat), "AddMarkedRooms")]
    [HarmonyPrefix]
    public static bool Prefix(FurCoat __instance, ActMap map, ref ActMap __result)
    {
        if (FurCoatFields.WasUsedUp.Get(__instance))
        {
            __instance.Status = RelicStatus.Disabled;
            __result = map;
            return false;
        }
        if (__instance.Owner.RunState.CurrentActIndex == __instance.FurCoatActIndex)
        {
            FurCoatFields.WasUsedUp.Set(__instance, true);
        }

        return true;
    }
}