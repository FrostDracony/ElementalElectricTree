using SRML;
using SRML.Utils;
using UnityEngine;
using MonomiPark.SlimeRancher.Regions;
using ElementalElectricTree;
using System.Collections.Generic;
namespace ElementalElectricTree.Creators
{
    public class ElectricCanyon
    {
        public static void Create()
        {
            /*#region Electric Canyon Parkour
            GameObject zoneELECTRIC_CANYON_ENTRANCE = new GameObject("zoneELECTRIC_CANYON_ENTRANCE");
            zoneELECTRIC_CANYON_ENTRANCE.SetActive(false);
            GameObject cellElectricCanyon_Entrance = new GameObject("cellElectricCanyon_Entrance")
            {
                transform = { parent = zoneELECTRIC_CANYON_ENTRANCE.transform }
            };
            GameObject Sector = new GameObject("Sector")
            {
                transform = { parent = cellElectricCanyon_Entrance.transform }
            };
            var zoneDirector = zoneELECTRIC_CANYON_ENTRANCE.AddComponent<ZoneDirector>();
            zoneDirector.zone = Ids.ELECTRIC_ISLAND;
            zoneDirector.auxItems = new ZoneDirector.AuxItemEntry[] { };
            zoneDirector.cratePrefab = SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.CRATE_MOSS_01);


            var idDirector = zoneELECTRIC_CANYON_ENTRANCE.AddComponent<IdDirector>();
            var cellDirector = cellElectricCanyon_Entrance.AddComponent<CellDirector>();

            var region = cellElectricCanyon_Entrance.AddComponent<Region>();
            region.root = Sector;
            Bounds bounds = new Bounds
            {
                center = new Vector3(-88.2462f, 18.4031f, -800.6295f),
                extents = new Vector3(61.4104f, 29.5816f, 29.1315f),
                size = new Vector3(122.8208f, 59.1632f, 58.2631f)
            };
            region.bounds = bounds;
            zoneELECTRIC_CANYON_ENTRANCE.SetActive(true);

            var nodeSlime = GameObjectUtils.InstantiateInactive(SRObjects.Get<GameObject>("nodeSlime"));
            nodeSlime.transform.localPosition = new Vector3(-447.1018f, 3.1856f, 1343.747f);
            nodeSlime.transform.SetParent(LoadZone.ParentCreating("Slimes", Sector));
            var directedSlimeSpawner = nodeSlime.GetComponent<DirectedSlimeSpawner>();
            cellDirector.spawners.Add(directedSlimeSpawner);
            nodeSlime.SetActive(true);
            //nodeSlime.

            //LoadZone.Load(Sector, new System.IO.StreamReader(Main.manifestResourceStreamZones).ReadToEnd());
            #endregion*/



            /*
            #region Electric Canyon Parkour



            GameObject zoneELECTRIC_CANYON_ENTRANCE = new GameObject("zoneELECTRIC_CANYON_ENTRANCE");
            zoneELECTRIC_CANYON_ENTRANCE.SetActive(false);
            GameObject cellElectricCanyon_Entrance = new GameObject("cellElectricCanyon_Entrance")
            {
                transform = { parent = zoneELECTRIC_CANYON_ENTRANCE.transform }
            };
            GameObject Sector = new GameObject("Sector")
            {
                transform = { parent = cellElectricCanyon_Entrance.transform }
            };
            var zoneDirector = zoneELECTRIC_CANYON_ENTRANCE.AddComponent<ZoneDirector>();
            zoneDirector.zone = Ids.ELECTRIC_ISLAND;
            zoneDirector.auxItems = new ZoneDirector.AuxItemEntry[] { };
            zoneDirector.cratePrefab = SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.CRATE_MOSS_01);


            var idDirector = zoneELECTRIC_CANYON_ENTRANCE.AddComponent<IdDirector>();
            var cellDirector = cellElectricCanyon_Entrance.AddComponent<CellDirector>();

            var region = cellElectricCanyon_Entrance.AddComponent<Region>();
            region.root = Sector;
            Bounds bounds = new Bounds
            {
                center = new Vector3(-21.3237f, 26.6994f, -978.0636f),
                extents = new Vector3(69.4729f, 64.014f, 77.7857f),
                size = new Vector3(138.9457f, 128.0279f, 155.5715f)
            };
            region.bounds = bounds;
            zoneELECTRIC_CANYON_ENTRANCE.SetActive(true);

            var nodeSlime = GameObjectUtils.InstantiateInactive(SRObjects.Get<GameObject>("nodeSlime"));
            nodeSlime.transform.localPosition = new Vector3(-447.1018f, 3.1856f, 1343.747f);
            nodeSlime.transform.SetParent(LoadZone.ParentCreating("Slimes", Sector));
            var directedSlimeSpawner = nodeSlime.GetComponent<DirectedSlimeSpawner>();
            cellDirector.spawners.Add(directedSlimeSpawner);
            nodeSlime.SetActive(true);
            //nodeSlime.

            LoadZone.Load(Sector, new System.IO.StreamReader(Main.manifestResourceStreamZones).ReadToEnd());










            #endregion
            */


        }

    }
}
