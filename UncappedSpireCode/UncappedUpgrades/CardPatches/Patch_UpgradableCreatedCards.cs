using System.Reflection;
using System.Runtime.CompilerServices;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;

namespace UncappedSpire.UncappedSpireCode.UncappedUpgrades.CardPatches;

[HarmonyPatch]
public class Patch_UpgradableCreatedCards
{
    private static int cachedMultiplier = UpgradeContext.GetMultiplier();
    static readonly Type[] Targets =
    [
        typeof(PrimalForce),
        typeof(Stoke),
        typeof(HiddenDaggers),
        typeof(StormOfSteel),
        typeof(KnifeTrap),
        typeof(Largesse),
        typeof(Quasar),
        typeof(Charge),
        typeof(ManifestAuthority),
        typeof(Guards),
        typeof(Reave),
        typeof(Dirge),
        typeof(Compact),
        typeof(Splash),
        typeof(Jackpot)
    ];
    
    static IEnumerable<MethodBase> TargetMethods()
    {
        foreach (var target in Targets)
        {
            var m = AccessTools.Method(target, "OnPlay");
            var attr = m.GetCustomAttribute<AsyncStateMachineAttribute>();
            if (attr == null)
            {
                MainFile.Logger.Error($"OnPlay AsyncStateMachineAttribute attribute not found for {target.Name}");
                continue;
            }
            var moveNextMethod = attr.StateMachineType.GetMethod("MoveNext", BindingFlags.NonPublic | BindingFlags.Instance);
            if (moveNextMethod == null)
            {
                MainFile.Logger.Error($"AsyncStateMachineAttribute state machine method not found for {target.Name}");
                continue;
            }
            
            yield return moveNextMethod;
        }
    }
}