using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SRML.Utils;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;
using Console = SRML.Console.Console;
using ElementalElectricTree;
using ElementalElectricTree.Other;

namespace Creators
{
    class Custom_Slime_Creator
    {
        unsafe public static void CreateNewElectricSlime(ReactToShock reactToShock1, ReactToShock reactToShock2)
        {
            GameObject ElectricSlime = SRBehaviour.InstantiateActor(SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Ids.ELECTRIC_SLIME), reactToShock1.regionMember.setId, reactToShock1.transform.position, reactToShock1.transform.rotation);
            MeshFilter ElectricParticles = GameObject.Instantiate<MeshFilter>(SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.VALLEY_AMMO_1).GetComponentsInChildren<MeshFilter>(true).First((MeshFilter x) => x.name == "Core Sphere")); //SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.VALLEY_AMMO_1).GetComponentInChildren<DestroyAndShockOnTouching>().destroyFX;
            GameObject ElectricSounds = SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.VALLEY_AMMO_1).GetComponentInChildren<DestroyAndShockOnTouching>().destroyFX;

            SRBehaviour.SpawnAndPlayFX(ElectricSounds, ElectricSlime, ElectricSlime.transform.position, ElectricSlime.transform.rotation);

            Vector3 targetPosition = SRSingleton<SceneContext>.Instance.Player.transform.position + new Vector3(0, 0.5F, 0);
            Abilities.CreateShoot(reactToShock1.gameObject.transform.position + new Vector3(0, 3, 0), targetPosition - (reactToShock1.gameObject.transform.position + new Vector3(0, 3, 0)), reactToShock1.regionMember.setId);

            //ElectricSlime.AddComponent<ShockOnTouch>();

            GameObject.Destroy(reactToShock1.gameObject);
            GameObject.Destroy(reactToShock2.gameObject);

        }

        unsafe public static void CreateElectricSlime(ReactToShock reactToShock)
        {
            GameObject ElectricSlime = SRBehaviour.InstantiateActor(SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Ids.ELECTRIC_SLIME), reactToShock.regionMember.setId, reactToShock.transform.position, reactToShock.transform.rotation);
            MeshFilter ElectricParticles = GameObject.Instantiate<MeshFilter>(SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.VALLEY_AMMO_1).GetComponentsInChildren<MeshFilter>(true).First((MeshFilter x) => x.name == "Core Sphere")); //SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.VALLEY_AMMO_1).GetComponentInChildren<DestroyAndShockOnTouching>().destroyFX;
            GameObject ElectricSounds = SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.VALLEY_AMMO_1).GetComponentInChildren<DestroyAndShockOnTouching>().destroyFX;

            SRBehaviour.SpawnAndPlayFX(ElectricSounds, ElectricSlime, ElectricSlime.transform.position, ElectricSlime.transform.rotation);
            //ElectricParticles.gameObject.transform.parent = ElectricSlime.transform;
            Vector3 targetPosition = SRSingleton<SceneContext>.Instance.Player.transform.position + new Vector3(0, 0.5F, 0);
            Abilities.CreateShoot(reactToShock.gameObject.transform.position + new Vector3(0, 3, 0), targetPosition - (reactToShock.gameObject.transform.position + new Vector3(0, 3, 0)), reactToShock.regionMember.setId);

            //ElectricSlime.AddComponent<ShockOnTouch>();

            //ElectricSlime.AddComponent<ShockOnTouch>();
            

            GameObject.Destroy(reactToShock.gameObject);
        }

        unsafe public static (SlimeDefinition, GameObject) CreateSlime(Identifiable.Id SlimeId, string SlimeName)
        {
            SlimeDefinition quicksilverSlimeDefinition = SRSingleton<GameContext>.Instance.SlimeDefinitions.GetSlimeByIdentifiableId(Identifiable.Id.QUICKSILVER_SLIME);
            SlimeDefinition goldSlimeDefinition = SRSingleton<GameContext>.Instance.SlimeDefinitions.GetSlimeByIdentifiableId(Identifiable.Id.GOLD_SLIME);
            
            SlimeDefinition slimeDefinition = (SlimeDefinition)PrefabUtils.DeepCopyObject(quicksilverSlimeDefinition);
            slimeDefinition.AppearancesDefault = new SlimeAppearance[1];
            slimeDefinition.Diet.Produces = new Identifiable.Id[1]
            {
                Identifiable.Id.NONE
            };

            slimeDefinition.Diet.MajorFoodGroups = new SlimeEat.FoodGroup[2]
            {
                SlimeEat.FoodGroup.MEAT,
                SlimeEat.FoodGroup.FRUIT,
            };

            slimeDefinition.Diet.AdditionalFoods = new Identifiable.Id[0];

            slimeDefinition.Diet.Favorites = new Identifiable.Id[1]
            {
                Identifiable.Id.NONE
            };

            slimeDefinition.Diet.EatMap?.Clear();

            slimeDefinition.CanLargofy = false;
            slimeDefinition.FavoriteToys = new Identifiable.Id[0];
            slimeDefinition.Name = SlimeName;
            slimeDefinition.IdentifiableId = SlimeId;

            GameObject slimeObject = PrefabUtils.CopyPrefab(SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.QUICKSILVER_SLIME));
            slimeObject.name = SlimeName;
            slimeObject.GetComponent<PlayWithToys>().slimeDefinition = slimeDefinition;
            slimeObject.GetComponent<SlimeAppearanceApplicator>().SlimeDefinition = slimeDefinition;
            slimeObject.GetComponent<SlimeEat>().slimeDefinition = slimeDefinition;
            slimeObject.GetComponent<Identifiable>().id = SlimeId;
            //UnityEngine.Object.Destroy(slimeObject.GetComponent<PinkSlimeFoodTypeTracker>());

            //Console.Log("starting printing hierarchy");

            /*foreach (Component component in SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.VALLEY_AMMO_1).GetComponentsInChildren<Component>(true))
            {
                Console.Log("   " + component.ToString());
            }*/



            //Console.Log("");
            //Console.Log("");

            //SlimeAppearance slimeAppearance = slimeDefinition.AppearancesDefault[0];
            SlimeAppearance slimeAppearance = (SlimeAppearance)PrefabUtils.DeepCopyObject(quicksilverSlimeDefinition.AppearancesDefault[0]);
            //SlimeAppearance slimeAppearance = UnityEngine.Object.Instantiate(quicksilverSlimeDefinition.AppearancesDefault[0]);
            slimeDefinition.AppearancesDefault[0] = slimeAppearance;
            SlimeAppearanceStructure[] structures = slimeAppearance.Structures;
            ////SRSingleton<GameContext>.Instance.OptionsDirector.mode;
            //SRQualitySettings.
            //Console.Log(structures.ToArray().ToString());

            

            int i = 0; //1 = Body, 2 = crest
            int i2 = 0;

            foreach (SlimeAppearanceStructure slimeAppearanceStructure in structures)
            {
                //Console.Log(slimeAppearanceStructure.ToString());
                Material[] defaultMaterials = slimeAppearanceStructure.DefaultMaterials;
                if (defaultMaterials != null && defaultMaterials.Length != 0)
                { //ElectricSlimeBody and BoldCrest
                    //Console.Log("Printing Structures stuff");
                    
                    foreach (SlimeAppearanceObject prefab in slimeAppearanceStructure.Element.Prefabs)
                    {
                        GameObject gameObject = prefab.gameObject;

                        var returnvalue = prefab.gameObject.PrintParent();
                        if (returnvalue != null)
                            returnvalue.PrintParent();

                        if (prefab.name.Contains("quickSilverCrest") == true)
                        {

                            if (gameObject.TryGetComponent<MeshFilter>(out MeshFilter filter))
                            {
                                //Console.Log("   Filter: " + filter.ToString());
                                filter.sharedMesh = Main.assetBundle.LoadAsset<GameObject>("BoldCrest").GetComponentInChildren<MeshFilter>(true).sharedMesh;
                            }

                            if (gameObject.TryGetComponent<SkinnedMeshRenderer>(out SkinnedMeshRenderer skinned))
                            {
                                //Console.Log("   Skinned: " + skinned.ToString());
                                skinned.sharedMesh = Main.assetBundle.LoadAsset<GameObject>("BoldCrest").GetComponentInChildren<MeshFilter>(true).sharedMesh;
                            }

                        }
                    }


                    /*foreach (SlimeAppearanceObject prefab in slimeAppearanceStructure.Element.Prefabs)
                    {
                        GameObject gameObject = prefab.gameObject;
                        string mesh = prefab.name.Contains("quickSilverCrest") ? "BoldCrest" : "ElectricSlimeBody"; //CrestWithBold + BoldCrest
                        prefab.IgnoreLODIndex = true;
                        Console.Log("");
                        Console.Log("");
                        Console.Log(mesh);
                        Console.Log("Logging Prefab: " + prefab.name + " with LOD of: " + prefab.LODIndex + " ignore: " + prefab.IgnoreLODIndex);
                        Console.Log("Prefab has as components: ");

                        foreach (Component component in gameObject.GetComponentsInChildren<Component>(true))
                        {
                            Console.Log("   " + component.ToString());
                        }

                        Console.Log("");
                        Console.Log("");
                        Console.Log("Get all meshes:");

                        foreach (MeshFilter component in gameObject.GetComponentsInChildren<MeshFilter>(true))
                        {
                            Console.Log("   " + component.ToString());
                        }

                        Console.Log("");
                        Console.Log("");

                        foreach (SlimeAppearanceObject component in gameObject.GetComponentsInChildren<SlimeAppearanceObject>(true))
                        {
                            Console.Log("SlimeAppearanceobject: ");
                            Console.Log("   " + component.ToString());
                            component.gameObject.PrintComponent();
                        }

                        Console.Log("");
                        Console.Log("");

                        *//*if (gameObject.TryGetComponent<MeshFilter>(out MeshFilter filter))
                        {
                            Console.Log("   Filter: " + filter.ToString());

                            if (prefab.LODIndex == 0 || prefab.LODIndex == 1)
                            {
                                filter.sharedMesh = Main.assetBundle.LoadAsset<GameObject>(mesh).GetComponentInChildren<MeshFilter>(true).sharedMesh;
                            }

                        }

                        if (gameObject.TryGetComponent<SkinnedMeshRenderer>(out SkinnedMeshRenderer skinned))
                        {
                            if (prefab.LODIndex == 0 || prefab.LODIndex == 1)
                            {
                                skinned.sharedMesh = Main.assetBundle.LoadAsset<GameObject>(mesh).GetComponentInChildren<MeshFilter>(true).sharedMesh;
                            }
                        }*//*

                        if (prefab.name.Contains("quickSilverCrest") == true || prefab.name.Contains("slime_quickSilver") == true || prefab.name.Contains("slime_default"))
                        {

                            *//*if(mesh == "BoldCrest")
                            {
                                prefab.name = "BoldCrest";
                            }
                            else
                            {
                                prefab.name = "ElectricSlimeBody";
                            }*//*

                            Console.Log("");
                            Console.Log("");
                            Console.Log("");
                            Console.Log("mesh" + mesh + " prefab name: " + prefab.name);
                            Console.Log("prefab Contains quickSilverCrest:  " + prefab.name.Contains("quickSilverCrest"));
                            Console.Log("prefab Contains slime_quickSilver:  " + prefab.name.Contains("slime_quickSilver"));

                            *//*foreach (Component component in gameObject.GetComponentsInChildren<Component>(true))
                            {
                                Console.Log("   " + component.ToString());
                            }*//*

                            if (gameObject.TryGetComponent<MeshFilter>(out MeshFilter filter))
                            {
                                Console.Log("   Filter: " + filter.ToString());

                                *//*if (prefab.LODIndex == 0 || prefab.LODIndex == 1)
                                {
                                    filter.sharedMesh = Main.assetBundle.LoadAsset<GameObject>(mesh).GetComponentInChildren<MeshFilter>(true).sharedMesh;
                                }*//*
                                filter.sharedMesh = Main.assetBundle.LoadAsset<GameObject>(mesh).GetComponentInChildren<MeshFilter>(true).sharedMesh;


                            }

                            if (gameObject.TryGetComponent<SkinnedMeshRenderer>(out SkinnedMeshRenderer skinned))
                            {
                                *//*if (prefab.LODIndex == 0 || prefab.LODIndex == 1)
                                {
                                    skinned.sharedMesh = Main.assetBundle.LoadAsset<GameObject>(mesh).GetComponentInChildren<MeshFilter>(true).sharedMesh;
                                }*//*
                                skinned.sharedMesh = Main.assetBundle.LoadAsset<GameObject>(mesh).GetComponentInChildren<MeshFilter>(true).sharedMesh;

                            }

                        }
                    }*/
                    //Console.Log("");
                    //Console.Log("");
                    //Console.Log("Finished Printing");
                }

                Material material = UnityEngine.Object.Instantiate(SRSingleton<GameContext>.Instance.SlimeDefinitions.GetSlimeByIdentifiableId(Identifiable.Id.GOLD_SLIME).AppearancesDefault[0].Structures[0].DefaultMaterials[0]);
                Material material2 = UnityEngine.Object.Instantiate(SRSingleton<GameContext>.Instance.SlimeDefinitions.GetSlimeByIdentifiableId(Identifiable.Id.BOOM_SLIME).AppearancesDefault[0].Structures[0].DefaultMaterials[0]);
                SRSingleton<GameContext>.Instance.SlimeDefinitions.GetSlimeByIdentifiableId(Identifiable.Id.BOOM_SLIME).AppearancesDefault[0].Structures[0].DefaultMaterials.PrintContent();
                SRSingleton<GameContext>.Instance.SlimeDefinitions.GetSlimeByIdentifiableId(Identifiable.Id.BOOM_SLIME).AppearancesDefault[0].Structures[0].DefaultMaterials[0].shader.PrintContent();
                Material silver_material = UnityEngine.Object.Instantiate(SRSingleton<GameContext>.Instance.SlimeDefinitions.GetSlimeByIdentifiableId(Identifiable.Id.QUICKSILVER_SLIME).AppearancesDefault[0].Structures[0].DefaultMaterials[0]);

                //Console.Log("   I = " + i);

                if (i == 0)
                {

                    //Console.Log(material.shader.name);
                    material.SetTexture(Shader.PropertyToID("_CubemapOverride"), Main.assetBundle.LoadAsset<Texture>("electric_sphere_1"));
                    material.SetFloat(Shader.PropertyToID("_Gloss"), 3.5F);
                    material.SetFloat(Shader.PropertyToID("_GlossPower"), 6F);
                    material.SetFloat(Shader.PropertyToID("_Shininess"), 2F);

                    material.shader.PrintContent();

                    slimeAppearanceStructure.DefaultMaterials[0] = material;
                }
                else
                {
                    material.SetFloat("_Shininess", 15f);
                    material.SetFloat("_Gloss", 10f);
                    slimeAppearanceStructure.DefaultMaterials[0] = material;
                }

                i++;
            }

            //Console.Log("Slime Created");
            slimeAppearance.Icon = Main.assetBundle.LoadAsset<Sprite>("ElectricSlimeSprite");

            slimeAppearance.ColorPalette = new SlimeAppearance.Palette
            {
                Top = Color.yellow,
                Middle = Color.yellow,
                Bottom = Color.yellow
            };

            slimeAppearance.ColorPalette.Ammo = Color.yellow;

            slimeObject.GetComponent<SlimeAppearanceApplicator>().Appearance = slimeAppearance;
            //Console.Log("Slime Created");
            slimeObject.AddComponent<ShockOnTouch>();
            slimeObject.AddComponent<ShootWhenAgitated>();

            slimeObject.GetComponent<DamagePlayerOnTouch>().damagePerTouch = 70;
            slimeObject.GetComponent<PuddleSlimeScoot>().straightlineForceFactor *= 300;

            slimeObject.AddComponent<SlimeRandomMove>().GetCopyOf(SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.PINK_SLIME).GetComponentInChildren<SlimeRandomMove>());

            GameObject.Destroy(slimeObject.GetComponent(typeof(SlimeFlee)));
            GameObject.Destroy(slimeObject.GetComponent(typeof(FollowWaypoints)));
            GameObject.Destroy(slimeObject.GetComponent(typeof(ReactToShock)));

            return (slimeDefinition, slimeObject);
        }
    }
}
