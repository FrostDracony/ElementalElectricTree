using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using Console = SRML.Console.Console;
using MonomiPark.SlimeRancher.Regions;
using ElementalElectricTree.Other;

namespace ElementalElectricTree.Patches
{
    [HarmonyPatch(typeof(RegionRegistry), "GetContaining", new Type[] { typeof(RegionRegistry.RegionSetId), typeof(List<Region>), typeof(Vector3) }, new ArgumentType[] { ArgumentType.Normal, ArgumentType.Ref, ArgumentType.Normal })]
    class DebugPatches
    {
        unsafe public static bool Prefix(RegionRegistry __instance, RegionRegistry.RegionSetId setId, ref List<Region> regions)
        {
            ///Console.Log(setId.ToString());
            //BoundsQuadtree<Region> test = null;

            /*foreach(Region region in regions)
            {
                Console.Log(region.ToString());
            }*/

            if(__instance.GetPrivateField<Dictionary<RegionRegistry.RegionSetId, BoundsQuadtree<Region>>>("regionsTrees").TryGetValue(setId, out BoundsQuadtree<Region> test))
            {
                //Console.Log("true");
                return true;
            }
            else //else, if its something modded, stop the code from continuing and avoid an error
            {
                //Console.Log("false");
                return false;
            }
        }
    }

    /*[HarmonyPatch(typeof(SlimeSubbehaviourPlexer), "RegistryFixedUpdate")]
    class DebugPatchesV2
    {
        public static bool Prefix(SlimeSubbehaviourPlexer __instance)
        {
            
            return true;
        }
    }*/

    [HarmonyPatch(typeof(Shinies.ShinySpawn), "ShinyCheck")]
    class DebugPatchesV2
    {
        public static bool Prefix(Shinies.ShinySpawn __instance, ref Shinies.ShinySpawn.Skin __result)
        {
            if(Main.IsModLoaded("shinyslimes"))
            {
                Identifiable.Id id = Identifiable.GetId(__instance.gameObject);

                if (id == Ids.ELECTRIC_SLIME || id == Ids.FORM_2_ELECTRIC_SLIME)
                {
                    int num = UnityEngine.Random.Range(1, 25);
                    bool flag = num == 1;
                    Shinies.ShinySpawn.Skin result = Shinies.ShinySpawn.Skin.Normal;
                    if (flag)
                    {
                        result = Shinies.ShinySpawn.Skin.Shiny;
                    }

                    __result = result;
                    return false;
                }
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(WeaponVacuum), "ExpelAmmo")]
    class DebugPatchesV1
    {
        public static bool Prefix(WeaponVacuum __instance, HashSet<GameObject> inVac)
        {

            GameObject selectedStored = __instance.player.Ammo.GetSelectedStored();
            if (selectedStored == null)
                return false;
            Identifiable component = selectedStored.GetComponent<Identifiable>();
            __instance.Expel(selectedStored, false);
            __instance.player.Ammo.DecrementSelectedAmmo(1);

            if (component != null)
            {
                __instance.tutDir.OnShoot(component.id);
            }

            __instance.ShootEffect();

            return false;
        }
    }

