using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;

namespace UncappedSpire.UncappedSpireCode.UncappedActs.NCharacterSelectScreenPatches;

[HarmonyPatch(typeof(NCharacterSelectScreen), nameof(NCharacterSelectScreen.ModifiersChanged))]
public class Patch_ModifiersChanged
{
    [HarmonyPrefix]
    public static bool Prefix()
    {
        return false;
    }
}