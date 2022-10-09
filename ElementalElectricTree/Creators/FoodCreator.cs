using System.Collections.Generic;

using ElementalElectricTree;

using UnityEngine;
using SRML.SR;
using SRML.Utils;
using System.Linq;

namespace Creators
{
    static class Custom_Food_Creator
    {
        public static void RegisterAllFoods()
        {
            RegisterFood(SlimeEat.FoodGroup.MEAT, CreateElectricHenPrefab(), Color.yellow, ((IEnumerable<PediaDirector.IdEntry>)SRSingleton<SceneContext>.Instance.PediaDirector.entries).First(x => x.id == PediaDirector.Id.HENHEN).icon);
            RegisterFood(SlimeEat.FoodGroup.MEAT, CreateElectricRoosterPrefab(), Color.yellow, Main.assetBundle.LoadAsset<Sprite>("electricRoostro"));
        }
        public static void RegisterFood(SlimeEat.FoodGroup foodGroup, GameObject gameObject, Color backGroundColor, Sprite icon, Identifiable.Id id = Identifiable.Id.NONE)
        {
            LookupRegistry.RegisterIdentifiablePrefab(gameObject);

            HashSet<Identifiable.Id> foodGroupSet = null;

            if (id == Identifiable.Id.NONE)
                id = gameObject.GetComponent<Identifiable>().id;

            switch (foodGroup)
            {
                case SlimeEat.FoodGroup.FRUIT:
                    foodGroupSet = Identifiable.FOOD_CLASS;
                    break;
                case SlimeEat.FoodGroup.VEGGIES:
                    foodGroupSet = Identifiable.VEGGIE_CLASS;
                    break;
                case SlimeEat.FoodGroup.MEAT:
                    foodGroupSet = Identifiable.MEAT_CLASS;
                    break;
                default:
                    break;
            }

            foodGroupSet.Add(id);
            Identifiable.NON_SLIMES_CLASS.Add(id);
            AmmoRegistry.RegisterAmmoPrefab(PlayerState.AmmoMode.DEFAULT, gameObject);
            // Icon that is below is just a placeholder. You can change it to anything or add your own!	
            //Sprite PlortIcon = ((IEnumerable<PediaDirector.IdEntry>)SRSingleton<SceneContext>.Instance.PediaDirector.entries).First<PediaDirector.IdEntry>((Func<PediaDirector.IdEntry, bool>)(x => x.id == PediaDirector.Id.PLORTS)).icon;
            //Sprite icon = SRSingleton<SceneContext>.Instance.PediaDirector.entries.First((PediaDirector.IdEntry x) => x.id == plortIcon).icon;

            //Color PureWhite = new Color32(255, 255, 255, byte.MaxValue); // RGB	
            LookupRegistry.RegisterVacEntry(VacItemDefinition.CreateVacItemDefinition(id, backGroundColor, icon));
            AmmoRegistry.RegisterSiloAmmo((System.Predicate<SiloStorage.StorageType>)(x => x == SiloStorage.StorageType.NON_SLIMES || x == SiloStorage.StorageType.PLORT), id);

            DroneRegistry.RegisterBasicTarget(id);
            AmmoRegistry.RegisterRefineryResource(id); //For if you want to make a gadget that uses it	
        }

        unsafe public static GameObject CreateElectricHenPrefab()
        {
            GameObject Prefab = PrefabUtils.CopyPrefab(SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.HEN));

            SkinnedMeshRenderer mRender = Prefab.transform.Find("Hen Hen/mesh_body1").gameObject.GetComponent<SkinnedMeshRenderer>();
            Material HenMat = UnityEngine.Object.Instantiate<Material>(mRender.sharedMaterial);
            HenMat.SetTexture("_RampGreen", ElementalElectricTree.Main.assetBundle.LoadAsset<Texture>("ramp_meat_yellow"));
            //HenMat.SetTexture("_RampRed", GreenRamp);
            //HenMat.SetTexture("_RampBlue", Main.assetBundle.LoadAsset<Texture>("ramp_meat_yellow"));
            //HenMat.SetTexture("_RampBlack", WhiteRamp);
            mRender.sharedMaterial = HenMat;
            foreach (MeshRenderer renderHen in Prefab.transform.Find("Hen Hen/root").GetComponentsInChildren<MeshRenderer>())
            {
                if (renderHen.sharedMaterial.name.Equals("HenHen"))
                {
                    renderHen.sharedMaterial = HenMat;
                }
            }
            //Prefab.transform.Find("Hen Hen/root").gameObject.AddComponent<ParticleSystem>().GetCopyOf(Main.assetBundle.LoadAsset<GameObject>("ElectricSparklesChickens").GetComponent<ParticleSystem>());
            /*Transform sparkles = Main.assetBundle.LoadAsset<GameObject>("ElectricSparklesChickens").transform;
            sparkles.parent = Prefab.transform;
            sparkles.position = new Vector3(0, 0.5F, 0);*/

