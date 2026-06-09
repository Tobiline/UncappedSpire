using HarmonyLib;
using MegaCrit.Sts2.Core.Models;

namespace UncappedSpire.UncappedSpireCode.UncappedEnchantments.CardModelPatches;

[HarmonyPatch(typeof(CardModel), "get_Enchantment")]
public class Patch_get_Enchantment
{
    [HarmonyPrefix]
    public static bool Prefix(CardModel __instance, ref EnchantmentModel? __result)
    {
        __result = SpireField__multiEnchantment._multiEnchantment.Get(__instance);
        return false;
    }
}