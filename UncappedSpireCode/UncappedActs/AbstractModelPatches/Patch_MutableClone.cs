using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using UncappedSpire.UncappedSpireCode.Config;

namespace UncappedSpire.UncappedSpireCode.UncappedActs.AbstractModelPatches;

[HarmonyPatch(typeof(AbstractModel), nameof(AbstractModel.MutableClone))]
public class Patch_MutableClone
{
    private static FieldInfo Field__baseValue = AccessTools.Field(typeof(DynamicVar), "_baseValue");
    
    [HarmonyPostfix]
    public static void Postfix(AbstractModel __instance, ref AbstractModel __result)
    {
        if (__result is not CardModel card || card.Type != CardType.Status)
            return;
        
        foreach (var key in card.DynamicVars.Keys.Where(k => ContextManager.ScalingStatusDynamicVarKeys.ContainsKey(k)))
        {
            var dynamicVar = card.DynamicVars[key];
            var scaleType = ContextManager.ScalingStatusDynamicVarKeys[key];
            var baseValue = (decimal)Field__baseValue.GetValue(dynamicVar)!;
            var upgradeValueBy = baseValue * (decimal)ContextManager.GetScaling(scaleType);
            
            Field__baseValue.SetValue(dynamicVar, upgradeValueBy);
        }
    }
}