// using HarmonyLib;
// using MegaCrit.Sts2.Core.AutoSlay;
// using MegaCrit.Sts2.Core.AutoSlay.Handlers;
// using MegaCrit.Sts2.Core.AutoSlay.Handlers.Rooms;
// using MegaCrit.Sts2.Core.AutoSlay.Handlers.Screens;
// using MegaCrit.Sts2.Core.Nodes.Events.Custom.CrystalSphere;
// using MegaCrit.Sts2.Core.Nodes.Screens;
// using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;
// using MegaCrit.Sts2.Core.Nodes.Screens.GameOverScreen;
// using MegaCrit.Sts2.Core.Rooms;
// using CombatRoomHandler = UncappedSpire.UncappedSpireCode.AutoMultiSlayer.RoomHandlers.CombatRoomHandler;
//
// namespace UncappedSpire.UncappedSpireCode.AutoMultiSlayer;
//
// [HarmonyPatch(typeof(AutoSlayer), MethodType.Constructor)]
// public class RoomHandlersPatch
// {
//     [HarmonyPrefix]
//     public static bool Prefix(
//         ref Dictionary<RoomType, IRoomHandler> ____roomHandlers,
//         ref MapScreenHandler ____mapHandler,
//         ref Dictionary<Type, IScreenHandler> ____screenHandlers)
//     {
//         var value = new CombatRoomHandler();
//         ____roomHandlers = new Dictionary<RoomType, IRoomHandler>
//         {
//             [RoomType.Monster] = value,
//             [RoomType.Elite] = value,
//             [RoomType.Boss] = value,
//             [RoomType.Event] = new EventRoomHandler(),
//             [RoomType.Shop] = new ShopRoomHandler(),
//             [RoomType.Treasure] = new TreasureRoomHandler(),
//             [RoomType.RestSite] = new RestSiteRoomHandler()
//         };
//         ____mapHandler = new MapScreenHandler();
//         ____screenHandlers = new Dictionary<Type, IScreenHandler>
//         {
//             [typeof(NRewardsScreen)] = new RewardsScreenHandler(),
//             [typeof(NCardRewardSelectionScreen)] = new CardRewardScreenHandler(),
//             [typeof(NDeckUpgradeSelectScreen)] = new DeckUpgradeScreenHandler(),
//             [typeof(NDeckTransformSelectScreen)] = new DeckTransformScreenHandler(),
//             [typeof(NDeckEnchantSelectScreen)] = new DeckEnchantScreenHandler(),
//             [typeof(NDeckCardSelectScreen)] = new DeckCardSelectScreenHandler(),
//             [typeof(NSimpleCardSelectScreen)] = new SimpleCardSelectScreenHandler(),
//             [typeof(NChooseACardSelectionScreen)] = new ChooseACardScreenHandler(),
//             [typeof(NChooseABundleSelectionScreen)] = new ChooseABundleScreenHandler(),
//             [typeof(NChooseARelicSelection)] = new ChooseARelicScreenHandler(),
//             [typeof(NGameOverScreen)] = new GameOverScreenHandler(),
//             [typeof(NCrystalSphereScreen)] = new CrystalSphereScreenHandler()
//         };
//         
//         return false;
//     }
// }