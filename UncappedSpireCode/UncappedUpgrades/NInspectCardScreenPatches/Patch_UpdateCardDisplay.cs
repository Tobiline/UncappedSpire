using System.Reflection;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Screens;
using UncappedSpire.UncappedSpireCode.UncappedUpgrades.CardModelPatches;
using UncappedSpire.UncappedSpireCode.UncappedUpgrades.UI.InspectCardScreen;

namespace UncappedSpire.UncappedSpireCode.UncappedUpgrades.NInspectCardScreenPatches;

[HarmonyPatch(typeof(NInspectCardScreen), "UpdateCardDisplay")]
public class Patch_UpdateCardDisplay
{
    private static readonly FieldInfo _card = HarmonyLib.AccessTools.Field(typeof(NInspectCardScreen), "_card");
    private static readonly FieldInfo _cards = HarmonyLib.AccessTools.Field(typeof(NInspectCardScreen), "_cards");
    private static readonly FieldInfo _index = HarmonyLib.AccessTools.Field(typeof(NInspectCardScreen), "_index");
    private static readonly MethodInfo _isShowingUpgradedCard = HarmonyLib.AccessTools.PropertyGetter(typeof(NInspectCardScreen), "IsShowingUpgradedCard");
    private static readonly FieldInfo _hoverTipRect = HarmonyLib.AccessTools.Field(typeof(NInspectCardScreen), "_hoverTipRect");
    
    [HarmonyPrefix]
    static bool Prefix(NInspectCardScreen __instance)
    {
        var cardObj = _card.GetValue(__instance);
        var cardsObj = _cards.GetValue(__instance);
        var indexObj = _index.GetValue(__instance);
        var isShowingUpgradedCardObj = _isShowingUpgradedCard.Invoke(__instance, []);
        var hoverTipRectObj = _hoverTipRect.GetValue(__instance);
        var uncappedCardInput = SpireField_UncappedCardInput.UncappedCardInput.Get(__instance);

        if (cardObj == null || cardsObj == null || indexObj == null || isShowingUpgradedCardObj == null ||
            hoverTipRectObj == null ||  uncappedCardInput == null)
            return false;
        
        var card = (NCard)cardObj;
        var cards = (List<CardModel>?)cardsObj;
        var index = (int)indexObj;
        var isShowingUpgradedCard = (bool)isShowingUpgradedCardObj;
        var hoverTipRect = (Control)hoverTipRectObj;

        if (cards == null) return false;
        
        CardModel card1 = cards[index];
        CardModel card2 = (CardModel)cards[index].MutableClone();
        if (isShowingUpgradedCard)
        {
            if (!card1.IsUpgraded && card1.IsUpgradable)
            {
                card2.UpgradePreviewType = CardUpgradePreviewType.Deck;
                var upgradeLevel = (int)uncappedCardInput.Value;
                card2.UpgradeToInternal(upgradeLevel);
            }
            card.Model = card2;
            card.ShowUpgradePreview();
        }
        else
        {
            if (card2.IsUpgraded)
                CardCmd.Downgrade(card2);
            card.Model = card2;
            card.UpdateVisuals(PileType.None, CardPreviewMode.Normal);
        }
        NHoverTipSet.Clear();
        NHoverTipSet.CreateAndShow(__instance, card2.HoverTips).SetAlignment(hoverTipRect, HoverTip.GetHoverTipAlignment(__instance));
        
        return false;
    }
}