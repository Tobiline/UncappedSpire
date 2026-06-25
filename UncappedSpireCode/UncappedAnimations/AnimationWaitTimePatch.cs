using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using UncappedSpire.UncappedSpireCode.Config;

namespace UncappedSpire.UncappedSpireCode.UncappedAnimations;

[HarmonyPatch]
public class AnimationWaitTimePatch
{
    [HarmonyPatch(typeof(Cmd), nameof(Cmd.CustomScaledWait))]
    [HarmonyPrefix]
    public static bool CustomScaledWaitPrefix(float fastSeconds, float standardSeconds, bool ignoreCombatEnd, CancellationToken cancellationToken, ref Task __result)
    {
        var isPlayerTurn =
            (LocalContext.GetMe(RunManager.Instance.DebugOnlyGetState())?.PlayerCombatState?.Phase ??
             PlayerTurnPhase.None) != PlayerTurnPhase.None;
        
        if (UncappedConfig.UncappedAnimationsEnabled && isPlayerTurn)
        {
            __result = CustomScaledWaitReplacement(fastSeconds, standardSeconds, ignoreCombatEnd, cancellationToken);
            return false;
        }

        return true;
    }

    private static async Task CustomScaledWaitReplacement(float fastSeconds, float standardSeconds, bool ignoreCombatEnd, CancellationToken cancellationToken)
    {
        var fastModeType = (cancellationToken == CancellationToken.None || fastSeconds == 0 || standardSeconds == 0)
            ? FastModeType.Instant 
            : SaveManager.Instance.PrefsSave.FastMode;

        if (fastModeType == FastModeType.Instant && !AnimationContext.IsAtBatchLimit)
        {
            AnimationContext.ConsecutiveInstantAnimationCount++;
        }
        else
        {
            await Cmd.Wait(0.1f, cancellationToken, ignoreCombatEnd);
            AnimationContext.ConsecutiveInstantAnimationCount = 0;
        }
    }
}