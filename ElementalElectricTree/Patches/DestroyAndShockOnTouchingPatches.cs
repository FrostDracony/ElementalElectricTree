using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;
using Console = SRML.Console.Console;
using ElementalElectricTree.Other;
using Creators;

namespace ElementalElectricTree.Patches
{
    /*[HarmonyPatch(typeof(DestroyAndShockOnTouching), "DestroyAndShock")]
    unsafe class DestroyAndShockOnTouchingPatches
    {
        public static unsafe void PostFix(DestroyAndShockOnTouching __instance)
        {
			Console.Log("Shocking the player");
			if (__instance.shockRadius > 0f)
			{
				Console.Log("2nd step");

				SphereOverlapTrigger.CreateGameObject(__instance.transform.position, __instance.shockRadius, delegate (IEnumerable<Collider> colliders)
				{
					Console.Log("Creating ball");
                    Console.Log("Content: ");
					colliders.ToArray().PrintContent();    
					foreach (Collider collider in colliders)
					{
						Console.Log("collider: " + collider.ToString());

                        if (collider.TryGetComponent<PlayerDamageable>(out PlayerDamageable damageable))
                        {
                            Console.Log("Player Found");
                            Console.Log(collider.ToString());
                            damageable.Damage(5000, __instance.gameObject); //Shake effect
                            break;
                            ////__instance.transform.parent.gameObject.AddComponent<DamagePlayerOnTouch>().GetCopyOf(SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Ids.ELECTRIC_SLIME).GetComponent<DamagePlayerOnTouch>());
                        }

						bool quicksilverFound = colliders.ToArray().FindContent(Identifiable.Id.QUICKSILVER_SLIME);
						bool goldenFound = colliders.ToArray().FindContent(Identifiable.Id.GOLD_SLIME);
						bool flag = quicksilverFound && goldenFound;
						Console.Log("Golden or Quicksilver Slime maybe found: " + goldenFound + " and " + quicksilverFound);
						if(flag)
                        {
							GameObject quicksilver = colliders.ToArray().GetContent(Identifiable.Id.QUICKSILVER_SLIME);
							GameObject golden = colliders.ToArray().GetContent(Identifiable.Id.GOLD_SLIME);
							Custom_Slime_Creator.CreateNewElectricSlime(quicksilver.GetComponent<ReactToShock>(), golden.GetComponent<ReactToShock>());
						}

					}

				}, 15);
			}
			Destroyer.DestroyActor(__instance.gameObject, "ShockBall.DestroyAndShock", false);
		}
    }*/
}
