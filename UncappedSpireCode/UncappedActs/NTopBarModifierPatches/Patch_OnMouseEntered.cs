using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.sts2.Core.Nodes.TopBar;

namespace UncappedSpire.UncappedSpireCode.UncappedActs.NTopBarModifierPatches;

[HarmonyPatch(typeof(NTopBarModifier),"OnMouseEntered")]
public class Patch_OnMouseEntered
{
    public static readonly FieldInfo Field__hoverTip = AccessTools.Field(typeof(NTopBarModifier), "_hoverTip");
    public static readonly FieldInfo Field__modifier = AccessTools.Field(typeof(NTopBarModifier), "_modifier");
    
    [HarmonyPrefix]
    public static void Prefix(NTopBarModifier __instance)
    {
        var modifier = Field__modifier.GetValue(__instance);
        if (modifier is UncappedActs uncappedActs)
        {
            Field__hoverTip.SetValue(__instance, new HoverTip(uncappedActs.Title, uncappedActs.Description));
        }
    }
}