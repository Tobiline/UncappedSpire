namespace UncappedSpire.UncappedSpireCode.LimitlessUpgrades;

public static class UpgradeContext
{
    private static AsyncLocal<int> _localMultiplier = new();

    public static int GetMultiplier()
    {
        return _localMultiplier.Value;
    }

    public static void AddOrUpdateMultiplier(int multiplier)
    {
        _localMultiplier.Value = multiplier;
    }

    public static void RemoveMultiplier()
    {
        _localMultiplier.Value = 1;
    }
}