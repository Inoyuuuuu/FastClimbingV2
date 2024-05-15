using BepInEx.Bootstrap;
using BepInEx;
using GameNetcodeStuff;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;

namespace FastClimbingV2.Patches
{
    [HarmonyPatch(typeof(PlayerControllerB))]
    internal class CheckForOtherModsPatch
    {
        [HarmonyPatch("ConnectClientToPlayerObject")]
        [HarmonyPostfix]
        static void CheckForGiantExtLaddersPatch()
        {
            Dictionary<string, PluginInfo> pluginInfos = Chainloader.PluginInfos;

            foreach (PluginInfo value in pluginInfos.Values)
            {
                if (value.Metadata.GUID.Equals(FastClimbingV2.giantExtLaddersGUID))
                {
                    FastClimbingV2.Logger.LogMessage("GiantExtensionLadders is installed!");

                    FastClimbingV2.isGiantExtLaddersActive = true;
                    return;
                }
            }

            FastClimbingV2.isGiantExtLaddersActive = false;
        }
    }
}
