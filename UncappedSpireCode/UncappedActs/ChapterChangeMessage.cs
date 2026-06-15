using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Game;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Runs;

namespace UncappedSpire.UncappedSpireCode.UncappedActs;

public class ChapterChangeMessage : INetMessage, IPacketSerializable, IRunLocationTargetedMessage
{
    public string seed;
    public float scalingHp;
    public float scalingDmg;
    
    public RunLocation Location { get; set; }
    
    public bool ShouldBroadcast => true;
    public NetTransferMode Mode => NetTransferMode.Reliable;
    public LogLevel LogLevel => LogLevel.VeryDebug;
    
    public void Serialize(PacketWriter writer)
    {
        writer.WriteString(seed);
        writer.WriteFloat(scalingHp);
        writer.WriteFloat(scalingDmg);
        writer.Write(Location);
    }

    public void Deserialize(PacketReader reader)
    {
        seed = reader.ReadString();
        scalingHp = reader.ReadFloat();
        scalingDmg = reader.ReadFloat();
        Location = reader.Read<RunLocation>();
    }
    
    public override string ToString()
    {
        return $"SeedChangeMessage seed {seed}";
    }
}