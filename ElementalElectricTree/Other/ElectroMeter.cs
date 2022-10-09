using Creators;
using UnityEngine;
using SRML.Console;
using ElementalElectricTree.Other;

namespace ElementalElectricTree
{
    public class ElectroMeter
    {
        public class Upgrade : PlotUpgrader
        {
            public override void Apply(LandPlot.Upgrade upgrade)
            {
                Console.Log("Applying Upgrade: " + upgrade);
                if (upgrade == Ids.ELECTROMETER)
                {
                    Console.Log("CUSTOM CORRAL UPGRADE");

                    GameObject ElectroMeter = Instantiate(Main.assetBundle.LoadAsset<GameObject>("ElectroMeter"), gameObject.transform);
                    ElectroMeter.SetActive(true);

                    ElectroMeter.FindChild("ElectroMeter Region").AddComponent<ElectroMeterRegion>();
                }
            }
        }
        

        public static SRML.SR.LandPlotUpgradeRegistry.UpgradeShopEntry CreateElectricContainerEntry()
        {
            return new SRML.SR.LandPlotUpgradeRegistry.UpgradeShopEntry
            {
                icon = Main.assetBundle.LoadAsset<Sprite>("electricMeter"),
                upgrade = Ids.ELECTROMETER,
                mainImg = Main.assetBundle.LoadAsset<Sprite>("electricMeter"),
                cost = 10000,
                landplotPediaId = PediaDirector.Id.CORRAL,
                isUnlocked = plot => 
                {
                    ("Progress with Mochi: " + SceneContext.Instance.ExchangeDirector.progressDir.GetProgress(SceneContext.Instance.ExchangeDirector.GetProgressEntry(ExchangeDirector.OfferType.MOCHI).progressType)).Log();
                    if(SceneContext.Instance.ExchangeDirector.progressDir.GetProgress(SceneContext.Instance.ExchangeDirector.GetProgressEntry(ExchangeDirector.OfferType.MOCHI).progressType) == 4)
                    {
                        return true;
                    }
                        
                    return false;
                },
                LandPlotName = "corral"
            };
        }
    }
}