using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace UncappedSpire.UncappedSpireCode.UncappedUpgrades.DynamicVarPatches;

[HarmonyPatch(typeof(DynamicVar), MethodType.Constructor, [typeof(string), typeof(decimal)])]
public class Patch_Constructor
{
    private static readonly FieldInfo getOwner = AccessTools.Field(typeof(DynamicVar), "_owner");
    
    [HarmonyPrefix]
    public static void Prefix(DynamicVar __instance, string name, decimal baseValue)
    {
        SpireField_InitialValue._initialValue.Set(__instance, baseValue);
    }
}