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

        private const float BASE_SPRINT_SPEED_MULTIPLIER = 1.85f;
        private const float MAX_SPRINT_SPEED_MULTIPLIER = 80f;
        private const float MIN_SPRINT_SPEED_MULTIPLIER = 1f;

        public static void BindConfigs(ConfigFile cfg)
        {
            CLIMB_SPEED_MULTIPLIER = cfg.Bind("SprintClimbing", "sprintClimbingSpeed", BASE_SPRINT_SPEED_MULTIPLIER, "A multiplier for the climbing speed while sprinting!");
            fixClimbspeed();
        }

        private static void fixClimbspeed()
        {
            if (CLIMB_SPEED_MULTIPLIER.Value > MAX_SPRINT_SPEED_MULTIPLIER)
            {
                CLIMB_SPEED_MULTIPLIER.Value = MAX_SPRINT_SPEED_MULTIPLIER;
                FastClimbingV2.Logger.LogInfo("climbing speed value too high, was set to " + MAX_SPRINT_SPEED_MULTIPLIER + ".");
            }
            if (CLIMB_SPEED_MULTIPLIER.Value < MIN_SPRINT_SPEED_MULTIPLIER)
            {
                CLIMB_SPEED_MULTIPLIER.Value = MIN_SPRINT_SPEED_MULTIPLIER;
                FastClimbingV2.Logger.LogInfo("climbing speed value too low, was set to " + MIN_SPRINT_SPEED_MULTIPLIER + ".");
            }
        }
    }
}
