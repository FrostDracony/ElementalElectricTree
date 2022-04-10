using SRML.Utils;
using UnityEngine;
using Console = SRML.Console.Console;
using ElementalElectricTree;
using ElementalElectricTree.Other;
using System.Collections.Generic;
using System.Linq;

namespace Creators
{
    class ElectricSlimeForm1Prefab
    {
        static void Log(object message) => Console.Log(message.ToString());

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
            Log("Printing slimeAppearance");

            int appearanceInt = 0;
            foreach (SlimeAppearance appearance in slimeDefinition.AppearancesDefault)
            {
                Log("Appearance " + appearance + " found in position " + appearanceInt);
                appearanceInt++;
            }
            slimeObject.GetComponent<SlimeAppearanceApplicator>().Appearance = (SlimeAppearance)PrefabUtils.DeepCopyObject(slimeDefinition.AppearancesDefault[0]);
            SlimeAppearanceApplicator slimeAppearanceApplicator = slimeObject.GetComponent<SlimeAppearanceApplicator>();

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

                    material.SetColor(Shader.PropertyToID("_TopColor"), new Color(0.541F, 0.667F, 0.663F, 1.000F));
                    material.SetColor(Shader.PropertyToID("_MiddleColor"), new Color(0.384F, 0.420F, 0.439F, 1.000F));
                    material.SetColor(Shader.PropertyToID("_BottomColor"), new Color(0.180F, 0.149F, 0.208F, 1.000F));

                    slimeAppearanceStructure.DefaultMaterials[0] = material;
                }
            }

            #region CustomModel
            slimeAppearance.Structures = new SlimeAppearanceStructure[2]
            {
            new SlimeAppearanceStructure(slimeAppearance.Structures[0]),
            new SlimeAppearanceStructure(SRSingleton<GameContext>.Instance.SlimeDefinitions.GetSlimeByIdentifiableId(Identifiable.Id.LUCKY_SLIME).GetAppearanceForSet(SlimeAppearance.AppearanceSaveSet.CLASSIC).Structures[2]),
            };

            Mesh customCrest = Main.newAssetBundle.LoadAsset<GameObject>("thundercrest").FindChild("default").GetComponent<MeshFilter>().sharedMesh;

            SlimeAppearanceObject[] slimeAppearanceObjects = slimeAppearance.Structures[1].Element.Prefabs.ToArray();

            GameObject LOD0 = PrefabUtils.CopyPrefab(slimeAppearanceObjects[0].gameObject);
            LOD0.GetComponent<MeshFilter>().sharedMesh = customCrest;
            LOD0.gameObject.transform.localPosition = new Vector3(0f, 1, 0.3f);
            var transformLocalRotation = LOD0.transform.localRotation;
            transformLocalRotation.eulerAngles = new Vector3(30f, 0, 0);
            LOD0.transform.localRotation = transformLocalRotation;

            slimeAppearanceObjects[0] = LOD0.GetComponent<SlimeAppearanceObject>();

            GameObject LOD1 = PrefabUtils.CopyPrefab(slimeAppearanceObjects[1].gameObject);
            LOD1.GetComponent<MeshFilter>().sharedMesh = customCrest;
            LOD1.gameObject.transform.localPosition = new Vector3(0f, 1, 0.3f);
            var transformLocalRotation1 = LOD1.transform.localRotation;
            transformLocalRotation1.eulerAngles = new Vector3(30f, 0, 0);
            LOD1.transform.localRotation = transformLocalRotation;

            slimeAppearanceObjects[1] = LOD1.GetComponent<SlimeAppearanceObject>();

            SlimeAppearanceElement slimeThunderCrest = ScriptableObject.CreateInstance<SlimeAppearanceElement>();
            slimeThunderCrest.Name = "slimeThunderCrest";
            slimeThunderCrest.Prefabs = slimeAppearanceObjects;
            slimeAppearance.Structures[1].Element = slimeThunderCrest;

            SlimeAppearance slimeShocked = Object.Instantiate(slimeDefinition.AppearancesDefault[0].ShockedAppearance);
            slimeShocked.Structures = new SlimeAppearanceStructure[]
            {
                slimeAppearance.Structures[0], slimeAppearance.Structures[1]
            };

            slimeShocked.Structures[0].DefaultMaterials[0] = slimeDefinition.AppearancesDefault[0].ShockedAppearance.Structures[0].DefaultMaterials[0];

            slimeAppearance.ShockedAppearance = slimeShocked;

            #endregion

            #region Components
            //slimeObject.AddComponent<ChangeParticlesNormal>();
            slimeObject.AddComponent<ChangeParticlesAngry>();

            //slimeObject.AddComponent<ElectricArrow>();
            slimeObject.AddComponent<ShockOnTouch>();
            //slimeObject.AddComponent<ShootWhenAgitated>();

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

            #endregion

            #region SecretStyle
            SlimeAppearance secretAppearance = (SlimeAppearance)PrefabUtils.DeepCopyObject(slimeAppearance);

            //ShortCutter.RegisterSecretStyle(Ids.ELECTRIC_SLIME, secretAppearance, slimeDefinition, "Black Lightning");

            #endregion

            return (slimeDefinition, slimeObject);
        }

    }
}
