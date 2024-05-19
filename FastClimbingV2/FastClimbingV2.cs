using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Logging;
using FastClimbingV2.Configs;
using FastClimbingV2.Patches;
using HarmonyLib;
using System.Collections.Generic;

namespace FastClimbingV2
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class FastClimbingV2 : BaseUnityPlugin
    {
        public static FastClimbingV2 Instance { get; private set; } = null!;
        internal new static ManualLogSource Logger { get; private set; } = null!;
        internal static Harmony? Harmony { get; set; }

        internal static string TINY_LADDER_PREFIX = "tinyLadder";

        internal static bool isOnTinyLadder = false;

        private void Awake()
        {
            Logger = base.Logger;
            Instance = this;
            FastClimbingConfigs.BindConfigs(((BaseUnityPlugin)FastClimbingV2.Instance).Config);

            Patch();

            Logger.LogInfo($"{MyPluginInfo.PLUGIN_GUID} v{MyPluginInfo.PLUGIN_VERSION} has loaded!");
        }

        internal static void Patch()
        {
            Harmony ??= new Harmony(MyPluginInfo.PLUGIN_GUID);

            Logger.LogDebug("Patching...");

            Harmony.PatchAll();

            Logger.LogDebug("Finished patching!");
        }

        
    }
}
