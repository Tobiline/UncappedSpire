using System.Reflection;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using UncappedSpire.UncappedSpireCode.Config;
using UncappedSpire.UncappedSpireCode.UncappedEnchantments.EnchantmentModelPatches;
using Vector2 = Godot.Vector2;

namespace UncappedSpire.UncappedSpireCode.UncappedEnchantments.NCardEnchantVfxPatches;

[HarmonyPatch(typeof(NCardEnchantVfx), nameof(NCardEnchantVfx._Ready))]
public class Patch__Ready
{
    private static FieldInfo Field__enchantmentSparkles = AccessTools.Field(typeof(NCardEnchantVfx), "_enchantmentSparkles");
    private static FieldInfo Field__enchantmentIcon = AccessTools.Field(typeof(NCardEnchantVfx), "_enchantmentIcon");
    private static FieldInfo Field__enchantmentLabel = AccessTools.Field(typeof(NCardEnchantVfx), "_enchantmentLabel");
    private static FieldInfo Field__cardModel = AccessTools.Field(typeof(NCardEnchantVfx), "_cardModel");
    private static FieldInfo Field__cardNode = AccessTools.Field(typeof(NCardEnchantVfx), "_cardNode");
    
    private static MethodInfo Method_PlayAnimation = AccessTools.Method(typeof(NCardEnchantVfx), "PlayAnimation");
    
    [HarmonyPrefix]
    public static bool Prefix(NCardEnchantVfx __instance)
    {
        if (!ContextManager.UncappedEnchantmentsEnabled)
            return true;
        
        Field__enchantmentSparkles.SetValue(__instance, __instance.GetNode<GpuParticles2D>("%EnchantmentAppearSparkles"));
        Field__enchantmentIcon.SetValue(__instance, __instance.GetNode<TextureRect>("%EnchantmentInViewport/Icon"));
        Field__enchantmentLabel.SetValue(__instance, __instance.GetNode<MegaLabel>("%EnchantmentInViewport/Label"));

        var _enchantmentIcon = (TextureRect)Field__enchantmentIcon.GetValue(__instance)!;
        var _enchantmentLabel = (MegaLabel)Field__enchantmentLabel.GetValue(__instance)!;
        var _cardModel = (CardModel)Field__cardModel.GetValue(__instance)!;

        var multiEnchant = (MultiEnchantment)_cardModel.Enchantment!;
        var actualEnchantmentIndex = multiEnchant.NextIndexToAnim;
        var actualEnchantment = multiEnchant.EnchantmentsOnCards[actualEnchantmentIndex].Enchantment!;
        
        SpireFields_EnchantmentModel.HideForAnim.Set(actualEnchantment, true);
        
        _enchantmentIcon.Texture = actualEnchantment.Icon;
        _enchantmentLabel.SetTextAutoSize(actualEnchantment.DisplayAmount.ToString());
        _enchantmentLabel.Visible = actualEnchantment.ShowAmount;
        
        Field__cardNode.SetValue(__instance, NCard.Create(_cardModel));
        
        var _cardNode = (NCard)Field__cardNode.GetValue(__instance)!;
        
        __instance.AddChildSafely(_cardNode);
        __instance.MoveChild(_cardNode, 0);
        _cardNode.UpdateVisuals(PileType.None, CardPreviewMode.Normal);
        _cardNode.EnchantmentTab.Visible = false;
        _cardNode.EnchantmentVfxOverride.Visible = true;

        var basePos = _cardNode.EnchantmentVfxOverride.Position;
        var finalPos = basePos;

        if (!(_cardModel.HasStarCostX || _cardModel.CurrentStarCost >= 0))
        {
            finalPos += Vector2.Up * 45f;
        }
        finalPos += Vector2.Down * (actualEnchantmentIndex * 57.5f);

        var viewportNode = __instance.GetNode<SubViewport>("%EnchantmentViewport");
        _cardNode.EnchantmentVfxOverride.Texture = viewportNode.GetTexture();
        
        if (actualEnchantmentIndex > 5)
        {
            finalPos += Vector2.Up * (6 * 57.5f);
                
            var tab = viewportNode.GetNode<TextureRect>("%EnchantmentInViewport");
            var icon = tab.GetChild<TextureRect>(0);
            var label = tab.GetChild<Label>(1);
                
            tab.FlipH = true;
            icon.Position += Vector2.Right * 8f;
            label.Position += Vector2.Right * 8f;
                
            finalPos += Vector2.Right * 260f;
        }

        _cardNode.EnchantmentVfxOverride.Position = finalPos;
        
        multiEnchant.NextIndexToAnim = -1;
        
        TaskHelper.RunSafely((Task)Method_PlayAnimation.Invoke(__instance, null)!).ContinueWith(_ =>
        {
            _cardNode.EnchantmentVfxOverride.Position = basePos;
        });
        
        SpireFields_EnchantmentModel.HideForAnim.Set(actualEnchantment, false);
        
        return false;
    }
}