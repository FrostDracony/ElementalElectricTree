using System;
using SRML.Utils;
using System.Reflection;
using System.IO;

using System.Collections.Generic;
using System.Linq;
using SRML;
using SRML.SR;
using Console = SRML.Console.Console;
using UnityEngine;
using Creators;
using ElementalElectricTree.Other;
using ElementalElectricTree.Patches;
using ElementalElectricTree.Creators;
using MoSecretStyles;

namespace ElementalElectricTree
{
    public class Main : ModEntryPoint
    {
        static Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(typeof(Main), "electric_tree_slimes");
        public static AssetBundle assetBundle = AssetBundle.LoadFromStream(manifestResourceStream);

        static Stream assetsManifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(typeof(Main), "electric_slime_assets");
        public static AssetBundle AssetsAssetBundle = AssetBundle.LoadFromStream(assetsManifestResourceStream);

        static Stream newManifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(typeof(Main), "electric_tree_slimes_2");
        public static AssetBundle newAssetBundle = AssetBundle.LoadFromStream(newManifestResourceStream);


        static Stream manifestResourceStream2 = Assembly.GetExecutingAssembly().GetManifestResourceStream(typeof(Main), "special_electric_slimes");
        public static AssetBundle assetBundle2 = AssetBundle.LoadFromStream(manifestResourceStream2);

        //public static Stream manifestResourceStreamZones = Assembly.GetExecutingAssembly().GetManifestResourceStream(typeof(Main), "Electric_Canyon_withList.json");

        public static Dictionary<Identifiable.Id, SlimeAppearance> SecretStyleList = new Dictionary<Identifiable.Id, SlimeAppearance>();

        public static Dictionary<string, GameObject> corralUpgradesModels = new Dictionary<string, GameObject>();

        public static Transform prefabParent;

        public Main()
        {
            GameObject target = new GameObject("PrefabParent");
            target.SetActive(false);
            UnityEngine.Object.DontDestroyOnLoad(target);
            prefabParent = target.transform;
        }

        public static bool IsModLoaded(string modId)
        {
            return SRModLoader.IsModPresent(modId);
        }

        public void ShinySlime()
        {
            var methodInfo = typeof(ShinySpawn_Shinify_Patches).GetMethod("Shinify");
            var methodInfo1 = typeof(ShinySpawn_ShinyCheck_Patches).GetMethod("ShinyCheck");

            HarmonyInstance.Patch(typeof(Shinies.ShinySpawn).GetMethod("Shinify"), null, new HarmonyLib.HarmonyMethod(methodInfo));
            HarmonyInstance.Patch(typeof(Shinies.ShinySpawn).GetMethod("ShinyCheck", BindingFlags.Instance | BindingFlags.NonPublic), new HarmonyLib.HarmonyMethod(methodInfo1));
        }

