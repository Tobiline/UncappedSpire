using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Runs;
using UncappedSpire.UncappedSpireCode.Config;

namespace UncappedSpire.UncappedSpireCode.UncappedAnimations;

[HarmonyPatch(typeof(TweenHelper), nameof(TweenHelper.AwaitFinished), [typeof(Tween), typeof(Node)])]
public class TweenPatch
{
    [HarmonyPrefix]
    public static void Prefix(ref Tween tween, Node owner)
    {
        var isPlayerTurn =
            (LocalContext.GetMe(RunManager.Instance.DebugOnlyGetState())?.PlayerCombatState?.Phase ??
             PlayerTurnPhase.None) != PlayerTurnPhase.None;
        if (UncappedConfig.UncappedAnimationsEnabled && isPlayerTurn && owner is NCombatRoom)
        {
            if (AnimationContext.IsAtBatchLimit)
            {
                AnimationContext.ConsecutiveInstantAnimationCount = 0;
            }
            else
            {
                tween.CustomStep(double.MaxValue);
                AnimationContext.ConsecutiveInstantAnimationCount++;
            }
        }
    }
}