using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace UncappedSpire.UncappedSpireCode.UncappedActs.CombatStatePatches;

[HarmonyPatch(typeof(CombatState), nameof(CombatState.CreateCard), [typeof(CardModel), typeof(Player)])]
public class Patch_CreateCard
{
    private static readonly MethodInfo ToFind_Method_SubscribeToModel = AccessTools.Method(typeof(CombatState), "AddCard", [typeof(CardModel), typeof(Player)]);
    private static readonly MethodInfo Method_ScaleCardDamage = AccessTools.Method(typeof(Patch_CreateCard), nameof(ScaleCardDamage));

    [HarmonyTranspiler]
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var code = new List<CodeInstruction>(instructions);
        
        for (var i = 0; i < code.Count; i++)
        {
            var instruction = code[i];
            if (instruction.opcode == OpCodes.Call && instruction.operand is MethodInfo method && method == ToFind_Method_SubscribeToModel)
            {
                var ldCard = new CodeInstruction(code[i - 2]);
                code.InsertRange(i - 3, [
                    ldCard,
                    new CodeInstruction(OpCodes.Call, Method_ScaleCardDamage)
                ]);
                break;
            }
        }

        return code;
    }

    private static FieldInfo Field__baseValue = AccessTools.Field(typeof(DynamicVar), "_baseValue");
    static void ScaleCardDamage(CardModel card)
    {
        if (card.Type != CardType.Status)
            return;

        foreach (var key in card.DynamicVars.Keys.Where(k => ChapterManager.ScalingStatusDynamicVarKeys.ContainsKey(k)))
        {
            var dynamicVar = card.DynamicVars[key];
            var scaleType = ChapterManager.ScalingStatusDynamicVarKeys[key];
            var baseValue = (decimal)Field__baseValue.GetValue(dynamicVar)!;
            //var upgradeValueBy = (dynamicVar.BaseValue * (decimal)ChapterManager.GetScaling(scaleType)) - dynamicVar.BaseValue;
            var upgradeValueBy = baseValue * (decimal)ChapterManager.GetScaling(scaleType);

            //dynamicVar.UpgradeValueBy(upgradeValueBy);
            Field__baseValue.SetValue(dynamicVar, upgradeValueBy);
        }
    }
}