using FastClimbingV2.Configs;
using GameNetcodeStuff;
using HarmonyLib;
using Unity.Burst.CompilerServices;

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

            if (!FastClimbingV2.isOnTinyLadder)
            {
                if (!isNormalClimbSpeedValueSet)
                {
                    normalClimbSpeedValue = __instance.climbSpeed;
                    isNormalClimbSpeedValueSet = true;
                }

                if (__instance.isPlayerControlled && __instance.isClimbingLadder)
                {
                    __instance.climbSpeed = normalClimbSpeedValue;

                    if (__instance.isSprinting)
                    {
                        if (__instance.sprintMeter > 0.3f)
                        {
                            __instance.climbSpeed = normalClimbSpeedValue * ConfigSync.Instance.climbSpeedMultiplier;
                        }
                        else
                        {
                            __instance.isExhausted = true;
                        }
                    }
                }
            }
            
        }
    }
}
