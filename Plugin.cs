using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using ModlistVerification.Patches;

namespace ModlistVerification;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    private readonly Harmony harmony = new(MyPluginInfo.PLUGIN_GUID);
    internal static new ManualLogSource Logger;
    internal static ClientModManager ClientModManager;

#pragma warning disable IDE0051 // Remove unused private members
    private void Awake()
#pragma warning restore IDE0051 // Remove unused private members
    {
        Logger = base.Logger;

        // Initialize the config file
        ConfigManager.Initialize(Config);

        Logger.LogInfo($"Initializing collection of client's installed mods...");
        ClientModManager = ClientModManager.Initialize();

        harmony.PatchAll(typeof(MenuPageLobbyPatch));
        harmony.PatchAll(typeof(PlayerAvatarPatch));
    }
}
