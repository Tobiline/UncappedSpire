using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Game;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Runs;

namespace UncappedSpire.UncappedSpireCode.UncappedActs;

public class ChapterChangeMessage : INetMessage, IPacketSerializable, IRunLocationTargetedMessage
{
    public required string seed;
    
    public  required RunLocation location;
    public RunLocation Location => location;
    public bool ShouldBroadcast => true;
    public NetTransferMode Mode => NetTransferMode.Reliable;
    public LogLevel LogLevel => LogLevel.VeryDebug;
    public bool ShouldBuffer => true;
    
    public void Serialize(PacketWriter writer)
    {
        writer.WriteString(seed);
        writer.Write(location);
    }

    public void Deserialize(PacketReader reader)
    {
        seed = reader.ReadString();
        location = reader.Read<RunLocation>();
    }
    
    public override string ToString()
    {
        return $"ChapterChangeMessage seed {seed}";
    }
}