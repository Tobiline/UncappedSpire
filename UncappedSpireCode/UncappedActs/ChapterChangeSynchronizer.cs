using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Runs;
using UncappedSpire.UncappedSpireCode.UncappedActs;

namespace UncappedSpire.UncappedSpireCode;

public class ChapterChangeSynchronizer : IDisposable
{
    private readonly RunLocationTargetedMessageBuffer _messageBuffer;
    
    private readonly INetGameService _gameService;

    private readonly IPlayerCollection _playerCollection;
    
    private readonly ulong _localPlayerId;

    private Player LocalPlayer => _playerCollection.GetPlayer(_localPlayerId);
    
    public ChapterChangeSynchronizer(RunLocationTargetedMessageBuffer messageBuffer, INetGameService gameService, IPlayerCollection playerCollection, ulong localPlayerId)
    {
        _playerCollection = playerCollection;
        _localPlayerId = localPlayerId;
        _gameService = gameService;
        _messageBuffer = messageBuffer;
        messageBuffer.RegisterMessageHandler<ChapterChangeMessage>(HandleSeedChange);
    }
    
    public void Dispose()
    {
        _messageBuffer.UnregisterMessageHandler<ChapterChangeMessage>(HandleSeedChange);
    }
    
    public Task<bool> DoLocalSeedChange(string seed, float scalingHp, float scalingDmg)
    {
        var message = new ChapterChangeMessage
        {
            seed = seed,
            scalingHp = scalingHp,
            scalingDmg = scalingDmg,
            Location = _messageBuffer.CurrentLocation
        };
        _gameService.SendMessage(message);
        return DoSeedChange(LocalPlayer, seed, scalingHp, scalingDmg);
    }
    
    private void HandleSeedChange(ChapterChangeMessage message, ulong senderId)
    {
        var player = _playerCollection.GetPlayer(senderId);
        if (player == LocalPlayer)
        {
            throw new InvalidOperationException("SeedChangeSynchronizer should not be sent to the Host!");
        }
        TaskHelper.RunSafely(DoSeedChange(player, message.seed, message.scalingHp, message.scalingDmg));
    }
    
    private async Task<bool> DoSeedChange(Player player, string seed, float scalingHp, float scalingDmg)
    {
        var Method_set_Rng = AccessTools.PropertySetter(typeof(RunState), nameof(RunState.Rng));
        var runRngSet = new RunRngSet(seed);
        Method_set_Rng.Invoke(player.RunState, [runRngSet]);
        player.InitializeSeed(seed);
        ChapterManager.Session_ScalingHp = scalingHp;
        ChapterManager.Session_ScalingDmg = scalingDmg;
        return true;
    }
}