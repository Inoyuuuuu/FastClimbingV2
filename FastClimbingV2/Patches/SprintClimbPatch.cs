using FastClimbingV2.Configs;
using GameNetcodeStuff;
using HarmonyLib;

namespace FastClimbingV2.Patches
{
    [HarmonyPatch(typeof(PlayerControllerB))]
    internal class SprintClimbPatch
    {
        private static float normalClimbSpeedValue;
        private static bool isNormalClimbSpeedValueSet = false;

        [HarmonyPatch("Update")]
        [HarmonyPriority(Priority.VeryLow)]
        [HarmonyPostfix]
        static void crankThatClimbingSpeed(PlayerControllerB __instance)
        {
            if (!isNormalClimbSpeedValueSet)
            {
                normalClimbSpeedValue = __instance.climbSpeed;
                isNormalClimbSpeedValueSet = true;
            }

            if (__instance.isPlayerControlled && __instance.isClimbingLadder && __instance.isSprinting)
            {
                if (__instance.sprintMeter > 0.3f)
                {
                    __instance.climbSpeed = normalClimbSpeedValue * ConfigSync.Instance.climbSpeedMultiplier;
                }
                else
                {
                    __instance.climbSpeed = normalClimbSpeedValue;
                    __instance.isExhausted = true;
                }
            }
            else
            {
                __instance.climbSpeed = normalClimbSpeedValue;
            }
        }
    }
}