    /*[HarmonyPatch(typeof(WeaponVacuum), "Expel", new Type[] { typeof(GameObject), typeof(bool) })]
    unsafe class DebugPatches1
    {
        unsafe public static bool Prefix(WeaponVacuum __instance, GameObject toExpel, bool ignoreEmotions)
        {
            ///gameObject.PrintComponent();
            Console.Log(toExpel.name + " was shooted!");
            if (toExpel.name == "electricSlimeForm1" || toExpel.name == "electricSlimeForm2")
            {
                float GetSpeed(Identifiable.Id localId)
                {
                    if (localId - Identifiable.Id.VALLEY_AMMO_1 <= 3)
                    {
                        return __instance.ejectSpeed * 3f;
                    }
                    return __instance.ejectSpeed;
                }

                vp_FPController componentInParent = __instance.GetComponentInParent<vp_FPController>();
                Ray ray = new Ray(__instance.vacOrigin.transform.position, __instance.vacOrigin.transform.up);
                float num = PhysicsUtil.RadiusOfObject(toExpel);
                float num2 = (ray.direction.y >= 0f) ? 0f : (-0.5f * ray.direction.y);
                Vector3 vector = ray.origin + ray.direction * (num * 0.2f + num2);
                Vector3 velocity = ray.direction * GetSpeed(__instance.GetPrivateField<PlayerState>("player").Ammo.GetSelectedId()) + componentInParent.Velocity;
                vector = HelperClass.EnsureNotShootingIntoRock(vector, ray, num, ref velocity);
                GameObject gameObject = SRBehaviour.InstantiateActor(toExpel, __instance.GetPrivateField<RegionRegistry>("regionRegistry").GetCurrentRegionSetId(), vector, Quaternion.identity, false);
                gameObject.transform.LookAt(__instance.transform);
                PhysicsUtil.RestoreFreezeRotationConstraints(gameObject);
                SlimeEmotions component = gameObject.GetComponent<SlimeEmotions>();
                if (component != null && __instance.GetPrivateField<PlayerState>("player").Ammo.GetSelectedId() != Identifiable.Id.NONE && !ignoreEmotions)
                {
                    component.SetAll(__instance.GetPrivateField<PlayerState>("player").Ammo.GetSelectedEmotions());
                }
                gameObject.GetComponent<Rigidbody>().velocity = velocity;
                gameObject.transform.DOScale(gameObject.transform.localScale, 0.1f).From(gameObject.transform.localScale * 0.2f, true).SetEase(Ease.Linear);
                gameObject.GetComponent<Vacuumable>().Launch(Vacuumable.LaunchSource.PLAYER);

                Console.Log(__instance.GetPrivateField<RegionRegistry>("regionRegistry").GetCurrentRegionSetId().ToString());

                if (__instance.GetPrivateField<RegionRegistry>("regionRegistry").GetCurrentRegionSetId() == RegionRegistry.RegionSetId.VALLEY)
                {
                    Console.Log("YES");
                    if(toExpel.name == "electricSlimeForm2")
                    {
                        Console.Log("electricSlimeForm2 -> electricSlimeForm1");
                        GameObject newSlime = SRBehaviour.InstantiateActor(SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Ids.ELECTRIC_SLIME), __instance.GetPrivateField<RegionRegistry>("regionRegistry").GetCurrentRegionSetId(), vector, Quaternion.identity, false);
                        Console.Log("newSlime: " + newSlime);
                        newSlime.transform.LookAt(__instance.transform);
                        PhysicsUtil.RestoreFreezeRotationConstraints(newSlime);
                        SlimeEmotions slimeEmotions = newSlime.GetComponent<SlimeEmotions>();
                        if (slimeEmotions != null && __instance.GetPrivateField<PlayerState>("player").Ammo.GetSelectedId() != Identifiable.Id.NONE && !ignoreEmotions)
                        {
                            slimeEmotions.SetAll(__instance.GetPrivateField<PlayerState>("player").Ammo.GetSelectedEmotions());
                        }
                        newSlime.GetComponent<Rigidbody>().velocity = velocity;
                        newSlime.transform.DOScale(newSlime.transform.localScale, 0.1f).From(newSlime.transform.localScale * 0.2f, true).SetEase(Ease.Linear);
                        newSlime.GetComponent<Vacuumable>().Launch(Vacuumable.LaunchSource.PLAYER);
                        GameObject.Destroy(gameObject);
                    }
                    *//*if (gameObject.GetComponent<switchSlimeForm>() == null)
                        gameObject.AddComponent<switchSlimeForm>();
                    gameObject.GetComponent<switchSlimeForm>().idOfSlime = Ids.ELECTRIC_SLIME;
                    gameObject.GetComponent<switchSlimeForm>().switchToForm1();*//*
                }
                else
                {
                    Console.Log("NO");
                    if (toExpel.name == "electricSlimeForm1")
                    {
                        Console.Log("electricSlimeForm1 -> electricSlimeForm2");
                        GameObject newSlime = SRBehaviour.InstantiateActor(SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Ids.FORM_2_ELECTRIC_SLIME), __instance.GetPrivateField<RegionRegistry>("regionRegistry").GetCurrentRegionSetId(), vector, Quaternion.identity, false);
                        Console.Log("newSlime: " + newSlime);
                        newSlime.transform.LookAt(__instance.transform);
                        PhysicsUtil.RestoreFreezeRotationConstraints(newSlime);
                        SlimeEmotions slimeEmotions = newSlime.GetComponent<SlimeEmotions>();
                        if (slimeEmotions != null && __instance.GetPrivateField<PlayerState>("player").Ammo.GetSelectedId() != Identifiable.Id.NONE && !ignoreEmotions)
                        {
                            slimeEmotions.SetAll(__instance.GetPrivateField<PlayerState>("player").Ammo.GetSelectedEmotions());
                        }
                        newSlime.GetComponent<Rigidbody>().velocity = velocity;
                        newSlime.transform.DOScale(newSlime.transform.localScale, 0.1f).From(newSlime.transform.localScale * 0.2f, true).SetEase(Ease.Linear);
                        newSlime.GetComponent<Vacuumable>().Launch(Vacuumable.LaunchSource.PLAYER);
                        GameObject.Destroy(gameObject);
                    }


                    *//*if (gameObject.GetComponent<switchSlimeForm>() == null)
                        gameObject.AddComponent<switchSlimeForm>();
                    gameObject.GetComponent<switchSlimeForm>().idOfSlime = Ids.FORM_2_ELECTRIC_SLIME;
                    gameObject.GetComponent<switchSlimeForm>().switchToForm2();*//*
                }
                return false;
            }
            return true;
        }
    }*/

