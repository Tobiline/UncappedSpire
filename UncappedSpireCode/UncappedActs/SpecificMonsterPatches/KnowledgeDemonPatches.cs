using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Monsters;
using UncappedSpire.UncappedSpireCode.Config;

namespace UncappedSpire.UncappedSpireCode.UncappedActs.SpecificMonsterPatches;

[HarmonyPatch(typeof(KnowledgeDemon), "ChooseCurse")]
public class KnowledgeDemonPatches
{
    public static FieldInfo Field__disintegrationDamageValues = AccessTools.Field(typeof(KnowledgeDemon), "_disintegrationDamageValues");
    public static FieldInfo Field__curseOfKnowledgeSets = AccessTools.Field(typeof(KnowledgeDemon), "_curseOfKnowledgeSets");
    public static MethodInfo Method_get_CurseOfKnowledgeCounter = AccessTools.PropertyGetter(typeof(KnowledgeDemon), "CurseOfKnowledgeCounter");
    
    [HarmonyPrefix]
    public static bool Prefix(KnowledgeDemon __instance, Creature target, ref Task __result)
    {
        __result = Replacement(__instance, target);
        return false;
    }
    
    public static async Task Replacement(KnowledgeDemon __instance, Creature target)
    {
        if (target.IsDead)
            return;
        var disintegrationDamageValues = (int[])Field__disintegrationDamageValues.GetValue(__instance)!;
        var curseOfKnowledgeSets = (IReadOnlyList<IReadOnlyList<KnowledgeDemon.IChoosable>>)Field__curseOfKnowledgeSets.GetValue(__instance)!;
        var CurseOfKnowledgeCounter = (int)Method_get_CurseOfKnowledgeCounter.Invoke(__instance, null)!;
        
        var disintegrationDamage = disintegrationDamageValues[CurseOfKnowledgeCounter];
        var cardModel = await CardSelectCmd.FromChooseACardScreen(new BlockingPlayerChoiceContext(), curseOfKnowledgeSets[CurseOfKnowledgeCounter].Select<KnowledgeDemon.IChoosable, CardModel>(c =>
        {
            var card = __instance.CombatState.CreateCard((CardModel) c, target.Player!);
            if (card is Disintegration)
                card.DynamicVars["DisintegrationPower"].BaseValue = disintegrationDamage * (decimal)ContextManager.Current_ScalingDmg;
            return card;
        }).ToList(), target.Player!);
        if (cardModel == null)
            return;
        await ((KnowledgeDemon.IChoosable) cardModel).OnChosen();
    }
}