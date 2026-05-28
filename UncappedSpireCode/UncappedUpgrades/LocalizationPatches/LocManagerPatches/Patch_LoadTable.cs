using HarmonyLib;
using MegaCrit.Sts2.Core.Localization;

namespace UncappedSpire.UncappedSpireCode.UncappedUpgrades.LocalizationPatches.LocManagerPatches;

[HarmonyPatch(typeof(LocManager), "LoadTable")]
public class Patch_LoadTable
{
    [HarmonyPostfix]
    static void Postfix(LocManager __instance, ref Dictionary<string, string> __result)
    {
        foreach (var key in __result.Keys.ToList())
        {
            var value = __result[key];

            __result[key] = Transform(value);
        }
    }
    
    static string Transform(string text)
    {
        return text
            .Replace("{IfUpgraded:show:+1}", "[green]{UpgradeLevel:cond:>0?+{}|}[/green]")
            .Replace("{IfUpgraded:show:X+1|X}", "X[green]{UpgradeLevel:cond:>0?+{}|}[/green]");
    }
}