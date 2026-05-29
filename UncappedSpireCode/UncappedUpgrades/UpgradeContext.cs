namespace UncappedSpire.UncappedSpireCode.UncappedUpgrades;

public static class UpgradeContext
{
    private static int _multiplier = 1;
    private static readonly AsyncLocal<bool> _multiplierEnabled = new();
    public static event Action<int> MultiplierChanged;

    public static int GetMultiplier()
    {
        return _multiplierEnabled.Value ? _multiplier : 1;
    }

    /// <summary>
    /// Use only if you know what you're doing
    /// </summary>
    /// <returns></returns>
    public static int GetMultiplierRaw()
    {
        return _multiplier;
    }

    public static void UpdateMultiplier(int multiplier)
    {
        if (_multiplier == multiplier)
            return;
        
        _multiplier = multiplier;
        MultiplierChanged?.Invoke(multiplier);
    }

    public static void EnableMultiplier()
    {
        _multiplierEnabled.Value = true;
    }

    public static void DisableMultiplier()
    {
        _multiplierEnabled.Value = false;
    }
}