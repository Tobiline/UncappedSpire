using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using UncappedSpire.UncappedSpireCode.UncappedUpgrades.HoverTipFactoryPatches;

namespace UncappedSpire.UncappedSpireCode.UncappedUpgrades.CardPatches;

[HarmonyPatch]
public class Patch_ExtraHoverTips_UpgradableCreatedCards
{
    static readonly Type[] Targets =
    [
        typeof(PrimalForce),
        typeof(Stoke),
        typeof(HiddenDaggers),
        typeof(StormOfSteel),
        typeof(KnifeTrap),
        typeof(Largesse),
        typeof(Quasar),
        typeof(Charge),
        typeof(ManifestAuthority),
        typeof(Guards),
        typeof(Reave),
        typeof(Dirge),
        typeof(Compact),
        typeof(Splash),
        typeof(Jackpot)
    ];
    
    static IEnumerable<MethodBase> TargetMethods()
    {
        return Targets.Select(target => AccessTools.PropertyGetter(target, "ExtraHoverTips"));
    }
    
    [HarmonyTranspiler]
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, MethodBase original)
    {
        var code = new List<CodeInstruction>(instructions);

        var methodAToFind = AccessTools.Method(typeof(HoverTipFactory), nameof(HoverTipFactory.FromCard), [typeof(CardModel), typeof(bool)]);
        var methodBToFind = AccessTools.Method(typeof(HoverTipFactory), nameof(HoverTipFactory.FromCard), [typeof(bool)]);
        var methodCToFind = AccessTools.Method(typeof(HoverTipFactory), nameof(HoverTipFactory.FromCardWithCardHoverTips));
        
        var methodAToCall = AccessTools.Method(typeof(HoverTipFactoryExtensions), nameof(HoverTipFactoryExtensions.FromCard), [typeof(CardModel), typeof(bool), typeof(int)]);
        var methodBToCall = AccessTools.Method(typeof(HoverTipFactoryExtensions), nameof(HoverTipFactoryExtensions.FromCard), [typeof(bool), typeof(int)]);
        var methodCToCall = AccessTools.Method(typeof(HoverTipFactoryExtensions), nameof(HoverTipFactoryExtensions.FromCardWithCardHoverTips));
        
        var getCurrentUpgradeLevel = AccessTools.PropertyGetter(typeof(CardModel), nameof(CardModel.CurrentUpgradeLevel));
        
        for (var i = 0; i < code.Count; i++)
        {
            var instruction = code[i];
            if (instruction.opcode == OpCodes.Call && instruction.operand is MethodInfo method)
            {
                var methodToCompare = method.IsGenericMethod ? method.GetGenericMethodDefinition() : method;
                MethodInfo? methodToReplace = null;
                if (methodToCompare == methodAToFind)
                {
                    methodToReplace = methodAToCall;
                }
                else if (methodToCompare == methodBToFind)
                {
                    var genericArgs = method.GetGenericArguments();
                    methodToReplace = methodBToCall.MakeGenericMethod(genericArgs);
                }
                else if (methodToCompare == methodCToFind)
                {
                    var genericArgs = method.GetGenericArguments();
                    methodToReplace = methodCToCall.MakeGenericMethod(genericArgs);
                }

                if (methodToReplace != null)
                {
                    code.RemoveAt(i);
                    code.InsertRange(i, [
                        new CodeInstruction(OpCodes.Ldarg_0),
                        new CodeInstruction(OpCodes.Call, getCurrentUpgradeLevel),
                        new CodeInstruction(OpCodes.Call, methodToReplace),
                    ]);
                    i += 2;
                }
            }
        }
    
        return code;
    }
}