using System;
using ElementalElectricTree.Other;
using HarmonyLib;
using UnityEngine;
using Console = SRML.Console.Console;

namespace ElementalElectricTree.Patches
{
    [HarmonyPatch(typeof(ReactToShock), "Update")]
    unsafe class PatchUpdateReact
    {
        unsafe public static bool Prefix(ReactToShock __instance)
        {
            if (__instance.ToString() != "slimeQuicksilver(Clone) (ReactToShock)")
            {
                return false;
            }
            return true;
        }
    }

    /*[HarmonyPatch(typeof(DestroyAndShockOnTouching), "DestroyAndShock")]
    unsafe class PatchDestroyAndShock
    {
        unsafe public static void Postfix(DestroyAndShockOnTouching __instance)
        {
            if (__instance.shockRadius > 0f)
            {
                Console.Log("Before the OverlapTrigger");
                bool slimeAlreadyCreated = false;
                SphereOverlapTrigger.CreateGameObject(__instance.transform.position, __instance.shockRadius, delegate (IEnumerable<Collider> colliders)
                {
                    Console.Log("");
                    Console.Log("");
                    Console.Log("");
                    Console.Log("Beginning");
                    if (slimeAlreadyCreated)
                        return;
                    Console.Log("Processing");

                    HashSet<ReactToShock> hashSet = new HashSet<ReactToShock>();
                    SlimeAppearance shocked = null;

                    foreach (Collider collider in colliders)
                    {
                        foreach (ReactToShock item in collider.gameObject.GetComponentsInParent<ReactToShock>())
                        {
                            hashSet.Add(item);
                        }
                    }

                    hashSet.ToArray().PrintContent<ReactToShock>();

                    ReactToShock quickSilverSlime = null;
                    ReactToShock goldSlime = null;

                    Console.Log("Prepare to error");
                    quickSilverSlime = hashSet.FirstOrDefault(x => x.ToString() == "slimeQuicksilver(Clone) (ReactToShock)");
                    goldSlime = hashSet.FirstOrDefault(x => x.ToString() == "slimeGold(Clone) (ReactToShock)");

                    if(quickSilverSlime != null)
                        shocked = quickSilverSlime.slimeAppearanceApplicator.Appearance;

                    *//* Console.Log("Actual Appearance: " + quickSilverSlime.slimeAppearanceApplicator.Appearance.ToString());
                     Console.Log("Actual Shocked Appearance: " + quickSilverSlime.slimeAppearanceApplicator.Appearance.ShockedAppearance.ToString());*//*
                    Console.Log("QUICKSILVER SLIME: " + (quickSilverSlime != null));
                    Console.Log("GOLD SLIME: " + (goldSlime != null));
                    Console.Log("SHOCKED APPEARANCE: " + (shocked != null));
                    Console.Log("ALREADY CREATED: " + slimeAlreadyCreated);
                    *//*if (quickSilverSlime.slimeAppearanceApplicator.Appearance.ToString() == "ShockedQuicksilverNormal (SlimeAppearance)")
                        shocked = quickSilverSlime.slimeAppearanceApplicator.Appearance;

                    if (quickSilverSlime.slimeAppearanceApplicator.Appearance.ShockedAppearance.ToString() == "ShockedQuicksilverNormal(Appearance)")
                        shocked = quickSilverSlime.slimeAppearanceApplicator.Appearance.ShockedAppearance;
                    *//*

                    if (quickSilverSlime != null && goldSlime != null && shocked != null && !slimeAlreadyCreated)
                    {
                        slimeAlreadyCreated = true;
                        Custom_Slime_Creator.CreateElectricSlime(quickSilverSlime, goldSlime, quickSilverSlime.slimeAppearanceApplicator.Appearance);
                        quickSilverSlime = null;
                        goldSlime = null;
                        shocked = null;

                        hashSet.Clear();

                        return;
                    }
                    Console.Log("Problem with creating electric slimes");
                }, 15);
            }
        }
    }*/

