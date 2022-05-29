using Rilisoft;
using System;
using UnityEngine;

namespace RilisoftBot
{
	public class ShootingBot : BaseShootingBot
	{
		private const float offsetPointDamagePlayer = 0.5f;

		[Header("Explosion damage settings")]
		public bool isProjectileExplosion;

		public float damagePerHitMin;

		public GameObject effectExplosion;

		public float radiusExplosion;

		public float speedBullet = 10f;

		[Header("Automatic bullet speed settings")]
		public bool isCalculateSpeedBullet;

		[Header("Shooting sound settings")]
		public AudioClip shootingSound;

		private float _normalBulletSpeed;

		[Header("Physics shot settings")]
		public bool isMoveByPhysics;

		public float force = 14f;

		public float angle = -10f;

		public ShootingBot()
		{
		}

		protected override void Fire(Transform pointFire, GameObject target)
		{
			Vector3 vector3 = target.transform.position;
			vector3.y += 0.5f;
			this.FireBullet(pointFire.position, vector3, true);
			if (Defs.isCOOP)
			{
				base.FireByRPC(pointFire.position, vector3);
			}
		}

		private void FireBullet(Vector3 pointFire, Vector3 positionToFire, bool doDamage)
		{
			BulletForBot shotFromPool = this.GetShotFromPool();
			if (this.isCalculateSpeedBullet)
			{
				this.animations[this.animationsName.Attack].speed = this.speedAnimationAttack;
			}
			if (!this.isMoveByPhysics)
			{
				shotFromPool.StartBullet(pointFire, positionToFire, this.GetBulletSpeed(), this.isFriendlyFire, doDamage);
			}
			else
			{
				Quaternion quaternion = Quaternion.AngleAxis(this.angle, base.transform.right);
				Vector3 vector3 = quaternion * base.transform.forward;
				shotFromPool.ApplyForceFroBullet(pointFire, positionToFire, this.isFriendlyFire, this.force, vector3, doDamage);
			}
			base.TryPlayAudioClip(this.shootingSound);
		}

		[PunRPC]
		[RPC]
		private void FireBulletRPC(Vector3 pointFire, Vector3 positionToFire)
		{
			this.FireBullet(pointFire, positionToFire, false);
		}

		private float GetBulletSpeed()
		{
			if (this.isCalculateSpeedBullet)
			{
				this.speedBullet = this._normalBulletSpeed * this.speedAnimationAttack;
			}
			return this.speedBullet;
		}

		private BulletForBot GetShotFromPool()
		{
			return base.GetShotEffectFromPool().GetComponent<BulletForBot>();
		}

		protected override void Initialize()
		{
			base.Initialize();
			float item = this.animations[this.animationsName.Attack].length;
			this._normalBulletSpeed = (this.attackDistance + this.rangeShootingDistance) / item;
		}

		protected override void InitializeShotsPool(int sizePool)
		{
			base.InitializeShotsPool(sizePool);
			float bulletSpeed = (this.attackDistance + this.rangeShootingDistance) / this.GetBulletSpeed();
			for (int i = 0; i < (int)this.bulletsEffectPool.Length; i++)
			{
				BulletForBot component = this.bulletsEffectPool[i].GetComponent<BulletForBot>();
				if (component != null)
				{
					component.lifeTime = bulletSpeed;
					component.OnBulletDamage += new BulletForBot.OnBulletDamageDelegate(this.MakeDamageTarget);
					component.needDestroyByStop = false;
				}
			}
		}

		private void MakeDamageTarget(GameObject target, Vector3 positionDamage)
		{
			if (this.isProjectileExplosion)
			{
				Collider[] colliderArray = Physics.OverlapSphere(positionDamage, this.radiusExplosion, Tools.AllAvailabelBotRaycastMask);
				if ((int)colliderArray.Length == 0)
				{
					return;
				}
				float single = this.radiusExplosion * this.radiusExplosion;
				for (int i = 0; i < (int)colliderArray.Length; i++)
				{
					if (colliderArray[i].gameObject != null)
					{
						Transform transforms = colliderArray[i].transform.root;
						if (!(transforms.gameObject == null) && !(base.transform.gameObject == null) && !transforms.Equals(base.transform))
						{
							float single1 = (transforms.position - positionDamage).sqrMagnitude;
							if (single1 <= single)
							{
								if (this.isFriendlyFire || !transforms.CompareTag("Enemy"))
								{
									float single2 = this.damagePerHitMin + (this.damagePerHit - this.damagePerHitMin) * ((single - single1) / single);
									UnityEngine.Object.Instantiate(this.effectExplosion, positionDamage, Quaternion.identity);
									base.MakeDamage(colliderArray[i].transform.root.transform, (float)((int)single2));
								}
							}
						}
					}
				}
			}
			else if (target != null)
			{
				base.MakeDamage(target.transform);
			}
		}

		protected override void OnBotDestroyEvent()
		{
			for (int i = 0; i < (int)this.bulletsEffectPool.Length; i++)
			{
				if (this.bulletsEffectPool[i].gameObject != null)
				{
					BulletForBot component = this.bulletsEffectPool[i].GetComponent<BulletForBot>();
					if (component != null)
					{
						component.OnBulletDamage -= new BulletForBot.OnBulletDamageDelegate(this.MakeDamageTarget);
						if (!component.IsUse)
						{
							UnityEngine.Object.Destroy(this.bulletsEffectPool[i]);
						}
						else
						{
							component.needDestroyByStop = true;
						}
					}
				}
			}
		}
	}
}