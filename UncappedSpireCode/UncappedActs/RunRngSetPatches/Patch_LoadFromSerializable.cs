using HarmonyLib;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace UncappedSpire.UncappedSpireCode.UncappedActs.RunRngSetPatches;

[HarmonyPatch(typeof(RunRngSet), nameof(RunRngSet.LoadFromSerializable))]
public class Patch_LoadFromSerializable
{
    [HarmonyPrefix]
    public static void Prefix(RunRngSet __instance, SerializableRunRngSet save)
    {
        save.Seed = __instance.StringSeed;
    }
}