    [HarmonyPatch(typeof(ReactToShock), "MaybeCreatePlorts", new Type[] { typeof(int), typeof(ReactToShock.PlortSounds) })]
    class PatchCreatePlorts
    {
        public static bool Prefix(ReactToShock __instance)
        {
            /*if(__instance.ToString() == "electricSlime(Clone) (ReactToShock)")
            {
                //Vector3 targetPosition = Camera.main.transform.position;
                Vector3 targetPosition = SRSingleton<SceneContext>.Instance.Player.transform.position+new Vector3(0,2,0);
                //Abilities.CreateShoot(__instance.gameObject.transform.position + new Vector3(0, 3, 0), targetPosition - (__instance.gameObject.transform.position + new Vector3(0, 3, 0)), __instance.regionMember.setId);
                Abilities.CreateShoot(__instance.gameObject.transform.position + new Vector3(0, 3, 0), targetPosition - (__instance.gameObject.transform.position + new Vector3(0, 3, 0)), __instance.regionMember.setId);
                return false;
            }*/
            Console.Log("Trying to produce Plorts. The slime is: " + __instance);
            if (__instance.ToString() == "slimeQuicksilver(Clone) (ReactToShock)")
            {
                Console.Log("Allowing PlortCreation");
                return true;
            }
            return false;
        }

        public static void Postfix(ReactToShock __instance)
        {
            Console.Log("ReactToShockPostfix");
            if (__instance.ToString() == "slimeQuicksilver(Clone) (ReactToShock)")
            {
                __instance.gameObject.PrintComponents<Component>();
                foreach(ParticleSystemRenderer particleSystemRenderer in __instance.GetComponentsInChildren<ParticleSystemRenderer>())
                {
                    Console.Log(particleSystemRenderer.ToString());
                    Console.Log(particleSystemRenderer.material.ToString());
                    Console.Log(particleSystemRenderer.material.shader.ToString());
                }
                Console.Log(__instance.GetPrivateField<SlimeAppearanceApplicator>("slimeAppearanceApplicator").Appearance.Structures[0].DefaultMaterials[0].ToString());
            }
        }

    }

    [HarmonyPatch(typeof(ReactToShock), "MaybeCreatePlorts", new Type[] { typeof(int) })]
    unsafe class PatchCreatePlorts2
    {
        unsafe public static bool Prefix(ReactToShock __instance)
        {
            /*if(__instance.ToString() == "electricSlime(Clone) (ReactToShock)")
            {
                //Vector3 targetPosition = Camera.main.transform.position;
                Vector3 targetPosition = SRSingleton<SceneContext>.Instance.Player.transform.position+new Vector3(0,2,0);
                //Abilities.CreateShoot(__instance.gameObject.transform.position + new Vector3(0, 3, 0), targetPosition - (__instance.gameObject.transform.position + new Vector3(0, 3, 0)), __instance.regionMember.setId);
                Abilities.CreateShoot(__instance.gameObject.transform.position + new Vector3(0, 3, 0), targetPosition - (__instance.gameObject.transform.position + new Vector3(0, 3, 0)), __instance.regionMember.setId);
                return false;
            }*/

            if (__instance.ToString() == "slimeQuicksilver(Clone) (ReactToShock)")
            {
                Console.Log("Allowing PlortCreation");
                return true;
            }

            return false;
        }
    }

