using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Models;
using UncappedSpire.UncappedSpireCode.UncappedEnchantments.EnchantmentModelPatches;

namespace UncappedSpire.UncappedSpireCode.UncappedEnchantments.CardModelPatches;

[HarmonyPatch(typeof(CardModel), "set_Enchantment")]
public class Patch_set_Enchantment
{
    [HarmonyPrefix]
    public static bool Prefix(CardModel __instance, EnchantmentModel? value)
    {
        if (value == null) 
            return false;
        
        var enchantmentType = value.GetType();
        if (enchantmentType == typeof(MultiEnchantment))
        {
            SpireField__multiEnchantment._multiEnchantment.Set(__instance, (MultiEnchantment)value);
            return false;
        }
        
        var _multiEnchantment = SpireField__multiEnchantment._multiEnchantment.Get(__instance);
        if (_multiEnchantment == null)
        {
            CardCmd.Enchant<MultiEnchantment>(__instance, 1M);
            _multiEnchantment = SpireField__multiEnchantment._multiEnchantment.Get(__instance);
        }
        
        var matchingEnchantment = SpireField_Enchantments.Enchantments[_multiEnchantment!]!.Find(e => e.GetType() == enchantmentType);
        if (matchingEnchantment != null && matchingEnchantment.ShowAmount)
        {
            matchingEnchantment.Amount += value.Amount;
            MainFile.Logger.Info("Enchantment: " + value.Title.GetRawText());
            MainFile.Logger.Info("Enchantment: " + value.Amount);
            MainFile.Logger.Info("Enchantment: " + value.DisplayAmount);
        }
        else
        {
            SpireField_Enchantments.Enchantments[_multiEnchantment!]!.Add(value);
        }
        
        return false;
    }
}