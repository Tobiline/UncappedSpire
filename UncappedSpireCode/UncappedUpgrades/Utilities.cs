using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SmartFormat.Core.Extensions;

namespace UncappedSpire.UncappedSpireCode.UncappedUpgrades;

public static class Utilities
{
    public static string GetFormattedStarIcons(int amount, string text, IFormattingInfo formattingInfo)
    {
        string? result;
        
        var shouldCollapse = amount is <= 0 or >= 4;
        if (shouldCollapse)
        {
            result = formattingInfo.CurrentValue is not DynamicVar dynamicVar
                ? $"{amount}{text}"
                : dynamicVar.ToHighlightedString(inverse: false) + text;
        }
        else
        {
            result = string.Concat(Enumerable.Repeat(text, amount));
        }
        
        return result;
    }
}