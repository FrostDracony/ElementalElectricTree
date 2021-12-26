using UnityEngine;
using ElementalElectricTree;
using ElementalElectricTree.Other;

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
            Material material = UnityEngine.Object.Instantiate(SRSingleton<GameContext>.Instance.SlimeDefinitions.GetSlimeByIdentifiableId(Identifiable.Id.BOOM_SLIME).AppearancesDefault[0].Structures[0].DefaultMaterials[0]);

            material.shader.PrintContent();

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

                    material.shader.PrintContent();

                    slimeAppearanceStructure.DefaultMaterials[0] = material;
                }

            }

            SlimeExpressionFace[] expressionFaces = slimeAppearance.Face.ExpressionFaces;
            for (int k = 0; k < expressionFaces.Length; k++)
            {
                SlimeExpressionFace slimeExpressionFace = expressionFaces[k];
                if ((bool)slimeExpressionFace.Mouth)
                {
                    slimeExpressionFace.Mouth.SetColor("_MouthBot", new Color32(205, 190, 255, 255));
                    slimeExpressionFace.Mouth.SetColor("_MouthMid", new Color32(182, 170, 226, 255));
                    slimeExpressionFace.Mouth.SetColor("_MouthTop", new Color32(182, 170, 205, 255));
                }
                if ((bool)slimeExpressionFace.Eyes)
                {
                    SRML.Console.Console.Log("SlimeEyes: " + slimeExpressionFace.Eyes.GetTexture("_FaceAtlas"));
                    slimeExpressionFace.Eyes.PrintContent();
                    slimeExpressionFace.Eyes.SetColor("_EyeRed", new Color32(205, 190, 255, 255));
                    slimeExpressionFace.Eyes.SetColor("_EyeGreen", new Color32(182, 170, 226, 255));
                    slimeExpressionFace.Eyes.SetColor("_EyeBlue", new Color32(182, 170, 205, 255));
                }
            }
            slimeAppearance.Face.OnEnable();

            /*List<SlimeAppearanceStructure> structureList = slimeAppearance.Structures.ToList();
            structureList.RemoveRange(1, 2);
            slimeAppearance.Structures = structureList.ToArray();*/

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

            return (slimeDefinition, slimeObject);
        }

    }
}
