namespace UncappedSpire.UncappedSpireCode.UncappedUpgrades;

public static class UpgradeContext
{
    private static int _multiplier = 1;
    public static event Action<int>? MultiplierChanged;

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