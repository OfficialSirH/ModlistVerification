using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using ModlistVerification.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace ModlistVerification;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;

    static List<BepInPlugin> ClientMods = [];

#pragma warning disable IDE0051 // Remove unused private members
    private void Awake()
#pragma warning restore IDE0051 // Remove unused private members
    {
        Logger = base.Logger;

        // Initialize the config file
        ConfigManager.Initialize(Config);

        Logger.LogInfo($"Initializing collection of client's installed mods...");

        ClientMods = AccessTools.AllTypes()
            .Select(type => type.SafeGetCustomAttribute<BepInPlugin>())
            .Where(type => type != null)
            .ToList();

        if (ClientMods.Count() == 0)
        {
            Logger.LogWarning("No mods found.");
            return;
        }

        Logger.LogInfo($"Found {ClientMods.Count()} mods.");

        if (ConfigManager.DebugLogging.Value)
        {
            foreach (var mod in ClientMods)
            {
                Logger.LogInfo($"Mod: {mod.Name} | GUID: {mod.GUID} | Version: {mod.Version}");
            }
        }
    }
}
