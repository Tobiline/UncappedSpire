// using Godot;
// using MegaCrit.Sts2.Core.AutoSlay;
// using MegaCrit.Sts2.Core.AutoSlay.Helpers;
// using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
//
// namespace UncappedSpire.UncappedSpireCode.AutoMultiSlayer;
//
// public static class AutoSlayExtensions
// {
//     // TODO: Add wait sync for other players?
//     public static async Task PlayMultiplayerMenuAsync(this AutoSlayer autoSlayer, CancellationToken ct)
//     {
//         AutoSlayLog.Action("Playing multiplayer menu");
//         Node root = ((SceneTree)Engine.GetMainLoop()).Root;
//         var mainMenu = await WaitHelper.ForNode<Control>(root, "/root/Game/RootSceneContainer/MainMenu", ct, TimeSpan.FromSeconds(30L));
//         var button = await WaitHelper.ForNode<NButton>(mainMenu, "Submenus/CharacterSelectScreen/ConfirmButton", ct);
//         await UiHelper.Click(button);
//     }
// }