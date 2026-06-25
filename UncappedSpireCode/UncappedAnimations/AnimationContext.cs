using UncappedSpire.UncappedSpireCode.Config;

namespace UncappedSpire.UncappedSpireCode.UncappedAnimations;

public static class AnimationContext
{
    public static int ConsecutiveInstantAnimationCount { get; set; }
    public static bool IsAtBatchLimit => ConsecutiveInstantAnimationCount >= UncappedConfig.AnimationBatchSize;
}