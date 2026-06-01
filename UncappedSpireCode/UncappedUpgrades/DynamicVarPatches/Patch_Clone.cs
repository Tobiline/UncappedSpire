using HarmonyLib;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace UncappedSpire.UncappedSpireCode.UncappedUpgrades.DynamicVarPatches;

[HarmonyPatch(typeof(DynamicVar), "Clone")]
public class Patch_Clone
{
    [HarmonyPostfix]
    static void Postfix(DynamicVar __instance, ref DynamicVar __result)
    {
        var _initialValue = SpireField_InitialValue._initialValue.Get(__instance);
        SpireField_InitialValue._initialValue.Set(__result, _initialValue);
    }
}