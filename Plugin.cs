using System;
using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace SPTvP.UserNicknamePlugin;

[BepInPlugin("com.stoneworth.sptvp.usernickname", "SPTvP User Nickname Plugin", "0.0.1")]
public sealed class Plugin : BaseUnityPlugin {
    private const string LaunchSessionVariable = "SPTVP_LAUNCH_SESSION";
    internal static LaunchIdentity? CurrentIdentity {
        get;
        private set;
    }

    private void Awake(){
        string handoffPath = Environment.GetEnvironmentVariable(LaunchSessionVariable);

        if(string.IsNullOrWhiteSpace(handoffPath)){
            Logger.LogInfo(
                "No SPTvP launch session was provided. " + "The User Nickname Plugin will remain inactive."
            );
            return;
        }
        if(!LaunchIdentityReader.TryRead(
            handoffPath,
            out LaunchIdentity? identity,
            out string error)){
                Logger.LogFatal(
                    "The SPTvP launch session was rejected: " + error
                );
                Application.Quit();
                return;
            }
        CurrentIdentity = identity;

        var harmony = new Harmony("com.stoneworth.sptvp.usernickname");

        harmony.PatchAll();

        Logger.LogInfo("SPTvP User Nickname Plugin enabled for " + identity.Username);
    }
}

