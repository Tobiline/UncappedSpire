using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Managers;

namespace UncappedSpire.UncappedSpireCode;

// TODO: After fix errors check if the architect option title is correct
[HarmonyPatch(typeof(EventModel), "SetEventState")]
public class Debug
{
    [HarmonyPrefix]
    public static void Prefix(EventOption __instance, LocString description, IEnumerable<EventOption> eventOptions)
    {
        
    }
}