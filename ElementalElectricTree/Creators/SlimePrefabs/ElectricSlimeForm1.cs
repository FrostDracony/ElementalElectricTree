using SRML.Utils;
using UnityEngine;
using Console = SRML.Console.Console;
using ElementalElectricTree;
using ElementalElectricTree.Other;
using System.Collections.Generic;

namespace Creators
{
    class ElectricSlimeForm1Prefab
    {
        public static (SlimeDefinition, GameObject) GetPrefab(bool autoRegister = true)
        {
            (SlimeDefinition, GameObject) SlimeTuple = Custom_Slime_Creator.CreateSlime(
                Ids.ELECTRIC_SLIME,
                "electricSlimeForm1",
                new SlimeEat.FoodGroup[2]
                {
                    SlimeEat.FoodGroup.MEAT,
                    SlimeEat.FoodGroup.FRUIT,
                },

                new Identifiable.Id[1]
                {
                    Ids.ELECTRIC_PLORT
                },

                new Identifiable.Id[2]
                {
                    Ids.ELECTRIC_HEN,
                    Ids.ELECTRIC_ROOSTER

                },
                Main.assetBundle.LoadAsset<Sprite>("ElectricSlimeSprite"),
                new SlimeAppearance.Palette
                {
                    Top = Color.yellow,
                    Middle = Color.yellow,
                    Bottom = Color.yellow
                },
                Color.yellow,
                Identifiable.Id.QUICKSILVER_SLIME,
                autoRegister);

            SlimeDefinition slimeDefinition = SlimeTuple.Item1;
            GameObject slimeObject = SlimeTuple.Item2;
            SlimeAppearance slimeAppearance = slimeObject.GetComponent<SlimeAppearanceApplicator>().Appearance;
            slimeDefinition.AppearancesDefault.PrintContent();
            int appearanceInt = 0;
            foreach(SlimeAppearance appearance in slimeDefinition.AppearancesDefault)
            {
                if(appearance.ShockedAppearance != null)
                {
                    Console.Log("Appearance number: " + appearanceInt + " has a shocked appearance");
                }
                appearanceInt++;
            }

            slimeObject.GetComponent<SlimeAppearanceApplicator>().Appearance = (SlimeAppearance)PrefabUtils.DeepCopyObject(slimeDefinition.AppearancesDefault[0]);
            SlimeAppearanceStructure[] structures = slimeAppearance.Structures;

            int i = 0; //1 = Body, 2 = crest
            foreach (SlimeAppearanceStructure slimeAppearanceStructure in structures)
            {
                Material[] defaultMaterials = slimeAppearanceStructure.DefaultMaterials;
                Material material = SRML.SRObjects.GetInst<Material>("slimeQuickSilverBase");
                Material goldMaterial = Object.Instantiate(SRSingleton<GameContext>.Instance.SlimeDefinitions.GetSlimeByIdentifiableId(Identifiable.Id.GOLD_SLIME).AppearancesDefault[0].Structures[0].DefaultMaterials[0]);

                Console.Log("Printing thing: " + i);

                if (i == 0)
                {

                    material.SetTexture(Shader.PropertyToID("_CubemapOverride"), Main.assetBundle.LoadAsset<Texture>("electric_sphere_1")); //Main.assetBundle.LoadAsset<Texture>("electric_sphere_1")
                    material.SetFloat(Shader.PropertyToID("_Gloss"), 3.5F);
                    material.SetFloat(Shader.PropertyToID("_GlossPower"), 6F);
                    material.SetFloat(Shader.PropertyToID("_Shininess"), 2F);

                    Console.Log(material.GetColor(Shader.PropertyToID("_TopColor")).ToString());
                    Console.Log(material.GetColor(Shader.PropertyToID("_MiddleColor")).ToString());
                    Console.Log(material.GetColor(Shader.PropertyToID("_BottomColor")).ToString());
                    Console.Log("");

                    material.SetColor(Shader.PropertyToID("_TopColor"), new Color(0.541F, 0.667F, 0.663F, 1.000F));
                    material.SetColor(Shader.PropertyToID("_MiddleColor"), new Color(0.384F, 0.420F, 0.439F, 1.000F));
                    material.SetColor(Shader.PropertyToID("_BottomColor"), new Color(0.180F, 0.149F, 0.208F, 1.000F));

                    material.shader.PrintContent();
                    slimeAppearanceStructure.DefaultMaterials[0] = material;
                }
                else
                {
                    int i2 = 0;
                    int num = slimeAppearance.Structures[1].Element.Prefabs.Length;
                    for (int j = 0; j < num; j++)
                    {
                        Console.Log("Creating the custommesh, loop number: " + j);
                        SlimeAppearanceObject prefab = slimeAppearance.Structures[1].Element.Prefabs[j];
                        SlimeAppearanceObject component = PrefabUtils.CopyPrefab(prefab.gameObject).GetComponent<SlimeAppearanceObject>();

                        var returnvalue = prefab.gameObject.PrintParent();
                        if (returnvalue != null)
                            returnvalue.PrintParent();

                        if (component.TryGetComponent<MeshFilter>(out MeshFilter filter))
                        {
                            filter.sharedMesh = Main.assetBundle.LoadAsset<GameObject>("BoldCrest").GetComponentInChildren<MeshFilter>(true).sharedMesh;
                        }

                        if (component.TryGetComponent<SkinnedMeshRenderer>(out SkinnedMeshRenderer skinned))
                        {
                            skinned.sharedMesh = Main.assetBundle.LoadAsset<GameObject>("BoldCrest").GetComponentInChildren<MeshFilter>(true).sharedMesh;
                        }

                        slimeAppearance.Structures[1].Element.Prefabs[i2] = component;

                        i2++;
                    }

                    slimeAppearanceStructure.DefaultMaterials[0] = goldMaterial;
                }

                i++;

            }

            //slimeObject.AddComponent<ChangeParticlesNormal>();
            slimeObject.AddComponent<ChangeParticlesAngry>();

            //slimeObject.AddComponent<ElectricArrow>();
            slimeObject.AddComponent<ShockOnTouch>();
            //slimeObject.AddComponent<ShootWhenAgitated>();
            slimeObject.AddComponent<LODSetter>();

            slimeObject.GetComponent<DamagePlayerOnTouch>().damagePerTouch = 70;
            slimeObject.GetComponent<PuddleSlimeScoot>().straightlineForceFactor *= 4;

            slimeObject.AddComponent<SlimeRandomMove>().GetCopyOf(SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.PINK_SLIME).GetComponentInChildren<SlimeRandomMove>());

