using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Models;
using UncappedSpire.UncappedSpireCode.UncappedEnchantments.EnchantmentModelPatches;

namespace UncappedSpire.UncappedSpireCode.UncappedEnchantments.CardModelPatches;

[HarmonyPatch(typeof(CardModel), "set_Enchantment")]
public class Patch_set_Enchantment
{
    static readonly FieldInfo Field_EnchantmentBackingField = AccessTools.Field(typeof(CardModel), "<Enchantment>k__BackingField");
    
    [HarmonyPrefix]
    public static bool Prefix(CardModel __instance, EnchantmentModel? value)
    {
        if (value == null) 
            return false;
        
        var enchantmentType = value.GetType();
        if (enchantmentType == typeof(MultiEnchantment))
        {
            Field_EnchantmentBackingField.SetValue(__instance, value);
            return false;
        }

        var isEnchantmentStorageCard = __instance.GetType() == typeof(EnchantmentStorageCard);
        
        var enchantmentBackingField = (EnchantmentModel?)Field_EnchantmentBackingField.GetValue(__instance);
        if (enchantmentBackingField == null && !isEnchantmentStorageCard)
        {
            //var multiEnchantment = ModelDb.Enchantment<MultiEnchantment>().ToMutable();
            CardCmd.Enchant<MultiEnchantment>(__instance, 1M);
        }

        if (!isEnchantmentStorageCard)
        {
            enchantmentBackingField = (EnchantmentModel)Field_EnchantmentBackingField.GetValue(__instance)!;
            ((MultiEnchantment)enchantmentBackingField).AddEnchantment(value);
        }
        else
        {
            Field_EnchantmentBackingField.SetValue(__instance, value);
        }
        
        return false;
    }
}