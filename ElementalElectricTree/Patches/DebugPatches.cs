using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;
using Console = SRML.Console.Console;
using MonomiPark.SlimeRancher.Regions;
using ElementalElectricTree.Other;
using SRML.SR;
namespace ElementalElectricTree.Patches
{
    [HarmonyPatch(typeof(RegionRegistry), "GetContaining", new Type[] { typeof(RegionRegistry.RegionSetId), typeof(List<Region>), typeof(Vector3) }, new ArgumentType[] { ArgumentType.Normal, ArgumentType.Ref, ArgumentType.Normal })]
    unsafe class DebugPatches
    {
        unsafe public static bool Prefix(RegionRegistry __instance, RegionRegistry.RegionSetId setId, ref List<Region> regions)
        {
            ///Console.Log(setId.ToString());
            //BoundsQuadtree<Region> test = null;

            /*foreach(Region region in regions)
            {
                Console.Log(region.ToString());
            }*/

            if(__instance.regionsTrees.TryGetValue(setId, out BoundsQuadtree<Region> test))
            {
                //Console.Log("true");
                return true;
            }
            else //else, if its something modded, stop the code from continuing and avoid an error
            {
                //Console.Log("false");
                return false;
            }
        }
    }

    [HarmonyPatch(typeof(WeaponVacuum), "Expel", new Type[] { typeof(GameObject), typeof(bool) })]
    unsafe class DebugPatches1
    {
        unsafe public static void Postfix(WeaponVacuum __instance, GameObject toExpel, bool ignoreEmotions)
        {
            ///toExpel.PrintComponent();
        }
    }

}
