using HarmonyLib;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Saves;

namespace UncappedSpire.UncappedSpireCode.UncappedActs.PlayerRngSetPatches;

[HarmonyPatch(typeof(PlayerRngSet), nameof(PlayerRngSet.LoadFromSerializable))]
public class Patch_LoadFromSerializable
{
    [HarmonyPrefix]
    public static void Prefix(PlayerRngSet __instance, SerializablePlayerRngSet save)
    {
        save.Seed = __instance.Seed;
    }
}