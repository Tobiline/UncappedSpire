using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using HarmonyLib;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Ancients;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Runs;
using UncappedSpire.UncappedSpireCode.Config;
using UncappedSpire.UncappedSpireCode.UncappedActs.RunManagerPatches;

namespace UncappedSpire.UncappedSpireCode.UncappedActs.TheArchitectPatches;

[HarmonyPatch]
public class Patch_AdvanceDialogue
{
    static MethodBase TargetMethod()
    {
        var m = AccessTools.Method(typeof(TheArchitect), "AdvanceDialogue");
        var attr = m.GetCustomAttribute<AsyncStateMachineAttribute>();
        if (attr == null)
        {
            throw new NullReferenceException("AdvanceDialogue AsyncStateMachineAttribute attribute not found");
        }
        var moveNextMethod = attr.StateMachineType.GetMethod("MoveNext", BindingFlags.NonPublic | BindingFlags.Instance);
        if (moveNextMethod == null)
        {
            throw new NullReferenceException("AsyncStateMachineAttribute state machine method not found");
        }
        return moveNextMethod;
    }
    
    [HarmonyTranspiler]
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var code = new List<CodeInstruction>(instructions);
        
        var methodToFind = AccessTools.Method(typeof(TheArchitect), "CreateProceedOption");
        var methodToCall = AccessTools.Method(typeof(Patch_AdvanceDialogue), nameof(AddMoveToNextChapterOption));
        
        for (var i = 0; i < code.Count; i++)
        {
            if (code[i].opcode == OpCodes.Call && code[i].operand is MethodInfo methodInfo && methodInfo == methodToFind)
            {
                var ldTheArchitect = code[i + 3].opcode;
                var ldEventOptions = code[i + 5].opcode;
                var stEventOptions = code[i + 2].opcode;
                
                code.InsertRange(i - 2, [
                    new CodeInstruction(ldTheArchitect),
                    new CodeInstruction(ldEventOptions),
                    new CodeInstruction(OpCodes.Call, methodToCall),
                    new CodeInstruction(stEventOptions)
                ]);
                break;
            }
        }

        return code;
    }
    
    public static MethodInfo Method_get_IsOnLastLine = AccessTools.PropertyGetter(typeof(TheArchitect), "IsOnLastLine");
    
    public static IReadOnlyList<EventOption> AddMoveToNextChapterOption(TheArchitect __instance, IReadOnlyList<EventOption> __result)
    {
        var isOnLastLine = (bool)Method_get_IsOnLastLine.Invoke(__instance, null)!;
        if (isOnLastLine && ContextManager.UncappedActsEnabled)
        {
            var winEventOption = new EventOption(
                __instance,
                async () => await MoveToNextChapter(__instance),
                "NEXT_CHAPTER",
                false,
                false
            ).ThatWontSaveToChoiceHistory();
            
            var list = __result.ToList();
            list.Add(winEventOption);
            return list;
        }
        
        return __result;
    }

    public static async Task MoveToNextChapter(TheArchitect __instance)
    {
        if (LocalContext.IsMe(__instance.Owner))
        {
            if (__instance.Owner.RunState.Players.Count > 1)
            {
                NCombatRoom.Instance?.SetWaitingForOtherPlayersOverlayVisible(visible: true);
            }
            
            await UncappedActsCore.EnterNextChapter();
        }
    }
}