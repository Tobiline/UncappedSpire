using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Runs;
using UncappedSpire.UncappedSpireCode.UncappedActs.RunManagerPatches;

namespace UncappedSpire.UncappedSpireCode.UncappedActs;

public class VoteToMoveToNextChapterAction : GameAction
{
    /// <summary>
    /// The player who is voting.
    /// </summary>
    private readonly Player _player;

    public override ulong OwnerId => _player.NetId;

    public override GameActionType ActionType => GameActionType.NonCombat;

    public VoteToMoveToNextChapterAction(Player player)
    {
        _player = player;
    }

    protected override Task ExecuteAction()
    {
        SpireFields_RunManager.ChapterChangeSynchronizer.Get(RunManager.Instance)!.OnPlayerReady(_player);
        return Task.CompletedTask;
    }

    public override INetAction ToNetAction()
    {
        return default(NetVoteToMoveToNextChapterAction);
    }

    public override string ToString()
    {
        return $"{"VoteForMapCoordAction"} {_player.NetId}";
    }
}