using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Runs;

namespace UncappedSpire.UncappedSpireCode.UncappedActs.RunManagerPatches;

[HarmonyPatch(typeof(RunManager), nameof(RunManager.EnterNextAct))]
public class Patch_EnterNextAct
{
    public static readonly MethodInfo Method_get_State = AccessTools.PropertyGetter(typeof(RunManager), "State");
    
    [HarmonyPrefix]
    public static bool Prefix(RunManager __instance, ref Task __result)
    {
        __result = Replacement(__instance);
        return false;
    }
    
    public static async Task Replacement(RunManager __instance)
    {
        var state = (RunState?)Method_get_State.Invoke(__instance, null);
        if (state == null)
        {
            return;
        }
        using (new NetLoadingHandle(__instance.NetService))
        {
            // TODO: Add option to win run at the end of a "Chapter" instead?
            // if (state.CurrentActIndex >= state.Acts.Count - 1)
            // {
            //     AbstractRoom currentRoom = state.CurrentRoom;
            //     if (currentRoom != null && currentRoom.IsVictoryRoom)
            //     {
            //         await __instance.WinRun();
            //         return;
            //     }
            //     if (TestMode.IsOff)
            //     {
            //         await NGame.Instance.Transition.RoomFadeOut();
            //     }
            //     __instance.ClearScreens();
            //     await __instance.EnterRoom(new EventRoom(ModelDb.Event<TheArchitect>()));
            //     await __instance.FadeIn();
            // }
            // else
            // {
            //     await __instance.EnterAct(state.CurrentActIndex + 1);
            // }

            if (state.CurrentActIndex < state.Acts.Count - 1)
            {
                await __instance.EnterAct(state.CurrentActIndex + 1);
            }
            else
            {
                if (__instance.NetService.Type is NetGameType.Host or NetGameType.Singleplayer)
                {
                    var seedChangeSynchronizer = SpireFields_RunManager.ChapterChangeSynchronizer.Get(RunManager.Instance)!;
                    _ = await seedChangeSynchronizer.DoLocalSeedChange(
                        SeedHelper.GetRandomSeed());
                }
                
                var uncappedActsModifier = state.Modifiers.First(m => m is UncappedActs) as UncappedActs;
                uncappedActsModifier!.CurrentChapter++;
                await __instance.EnterAct(0);
            }
        }
    }
}