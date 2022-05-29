using System;
using UnityEngine;

namespace RilisoftBot
{
	public class MeleeBot : BaseBot
	{
		[Header("Melee damage settings")]
		public float timeToTakeDamage = 2f;

		private float _animationAttackLength;

		public MeleeBot()
		{
		}

		public override bool CheckEnemyInAttackZone(float distanceToEnemy)
		{
			return base.GetSquareAttackDistance() >= distanceToEnemy;
		}

		public float CheckTimeToTakeDamage()
		{
			if (!this.isAutomaticAnimationEnable)
			{
				return this.timeToTakeDamage * Mathf.Pow(0.95f, (float)GlobalGameController.AllLevelsCompleted);
			}
			this.animations[this.animationsName.Attack].speed = this.speedAnimationAttack;
			float single = this._animationAttackLength / this.speedAnimationAttack;
			return single * Mathf.Pow(0.95f, (float)GlobalGameController.AllLevelsCompleted);
		}

		protected override void Initialize()
		{
			base.Initialize();
			this._animationAttackLength = this.animations[this.animationsName.Attack].length;
		}
	}
}