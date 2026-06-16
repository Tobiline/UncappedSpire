using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Runs;

namespace UncappedSpire.UncappedSpireCode.UncappedActs.NGamePatches;

[HarmonyPatch(typeof(NGame), nameof(NGame.StartNewSingleplayerRun))]
public class Patch_StartNewSingleplayerRun
{
    public static void Prefix(CharacterModel character, bool shouldSave, IReadOnlyList<ActModel> acts,
        ref IReadOnlyList<ModifierModel> modifiers, string seed, GameMode gameMode, int ascensionLevel, DateTimeOffset? dailyTime)
    {
        var uncappedActs = ModelDb.Modifier<UncappedActs>().ToMutable();
        modifiers = [uncappedActs, ..modifiers];
    }
}