using SRML.SR;
using SRML.Utils;
using UnityEngine;
using Console = SRML.Console.Console;
using ElementalElectricTree;
using ElementalElectricTree.Other;
using MonomiPark.SlimeRancher.Regions;

namespace Creators
{
    class Custom_Slime_Creator
    {
        public static void RegisterSlime((SlimeDefinition, GameObject) Tuple)
        {

            //Getting the SlimeDefinition and GameObject separated
            SlimeDefinition Slime_Slime_Definition = Tuple.Item1;
            GameObject Slime_Slime_Object = Tuple.Item2;

            //And well, registering it!

            LookupRegistry.RegisterIdentifiablePrefab(Slime_Slime_Object);
            SlimeRegistry.RegisterSlimeDefinition(Slime_Slime_Definition);

            AmmoRegistry.RegisterAmmoPrefab(PlayerState.AmmoMode.DEFAULT, Slime_Slime_Object);
            AmmoRegistry.RegisterAmmoPrefab(PlayerState.AmmoMode.NIMBLE_VALLEY, Slime_Slime_Object);

            SlimeDefinition slimeByIdentifiableId = SRSingleton<GameContext>.Instance.SlimeDefinitions.GetSlimeByIdentifiableId(Identifiable.Id.TARR_SLIME);
            slimeByIdentifiableId.Diet.EatMap.Add(new SlimeDiet.EatMapEntry
            {
                eats = Slime_Slime_Definition.IdentifiableId,
                becomesId = Identifiable.Id.NONE,
                driver = SlimeEmotions.Emotion.NONE,
                producesId = Identifiable.Id.NONE
            });

        }

        unsafe public static (SlimeDefinition, GameObject) CreatPlasmaSlimePrefab(bool autoRegister = true)
        {

            (SlimeDefinition, GameObject) SlimeTuple = CreateSlime(
                Ids.PLASMA_SLIME,
                "plasmaSlime",
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
                autoRegister);

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
                    material.SetColor("_TopColor", new Color32(190, 28, 255, 255));
                    material.SetColor("_MiddleColor", new Color32(159, 28, 255, 255));
                    material.SetColor("_BottomColor", new Color32(120, 28, 255, 255));
                    material.SetColor("_SpecColor", new Color32(205, 28, 255, 255));

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

            return (slimeDefinition, slimeObject);
        }

        unsafe public static void CreateElectricSlime(ReactToShock quickSilverSlime, ReactToShock goldSlime, SlimeAppearance shocked)
        {
            Console.Log("Quicksilver position: " + quickSilverSlime.gameObject.transform.position);
            Console.Log("GoldSlime position: " + goldSlime.gameObject.transform.position);

            Vector3 Position = Vector3.Lerp(quickSilverSlime.gameObject.transform.position, goldSlime.gameObject.transform.position, 0.5F);
            Console.Log("FinalPosition: " + Position);
            if (quickSilverSlime.GetPrivateField<RegionMember>("regionMember").setId == RegionRegistry.RegionSetId.VALLEY)
            {
                //SRBehaviour.InstantiateActor(SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Ids.ELECTRIC_SLIME), GameObject.FindObjectOfType<WeaponVacuum>().GetPrivateField<RegionRegistry>("regionRegistry").GetCurrentRegionSetId(), Position, Quaternion.identity, false);
                
                GameObject ElectricSlime = SRBehaviour.InstantiateActor(SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Ids.ELECTRIC_SLIME), SceneContext.Instance.Player.GetComponent<RegionMember>().setId, true);
                ElectricSlime.transform.position = Position;
            }
            else
            {
                GameObject ElectricSlime = SRBehaviour.InstantiateActor(SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Ids.FORM_2_ELECTRIC_SLIME), SceneContext.Instance.Player.GetComponent<RegionMember>().setId, true);
                ElectricSlime.transform.position = Position;
                
                //SRBehaviour.InstantiateActor(SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Ids.FORM_2_ELECTRIC_SLIME), GameObject.FindObjectOfType<WeaponVacuum>().GetPrivateField<RegionRegistry>("regionRegistry").GetCurrentRegionSetId(), Position, Quaternion.identity, false);
            }
            Vector3 targetPosition = SRSingleton<SceneContext>.Instance.Player.transform.position + new Vector3(0, 0.5F, 0);
            Abilities.CreateShoot(Position + new Vector3(0, 3, 0), targetPosition - (Position + new Vector3(0, 3, 0)), quickSilverSlime.GetPrivateField<RegionMember>("regionMember").setId);

