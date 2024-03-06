using BepInEx.Configuration;
using GameNetcodeStuff;
using HarmonyLib;
using System;
using System.Runtime.Serialization;
using Unity.Collections;
using Unity.Netcode;

namespace FastClimbingV2.Configs
{
    [Serializable]
    public static class FastClimbingConfigs
    {
        public static ConfigEntry<float> CLIMB_SPEED_MULTIPLIER;

        private const float sprintClimbSpeedMultiplierBaseValue = 1.8f;
        public static void BindConfigs(ConfigFile cfg)
        {
            CLIMB_SPEED_MULTIPLIER = cfg.Bind("SprintClimbing", "sprintClimbingSpeed", sprintClimbSpeedMultiplierBaseValue, "A multiplier for the climbing speed while sprinting!");
            fixClimbspeed();
        }

        private static void fixClimbspeed()
        {
            if (CLIMB_SPEED_MULTIPLIER.Value > 50.0f)
            {
                CLIMB_SPEED_MULTIPLIER.Value = 50.0f;
                FastClimbingV2.Logger.LogInfo("climbing speed value too high, was set to 50.");
            }
            if (CLIMB_SPEED_MULTIPLIER.Value < 1.0f)
            {
                CLIMB_SPEED_MULTIPLIER.Value = 1.0f;
                FastClimbingV2.Logger.LogInfo("climbing speed value too low, was set to 1.");
            }
        }
    }
}