        public override void PreLoad()
        {
            Console.Log("PreLoading Electric");
            Main.assetBundle.GetAllAssetNames().PrintContent();
            PediaRegistry.RegisterIdentifiableMapping(Ids.FORM_2_ELECTRIC_SLIME_ENTRY, Ids.FORM_2_ELECTRIC_SLIME);
            PediaRegistry.SetPediaCategory(Ids.FORM_2_ELECTRIC_SLIME_ENTRY, PediaRegistry.PediaCategory.SLIMES);

            PediaRegistry.RegisterIdentifiableMapping(Ids.ELECTRIC_SLIME_ENTRY, Ids.ELECTRIC_SLIME);
            PediaRegistry.SetPediaCategory(Ids.ELECTRIC_SLIME_ENTRY, PediaRegistry.PediaCategory.SLIMES);

            PediaRegistry.RegisterIdentifiableMapping(PediaDirector.Id.PLORTS, Ids.ELECTRIC_PLORT);
            Identifiable.PLORT_CLASS.Add(Ids.ELECTRIC_PLORT);
            Identifiable.NON_SLIMES_CLASS.Add(Ids.ELECTRIC_PLORT);
            Identifiable.NON_SLIMES_CLASS.Add(Ids.ELECTRIC_CANNON_SHOT);

            /*new SlimePediaEntryTranslation(Ids.ELECTRIC_SLIME_ENTRY);
            new SlimePediaEntryTranslation(Ids.PLASMA_SLIME_ENTRY);*/
            /*new SlimePediaEntryTranslation(Ids.ELECTRIC_SLIME_ENTRY)
                .SetTitleTranslation("Electric Slime")
                .SetIntroTranslation("An dangerous slime with a dangerous high volt, watch out!")
                .SetDietTranslation("Fruit and Hens")
                .SetFavoriteTranslation("Electric Hens")
                .SetSlimeologyTranslation("This slime is really mysterious. It seems to emmit a high concentration of electricity, and when it gets angry it can concentrate all his energy in one, unique electric ball that is probably the deadliest shoot a slime can do! They seem to be made out of pure electricity, because they reacted to all electricity tests we made with them. And they have to be powerfull, because they can shock quickislvers! Such a slime was never be seen before, and by its form we can think it's one of the ancestors of the quicksilvers! Who knows how many secrets he can learn from it and it's origins...")
                .SetRisksTranslation("Because of all the electricity, touching this slime can hurt the rancher and, if you are unlucky, even electroshocking you (the same effects of beign stunned, only that you need more time to recover and it dosen't recover immediately). This slime can shoot extrelmy powerfull electroshock balls to the rancher, to avoid at all costs (WIP: In the future it will be able to disable corrals, so think twice where you place it)!")
                .SetPlortonomicsTranslation("???");*/


            Console.RegisterCommand(new PrintComponentsCommand());
            Console.RegisterCommand(new PrintMaterialInfosCommand());
            Console.RegisterCommand(new PrintMaterialCommand());
            Console.RegisterCommand(new PrintPlotComponentsCommand());
            Console.RegisterCommand(new PrintSlimeAppearanceContentCommand());
            Console.RegisterCommand(new PrintLODContent());
            Console.RegisterCommand(new PrintPlayerComponentsCommand());

            //SRML.LandPlotUpgradeRegistry.RegisterPlotUpgrader<ElectroMeter.Upgrade>(LandPlot.Id.CORRAL);

            Translations.Translate();

            if (IsModLoaded("shinyslimes"))
            {
                Console.Log("ShinySlimes is loaded");
                ShinySlime();
            }

            LandPlotUpgradeRegistry.RegisterPurchasableUpgrade<CorralUI>(ElectroMeter.CreateElectricContainerEntry());


            HarmonyInstance.PatchAll();
        }

