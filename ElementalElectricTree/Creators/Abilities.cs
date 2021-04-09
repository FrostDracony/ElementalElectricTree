using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using Assets.Script.Util.Extensions;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

using Console = SRML.Console.Console;
using MonomiPark.SlimeRancher.Regions;

using ElementalElectricTree;
using ElementalElectricTree.Other;

namespace Creators
{
	class SpecialObjects //(UnityEngine.SphereCollider)
	{
		/*unsafe public static GameObject CreateShockBall()
        {
			GameObject Shoot = GameObject.Instantiate<GameObject>(SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.VALLEY_AMMO_1));
			GameObject.Destroy(Shoot.GetComponentInChildren<DestroyAndShockOnTouching>());
			Shoot.AddComponent<ShockPlayer>();
			return Shoot;
		}*/
    }

	class Abilities
    {
        unsafe public static void CreateShoot(Vector3 origin, Vector3 direction, RegionRegistry.RegionSetId id)
        {

			//GameObject Shoot = SRBehaviour.InstantiateActor(SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.VALLEY_AMMO_1), id); //SpecialObjects.CreateShockBall();
			GameObject Shoot = SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.VALLEY_AMMO_1); //SpecialObjects.CreateShockBall();
			//GameObject.Destroy(reactToShock);
			//GameObject.Destroy();
			Physics.IgnoreCollision(Shoot.GetComponent<SphereCollider>(), SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Ids.ELECTRIC_SLIME).GetComponent<Collider>(), true);
			Shoot.AddComponent<DamagePlayerOnTouch>().GetCopyOf(SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.ROCK_SLIME).GetComponent<DamagePlayerOnTouch>());
			Shoot.GetComponent<DamagePlayerOnTouch>().damagePerTouch = 250;
            
			WeaponVacuum weaponVacuum = GameObject.FindObjectOfType<WeaponVacuum>();
			GameObject vacOrigin = weaponVacuum.vacOrigin;
			vp_FPController componentInParent = GameObject.FindObjectOfType<vp_FPController>();

			//Ray ray = new Ray(vacOrigin.transform.position, vacOrigin.transform.up);
			Ray ray = new Ray(origin, direction);

            float num = PhysicsUtil.RadiusOfObject(Shoot);

			/*float num2 = (ray.direction.y >= 0f) ? 0f : (-0.5f * ray.direction.y);
			Console.Log("Weird number");

			Vector3 vector = ray.origin + ray.direction * (num * 0.2f + num2);
			Console.Log("Vector");

			Vector3 velocity = ray.direction * weaponVacuum.GetSpeed(weaponVacuum.player.Ammo.GetSelectedId()) + (componentInParent.Velocity*100);
			Console.Log("velocity");

			Vector3 vector = origin;
			vector += new Vector3(0, 3, 0);

			Vector3 newDirection = SRSingleton<SceneContext>.Instance.Player.transform.position - vector;
			Ray true_ray = new Ray(vector, newDirection);*/

			/*vector = weaponVacuum.EnsureNotShootingIntoRock(vector, ray, num, ref velocity);
			Console.Log("no rocks, " + weaponVacuum.regionRegistry.GetCurrentRegionSetId().ToString());*/

			//GameObject gameObject = SRBehaviour.InstantiateActor(Shoot, weaponVacuum.regionRegistry.GetCurrentRegionSetId(), vector, Quaternion.identity, false);
			Vector3 velocity = ray.direction * weaponVacuum.GetSpeed(weaponVacuum.player.Ammo.GetSelectedId()) + (componentInParent.Velocity * 400);
			GameObject gameObject = SRBehaviour.InstantiateActor(Shoot, weaponVacuum.regionRegistry.GetCurrentRegionSetId(), origin, Quaternion.identity, false);

			//gameObject.transform.position += new Vector3(0,3,0);

			gameObject.transform.LookAt(weaponVacuum.transform);

			PhysicsUtil.RestoreFreezeRotationConstraints(gameObject);

			gameObject.GetComponent<Rigidbody>().velocity = velocity;

			//TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.From<Vector3, Vector3, VectorOptions>(ShortcutExtensions.DOScale(gameObject.transform, gameObject.transform.localScale, 0.1f), gameObject.transform.localScale * 0.2f, true), DG.Tweening.Ease.Linear);
			//Console.Log("Tween");
			gameObject.GetComponent<Vacuumable>().Launch(Ids.NONE);
			gameObject.GetComponent<Vacuumable>().size = Vacuumable.Size.GIANT;
			gameObject.transform.DOScale(gameObject.transform.localScale, 0.1f).From(gameObject.transform.localScale * 0.2f, true).SetEase(Ease.Linear);

		}
	}
}