    [HarmonyPatch(typeof(SolarShieldUpgrader), "Apply")]
    class DebugPatches2
    {
        public static void Postfix(SolarShieldUpgrader __instance)
        {
            foreach(GameObject gameObject in __instance.shields)
            {
                Console.Log((gameObject.scene.name.Equals(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name).ToString()));
            }
        }
    }

    [HarmonyPatch(typeof(VortexAdder), "CanAdd")]
    class DebugPatches3
    {
        public static bool Prefix(VortexAdder __instance, GameObject gameObj)
        {
            Identifiable identifiable = gameObj.GetComponent<Identifiable>();
            if(identifiable != null)
            {
                if(identifiable.id == Identifiable.Id.VALLEY_AMMO_1 || identifiable.id == Identifiable.Id.VALLEY_AMMO_2)
                {
                    return false;
                }
            }
            return true;
        }
    }
    //ExchangeRewardItemEntryUI.SetEntry
    [HarmonyPatch(typeof(ExchangeRewardItemEntryUI), "SetEntry")]
    class DebugPatchesPre10
    {
        public static bool Prefix(ExchangeRewardItemEntryUI __instance, ExchangeDirector.ItemEntry entry)
        {
            //Console.Log("ExchangeRewardItemEntryUI.SetEntry Patch with entry being: " + (entry = null));
            /*Console.Log("entry: " + (entry = null));
            Console.Log("gameObject is: " + __instance.gameObject);
            Console.Log("Ok, everything ok here 1");*/
             
            if (entry == null)
            {
                __instance.gameObject.SetActive(false);
                return false;
            }
            __instance.gameObject.SetActive(true);

            if (entry.specReward != ExchangeDirector.NonIdentReward.NONE)
            {
                __instance.icon.sprite = __instance.exchangeDir.GetSpecRewardIcon(entry.specReward);
                __instance.amountText.text = __instance.GetCountDisplayForReward(entry.specReward);
                return false;
            }

            __instance.icon.sprite = __instance.lookupDir.GetIcon(entry.id);
            __instance.amountText.text = entry.count.ToString();
            return false;
        }
    }

    [HarmonyPatch(typeof(ExchangeDirector), "GetSpecRewardIcon")]
    class DebugPatches10
    {
        public static bool Prefix(ExchangeDirector __instance, ExchangeDirector.NonIdentReward specReward, ref Sprite __result)
        {
            if (__instance.nonIdentRewardDict.ContainsKey(specReward))
            {
                __result = __instance.nonIdentRewardDict[specReward];
            }
            else
            {
                __result = Main.assetBundle.LoadAsset<Sprite>("electricMeter");
            }
            return false;
        }
    }

