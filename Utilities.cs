using BepInEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModlistVerification
{
    internal class Utilities
    {
        /// <summary>
        /// Verifies the modlist against the host's modlist.
        /// Will return a list of mods that are missing or have different versions.
        /// The list will be formmatted for a UI prompt as:
        /// <br></br>
        /// Missing mod: <color=red>ModName</color> Version: <color=red>Version</color>
        /// <br></br>
        /// Mismatched mod(yellow for patch, orange for minor, red for major): 
        /// <color=severity_color>ModName</color> Host&Client Version: HostVersion - <color=severity_color>Version</color>
        /// </summary>
        public static string[] VerifyModlistAgainstHost(List<ModMetadata> hostModlist, ModMetadata[] clientModlist)
        {
            List<string> modDiscrepancies = [];
            // Missing Mods
            List<ModMetadata> missingMods = [];
            foreach (var mod in hostModlist)
            {
                if (!clientModlist.Contains(mod, new IModMetadataEqualityComparer()))
                {
                    modDiscrepancies.Add($"Missing mod: <color=red>{mod.Name}</color> Version: <color=red>{mod.Version}</color>");
                    missingMods.Add(mod);
                }
            }

            foreach (var mod in missingMods)
            {
                hostModlist.Remove(mod);
            }
            missingMods.Clear();

            // Mismatched Mods
            foreach (var mod in hostModlist)
            {
                var clientMod = clientModlist.FirstOrDefault(m => m.Name == mod.Name);
                if (clientMod.Version != mod.Version)
                {
                    // Check for major, minor, and patch version differences
                    var hostVersionParts = mod.Version.Split('.');
                    var clientVersionParts = clientMod.Version.Split('.');
                    if (hostVersionParts.Length == 3 && clientVersionParts.Length == 3)
                    {
                        int hostMajor = int.Parse(hostVersionParts[0]);
                        int hostMinor = int.Parse(hostVersionParts[1]);
                        int hostPatch = int.Parse(hostVersionParts[2]);
                        int clientMajor = int.Parse(clientVersionParts[0]);
                        int clientMinor = int.Parse(clientVersionParts[1]);
                        int clientPatch = int.Parse(clientVersionParts[2]);
                        if (clientMajor != hostMajor)
                        {
                            modDiscrepancies.Add($"<color=red>{mod.Name}</color> Host Version: {mod.Version} - <color=red>{clientMod.Version}</color>");
                        }
                        else if (clientMinor != hostMinor)
                        {
                            modDiscrepancies.Add($"<color=orange>{mod.Name}</color> Host Version: {mod.Version} - <color=orange>{clientMod.Version}</color>");
                        }
                        else if (clientPatch != hostPatch)
                        {
                            modDiscrepancies.Add($"<color=yellow>{mod.Name}</color> Host Version: {mod.Version} - <color=yellow>{clientMod.Version}</color>");
                        }
                    }
                }
            }

            return [.. modDiscrepancies];
        }

        public class IModMetadataEqualityComparer : IEqualityComparer<ModMetadata>
        {
            public bool Equals(ModMetadata x, ModMetadata y)
            {
                return x.Name == y.Name;
            }
            public int GetHashCode(ModMetadata obj)
            {
                return obj.GUID.GetHashCode() ^ obj.Version.GetHashCode();
            }
        }
    }
}