            Destroyer.DestroyActor(quickSilverSlime.gameObject, "transforming to electric slime");
            Destroyer.DestroyActor(goldSlime.gameObject, "transforming to electric slime");
        }

        /*unsafe public static void CreateElectricSlime(ReactToShock reactToShock)
        {
            try
            {
                Console.Log(SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.QUICKSILVER_SLIME).GetComponentInChildren<SlimeAppearanceApplicator>().Appearance + " Appearance really exists!");
                Console.Log(SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.QUICKSILVER_SLIME).GetComponentInChildren<SlimeAppearanceApplicator>().Appearance.ShockedAppearance + " Shocked Appearance really exists!");
            }
            catch (Exception)
            {

                Console.Log("nope, no shocked appearance nor normal appearance...");
            }

            GameObject ElectricSlime = SRBehaviour.InstantiateActor(SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Ids.ELECTRIC_SLIME), reactToShock.GetPrivateField<RegionMember>("regionMember").setId, reactToShock.transform.position, reactToShock.transform.rotation);
            GameObject ElectricSounds = SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.VALLEY_AMMO_1).GetComponentInChildren<DestroyAndShockOnTouching>().destroyFX;

            SRBehaviour.SpawnAndPlayFX(ElectricSounds, ElectricSlime, ElectricSlime.transform.position, ElectricSlime.transform.rotation);
            Vector3 targetPosition = SRSingleton<SceneContext>.Instance.Player.transform.position + new Vector3(0, 0.5F, 0);
            Abilities.CreateShoot(reactToShock.gameObject.transform.position + new Vector3(0, 3, 0), targetPosition - (reactToShock.gameObject.transform.position + new Vector3(0, 3, 0)), reactToShock.GetPrivateField<RegionMember>("regionMember").setId);


            GameObject.Destroy(reactToShock.gameObject);
        }*/

        unsafe public static (SlimeDefinition, GameObject) CreateSlime(Identifiable.Id SlimeId, string SlimeName, SlimeEat.FoodGroup[] foodGroup, Identifiable.Id[] produces, Identifiable.Id[] favorites, Sprite icon, SlimeAppearance.Palette spashColor, Color inventoryColor, Identifiable.Id slimeObjectToUse, bool autoRegister)
        {
            SlimeDefinition slimeDefinitionToUse = SRSingleton<GameContext>.Instance.SlimeDefinitions.GetSlimeByIdentifiableId(slimeObjectToUse);
            
            SlimeDefinition slimeDefinition = (SlimeDefinition)PrefabUtils.DeepCopyObject(slimeDefinitionToUse);
            slimeDefinition.AppearancesDefault = new SlimeAppearance[1];
            slimeDefinition.Diet.Produces = produces;

            slimeDefinition.Diet.MajorFoodGroups = foodGroup;

            slimeDefinition.Diet.AdditionalFoods = new Identifiable.Id[0];

            slimeDefinition.Diet.Favorites = favorites;

            slimeDefinition.Diet.EatMap?.Clear();

            slimeDefinition.CanLargofy = false;
            slimeDefinition.FavoriteToys = new Identifiable.Id[0];
            slimeDefinition.Name = SlimeName;
            slimeDefinition.IdentifiableId = SlimeId;

            GameObject slimeObject = PrefabUtils.CopyPrefab(SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(slimeObjectToUse));
            slimeObject.name = SlimeName;
            slimeObject.GetComponent<PlayWithToys>().slimeDefinition = slimeDefinition;
            slimeObject.GetComponent<SlimeAppearanceApplicator>().SlimeDefinition = slimeDefinition;
            slimeObject.GetComponent<SlimeEat>().slimeDefinition = slimeDefinition;
            slimeObject.GetComponent<Identifiable>().id = SlimeId;
            if (slimeObject.GetComponent<PinkSlimeFoodTypeTracker>())
                GameObject.Destroy(slimeObject.GetComponent<PinkSlimeFoodTypeTracker>());

            SlimeAppearance slimeAppearance = (SlimeAppearance)PrefabUtils.DeepCopyObject(slimeDefinitionToUse.AppearancesDefault[0]);
            slimeDefinition.AppearancesDefault[0] = slimeAppearance;

            LookupRegistry.RegisterVacEntry(SlimeId, inventoryColor, icon);

            slimeAppearance.Icon = icon;

            slimeAppearance.ColorPalette = spashColor;

            slimeAppearance.ColorPalette.Ammo = inventoryColor;

            slimeObject.GetComponent<SlimeAppearanceApplicator>().Appearance = slimeAppearance;

            return (slimeDefinition, slimeObject);
        }
    }
}
