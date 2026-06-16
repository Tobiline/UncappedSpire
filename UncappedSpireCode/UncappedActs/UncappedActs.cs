using System.Globalization;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;

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
            baseString.Add("ScalingHp", ChapterManager.Current_ScalingHp.ToString());
            baseString.Add("ScalingDmg", ChapterManager.Current_ScalingDmg.ToString());
            
            return baseString;
        }
    }

    private int _currentChapter = 1;
    //private float _scalingHpIncrement = 1f;
    //private float _scalingDmgIncrement = 1f;
    
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
    
    
    //public SavedSpireField<UncappedActs, float> SP_ScalingHpIncrement = new(() => 1f, "UncappedActs-SP_ScalingHpIncrement");
    
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
        // get => SP_ScalingHpIncrement.Get(this);
        // set
        // {
        //     
        //     SP_ScalingHpIncrement.Set(this, value);
        //     SetScalingHp();
        // }
    }
    //public SavedSpireField<UncappedActs, float> SP_ScalingDmgIncrement = new(() => 1f, "UncappedActs-SP_ScalingDmgIncrement");

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
        // get => SP_ScalingDmgIncrement.Get(this);
        // set
        // {
        //     SP_ScalingDmgIncrement.Set(this, value);
        //     SetScalingDmg();
        // }
    }

    private void SetChapter()
    {
        ChapterManager.Current_Chapter = CurrentChapter;
    }
    private void SetScalingHp()
    {
        var newScalingHp = CurrentChapter <= 1 ? 1 : (CurrentChapter - 1) * ScalingHpIncrement;
        ChapterManager.Current_ScalingHp = newScalingHp;
    }
    private void SetScalingDmg()
    {
        var newScalingDmg = CurrentChapter <= 1 ? 1 : (CurrentChapter - 1) * ScalingDmgIncrement;
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
        SerializedScalingHpIncrement = ChapterManager.Config_ScalingHp.ToString("R", CultureInfo.InvariantCulture);
        SerializedScalingDmgIncrement = ChapterManager.Config_ScalingDmg.ToString("R", CultureInfo.InvariantCulture);
        // SP_ScalingHpIncrement.Set(this, ChapterManager.Config_ScalingHp);
        // SP_ScalingDmgIncrement.Set(this, ChapterManager.Config_ScalingDmg);
    }
}