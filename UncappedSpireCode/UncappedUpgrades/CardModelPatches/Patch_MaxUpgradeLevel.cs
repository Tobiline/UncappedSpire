using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using UncappedSpire.UncappedSpireCode.Config;
using UncappedSpire.UncappedSpireCode.UncappedActs;

namespace UncappedSpire.UncappedSpireCode.UncappedUpgrades.CardModelPatches;

[HarmonyPatch(typeof(CardModel), nameof(CardModel.MaxUpgradeLevel), MethodType.Getter)]
public static class Patch_MaxUpgradeLevel
{
    // Only for -1 energy per upgrade
    public static readonly HashSet<Type> CardsWithOnlyEnergyUpgrades =
    [
        // Ironclad
        typeof(BodySlam),
        typeof(Havoc),
        typeof(InfernalBlade),
        typeof(ExpectAFight),
        typeof(Stampede),
        typeof(Tank),
        typeof(DarkEmbrace),
        typeof(Hellraiser),
        typeof(Unmovable),
        typeof(Barricade),
        typeof(Corruption),
        
        // Silent
        typeof(Mirage),
        typeof(Flanking),
        typeof(Murder),
        typeof(ShadowStep),
        typeof(Shadowmeld),
        typeof(BulletTime),
        typeof(Nightmare),
        typeof(ToolsOfTheTrade),
        typeof(MasterPlanner),
        typeof(Tracking),
        
        // Regent
        typeof(Orbit),
        typeof(SpectrumShift),
        typeof(SevenStars),
        typeof(HammerTime),
        typeof(SwordSage),
        typeof(MonarchsGaze),
        
        // Necrobinder
        typeof(Pagestorm),
        typeof(Sacrifice),
        typeof(Seance),
        typeof(Eidolon),
        typeof(SentryMode),
        typeof(Demesne),
        typeof(Protector),
        typeof(ForbiddenGrimoire),
        
        // Defect
        typeof(Dualcast),
        typeof(Zap),
        typeof(DoubleEnergy),
        typeof(WhiteNoise),
        typeof(Fusion),
        typeof(Subroutine),
        typeof(Feral),
        typeof(SignalBoost),
        typeof(CreativeAi),
        typeof(Quadcast),
        
        // Colorless
        typeof(MindBlast),
        typeof(Automation),
        typeof(Stratagem),
        typeof(Alchemize),
        typeof(Nostalgia),
        typeof(Mayhem),
        typeof(Calamity),
        
        // Ancients
        typeof(Apotheosis),
        
        // Special
        typeof(Distraction),
        typeof(Entrench),
        typeof(SovereignBlade),
    ];

    public static readonly Dictionary<Type, int> CardUpgradeMaxMap = new()
    {
        // Ironclad
        [typeof(Armaments)] = 1,
        [typeof(DemonicShield)] = 1,
        [typeof(Juggling)] = 1,
        [typeof(Aggression)] = 1,
        
        // Silent
        [typeof(BladeDance)] = 7,
        [typeof(CloakAndDagger)] = 9,
        [typeof(CalculatedGamble)] = 1,
        [typeof(UpMySleeve)] = 7,
        [typeof(InfiniteBlades)] = 1,
        [typeof(WellLaidPlans)] = 9,
        [typeof(Speedster)] = 1,
        [typeof(BladeOfInk)] = 8,
        [typeof(Afterimage)] = 1,
        [typeof(FanOfKnives)] = 6,
        
        // Regent
        [typeof(KnowThyPlace)] = 1,
        [typeof(Monologue)] = 1,
        [typeof(RoyalGamble)] = 1,
        [typeof(BigBang)] = 1,
        [typeof(Arsenal)] = 1,
        [typeof(Tyranny)] = 1,
        [typeof(VoidForm)] = 1,
        [typeof(TheSealedThrone)] = 1,
        
        // Necrobinder
        [typeof(Wisp)] = 1,
        [typeof(Dredge)] = 1,
        [typeof(TimesUp)] = 1,
        [typeof(BansheesCry)] = 5,
        [typeof(Transfigure)] = 1,
        [typeof(CallOfTheVoid)] = 1,
        [typeof(ReaperForm)] = 1,
        
        // Defect
        [typeof(Hotfix)] = 1,
        [typeof(Chill)] = 1,
        [typeof(Synchronize)] = 1,
        [typeof(Capacitor)] = 8,
        [typeof(Ignition)] = 1,
        [typeof(Rainbow)] = 1,
        [typeof(Voltaic)] = 1,
        [typeof(MachineLearning)] = 1,
        [typeof(TrashToTreasure)] = 1,
        [typeof(EchoForm)] = 1,
        
        // Colorless
        [typeof(Prolong)] = 1,
        [typeof(ThinkingAhead)] = 1,
        [typeof(Discovery)] = 1,
        [typeof(GoldAxe)] = 1,
        [typeof(SecretTechnique)] = 1,
        [typeof(SecretWeapon)] = 1,
        [typeof(Anointed)] = 1,
        [typeof(Mimic)] = 1,
        [typeof(Scrawl)] = 1,
        [typeof(BeaconOfHope)] = 1,
        [typeof(Entropy)] = 1,
        
        // Ancients
        [typeof(Wish)] = 1,
        [typeof(Apparition)] = 1,
        
        // Special
        [typeof(Enlightenment)] = 1,
        [typeof(HelloWorld)] = 1,
    };
    
    public static void Postfix(CardModel __instance, ref int __result)
    {
        if (ContextManager.UncappedUpgradesEnabled)
        {
            var instanceType = __instance.GetType();
            if (CardsWithOnlyEnergyUpgrades.Contains(instanceType))
            {
                __result = __instance.EnergyCost.Canonical;
            }
            else if (CardUpgradeMaxMap.TryGetValue(instanceType, out var maxLevel))
            {
                __result = maxLevel;
            }
            else
            {
                __result = int.MaxValue;
            }
        }
    }
}