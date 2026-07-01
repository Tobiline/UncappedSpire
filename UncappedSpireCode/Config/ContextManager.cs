using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Runs;

namespace UncappedSpire.UncappedSpireCode.Config;

// Holds the UncappedActs context globally
public static class ContextManager
{
    public static UncappedSpireModifier? State { get; set; }
    
    private static MethodInfo Method_get_State = AccessTools.PropertyGetter(typeof(RunManager), "State");
    public static bool IsCurrentlyInRun => Method_get_State.Invoke(RunManager.Instance, null) != null;
    
    // Assume all features are on when serializing as State hasn't been populated yet
    public static bool IsInitializing { get; set; }
    public static bool UncappedActsEnabled => IsInitializing || (IsCurrentlyInRun ? State!.UncappedActsEnabled : UncappedConfig.UncappedActsEnabled);
    public static bool UncappedEnchantmentsEnabled => IsInitializing || (IsCurrentlyInRun ? State!.UncappedEnchantmentsEnabled : UncappedConfig.UncappedEnchantmentsEnabled);
    public static bool UncappedUpgradesEnabled => IsInitializing || (IsCurrentlyInRun ? State!.UncappedUpgradesEnabled : UncappedConfig.UncappedUpgradesEnabled);
    public static bool UncappedRelicsEnabled => IsInitializing || TEMP_UncappedRelicsEnabled();

    // TODO: Remove
    private static bool TEMP_UncappedRelicsEnabled()
    {
        if (RunManager.Instance.NetService.Type == NetGameType.Singleplayer || CommandLineHelper.HasArg("fastmp"))
        {
            return IsCurrentlyInRun ? State!.UncappedRelicsEnabled : UncappedConfig.UncappedRelicsEnabled;
        }

        return false;
    }
    
    public static bool ActThreeBossRewardsEnabled => IsCurrentlyInRun ? State!.ActThreeBossRewardsEnabled : UncappedConfig.ActThreeBossRewardsEnabled;
    public static int Current_Chapter => IsCurrentlyInRun ? State!.CurrentChapter : 1;
    public static float Current_ScalingHp => IsCurrentlyInRun ? State!.GetHpScaling() : 1;
    public static float Current_ScalingDmg => IsCurrentlyInRun ? State!.GetDmgScaling() : 1;
    
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
            && justRemovedPower.TryGetTemporaryPowerBaseClass(out var tempPowerType)
            && PowerScalingImplementationTypes.TryGetValue(tempPowerType!, out var removedPowerImpType)
            && removedPowerImpType == ScalingImplementationType.TemporaryDataModify)
            return false;

        if (!PowerScalingTypes.TryGetValue(powerType, out var scalingType))
            return false;
        
        scaling = GetScaling(scalingType);
        return true;
    }

    public static bool TryGetTemporaryPowerBaseClass(this PowerModel powerModel, out Type? temporaryClassType)
    {
        temporaryClassType = null;
        var powerModelType = powerModel.GetType();
        
        if (!typeof(ITemporaryPower).IsAssignableFrom(powerModelType))
            return false;
        
        temporaryClassType = powerModelType.BaseType == typeof(PowerModel) ? powerModelType : powerModelType.BaseType;

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
        [typeof(VitalSparkPower)] = ScalingType.Dmg,
        
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
        [typeof(VitalSparkPower)] = ScalingImplementationType.DataModify,
        
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