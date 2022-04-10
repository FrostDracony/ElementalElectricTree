using System.Collections;
using System.Collections.Generic;
using ElementalElectricTree.Other;
using System.Linq;
using UnityEngine;
using Console = SRML.Console.Console;
using SRML.Utils;
using MonomiPark.SlimeRancher.Regions;
using ElementalElectricTree;
using DG.Tweening;


namespace Creators
{
    /*    class switchSlimeForm : SRBehaviour
		{

			public Identifiable.Id idOfSlime;

			public void switchToForm1()
			{
				Console.Log("Switching to Form 1 of " + gameObject + " id is set to: " + Ids.FORM_2_ELECTRIC_SLIME);
				if (GetComponent<Identifiable>().id == Ids.ELECTRIC_SLIME)
				{
					Console.Log("Slime is already set to the right form");
					return;
				}
				GameObject prefab = PrefabUtils.CopyPrefab(SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Ids.FORM_2_ELECTRIC_SLIME));
				Console.Log("Getting the prefab: " + prefab + " test prefab: " + SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.PINK_SLIME));
				//prefab.GetComponent<SlimeEat>().slimeDefinition = SRSingleton<GameContext>.Instance.SlimeDefinitions.GetSlimeByIdentifiableId(idOfSlime);
				GameObject newSlime = SRBehaviour.InstantiateActor(SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Ids.FORM_2_ELECTRIC_SLIME), GetComponent<RegionMember>().setId, gameObject.transform.position, gameObject.transform.rotation);
				Console.Log("new slime: " + newSlime);
				ElementalElectricTree.SlimeForms.ElectricSlimes.AddAllSlimeForms(newSlime);
				Console.Log("Setting slimeforms");
				*//*newSlime.AddComponent<switchSlimeForm>().slimeObjectForm1 = GetComponent<switchSlimeForm>().slimeObjectForm1;
				newSlime.AddComponent<switchSlimeForm>().slimeObjectForm2 = GetComponent<switchSlimeForm>().slimeObjectForm2;*//*

				void onRegionsChanged(List<Region> exited, List<Region> joined)
				{
					Console.Log("Regions changed");
					foreach (Region region in joined)
					{
						if (region.setId == RegionRegistry.RegionSetId.VALLEY)
						{
							newSlime.GetComponent<switchSlimeForm>().switchToForm1();
						}
						else
						{
							newSlime.GetComponent<switchSlimeForm>().switchToForm2();
						}
					}
				}
				Console.Log("Added RegionsChanged event");
				//newSlime.GetComponent<RegionMember>().regionsChanged += onRegionsChanged;
				Console.Log("GoodBye, " + this);
				Destroy(gameObject);
			}

			public void switchToForm2()
			{
				Console.Log("Switching to Form 2 of " + gameObject + " id is set to: " + Ids.ELECTRIC_SLIME);
				if(GetComponent<Identifiable>().id == Ids.FORM_2_ELECTRIC_SLIME)
				{
					Console.Log("Slime is already set to the right form");
					return;
				}
				GameObject prefab = PrefabUtils.CopyPrefab(SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Ids.ELECTRIC_SLIME));
				Console.Log("Getting the prefab: " + prefab);
				//prefab.GetComponent<SlimeEat>().slimeDefinition = SRSingleton<GameContext>.Instance.SlimeDefinitions.GetSlimeByIdentifiableId(idOfSlime);
				GameObject newSlime = SRBehaviour.InstantiateActor(SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Ids.ELECTRIC_SLIME), GetComponent<RegionMember>().setId, gameObject.transform.position, gameObject.transform.rotation);
				Console.Log("new slime: " + newSlime);
				ElementalElectricTree.SlimeForms.ElectricSlimes.AddAllSlimeForms(newSlime);
				Console.Log("Setting slimeforms");
				void onRegionsChanged(List<Region> exited, List<Region> joined)
				{
					Console.Log("Regions changed");
					foreach (Region region in joined)
					{
						if (region.setId == RegionRegistry.RegionSetId.VALLEY)
						{
							newSlime.GetComponent<switchSlimeForm>().switchToForm1();
						}
						else
						{
							newSlime.GetComponent<switchSlimeForm>().switchToForm2();
						}
					}
				}
				Console.Log("Added RegionsChanged event");
				//newSlime.GetComponent<RegionMember>().regionsChanged += onRegionsChanged;

				Console.Log("GoodBye, " + this);
				Destroy(gameObject);
			}
		}
	*/

