using MegaCrit.Sts2.Core.Models.Powers;

namespace UncappedSpire.UncappedSpireCode.UncappedActs;

// TODO: Default it if out of run
public static class ChapterManager
{
    public static float Config_ScalingHp { get; private set; } = 6f;
    public static float Config_ScalingDmg { get; private set; } = 2.4f;
    public static float Session_ScalingHp { get; set; } = 1000f;
    public static float Session_ScalingDmg { get; set; } = 1000f;

    public static float GetScaling(ScaleType scaleType)
    {
        return scaleType switch
        {
            ScaleType.Dmg => Session_ScalingDmg,
            ScaleType.Hp => Session_ScalingHp,
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