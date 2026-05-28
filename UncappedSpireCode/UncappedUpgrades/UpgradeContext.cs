namespace UncappedSpire.UncappedSpireCode.UncappedUpgrades;

public static class UpgradeContext
{
    private static AsyncLocal<int> _localMultiplier = new();

    public static int GetMultiplier()
    {
        MainFile.Logger.Info("Obtained Multiplier: " + _localMultiplier.Value);
        return _localMultiplier.Value;
    }

    public static void AddOrUpdateMultiplier(int multiplier)
    {
        MainFile.Logger.Info("Added / Updated Multiplier: " + multiplier);
        _localMultiplier.Value = multiplier;
    }

    public static void RemoveMultiplier()
    {
        MainFile.Logger.Info("Removed Multiplier: " + _localMultiplier.Value);
        _localMultiplier.Value = 1;
    }
}