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
using TranslationAPI;
using MoSecretStyles;
namespace ElementalElectricTree
{
    public class Main : ModEntryPoint
    {
        static Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(typeof(Main), "electric_tree_slimes");
        public static AssetBundle assetBundle = AssetBundle.LoadFromStream(manifestResourceStream);

        static Stream newManifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(typeof(Main), "electric_tree_slimes_2");
        public static AssetBundle newAssetBundle = AssetBundle.LoadFromStream(newManifestResourceStream);


        static Stream manifestResourceStream2 = Assembly.GetExecutingAssembly().GetManifestResourceStream(typeof(Main), "special_electric_slimes");
        public static AssetBundle assetBundle2 = AssetBundle.LoadFromStream(manifestResourceStream2);

        public static Dictionary<Identifiable.Id, Dictionary<SlimeDefinition, SlimeAppearance>> SecretStyleList = new Dictionary<Identifiable.Id, Dictionary<SlimeDefinition, SlimeAppearance>>();

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
            //Console.Log("Loading Electric");
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
            SRML.LandPlotUpgradeRegistry.RegisterPlotUpgrader<ElectroMeter.Upgrade>(LandPlot.Id.CORRAL);

            Translations.Translate();



            if (IsModLoaded("shinyslimes"))
            {
                Console.Log("ShinySlimes is loaded");
                ShinySlime();
            }



            HarmonyInstance.PatchAll();
        }

