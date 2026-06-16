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
    
    public static float GetScaling(ScaleType scaleType)
    {
        return scaleType switch
        {
            ScaleType.Hp => Current_ScalingHp,
            ScaleType.Dmg => Current_ScalingDmg,
            _ => 1f
        };
    }
    
    public static Dictionary<Type, ScaleType> MonsterScalingPowers = new()
    {
        [typeof(StrengthPower)] = ScaleType.Dmg,
        [typeof(ThieveryPower)] = ScaleType.Dmg, // TODO: Maybe remove
        [typeof(PlatingPower)] = ScaleType.Hp,
        [typeof(ThornsPower)] = ScaleType.Dmg,
        //[typeof(RitualPower)] = ScaleType.Dmg, // TODO: FIX THIS AS IT APPLIES STRENGTH POWER
        [typeof(GalvanicPower)] = ScaleType.Dmg,
        [typeof(ReattachPower)] = ScaleType.Hp,
        [typeof(PlowPower)] = ScaleType.Hp,
        [typeof(ShriekPower)] = ScaleType.Hp,
        [typeof(SteamEruptionPower)] = ScaleType.Dmg,
        [typeof(RavenousPower)] = ScaleType.Dmg,
        [typeof(SkittishPower)] = ScaleType.Hp,
        [typeof(HardenedShellPower)] = ScaleType.Hp,
        [typeof(HardToKillPower)] = ScaleType.Hp,
        [typeof(RampartPower)] = ScaleType.Hp,
        //[typeof(HighVoltagePower)] = ScaleType.Dmg, // TODO: FIX THIS AS IT APPLIES STRENGTH POWER
        [typeof(DexterityPower)] = ScaleType.Hp,
        //[typeof(EnragePower)] = ScaleType.Dmg, // TODO: FIX THIS AS IT APPLIES STRENGTH POWER
        [typeof(CurlUpPower)] = ScaleType.Hp,
        //[typeof(CrabRagePower)] = // TODO: FIX THIS AS IT APPLIES STRENGTH POWER
    };

    public static Dictionary<string, ScaleType> ScalingStatusDynamicVarKeys = new()
    {
        ["Damage"] = ScaleType.Dmg,
        ["DisintegrationPower"] = ScaleType.Dmg,
    };
}

public enum ScaleType
{
    Hp,
    Dmg
}