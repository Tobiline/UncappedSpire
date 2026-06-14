using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.Models.Relics;

namespace UncappedSpire.UncappedSpireCode.UncappedEnchantments.RelicPatches;

[HarmonyPatch(typeof(PaelsGrowth), nameof(PaelsGrowth.TryModifyRestSiteOptions))]
public class Patch_PaelsGrowth_TryModifyRestOptions
{
    [HarmonyPrefix]
    public static bool Prefix(PaelsGrowth __instance, Player player, ICollection<RestSiteOption> options)
    {
        return options.All(o => o.OptionId != "CLONE");
    }
}