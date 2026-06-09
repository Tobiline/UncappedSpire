// using HarmonyLib;
// using MegaCrit.Sts2.Core.Models;
// using UncappedSpire.UncappedSpireCode.UncappedEnchantments.EnchantmentModelPatches;
//
// namespace UncappedSpire.UncappedSpireCode.UncappedEnchantments.CardModelPatches;
//
// [HarmonyPatch(typeof(CardModel), "get_Enchantment")]
// public class Patch_get_Enchantment
// {
//     [HarmonyPrefix]
//     public static bool Prefix(CardModel __instance, ref EnchantmentModel? __result)
//     {
//         // var multiEnchantment = SpireField__multiEnchantment._multiEnchantment.Get(__instance);
//         // if (multiEnchantment == null || SpireField_Enchantments.Enchantments[multiEnchantment]!.Count > 0)
//         // {
//         //     __result = multiEnchantment;
//         // }
//         // else
//         // {
//         //     __result = null;
//         // }
//         
//         __result = SpireField__multiEnchantment._multiEnchantment.Get(__instance);
//         return false;
//     }
// }