using BepInEx.Bootstrap;
using BepInEx;
using GameNetcodeStuff;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace FastClimbingV2.Patches
{
    [HarmonyPatch(typeof(InteractTrigger))]
    internal class CheckForOtherModsPatch
    {
        [HarmonyPrefix]
        [HarmonyPriority(Priority.High)]
        [HarmonyPatch("ladderClimbAnimation")]
        static void CheckForGiantExtLaddersPatch(InteractTrigger __instance, ref PlayerControllerB playerController)
        {
            if (__instance.transform.parent != null
                && __instance.transform.parent.transform.parent != null
                && __instance.transform.parent.transform.parent.transform.parent != null 
                && __instance.transform.parent.transform.parent.transform.parent.name.StartsWith(FastClimbingV2.TINY_LADDER_PREFIX))
            {
                FastClimbingV2.Logger.LogDebug("player started climbing this ladder: " + __instance.transform.parent.transform.parent.transform.parent.name);
                FastClimbingV2.isOnTinyLadder = true;
            } else
            {
                FastClimbingV2.isOnTinyLadder = false;
            }
        }
    }
}
