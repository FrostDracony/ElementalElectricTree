using UnityEngine;
using DG.Tweening;
using MonomiPark.SlimeRancher.Regions;
using ElementalElectricTree;

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
		public static GameObject CreateShoot(Vector3 origin)
		{

			GameObject Shoot = SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.VALLEY_AMMO_1);

			Shoot.AddComponent<DamagePlayerOnTouch>().GetCopyOf(SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.ROCK_SLIME).GetComponent<DamagePlayerOnTouch>());
			Shoot.GetComponent<DamagePlayerOnTouch>().damagePerTouch = 250;

			WeaponVacuum weaponVacuum = Object.FindObjectOfType<WeaponVacuum>();
			vp_FPController componentInParent = Object.FindObjectOfType<vp_FPController>();

			Vector3 playerPosition = new Vector3(SRSingleton<SceneContext>.Instance.Player.transform.position.x, SRSingleton<SceneContext>.Instance.Player.transform.position.y, SRSingleton<SceneContext>.Instance.Player.transform.position.z) + new Vector3(0, 0.5F, 0);

			Vector3 direction = playerPosition - origin;

			Vector3 velocity = direction.normalized * weaponVacuum.ejectSpeed * 3f + (componentInParent.Velocity * 400);
			GameObject gameObject = SRBehaviour.InstantiateActor(Shoot, weaponVacuum.GetPrivateField<RegionRegistry>("regionRegistry").GetCurrentRegionSetId(), origin, Quaternion.identity, false);

			//gameObject.transform.position += new Vector3(0,3,0)
			;
			gameObject.transform.LookAt(weaponVacuum.transform);

			PhysicsUtil.RestoreFreezeRotationConstraints(gameObject);

			gameObject.GetComponent<Rigidbody>().velocity = velocity;

			//Vector3.MoveTowards(origin, playerPosition, Time.deltaTime* weaponVacuum.ejectSpeed * 3f * 50f);

			//TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.From<Vector3, Vector3, VectorOptions>(ShortcutExtensions.DOScale(gameObject.transform, gameObject.transform.localScale, 0.1f), gameObject.transform.localScale * 0.2f, true), DG.Tweening.Ease.Linear);
			//Console.Log("Tween");
			gameObject.GetComponent<Vacuumable>().Launch(Ids.NONE);
			gameObject.GetComponent<Vacuumable>().size = Vacuumable.Size.GIANT;
			gameObject.transform.DOScale(gameObject.transform.localScale, 0.1f).From(gameObject.transform.localScale * 0.2f, true).SetEase(Ease.Linear);

			return gameObject;

		}

		public static void CreateShoot(Vector3 origin, Vector3 direction, RegionRegistry.RegionSetId id)
        {

			GameObject Shoot = SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.VALLEY_AMMO_1);

			Shoot.AddComponent<DamagePlayerOnTouch>().GetCopyOf(SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.ROCK_SLIME).GetComponent<DamagePlayerOnTouch>());
			Shoot.GetComponent<DamagePlayerOnTouch>().damagePerTouch = 250;
            
			WeaponVacuum weaponVacuum = GameObject.FindObjectOfType<WeaponVacuum>();
			GameObject vacOrigin = weaponVacuum.vacOrigin;
			vp_FPController componentInParent = GameObject.FindObjectOfType<vp_FPController>();

			Ray ray = new Ray(origin, direction);

			Vector3 velocity = ray.direction * weaponVacuum.ejectSpeed * 3f + (componentInParent.Velocity * 400);
			GameObject gameObject = SRBehaviour.InstantiateActor(Shoot, weaponVacuum.GetPrivateField<RegionRegistry>("regionRegistry").GetCurrentRegionSetId(), origin, Quaternion.identity, false);

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
