namespace UncappedSpire.UncappedSpireCode.UncappedUpgrades;

public static class UpgradeContext
{
    private static int _multiplier;
    public static event Action<int>? MultiplierChanged;
    public static event Action? EnabledInConfig;

    public static void UpdateEnabled()
    {
        EnabledInConfig?.Invoke();
    }
    
    public static int GetMultiplier()
    {
        return _multiplier;
    }

    public static void UpdateMultiplier(int multiplier, bool updateInputs = true)
    {
        if (_multiplier == multiplier)
            return;
        
        _multiplier = multiplier;
        if (updateInputs)
        {
            MultiplierChanged?.Invoke(multiplier);
        }
    }
}