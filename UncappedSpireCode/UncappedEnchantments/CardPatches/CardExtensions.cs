using System.Reflection;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Entities.Enchantments;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using UncappedSpire.UncappedSpireCode.UncappedEnchantments.CardModelPatches;
using UncappedSpire.UncappedSpireCode.UncappedEnchantments.EnchantmentModelPatches;
using UncappedSpire.UncappedSpireCode.UncappedEnchantments.UI;

namespace UncappedSpire.UncappedSpireCode.UncappedEnchantments.CardPatches;

public static class CardExtensions
{
    private static readonly MethodInfo Method_OnAfflictionChanged = AccessTools.Method(typeof(NCard), "OnAfflictionChanged");
    private static readonly MethodInfo Method_OnEnchantmentStatusChanged = AccessTools.Method(typeof(NCard), "OnEnchantmentStatusChanged");
    
    private static readonly FieldInfo Field__enchantmentTab = AccessTools.Field(typeof(NCard), "_enchantmentTab");
    private static readonly FieldInfo Field__defaultEnchantmentPosition = AccessTools.Field(typeof(NCard), "_defaultEnchantmentPosition");
    
    private static readonly FieldInfo Field__v = AccessTools.Field(typeof(NCard), "_v");
    private static readonly FieldInfo Field__s = AccessTools.Field(typeof(NCard), "_s");
    private static readonly FieldInfo Field__h = AccessTools.Field(typeof(NCard), "_h");

    // public static void SubscribeToModel(this NCard nCard, CardModel? model)
    // {
    //     if (model != null && nCard.IsInsideTree())
    //     {
    //         model.AfflictionChanged += Method_OnAfflictionChanged.CreateDelegate<Action>(nCard);
    //         SpireField__multiEnchantment.EnchantmentsChanged.Get(model)!.Changed += nCard.OnEnchantmentsChanged;
    //         if (model.Enchantment != null)
    //         {
    //             nCard.SubscribeToEnchantments(SpireField_Enchantments.Enchantments[model.Enchantment]!);
    //         }
    //     }
    // }
    //
    // public static void UnsubscribeFromModel(this NCard nCard, CardModel? model)
    // {
    //     if (model != null)
    //     {
    //         model.AfflictionChanged -= Method_OnAfflictionChanged.CreateDelegate<Action>(nCard);
    //         SpireField__multiEnchantment.EnchantmentsChanged.Get(model)!.Changed -= nCard.OnEnchantmentsChanged;
    //         if (model.Enchantment != null)
    //         {
    //             nCard.UnsubscribeFromEnchantments(SpireField_Enchantments.Enchantments[model.Enchantment]!);
    //         }
    //     }
    // }
    //
    // public static void OnEnchantmentsChanged(this NCard nCard)
    // {
    //     nCard.UnsubscribeFromEnchantments(CardSpireFields._subscribedEnchantments.Get(nCard)!);
    //     if (nCard.Model?.Enchantment != null)
    //     {
    //         nCard.SubscribeToEnchantments(SpireField_Enchantments.Enchantments[nCard.Model.Enchantment!]);
    //     }
    //     nCard.UpdateEnchantmentVisuals();
    // }
    //
    // private static void UnsubscribeFromEnchantments(this NCard nCard, List<EnchantmentModel> model)
    // {
    //     var _subscribedEnchantments = CardSpireFields._subscribedEnchantments.Get(nCard);
    //     if (model == _subscribedEnchantments)
    //     {
    //         foreach (var enchantment in _subscribedEnchantments)
    //         {
    //             enchantment.StatusChanged -= Method_OnEnchantmentStatusChanged.CreateDelegate<Action>(nCard);
    //         }
    //
    //         CardSpireFields._subscribedEnchantments.Set(nCard, []);
    //     }
    // }
    //
    // private static void SubscribeToEnchantments(this NCard nCard, List<EnchantmentModel>? model)
    // {
    //     if (nCard.IsInsideTree())
    //     {
    //         CardSpireFields._subscribedEnchantments.Set(nCard, model);
    //         
    //         var _subscribedEnchantments = CardSpireFields._subscribedEnchantments.Get(nCard)!;
    //         foreach (var enchantment in _subscribedEnchantments)
    //         {
    //             enchantment.StatusChanged += Method_OnEnchantmentStatusChanged.CreateDelegate<Action>(nCard);
    //         }
    //     }
    // }

