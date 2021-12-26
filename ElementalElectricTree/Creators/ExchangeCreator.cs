using System;
using System.Collections.Generic;
using System.Linq;
using Console = SRML.Console.Console;
using UnityEngine;
namespace Creators
{
    static class ExchangeCreator
    {
        public static List<RancherChatMetadata.Entry> finalEntriesIntro = new List<RancherChatMetadata.Entry>();
        public static List<RancherChatMetadata.Entry> finalEntriesRepeat = new List<RancherChatMetadata.Entry>();
        public static List<RancherChatMetadata.Entry> entriesAtEndingIntro = new List<RancherChatMetadata.Entry>();
        public static List<RancherChatMetadata.Entry> entriesAtEndingRepeat = new List<RancherChatMetadata.Entry>();
        public static ExchangeDirector exchangeDirector = SRSingleton<SceneContext>.Instance.ExchangeDirector;
        public static Dictionary<ExchangeDirector.OfferType, List<ExchangeDirector.RewardLevel>> moddedEntries = new Dictionary<ExchangeDirector.OfferType, List<ExchangeDirector.RewardLevel>>();
        public static Dictionary<ExchangeDirector.NonIdentReward, Sprite> moddedSpecRewardIcons = new Dictionary<ExchangeDirector.NonIdentReward, Sprite>();

        public static ExchangeDirector.RewardLevel CreateRewardLevel(int cnt, RancherChatMetadata intro, RancherChatMetadata repeat, Identifiable.Id item, ExchangeDirector.NonIdentReward rew)
        {
            return new ExchangeDirector.RewardLevel()
            { 
                count = cnt,
                rancherChatIntro = intro,
                rancherChatRepeat = repeat,
                requestedItem = item,
                reward = rew
            };
        }

        public static bool MaybeAddRewardLevels(ExchangeDirector.ProgressOfferEntry progressOfferEntry)
        {
            if (moddedEntries.ContainsKey(progressOfferEntry.specialOfferType))
            {
                return true;
            }
            return false;
        }