    [HarmonyPatch(typeof(ExchangeDirector), "Update")]
    class DebugPatches4Dot1
    {
        public static bool Prefix(ExchangeDirector __instance)
        {
            if (!__instance.worldModel.currOffers.ContainsKey(ExchangeDirector.OfferType.MOCHI_RECUR) && (__instance.worldModel.currOffers.ContainsKey(ExchangeDirector.OfferType.MOCHI) || __instance.progressDir.GetProgress(ProgressDirector.ProgressType.MOCHI_REWARDS) >= 4f))
            {
                __instance.worldModel.currOffers[ExchangeDirector.OfferType.MOCHI_RECUR] = __instance.CreateMochiRecurOffer();
                __instance.OfferDidChange();
                return false;
            }

            return true;
        }
    }

    /*    [HarmonyPatch(typeof(ExchangeDirector), "MaybeStartNext")]
        class DebugPatches4
        {
            public static bool Prefix(ExchangeDirector __instance, ExchangeDirector.OfferType offerType, ref bool __result)
            {
                Console.Log("ExchangeDirector.MaybeStartNext Patch");

                __result = false;

                if (offerType == ExchangeDirector.OfferType.GENERAL || offerType.ToString().Contains("_RECUR"))
                    return true;

                ExchangeDirector.ProgressOfferEntry progressEntry = __instance.GetProgressEntry(offerType);

                *//*Console.Log(offerType.ToString());
                Console.Log((progressEntry != null).ToString());
                Console.Log((!__instance.worldModel.currOffers.ContainsKey(progressEntry.specialOfferType)).ToString());
                Console.Log((__instance.progressDir.GetProgress(progressEntry.progressType)).ToString());

                bool flag = ExchangeCreator.MaybeAddRewardLevels(progressEntry) 
                    ? 
                    __instance.progressDir.GetProgress(progressEntry.progressType) < progressEntry.rewardLevels.Length + ExchangeCreator.moddedEntries.Get(progressEntry.specialOfferType).Count
                    : 
                    __instance.progressDir.GetProgress(progressEntry.progressType) < progressEntry.rewardLevels.Length
                    ;

                if (progressEntry != null && !__instance.worldModel.currOffers.ContainsKey(progressEntry.specialOfferType) && flag)
                {
                    ExchangeDirector.RewardLevel[] rewardLevels = __instance.progressDir.GetProgress(progressEntry.progressType) < 3 ? progressEntry.rewardLevels : ExchangeCreator.moddedEntries.Get(offerType).ToArray();
                    __instance.worldModel.currOffers[progressEntry.specialOfferType] = __instance.CreateProgressOffer(progressEntry.specialOfferType, progressEntry.progressType, rewardLevels);
                    __instance.OfferDidChange();
                    __result = __instance.CreateRancherChatUI(offerType, true);
                }*//*

                bool flag = progressEntry.progressType == ProgressDirector.ProgressType.MOCHI_REWARDS ? 
                    __instance.progressDir.GetProgress(progressEntry.progressType) < progressEntry.rewardLevels.Length
                    : 
                    __instance.progressDir.GetProgress(progressEntry.progressType) < progressEntry.rewardLevels.Length;

                if (progressEntry != null && !__instance.worldModel.currOffers.ContainsKey(progressEntry.specialOfferType) && flag)
                {
                    ExchangeDirector.RewardLevel[] rewardLevels = __instance.progressDir.GetProgress(progressEntry.progressType) < 3 ? progressEntry.rewardLevels : ExchangeCreator.moddedEntries.Get(offerType).ToArray();
                    __instance.worldModel.currOffers[progressEntry.specialOfferType] = __instance.CreateProgressOffer(progressEntry.specialOfferType, progressEntry.progressType, rewardLevels);
                    __instance.OfferDidChange();
                    __result = __instance.CreateRancherChatUI(offerType, true);
                }

                return false;
            }
        }*/