    public static void UpdateEnchantmentVisuals(this NCard nCard)
    {
        if (nCard.Model == null)
        {
            throw new InvalidOperationException("Cannot show enchantment with no model.");
        }
        var enchantments = nCard.Model.Enchantment != null ? ((MultiEnchantment)nCard.Model.Enchantment).EnchantmentsOnCards.Select(c => c.Enchantment!).ToList() : [];
        var _enchantmentTabContainerLeft = SpireField__enchantmentTabContainer._enchantmentTabContainerLeft.Get(nCard);
        if (_enchantmentTabContainerLeft != null && GodotObject.IsInstanceValid(_enchantmentTabContainerLeft))
        {
            foreach (var child in _enchantmentTabContainerLeft.GetChildren())
            {
                child.QueueFree();
            }
        }
        var _enchantmentTabContainerRight = SpireField__enchantmentTabContainer._enchantmentTabContainerRight.Get(nCard);
        if (_enchantmentTabContainerRight != null && GodotObject.IsInstanceValid(_enchantmentTabContainerRight))
        {
            foreach (var child in _enchantmentTabContainerRight.GetChildren())
            {
                child.QueueFree();
            }
        }
        
        var _enchantmentTab = (Control)Field__enchantmentTab.GetValue(nCard)!;
        for (var i = 0; i < enchantments.Count; i++)
        {
            var enchantment = enchantments[i];
            
            var newTab = (TextureRect)_enchantmentTab.Duplicate();
            var newTabIcon = (TextureRect)newTab.GetChild(0);
            var newTabLabel = (MegaLabel)newTab.GetChild(1);

            newTab.Visible = true;
            newTabIcon.Texture = enchantment.Icon;
            newTabLabel.SetTextAutoSize(enchantment.DisplayAmount.ToString());
            newTabLabel.Visible = enchantment.ShowAmount;
            nCard.SetEnchantmentStatus(enchantment.Status, newTab, newTabIcon, newTabLabel);

            if (i <= 5)
            {
                _enchantmentTabContainerLeft.AddChild(newTab);
            }
            else
            {
                newTab.FlipH = true;
                var offsetShift = 8;
                newTabIcon.OffsetLeft += offsetShift;
                newTabIcon.OffsetRight += offsetShift;
                newTabLabel.OffsetLeft += offsetShift;
                newTabLabel.OffsetRight += offsetShift;
                _enchantmentTabContainerRight.AddChild(newTab);
            }
        }

        var _enchantmentTabContainer = SpireField__enchantmentTabContainer._enchantmentTabContainer.Get(nCard);
        var _defaultEnchantmentPosition = (Vector2)Field__defaultEnchantmentPosition.GetValue(nCard)!;
        if (nCard.Model.HasStarCostX || nCard.Model.CurrentStarCost >= 0)
        {
            _enchantmentTabContainer.Position = _defaultEnchantmentPosition;
        }
        else
        {
            _enchantmentTabContainer.Position = _defaultEnchantmentPosition + Vector2.Up * 45f;
        }
    }

    private static void SetEnchantmentStatus(this NCard nCard, EnchantmentStatus status, Control enchantmentTab, TextureRect enchantmentIcon, MegaLabel enchantmentLabel)
    {
        var _h = (StringName)Field__h.GetValue(nCard)!;
        var _s = (StringName)Field__s.GetValue(nCard)!;
        var _v = (StringName)Field__v.GetValue(nCard)!;
        
        if (status == EnchantmentStatus.Disabled)
        {
            enchantmentTab.Modulate = new Color(1f, 1f, 1f, 0.9f);
            var shaderMaterial = (ShaderMaterial)enchantmentTab.Material;
            shaderMaterial.SetShaderParameter(_h, 0.25);
            shaderMaterial.SetShaderParameter(_s, 0.1);
            shaderMaterial.SetShaderParameter(_v, 0.6);
            enchantmentIcon.UseParentMaterial = true;
            enchantmentLabel.SelfModulate = StsColors.gray;
        }
        else
        {
            enchantmentTab.Modulate = Colors.White;
            var shaderMaterial2 = (ShaderMaterial)enchantmentTab.Material;
            shaderMaterial2.SetShaderParameter(_h, 0.25);
            shaderMaterial2.SetShaderParameter(_s, 0.4);
            shaderMaterial2.SetShaderParameter(_v, 0.6);
            enchantmentIcon.UseParentMaterial = false;
            enchantmentLabel.SelfModulate = Colors.White;
        }
    }
}