using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

using Console = SRML.Console.Console;
using MonomiPark.SlimeRancher.Regions;

using ElementalElectricTree.Other;

namespace Creators
{
	class ShootWhenAgitated : SlimeSubbehaviour
	{
		IEnumerator currentCoroutine;
		const float delay = 0.125F;
		const int reloadTime = 10;
		int quickShootRemains = 120;
		bool waitBeforeShoot = false;

		public override void Awake() => base.Awake();

		public override void Action()
		{

			if(waitBeforeShoot == false)
            {
				waitBeforeShoot = true;

				if (currentCoroutine != null)
				{
					StopCoroutine(currentCoroutine);
					currentCoroutine = null;
				}

				Shoot();

				if (quickShootRemains == 0)
					currentCoroutine = Delay(reloadTime);

			}
            else if(currentCoroutine == null && quickShootRemains > 0)
            {
				currentCoroutine = Delay(delay);
				StartCoroutine(currentCoroutine);
				
			}

		}

		public IEnumerator Delay(float timeToWait)
        {
			yield return new WaitForSeconds(timeToWait);

			if (quickShootRemains == 0)
			{
				quickShootRemains = 120;
			}

			waitBeforeShoot = false;
		}

		public void Shoot()
        {
			if (quickShootRemains != 0)
			{
				Vector3 targetPosition = SRSingleton<SceneContext>.Instance.Player.transform.position + new Vector3(0, 0.5F, 0);
				Abilities.CreateShoot(gameObject.transform.position + new Vector3(0, 3, 0), targetPosition - (gameObject.transform.position + new Vector3(0, 3, 0)), gameObject.GetComponent<RegionMember>().setId);
				quickShootRemains--;
			}

		}


		public override float Relevancy(bool isGrounded)
		{
			if (this.emotions.GetCurr(SlimeEmotions.Emotion.AGITATION) >= 0.5f)
				return 1f;

			return 0f;
		}

		public override void Selected()
        {
        }


    }

	class SpeedupWhenAgitated : SlimeSubbehaviour
	{
		bool inited = false;
		bool canChange = false;
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
		public float shockRadius;

		// Token: 0x04001287 RID: 4743
		public GameObject destroyFX = ValleyAmmo.destroyFX;

		// Token: 0x04001288 RID: 4744
		public bool destroying;

		public void ShockWhenTouch(Collision collision)
		{
			Collider collider = collision.collider;

			if(collider.TryGetComponent<ReactToShock>(out ReactToShock reactToShock)) {
				reactToShock.DoShock(Identifiable.Id.VALLEY_AMMO_2);
            }

		}

		public void OnCollisionEnter(Collision col)
		{
			base.StartCoroutine(this.ShockWhenTouchAtEndOfFrame(col));
		}

		// Token: 0x060013C1 RID: 5057 RVA: 0x0004CE37 File Offset: 0x0004B037
		public IEnumerator ShockWhenTouchAtEndOfFrame(Collision col)
		{
			yield return new WaitForEndOfFrame();
			this.ShockWhenTouch(col);
			yield break;
		}
	}
}
