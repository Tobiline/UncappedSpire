using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;

namespace UncappedSpire.UncappedSpireCode.UncappedEnchantments;

public static class EnchantmentModelAccessUtil
{
    public static readonly MethodInfo Method_OnEnchant = AccessTools.Method(typeof(EnchantmentModel), "OnEnchant");
}