    class ChangeParticlesAngry : SlimeSubbehaviour
	{
		public bool blocked = false;

		/*float duration;
		float startDuration = 0;
		float endDuration = 10;

		float timeBtwUse;
		public float startTimeBtwUse = 10;*/

		public GameObject projectile = SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.VALLEY_AMMO_1);
		public GameObject cannonProjectile = SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.VALLEY_AMMO_2);

		float timeBtwCannonShots;
		public float startTimeBtwCannonShots = SceneContext.Instance.GameModeConfig.gameModel.currGameMode == PlayerState.GameMode.CLASSIC ? 15 : 30;

		float timeBtwShots;
		public float startTimeBtwShots = SceneContext.Instance.GameModeConfig.gameModel.currGameMode == PlayerState.GameMode.CLASSIC ? 6 : 10;
		public bool canShoot = true;

		public override void Selected()
        {
        }

		public override void Start()
		{
			//duration = startDuration;
			timeBtwShots = startTimeBtwShots;
			//timeBtwUse = startTimeBtwUse;
			timeBtwCannonShots = startTimeBtwCannonShots;
			base.Start();
		}

		public override void Awake() => base.Awake();

		public override void Action()
		{

			if (emotions.GetCurr(SlimeEmotions.Emotion.AGITATION) >= 0.6f)
			{
				void CreateShot(Vector3 origin, Vector3 direction)
				{
					GameObject Shoot = projectile;
					Shoot.AddComponent<DamagePlayerOnTouch>().GetCopyOf(SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.ROCK_SLIME).GetComponent<DamagePlayerOnTouch>());
					Shoot.GetComponent<DamagePlayerOnTouch>().damagePerTouch = 250;

					WeaponVacuum weaponVacuum = FindObjectOfType<WeaponVacuum>();
					vp_FPController componentInParent = FindObjectOfType<vp_FPController>();

					Vector3 velocity = direction * weaponVacuum.ejectSpeed * 3f + (componentInParent.Velocity);
					GameObject Shot = InstantiateActor(Shoot, weaponVacuum.GetPrivateField<RegionRegistry>("regionRegistry").GetCurrentRegionSetId(), origin + direction.normalized * 5, Quaternion.identity, false);
					Shot.transform.LookAt(weaponVacuum.transform);

					PhysicsUtil.RestoreFreezeRotationConstraints(Shot);

					Shot.GetComponent<Rigidbody>().velocity = velocity;

					Shot.GetComponent<Vacuumable>().Launch(Ids.NONE);
					Shot.GetComponent<Vacuumable>().size = Vacuumable.Size.GIANT;

					Shot.transform.DOScale(Shot.transform.localScale, 0.1f).From(Shot.transform.localScale * 0.2f, true).SetEase(Ease.Linear);
				}

				if (timeBtwShots <= 0 && canShoot)
				{
					Vector3 targetPosition = SRSingleton<SceneContext>.Instance.Player.transform.position + new Vector3(0, 0.1F, 0);
					CreateShot(gameObject.transform.position + new Vector3(0, 3, 0), targetPosition - (gameObject.transform.position + new Vector3(0, 3, 0)));

					timeBtwShots = startTimeBtwShots;
				}
				else
				{
					timeBtwShots -= Time.deltaTime;
				}
			}

			if (emotions.GetCurr(SlimeEmotions.Emotion.AGITATION) >= 0.85f)
			{
				/*if (GetComponent<Identifiable>().id == Ids.ELECTRIC_SLIME)
				{
					void TransformIntoElectricArrow()
					{
						if (transform.Find("ElectricCharge(Clone)") == null)
						{
							Instantiate(Main.assetBundle.LoadAsset<GameObject>("ElectricCharge").FindChild("Particles"), transform);
						}

						*//*Vector3 origin = SRSingleton<SceneContext>.Instance.Player.transform.position + new Vector3(0, 0.5F, 0);
						Vector3 direction = gameObject.transform.position + new Vector3(0, 3, 0);
						Vector3 targetForce = origin - direction;

						gameObject.GetComponent<Rigidbody>().AddForce(targetForce * 10, ForceMode.Impulse);*//*
					}
					Console.Log("duration: " + duration + " bool: " + (duration >= endDuration) + " timeBtwUse: " + timeBtwUse);
					duration += Time.deltaTime;
					if (duration >= endDuration)
					{
						if (transform.Find("ElectricCharge(Clone)"))
							Destroy(transform.Find("ElectricCharge(Clone)"));
						Console.Log("Destroying ElectricChargeParticles");
					}

					//Console.Log("DoTheAbility Arrow");
					if (timeBtwUse <= 0 && duration >= endDuration)
					{
						if (canShoot)
							canShoot = false;

						Destroy(transform.Find("MagicCircleVFX(Clone)").gameObject);
						TransformIntoElectricArrow();

						timeBtwUse = startTimeBtwUse;
						duration = startDuration;
					}
					else
					{
						if (!canShoot)
							canShoot = true;

						//Destroy(transform.Find("ElectricCharge(Clone)"));

						timeBtwUse -= Time.deltaTime;

						if (transform.Find("MagicCircleVFX(Clone)"))
							return;
						Console.Log("Create Particles");

						GameObject particles = Main.assetBundle.LoadAsset<GameObject>("MagicCircleVFX");
						particles.transform.position = new Vector3(0, 0.25F, 0);
						if (Shader.Find("SR/Particles/Additive") != null)
						{
							foreach (ParticleSystemRenderer particleSystemRenderer in particles.GetComponentsInChildren<ParticleSystemRenderer>())
							{
								particleSystemRenderer.material.shader = Shader.Find("SR/Particles/Additive");
							}
						}
						Instantiate(particles, transform);
					}
				}*/
				
				if (GetComponent<Identifiable>().id == Ids.FORM_2_ELECTRIC_SLIME)
				{
					void CreateCannonShot(Vector3 origin, Vector3 direction)
					{
						GameObject Shoot = cannonProjectile;
						Shoot.AddComponent<DamagePlayerOnTouch>().GetCopyOf(SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.ROCK_SLIME).GetComponent<DamagePlayerOnTouch>());
						Shoot.GetComponent<DamagePlayerOnTouch>().damagePerTouch = 1000;

						WeaponVacuum weaponVacuum = FindObjectOfType<WeaponVacuum>();
						vp_FPController componentInParent = FindObjectOfType<vp_FPController>();

						Vector3 velocity = direction * weaponVacuum.ejectSpeed * 3f + (componentInParent.Velocity * 100);
						GameObject Shot = InstantiateActor(Shoot, weaponVacuum.GetPrivateField<RegionRegistry>("regionRegistry").GetCurrentRegionSetId(), origin + direction.normalized * 5, Quaternion.identity, false);
						Shot.transform.LookAt(weaponVacuum.transform);

						PhysicsUtil.RestoreFreezeRotationConstraints(Shot);

						Shot.GetComponent<Rigidbody>().velocity = velocity;

						Shot.GetComponent<Vacuumable>().Launch(Ids.NONE);
						Shot.GetComponent<Vacuumable>().size = Vacuumable.Size.GIANT;

						Shot.transform.DOScale(Shot.transform.localScale, 0.1f).From(Shot.transform.localScale * 0.2f, true).SetEase(Ease.Linear);
					}

					Console.Log("timeBtwCannonShots: " + timeBtwCannonShots);

					if (timeBtwCannonShots <= 0)
					{
						if (canShoot)
							canShoot = false;

						Vector3 targetPosition = SRSingleton<SceneContext>.Instance.Player.transform.position + new Vector3(0, 0.1F, 0);
						CreateCannonShot(gameObject.transform.position + new Vector3(0, 3, 0), targetPosition - (gameObject.transform.position + new Vector3(0, 3, 0)));

						timeBtwCannonShots = startTimeBtwCannonShots;
					}
					else
					{
						if (!canShoot)
							canShoot = true;

						timeBtwCannonShots -= Time.deltaTime;
					}

				}
			}

			if (!blocked)
			{
				Destroy(gameObject.GetComponentInChildren<ParticleSystem>().gameObject);
				GameObject particles = Main.assetBundle.LoadAsset<GameObject>("MagicCircleVFX");
				particles.transform.position = new Vector3(0, 0.25F, 0);
				if (Shader.Find("SR/Particles/Additive") != null)
				{
					foreach (ParticleSystemRenderer particleSystemRenderer in particles.GetComponentsInChildren<ParticleSystemRenderer>())
					{
						particleSystemRenderer.material.shader = Shader.Find("SR/Particles/Additive");
					}
				}

				Instantiate(particles, transform);
				blocked = true;
				GetComponent<ChangeParticlesNormal>().blocked = false;
			}
		}

		public override float Relevancy(bool isGrounded)
		{
			if (emotions.GetCurr(SlimeEmotions.Emotion.AGITATION) >= 0.5f)
            {
				return 1f;
            }
            else if (emotions.GetCurr(SlimeEmotions.Emotion.AGITATION) < 0.5f)
			{
				if (transform.Find("MagicCircleVFX(Clone)"))
				{
					Destroy(transform.Find("MagicCircleVFX(Clone)").gameObject);
					GameObject particles = Main.assetBundle.LoadAsset<GameObject>("ElectricSparklesSlimes");
					particles.transform.position = new Vector3(0, 0.5F, 0);

					if (Shader.Find("SR/Particles/Additive") != null)
					{
						foreach (ParticleSystemRenderer particleSystemRenderer in particles.GetComponentsInChildren<ParticleSystemRenderer>())
						{
							particleSystemRenderer.material.shader = Shader.Find("SR/Particles/Additive");
						}
					}

					Instantiate(particles, transform);
				}
			}

			return 0f;
		}

	}

	class ChangeParticlesNormal : SlimeSubbehaviour
	{
		public bool blocked = false;
		public override void Selected()
		{
		}

		public override void Action()
		{
			if (!blocked)
			{
				Destroy(gameObject.GetComponentInChildren<ParticleSystem>().gameObject);
				GameObject particles = Main.assetBundle.LoadAsset<GameObject>("ElectricSparklesSlimes");
				particles.transform.position = new Vector3(0, 0.5F, 0);

				if (Shader.Find("SR/Particles/Additive") != null)
				{
					foreach (ParticleSystemRenderer particleSystemRenderer in particles.GetComponentsInChildren<ParticleSystemRenderer>())
					{
						particleSystemRenderer.material.shader = Shader.Find("SR/Particles/Additive");
					}
				}

				Instantiate(particles, transform);
				blocked = true;
				GetComponent<ChangeParticlesAngry>().blocked = false;
			}

		}

		public override float Relevancy(bool isGrounded)
		{
			if (emotions.GetCurr(SlimeEmotions.Emotion.AGITATION) < 0.5f)
				return 1f;

			return 0f;
		}

	}

	class Effects : SRBehaviour
    {
		public bool update = false;

		IEnumerator Delay(float time, System.Random random)
        {
			int length = 10;
			Ammo ammo = SceneContext.Instance.PlayerState.Ammo;
			bool isClassicMode = SceneContext.Instance.GameModeConfig.gameModel.currGameMode == PlayerState.GameMode.CLASSIC ? true : false;
			int max = isClassicMode ? 100 : 30;

			max = ammo.Slots[ammo.selectedAmmoIdx].count > max ? ammo.Slots[ammo.selectedAmmoIdx].count : max;
			
			length = isClassicMode ? random.Next(10, max) : random.Next(10, max);

			for (int i = 0; i < length; i++)
            {
				FindObjectOfType<WeaponVacuum>().ExpelAmmo(null);
				yield return new WaitForSeconds(time);
            }
        }

		void Ammo(System.Random random)
        {

			if (SceneContext.Instance.PlayerState.Ammo.Slots[0] == null &&
				SceneContext.Instance.PlayerState.Ammo.Slots[1] == null &&
				SceneContext.Instance.PlayerState.Ammo.Slots[2] == null &&
				SceneContext.Instance.PlayerState.Ammo.Slots[3] == null)
			{
				Console.Log("Every slot is empty");
				return;
			}

			Console.Log("Printing AmmoLine 1");
			if (!SceneContext.Instance.PlayerState.Ammo.SetAmmoSlot(random.Next(0, 4)))
			{
				Console.Log("Printing AmmoLine 2");
				SceneContext.Instance.PlayerState.Ammo.SetAmmoSlot(random.Next(0, 3));
			}

			bool flag = SceneContext.Instance.PlayerState.Ammo.GetSelectedStored() == null;
			Console.Log("Flag is: " + flag);
			if(!flag)
            {
				Console.Log("SelectedStored isnt null");
				flag = SceneContext.Instance.PlayerState.Ammo.GetSelectedStored().GetComponent<Identifiable>() == null;
			}
			if (!flag)
            {
				Console.Log("SelectedStored's Component isnt null");
				flag = SceneContext.Instance.PlayerState.Ammo.GetSelectedStored().GetComponent<Identifiable>().id == Identifiable.Id.NONE;
				if(!flag)
                {
					Console.Log("Id: " + SceneContext.Instance.PlayerState.Ammo.GetSelectedStored().GetComponent<Identifiable>().id);
                }
			}

			if (flag)
            {
				Console.Log("Printing AmmoLine 3");
				Ammo(random);
            }
		}

		public void Freeze()
        {
			Console.Log("Freezing the Player");
			Console.Log("Printing Line 1");
			SceneContext.Instance.Player.GetComponent<vp_FPController>().MotorAcceleration = 0;
			Console.Log("Printing Line 2");
			System.Random random = new System.Random();

			Console.Log("The Length of the slots is: " + SceneContext.Instance.PlayerState.Ammo.Slots.Length);

			if (!SceneContext.Instance.PlayerState.Ammo.SetAmmoSlot(random.Next(0, 4)))
            {
				SceneContext.Instance.PlayerState.Ammo.SetAmmoSlot(random.Next(0, 3));
			}

			if (SceneContext.Instance.PlayerState.Ammo.Slots[0] == null &&
				SceneContext.Instance.PlayerState.Ammo.Slots[1] == null &&
				SceneContext.Instance.PlayerState.Ammo.Slots[2] == null &&
				SceneContext.Instance.PlayerState.Ammo.Slots[3] == null)
			{
				Console.Log("Every slot is empty");
				Invoke("Unfreeze", SceneContext.Instance.GameModeConfig.gameModel.currGameMode == PlayerState.GameMode.CLASSIC ? 5 : 3);
				return;
			}

			//Ammo(random);

			Console.Log("Starting Coroutine");
			if (SceneContext.Instance.PlayerState.Ammo.HasSelectedAmmo())
            {
				Console.Log("Player has selected Ammo");
				StartCoroutine(Delay(0.05f, random));
			}

			Invoke("Unfreeze", SceneContext.Instance.GameModeConfig.gameModel.currGameMode == PlayerState.GameMode.CLASSIC ? 5 : 3);
		}

		public void Unfreeze()
        {
			Console.Log("Unfreezing the Player");
			SceneContext.Instance.player.GetComponent<vp_FPController>().MotorAcceleration = 0.25f;
		}


	}

	class ElectricCannonBullet : SlimeSubbehaviour
    {
		float timeBtwShots;
		public float startTimeBtwShots = 10;

		public void CreateCannonShot(Vector3 origin, Vector3 direction)
		{
			GameObject Shoot = SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.VALLEY_AMMO_2);
			Shoot.AddComponent<DamagePlayerOnTouch>().GetCopyOf(SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.ROCK_SLIME).GetComponent<DamagePlayerOnTouch>());
			Shoot.GetComponent<DamagePlayerOnTouch>().damagePerTouch = 1000;

			WeaponVacuum weaponVacuum = FindObjectOfType<WeaponVacuum>();
			vp_FPController componentInParent = GameObject.FindObjectOfType<vp_FPController>();

			Vector3 velocity = direction * weaponVacuum.ejectSpeed * 3f + (componentInParent.Velocity * 100);
			GameObject Shot = InstantiateActor(Shoot, weaponVacuum.GetPrivateField<RegionRegistry>("regionRegistry").GetCurrentRegionSetId(), origin + direction.normalized * 5, Quaternion.identity, false);
			Shot.transform.LookAt(weaponVacuum.transform);

			PhysicsUtil.RestoreFreezeRotationConstraints(Shot);

			Shot.GetComponent<Rigidbody>().velocity = velocity;

			Shot.GetComponent<Vacuumable>().Launch(Ids.NONE);
			Shot.GetComponent<Vacuumable>().size = Vacuumable.Size.GIANT;

			/*Destroy(Shot.GetComponent<SphereCollider>());
			Destroy(Shot.FindChild("DelaunchTrigger"));
			Destroy(Shot.FindChild("Core Sphere"));
			Destroy(Shot.FindChild("Shadow"));
			Instantiate(Main.assetBundle2.LoadAsset<GameObject>("ElectricCannon"), Shot.transform);*/

			Shot.transform.DOScale(Shot.transform.localScale, 0.1f).From(Shot.transform.localScale * 0.2f, true).SetEase(Ease.Linear);
		}

		public override void Action()
        {
			Console.Log("DoTheAbility Cannon");

			if (timeBtwShots <= 0)
			{
				if (GetComponent<ShootWhenAgitated>().canShoot)
					GetComponent<ShootWhenAgitated>().canShoot = false;

				Vector3 targetPosition = SRSingleton<SceneContext>.Instance.Player.transform.position + new Vector3(0, 0.5F, 0);
				CreateCannonShot(gameObject.transform.position + new Vector3(0, 3, 0), targetPosition - (gameObject.transform.position + new Vector3(0, 3, 0)));

				timeBtwShots = startTimeBtwShots;
			}
			else
			{
				if (!GetComponent<ShootWhenAgitated>().canShoot)
					GetComponent<ShootWhenAgitated>().canShoot = true;

				timeBtwShots -= Time.deltaTime;
			}

		}

        public override float Relevancy(bool isGrounded)
        {
			if (emotions.GetCurr(SlimeEmotions.Emotion.AGITATION) >= 0.85f)
				return 1f;

			return 0f;
		}

		public override void Start()
		{
			timeBtwShots = startTimeBtwShots;
			base.Start();
		}

		public override void Selected()
        {
        }

    }
	class ElectricArrow : SlimeSubbehaviour
	{
		float timeBtwUse;
		public float startTimeBtwUse = 10;

		public void TransformIntoElectricArrow()
		{
			if (transform.Find("ElectricCharge(Clone)") == null) {
				GameObject particles = Instantiate(Main.assetBundle.LoadAsset<GameObject>("ElectricCharge").FindChild("Particles"), transform);
				Console.Log("FindChild is: " + transform.Find("ElectricCharge(Clone)"));
			}

			/*Vector3 origin = SRSingleton<SceneContext>.Instance.Player.transform.position + new Vector3(0, 0.5F, 0);
			Vector3 direction = gameObject.transform.position + new Vector3(0, 3, 0);
			Vector3 targetForce = origin - direction;

			gameObject.GetComponent<Rigidbody>().AddForce(targetForce * 10, ForceMode.Impulse);*/
		}

		public override void Awake() => base.Awake();


		public override void Action()
		{
			Console.Log("DoTheAbility Arrow");
			if (timeBtwUse <= 0)
			{
				if (GetComponent<ShootWhenAgitated>().canShoot)
					GetComponent<ShootWhenAgitated>().canShoot = false;

				TransformIntoElectricArrow();

				timeBtwUse = startTimeBtwUse;
			}
			else
			{
				if (!GetComponent<ShootWhenAgitated>().canShoot)
					GetComponent<ShootWhenAgitated>().canShoot = true;

				Destroy(transform.Find("ElectricCharge(Clone)"));

				timeBtwUse -= Time.deltaTime;
			}

		}

		public override float Relevancy(bool isGrounded)
		{
			if (emotions.GetCurr(SlimeEmotions.Emotion.AGITATION) >= 0.85f)
				return 1f;

			return 0f;
		}

		public override void Selected() 
		{
		}

    }

	class Projectile : MonoBehaviour
    {
		//Vector3 target;

		void Start()
        {
			//target = new Vector3()
        }
    }

	class ShootWhenAgitated : SlimeSubbehaviour
	{
		float timeBtwShots;
		public float startTimeBtwShots = 1;
		public bool canShoot = true;

		public void CreateShot(Vector3 origin, Vector3 direction)
		{
			GameObject Shoot = SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.VALLEY_AMMO_1);
			Shoot.AddComponent<DamagePlayerOnTouch>().GetCopyOf(SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.ROCK_SLIME).GetComponent<DamagePlayerOnTouch>());
			Shoot.GetComponent<DamagePlayerOnTouch>().damagePerTouch = 250;

			WeaponVacuum weaponVacuum = FindObjectOfType<WeaponVacuum>();
			vp_FPController componentInParent = FindObjectOfType<vp_FPController>();

			Vector3 velocity = direction * weaponVacuum.ejectSpeed * 3f + (componentInParent.Velocity);
			GameObject Shot = InstantiateActor(Shoot, weaponVacuum.GetPrivateField<RegionRegistry>("regionRegistry").GetCurrentRegionSetId(), origin + direction.normalized * 5, Quaternion.identity, false);
			Shot.transform.LookAt(weaponVacuum.transform);

			PhysicsUtil.RestoreFreezeRotationConstraints(Shot);

			Shot.GetComponent<Rigidbody>().velocity = velocity;

			Shot.GetComponent<Vacuumable>().Launch(Ids.NONE);
			Shot.GetComponent<Vacuumable>().size = Vacuumable.Size.GIANT;

			Shot.transform.DOScale(Shot.transform.localScale, 0.1f).From(Shot.transform.localScale * 0.2f, true).SetEase(Ease.Linear);
		}

		public override void Awake() => base.Awake();

        public override void Start()
        {
			timeBtwShots = startTimeBtwShots;
            base.Start();
        }

        public override void Action()
		{
			if(timeBtwShots <= 0 && canShoot)
            {
				Vector3 targetPosition = SRSingleton<SceneContext>.Instance.Player.transform.position;// + new Vector3(0, 0.5F, 0);
				CreateShot(gameObject.transform.position + new Vector3(0, 3, 0), targetPosition - (gameObject.transform.position + new Vector3(0, 3, 0)));

				//Vector3.MoveTowards(origin, playerPosition, Time.deltaTime * weaponVacuum.ejectSpeed * 3f * 50f);

				timeBtwShots = startTimeBtwShots;
            }
            else
            {
				timeBtwShots -= Time.deltaTime;
            }
		}

		public override float Relevancy(bool isGrounded)
		{
			if (emotions.GetCurr(SlimeEmotions.Emotion.AGITATION) >= 0.25f)
				return 1f;

			return 0f;
		}

		public void Update()
        {

        }

		public override void Selected()
        {
        }


    }

	class SpeedupWhenAgitated : SlimeSubbehaviour
	{
		bool inited = false;
		const int speedupNumber = 200;

		public override void Awake() => base.Awake();

		public override void Action()
		{

			if (inited)
			{
				gameObject.GetComponent<PuddleSlimeScoot>().straightlineForceFactor *= speedupNumber;
            }
            else
            {
				gameObject.GetComponent<PuddleSlimeScoot>().straightlineForceFactor /= speedupNumber;
			}
			

		}

		public override float Relevancy(bool isGrounded)
		{
			if (this.emotions.GetCurr(SlimeEmotions.Emotion.AGITATION) >= 0.5f) {
				inited = true;
				return 1f;
			}

			inited = false;
			return 0f;
		}

		public override void Selected()
		{
		}


	}

	class ShockOnTouch : SRBehaviour
    {

		static DestroyAndShockOnTouching ValleyAmmo = SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.VALLEY_AMMO_1).GetComponentInChildren<DestroyAndShockOnTouching>();

		// Token: 0x04001287 RID: 4743
		public GameObject destroyFX = ValleyAmmo.destroyFX;

		// Token: 0x04001288 RID: 4744

		public void ShockWhenTouch(Collision collision)
		{
			Collider collider = collision.collider;

			if(collider.TryGetComponent<ReactToShock>(out ReactToShock reactToShock)) {
				reactToShock.DoShock(Identifiable.Id.VALLEY_AMMO_2);
            }

		}

		public void OnCollisionEnter(Collision col)
		{
			StartCoroutine(ShockWhenTouchAtEndOfFrame(col));
		}

		// Token: 0x060013C1 RID: 5057 RVA: 0x0004CE37 File Offset: 0x0004B037
		public IEnumerator ShockWhenTouchAtEndOfFrame(Collision col)
		{
			yield return new WaitForEndOfFrame();
			ShockWhenTouch(col);
			yield break;
		}
	}

	class SSAdapter : SRBehaviour
    {
		public void Start()
        {

			Destroy(this);
        }
    }

	class ElectroMeterRegion : SRBehaviour
    {
		public List<GameObject> gameObjects = new List<GameObject>();

		public void OnDisable()
        {
			Console.Log("Time to disable");
			foreach(GameObject gameObject in gameObjects)
            {
				if(gameObject.GetComponent<Identifiable>().id == Ids.ELECTRIC_SLIME)
                {
					gameObject.GetComponent<PuddleSlimeScoot>().straightlineForceFactor = SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.QUICKSILVER_SLIME).GetComponent<PuddleSlimeScoot>().straightlineForceFactor * 4;
                }
                else if (gameObject.GetComponent<Identifiable>().id == Ids.FORM_2_ELECTRIC_SLIME)
				{
					gameObject.GetComponent<SlimeRandomMove>().scootSpeedFactor = SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.PINK_SLIME).GetComponent<SlimeRandomMove>().scootSpeedFactor * 6;
					gameObject.GetComponent<SlimeRandomMove>().verticalFactor = SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.PINK_SLIME).GetComponent<SlimeRandomMove>().verticalFactor * 3;
				}
            }
			gameObjects.Clear();
        }

		public void OnTriggerEnter(Collider slime)
        {
			Identifiable.Id id = Identifiable.Id.NONE;

			Console.Log("OnEnter: collider is: " + slime.gameObject);

			if (slime.gameObject.GetComponent<Identifiable>() != null)
            {
				id = slime.gameObject.GetComponent<Identifiable>().id;
				if (id == Ids.ELECTRIC_SLIME)
                {
					slime.gameObject.GetComponent<PuddleSlimeScoot>().straightlineForceFactor /= 4;
				}
				else if (id == Ids.FORM_2_ELECTRIC_SLIME)
                {
					slime.gameObject.GetComponent<SlimeRandomMove>().scootSpeedFactor /= 6;
					slime.gameObject.GetComponent<SlimeRandomMove>().verticalFactor /= 3;
				}
			}
        }

		public void OnTriggerExit(Collider slime)
		{
			Identifiable.Id id = Identifiable.Id.NONE;

			Console.Log("OnExit: collider is: " + slime.gameObject);
			
			if (slime.gameObject.GetComponent<Identifiable>() != null)
			{
				id = slime.gameObject.GetComponent<Identifiable>().id;
				if (id == Ids.ELECTRIC_SLIME)
				{
					slime.gameObject.GetComponent<PuddleSlimeScoot>().straightlineForceFactor *= 4;
				}
				else if (id == Ids.FORM_2_ELECTRIC_SLIME)
				{
					slime.gameObject.GetComponent<SlimeRandomMove>().scootSpeedFactor *= 6;
					slime.gameObject.GetComponent<SlimeRandomMove>().verticalFactor *= 3;
				}
			}
		}
	}

	class ElectricSlimeCreator : SRBehaviour
	{

		DestroyAndShockOnTouching ValleyAmmo = null;

		public bool slimeAlreadyCreated = false;

		public void ShockWhenTouch(Collision collision)
		{
			Collider collider = collision.collider;

			if (collider.TryGetComponent<ReactToShock>(out ReactToShock reactToShock))
			{
				reactToShock.DoShock(Identifiable.Id.VALLEY_AMMO_2);
			}

		}

		public void OnCollisionEnter(Collision col)
		{
			ValleyAmmo = GetComponentInChildren<DestroyAndShockOnTouching>();

			if (ValleyAmmo.shockRadius > 0f && !slimeAlreadyCreated)
			{
				SphereOverlapTrigger.CreateGameObject(ValleyAmmo.transform.position, ValleyAmmo.shockRadius, delegate (IEnumerable<Collider> colliders)
				{

					if (slimeAlreadyCreated)
						return;

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

					quickSilverSlime = hashSet.FirstOrDefault(x => x.ToString() == "slimeQuicksilver(Clone) (ReactToShock)");
					goldSlime = hashSet.FirstOrDefault(x => x.ToString() == "slimeGold(Clone) (ReactToShock)");

					if (quickSilverSlime != null)
						shocked = quickSilverSlime.GetPrivateField<SlimeAppearanceApplicator>("slimeAppearanceApplicator").Appearance;

					if (quickSilverSlime != null && goldSlime != null && shocked != null && !slimeAlreadyCreated)
					{
						slimeAlreadyCreated = true;
						Custom_Slime_Creator.CreateElectricSlime(quickSilverSlime, goldSlime, shocked);
						GameObject.Destroy(this);

						return;
					}
				}, 15);
			}
		}

	}
}
