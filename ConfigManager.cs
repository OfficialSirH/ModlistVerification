using BepInEx.Configuration;
using REPOLib.Patches;

namespace ModlistVerification;

internal static class ConfigManager
{
    public static ConfigFile ConfigFile { get; private set; }

    public static ConfigEntry<bool> DebugLogging { get; private set; }

    public static void Initialize(ConfigFile configFile)
    {
        ConfigFile = configFile;
        BindConfigs();
    }

    private static void BindConfigs()
    {
        DebugLogging = ConfigFile.Bind("General", "DebugLogging", defaultValue: false, "Enable debug logging.");
    }
}