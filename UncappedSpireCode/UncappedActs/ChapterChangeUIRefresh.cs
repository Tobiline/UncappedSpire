using Godot;
using HarmonyLib;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace UncappedSpire.UncappedSpireCode.UncappedActs;

public static class ChapterChangeUIRefresh
{
    public static void RefreshTopBar(Player player)
    {
        var runState = player.RunState;
        
        var root = ((SceneTree)Engine.GetMainLoop()).Root;
        var topBar = root.GetNodeOrNull<NTopBar>("/root/Game/RootSceneContainer/Run/GlobalUi/TopBar");
        
        var field_ascensionLabel = AccessTools.Field(typeof(NTopBar), "_ascensionLabel");
        var _ascensionLabel = (MegaLabel)field_ascensionLabel.GetValue(topBar)!;
        
        var field_ascensionIcon = AccessTools.Field(typeof(NTopBar), "_ascensionIcon");
        var _ascensionIcon = (Control)field_ascensionIcon.GetValue(topBar)!;
        
        if (runState.AscensionLevel > 0)
        {
            _ascensionIcon.Visible = true;
            _ascensionLabel.SetTextAutoSize(runState.AscensionLevel.ToString());
        }
        
        topBar.PortraitTip.Initialize(runState);
        topBar.Deck.Initialize(player);
    }
}