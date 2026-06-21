using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;

namespace UncappedSpire.UncappedSpireCode.UncappedUI;

[HarmonyPatch(typeof(NMainMenuBg), "_Ready")]
public class NMainMenuBgPatches
{
    [HarmonyPrefix]
    public static void _Ready(NMainMenuBg __instance)
    {
        var bg = __instance.GetNode<Node2D>("BgContainer/Bg");
        bg.Modulate = new Color(0.35f, 0.25f, 0.25f);
        
        var fg = __instance.GetNode<Node2D>("BgContainer/Fg");
        fg.Modulate = new Color(0.75f, 0.5f, 0.5f);
    }
}