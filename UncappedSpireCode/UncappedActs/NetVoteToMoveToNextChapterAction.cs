using System.Runtime.InteropServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace UncappedSpire.UncappedSpireCode.UncappedActs;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public class NetVoteToMoveToNextChapterAction : INetAction, IPacketSerializable
{
    public GameAction ToGameAction(Player player)
    {
        return new VoteToMoveToNextChapterAction(player);
    }

    public void Serialize(PacketWriter writer)
    {
    }

    public void Deserialize(PacketReader reader)
    {
    }

    public override string ToString()
    {
        return "NetVoteForMapCoordAction";
    }
}