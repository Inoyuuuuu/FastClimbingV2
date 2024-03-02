using BepInEx.Configuration;
using CSync;
using CSync.Lib;
using CSync.Util;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace FastClimbingV2.Configs
{
    [DataContract]
    internal class FastClimbingConfigs : SyncedConfig<FastClimbingConfigs>
    {
        [DataMember] public SyncedEntry<float> CLIMB_SPEED_MULTIPLIER { get; private set; }
        private const float sprintClimbSpeedMultiplierBaseValue = 1.8f;

        public FastClimbingConfigs(ConfigFile cfg) : base(MyPluginInfo.PLUGIN_NAME)
        {
            ConfigManager.Register(this);

            CLIMB_SPEED_MULTIPLIER = cfg.BindSyncedEntry("SprintClimbing", "sprintClimbingSpeed", sprintClimbSpeedMultiplierBaseValue, "A multiplier for the climbing speed while sprinting!");
            
            fixClimbspeed();
        }

        private void fixClimbspeed()
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
