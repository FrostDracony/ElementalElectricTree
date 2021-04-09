using System;

using System.Reflection;
using System.IO;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SRML;
using SRML.SR;
using Console = SRML.Console.Console;
using SRML.Utils;
using SRML.SR.Translation;
using SRML.SR.SaveSystem;
using UnityEngine;
using HarmonyLib;
using Creators;
using ElementalElectricTree.Other;

namespace ElementalElectricTree
{
    public class Main : ModEntryPoint
    {

        static Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(typeof(Main), "slime_body_parts");
        public static AssetBundle assetBundle = AssetBundle.LoadFromStream(manifestResourceStream);

        public static bool IsModLoaded(string modId)
        {
            return SRModLoader.IsModPresent(modId);
        }

        public override void PreLoad()
        {
            //Console.Log("Loading Electric");
            PediaRegistry.RegisterIdentifiableMapping(Ids.ELECTRIC_SLIME_ENTRY, Ids.ELECTRIC_SLIME);
            PediaRegistry.SetPediaCategory(Ids.ELECTRIC_SLIME_ENTRY, PediaRegistry.PediaCategory.SLIMES);
            new SlimePediaEntryTranslation(Ids.ELECTRIC_SLIME_ENTRY)
                .SetTitleTranslation("Electric Slime")
                .SetIntroTranslation("An dangerous slime with a dangerous high volt, watch out!")
                .SetDietTranslation("Fruit and Hens")
                .SetFavoriteTranslation("Electric Hens")
                .SetSlimeologyTranslation("This slime is really mysterious. It seems to emmit a high concentration of electricity, and when it gets angry it can concentrate all his energy in one, unique electric ball that is probably the deadliest shoot a slime can do! They seem to be made out of pure electricity, because they reacted to all electricity tests we made with them. And they have to be powerfull, because they can shock quickislvers! Such a slime was never be seen before, and by its form we can think it's one of the ancestors of the quicksilvers! Who knows how many secrets he can learn from it and it's origins...")
                .SetRisksTranslation("Because of all the electricity, touching this slime can hurt the rancher and, if you are unlucky, even electroshocking you (the same effects of beign stunned, only that you need more time to recover and it dosen't recover immediately). This slime can shoot extrelmy powerfull electroshock balls to the rancher, to avoid at all costs (WIP: In the future it will be able to disable corrals, so think twice where you place it)!")
                .SetPlortonomicsTranslation("???");
            PediaRegistry.RegisterIdEntry(Ids.ELECTRIC_SLIME_ENTRY, assetBundle.LoadAsset<Sprite>("ElectricSlimeSprite"));

            //Console.RegisterCommand(new PrintComponentsCommand());

            HarmonyInstance.PatchAll();
        }

        unsafe public override void Load()
        {
            AmmoRegistry.RegisterAmmoPrefab(PlayerState.AmmoMode.DEFAULT, SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.VALLEY_AMMO_4));
            AmmoRegistry.RegisterAmmoPrefab(PlayerState.AmmoMode.DEFAULT, SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.VALLEY_AMMO_2));
            AmmoRegistry.RegisterAmmoPrefab(PlayerState.AmmoMode.DEFAULT, SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.VALLEY_AMMO_1));
            AmmoRegistry.RegisterAmmoPrefab(PlayerState.AmmoMode.DEFAULT, SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.VALLEY_AMMO_3));
            GameObject goldSlime = SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.GOLD_SLIME);

            ReactToShock goldSlimeRTS = goldSlime.AddComponent<ReactToShock>().GetCopyOf(SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.QUICKSILVER_SLIME).GetComponentInChildren<ReactToShock>());
        } 

        public override void PostLoad()
        {
            (SlimeDefinition, GameObject) SlimeTuple = Custom_Slime_Creator.CreateSlime(Ids.ELECTRIC_SLIME, "electricSlime"); //Insert your own Id in the first argumeter
            //Getting the SlimeDefinition and GameObject separated
            SlimeDefinition Slime_Slime_Definition = SlimeTuple.Item1;
            GameObject Slime_Slime_Object = SlimeTuple.Item2;

            //And well, registering it!

            LookupRegistry.RegisterIdentifiablePrefab(Slime_Slime_Object);
            SlimeRegistry.RegisterSlimeDefinition(Slime_Slime_Definition);

            /*Console.Log("SlimeBones 0: " + Slime_Slime_Object.GetComponent<SlimeAppearanceApplicator>().Appearance.Structures[0].Element.Name);
            Console.Log("SlimeBones 1: " + Slime_Slime_Object.GetComponent<SlimeAppearanceApplicator>().Appearance.Structures[1].Element.Name);*/

            /*foreach (SlimeAppearanceObject prefab in Slime_Slime_Object.GetComponent<SlimeAppearanceApplicator>().Appearance.Structures[1].Element.Prefabs)
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

                if (gameObject.TryGetComponent<MeshFilter>(out MeshFilter filter))
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
                }

            }*/

            AmmoRegistry.RegisterAmmoPrefab(PlayerState.AmmoMode.DEFAULT, Slime_Slime_Object);
            AmmoRegistry.RegisterAmmoPrefab(PlayerState.AmmoMode.NIMBLE_VALLEY, Slime_Slime_Object);
            TranslationPatcher.AddActorTranslation("l.electric_slime", "Electric Slime");
            LookupRegistry.RegisterVacEntry(Ids.ELECTRIC_SLIME, Color.yellow, assetBundle.LoadAsset<Sprite>("ElectricSlimeSprite"));
            /*foreach(SlimeAppearanceObject slimeAppearanceObject in GameObject.FindObjectsOfType<SlimeAppearanceObject>())
            {
                Console.Log("!WARNING: " + slimeAppearanceObject.ToString() + ". Parent: " + slimeAppearanceObject.PrintParent());
                Console.Log("");
            }*/

            SlimeDefinition slimeByIdentifiableId3 = SRSingleton<GameContext>.Instance.SlimeDefinitions.GetSlimeByIdentifiableId(Identifiable.Id.TARR_SLIME);
            slimeByIdentifiableId3.Diet.EatMap.Add(new SlimeDiet.EatMapEntry
            {
                eats = Ids.ELECTRIC_SLIME,
                becomesId = Identifiable.Id.NONE,
                driver = SlimeEmotions.Emotion.NONE,
                producesId = Identifiable.Id.NONE
            });

        }
    }

    
}
