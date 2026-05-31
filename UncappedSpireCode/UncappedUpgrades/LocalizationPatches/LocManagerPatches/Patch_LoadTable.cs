using System.Text.RegularExpressions;
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

    private static readonly (Regex regex, MatchEvaluator transform)[] Rules =
    [
        (
            // Specific creatable card
            new(@"(\{[^}]*IfUpgraded[^}]*\+)(1)?([^}]*\})"),
            m => $"{m.Groups[1].Value}{{UpgradeLevel}}{m.Groups[3].Value}"
        ), (
            // Random creatable card
            new(@"(\{[^}]*IfUpgraded:show:\s?\[gold\])([^}]*)(\[\/gold\][^}]*\})"),
            m => $"{m.Groups[1].Value}+{{UpgradeLevel}}{m.Groups[3].Value}"
        ) 
    ];
    
    static string Transform(string text)
    {
        foreach (var (regex, transform) in Rules)
        {
            var modified = regex.Replace(text, transform);
            if (modified != text) 
                return modified;
        }

        return text;
    }
}