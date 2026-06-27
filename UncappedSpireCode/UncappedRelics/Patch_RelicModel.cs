using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using UncappedSpire.UncappedSpireCode.Config;

namespace UncappedSpire.UncappedSpireCode.UncappedRelics;

[HarmonyPatch(typeof(RelicModel), nameof(RelicModel.IsStackable), MethodType.Getter)]
public class Patch_RelicModel
{
    [HarmonyPostfix]
    public static void Postfix(RelicModel __instance, ref bool __result)
    {
        if (ContextManager.UncappedRelicsEnabled && !RelicContext.IsNotStackable.Contains(__instance.GetType()))
        {
            __result = true;
        }
    }
}