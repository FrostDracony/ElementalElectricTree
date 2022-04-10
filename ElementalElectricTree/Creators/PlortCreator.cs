using UnityEngine;
using SRML.Utils;
using SRML.SR;

using ElementalElectricTree.Other;
using ElementalElectricTree;
namespace Creators
{
    static class Custom_Plort_Creator
    {
        unsafe public static void RegisterPlort(GameObject plort, Sprite icon, Color backGroundColor, int price, int saturation)
        {
            Identifiable.Id id = plort.GetComponent<Identifiable>().id;
            AmmoRegistry.RegisterAmmoPrefab(PlayerState.AmmoMode.DEFAULT, plort);
            // Icon that is below is just a placeholder. You can change it to anything or add your own!	
            //Sprite PlortIcon = ((IEnumerable<PediaDirector.IdEntry>)SRSingleton<SceneContext>.Instance.PediaDirector.entries).First<PediaDirector.IdEntry>((Func<PediaDirector.IdEntry, bool>)(x => x.id == PediaDirector.Id.PLORTS)).icon;
            //Sprite icon = SRSingleton<SceneContext>.Instance.PediaDirector.entries.First((PediaDirector.IdEntry x) => x.id == plortIcon).icon;

            //Color PureWhite = new Color32(255, 255, 255, byte.MaxValue); // RGB	
            LookupRegistry.RegisterVacEntry(VacItemDefinition.CreateVacItemDefinition(id, backGroundColor, icon));
            AmmoRegistry.RegisterSiloAmmo((x => x == SiloStorage.StorageType.NON_SLIMES || x == SiloStorage.StorageType.PLORT), id);

            //float price = 600f; //Starting price for plort	
            //float saturation = 10f; //Can be anything. The higher it is, the higher the plort price changes every day. I'd recommend making it small so you don't destroy the economy lol.	
            PlortRegistry.AddEconomyEntry(id, price, saturation); //Allows it to be sold while the one below this adds it to plort market.	
            PlortRegistry.AddPlortEntry(id); //PlortRegistry.AddPlortEntry(YourCustomEnum, new ProgressDirector.ProgressType[1]{ProgressDirector.ProgressType.NONE});	
            DroneRegistry.RegisterBasicTarget(id);
            AmmoRegistry.RegisterRefineryResource(id); //For if you want to make a gadget that uses it	
        }

        unsafe public static GameObject CreateElectricPlortPrefab(string name, Identifiable.Id id, Identifiable.Id plortToCopyFrom)
        {
            GameObject Prefab = PrefabUtils.CopyPrefab(SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(plortToCopyFrom));
            Prefab.name = name;

            Prefab.GetComponent<Identifiable>().id = id;
            Prefab.GetComponent<Vacuumable>().size = Vacuumable.Size.NORMAL;

            Prefab.GetComponent<MeshRenderer>().material = Object.Instantiate<Material>(Prefab.GetComponent<MeshRenderer>().material);
            Color PureYellow = new Color32(255, 255, 0, byte.MaxValue);

            Prefab.GetComponent<MeshRenderer>().material.SetColor("_TopColor", PureYellow);
            Prefab.GetComponent<MeshRenderer>().material.SetColor("_MiddleColor", PureYellow);
            Prefab.GetComponent<MeshRenderer>().material.SetColor("_BottomColor", PureYellow);
            Prefab.GetComponent<MeshRenderer>().material.SetTexture("_CubemapOverride", Main.assetBundle.LoadAsset<Texture>("electric_sphere_1"));
            LookupRegistry.RegisterIdentifiablePrefab(Prefab);
            Object.Destroy(Prefab.GetComponent<QuicksilverPlortCollector>());

            return Prefab;
        }
    }
}
