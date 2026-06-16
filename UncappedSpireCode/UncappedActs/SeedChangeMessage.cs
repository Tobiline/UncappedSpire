using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Game;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Runs;

namespace UncappedSpire.UncappedSpireCode.UncappedActs;

public class SeedChangeMessage : INetMessage, IPacketSerializable, IRunLocationTargetedMessage
{
    public string seed;
    
    public RunLocation Location { get; set; }
    
    public bool ShouldBroadcast => true;
    public NetTransferMode Mode => NetTransferMode.Reliable;
    public LogLevel LogLevel => LogLevel.VeryDebug;
    
    public void Serialize(PacketWriter writer)
    {
        writer.WriteString(seed);
        writer.Write(Location);
    }

    public void Deserialize(PacketReader reader)
    {
        seed = reader.ReadString();
        Location = reader.Read<RunLocation>();
    }
    
    public override string ToString()
    {
        return $"SeedChangeMessage seed {seed}";
    }
}