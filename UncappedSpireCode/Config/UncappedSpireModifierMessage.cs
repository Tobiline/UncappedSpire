using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Game;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Runs;

namespace UncappedSpire.UncappedSpireCode.Config;

public class UncappedSpireModifierMessage : INetMessage, IPacketSerializable, IRunLocationTargetedMessage
{
    public bool uncappedActsEnabled;
    public bool uncappedEnchantmentsEnabled;
    public bool uncappedUpgradesEnabled;
    public bool actThreeBossRewardsEnabled;
    public float scalingHpIncrement;
    public float scalingDmgIncrement;
    
    public RunLocation location;
    public RunLocation Location => location;
    
    public bool ShouldBroadcast => true;
    public NetTransferMode Mode => NetTransferMode.Reliable;
    public LogLevel LogLevel => LogLevel.VeryDebug;
    public bool ShouldBuffer => true;
    
    public void Serialize(PacketWriter writer)
    {
        writer.WriteBool(uncappedActsEnabled);
        writer.WriteBool(uncappedEnchantmentsEnabled);
        writer.WriteBool(uncappedUpgradesEnabled);
        writer.WriteBool(actThreeBossRewardsEnabled);
        writer.WriteFloat(scalingHpIncrement);
        writer.WriteFloat(scalingDmgIncrement);
        
        writer.Write(location);
    }

    public void Deserialize(PacketReader reader)
    {
        uncappedActsEnabled = reader.ReadBool();
        uncappedEnchantmentsEnabled = reader.ReadBool();
        uncappedUpgradesEnabled = reader.ReadBool();
        actThreeBossRewardsEnabled = reader.ReadBool();
        scalingHpIncrement = reader.ReadFloat();
        scalingDmgIncrement = reader.ReadFloat();
        
        location = reader.Read<RunLocation>();
    }
    
    public override string ToString()
    {
        return @$"UncappedSpireModifierMessage 
uncappedActsEnabled {uncappedActsEnabled}
uncappedEnchantmentsEnabled {uncappedEnchantmentsEnabled}
uncappedUpgradesEnabled {uncappedUpgradesEnabled}
actThreeBossRewardsEnabled {actThreeBossRewardsEnabled}
scalingHpIncrement {scalingHpIncrement}
serializedScalingDmgIncrement {scalingDmgIncrement}";
    }
}