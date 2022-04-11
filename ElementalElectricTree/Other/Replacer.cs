using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using SRML;
using SRML.Console;
using SRML.SR;
using SRML.SR.SaveSystem;
using SRML.SR.Utils;
using SRML.Utils;
using UnityEngine;
using UnityEngine.Serialization;
using Console = SRML.Console.Console;
using Object = UnityEngine.Object;

namespace ElementalElectricTree.Other
{
    public enum IdHandlerEnum
    {
        None,
        LandPlotLocation,
        QuicksilverEnergyGenerator,
        DecorizerStorage,
        GordoEat,
        PhaseSite,
        HobsonNote,
        AccessDoor,
        GadgetSite,
        LiquidSource,

    }

    public class Replacer : MonoBehaviour
    {
        private IdDirector _idDirector;
        public IdDirector GlobalIdDirector
        {
            get
            {
                if (!(bool)_idDirector)
                {
                    _idDirector = GetComponentInParent<IdDirector>() ? GetComponentInParent<IdDirector>() : IdHandlerUtils.GlobalIdDirector;
                }
                return _idDirector;
            }
        }

        public static Dictionary<IdHandlerEnum, int> IDHandlerEnums = new Dictionary<IdHandlerEnum, int>
        {
            {IdHandlerEnum.GordoEat, 0},
            {IdHandlerEnum.LiquidSource, 0},
            {IdHandlerEnum.AccessDoor, 0},
            {IdHandlerEnum.GadgetSite, 0},
            {IdHandlerEnum.HobsonNote, 0}

        };


        public void OnDestroy()
        {
        }


        public void BuildIdHandler(string id)
        {
            LiquidSource[] liquidSources = GetComponentsInChildren<LiquidSource>();
            GordoEat[] gordoEats = GetComponentsInChildren<GordoEat>();
            AccessDoor[] accessDoors = GetComponentsInChildren<AccessDoor>();
            foreach (var accessDoor in accessDoors)
            {
                accessDoor.director = this.GlobalIdDirector;
                accessDoor.director.persistenceDict.Add(accessDoor, ModdedStringRegistry.ClaimID(accessDoor.IdPrefix(), id));
                gameObject.SetActive(true);
                accessDoor.director.persistenceDict.Remove(accessDoor);



            }
            foreach (var liquidSource in liquidSources)
            {
                liquidSource.director = this.GlobalIdDirector;
                liquidSource.director.persistenceDict.Add(liquidSource, ModdedStringRegistry.ClaimID(liquidSource.IdPrefix(), id));
                gameObject.SetActive(true);
                liquidSource.director.persistenceDict.Remove(liquidSource);


            }
            foreach (var gordoEat in gordoEats)
            {
                gordoEat.director = this.GlobalIdDirector;
                gordoEat.director.persistenceDict.Add(gordoEat, ModdedStringRegistry.ClaimID(gordoEat.IdPrefix(), id));
                gameObject.SetActive(true);
                gordoEat.director.persistenceDict.Remove(gordoEat);


            }



        }

        public GameObject BuildAccessDoor()
        {

            IDHandlerEnums[IdHandlerEnum.AccessDoor] += 1;
            var accessDoor = GetComponent<AccessDoor>();
            accessDoor.director = this.GlobalIdDirector;
            accessDoor.director.persistenceDict.Add(accessDoor, ModdedStringRegistry.ClaimID(accessDoor.IdPrefix(), IDHandlerEnums[IdHandlerEnum.AccessDoor].ToString()));


            return gameObject;

        }

        public GameObject BuildHobsonNote(string hobson)
        {

            IDHandlerEnums[IdHandlerEnum.HobsonNote] += 1;
            foreach (var journalEntry in GetComponentsInChildren<JournalEntry>())
            {
                journalEntry.entryKey = hobson;

            }



            return this.gameObject;
        }

        public GameObject BuildTeleport(string destination, string source)
        {
            foreach (var teleportSource in GetComponentsInChildren<TeleportSource>())
            {
                teleportSource.destinationSetName = source;
            }

            foreach (var teleportDestination in GetComponentsInChildren<TeleportDestination>())
            {
                teleportDestination.teleportDestinationName = destination;
            }
            return gameObject;
        }

        public GameObject BuildLiquidSource()
        {
            IDHandlerEnums[IdHandlerEnum.LiquidSource] += 1;

            foreach (var liquidSource in GetComponentsInChildren<LiquidSource>())
            {
                liquidSource.director = this.GlobalIdDirector;
                liquidSource.director.persistenceDict.Add(liquidSource, ModdedStringRegistry.ClaimID(liquidSource.IdPrefix(), IDHandlerEnums[IdHandlerEnum.LiquidSource].ToString()));

            }
            return this.gameObject;

        }


        public GameObject BuildGordo()
        {
            IDHandlerEnums[IdHandlerEnum.GordoEat] += 1;
            var gordoEat = GetComponent<GordoEat>();
            gordoEat.director = GlobalIdDirector;
            string id = ModdedStringRegistry.ClaimID(gordoEat.IdPrefix(), IDHandlerEnums[IdHandlerEnum.GordoEat].ToString());
            gordoEat.director.persistenceDict.Add(gordoEat, id);



            return gameObject;
        }
        public GameObject BuildGordo(Identifiable.Id gordoId)
        {


            IDHandlerEnums[IdHandlerEnum.GordoEat] += 1;
            var gordoPrefab = GameObjectUtils.InstantiateInactive(SRSingleton<GameContext>.Instance.LookupDirector.GetGordo(gordoId));
            gordoPrefab.transform.position = transform.position;
            gordoPrefab.transform.parent = transform;
            gordoPrefab.transform.rotation = transform.rotation;
            var idHandler = gordoPrefab.GetComponent<GordoEat>();
            idHandler.director = this.GlobalIdDirector;
            string id = ModdedStringRegistry.ClaimID(idHandler.IdPrefix(), IDHandlerEnums[IdHandlerEnum.GordoEat].ToString());
            idHandler.director.persistenceDict.Add(idHandler, id);
            gordoPrefab.SetActive(true);



            return gordoPrefab;
        }

        public GameObject BuildGadgetSite()
        {
            IDHandlerEnums[IdHandlerEnum.GadgetSite] += 1;
            var gadgetSite = gameObject.GetComponent<GadgetSite>();
            gadgetSite.director = this.GlobalIdDirector;
            gadgetSite.director.persistenceDict.Add(gadgetSite, ModdedStringRegistry.ClaimID(gadgetSite.IdPrefix(), IDHandlerEnums[IdHandlerEnum.GadgetSite].ToString()));
            return this.gameObject;
        }

    }

}