using UnityEngine;
using ElementalElectricTree;
using ElementalElectricTree.Other;
using SRML.Utils;
using System.Linq;

namespace Creators
{
    class ElectricSlimeForm2Prefab
    {
        public static (SlimeDefinition, GameObject) GetPrefab(bool autoRegister = true)
        {
            (SlimeDefinition, GameObject) SlimeTuple = Custom_Slime_Creator.CreateSlime(
                Ids.FORM_2_ELECTRIC_SLIME,
                "electricSlimeForm2",
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
                Main.assetBundle.LoadAsset<Sprite>("PlasmaSlime"),
                new SlimeAppearance.Palette
                {
                    Top = Color.yellow,
                    Middle = Color.yellow,
                    Bottom = Color.yellow
                },
                Color.yellow,
                Identifiable.Id.PINK_SLIME,
                autoRegister);

            SlimeDefinition slimeDefinition = SlimeTuple.Item1;
            GameObject slimeObject = SlimeTuple.Item2;
            SlimeAppearance slimeAppearance = slimeObject.GetComponent<SlimeAppearanceApplicator>().Appearance;

            SlimeAppearanceStructure[] structures = slimeAppearance.Structures;
            
            Material material = Object.Instantiate(SRSingleton<GameContext>.Instance.SlimeDefinitions.GetSlimeByIdentifiableId(Identifiable.Id.BOOM_SLIME).AppearancesDefault[0].Structures[0].DefaultMaterials[0]);

            foreach (SlimeAppearanceStructure slimeAppearanceStructure in structures)
            {
                Material[] defaultMaterials = slimeAppearanceStructure.DefaultMaterials;
                if (defaultMaterials != null && defaultMaterials.Length != 0)
                {
                    material.SetColor("_TopColor", Color.yellow);
                    material.SetColor("_MiddleColor", Color.yellow);
                    material.SetColor("_BottomColor", Color.yellow);
                    //material.SetColor("_SpecColor", Color.yellow);

                    material.SetFloat(Shader.PropertyToID("_Gloss"), 3.5F);
                    material.SetFloat(Shader.PropertyToID("_GlossPower"), 6F);
                    material.SetFloat(Shader.PropertyToID("_Shininess"), 2F);
                    material.SetFloat(Shader.PropertyToID("_CrackAmount"), 0);
                    material.SetFloat(Shader.PropertyToID("_Char"), 0);

                    slimeAppearanceStructure.DefaultMaterials[0] = material;
                }

            }

            slimeAppearance.Face.OnEnable();

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
            /*LOD0.gameObject.transform.localPosition = new Vector3(0f, 1, 0.3f);
            var transformLocalRotation = LOD0.transform.localRotation;
            transformLocalRotation.eulerAngles = new Vector3(30f, 0, 0);
            LOD0.transform.localRotation = transformLocalRotation;*/

            slimeAppearanceObjects[0] = LOD0.GetComponent<SlimeAppearanceObject>();

            GameObject LOD1 = PrefabUtils.CopyPrefab(slimeAppearanceObjects[1].gameObject);
            LOD1.GetComponent<MeshFilter>().sharedMesh = customCrest;
            /*LOD1.gameObject.transform.localPosition = new Vector3(0f, 1, 0.3f);
            var transformLocalRotation1 = LOD1.transform.localRotation;
            transformLocalRotation1.eulerAngles = new Vector3(30f, 0, 0);
            LOD1.transform.localRotation = transformLocalRotation;*/

            slimeAppearanceObjects[1] = LOD1.GetComponent<SlimeAppearanceObject>();

            ShortCutter.Log("Pls work");

            SlimeAppearanceElement slimeThunderCrest = ScriptableObject.CreateInstance<SlimeAppearanceElement>();
            slimeThunderCrest.Name = "slimeThunderCrest";
            slimeThunderCrest.Prefabs = slimeAppearanceObjects;
            slimeAppearance.Structures[1].Element = slimeThunderCrest;

            ShortCutter.Log("and now?");

            SlimeDefinition quickSilverDefinitionToUse = SRSingleton<GameContext>.Instance.SlimeDefinitions.GetSlimeByIdentifiableId(Identifiable.Id.QUICKSILVER_SLIME);
            SlimeDefinition quickSilverDefinition = (SlimeDefinition)PrefabUtils.DeepCopyObject(quickSilverDefinitionToUse);

            SlimeAppearance slimeShocked = Object.Instantiate(quickSilverDefinition.AppearancesDefault[0].ShockedAppearance);

            slimeShocked.Structures = new SlimeAppearanceStructure[]
            {
                slimeAppearance.Structures[0], slimeAppearance.Structures[1]
            };

            slimeShocked.Structures[0].DefaultMaterials[0] = quickSilverDefinition.AppearancesDefault[0].ShockedAppearance.Structures[0].DefaultMaterials[0];

            slimeAppearance.ShockedAppearance = slimeShocked;

            #endregion


            #region Components
            //slimeObject.AddComponent<ChangeParticlesNormal>();
            slimeObject.AddComponent<ChangeParticlesAngry>();
            
            //slimeObject.AddComponent<ElectricCannonBullet>();
            slimeObject.AddComponent<ShockOnTouch>();
            slimeObject.AddComponent<DamagePlayerOnTouch>().GetCopyOf(SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.ROCK_SLIME).GetComponentInChildren<DamagePlayerOnTouch>());

            slimeObject.GetComponent<DamagePlayerOnTouch>().damagePerTouch = 70;

            slimeObject.GetComponent<SlimeRandomMove>().scootSpeedFactor *= 6;
            slimeObject.GetComponent<SlimeRandomMove>().verticalFactor *= 3;

            Object.Destroy(slimeObject.GetComponent(typeof(PinkSlimeFoodTypeTracker)));

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

            return (slimeDefinition, slimeObject);
        }

    }
}
