using System.Globalization;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace UncappedSpire.UncappedSpireCode.Config;

// This acts as the UncappedSpire specific runstate cache

// Config is saved at the time of creation to prevent any
// bugs of changing the config after the run has already started
public class UncappedSpireModifier : ModifierModel
{
    static UncappedSpireModifier()
    {
        SavedPropertiesTypeCache.InjectTypeIntoCache(typeof(UncappedSpireModifier));
    }
    
    protected override string IconPath => "res://images/ui/main_menu/patch_notes_icon.png";
    
    public override LocString Description
    {
        get
        {
            var baseString = new LocString("modifiers", Id.Entry + ".description");
            baseString.Add("UncappedActsEnabled",ContextManager.UncappedActsEnabled ? "[green]✓[/green]" : "[red]✗[/red]");
            baseString.Add("UncappedEnchantmentsEnabled",ContextManager.UncappedEnchantmentsEnabled ? "[green]✓[/green]" : "[red]✗[/red]");
            baseString.Add("UncappedUpgradesEnabled",ContextManager.UncappedUpgradesEnabled ? "[green]✓[/green]" : "[red]✗[/red]");
            baseString.Add("UncappedRelicsEnabled",ContextManager.UncappedRelicsEnabled ? "[green]✓[/green]" : "[red]✗[/red]");
            baseString.Add("ActThreeBossRewardsEnabled", ContextManager.ActThreeBossRewardsEnabled ? "[green]✓[/green]" : "[red]✗[/red]");
            
            baseString.Add("ActsEnabled", ContextManager.UncappedActsEnabled);
            baseString.Add("CurrentChapter", CurrentChapter.ToString());
            baseString.Add("ScalingHp",ContextManager.Current_ScalingHp.ToString("G3"));
            baseString.Add("ScalingDmg",ContextManager.Current_ScalingDmg.ToString("G3"));
            baseString.Add("ScalingHpIncrement",ScalingHpIncrement.ToString("F1"));
            baseString.Add("ScalingDmgIncrement",ScalingDmgIncrement.ToString("F1"));
            
            return baseString;
        }
    }

    [SavedProperty] 
    public bool UncappedActsEnabled { get; set; } = true;
    [SavedProperty]
    public bool UncappedEnchantmentsEnabled { get; set; } = true;
    [SavedProperty] 
    public bool UncappedUpgradesEnabled { get; set; } = true;
    [SavedProperty] 
    public bool UncappedRelicsEnabled { get; set; } = true;

    [SavedProperty]
    public bool ActThreeBossRewardsEnabled { get; set; } = true;
    [SavedProperty] 
    public int CurrentChapter { get; set; } = 1;
    
    [SavedProperty] 
    public string SerializedScalingHpIncrement { get; set; } = "1";
    public float ScalingHpIncrement
    {
        get => float.Parse(SerializedScalingHpIncrement, CultureInfo.InvariantCulture);
        set
        {
            SerializedScalingHpIncrement = value.ToString("R", CultureInfo.InvariantCulture);
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
        }
    }

    public float GetHpScaling()
    {
        return (float)Math.Pow(ScalingHpIncrement, CurrentChapter - 1);
    }

    public float GetDmgScaling()
    {
        return (float)Math.Pow(ScalingDmgIncrement, CurrentChapter - 1);
    }
    
    protected override void AfterRunLoaded(RunState runState)
    {
        ContextManager.State = this;
    }

    protected override void AfterRunCreated(RunState runState)
    {
        ContextManager.State = this;
        
        UncappedActsEnabled = UncappedConfig.UncappedActsEnabled;
        UncappedEnchantmentsEnabled = UncappedConfig.UncappedEnchantmentsEnabled;
        UncappedUpgradesEnabled = UncappedConfig.UncappedUpgradesEnabled;
        UncappedRelicsEnabled = UncappedConfig.UncappedRelicsEnabled;
        
        ActThreeBossRewardsEnabled = UncappedConfig.ActThreeBossRewardsEnabled;
        
        SerializedScalingHpIncrement = UncappedConfig.HpScaling.ToString("R", CultureInfo.InvariantCulture);
        SerializedScalingDmgIncrement = UncappedConfig.DmgScaling.ToString("R", CultureInfo.InvariantCulture);
    }

    public override string ToString()
    {
        return @$"UncappedActsEnabled: {UncappedActsEnabled}
UncappedEnchantmentsEnabled: {UncappedEnchantmentsEnabled}
UncappedUpgradesEnabled: {UncappedUpgradesEnabled}
UncappedRelicsEnabled: {UncappedRelicsEnabled}
ActThreeBossRewardsEnabled: {ActThreeBossRewardsEnabled}
CurrentChapter: {CurrentChapter}
SerializedScalingHpIncrement: {SerializedScalingHpIncrement}
SerializedScalingDmgIncrement: {SerializedScalingDmgIncrement}";
    }
}