using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace UncappedSpire.UncappedSpireCode.UncappedActs;

// TODO: Default it if out of run
public static class ChapterManager
{
    public static float Config_ScalingHp { get; private set; } = 6f;
    public static float Config_ScalingDmg { get; private set; } = 2.4f;

    public static int Current_Chapter { get; set; } = 1;
    public static float Current_ScalingHp { get; set; } = 1f;
    public static float Current_ScalingDmg { get; set; } = 1f;
    
    public static float GetScaling(ScalingType scalingType)
    {
        return scalingType switch
        {
            ScalingType.Hp => Current_ScalingHp,
            ScalingType.Dmg => Current_ScalingDmg,
            _ => 1f
        };
    }

    public static PowerModel? JustRemovedPower { get; set; }

    public static bool TryGetScaling(this PowerModel powerModel, ScalingImplementationType implementationType, out float scaling)
    {
        scaling = 1f;
        var justRemovedPower = JustRemovedPower;
        JustRemovedPower = null;
        var powerType = powerModel.GetType();
        
        if (!PowerScalingImplementationTypes.TryGetValue(powerType, out var impType))
            return false;

        if (impType != implementationType)
            return false;
        
        // This stop temp strength regain from being scaled like from Mangle at end of turn
        if (implementationType == ScalingImplementationType.DataModify && justRemovedPower != null 
            && PowerScalingImplementationTypes.TryGetValue(justRemovedPower.GetType(), out var removedPowerImpType)
            && removedPowerImpType == ScalingImplementationType.TemporaryDataModify)
            return false;

        if (!PowerScalingTypes.TryGetValue(powerType, out var scalingType))
            return false;
        
        scaling = GetScaling(scalingType);
        return true;
    }

    public static Dictionary<Type, ScalingType> PowerScalingTypes = new()
    {
        // Dmg
        [typeof(ThieveryPower)] = ScalingType.Dmg, // LOL
        [typeof(RitualPower)] = ScalingType.Dmg,
        [typeof(RavenousPower)] = ScalingType.Dmg,
        [typeof(HighVoltagePower)] = ScalingType.Dmg,
        [typeof(EnragePower)] = ScalingType.Dmg,
        [typeof(CrabRagePower)] = ScalingType.Dmg,
        [typeof(TerritorialPower)] = ScalingType.Dmg,
        [typeof(ConstrictPower)] = ScalingType.Dmg,
        [typeof(StrengthPower)] = ScalingType.Dmg,
        [typeof(ThornsPower)] = ScalingType.Dmg,
        [typeof(GalvanicPower)] = ScalingType.Dmg,
        [typeof(SteamEruptionPower)] = ScalingType.Dmg,
        [typeof(SuckPower)] = ScalingType.Dmg,
        [typeof(VigorPower)] = ScalingType.Dmg,
        
        // Hp
        [typeof(PlatingPower)] = ScalingType.Hp,
        [typeof(ReattachPower)] = ScalingType.Hp,
        [typeof(PlowPower)] = ScalingType.Hp,
        [typeof(ShriekPower)] = ScalingType.Hp,
        [typeof(SkittishPower)] = ScalingType.Hp,
        [typeof(HardenedShellPower)] = ScalingType.Hp,
        [typeof(HardToKillPower)] = ScalingType.Hp,
        [typeof(RampartPower)] = ScalingType.Hp,
        [typeof(DexterityPower)] = ScalingType.Hp,
        [typeof(CurlUpPower)] = ScalingType.Hp,
    };

    public static Dictionary<Type, ScalingImplementationType> PowerScalingImplementationTypes = new()
    {
        // Data Modify
        [typeof(ThieveryPower)] = ScalingImplementationType.DataModify,
        [typeof(StrengthPower)] = ScalingImplementationType.DataModify,
        [typeof(ThornsPower)] = ScalingImplementationType.DataModify,
        [typeof(GalvanicPower)] = ScalingImplementationType.DataModify,
        [typeof(ReattachPower)] = ScalingImplementationType.DataModify,
        [typeof(PlowPower)] = ScalingImplementationType.DataModify,
        [typeof(ShriekPower)] = ScalingImplementationType.DataModify,
        [typeof(HardenedShellPower)] = ScalingImplementationType.DataModify,
        [typeof(HardToKillPower)] = ScalingImplementationType.DataModify,
        [typeof(DexterityPower)] = ScalingImplementationType.DataModify,
        [typeof(VigorPower)] = ScalingImplementationType.DataModify,
        
        // Display Modify
        [typeof(RitualPower)] = ScalingImplementationType.DisplayModify,
        [typeof(RavenousPower)] = ScalingImplementationType.DisplayModify,
        [typeof(HighVoltagePower)] = ScalingImplementationType.DisplayModify,
        [typeof(EnragePower)] = ScalingImplementationType.DisplayModify,
        [typeof(CrabRagePower)] = ScalingImplementationType.DisplayModify,
        [typeof(TerritorialPower)] = ScalingImplementationType.DisplayModify,
        [typeof(PlatingPower)] = ScalingImplementationType.DisplayModify,
        [typeof(SkittishPower)] = ScalingImplementationType.DisplayModify,
        [typeof(CurlUpPower)] = ScalingImplementationType.DisplayModify,
        [typeof(RampartPower)] = ScalingImplementationType.DisplayModify,
        [typeof(SuckPower)] = ScalingImplementationType.DisplayModify,
        [typeof(SteamEruptionPower)] = ScalingImplementationType.DisplayModify,
        
        // Temporary Data Modify (Does not need PowerScalingTypes entry)
        [typeof(TemporaryStrengthPower)] = ScalingImplementationType.TemporaryDataModify,
        [typeof(TemporaryDexterityPower)] = ScalingImplementationType.TemporaryDataModify,
        
        // Non-Self Applied Data Modify
        [typeof(ConstrictPower)] = ScalingImplementationType.NonSelfAppliedDataModify,
    };

    public static Dictionary<string, ScalingType> ScalingStatusDynamicVarKeys = new()
    {
        ["Damage"] = ScalingType.Dmg,
    };

    public static HashSet<string> LocStringVariablesToScale =
    [
        "Amount",
        "Decrement",
        "StrengthPower",
        "Block"
    ];
}

public enum ScalingType
{
    Hp,
    Dmg
}

public enum ScalingImplementationType
{
    DataModify,
    DisplayModify,
    TemporaryDataModify,
    NonSelfAppliedDataModify,
}