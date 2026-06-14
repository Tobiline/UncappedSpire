using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.RestSite;
using UncappedSpire.UncappedSpireCode.UncappedEnchantments.Deck;

namespace UncappedSpire.UncappedSpireCode.UncappedEnchantments.CloneRestSiteOptionPatches;

[HarmonyPatch]
public class Patch_OnSelect
{
    static MethodBase TargetMethod()
    {
        var m = AccessTools.Method(typeof(CloneRestSiteOption), nameof(CloneRestSiteOption.OnSelect));
        var attr = m.GetCustomAttribute<AsyncStateMachineAttribute>();
        if (attr == null)
        {
            throw new NullReferenceException("OnPlay AsyncStateMachineAttribute attribute not found");
        }
        var moveNextMethod = attr.StateMachineType.GetMethod("MoveNext", BindingFlags.NonPublic | BindingFlags.Instance);
        if (moveNextMethod == null)
        {
            throw new NullReferenceException("AsyncStateMachineAttribute state machine method not found");
        }
        return moveNextMethod;
    }
    
    public static MethodInfo Method_get_Cards = AccessTools.PropertyGetter(typeof(CardPile), nameof(CardPile.Cards));
    public static MethodInfo Method_GetClonable = AccessTools.Method(typeof(DeckExtensions), nameof(DeckExtensions.GetCloneable));
    
    [HarmonyTranspiler]
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var code = new List<CodeInstruction>(instructions);
        
        for (var i = 0; i < code.Count; i++)
        {
            if (code[i].opcode == OpCodes.Callvirt && code[i].operand is MethodInfo method && method == Method_get_Cards)
            {
                code.RemoveRange(i, 12);
                code.InsertRange(i, [
                    new CodeInstruction(OpCodes.Call, Method_GetClonable)
                ]);
                break;
            }
        }

        return code;
    }
}