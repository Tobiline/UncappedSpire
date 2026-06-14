using BaseLib.Utils;
using MegaCrit.Sts2.Core.Models;

namespace UncappedSpire.UncappedSpireCode.UncappedEnchantments.EnchantmentModelPatches;

public class SpireFields_EnchantmentModel
{
    public static readonly SpireField<EnchantmentModel, bool> HideForAnim = new(() => false);
}