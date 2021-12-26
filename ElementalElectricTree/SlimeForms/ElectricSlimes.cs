namespace ElementalElectricTree.SlimeForms
{
    class ElectricSlimes
    {
/*        public static void AddAllSlimeForms(GameObject slimeObject)
        {

            if (slimeObject.GetComponent<switchSlimeForm>() == null)
                slimeObject.AddComponent<switchSlimeForm>();

            slimeObject.GetComponent<switchSlimeForm>().idOfSlime = slimeObject.GetComponent<Identifiable>().id;
            
        }

        public static void AddAllSlimeForms(GameObject slimeObject, Identifiable.Id id)
        {

            if (slimeObject.GetComponent<switchSlimeForm>() == null)
                slimeObject.AddComponent<switchSlimeForm>();

            slimeObject.GetComponent<switchSlimeForm>().idOfSlime = id;

        }*/

/*        public static (SlimeDefinition, GameObject) GetForm2()
        {
            (SlimeDefinition, GameObject) SlimeTuple = Custom_Slime_Creator.CreateSlime(
                Ids.FORM_2_ELECTRIC_SLIME,
                "electricSlimeForm2",
                new SlimeEat.FoodGroup[1]
                {
                    SlimeEat.FoodGroup.FRUIT
                },

                new Identifiable.Id[1]
                {
                    Identifiable.Id.NONE
                },
                new Identifiable.Id[1]
                {
                    Identifiable.Id.NONE
                },
                Main.assetBundle.LoadAsset<Sprite>("PlasmaSlime"),
                new SlimeAppearance.Palette
                {
                    Top = Color.magenta,
                    Middle = Color.magenta,
                    Bottom = Color.magenta
                },
                Color.magenta,
                Identifiable.Id.PINK_SLIME,
                true);

            SlimeDefinition slimeDefinition = SlimeTuple.Item1;
            GameObject slimeObject = SlimeTuple.Item2;
            SlimeAppearance slimeAppearance = slimeObject.GetComponent<SlimeAppearanceApplicator>().Appearance;

            SlimeAppearanceStructure[] structures = slimeAppearance.Structures;
            Material material = UnityEngine.Object.Instantiate(SRSingleton<GameContext>.Instance.SlimeDefinitions.GetSlimeByIdentifiableId(Identifiable.Id.PINK_SLIME).AppearancesDefault[0].Structures[0].DefaultMaterials[0]);

            material.shader.PrintContent();

            foreach (SlimeAppearanceStructure slimeAppearanceStructure in structures)
            {
                Material[] defaultMaterials = slimeAppearanceStructure.DefaultMaterials;
                if (defaultMaterials != null && defaultMaterials.Length != 0)
                {
                    material.SetColor("_TopColor", Color.yellow);
                    material.SetColor("_MiddleColor", Color.yellow);
                    material.SetColor("_BottomColor", Color.yellow);
                    material.SetColor("_SpecColor", Color.yellow);

                    material.SetFloat(Shader.PropertyToID("_Gloss"), 3.5F);
                    material.SetFloat(Shader.PropertyToID("_GlossPower"), 6F);
                    material.SetFloat(Shader.PropertyToID("_Shininess"), 2F);

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
                    slimeExpressionFace.Eyes.SetColor("_EyeRed", new Color32(205, 190, 255, 255));
                    slimeExpressionFace.Eyes.SetColor("_EyeGreen", new Color32(182, 170, 226, 255));
                    slimeExpressionFace.Eyes.SetColor("_EyeBlue", new Color32(182, 170, 205, 255));
                }
            }
            slimeAppearance.Face.OnEnable();


            slimeObject.AddComponent<ShockOnTouch>();
            slimeObject.AddComponent<ShootWhenAgitated>();
            slimeObject.AddComponent<DamagePlayerOnTouch>().GetCopyOf(SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.ROCK_SLIME).GetComponentInChildren<DamagePlayerOnTouch>());

            slimeObject.GetComponent<DamagePlayerOnTouch>().damagePerTouch = 140;

            slimeObject.GetComponent<SlimeRandomMove>().scootSpeedFactor *= 6;
            slimeObject.GetComponent<SlimeRandomMove>().verticalFactor *= 3;

            GameObject.Destroy(slimeObject.GetComponent(typeof(PinkSlimeFoodTypeTracker)));


            Console.Log("Before the AssetBundle");
            GameObject particles = Main.assetBundle.LoadAsset<GameObject>("MagicCircleVFX");
            Console.Log("After the AssetBundle. Particles are: " + particles);
            particles.transform.position = new Vector3(0, 0.25F, 0);

            if (Shader.Find("SR/Particles/Additive") != null)
            {
                foreach (ParticleSystemRenderer particleSystemRenderer in particles.GetComponentsInChildren<ParticleSystemRenderer>())
                {
                    particleSystemRenderer.material.shader = Shader.Find("SR/Particles/Additive");
                }
            }

            UnityEngine.Object.Instantiate<GameObject>(particles, slimeObject.transform);

            return (slimeDefinition, slimeObject);
        }
*/
        /*public static GameObject GetForm1()
        {
            (SlimeDefinition, GameObject) SlimeTuple = Custom_Slime_Creator.CreateSlime(
                Ids.ELECTRIC_SLIME,
                "electricSlime",
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
                false);

            SlimeDefinition slimeDefinition = SlimeTuple.Item1;
            GameObject slimeObject = SlimeTuple.Item2;
            SlimeAppearance slimeAppearance = slimeObject.GetComponent<SlimeAppearanceApplicator>().Appearance;

            Material Extramaterial = UnityEngine.Object.Instantiate(SRSingleton<SceneContext>.Instance.SlimeAppearanceDirector.GetChosenSlimeAppearance(Identifiable.Id.QUICKSILVER_SLIME).ShockedAppearance.Structures[0].DefaultMaterials[0]);
            slimeObject.GetComponent<SlimeAppearanceApplicator>().Appearance = (SlimeAppearance)PrefabUtils.DeepCopyObject(slimeDefinition.AppearancesDefault[0]);
            SlimeAppearanceStructure[] structures = slimeAppearance.Structures;

            int i = 0; //1 = Body, 2 = crest

            foreach (SlimeAppearanceStructure slimeAppearanceStructure in structures)
            {
                Material[] defaultMaterials = slimeAppearanceStructure.DefaultMaterials;

                SRSingleton<GameContext>.Instance.SlimeShaders.cloakableShaders.ToArray().PrintContent<Shader>();
                Console.Log(SRSingleton<GameContext>.Instance.SlimeShaders.cloakMaterial.ToString());
                SRSingleton<GameContext>.Instance.SlimeShaders.defaultCloakableMaterials.PrintContent<Material>();
                Material material = UnityEngine.Object.Instantiate(SRSingleton<GameContext>.Instance.SlimeDefinitions.GetSlimeByIdentifiableId(Identifiable.Id.GOLD_SLIME).AppearancesDefault[0].Structures[0].DefaultMaterials[0]);

                Console.Log("Printing thing: " + i);
                Console.Log("Extramaterial is: " + (Extramaterial != null));
                if (Extramaterial != null)
                    Console.Log(Extramaterial.GetTexture("_CubemapOverride").ToString());

                if (i == 0)
                {

                    material.SetTexture(Shader.PropertyToID("_CubemapOverride"), Main.assetBundle.LoadAsset<Texture>("electric_sphere_1")); //Main.assetBundle.LoadAsset<Texture>("electric_sphere_1")
                    material.SetFloat(Shader.PropertyToID("_Gloss"), 3.5F);
                    material.SetFloat(Shader.PropertyToID("_GlossPower"), 6F);
                    material.SetFloat(Shader.PropertyToID("_Shininess"), 2F);

                    material.shader.PrintContent();
                    slimeAppearanceStructure.DefaultMaterials[0] = material;
                }
                else
                {

                    int i2 = 0;
                    int num = slimeAppearance.Structures[1].Element.Prefabs.Length;
                    for (int j = 0; j < num; j++)
                    {
                        SlimeAppearanceObject prefab = slimeAppearance.Structures[1].Element.Prefabs[j];
                        SlimeAppearanceObject component = PrefabUtils.CopyPrefab(prefab.gameObject).GetComponent<SlimeAppearanceObject>();

                        var returnvalue = prefab.gameObject.PrintParent();
                        if (returnvalue != null)
                            returnvalue.PrintParent();
                        Console.Log("");
                        Console.Log("");
                        Console.Log("");
                        Console.Log("");
                        Console.Log("               Prefab in custom mesh is: " + prefab.name);
                        prefab.PrintComponents<Component>();
                        Console.Log("");
                        Console.Log("");
                        Console.Log("");
                        Console.Log("");
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

                    slimeAppearanceStructure.DefaultMaterials[0] = material;
                }

                i++;

            }

            slimeObject.AddComponent<ShockOnTouch>();
            slimeObject.AddComponent<ShootWhenAgitated>();

            slimeObject.GetComponent<DamagePlayerOnTouch>().damagePerTouch = 70;
            slimeObject.GetComponent<PuddleSlimeScoot>().straightlineForceFactor *= 4;

            slimeObject.AddComponent<SlimeRandomMove>().GetCopyOf(SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.PINK_SLIME).GetComponentInChildren<SlimeRandomMove>());

            GameObject.Destroy(slimeObject.GetComponent(typeof(SlimeFlee)));
            GameObject.Destroy(slimeObject.GetComponent(typeof(FollowWaypoints)));
            GameObject.Destroy(slimeObject.GetComponent(typeof(ReactToShock)));

            void onRegionsChanged(List<Region> exited, List<Region> joined)
            {
                Console.Log("Regions changed");
                foreach (Region region in joined)
                {
                    if (region.setId == RegionRegistry.RegionSetId.VALLEY)
                    {
                        slimeObject.GetComponent<switchSlimeForm>().switchToForm1();
                    }
                    else
                    {
                        slimeObject.GetComponent<switchSlimeForm>().switchToForm2();
                    }
                }
            }

            slimeObject.GetComponent<RegionMember>().regionsChanged += onRegionsChanged;

            Console.Log("Before the AssetBundle");
            GameObject particles = Main.assetBundle.LoadAsset<GameObject>("ElectricSparklesSlimes");
            Console.Log("After the AssetBundle. Particles are: " + particles);
            particles.transform.position = new Vector3(0, 0.5F, 0);

            if (Shader.Find("SR/Particles/Additive") != null)
            {
                foreach (ParticleSystemRenderer particleSystemRenderer in particles.GetComponentsInChildren<ParticleSystemRenderer>())
                {
                    particleSystemRenderer.material.shader = Shader.Find("SR/Particles/Additive");
                }
            }

            UnityEngine.Object.Instantiate<GameObject>(particles, slimeObject.transform);


            return slimeObject;
        }
*/
    }
}