    /*[HarmonyPatch(typeof(ExchangeDirector), "GetRancherChatMetadata")]
    class DebugPatchesIdk
    {
        public static bool Prefix(ExchangeDirector __instance, ExchangeDirector.OfferType offerType, bool intro, ref RancherChatMetadata __result)
        {
            Console.Log("ExchangeDirector.GetRancherChatMetadata Patch");

            if(offerType == ExchangeDirector.OfferType.MOCHI_RECUR)
            {

                if (__instance.progressDir.GetProgress(ProgressDirector.ProgressType.MOCHI_REWARDS) >= 3)
                {
                    intro = __instance.progressDir.SetUniqueProgress(ProgressDirector.ProgressType.MOCHI_SEEN_FINAL_CHAT);
                    if (!intro)
                    {
                        __result = __instance.CreateRancherChat("mochi", new string[] { "I am the best one, the best of the bests", "Cyall losers" });
                    }
                    __result = __instance.CreateRancherChat("mochi", new string[] { "I suppose this is a trap?", "Well, then..." });
                    return false;
                }

            }

            return true;
        }
    }*/

    /*    [HarmonyPatch(typeof(ExchangeDirector), "CreateProgressOffer")]
        class DebugPatches5
        {
            public static bool Prefix(ExchangeDirector __instance, ExchangeDirector.OfferType offerType, ProgressDirector.ProgressType progressType, ExchangeDirector.RewardLevel[] rewardLevels, ref ExchangeDirector.Offer __result)
            {
                Console.Log("ExchangeDirector.CreateProgressOffer Patch");
                int num = __instance.progressDir.GetProgress(progressType) + 1;
                Console.Log("num setted");
                Console.Log("Progress is: " + __instance.progressDir.GetProgress(progressType) + ", aka: " + (__instance.progressDir.GetProgress(progressType) < 3) + " and the length is: " + rewardLevels.Length);

                if(__instance.progressDir.GetProgress(progressType) >= 3)
                {
                    foreach (ExchangeDirector.RewardLevel reward in rewardLevels)
                    {
                        Console.Log(reward.requestedItem.ToString());
                    }
                }

                bool modded = __instance.progressDir.GetProgress(progressType) < 3;
                ExchangeDirector.RewardLevel rewardLevel = modded ? rewardLevels[num - 1] : rewardLevels[num - 4]; //If the progress is a vanilla one (from  0 to 2, so 3 quests in total) then dont change the code snippet. Else, make it "rewardLevels[num - 4]" for the modded quests (if you're doing your 4th quest, then num will be 4, so 4 - 4 and you get 0, that means the first modded quest)
                Console.Log("rewardLevel");
                string offerId = string.Concat(new object[]
                {
                    "m.offer.",
                    modded ? progressType.ToString().ToLowerInvariant() : progressType.ToString().ToLowerInvariant() + "_modded",
                    "_level",
                    num
                });
                List<ExchangeDirector.RequestedItemEntry> list = new List<ExchangeDirector.RequestedItemEntry>();
                list.Add(new ExchangeDirector.RequestedItemEntry(rewardLevel.requestedItem, rewardLevel.count, 0));
                List<ExchangeDirector.ItemEntry> list2 = new List<ExchangeDirector.ItemEntry>();
                list2.Add(new ExchangeDirector.ItemEntry(rewardLevel.reward));

                Console.Log(offerId + ", " + rewardLevel.requestedItem + ", " + rewardLevel.count + ", " + rewardLevel.reward);

                __result = new ExchangeDirector.Offer(offerId, offerType.ToString().ToLowerInvariant(), double.PositiveInfinity, double.NegativeInfinity, list, list2);
                Console.Log("__result setted");
                return false;
            }
        }*/

