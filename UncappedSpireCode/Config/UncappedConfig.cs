using BaseLib.Config;
using UncappedSpire.UncappedSpireCode.UncappedUpgrades;

namespace UncappedSpire.UncappedSpireCode.Config;

internal class UncappedConfig : SimpleModConfig
{
    #region Uncapped Acts
    [ConfigSection("Uncapped Acts")] 
    public static bool UncappedActsEnabled { get; set; } = true;
    [ConfigHoverTip]
    public static ScalingDifficulty ScalingDifficulty { get; set; } = ScalingDifficulty.Normal;
    [ConfigVisibleIf(nameof(ScalingDifficulty), ScalingDifficulty.Custom)]
    [ConfigSlider(min: 1, max: 20, step: 0.1, Format = "x{0:F1}")]
    public static float CustomHpScaling { get; set; } = 6f;
    [ConfigVisibleIf(nameof(ScalingDifficulty), ScalingDifficulty.Custom)]
    [ConfigSlider(min: 1, max: 20, step: 0.1, Format = "x{0:F1}")]
    public static float CustomDmgScaling { get; set; } = 2.4f;
    public static bool ActThreeBossRewardsEnabled { get; set; } = true;
    
    [ConfigIgnore]
    public static float HpScaling => GetHpScaling();
    [ConfigIgnore]
    public static float DmgScaling => GetDmgScaling();

    private static float GetHpScaling()
    {
        return _scalingDiffultyDefaults.TryGetValue(ScalingDifficulty, out var scaling) 
            ? scaling.hpScaling 
            : CustomHpScaling;
    }
    
    private static float GetDmgScaling()
    {
        return _scalingDiffultyDefaults.TryGetValue(ScalingDifficulty, out var scaling) 
            ? scaling.dmgScaling 
            : CustomDmgScaling;
    }
    
    private static Dictionary<ScalingDifficulty, (float hpScaling, float dmgScaling)> _scalingDiffultyDefaults = new()
    {
        [ScalingDifficulty.Easy] = (3f, 1.2f),
        [ScalingDifficulty.Normal] = (6f, 2.4f),
        [ScalingDifficulty.Hard] = (9f, 3.6f)
    };
    #endregion
    
    #region Uncapped Enchantments
    [ConfigSection("Uncapped Enchantments")]
    public static bool UncappedEnchantmentsEnabled { get; set; } = true;
    #endregion
    
    #region Uncapped Upgrades
    private static bool _uncappedUpgradesEnabled = true;

    [ConfigSection("Uncapped Upgrades")]
    public static bool UncappedUpgradesEnabled
    {
        get => _uncappedUpgradesEnabled;
        set
        {
            _uncappedUpgradesEnabled = value;
            UpgradeContext.UpdateEnabled();
        }
    }
    #endregion
    
    #region Uncapped Relics
    [ConfigSection("Uncapped Relics")]
    public static bool UncappedRelicsEnabled { get; set; } = true;
    #endregion
    
    #region Uncapped Animations
    [ConfigSection("Uncapped Animations")]
    public static bool UncappedAnimationsEnabled { get; set; } = true;
    [ConfigHoverTip]
    [ConfigSlider(min: 1, max: 100, step: 1)]
    public static int AnimationBatchSize { get; set; } = 25;
    #endregion
}

internal enum ScalingDifficulty
{
    Easy,
    Normal,
    Hard,
    Custom
}