            Prefab.name = "electricHen";
            Prefab.GetComponent<Identifiable>().id = Ids.ELECTRIC_HEN;

            return Prefab;
        }

        unsafe public static GameObject CreateElectricRoosterPrefab()
        {
            GameObject Prefab = PrefabUtils.CopyPrefab(SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.ROOSTER));
            /*Console.Log("                                                           YOOOO");
            Main.assetBundle.GetAllAssetNames().PrintContent();*/

            SkinnedMeshRenderer mRender = Prefab.transform.Find("Roostro/mesh_body1").gameObject.GetComponent<SkinnedMeshRenderer>();
            Material HenMat = UnityEngine.Object.Instantiate<Material>(mRender.sharedMaterial);
            HenMat.SetTexture("_RampGreen", ElementalElectricTree.Main.assetBundle.LoadAsset<Texture>("ramp_meat_yellow"));
            //HenMat.SetTexture("_RampRed", Main.assetBundle.LoadAsset<Texture>("ramp_meat_white"));
            //HenMat.SetTexture("_RampBlue", Main.assetBundle.LoadAsset<Texture>("ramp_meat_yellow"));
            //HenMat.SetTexture("_RampBlack", Main.assetBundle.LoadAsset<Texture>("ramp_meat_white"));
            mRender.sharedMaterial = HenMat;
            foreach (MeshRenderer renderHen in Prefab.transform.Find("Roostro/root").GetComponentsInChildren<MeshRenderer>())
            {
                if (renderHen.sharedMaterial.name.Equals("Roostro"))
                {
                    renderHen.sharedMaterial = HenMat;
                }
            }
            //Prefab.transform.Find("Roostro/root").gameObject.AddComponent<ParticleSystem>().GetCopyOf(Main.assetBundle.LoadAsset<GameObject>("ElectricSparklesChickens").GetComponent<ParticleSystem>());

            /*Transform sparkles = Main.assetBundle.LoadAsset<GameObject>("ElectricSparklesChickens").transform;
            sparkles.parent = Prefab.transform;
            sparkles.position = new Vector3(0,0.5F,0);*/

            /*try
            {
                Main.assetBundle.LoadAsset<GameObject>("birdHenElectric");
                Console.Log("BirdHen Electric is there");
            }
            catch (System.Exception e)
            {

                Console.Log(e.Message);
            }

            try
            {
                Main.assetBundle.LoadAsset<GameObject>("birdHenElectric").transform.Find;
                Console.Log("BirdHen Electric is there");
            }
            catch (System.Exception e)
            {

                Console.Log(e.Message);
            }*/

            Prefab.name = "electricRooster";
            Prefab.GetComponent<Identifiable>().id = Ids.ELECTRIC_ROOSTER;

            return Prefab;
        }


        unsafe public static GameObject CreateFruitVeggie(string name, Identifiable.Id id)
        {
            GameObject Prefab = PrefabUtils.CopyPrefab(SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.OCAOCA_VEGGIE)); //It can be everything, as long this is a fruit/vegetable
            GameObject AssetFood = ElementalElectricTree.Main.assetBundle.LoadAsset<GameObject>("NAME OF OBJECT HERE"); //Your mesh located into the assetbundle
            Prefab.name = name;

            Prefab.GetComponent<Identifiable>().id = id;
            GameObject MeshPart = Prefab.FindChildWithPartialName("model_");
            MeshPart.GetComponent<MeshFilter>().sharedMesh = AssetFood.GetComponentInChildren<MeshFilter>().sharedMesh;
            MeshPart.GetComponent<MeshRenderer>().sharedMaterials = AssetFood.GetComponentInChildren<MeshRenderer>().sharedMaterials;

            return Prefab;
        }
    }
}
