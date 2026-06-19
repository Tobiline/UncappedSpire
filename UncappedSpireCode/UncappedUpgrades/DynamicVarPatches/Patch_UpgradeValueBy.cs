using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace UncappedSpire.UncappedSpireCode.UncappedUpgrades.DynamicVarPatches;

[HarmonyPatch(typeof(DynamicVar), "UpgradeValueBy")]
public class Patch_UpgradeValueBy
{
    private static readonly FieldInfo getOwner = AccessTools.Field(typeof(DynamicVar), "_owner");
    
    [HarmonyPrefix]
    static void Prefix(DynamicVar __instance, ref decimal addend)
    {
        if (getOwner.GetValue(__instance) is CardModel card && card.Type != CardType.Status)
        {
            __instance.BaseValue = SpireField_InitialValue._initialValue.Get(__instance);
            addend *= card.CurrentUpgradeLevel;
        }
    }
}