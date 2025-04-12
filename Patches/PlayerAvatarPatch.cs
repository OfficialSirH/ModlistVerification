using HarmonyLib;

namespace ModlistVerification.Patches
{
    [HarmonyPatch(typeof(PlayerAvatar))]
    [HarmonyPatch(nameof(PlayerAvatar.Awake))]
    class PlayerAvatarPatch
    {
        public static void Postfix(PlayerAvatar __instance)
        {
            // add ClientModManager component
            __instance.gameObject.AddComponent<ClientModManager>();
        }
    }
}
