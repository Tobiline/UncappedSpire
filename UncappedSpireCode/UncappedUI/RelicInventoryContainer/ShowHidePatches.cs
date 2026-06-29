using System.Reflection;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Relics;

namespace UncappedSpire.UncappedSpireCode.UncappedUI.RelicInventoryContainer;

[HarmonyPatch]
public class ShowHidePatches
{
    private static FieldInfo _originalPos = AccessTools.Field(typeof(NRelicInventory), "_originalPos");
    
    [HarmonyPatch(typeof(NRelicInventory), nameof(NRelicInventory.ShowImmediately))]
    [HarmonyPostfix]
    public static void ShowImmediatelyPostfix(NRelicInventory __instance)
    {
        var scrollContainer = __instance.GetParent<ScrollContainer>();
        var scrollContainerControl = scrollContainer.GetParent<Control>();

        scrollContainer.Visible = true;
        scrollContainerControl.Visible = true;
    }
    
    [HarmonyPatch(typeof(NRelicInventory), nameof(NRelicInventory.HideImmediately))]
    [HarmonyPostfix]
    public static void HideImmediatelyPostfix(NRelicInventory __instance)
    {
        var scrollContainer = __instance.GetParent<ScrollContainer>();
        var scrollContainerControl = scrollContainer.GetParent<Control>();

        scrollContainer.Visible = false;
        scrollContainerControl.Visible = false;
    }
    
    [HarmonyPatch(typeof(NRelicInventory), nameof(NRelicInventory.AnimShow))]
    [HarmonyPostfix]
    public static void AnimShowPostfix(NRelicInventory __instance)
    {
        var scrollContainer = __instance.GetParent<ScrollContainer>();
        var scrollContainerControl = scrollContainer.GetParent<Control>();
        
        scrollContainer.Visible = true;
        scrollContainerControl.Visible = true;
    }
    
    [HarmonyPatch(typeof(NRelicInventory), nameof(NRelicInventory.AnimHide))]
    [HarmonyPostfix]
    public static void AnimHidePostfix(NRelicInventory __instance)
    {
        var scrollContainer = __instance.GetParent<ScrollContainer>();
        var scrollContainerControl = scrollContainer.GetParent<Control>();
        
        scrollContainer.Visible = false;
        scrollContainerControl.Visible = false;
    }
}