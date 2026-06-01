using System.Text.RegularExpressions;
using HarmonyLib;
using MegaCrit.Sts2.Core.Localization;

namespace UncappedSpire.UncappedSpireCode.UncappedUpgrades.LocalizationPatches.LocManagerPatches;

// TODO: Optimise this
[HarmonyPatch(typeof(LocManager), "LoadTable")]
public class Patch_LoadTable
{
    public static string? timesText;
    
    [HarmonyPostfix]
    static void Postfix(LocManager __instance, ref Dictionary<string, string> __result)
    {
        // Grab timesText
        if (__result.TryGetValue("LOOP.description", out var loopDesc))
        {
            var match = new Regex(@"(\{IfUpgraded:show:\s2\s)([^}]*)(\})").Match(loopDesc);
            timesText = match.Groups[2].Value;
        }
        
        foreach (var key in __result.Keys.ToList())
        {
            var value = __result[key];

            __result[key] = Transform(key, value);
        }
    }

    private static readonly (Regex regex, MatchEvaluator transform)[] Rules =
    [
        (
            // Specific creatable card
            new(@"(\{[^}]*IfUpgraded[^}]*\+)(1)?([^}]*\})"),
            m => $"{m.Groups[1].Value}{{UpgradeLevel}}{m.Groups[3].Value}"
        ), (
            // Spinner
            new(@"(\{[^}]*IfUpgraded:show:\s?\[gold\][^}]*\[\/gold\]\s)([^}]*)(\s\[gold\][^}]*\[\/gold\][^}]*\})"),
            m => $"{m.Groups[1].Value}{{UpgradeLevel}}{m.Groups[3].Value}"
        ), (
            // Random creatable card
            new(@"(\{[^}]*IfUpgraded:show:\s?\[gold\])([^}]*)(\[\/gold\][^}]*\})"),
            m => $"{m.Groups[1].Value}+{{UpgradeLevel}}{m.Groups[3].Value}"
        ) 
    ];
    
    static string Transform(string key, string text)
    {
        if (key == "STACK.description")
            return text;
        
        foreach (var (regex, transform) in Rules)
        {
            if (key == "DARKNESS.description")
            {
                return new Regex(@"(\{IfUpgraded:show:\s?)([^}]*)(\|\})").Replace(text, 
                    m => $"{m.Groups[1].Value}{{UpgradeLevelPlusOne}} {timesText}{m.Groups[3].Value}");
            }
            if (key == "LOOP.description")
            {
                return new Regex(@"(\{IfUpgraded:show:\s)(2)(\s[^}]*\})").Replace(text,
                    m => $"{m.Groups[1].Value}{{UpgradeLevelPlusOne}}{m.Groups[3].Value}");
            }
            if (key == "KNOCKDOWN.description")
            {
                return new Regex(@"{IfUpgraded:show:triple\|double}").Replace(text,
                    m => "x{IfUpgraded:show:[gold]{UpgradeLevelPlusTwo}[/gold]|{UpgradeLevelPlusTwo}}");
            }
            
            var modified = regex.Replace(text, transform);
            if (modified != text) 
                return modified;
        }

        return text;
    }
}