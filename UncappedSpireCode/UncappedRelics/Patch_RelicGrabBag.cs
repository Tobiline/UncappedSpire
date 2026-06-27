using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using UncappedSpire.UncappedSpireCode.Config;
using UncappedSpire.UncappedSpireCode.Util;

namespace UncappedSpire.UncappedSpireCode.UncappedRelics;

[HarmonyPatch(typeof(RelicGrabBag), "GetDeque")]
public class Patch_RelicGrabBag
{
    private static FieldInfo Field__deques = AccessTools.Field(typeof(RelicGrabBag), "_deques");
    
    [HarmonyPrefix]
    public static void GetDequePostfix(RelicGrabBag __instance, RelicRarity rarity, ref List<RelicModel> __result)
    {
        if (!ContextManager.UncappedRelicsEnabled)
            return;

        var rng = RunUtil.GetLocalPlayer()!.PlayerRng.Rewards;
        
        // TODO: Find a better way to do this...
        if (RunUtil.GetLocalPlayer()!.RunState.CurrentRoom is TreasureRoom)
        {
            rng = RunUtil.GetLocalPlayer()!.RunState.Rng.UpFront;
        }
        
        var deques = (Dictionary<RelicRarity, List<RelicModel>>)Field__deques.GetValue(__instance)!;
        foreach (var value2 in deques.Values)
        {
            value2.UnstableShuffle(rng);
        }
    }
}