    [HarmonyPatch(typeof(ReactToShock), "UpdateAppearances")]
    unsafe class PatchUpdateAppearances
    {
        unsafe public static void Postfix(ReactToShock __instance, SlimeAppearance baseAppearance)
        {
            Console.Log("");
            Console.Log("");
            Console.Log("");

            Console.Log("SlimeAppearance Normal: " + baseAppearance);
            Console.Log("SlimeAppearance Shocked: " + baseAppearance.ShockedAppearance);
            Console.Log("SlimeAppearanceApplicator Appearance: " + __instance.GetPrivateField<SlimeAppearanceApplicator>("slimeAppearanceApplicator").Appearance);
            Console.Log("SlimeAppearanceApplicator Shocked: " + __instance.GetPrivateField<SlimeAppearanceApplicator>("slimeAppearanceApplicator").Appearance.ShockedAppearance);
            if(__instance.GetPrivateField<SlimeAppearanceApplicator>("slimeAppearanceApplicator").Appearance.ShockedAppearance != null)
            {
                Console.Log(__instance.slimeAppearanceApplicator.Appearance.ShockedAppearance.Structures[0].DefaultMaterials[0].ToString());
                Console.Log("Length of the structure: " + __instance.slimeAppearanceApplicator.Appearance.ShockedAppearance.Structures.Length);
                Console.Log("Length of the lods: " + __instance.GetComponent<SlimeAppearanceApplicator>().LODGroup.lodCount);
                int i2 = 0;
                int num = __instance.slimeAppearanceApplicator.Appearance.ShockedAppearance.Structures[0].Element.Prefabs.Length;
                Console.Log("Logging the Prefab information");

                int lodInt = 0;

                foreach (LOD lod in __instance.GetComponent<SlimeAppearanceApplicator>().LODGroup.GetLODs())
                {
                    lodInt++;
                    Console.Log("Lod augmented");
                    int rendererInt = 0;
                    foreach (Renderer renderer in lod.renderers)
                    {
                        rendererInt++;
                        Console.Log("Renderer number: " + rendererInt + " of LOD number: " + lodInt + " is: " + renderer.name);
                    }
                }

                for (int j = 0; j < num; j++)
                {
                    Console.Log("");
                    Console.Log("loop number: " + j);
                    SlimeAppearanceObject prefab = __instance.slimeAppearanceApplicator.Appearance.ShockedAppearance.Structures[0].Element.Prefabs[j];
                    SlimeAppearanceObject component = SRML.Utils.PrefabUtils.CopyPrefab(prefab.gameObject).GetComponent<SlimeAppearanceObject>();

                    var returnvalue = prefab.gameObject.PrintParent();
                    if (returnvalue != null)
                        returnvalue.PrintParent();

                    if (component.TryGetComponent<MeshFilter>(out MeshFilter filter))
                    {
                        Console.Log("The material of the mesh " + filter + " is: " + component.GetComponent<MeshRenderer>().sharedMaterial);
                        component.GetComponent<MeshRenderer>().sharedMaterials.PrintContent(); 
                        Console.Log("");
                        Console.Log("");
                    }

                    if (component.TryGetComponent<SkinnedMeshRenderer>(out SkinnedMeshRenderer skinned))
                    {
                        Console.Log("The material of the mesh " + skinned.sharedMesh + " is: " + skinned.sharedMaterial);
                        skinned.sharedMaterials.PrintContent();
                        Console.Log("");
                        Console.Log("");
                    }

                    i2++;
                }


                Console.Log("");
                Console.Log("");
                Console.Log("");
                Console.Log("Logging the DefaultMaterials information");
                __instance.slimeAppearanceApplicator.Appearance.ShockedAppearance.Structures[0].DefaultMaterials.PrintContent();


                Console.Log("");
                Console.Log("");
                Console.Log("");
                Console.Log("Logging sussy materials");
                foreach (SlimeAppearanceMaterials slimeAppearanceMaterials in __instance.slimeAppearanceApplicator.Appearance.ShockedAppearance.Structures[0].ElementMaterials)
                {
                    Console.Log(slimeAppearanceMaterials.OverrideDefaults.ToString());
                    slimeAppearanceMaterials.Materials.PrintContent();
                }

            }
            Console.Log("");
            Console.Log("");
            Console.Log("");
        }
    }

}
