using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using UncappedSpire.UncappedSpireCode.Config;
using UncappedSpire.UncappedSpireCode.UncappedActs.RunManagerPatches;

namespace UncappedSpire.UncappedSpireCode.UncappedActs;

public static class UncappedActsCore
{
    public static readonly MethodInfo Method_get_State = AccessTools.PropertyGetter(typeof(RunManager), "State");
    public static readonly FieldInfo Field__mapPointHistory = AccessTools.Field(typeof(RunState), "_mapPointHistory");
    
    public static async Task EnterNextChapter()
    {
        MainFile.Logger.Info("Entering next chapter");
        MainFile.Logger.Info("Entering next chapter");
        MainFile.Logger.Info("Entering next chapter");
        MainFile.Logger.Info("Entering next chapter");
        
        var state = (RunState?)Method_get_State.Invoke(RunManager.Instance, null);
        if (state == null)
        {
            return;
        }
        
        // TODO: Save the old run history somewhere to display???
        Field__mapPointHistory.SetValue(state, new List<List<MapPointHistoryEntry>>());
                
        if (RunManager.Instance.NetService.Type is NetGameType.Host or NetGameType.Singleplayer)
        {
            var chapterChangeSynchronizer = SpireFields_RunManager.ChapterChangeSynchronizer.Get(RunManager.Instance)!;
            _ = await chapterChangeSynchronizer.DoLocalSeedChange(
                SeedHelper.GetRandomSeed());
        }
                
        var uncappedActsModifier = state.Modifiers.First(m => m is UncappedSpireModifier) as UncappedSpireModifier;
        uncappedActsModifier!.CurrentChapter++;
        await RunManager.Instance.EnterAct(0);
    }
}