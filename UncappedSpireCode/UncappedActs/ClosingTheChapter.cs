using System.Reflection;
using BaseLib.Abstracts;
using HarmonyLib;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.TestSupport;
using UncappedSpire.UncappedSpireCode.Config;
using UncappedSpire.UncappedSpireCode.UncappedActs.RunManagerPatches;

namespace UncappedSpire.UncappedSpireCode.UncappedActs;

public class ClosingTheChapter : CustomEventModel
{
    public override bool IsShared => true;

    public override string CustomInitialPortraitPath => "res://UncappedSpire/images/events/closing_the_chapter.png";

    public override List<(string, string)> Localization => LocManager.Instance.Language switch
    { 
        _ => new EventLoc("Closing the Chapter", 
            new EventPageLoc("INITIAL", "So close to the peak, you face two last choices...", 
                new EventOptionLoc("MEET_THE_ARCHITECT", "Up the Spiral Staircase", "Meet [sine][red]The Architect[/red][/sine]"), 
                new EventOptionLoc("START_A_NEW_CHAPTER", "Through the Mysterious Door", "Start a new [green]Chapter[/green]"))
        )
    };
    
    protected override IReadOnlyList<EventOption> GenerateInitialOptions()
    {
        return
        [
            new EventOption(this, MeetTheArchitect, "UNCAPPEDSPIRE-CLOSING_THE_CHAPTER.pages.INITIAL.options.MEET_THE_ARCHITECT"),
            new EventOption(this, StartANewChapter, "UNCAPPEDSPIRE-CLOSING_THE_CHAPTER.pages.INITIAL.options.START_A_NEW_CHAPTER")
        ];
    }
    
    public override bool IsAllowed(IRunState runState)
    {
        return false;
    }

    private static readonly MethodInfo Method_ClearScreens = AccessTools.Method(typeof(RunManager), "ClearScreens");
    private static readonly MethodInfo Method_FadeIn = AccessTools.Method(typeof(RunManager), "FadeIn");
    private async Task MeetTheArchitect()
    {
        // TODO: I mean if it works but probably find a better way to do this...
        TaskHelper.RunSafely(MeetTheArchitectInner());
    }

    private async Task MeetTheArchitectInner()
    {
        var runManager = RunManager.Instance;
        using (new NetLoadingHandle(runManager.NetService))
        {
            if (TestMode.IsOff)
            {
                await NGame.Instance!.Transition.RoomFadeOut();
            }
            Method_ClearScreens.Invoke(runManager, null);
            await runManager.EnterRoom(new EventRoom(ModelDb.Event<TheArchitect>()));
            await (Task)Method_FadeIn.Invoke(runManager, [true])!;
        }
    }

    private static readonly MethodInfo Method_get_State = AccessTools.PropertyGetter(typeof(RunManager), "State");
    private static readonly FieldInfo Field__mapPointHistory = AccessTools.Field(typeof(RunState), "_mapPointHistory");
    private async Task StartANewChapter()
    {
        if (LocalContext.IsMe(Owner))
        {
            var state = (RunState)Method_get_State.Invoke(RunManager.Instance, null)!;
        
            // TODO: Save the old run history somewhere to display???
            Field__mapPointHistory.SetValue(state, new List<List<MapPointHistoryEntry>>());
                
            if (RunManager.Instance.NetService.Type is NetGameType.Host or NetGameType.Singleplayer)
            {
                var chapterChangeSynchronizer = SpireFields_RunManager.ChapterChangeSynchronizer.Get(RunManager.Instance)!;
                _ = chapterChangeSynchronizer.DoLocalSeedChange(SeedHelper.GetRandomSeed());
            }

            state.CurrentActIndex = -1;
            var uncappedActsModifier = state.Modifiers.First(m => m is UncappedSpireModifier) as UncappedSpireModifier;
            uncappedActsModifier!.CurrentChapter++;
            
            RunManager.Instance.ActChangeSynchronizer.SetLocalPlayerReady();
        }
    }
}