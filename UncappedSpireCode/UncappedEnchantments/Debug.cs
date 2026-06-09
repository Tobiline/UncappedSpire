using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Models;
using UncappedSpire.UncappedSpireCode.UncappedEnchantments.CardModelPatches;
using UncappedSpire.UncappedSpireCode.UncappedEnchantments.EnchantmentModelPatches;

namespace UncappedSpire.UncappedSpireCode.UncappedEnchantments;

[HarmonyPatch(typeof(CombatState), "CloneCard")]
public static class Debug
{
    // [HarmonyPrefix]
    // public static void Prefix(CombatState __instance, CardModel mutableCard)
    // {
    //     var model1 = ModelDb.Enchantment<MultiEnchantment>().ToMutable();
    //     var model2 = ModelDb.Enchantment<MultiEnchantment>().ToMutable();
    //
    //     if (mutableCard.Enchantment is not null)
    //     {
    //         MainFile.Logger.Info($"Card: {mutableCard.Title}, {((MultiEnchantment)mutableCard.Enchantment).SerializableEnchantmentOnCards.Count}");
    //     }
    // }
    
    // [HarmonyPostfix]
    // public static void Postfix(CombatState __instance, CardModel __result, CardModel mutableCard)
    // {
    //     if (__result.Enchantment is not null)
    //     {
    //         MainFile.Logger.Info("After: " + ((MultiEnchantment)__result.Enchantment).RandomNumber);
    //     }
    // }
}