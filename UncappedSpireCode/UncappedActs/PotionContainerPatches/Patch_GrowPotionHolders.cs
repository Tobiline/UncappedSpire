using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Potions;

namespace UncappedSpire.UncappedSpireCode.UncappedActs.PotionContainerPatches;

[HarmonyPatch(typeof(NPotionContainer), "GrowPotionHolders")]
public class Patch_GrowPotionHolders
{
    [HarmonyPrefix]
    public static void Prefix(int newMaxPotionSlots, List<NPotionHolder> ____holders, Control ____potionHolders)
    {
        if (newMaxPotionSlots < ____holders.Count)
        {
            var emptyFirstIndexes = Enumerable.Range(0, ____holders.Count)
                .OrderBy(i => ____holders[i].HasPotion).ToList();

            for (var i = 0; i < ____holders.Count - newMaxPotionSlots; i++)
            {
                var toRemoveIndex = emptyFirstIndexes[i];
                if (____holders[toRemoveIndex].HasPotion)
                {
                    ____holders[toRemoveIndex].DiscardPotion();
                }
                ____potionHolders.RemoveChildSafely(____holders[toRemoveIndex]);
                ____holders.RemoveAt(toRemoveIndex);
            }
        }
    }
}