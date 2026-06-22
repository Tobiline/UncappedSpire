using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Unlocks;

namespace UncappedSpire.UncappedSpireCode.UncappedActs;

public class ChapterChangeSynchronizer : IDisposable
{
    private readonly INetGameService _gameService;
    
    private readonly RunLocationTargetedMessageBuffer _messageBuffer;

    private readonly RunState _runState;
    
    private readonly ulong _localPlayerId;

    private Player LocalPlayer => _runState.GetPlayer(_localPlayerId)!;
    
    public ChapterChangeSynchronizer(RunLocationTargetedMessageBuffer messageBuffer, INetGameService gameService, RunState runState, ulong localPlayerId)
    {
        _runState = runState;
        _messageBuffer = messageBuffer;
        _localPlayerId = localPlayerId;
        _gameService = gameService;
        _runState = runState;
        _messageBuffer.RegisterMessageHandler<ChapterChangeMessage>(HandleChapterChangeMessage);
    }
    
    public void Dispose()
    {
        _messageBuffer.UnregisterMessageHandler<ChapterChangeMessage>(HandleChapterChangeMessage);
    }
    
    public bool DoLocalSeedChange(string seed)
    {
        var message = new ChapterChangeMessage
        {
            seed = seed,
            location = _messageBuffer.CurrentLocation
        };
        _gameService.SendMessage(message);
        return DoSeedChange(LocalPlayer, seed);
    }
    
    private void HandleChapterChangeMessage(ChapterChangeMessage message, ulong senderId)
    {
        var player = _runState.GetPlayer(senderId)!;
        if (player == LocalPlayer)
        {
            throw new InvalidOperationException("SeedChangeSynchronizer should not be sent to the Host!");
        }
        _ = DoSeedChange(player, message.seed);
    }
    
    private bool DoSeedChange(Player player, string seed)
    {
        var Method_set_Rng = AccessTools.PropertySetter(typeof(RunState), nameof(RunState.Rng));
        var runRngSet = new RunRngSet(seed);
        Method_set_Rng.Invoke(player.RunState, [runRngSet]);
        
        player.InitializeSeed(seed);
        
        var rng = new Rng((uint)StringHelper.GetDeterministicHashCode(seed));
        var acts = ActModel.GetRandomList(rng, UnlockState.all, _gameService.Type.IsMultiplayer());
        var mutableActs = acts.Select(a => a.ToMutable()).ToList();
        foreach (var act in mutableActs)
        {
            act.AssertMutable();
        }
        
        var actsSetter = AccessTools.PropertySetter(typeof(RunState), nameof(RunState.Acts));
        actsSetter.Invoke(player.RunState, [mutableActs]);
        
        RunManager.Instance.GenerateRooms();
        
        return true;
    }
}