        public override void Load()
        {
            #region Translation
            if (IsModLoaded("translationapi"))
            {
                ShortCutter.Log("TranslationAPI is loaded");
                TranslationAPI.TranslationUtil.RegisterAssembly(Assembly.GetExecutingAssembly());
            }
            #endregion

            #region SecretStyles
            if (IsModLoaded("mosecretstyles"))
            {
                ShortCutter.Log("MORE SECRET STYLES");
                SRObjects.Get<Shader>("SR/AMP/Slime/Body/Matcap Stripe").PrintContent();

                ModSecretStyle.onSecretStylesInitialization += () =>
                {
                    #region BLACK LIGHTNING
                    ShortCutter.Log("MORE SECRET STYLES STARTING");
                    Identifiable.Id Form1Id = Ids.ELECTRIC_SLIME;
                    ModSecretStyle modSecretStyle = new ModSecretStyle(Form1Id, new Vector3(-146.9633f, 44.74749f, -948.8106f), new Quaternion(), "cellValley_RaceTrack2", "podSSBlackLightning");
                    modSecretStyle.SecretStyle.NameXlateKey = "t.secret_style_electric_slime";
                    Material material = new Material(SceneContext.Instance.SlimeAppearanceDirector.SlimeDefinitions.GetSlimeByIdentifiableId(Identifiable.Id.QUICKSILVER_SLIME).AppearancesDefault[0].ShockedAppearance.Structures[0].DefaultMaterials[0]);
                    Material whiteMaterial = new Material(SceneContext.Instance.SlimeAppearanceDirector.SlimeDefinitions.GetSlimeByIdentifiableId(Identifiable.Id.QUICKSILVER_SLIME).AppearancesDefault[0].ShockedAppearance.Structures[0].DefaultMaterials[0]);
                    material.name = "BLACK_THUNDERS";
                    whiteMaterial.name = "BRIGHT";

                    modSecretStyle.SecretStyle.Structures[0].DefaultMaterials[0] = material;
                    modSecretStyle.SecretStyle.Structures[1].DefaultMaterials[0] = whiteMaterial;

                    modSecretStyle.SecretStyle.Icon = AssetsAssetBundle.LoadAsset<Sprite>("SS_ElectricSlime");
                    modSecretStyle.Definition.GetAppearanceForSet(SlimeAppearance.AppearanceSaveSet.CLASSIC).Icon = AssetsAssetBundle.LoadAsset<Sprite>("ElectricSlime");

                    ShortCutter.Log("_Gloss" + material.GetFloat("_Gloss"));
                    ShortCutter.Log("_GlossPower" + material.GetFloat("_GlossPower"));

                    #region Modyfing Materials
                    material.SetColor("_TopColor", new Color(0, 0, 0));
                    material.SetColor("_MiddleColor", new Color(0, 0, 0));
                    material.SetColor("_BottomColor", new Color(0, 0, 0));
                    material.SetColor("_GlowTop", new Color(0, 0, 0));
                    material.SetFloat("_GlossPower", 0.0f);
                    material.SetFloat("_Gloss", 0.0f);

                    whiteMaterial.SetColor("_TopColor", Color.white);
                    whiteMaterial.SetColor("_MiddleColor", Color.white);
                    whiteMaterial.SetColor("_BottomColor", Color.white);
                    whiteMaterial.SetColor("_GlowTop", Color.white);
                    #endregion

                    #region Other Modifications
                    modSecretStyle.SecretStyle.ColorPalette = new SlimeAppearance.Palette()
                    {
                        Bottom = Color.black,
                        Top = Color.black,
                        Middle = Color.black
                    };
                    #endregion
                    #endregion



                    #region OVERDRIVE

                    ShortCutter.Log("Starting Form2");
                    Identifiable.Id Form2Id = Ids.FORM_2_ELECTRIC_SLIME;
                    ModSecretStyle modSecretStyleOverDrive = new ModSecretStyle(Form2Id, new Vector3(-156.8036f, 38.16051f, -891.7065f), new Quaternion(), "cellValley_RaceTrack2", "podSSOverDrive");
                    modSecretStyleOverDrive.SecretStyle.NameXlateKey = "t.secret_style_electric_slime_form_2";

                    ShortCutter.Log("Material Part");
                    Material overdriveMaterial = new Material(SceneContext.Instance.SlimeAppearanceDirector.SlimeDefinitions.GetSlimeByIdentifiableId(Identifiable.Id.QUICKSILVER_SLIME).AppearancesDefault[0].ShockedAppearance.Structures[0].DefaultMaterials[0]);
                    overdriveMaterial.name = "OVERDRIVE";

                    modSecretStyleOverDrive.SecretStyle.Structures[0].DefaultMaterials[0] = overdriveMaterial;

                    ShortCutter.Log("Even more things");
                    modSecretStyleOverDrive.SecretStyle.Icon = AssetsAssetBundle.LoadAsset<Sprite>("Overcharged");
                    modSecretStyleOverDrive.Definition.GetAppearanceForSet(SlimeAppearance.AppearanceSaveSet.CLASSIC).Icon = AssetsAssetBundle.LoadAsset<Sprite>("Form2ElectricSlime");

                    ShortCutter.Log("_Gloss" + overdriveMaterial.GetFloat("_Gloss"));
                    ShortCutter.Log("_GlossPower" + overdriveMaterial.GetFloat("_GlossPower"));

                    #region Modyfing Materials
                    overdriveMaterial.SetColor("_TopColor", Color.cyan);
                    overdriveMaterial.SetColor("_MiddleColor", Color.cyan);
                    overdriveMaterial.SetColor("_BottomColor", Color.cyan);
                    ShortCutter.Log("First Colors set");

                    ShortCutter.Log("material sets");

                    #endregion

                    #region Other Modifications
                    ShortCutter.Log("Final");
                    modSecretStyleOverDrive.SecretStyle.ColorPalette = new SlimeAppearance.Palette()
                    {
                        Bottom = Color.cyan,
                        Top = Color.cyan,
                        Middle = Color.cyan
                    };
                    ShortCutter.Log("Done");

                    #endregion
                    #endregion

                    ShortCutter.Log("Logging texture");
                    ShortCutter.Log(material.GetTexture(Shader.PropertyToID("_StripeTexture")));
                };

            }
            #endregion

            #region Registering Ammo
            AmmoRegistry.RegisterAmmoPrefab(PlayerState.AmmoMode.DEFAULT, SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.VALLEY_AMMO_4));
            AmmoRegistry.RegisterAmmoPrefab(PlayerState.AmmoMode.DEFAULT, SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.VALLEY_AMMO_2));
            AmmoRegistry.RegisterAmmoPrefab(PlayerState.AmmoMode.DEFAULT, SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.VALLEY_AMMO_1));
            AmmoRegistry.RegisterAmmoPrefab(PlayerState.AmmoMode.DEFAULT, SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.VALLEY_AMMO_3));
            #endregion

            //Without this, no electric slimes (needed to check when gold slime, quicksilver and valley shots collide
            GameObject goldSlime = SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.GOLD_SLIME);
            goldSlime.AddComponent<ReactToShock>().GetCopyOf(SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.QUICKSILVER_SLIME).GetComponentInChildren<ReactToShock>());

            #region Making Stuff Vaccable
            SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.VALLEY_AMMO_4).AddComponent<ElectricSlimeCreator>();
            SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.VALLEY_AMMO_3).AddComponent<ElectricSlimeCreator>();
            SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.VALLEY_AMMO_2).AddComponent<ElectricSlimeCreator>();
            SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.VALLEY_AMMO_1).AddComponent<ElectricSlimeCreator>();
            #endregion

            #region Registering Slims
            Custom_Slime_Creator.RegisteringAllSlimes();
            #endregion

            #region Registering Foods
            Custom_Food_Creator.RegisterAllFoods();
            #endregion

            #region Registering Plorts
            Custom_Plort_Creator.RegisterAllPlorts();
            #endregion

            PediaRegistry.RegisterIdEntry(Ids.ELECTRIC_SLIME_ENTRY, AssetsAssetBundle.LoadAsset<Sprite>("ElectricSlime"));
            PediaRegistry.RegisterIdEntry(Ids.FORM_2_ELECTRIC_SLIME_ENTRY, AssetsAssetBundle.LoadAsset<Sprite>("Form2ElectricSlime"));
            PediaRegistry.RegisterIdEntry(Ids.PLASMA_SLIME_ENTRY, assetBundle.LoadAsset<Sprite>("ElectricSlimeSprite"));

            assetBundle.GetAllAssetNames().PrintContent<string>();
            assetBundle2.GetAllAssetNames().PrintContent<string>();
            newAssetBundle.GetAllAssetNames().PrintContent<string>();
        }

        public override void PostLoad()
        {
            SRCallbacks.OnSaveGameLoaded += SRCallbacks_OnSaveGameLoaded;
            SRCallbacks.PreSaveGameLoad += SRCallbacks_PreSaveGameLoad;
        }

        private void SRCallbacks_PreSaveGameLoad(SceneContext t)
        {
            #region Creating Zones
            ElectricCanyon.Create();
            #endregion
        }

        private void SRCallbacks_OnSaveGameLoaded(SceneContext t)
        {

            if (SceneContext.Instance.Player.GetComponent<Effects>() == null)
                SceneContext.Instance.Player.AddComponent<Effects>();

            #region Changing Mochi's RewardLevels
            ExchangeDirector exchangeDirector = SRSingleton<SceneContext>.Instance.ExchangeDirector;
            ExchangeDirector.ProgressOfferEntry progressOfferEntry1 = exchangeDirector.progressOffers[1];
            progressOfferEntry1.rewardLevels[0].requestedItem = Identifiable.Id.PINK_SLIME;
            progressOfferEntry1.rewardLevels[0].count = 1;

            progressOfferEntry1.rewardLevels[1].requestedItem = Identifiable.Id.PINK_SLIME;
            progressOfferEntry1.rewardLevels[1].count = 1;

            progressOfferEntry1.rewardLevels[2].requestedItem = Identifiable.Id.PINK_SLIME;
            progressOfferEntry1.rewardLevels[2].count = 1;

            Array.Resize(ref progressOfferEntry1.rewardLevels, progressOfferEntry1.rewardLevels.Length + 1);
            #endregion

            #region Conversations
            RancherChatMetadata.Entry[] introConversation = exchangeDirector.CreateRancherChatConversation("mochi",
                new string[]
                {
                    "Hmm, you need to ask me something? what do you want?",
                    "You need my help? What about?",
                    "Time is money, so I won't help you...",
                    "...unless it's something interesting",
                    "Hmm, a new type of slime never seen before and you're telling me that it is impossible to contain those?",
                    "Nothing is impossible for Mochi Miles!",
                    "I agree to help you, but only at 1 condition: I need 50 plorts of this slime to experiment on",
                    "Maybe my father wil be proud of me if I do this and finally accept me.",
                    "Maybe I'll return back to Earth and see my family again, maybe my friends too...",
                    "Maybe my friends too...",
                    "What're you still doing here? Go and bring me those plorts!"
                    },
                new Sprite[]
                {
                    SRObjects.Get<Sprite>("mochi_default"),
                    SRObjects.Get<Sprite>("mochi_confident"),
                    SRObjects.Get<Sprite>("mochi_mocking"),
                    SRObjects.Get<Sprite>("mochi_charming"),
                    SRObjects.Get<Sprite>("mochi_default"),
                    SRObjects.Get<Sprite>("mochi_boastful"),
                    SRObjects.Get<Sprite>("mochi_confident"),
                    SRObjects.Get<Sprite>("mochi_shy"),
                    SRObjects.Get<Sprite>("mochi_sad1"),
                    SRObjects.Get<Sprite>("mochi_sad2"),
                    SRObjects.Get<Sprite>("mochi_angry")
                });

            RancherChatMetadata.Entry[] repeatConversation = exchangeDirector.CreateRancherChatConversation("mochi",
                new string[]
                {
                    "Well, what are you still doing here?",
                    "If you want my help, then bring me those plorts",
                },
                new Sprite[]
                {
                    SRObjects.Get<Sprite>("mochi_sad"),
                    SRObjects.Get<Sprite>("mochi_shy")
                });

            RancherChatMetadata.Entry[] conversation1 = exchangeDirector.CreateRancherChatConversation("mochi", 
                new string[] 
                { 
                    "Well, great Job.", 
                    "As promised, I was able to make a machine that would be able to limit the powers of the electric slimes.",
                    "I'd say I did everything alone, but...",
                    "I needed the help of Thora.",
                    "Yes, the old lady, she gave me a part of her knowledge and, oh gosh, Viktor would jaleaous if he'd see what I learned.",
                    "I now need to take a break, all that job exhausted me.",
                    "Nevertheless, you should go to talking to her next time there is a problem.",
                    "To be honest, she knows way more than she makes us believe...",
                    "Even Thora knows more than me...",
                    "...",
                }, 
                new Sprite[] 
                {
                    SRObjects.Get<Sprite>("mochi_default"),
                    SRObjects.Get<Sprite>("mochi_boastful"),
                    SRObjects.Get<Sprite>("mochi_sad1"),
                    SRObjects.Get<Sprite>("mochi_sad2"),
                    SRObjects.Get<Sprite>("mochi_angry"),
                    SRObjects.Get<Sprite>("mochi_charming"),
                    SRObjects.Get<Sprite>("mochi_shy"),
                    SRObjects.Get<Sprite>("mochi_shy"),
                    SRObjects.Get<Sprite>("mochi_sad1"),
                    SRObjects.Get<Sprite>("mochi_sad2"),
                });
            #endregion

            RancherChatMetadata rancherChatMetadataIntro = ScriptableObject.CreateInstance<RancherChatMetadata>();
            rancherChatMetadataIntro.entries = introConversation;

            RancherChatMetadata rancherChatMetadataRepeat = ScriptableObject.CreateInstance<RancherChatMetadata>();
            rancherChatMetadataRepeat.entries = repeatConversation;


            rancherChatMetadataIntro.entries = progressOfferEntry1.rancherChatEndIntro.entries.Concat(rancherChatMetadataIntro.entries).ToArray();
            progressOfferEntry1.rancherChatEndIntro.entries = conversation1;


            progressOfferEntry1.rewardLevels[3] = ExchangeCreator.CreateRewardLevel(
                                        50,
                                        rancherChatMetadataIntro,
                                        rancherChatMetadataRepeat,
                                        Ids.ELECTRIC_PLORT,
                                        Ids.REWARD_ELECTRIC_CONTAINER_UPGRADE
                                      );


        }

    }

    
}
