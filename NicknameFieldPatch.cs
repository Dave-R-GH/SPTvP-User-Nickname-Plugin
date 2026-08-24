using Arena.UI;
using EFT;
using EFT.Settings.Game;
using EFT.UI.Settings;
using HarmonyLib;

namespace SPTvP.UserNicknamePlugin;

/* Patch for locking the nickname of a user to their
   SPTvP username, and not allowing any changes */

    // Profile Creation Part
[HarmonyPatch(typeof(NicknameField))]
internal static class NicknameFieldPatch {

    [HarmonyPostfix]
    [HarmonyPatch(nameof(NicknameField.Init))]
    private static void InitPostfix(NicknameField __instance){
        ApplyLaunchUsername(__instance);
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(NicknameField.ValidationCallback))]
    private static void ValidationCallbackPostfix(NicknameField __instance){
        ApplyLaunchUsername(__instance);
        __instance._inputField.interactable = false;
    }

    private static void ApplyLaunchUsername(NicknameField __instance){
        LaunchIdentity? identity = Plugin.CurrentIdentity;

        if(identity == null){
            return;
        }

        string username = identity.Username;

        __instance._inputField.SetTextWithoutNotify(username);
        __instance._statusLabel.text = "SPTvP Username";
        __instance._inputField.readOnly = true;
        __instance._inputField.interactable = false;
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(NicknameField.SetInfoPanel))]
    private static void SetInfoPanelPostfix(NicknameField __instance){
        __instance._statusLabel.text = "SPTvP Username";
    }
}

    // Game Settings Part
[HarmonyPatch(typeof(GameSettingsTab))]
internal static class GameSettingsPatch {

    [HarmonyPostfix]
    [HarmonyPatch(nameof(GameSettingsTab.Show),
    new [] { typeof(GameSettingsGroup), typeof(IEftSession), typeof(bool) }
    )]
    private static void LockNicknameTextboxPostfix(GameSettingsTab __instance){
        __instance._nicknameInput.interactable = false;
        __instance._changeNicknameButton.Interactable = false;
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(GameSettingsTab.UpdateChangeNicknameButton))]
    private static void LockNicknameChangeButtonPostfix(GameSettingsTab __instance){
        __instance._changeNicknameButton.Interactable = false;
    }
}
