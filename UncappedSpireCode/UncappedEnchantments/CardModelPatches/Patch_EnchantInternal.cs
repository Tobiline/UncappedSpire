using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;

namespace UncappedSpire.UncappedSpireCode.UncappedEnchantments.CardModelPatches;

[HarmonyPatch(typeof(CardModel), nameof(CardModel.EnchantInternal))]
public class Patch_EnchantInternal
{
    private static readonly MethodInfo ToFind_Method_set_Enchantment = AccessTools.PropertySetter(typeof(CardModel), nameof(CardModel.Enchantment));
    private static FieldInfo Field_EnchantmentChanged = AccessTools.Field(typeof(CardModel), nameof(CardModel.EnchantmentChanged));
    
    [HarmonyPrefix]
    public static bool Prefix(CardModel __instance, EnchantmentModel enchantment, decimal amount)
    {
        enchantment.Amount = (int)amount;
        __instance.AssertMutable();
        enchantment.AssertMutable();
        ToFind_Method_set_Enchantment.Invoke(__instance, [enchantment]);
        enchantment.ApplyInternal(__instance, amount);
        var del = (Action?)Field_EnchantmentChanged.GetValue(__instance);
        del?.Invoke();

        return false;
    }
}