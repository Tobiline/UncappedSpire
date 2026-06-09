using HarmonyLib;
using MegaCrit.Sts2.Core.Models;

namespace UncappedSpire.UncappedSpireCode.UncappedEnchantments;

[HarmonyPatch(typeof(CardModel), "EnchantInternal")]
public static class Debug
{
    [HarmonyPrefix]
    public static void Prefix(CardModel __instance, EnchantmentModel enchantment, decimal amount)
    {
        // MainFile.Logger.Info("Card: " + __instance.Title);
        // MainFile.Logger.Info("Enchantment: " + enchantment.Title.GetRawText());
        // MainFile.Logger.Info("Enchantment current card: " + enchantment.Card);
    }
}