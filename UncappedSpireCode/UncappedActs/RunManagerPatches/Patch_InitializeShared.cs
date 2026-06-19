using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Runs;

namespace UncappedSpire.UncappedSpireCode.UncappedActs.RunManagerPatches;

[HarmonyPatch(typeof(RunManager), "InitializeShared")]
public class Patch_InitializeShared
{
    private static readonly MethodInfo Method_get_State = AccessTools.PropertyGetter(typeof(RunManager), "State");
    
    // TODO: Is run after ActionExecutor.Pause();, may be a problem
    [HarmonyPostfix]
    public static void Postfix(RunManager __instance)
    {
        SpireFields_RunManager.ChapterChangeSynchronizer.Set(__instance, new ChapterChangeSynchronizer(
            __instance.RunLocationTargetedBuffer,
            __instance.NetService,
            (RunState)Method_get_State.Invoke(__instance, null)!,
            __instance.NetService.NetId));
    }
}