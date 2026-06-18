using System.Reflection;
using System.Text.Json.Serialization.Metadata;
using BaseLib.Config;
using BaseLib.Patches.Saves;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;
using UncappedSpire.UncappedSpireCode.Config;

namespace UncappedSpire.UncappedSpireCode;

//You're recommended but not required to keep all your code in this package and all your assets in the UncappedSpire folder.
[ModInitializer(nameof(Initialize))]
public partial class MainFile : Node
{
    public const string ModId = "UncappedSpire"; //At the moment, this is used only for the Logger and harmony names.

    public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } =
        new(ModId, MegaCrit.Sts2.Core.Logging.LogType.Generic);

    public static void Initialize()
    {
        //If you want to use scripts defined in your mod for Godot scenes, uncomment the following line.
        //Godot.Bridge.ScriptManagerBridge.LookupScriptsInAssembly(Assembly.GetExecutingAssembly());
        
        ExtendedSaveTypes.RegisterAdditionalSaveType<float>((resolver, options) =>
            JsonTypeInfo.CreateJsonTypeInfo<float>(options));
        
        ModConfigRegistry.Register(ModId, new UncappedConfig());
        
        var assembly = Assembly.GetExecutingAssembly();
        Godot.Bridge.ScriptManagerBridge.LookupScriptsInAssembly(assembly);
        
        Harmony harmony = new(ModId);

        harmony.PatchAll();
    }
}