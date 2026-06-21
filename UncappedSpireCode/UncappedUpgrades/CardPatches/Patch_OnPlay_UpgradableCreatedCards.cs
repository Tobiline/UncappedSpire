using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace UncappedSpire.UncappedSpireCode.UncappedUpgrades.CardPatches;

[HarmonyPatch]
public class Patch_OnPlay_UpgradableCreatedCards
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
        typeof(Jackpot),
        typeof(Begone)
    ];
    
    static IEnumerable<MethodBase> TargetMethods()
    {
        foreach (var target in Targets)
        {
            var m = AccessTools.Method(target, "OnPlay");
            var attr = m.GetCustomAttribute<AsyncStateMachineAttribute>();
            if (attr == null)
            {
                MainFile.Logger.Error($"OnPlay AsyncStateMachineAttribute attribute not found for {target.Name}");
                continue;
            }
            var moveNextMethod = attr.StateMachineType.GetMethod("MoveNext", BindingFlags.NonPublic | BindingFlags.Instance);
            if (moveNextMethod == null)
            {
                MainFile.Logger.Error($"AsyncStateMachineAttribute state machine method not found for {target.Name}");
                continue;
            }
            
            yield return moveNextMethod;
        }
    }
    
    [HarmonyTranspiler]
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, MethodBase original)
    {
        var code = new List<CodeInstruction>(instructions);

        var methodToFind = AccessTools.Method(typeof(CardCmd), nameof(CardCmd.Upgrade), [typeof(CardModel), typeof(CardPreviewStyle)]);
        var getCurrentUpgradeLevel = AccessTools.PropertyGetter(typeof(CardModel), nameof(CardModel.CurrentUpgradeLevel));
        var setCurrentUpgradeLevel = AccessTools.PropertySetter(typeof(CardModel), nameof(CardModel.CurrentUpgradeLevel));
        
        for (var i = 0; i < code.Count; i++)
        {
            var instruction = code[i];
            if (instruction.opcode == OpCodes.Call && instruction.operand is MethodInfo method && method == methodToFind)
            {
                var cardLdLocInd = 0;
                for (var x = i; x >= 0; x--)
                {
                    if (code[x].IsLdloc())
                    {
                        cardLdLocInd = x;
                        break;
                    }
                }

                var cardLdRange = code.GetRange(cardLdLocInd, (i - cardLdLocInd) - 1);
                
                code.InsertRange(i - 2, [
                    .. cardLdRange,
                    new CodeInstruction(OpCodes.Ldloc_1),
                    new CodeInstruction(OpCodes.Callvirt, getCurrentUpgradeLevel),
                    new CodeInstruction(OpCodes.Ldc_I4_1),
                    new CodeInstruction(OpCodes.Sub),
                    new CodeInstruction(OpCodes.Callvirt, setCurrentUpgradeLevel),
                ]);
                i += cardLdRange.Count + 5;
            }
        }
    
        return code;
    }
}