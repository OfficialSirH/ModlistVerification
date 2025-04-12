using HarmonyLib;
using Photon.Pun;
using System.Linq;

namespace ModlistVerification.Patches
{
    [HarmonyPatch(typeof(MenuPageLobby))]
    internal class MenuPageLobbyPatch
    {
        [HarmonyPatch(nameof(MenuPageLobby.PlayerAdd))]
        [HarmonyPostfix]
        private static void PlayerAdd(MenuPageLobby __instance, PlayerAvatar player)
        {
            if (!SemiFunc.IsMasterClient()) return;

            if (ConfigManager.DebugLogging.Value)
            {
                Plugin.Logger.LogInfo($"Player {player.photonView.Owner.NickName} added to lobby.");
            }

            Plugin.ClientModManager.PropagateModlist(player);
        }
    }
}
