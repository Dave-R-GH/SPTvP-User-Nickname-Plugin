using BepInEx;
using HarmonyLib;

namespace SPTvP.UserPlugin;

[BepInPlugin("com.stoneworth.sptvp", "Stoneworth-SPTvP", "0.0.1")]
public sealed class Plugin : BaseUnityPlugin {
    private void Awake(){
        var harmony = new Harmony("com.stoneworth.sptvp");
        harmony.PatchAll();
        Logger.LogInfo("SPTvP User Nickname Plugin Loaded. (0.0.1)");
    }
}