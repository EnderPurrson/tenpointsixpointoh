using System;
using UnityEngine;

namespace RilisoftBot
{
	public class BaseShootingBot : BaseBot
	{
		protected const int MaxCountShootEffectInPool = 4;

		[Header("Shooting damage settings")]
		public GameObject bulletPrefab;

		public bool isFriendlyFire;

		public Transform[] firePoints;

		public bool isSequentialShooting;

		[Header("Detect shot settings")]
		public float rangeShootingDistance = 10f;

		public Transform headPoint;

		protected GameObject[] bulletsEffectPool;

		private bool _isEnemyEnterInAttackZone;

		private int _nextShootEffectIndex;

		private int _nextFirePointIndex;

		public BaseShootingBot()
		{
		}

		public override bool CheckEnemyInAttackZone(float distanceToEnemy)
		{
			float squareAttackDistance = base.GetSquareAttackDistance();
			if (distanceToEnemy < squareAttackDistance)
			{
				this._isEnemyEnterInAttackZone = true;
				return true;
			}
			if (this._isEnemyEnterInAttackZone)
			{
				squareAttackDistance = squareAttackDistance + this.rangeShootingDistance * this.rangeShootingDistance;
				if (distanceToEnemy < squareAttackDistance)
				{
					return true;
				}
			}
			this._isEnemyEnterInAttackZone = false;
			return false;
		}

		protected virtual void Fire(Transform pointFire, GameObject target)
		{
		}

		private string GameNameShootingAnimation()
		{
			if (this.modelCollider == null)
			{
				return string.Empty;
			}
			return string.Format("{0}_shooting", this.modelCollider.gameObject.name);
		}

		protected virtual Transform GetFirePointForSequentialShot()
		{
			int num = this._nextFirePointIndex;
			this._nextFirePointIndex++;
			if (this._nextFirePointIndex >= (int)this.firePoints.Length)
			{
				this._nextFirePointIndex = 0;
			}
			return this.firePoints[num];
		}

		public override Vector3 GetHeadPoint()
		{
			if (this.headPoint == null)
			{
				return base.GetHeadPoint();
			}
			return this.headPoint.position;
		}

		public override float GetMaxAttackDistance()
		{
			float single = this.rangeShootingDistance * this.rangeShootingDistance;
			return base.GetSquareAttackDistance() + single;
		}

		protected GameObject GetShotEffectFromPool()
		{
			int num = this._nextShootEffectIndex;
			this._nextShootEffectIndex++;
			if (this._nextShootEffectIndex >= (int)this.bulletsEffectPool.Length)
			{
				this._nextShootEffectIndex = 0;
			}
			return this.bulletsEffectPool[num];
		}

		protected override void Initialize()
		{
			base.Initialize();
			this.animationsName.Attack = this.GameNameShootingAnimation();
			BotAnimationEventHandler componentInChildren = base.GetComponentInChildren<BotAnimationEventHandler>();
			if (componentInChildren != null)
			{
				componentInChildren.OnDamageEvent += new BotAnimationEventHandler.OnDamageEventDelegate(this.OnShoot);
			}
			this.animations[this.animationsName.Attack].speed = this.speedAnimationAttack;
			this.InitializeShotsPool(4);
			this._isEnemyEnterInAttackZone = false;
		}

		protected virtual void InitializeShotsPool(int sizePool)
		{
			int num = sizePool * (int)this.firePoints.Length;
			this.bulletsEffectPool = new GameObject[num];
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.bulletPrefab);
				this.bulletsEffectPool[i] = gameObject;
			}
		}

		protected virtual void MakeShot(GameObject target)
		{
			if ((int)this.firePoints.Length == 1)
			{
				this.Fire(this.firePoints[0], target);
				return;
			}
			if (this.isSequentialShooting)
			{
				this.Fire(this.GetFirePointForSequentialShot(), target);
				return;
			}
			for (int i = 0; i < (int)this.firePoints.Length; i++)
			{
				this.Fire(this.firePoints[i], target);
			}
		}

		private void OnShoot()
		{
			if (this.botAiController == null || this.botAiController.currentTarget == null)
			{
				return;
			}
			this.MakeShot(this.botAiController.currentTarget.gameObject);
		}
	}
}