    [HarmonyPatch(typeof(ExchangeDirector), "CreateProgressOffer")]
    class DebugPatches5
    {
        public static bool Prefix(ExchangeDirector __instance, ExchangeDirector.OfferType offerType, ProgressDirector.ProgressType progressType, ExchangeDirector.RewardLevel[] rewardLevels, ref ExchangeDirector.Offer __result)
        {
            int num = __instance.progressDir.GetProgress(progressType) + 1;

            foreach (ExchangeDirector.RewardLevel reward in rewardLevels)
            {
                Console.Log(reward.reward.ToString());
            }
            
            ExchangeDirector.RewardLevel rewardLevel = rewardLevels[num - 1]; //If the progress is a vanilla one (from  0 to 2, so 3 quests in total) then dont change the code snippet. Else, make it "rewardLevels[num - 4]" for the modded quests (if you're doing your 4th quest, then num will be 4, so 4 - 4 and you get 0, that means the first modded quest)
            string offerId = string.Concat(new object[]
            {
                "m.offer.",
                progressType.ToString().ToLowerInvariant(),
                "_level",
                num
            });
            List<ExchangeDirector.RequestedItemEntry> list = new List<ExchangeDirector.RequestedItemEntry>();
            list.Add(new ExchangeDirector.RequestedItemEntry(rewardLevel.requestedItem, rewardLevel.count, 0));
            List<ExchangeDirector.ItemEntry> list2 = new List<ExchangeDirector.ItemEntry>();
            list2.Add(new ExchangeDirector.ItemEntry(rewardLevel.reward));

            Console.Log(offerId + ", " + rewardLevel.requestedItem + ", " + rewardLevel.count + ", " + rewardLevel.reward);

            __result = new ExchangeDirector.Offer(offerId, offerType.ToString().ToLowerInvariant(), double.PositiveInfinity, double.NegativeInfinity, list, list2);
            return false;
        }
    }

    /*[HarmonyPatch(typeof(ExchangeAcceptor), "Awake")]
    class DebugPatches6
    {
        public static void Postfix(ExchangeAcceptor __instance)
        {

            __instance.awarders.PrintContent();

            foreach(var award in __instance.awarders)
            {
                Console.Log("");
            }
        }
    }*/

    [HarmonyPatch(typeof(UITemplates), "CreatePurchaseUI")]
    class DebugPatches7
    {
        public static void Postfix(UITemplates __instance, Sprite titleIcon, string titleKey, PurchaseUI.Purchasable[] purchasables, bool hideNubuckCost, PurchaseUI.OnClose onClose, bool unavailInMainList = false)
        {
            Console.Log("UITemplates.CreatePurchaseUI Patch");
            Console.Log("titleKey: " + titleKey);
            Console.Log("Parent to: " + __instance.transform.parent);
        }
    }

    [HarmonyPatch(typeof(RancherProgressAwarder), "DoAward")]
    class DebugPatches8
    {
        public static void Postfix(RancherProgressAwarder __instance)
        {
            Console.Log("RancherProgressAwarder Patch");
            Console.Log(__instance.offerType + " Quest Resetted");
            if(__instance.offerType == ExchangeDirector.OfferType.MOCHI && SceneContext.Instance.ProgressDirector.GetProgress(__instance.progressType) >= 3)
            {
                SlimeDefinition slimeDefinition = GameContext.Instance.SlimeDefinitions.GetSlimeByIdentifiableId(Ids.ELECTRIC_SLIME);
                SlimeAppearance secretAppearance = Main.SecretStyleList[Ids.ELECTRIC_SLIME].Get(slimeDefinition);

                SceneContext.Instance.SlimeAppearanceDirector.UnlockAppearance(slimeDefinition, secretAppearance);
            }
        }
    }

    /*[HarmonyPatch(typeof(ExchangeDirector), "Awake")]
    class DebugPatches9
    {
        public static void Prefix(ExchangeDirector __instance)
        {
            Console.Log("ExchangeDirector.Awake Patch");
        }
    }*/


    /*[HarmonyPatch(typeof(ExchangeChatActivator), "Activate")]
    class DebugPatches11
    {
        public static bool Prefix(ExchangeChatActivator __instance)
        {
            Console.Log("ExchangeChatActivator.Activate Patch");

            foreach (ExchangeDirector.OfferType offerType in __instance.offerTypes)
            {
                if (offerType == ExchangeDirector.OfferType.MOCHI)
                {
                    if (__instance.exchangeDir.MaybeStartNext(offerType) || __instance.exchangeDir.CreateRancherChatUI(offerType, false))
                    {

                        return false;
                    }
                }
            }

            return true;
        }
    }*/
}