        public static void CreateRancherQuest(ExchangeDirector.OfferType rancherOffer, int count, Identifiable.Id requestedItem,
            ExchangeDirector.NonIdentReward reward, RancherChatMetadata rancherChatMetadataRepeat, RancherChatMetadata rancherChatMetadataIntro, RancherChatMetadata.Entry[] messagesAtEndRepeat, 
            RancherChatMetadata.Entry[] messagesAtEndIntro, Sprite spriteToRegister, bool finalQuest = false)
        {
            Console.Log("REGISTERING RANCHER QUEST");
            foreach (ExchangeDirector.ProgressOfferEntry progressOfferEntry in exchangeDirector.progressOffers)
            {
                Console.Log("Loop initiated with: " + progressOfferEntry.specialOfferType);
                if (progressOfferEntry.specialOfferType == rancherOffer)
                {
                    List<ExchangeDirector.RewardLevel> allModdedRewards = new List<ExchangeDirector.RewardLevel>();
                    Console.Log("allModdedRewards setted");
                    Console.Log("moddedEntries dosent exist, lets see:");
                    Console.Log((moddedEntries == null).ToString());
                    Console.Log("oh wait, maybe it exists, tho");
                    if (moddedEntries.ContainsKey(rancherOffer))
                    {
                        Console.Log("moddedEntries contains the key");
                        allModdedRewards = moddedEntries.Get(rancherOffer);
                        Console.Log("allModdedRewards assigned");
                    }
                    else
                    {
                        Console.Log("moddedEntries dosent contains the key");
                        moddedEntries.Add(rancherOffer, allModdedRewards);
                        Console.Log("allModdedRewards not setted");
                    }

                    ExchangeDirector.RewardLevel newRewardLevel = new ExchangeDirector.RewardLevel();
                    Console.Log("new rewardLevel");
                    newRewardLevel.requestedItem = requestedItem;
                    Console.Log("requestedItem setted");
                    newRewardLevel.reward = reward;
                    Console.Log("Reward setted");
                    newRewardLevel.count = count;
                    Console.Log("Count setted");
                    newRewardLevel.rancherChatRepeat = rancherChatMetadataRepeat;
                    Console.Log("RancherChatRepeat setted");
                    newRewardLevel.rancherChatIntro = rancherChatMetadataIntro;
                    Console.Log("RancherChatIntro etted");

                    Console.Log("finalQuest, is it the fault of the error?");
                    Console.Log("finalQuest: " + finalQuest);
                    Console.Log("Uh nope, not the fault of finalQuest");

                    if (!finalQuest)
                    {
                        Console.Log("finalQuest is true ");
                        if (entriesAtEndingIntro.Count > 0)
                        {
                            Console.Log("entriesAtEndingIntro.Count: " + entriesAtEndingIntro.Count);
                            Console.Log("newRewardLevel: " + (newRewardLevel != null));
                            Console.Log("   " + newRewardLevel);
                            Console.Log("rancherChatIntro: " + (newRewardLevel.rancherChatIntro != null));
                            Console.Log("   " + newRewardLevel.rancherChatIntro);
                            Console.Log("entries: " + (newRewardLevel.rancherChatIntro.entries != null) + ", " + newRewardLevel.rancherChatIntro.entries.Length);

                            Array.ForEach(newRewardLevel.rancherChatIntro.entries, x => Console.Log("   " + x.messageText));

                            entriesAtEndingIntro.ForEach(x => {
                                Console.Log("x: " + (x != null));
                                Console.Log("   " + x.messageText);
                                newRewardLevel.rancherChatIntro.entries.Prepend(x);
                            });
                            entriesAtEndingIntro.Clear();
                        }

                        if (entriesAtEndingRepeat.Count > 0)
                        {
                            entriesAtEndingRepeat.ForEach(x => newRewardLevel.rancherChatRepeat.entries.Prepend(x));
                            entriesAtEndingRepeat.Clear();
                        }

                        Array.ForEach(messagesAtEndIntro, x => entriesAtEndingIntro.Add(x));
                        Array.ForEach(messagesAtEndRepeat, x => entriesAtEndingRepeat.Add(x));
                    }
                    else
                    {
                        Console.Log("finalQuest is false");
                        ExchangeDirector.OfferType offerType = (ExchangeDirector.OfferType)Enum.Parse(typeof(ExchangeDirector.OfferType), rancherOffer.ToString() + "_RECUR");
                        Console.Log("offerType: " + offerType);

                        Array.ForEach(messagesAtEndIntro, x => exchangeDirector.GetProgressEntry(rancherOffer).rancherChatEndIntro.entries.Prepend(x));
                        Array.ForEach(messagesAtEndRepeat, x => exchangeDirector.GetProgressEntry(rancherOffer).rancherChatEndRepeat.entries.Prepend(x));

                        finalEntriesIntro.Clear();
                        finalEntriesRepeat.Clear();

                        Array.ForEach(messagesAtEndIntro, x => finalEntriesIntro.Append(x));
                        Array.ForEach(messagesAtEndRepeat, x => finalEntriesRepeat.Append(x));

                        if (entriesAtEndingIntro.Count > 0)
                        {
                            entriesAtEndingIntro.ForEach(x => newRewardLevel.rancherChatIntro.entries.Prepend(x));
                            entriesAtEndingIntro.Clear();
                        }

                        if (entriesAtEndingRepeat.Count > 0)
                        {
                            entriesAtEndingRepeat.ForEach(x => newRewardLevel.rancherChatRepeat.entries.Prepend(x));
                            entriesAtEndingRepeat.Clear();
                        }

                    }

                    if (allModdedRewards.Contains(newRewardLevel))
                    {
                        Console.Log("Well, that already exists, so lets not create it again");
                        return;
                    }
                    Console.Log("Nothing abnormal found, so let's continue");
                    allModdedRewards.Add(newRewardLevel);
                    Console.Log("NewRewardLevel added to allModdedRewards");

                    moddedSpecRewardIcons.Add(reward, spriteToRegister);
                }
            }
        }

        public static ExchangeDirector.Offer CreateOffer(string rancherId, int num)
        {
            return exchangeDirector.offerGenerators[rancherId].Generate(exchangeDirector,
                exchangeDirector.CreateWhiteList(),
                exchangeDirector.timeDir.GetNextHourAtLeastHalfDay(12f),
                exchangeDirector.timeDir.HoursFromNow(2f), num, false, 
                SRSingleton<SceneContext>.Instance.GameModeConfig.GetModeSettings().exchangeRewardsGoldPlorts);
        }
    }
}
