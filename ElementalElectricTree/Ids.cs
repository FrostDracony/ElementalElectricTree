using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SRML.Utils.Enum;
using SRML.Utils;
using MonomiPark.SlimeRancher.Regions;
using UnityEngine;

namespace ElementalElectricTree
{
    [EnumHolder]
    class Ids
    {
        public static readonly Gadget.Id THUNDERCLAP_GENERATOR_GADGET;
        public static readonly Identifiable.Id ELECTRIC_SLIME;
        public static readonly Vacuumable.LaunchSource NONE;
		public static readonly PediaDirector.Id ELECTRIC_SLIME_ENTRY;
        /*public static readonly BoundsQuadtree<Region> regionsTrees = new Dictionary<RegionRegistry.RegionSetId, BoundsQuadtree<Region>>(RegionRegistry.RegionSetIdComparer.Instance)
		{
			{
				RegionRegistry.RegionSetId.HOME,
				new BoundsQuadtree<Region>(2000f, Vector3.zero, 250f, 1.2f)
			},
			{
				RegionRegistry.RegionSetId.DESERT,
				new BoundsQuadtree<Region>(2000f, Vector3.up * 1000f, 250f, 1.2f)
			},
			{
				RegionRegistry.RegionSetId.VALLEY,
				new BoundsQuadtree<Region>(2000f, Vector3.back * 900f, 250f, 1.2f)
			},
			{
				RegionRegistry.RegionSetId.VIKTOR_LAB,
				new BoundsQuadtree<Region>(2000f, new Vector3(914f, 0f, -180f), 250f, 1.2f)
			},
			{
				RegionRegistry.RegionSetId.SLIMULATIONS,
				new BoundsQuadtree<Region>(2000f, new Vector3(1142f, 0f, 1562f), 250f, 1.2f)
			}
		};*/
	}

    class Generators
    {
        //public static 
    }
}
