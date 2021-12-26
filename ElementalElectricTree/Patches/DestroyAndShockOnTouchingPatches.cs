using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using Console = SRML.Console.Console;
using Creators;

namespace ElementalElectricTree.Patches
{
    [HarmonyPatch(typeof(DestroyAndShockOnTouching), "DestroyAndShock")]
    class DestroyAndShockOnTouchingPatches
    {
        public static void Prefix(DestroyAndShockOnTouching __instance)
        {
			if (__instance.shockRadius > 0f)
			{
				SphereOverlapTrigger.CreateGameObject(__instance.transform.position, __instance.shockRadius, delegate (IEnumerable<Collider> colliders)
				{
					foreach (Collider collider in colliders)
					{

						if (SceneContext.Instance.Player == collider.gameObject)//PhysicsUtil.IsPlayerMainCollider(collider))
						{
							GameObject player = collider.gameObject;
							Damageable damageable = player.GetInterfaceComponent<Damageable>();
							if (__instance.shockRadius == GameContext.Instance.LookupDirector.GetPrefab(Identifiable.Id.VALLEY_AMMO_1).GetComponent<DestroyAndShockOnTouching>().shockRadius)
							{
								int damage = SceneContext.Instance.GameModeConfig.gameModel.currGameMode == PlayerState.GameMode.CLASSIC ? 150 : 100;
								if (damageable.Damage(damage, player))
								{
									Console.Log("Kill");
									DeathHandler.Kill(SceneContext.Instance.Player, DeathHandler.Source.SLIME_EXPLODE, player, "ElectricSlime's Projectile");
								}
								else
								{
									Console.Log("Electrocute");
									player.GetComponent<Effects>().Freeze();
									//player.GetComponent<Effects>().ElectroCute();
								}
							} else if(__instance.shockRadius == GameContext.Instance.LookupDirector.GetPrefab(Identifiable.Id.VALLEY_AMMO_2).GetComponent<DestroyAndShockOnTouching>().shockRadius)
                            {
								if (damageable.Damage(1000, player))
								{
									Console.Log("Kill");
									DeathHandler.Kill(SceneContext.Instance.Player, DeathHandler.Source.SLIME_EXPLODE, player, "ElectricSlime's Cannon Projectile");
								}
								else
								{
									Console.Log("Electrocute");

									player.GetComponent<Effects>().Freeze();
									//player.GetComponent<Effects>().ElectroCute();
								}
							}

							/*Console.Log("Id: " + __instance.GetComponent<Identifiable>().id);
							if(__instance.GetComponent<Identifiable>().id == Identifiable.Id.VALLEY_AMMO_1)
                            {
								Console.Log("Valley1");
								if(damageable.Damage(250, __instance.gameObject))
                                {
									Console.Log("Kill");
									DeathHandler.Kill(SceneContext.Instance.Player, DeathHandler.Source.SLIME_EXPLODE, __instance.gameObject, "ElectricSlime's Projectile");
                                }
                                else
                                {
									Console.Log("Electrocute");
									Effects.ElectroCute();
                                }
							}

							if (__instance.GetComponent<Identifiable>().id == Identifiable.Id.VALLEY_AMMO_2)
							{
								Console.Log("Valley2");
								if (damageable.Damage(1000, __instance.gameObject))
								{
									Console.Log("Kill");
									DeathHandler.Kill(SceneContext.Instance.Player, DeathHandler.Source.SLIME_EXPLODE, __instance.gameObject, "ElectricSlime's Cannon Projectile");
								}
								else
								{
									Console.Log("Electrocute");
									Effects.ElectroCute();
								}
							}*/
							break;
						}
					}

				}, 15);
			}
			Destroyer.DestroyActor(__instance.gameObject, "ShockBall.DestroyAndShock", false);
		}
    }
}
