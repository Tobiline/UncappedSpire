using BaseLib.Utils;
using MegaCrit.Sts2.Core.Models;

namespace UncappedSpire.UncappedSpireCode.UncappedEnchantments.EnchantmentModelPatches;

public class SpireField_Enchantments
{
    public static readonly SpireField<EnchantmentModel, List<EnchantmentModel>> Enchantments = new(() => []);
}