using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
using UncappedSpire.UncappedSpireCode.Config;

namespace UncappedSpire.UncappedSpireCode.UncappedRelics;

[HarmonyPatch(typeof(RelicGrabBag), "GetDeque")]
public class Patch_RelicGrabBag
{
    private static FieldInfo Field__deques = AccessTools.Field(typeof(RelicGrabBag), "_deques");
    private static MethodInfo Method_get_State = AccessTools.PropertyGetter(typeof(RunManager), "State");
    
    [HarmonyPrefix]
    public static void GetDequePostfix(RelicGrabBag __instance, RelicRarity rarity, ref List<RelicModel> __result)
    {
        if (!ContextManager.UncappedRelicsEnabled)
            return;
        
        var deques = (Dictionary<RelicRarity, List<RelicModel>>)Field__deques.GetValue(__instance)!;
        var state = (RunState)Method_get_State.Invoke(RunManager.Instance, null)!;
        var rng = state.Rng.UpFront;
        foreach (var value2 in deques.Values)
        {
            value2.UnstableShuffle(rng);
        }
    }
}