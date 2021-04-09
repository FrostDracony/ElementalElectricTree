using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;
using Console = SRML.Console.Console;
using Creators;

namespace ElementalElectricTree.Patches
{
    [HarmonyPatch(typeof(ReactToShock), "Update")]
    unsafe class PatchUpdateReact
    {
        unsafe public static bool Prefix(ReactToShock __instance)
        {
            if (__instance.ToString() != "slimeQuicksilver(Clone) (ReactToShock)")
            {
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(ReactToShock), "MaybeCreatePlorts", new Type[] { typeof(int), typeof(ReactToShock.PlortSounds) })]
    unsafe class PatchCreatePlorts
    {
        unsafe public static bool Prefix(ReactToShock __instance)
        {
            /*if(__instance.ToString() == "electricSlime(Clone) (ReactToShock)")
            {
                //Vector3 targetPosition = Camera.main.transform.position;
                Vector3 targetPosition = SRSingleton<SceneContext>.Instance.Player.transform.position+new Vector3(0,2,0);
                //Abilities.CreateShoot(__instance.gameObject.transform.position + new Vector3(0, 3, 0), targetPosition - (__instance.gameObject.transform.position + new Vector3(0, 3, 0)), __instance.regionMember.setId);
                Abilities.CreateShoot(__instance.gameObject.transform.position + new Vector3(0, 3, 0), targetPosition - (__instance.gameObject.transform.position + new Vector3(0, 3, 0)), __instance.regionMember.setId);
                return false;
            }*/

            if (__instance.ToString() == "slimeGold(Clone) (ReactToShock)")
            {

                Custom_Slime_Creator.CreateElectricSlime(__instance);

                return false;
            }
            return true;
        }
    }

}
