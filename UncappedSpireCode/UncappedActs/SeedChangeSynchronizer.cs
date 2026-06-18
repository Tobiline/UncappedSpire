using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Runs;

namespace UncappedSpire.UncappedSpireCode.UncappedActs;

public class SeedChangeSynchronizer : IDisposable
{
    private readonly RunLocationTargetedMessageBuffer _messageBuffer;
    
    private readonly INetGameService _gameService;

    private readonly IPlayerCollection _playerCollection;
    
    private readonly ulong _localPlayerId;

    private Player LocalPlayer => _playerCollection.GetPlayer(_localPlayerId);
    
    public SeedChangeSynchronizer(RunLocationTargetedMessageBuffer messageBuffer, INetGameService gameService, IPlayerCollection playerCollection, ulong localPlayerId)
    {
        _playerCollection = playerCollection;
        _localPlayerId = localPlayerId;
        _gameService = gameService;
        _messageBuffer = messageBuffer;
        messageBuffer.RegisterMessageHandler<SeedChangeMessage>(HandleSeedChange);
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
        var player = _playerCollection.GetPlayer(senderId);
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
        return true;
    }
}