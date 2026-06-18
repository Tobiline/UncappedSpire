using System.Globalization;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;
using UncappedSpire.UncappedSpireCode.Config;

namespace UncappedSpire.UncappedSpireCode.UncappedActs;

public class UncappedActs : ModifierModel
{
    static UncappedActs()
    {
        SavedPropertiesTypeCache.InjectTypeIntoCache(typeof(UncappedActs));
    }
    
    protected override string IconPath => "res://UncappedSpire/images/modifiers/uncapped_acts.png";
    
    public override LocString Description
    {
        get
        {
            var baseString = new LocString("modifiers", Id.Entry + ".description");
            baseString.Add("CurrentChapter", CurrentChapter);
            baseString.Add("ScalingHpIncrement", ScalingHpIncrement.ToString("F1"));
            baseString.Add("ScalingDmgIncrement", ScalingDmgIncrement.ToString("F1"));
            baseString.Add("ScalingHp", ChapterManager.Current_ScalingHp.ToString("G3"));
            baseString.Add("ScalingDmg", ChapterManager.Current_ScalingDmg.ToString("G3"));
            
            return baseString;
        }
    }

    private int _currentChapter = 1;
    
    [SavedProperty]
    public int CurrentChapter
    {
        get => _currentChapter;
        set
        {
            _currentChapter = value;
            SetChapter();
            SetScalingHp();
            SetScalingDmg();
        }
    }
    
    [SavedProperty] 
    public string SerializedScalingHpIncrement { get; set; } = "1";
    public float ScalingHpIncrement
    {
        get => float.Parse(SerializedScalingHpIncrement, CultureInfo.InvariantCulture);
        set
        {
            SerializedScalingHpIncrement = value.ToString("R", CultureInfo.InvariantCulture);
            SetScalingHp();
        }
    }

    [SavedProperty] 
    public string SerializedScalingDmgIncrement { get; set; } = "1";
    public float ScalingDmgIncrement
    {
        get => float.Parse(SerializedScalingDmgIncrement, CultureInfo.InvariantCulture);
        set
        {
            SerializedScalingDmgIncrement = value.ToString("R", CultureInfo.InvariantCulture);
            SetScalingHp();
        }
    }

    private void SetChapter()
    {
        ChapterManager.Current_Chapter = CurrentChapter;
    }
    private void SetScalingHp()
    {
        var newScalingHp = (float)Math.Pow(ScalingHpIncrement, CurrentChapter - 1);
        ChapterManager.Current_ScalingHp = newScalingHp;
    }
    private void SetScalingDmg()
    {
        var newScalingDmg = (float)Math.Pow(ScalingDmgIncrement, CurrentChapter - 1);
        ChapterManager.Current_ScalingDmg = newScalingDmg;
    }
    
    protected override void AfterRunLoaded(RunState runState)
    {
        SetChapter();
        SetScalingHp();
        SetScalingDmg();
        MainFile.Logger.Info($"Loaded UncappedActs Save, Chapter: {CurrentChapter}, ScalingHp: {ChapterManager.Current_ScalingHp}, ScalingDmg: {ChapterManager.Current_ScalingDmg}");
    }

    protected override void AfterRunCreated(RunState runState)
    {
        SerializedScalingHpIncrement = UncappedConfig.HpScaling.ToString("R", CultureInfo.InvariantCulture);
        SerializedScalingDmgIncrement = UncappedConfig.DmgScaling.ToString("R", CultureInfo.InvariantCulture);
    }
}