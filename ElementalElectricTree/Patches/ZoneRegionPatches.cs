using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using MonomiPark.SlimeRancher.Regions;

namespace ElementalElectricTree.Patches
{
    [HarmonyPatch(typeof(ZoneDirector), nameof(ZoneDirector.GetRegionSetId))]
    public static class ZoneRegionPatches
    {
        internal static bool Prefix(ZoneDirector.Zone zone, ref RegionRegistry.RegionSetId __result)
        {
            if (zone == Ids.ELECTRIC_ISLAND)
            {
                __result = RegionRegistry.RegionSetId.VALLEY;
                return false;
            }

            return true;
        }

    }
}
