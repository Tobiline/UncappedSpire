using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Runs;

namespace UncappedSpire.UncappedSpireCode.Config;

public class UncappedSpireModifierSynchronizer : IDisposable
{
    private readonly INetGameService _gameService;
    
    private readonly RunLocationTargetedMessageBuffer _messageBuffer;

    private readonly RunState _runState;
    
    private readonly ulong _localPlayerId;

    private Player LocalPlayer => _runState.GetPlayer(_localPlayerId);
    
    public UncappedSpireModifierSynchronizer(RunLocationTargetedMessageBuffer messageBuffer, INetGameService gameService, RunState runState, ulong localPlayerId)
    {
        _runState = runState;
        _messageBuffer = messageBuffer;
        _localPlayerId = localPlayerId;
        _gameService = gameService;
        _runState = runState;
        _messageBuffer.RegisterMessageHandler<UncappedSpireModifierMessage>(HandleChapterChangeMessage);
    }
    
    public void Dispose()
    {
        _messageBuffer.UnregisterMessageHandler<UncappedSpireModifierMessage>(HandleChapterChangeMessage);
    }
    
    public bool DoLocalUncappedModifierSet(
        bool uncappedActsEnabled,
        bool uncappedEnchantmentsEnabled,
        bool uncappedUpgradesEnabled,
        bool actThreeBossRewardsEnabled,
        float scalingHpIncrement,
        float scalingDmgIncrement)
    {
        var message = new UncappedSpireModifierMessage
        {
            uncappedActsEnabled = uncappedActsEnabled,
            uncappedEnchantmentsEnabled = uncappedEnchantmentsEnabled,
            uncappedUpgradesEnabled = uncappedUpgradesEnabled,
            actThreeBossRewardsEnabled = actThreeBossRewardsEnabled,
            scalingHpIncrement = scalingHpIncrement,
            scalingDmgIncrement = scalingDmgIncrement,
            location = _messageBuffer.CurrentLocation
        };
        _gameService.SendMessage(message);
        return DoUncappedModifierSet(LocalPlayer,
            uncappedActsEnabled,
            uncappedEnchantmentsEnabled,
            uncappedUpgradesEnabled,
            actThreeBossRewardsEnabled,
            scalingHpIncrement,
            scalingDmgIncrement);
    }
    
    private void HandleChapterChangeMessage(UncappedSpireModifierMessage message, ulong senderId)
    {
        var player = _runState.GetPlayer(senderId);
        if (player == LocalPlayer)
        {
            throw new InvalidOperationException("UncappedSpireModifierMessage should not be sent to the Host!");
        }
        _ = DoUncappedModifierSet(player, 
            message.uncappedActsEnabled,
            message.uncappedEnchantmentsEnabled,
            message.uncappedUpgradesEnabled,
            message.actThreeBossRewardsEnabled,
            message.scalingHpIncrement,
            message.scalingDmgIncrement);
    }
    
    private bool DoUncappedModifierSet(Player player,
        bool uncappedActsEnabled,
        bool uncappedEnchantmentsEnabled,
        bool uncappedUpgradesEnabled,
        bool actThreeBossRewardsEnabled,
        float scalingHpIncrement,
        float scalingDmgIncrement)
    {
        var uncappedSpireModifier = player.RunState.Modifiers.First(m => m is UncappedSpireModifier) as UncappedSpireModifier;
        if (uncappedSpireModifier == null)
        {
            throw new NullReferenceException($"UncappedSpireModifier does not exist in player {player.NetId} modifier list!");
        }
        
        uncappedSpireModifier.UncappedActsEnabled = uncappedActsEnabled;
        uncappedSpireModifier.UncappedEnchantmentsEnabled = uncappedEnchantmentsEnabled;
        uncappedSpireModifier.UncappedUpgradesEnabled = uncappedUpgradesEnabled;
        uncappedSpireModifier.ActThreeBossRewardsEnabled = actThreeBossRewardsEnabled;
        uncappedSpireModifier.ScalingHpIncrement = scalingHpIncrement;
        uncappedSpireModifier.ScalingDmgIncrement = scalingDmgIncrement;
        
        return true;
    }
}