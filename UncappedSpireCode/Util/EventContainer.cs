namespace UncappedSpire.UncappedSpireCode.Util;

public class EventContainer
{
    public event Action? Changed;

    public void Raise()
    {
        Changed?.Invoke();
    }
}