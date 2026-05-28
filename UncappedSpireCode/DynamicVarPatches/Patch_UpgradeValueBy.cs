using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace UncappedSpire.UncappedSpireCode.DynamicVarPatches;

[HarmonyPatch(typeof(DynamicVar), "UpgradeValueBy")]
public class Patch_UpgradeValueBy
{
    [HarmonyPrefix]
    static void Prefix(DynamicVar __instance, ref decimal addend)
    {
        addend *= UpgradeContext.Multiplier;
    }
}