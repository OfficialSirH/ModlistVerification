using BepInEx;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModlistVerification.Extensions
{
    internal static class BepInPluginExtensions
    {
        public static ModMetadata ToModMetadata(this BepInPlugin plugin)
        {
            return new ModMetadata
            {
                Name = plugin.Name,
                Version = plugin.Version.ToString(),
                GUID = plugin.GUID
            };
        }
    }
}
