using SRML;
using SRML.Utils;
using UnityEngine;
using MonomiPark.SlimeRancher.Regions;
using ElementalElectricTree;
using System.Collections.Generic;
namespace ElementalElectricTree.Creators
{
    internal class ElectricIsland
    {
        public static void Create()
        {
            GameObject zoneMUD = new GameObject("zoneMUD");
            zoneMUD.SetActive(false);
            GameObject cellMud_Entrance = new GameObject("cellMud_Entrance")
            {
                transform = { parent = zoneMUD.transform }
            };
            GameObject Sector = new GameObject("Sector")
            {
                transform = { parent = cellMud_Entrance.transform }
            };
            var zoneDirector = zoneMUD.AddComponent<ZoneDirector>();
            zoneDirector.zone = Ids.ELECTRIC_ISLAND;
            zoneDirector.auxItems = new ZoneDirector.AuxItemEntry[] { };
            zoneDirector.cratePrefab = SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.CRATE_MOSS_01);


            var idDirector = zoneMUD.AddComponent<IdDirector>();
            var cellDirector = cellMud_Entrance.AddComponent<CellDirector>();

            var region = cellMud_Entrance.AddComponent<Region>();
            region.root = Sector;
            Bounds bounds = new Bounds
            {
                center = new Vector3(-377.0013f, -26.4322f, 1292.162f),
                extents = new Vector3(177.837f, 167.3754f, 190.7108f),
                size = new Vector3(355.674f, 334.7507f, 381.4216f)
            };
            region.bounds = bounds;
            zoneMUD.SetActive(true);

            var nodeSlime = GameObjectUtils.InstantiateInactive(SRObjects.Get<GameObject>("nodeSlime"));
            nodeSlime.transform.localPosition = new Vector3(-447.1018f, 3.1856f, 1343.747f);
            nodeSlime.transform.SetParent(LoadZone.ParentCreating("Slimes", Sector));
            var directedSlimeSpawner = nodeSlime.GetComponent<DirectedSlimeSpawner>();
            cellDirector.spawners.Add(directedSlimeSpawner);
            nodeSlime.SetActive(true);
            //nodeSlime.
            LoadZone.Load(Sector, @"C:\Program Files (x86)\Steam\steamapps\common\Slime Rancher\BetterBuild\MapsToLoad\");
        }

    }
}
