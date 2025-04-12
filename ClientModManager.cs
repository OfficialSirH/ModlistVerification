using MenuLib.MonoBehaviors;
using MenuLib;
using Photon.Pun;
using System.Collections.Generic;
using HarmonyLib;
using System.Linq;
using ModlistVerification.Extensions;
using BepInEx;
using UnityEngine;

namespace ModlistVerification
{
    internal class ClientModManager : MonoBehaviour
    {
        public static List<ModMetadata> ClientMods = [];

        public static ClientModManager Initialize()
        {

            ClientMods = [.. AccessTools.AllTypes()
            .Select(type => type.SafeGetCustomAttribute<BepInPlugin>())
            .Where(plugin => plugin != null)
            .Select(plugin => plugin.ToModMetadata())];

            if (ClientMods.Count() == 0)
            {
                Plugin.Logger.LogWarning("No mods found.");
                return new ClientModManager();
            }

            Plugin.Logger.LogInfo($"Found {ClientMods.Count()} mods.");

            if (ConfigManager.DebugLogging.Value)
            {
                foreach (var mod in ClientMods)
                {
                    Plugin.Logger.LogInfo($"Mod: {mod.Name} | GUID: {mod.GUID} | Version: {mod.Version}");
                }
            }

            return new ClientModManager();
        }

        public void PropagateModlist(PlayerAvatar player)
        {
            player.photonView.RPC("HostModlistRPC", RpcTarget.Others, [ClientMods.Select(ModMetadata.ToRPCBuffer).ToArray()]);
        }

        [PunRPC]
        public void HostModlistRPC(object[][] hostModlistBuffer)
        {
            ModMetadata[] hostModlist = new ModMetadata[hostModlistBuffer.Length];
            for (int i = 0; i < hostModlistBuffer.Length; i++)
            {
                hostModlist[i] = ModMetadata.FromRPCBuffer(hostModlistBuffer[i]);
            }

            if (ConfigManager.DebugLogging.Value)
            {
                Plugin.Logger.LogInfo($"Host modlist received: {hostModlist.Length} mods");
                foreach (var mod in hostModlist)
                {
                    Plugin.Logger.LogInfo($"Mod: {mod.Name} | GUID: {mod.GUID} | Version: {mod.Version}");
                }
            }

            string[] modDiscrepancies = Utilities.VerifyModlistAgainstHost([.. hostModlist], [.. ClientMods]);

            if (ConfigManager.DebugLogging.Value)
            {
                foreach (var mod in modDiscrepancies)
                {
                    Plugin.Logger.LogDebug(mod);
                }
            }

            // create menu that displays the mod discrepancies
            var modDiscrepanciesPrompt = MenuAPI.CreateREPOPopupPage("Mod Discrepancies",
                                        presetSide: REPOPopupPage.PresetSide.Right,
                                        shouldCachePage: false,
                                        pageDimmerVisibility: true,
                                        spacing: 1.5f);

            foreach (var mod in modDiscrepancies)
            {
                modDiscrepanciesPrompt.AddElementToScrollView(scrollView =>
                {
                    // create a label for each mod discrepancy
                    var label = MenuAPI.CreateREPOLabel(mod, scrollView, localPosition: Vector2.zero);

                    return label.rectTransform;
                });
            }

            modDiscrepanciesPrompt.OpenPage(openOnTop: false);
        }
    }
}