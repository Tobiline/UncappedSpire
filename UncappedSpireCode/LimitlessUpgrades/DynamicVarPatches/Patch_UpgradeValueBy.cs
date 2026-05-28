using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace UncappedSpire.UncappedSpireCode.LimitlessUpgrades.DynamicVarPatches;

[HarmonyPatch(typeof(DynamicVar), "UpgradeValueBy")]
public class Patch_UpgradeValueBy
{
    public static FieldInfo Field__owner = HarmonyLib.AccessTools.Field(typeof(DynamicVar), "_owner");
    
    [HarmonyPrefix]
    static void Prefix(DynamicVar __instance, ref decimal addend)
    {
        addend *= UpgradeContext.GetMultiplier();
    }
}