using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Game.Lobby;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Runs;
using UncappedSpire.UncappedSpireCode.Config;
using UncappedSpire.UncappedSpireCode.UncappedActs.RunManagerPatches;

namespace UncappedSpire.UncappedSpireCode.UncappedActs.NGamePatches;

[HarmonyPatch(typeof(NGame), nameof(NGame.StartNewMultiplayerRun))]
public class Patch_StartNewMultiplayerRun
{
    [HarmonyPrefix]
    public static void Prefix(StartRunLobby lobby, bool shouldSave, IReadOnlyList<ActModel> acts,
        ref IReadOnlyList<ModifierModel> modifiers, string seed, int ascensionLevel, DateTimeOffset? dailyTime = null)
    {
        var uncappedActs = ModelDb.Modifier<UncappedSpireModifier>().ToMutable();
        modifiers = [uncappedActs, ..modifiers];
    }

    [HarmonyPostfix]
    public static void Postfix(StartRunLobby lobby, bool shouldSave, IReadOnlyList<ActModel> acts,
        ref IReadOnlyList<ModifierModel> modifiers, string seed, int ascensionLevel, DateTimeOffset? dailyTime = null)
    {
        if (lobby.NetService.Type == NetGameType.Host)
        {
            var chapterChangeSynchronizer = SpireFields_RunManager.UncappedSpireModifierSynchronizer.Get(RunManager.Instance)!;
            _ = chapterChangeSynchronizer.DoLocalUncappedModifierSet(
                UncappedConfig.UncappedActsEnabled,
                UncappedConfig.UncappedEnchantmentsEnabled,
                UncappedConfig.UncappedUpgradesEnabled,
                UncappedConfig.ActThreeBossRewardsEnabled,
                UncappedConfig.HpScaling,
                UncappedConfig.DmgScaling);
        }
    }
}