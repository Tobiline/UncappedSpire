using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.Screens;
using UncappedSpire.UncappedSpireCode.UncappedUpgrades.CardModelPatches;

namespace UncappedSpire.UncappedSpireCode.UncappedUpgrades;

[HarmonyPatch]
public class Patches_UpgradeInternal
{
    static IEnumerable<MethodBase> TargetMethods()
    {
        return
        [
            AccessTools.Method(typeof(NGridCardHolder), "UpdateCardModel"),
            AccessTools.Method(typeof(NInspectCardScreen), "UpdateCardDisplay"),
        ];
    }
    
    [HarmonyTranspiler]
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var code = new List<CodeInstruction>(instructions);

        var methodToFind = AccessTools.Method(typeof(CardModel), "UpgradeInternal");
        var getMultiplier = AccessTools.Method(typeof(UpgradeContext), nameof(UpgradeContext.GetMultiplier));
        var upgradeInternal = AccessTools.Method(typeof(CardModelExtensions), nameof(CardModelExtensions.UpgradeInternal));
        
        for (var i = 0; i < code.Count; i++)
        {
            var instruction = code[i];
            if (instruction.opcode == OpCodes.Callvirt && instruction.operand is MethodInfo method && method == methodToFind)
            {
                code.RemoveAt(i);
                code.InsertRange(i, [
                    new CodeInstruction(OpCodes.Call, getMultiplier),
                    new CodeInstruction(OpCodes.Call, upgradeInternal),
                ]);
                break;
            }
        }
    
        return code;
    }
}