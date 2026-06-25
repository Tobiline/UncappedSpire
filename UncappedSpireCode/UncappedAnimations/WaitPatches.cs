using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Runs;
using UncappedSpire.UncappedSpireCode.Config;

namespace UncappedSpire.UncappedSpireCode.UncappedAnimations;

[HarmonyPatch(typeof(Cmd), nameof(Cmd.Wait), [typeof(float), typeof(CancellationToken), typeof(bool)])]
public class WaitPatches
{
    [HarmonyPrefix]
    public static bool WaitPrefix(ref float seconds, CancellationToken cancelToken, bool ignoreCombatEnd, ref Task __result, out bool __state
        )
    {
        __state = false;
        var isPlayerTurn =
            (LocalContext.GetMe(RunManager.Instance.DebugOnlyGetState())?.PlayerCombatState?.Phase ??
             PlayerTurnPhase.None) != PlayerTurnPhase.None;
        if (UncappedConfig.UncappedAnimationsEnabled && isPlayerTurn)
        {
            if (AnimationContext.IsAtBatchLimit)
            {
                seconds = 0.1f;
                __state = true;
            }
            else
            {
                AnimationContext.ConsecutiveInstantAnimationCount++;
                seconds = 0;
                __result = Task.CompletedTask;
                return false;
            }
        }

        return true;
    }

    [HarmonyPostfix]
    public static void WaitPostfix(bool __state)
    {
        if (__state)
        {
            AnimationContext.ConsecutiveInstantAnimationCount = 0;
        }
    }
}