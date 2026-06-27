using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using UncappedSpire.UncappedSpireCode.Config;

namespace UncappedSpire.UncappedSpireCode.UncappedActs.PowerModelPatches;

[HarmonyPatch]
public class Patch_DisplayAmount
{
    static IEnumerable<MethodBase> TargetMethods()
    {
        var propName = nameof(PowerModel.DisplayAmount);
        var displayOnlyTypes = ContextManager.PowerScalingImplementationTypes
            .Where(kvp => kvp.Value == ScalingImplementationType.DisplayModify)
            .Select(kvp => kvp.Key);

        var methodBases = displayOnlyTypes
            .Where(t => typeof(PowerModel).IsAssignableFrom(t))
            .Select(t => (MethodBase)AccessTools.PropertyGetter(t, propName))
            .Where(m => m != null)
            .Distinct();

        foreach (var m in methodBases)
        {
            yield return m;
        }
    }
    
    [HarmonyPostfix]
    public static void Postfix(PowerModel __instance, ref int __result)
    {
        if (((__instance.Target != null && __instance.Target.IsEnemy) || (__instance.Owner != null && __instance.Owner.IsEnemy))
            && (__instance.Applier == null || __instance.Applier.IsEnemy)
            && __instance.TryGetScaling(ScalingImplementationType.DisplayModify, out var scaling))
        {
            __result = (int)(__result * scaling);
        }
    }
}