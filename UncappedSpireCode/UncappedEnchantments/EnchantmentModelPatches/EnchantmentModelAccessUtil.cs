using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;

namespace UncappedSpire.UncappedSpireCode.UncappedEnchantments.EnchantmentModelPatches;

public static class EnchantmentModelAccessUtil
{
    public static readonly MethodInfo Method_OnEnchant = AccessTools.Method(typeof(EnchantmentModel), "OnEnchant");
    public static FieldInfo Field__card = AccessTools.Field(typeof(EnchantmentModel), "_card");
    public static MethodInfo Method_set_Card = AccessTools.PropertySetter(typeof(EnchantmentModel), "Card");
}