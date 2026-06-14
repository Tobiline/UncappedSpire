using HarmonyLib;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace UncappedSpire.UncappedSpireCode.UncappedEnchantments.EnchantmentModelPatches;

[HarmonyPatch(typeof(EnchantmentModel), "get_" + nameof(EnchantmentModel.HoverTips))]
public class Patch_HoverTips
{
    // Removes the placeholder Hovertip
    [HarmonyPostfix]
    public static void Postfix(EnchantmentModel __instance, ref IEnumerable<IHoverTip> __result)
    {
        if (__instance is MultiEnchantment)
        {
            __result = __result.Skip(1);
        }
    }
}