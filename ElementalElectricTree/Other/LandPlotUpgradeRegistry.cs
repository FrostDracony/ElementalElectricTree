using SRML.SR.SaveSystem;
using System;
using SRML.SR;
using UnityEngine;

namespace SRML
{
    public static class LandPlotUpgradeRegistry
    {
        internal static IDRegistry<LandPlot.Upgrade> moddedUpgrades = new IDRegistry<LandPlot.Upgrade>();


        static LandPlotUpgradeRegistry()
        {
            ModdedIDRegistry.RegisterIDRegistry(LandPlotUpgradeRegistry.moddedUpgrades);
            ModdedIDRegistry.RegisterIDRegistry(moddedUpgrades);
        }

        public static LandPlot.Upgrade CreateLandPlotUpgrade(object value, string name)
        {
            if (SRModLoader.CurrentLoadingStep > SRModLoader.LoadingStep.PRELOAD)
                throw new Exception("Can't register landplot upgrades outside of the PreLoad step");
            return moddedUpgrades.RegisterValueWithEnum((LandPlot.Upgrade)value, name);
        }

/*        public static void RegisterPurchasableUpgrade<T>(SRML.SR.LandPlotUpgradeRegistry.UpgradeShopEntry entry, bool unlocked = true) where T : LandPlotUI
        {
            PurchasableUIRegistry.RegisterPurchasable<T>((x) => new PurchaseUI.Purchasable(entry.NameKey, entry.icon, entry.mainImg, entry.DescKey, entry.cost, entry.landplotPediaId, () =>
            {
                x.Upgrade(entry.upgrade, entry.cost); ;
            }, entry.isUnlocked ?? (() => true), () => !x.activator.HasUpgrade(entry.upgrade), null, null, null, null, false));
        }*/

        public static void RegisterPurchasableUpgrade<T>(SRML.SR.LandPlotUpgradeRegistry.UpgradeShopEntry entry) where T : LandPlotUI
        {
            PurchasableUIRegistry.RegisterPurchasable<T>((x) => new PurchaseUI.Purchasable(entry.NameKey, entry.icon, entry.mainImg, entry.DescKey, entry.cost, entry.landplotPediaId, () =>
            {
                x.Upgrade(entry.upgrade, entry.cost); ;
            }, entry.isUnlocked ?? (() => false), () => !x.activator.HasUpgrade(entry.upgrade), null, null, null, null, false));
        }

        public static void RegisterPlotUpgrader<T>(LandPlot.Id plot) where T : PlotUpgrader
        {
            switch (SRModLoader.CurrentLoadingStep)
            {
                case SRModLoader.LoadingStep.PRELOAD:
                    SRCallbacks.OnGameContextReady += () =>
                    {
                        GameContext.Instance.LookupDirector.GetPlotPrefab(plot).AddComponent<T>();
                    };
                    break;
                default:
                    GameContext.Instance.LookupDirector.GetPlotPrefab(plot).AddComponent<T>();
                    break;
            }
        }

        public struct UpgradeShopEntry
        {
            public LandPlot.Upgrade upgrade;
            public Sprite icon;
            public Sprite mainImg;
            public int cost;
            public PediaDirector.Id landplotPediaId;
            public Func<bool> isUnlocked;

            string landplotName;

            public string LandPlotName
            {
                get
                {
                    if (landplotName != null) return landplotName;
                    return landplotPediaId.ToString().ToLower();
                }
                set
                {
                    landplotName = value.ToLower();
                }
            }

            public string DescKey => $"m.upgrade.desc.{landplotName}.{upgrade.ToString().ToLower()}";
            public string NameKey => $"m.upgrade.name.{landplotName}.{upgrade.ToString().ToLower()}";
        }
    }
}