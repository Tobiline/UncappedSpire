using HarmonyLib;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Cards;
using UncappedSpire.UncappedSpireCode.Config;

namespace UncappedSpire.UncappedSpireCode.UncappedActs.DynamicVarPatches;

[HarmonyPatch(typeof(DynamicVar), nameof(DynamicVar.UpgradeValueBy))]
public class Patch_UpgradeValueBy
{
    [HarmonyPrefix]
    public static void Prefix(DynamicVar __instance, ref decimal addend, AbstractModel? ____owner)
    {
        if (____owner == null || ____owner is not CardModel card || card.Type != CardType.Status)
            return;
        
        if (ContextManager.ScalingStatusDynamicVarKeys.TryGetValue(__instance.Name, out var scaleType))
        {
            addend *= (decimal)ContextManager.GetScaling(scaleType);
        }
    }
}