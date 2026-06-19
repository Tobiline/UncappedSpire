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
        var light = __instance.GetNode<Node2D>("BgContainer/Bg");
        light.Modulate = new Color(0.25f, 0.25f, 0.25f);
    }
}