            Object.Destroy(slimeObject.GetComponent(typeof(SlimeFlee)));
            Object.Destroy(slimeObject.GetComponent(typeof(FollowWaypoints)));
            Object.Destroy(slimeObject.GetComponent(typeof(ReactToShock)));

            GameObject particles = Main.assetBundle.LoadAsset<GameObject>("ElectricSparklesSlimes");
            particles.transform.position = new Vector3(0, 0.5F, 0);

            if (Shader.Find("SR/Particles/Additive") != null)
            {
                foreach (ParticleSystemRenderer particleSystemRenderer in particles.GetComponentsInChildren<ParticleSystemRenderer>())
                {
                    particleSystemRenderer.material.shader = Shader.Find("SR/Particles/Additive");
                }
            }

            Object.Instantiate(particles, slimeObject.transform);

            SlimeAppearance secretAppearance = (SlimeAppearance)PrefabUtils.DeepCopyObject(slimeAppearance);
            secretAppearance.NameXlateKey = "l.secret_style_electric_slime";

            secretAppearance.Structures[0].DefaultMaterials[0].SetTexture(Shader.PropertyToID("_CubemapOverride"), SRML.SRObjects.GetInst<Material>("slimeQuickSilverBase").GetTexture(Shader.PropertyToID("_CubemapOverride")));

            Dictionary<SlimeDefinition, SlimeAppearance> newDict = new Dictionary<SlimeDefinition, SlimeAppearance>
            {
                { slimeDefinition, secretAppearance }
            };

            Main.SecretStyleList.Add(Ids.ELECTRIC_SLIME, newDict);

            GameContext.Instance.DLCDirector.onPackageInstalled += (s =>
            {
                if (s != DLCPackage.Id.SECRET_STYLE)
                    return;

                slimeDefinition.RegisterDynamicAppearance(secretAppearance);
                ///SceneContext.Instance.SlimeAppearanceDirector.UnlockAppearance(slimeDefinition, secretAppearance);
            });

            return (slimeDefinition, slimeObject);
        }

    }
}
