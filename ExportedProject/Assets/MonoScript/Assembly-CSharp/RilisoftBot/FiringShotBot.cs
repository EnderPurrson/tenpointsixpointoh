using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace RilisoftBot
{
	public class FiringShotBot : BaseShootingBot
	{
		[Header("Firing settings")]
		[Range(0.1f, 1f)]
		public float chanceMakeDamage = 1f;

		public float timeShowFireEffect = 2f;

		private bool _isEffectFireShow;

		public FiringShotBot()
		{
		}

		protected override void Fire(Transform pointFire, GameObject target)
		{
			ParticleSystem fireShotEffectFromPool = this.GetFireShotEffectFromPool();
			if ((int)this.firePoints.Length != 1)
			{
				base.StartCoroutine(this.ShowFireEffect(pointFire, fireShotEffectFromPool));
			}
			else
			{
				base.StartCoroutine(this.ShowFireEffect(fireShotEffectFromPool));
			}
			if (this.chanceMakeDamage >= UnityEngine.Random.@value)
			{
				base.MakeDamage(target.transform);
			}
		}

		private ParticleSystem GetFireShotEffectFromPool()
		{
			return base.GetShotEffectFromPool().GetComponent<ParticleSystem>();
		}

		protected override void InitializeShotsPool(int sizePool)
		{
			base.InitializeShotsPool(sizePool);
			Transform transforms = ((int)this.firePoints.Length != 1 ? base.transform : this.firePoints[0]);
			for (int i = 0; i < (int)this.bulletsEffectPool.Length; i++)
			{
				this.bulletsEffectPool[i].transform.parent = transforms;
				this.bulletsEffectPool[i].transform.localPosition = Vector3.zero;
				this.bulletsEffectPool[i].transform.rotation = Quaternion.identity;
				this.bulletsEffectPool[i].GetComponent<ParticleSystem>().Stop();
			}
		}

		[DebuggerHidden]
		private IEnumerator ShowFireEffect(GameObject effect)
		{
			FiringShotBot.u003cShowFireEffectu003ec__Iterator11D variable = null;
			return variable;
		}

		[DebuggerHidden]
		private IEnumerator ShowFireEffect(ParticleSystem effect)
		{
			FiringShotBot.u003cShowFireEffectu003ec__Iterator11E variable = null;
			return variable;
		}

		[DebuggerHidden]
		private IEnumerator ShowFireEffect(Transform pointFire, ParticleSystem effect)
		{
			FiringShotBot.u003cShowFireEffectu003ec__Iterator11F variable = null;
			return variable;
		}
	}
}