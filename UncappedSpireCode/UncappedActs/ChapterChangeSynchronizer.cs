using HarmonyLib;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Nodes.Screens;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Unlocks;

namespace UncappedSpire.UncappedSpireCode.UncappedActs;

public class ChapterChangeSynchronizer : IDisposable
{
    private readonly RunLocationTargetedMessageBuffer _messageBuffer;
    
    private readonly INetGameService _gameService;

    private readonly RunState _runState;
    
    private readonly ulong _localPlayerId;
    
    private readonly List<bool> _readyPlayers = [];

    private Player LocalPlayer => _runState.GetPlayer(_localPlayerId);
    
    public ChapterChangeSynchronizer(RunLocationTargetedMessageBuffer messageBuffer, INetGameService gameService, RunState runState, ulong localPlayerId)
    {
        _runState = runState;
        _localPlayerId = localPlayerId;
        _gameService = gameService;
        _messageBuffer = messageBuffer;
        messageBuffer.RegisterMessageHandler<SeedChangeMessage>(HandleSeedChange);
        _runState = runState;
        for (int i = 0; i < runState.Players.Count; i++)
        {
            _readyPlayers.Add(item: false);
        }
    }
    
    public void SetLocalPlayerReady()
    {
        MainFile.Logger.Info("Local player ready to move to next chapter");
        RunManager.Instance.ActionQueueSynchronizer.RequestEnqueue(new VoteToMoveToNextChapterAction(LocalContext.GetMe(_runState)));
    }
    
    public bool IsWaitingForOtherPlayers()
    {
        var playerSlotIndex = _runState.GetPlayerSlotIndex(LocalContext.NetId.Value);
        for (var i = 0; i < _readyPlayers.Count; i++)
        {
            if (!_readyPlayers[i] && i != playerSlotIndex)
            {
                return true;
            }
        }
        return false;
    }
    
    public void OnPlayerReady(Player player)
    {
        MainFile.Logger.Debug($"Player {player.NetId} ready to move to next chapter");
        var playerSlotIndex = _runState.GetPlayerSlotIndex(player);
        _readyPlayers[playerSlotIndex] = true;
        if (_readyPlayers.All(x => x))
        {
            MoveToNextChapter();
        }
    }
    
    private void MoveToNextChapter()
    {
        for (var i = 0; i < _readyPlayers.Count; i++)
        {
            _readyPlayers[i] = false;
        }
        MainFile.Logger.Info("All players ready to move to next chapter, beginning transition");
        TaskHelper.RunSafely(UncappedActsCore.EnterNextChapter());
        if (NOverlayStack.Instance?.Peek() is NRewardsScreen nRewardsScreen)
        {
            nRewardsScreen.HideWaitingForPlayersScreen();
        }
    }
    
    public void Dispose()
    {
        _messageBuffer.UnregisterMessageHandler<SeedChangeMessage>(HandleSeedChange);
    }
    
    public async Task<bool> DoLocalSeedChange(string seed)
    {
        var message = new SeedChangeMessage
        {
            seed = seed,
            Location = _messageBuffer.CurrentLocation
        };
        _gameService.SendMessage(message);
        return await DoSeedChange(LocalPlayer, seed);
    }
    
    private void HandleSeedChange(SeedChangeMessage message, ulong senderId)
    {
        var player = _runState.GetPlayer(senderId);
        if (player == LocalPlayer)
        {
            throw new InvalidOperationException("SeedChangeSynchronizer should not be sent to the Host!");
        }
        TaskHelper.RunSafely(DoSeedChange(player, message.seed));
    }
    
    private async Task<bool> DoSeedChange(Player player, string seed)
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