        public override void Load()
        {
            if (IsModLoaded("mosecretstyles"))
            {
                ShortCutter.Log("MORE SECRET STYLES");
                SRObjects.Get<Shader>("SR/AMP/Slime/Body/Matcap Stripe").PrintContent();

                ModSecretStyle.onSecretStylesInitialization += () =>
                {
                    ShortCutter.Log("MORE SECRET STYLES STARTING");
                    Identifiable.Id Form1Id = Ids.ELECTRIC_SLIME;
                    ModSecretStyle modSecretStyle = new ModSecretStyle(Form1Id, new Vector3(83.18477f, 14.73f, -142.1283f), new Quaternion(), "cellRanch_Home", "podSSBlackLightning");
                    modSecretStyle.SecretStyle.NameXlateKey = "t.secret_style_electric_slime";
                    TranslationPatcher.AddActorTranslation("t.secret_style_electric_slime", "Black Lightning");
                    
                    Material material = new Material(SceneContext.Instance.SlimeAppearanceDirector.SlimeDefinitions.GetSlimeByIdentifiableId(Identifiable.Id.QUICKSILVER_SLIME).AppearancesDefault[0].ShockedAppearance.Structures[0].DefaultMaterials[0]);
                    material.name = "BLACK_THUNDERS";


                    modSecretStyle.SecretStyle.Structures[0].DefaultMaterials[0] = material;
                    modSecretStyle.SecretStyle.Structures[1].DefaultMaterials[0] = material;
                    modSecretStyle.SecretStyle.Icon = assetBundle.LoadAsset<Sprite>("ElectricSlimeSprite");
                    modSecretStyle.Definition.GetAppearanceForSet(SlimeAppearance.AppearanceSaveSet.CLASSIC).Icon = assetBundle.LoadAsset<Sprite>("ElectricSlimeSprite");
                    material.SetColor("_TopColor", Color.red);
                    material.SetColor("_MiddleColor", Color.red);
                    material.SetColor("_BottomColor", Color.red);
                    material.SetColor("_GlowTop", Color.red);

                    ShortCutter.Log("Logging texture");
                    ShortCutter.Log(material.GetTexture(Shader.PropertyToID("_StripeTexture")));
                };
            }

            AmmoRegistry.RegisterAmmoPrefab(PlayerState.AmmoMode.DEFAULT, SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.VALLEY_AMMO_4));
            AmmoRegistry.RegisterAmmoPrefab(PlayerState.AmmoMode.DEFAULT, SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.VALLEY_AMMO_2));
            AmmoRegistry.RegisterAmmoPrefab(PlayerState.AmmoMode.DEFAULT, SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.VALLEY_AMMO_1));
            AmmoRegistry.RegisterAmmoPrefab(PlayerState.AmmoMode.DEFAULT, SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.VALLEY_AMMO_3));

            GameObject goldSlime = SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.GOLD_SLIME);

            goldSlime.AddComponent<ReactToShock>().GetCopyOf(SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.QUICKSILVER_SLIME).GetComponentInChildren<ReactToShock>());

            SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.VALLEY_AMMO_4).AddComponent<ElectricSlimeCreator>();
            SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.VALLEY_AMMO_3).AddComponent<ElectricSlimeCreator>();
            SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.VALLEY_AMMO_2).AddComponent<ElectricSlimeCreator>();
            SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.VALLEY_AMMO_1).AddComponent<ElectricSlimeCreator>();

            Custom_Slime_Creator.RegisterSlime(ElectricSlimeForm1Prefab.GetPrefab());
            Custom_Slime_Creator.RegisterSlime(ElectricSlimeForm2Prefab.GetPrefab());
            Custom_Slime_Creator.RegisterSlime(Custom_Slime_Creator.CreatPlasmaSlimePrefab());

            Custom_Food_Creator.RegisterFood(SlimeEat.FoodGroup.MEAT, Custom_Food_Creator.CreateElectricHenPrefab(), Color.yellow, ((IEnumerable<PediaDirector.IdEntry>)SRSingleton<SceneContext>.Instance.PediaDirector.entries).First<PediaDirector.IdEntry>((Func<PediaDirector.IdEntry, bool>)(x => x.id == PediaDirector.Id.HENHEN)).icon);
            Custom_Food_Creator.RegisterFood(SlimeEat.FoodGroup.MEAT, Custom_Food_Creator.CreateElectricRoosterPrefab(), Color.yellow, assetBundle.LoadAsset<Sprite>("electricRoostro"));

            Custom_Plort_Creator.RegisterPlort(

                Custom_Plort_Creator.CreateElectricPlortPrefab(
                    "electricPlort",
                    Ids.ELECTRIC_PLORT,
                    Identifiable.Id.QUICKSILVER_PLORT),

                assetBundle.LoadAsset<Sprite>("ElectricPlorts"),
                Color.yellow,
                600,
                8
            );

            if (IsModLoaded("translationapi"))
            {
                TranslationUtil.RegisterAssembly(Assembly.GetExecutingAssembly());
            }

            PediaRegistry.RegisterIdEntry(Ids.ELECTRIC_SLIME_ENTRY, assetBundle.LoadAsset<Sprite>("ElectricSlimeSprite"));
            PediaRegistry.RegisterIdEntry(Ids.FORM_2_ELECTRIC_SLIME_ENTRY, assetBundle.LoadAsset<Sprite>("ElectricSlimeSprite"));
            PediaRegistry.RegisterIdEntry(Ids.PLASMA_SLIME_ENTRY, assetBundle.LoadAsset<Sprite>("ElectricSlimeSprite"));

            assetBundle.GetAllAssetNames().PrintContent<string>();
            assetBundle2.GetAllAssetNames().PrintContent<string>();
            newAssetBundle.GetAllAssetNames().PrintContent<string>();

            SRML.LandPlotUpgradeRegistry.RegisterPurchasableUpgrade<CorralUI>(ElectroMeter.CreateElectricContainerEntry());
        }

        public override void PostLoad()
        {
            SRCallbacks.OnSaveGameLoaded += SRCallbacks_OnSaveGameLoaded;
        }

        private void SRCallbacks_OnSaveGameLoaded(SceneContext t)
        {
            if (SceneContext.Instance.Player.GetComponent<Effects>() == null)
                SceneContext.Instance.Player.AddComponent<Effects>();

            ExchangeDirector exchangeDirector = SRSingleton<SceneContext>.Instance.ExchangeDirector;
            ExchangeDirector.ProgressOfferEntry progressOfferEntry1 = exchangeDirector.progressOffers[1];
            progressOfferEntry1.rewardLevels[0].requestedItem = Identifiable.Id.PINK_SLIME;
            progressOfferEntry1.rewardLevels[0].count = 1;

            progressOfferEntry1.rewardLevels[1].requestedItem = Identifiable.Id.PINK_SLIME;
            progressOfferEntry1.rewardLevels[1].count = 1;

            progressOfferEntry1.rewardLevels[2].requestedItem = Identifiable.Id.PINK_SLIME;
            progressOfferEntry1.rewardLevels[2].count = 1;

            Array.Resize(ref progressOfferEntry1.rewardLevels, 4);

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
                    "I needed the help of Thora (I'm not joking).",
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
