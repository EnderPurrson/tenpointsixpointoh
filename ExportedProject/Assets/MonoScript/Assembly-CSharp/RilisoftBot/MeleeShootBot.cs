using System;
using UnityEngine;

namespace RilisoftBot
{
	public class MeleeShootBot : ShootingBot
	{
		[Header("Melee damage settings")]
		public float meleeAttackDetect = 6f;

		public float meleeAttackDistance = 3f;

		public float meleeDamagePerHit = 5f;

		[Header("Attack settings")]
		public MeleeShootBot.AttackType attackType;

		public float minTimeToShoot = 30f;

		public float maxTimeToShoot = 40f;

		private bool isEnemyInMeleeZone;

		private bool isEnemyEnterInAttackZone;

		private string animationNameMelee;

		private bool wasMeleeShot;

		private float nextShootTime;

		public MeleeShootBot()
		{
		}

		public override bool CheckEnemyInAttackZone(float distanceToEnemy)
		{
			float squareAttackDistance = base.GetSquareAttackDistance();
			this.isEnemyInMeleeZone = false;
			if (distanceToEnemy < squareAttackDistance)
			{
				if (distanceToEnemy < this.meleeAttackDetect * this.meleeAttackDetect)
				{
					this.isEnemyInMeleeZone = true;
					if (distanceToEnemy < this.meleeAttackDistance * this.meleeAttackDistance)
					{
						return true;
					}
					return false;
				}
				if (this.attackType == MeleeShootBot.AttackType.MeleeAndShootAtTime && this.nextShootTime < Time.time)
				{
					return true;
				}
				if (this.attackType != MeleeShootBot.AttackType.MeleeAndShootAtTime)
				{
					this.isEnemyEnterInAttackZone = true;
					return true;
				}
			}
			if (this.isEnemyEnterInAttackZone)
			{
				squareAttackDistance = squareAttackDistance + this.rangeShootingDistance * this.rangeShootingDistance;
				if (distanceToEnemy < squareAttackDistance)
				{
					return true;
				}
			}
			this.isEnemyEnterInAttackZone = false;
			return false;
		}

		public override void DelayShootAfterEvent(float seconds)
		{
			if (this.nextShootTime < Time.time + seconds)
			{
				this.nextShootTime = Time.time + seconds;
			}
		}

		private string GameNameMeleeAnimation()
		{
			if (this.modelCollider == null)
			{
				return string.Empty;
			}
			return string.Format("{0}_shooting", this.modelCollider.gameObject.name);
		}

		protected override void Initialize()
		{
			base.Initialize();
			this.animationNameMelee = this.GameNameMeleeAnimation();
			this.nextShootTime = Time.time + UnityEngine.Random.Range(this.minTimeToShoot, this.maxTimeToShoot);
		}

		protected override void MakeShot(GameObject target)
		{
			if (!this.wasMeleeShot)
			{
				if (this.attackType == MeleeShootBot.AttackType.MeleeAndShootAtTime)
				{
					this.nextShootTime = Time.time + UnityEngine.Random.Range(this.minTimeToShoot, this.maxTimeToShoot);
				}
				base.MakeShot(target);
			}
			else
			{
				this.Melee(target);
			}
		}

		private void Melee(GameObject target)
		{
			if (Vector3.SqrMagnitude(target.transform.position - base.transform.position) < this.meleeAttackDistance * this.meleeAttackDistance)
			{
				base.MakeDamage(target.transform, this.meleeDamagePerHit);
			}
		}

		protected override void PlayAnimationZombieAttackOrStop()
		{
			if (!this.isEnemyInMeleeZone)
			{
				this.wasMeleeShot = false;
				if (this.animations[this.animationsName.Attack])
				{
					this.animations.CrossFade(this.animationsName.Attack);
				}
				else if (this.animations[this.animationsName.Stop])
				{
					this.animations.CrossFade(this.animationsName.Stop);
				}
			}
			else
			{
				this.wasMeleeShot = true;
				if (this.animations[this.animationNameMelee])
				{
					this.animations.CrossFade(this.animationNameMelee);
				}
				else if (this.animations[this.animationsName.Stop])
				{
					this.animations.CrossFade(this.animationsName.Stop);
				}
			}
		}

		public enum AttackType
		{
			MeleeAndShoot,
			MeleeAndShootAtTime
		}
	}
}