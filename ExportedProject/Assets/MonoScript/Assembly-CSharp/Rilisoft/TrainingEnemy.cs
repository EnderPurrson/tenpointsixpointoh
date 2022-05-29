using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Linq;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class TrainingEnemy : MonoBehaviour
	{
		public AudioClip wakeUpAudioClip;

		public AudioClip dieAudioClip;

		public GameObject aimTarget;

		public int hitPoints = 3;

		private SkinnedMeshRenderer meshRender;

		public Texture hitTexture;

		private Texture skinTexture;

		[ReadOnly]
		public int baseHitPoints;

		public float heightFlyOutHitEffect = 1.75f;

		private Collider _headCol;

		private AudioSource _audioSource;

		private Animation _animation;

		private TrainingEnemy.State _currentState;

		private Collider HeadCollider
		{
			get
			{
				if (this._headCol != null)
				{
					return this._headCol;
				}
				GameObject gameObject = base.gameObject.Descendants("HeadCollider").FirstOrDefault<GameObject>();
				if (gameObject != null)
				{
					this._headCol = gameObject.GetComponent<Collider>();
				}
				return this._headCol;
			}
		}

		public TrainingEnemy()
		{
		}

		public void ApplyDamage(float damage, bool isHeadShot)
		{
			if (this._currentState != TrainingEnemy.State.Awakened)
			{
				return;
			}
			base.StartCoroutine(this.HighlightHitCoroutine());
			if (!isHeadShot)
			{
				this.ShowHitEffect();
			}
			else
			{
				this.ShowHeadShotEffect();
			}
			this.hitPoints--;
			if (this._animation != null)
			{
				this._animation.Play("Dummy_Damage", PlayMode.StopSameLayer);
			}
			if (this.hitPoints <= 0)
			{
				this._currentState = TrainingEnemy.State.Dead;
				if (this.aimTarget != null)
				{
					UnityEngine.Object.Destroy(this.aimTarget);
					this.aimTarget = null;
				}
				base.StartCoroutine(this.DieCoroutine());
			}
		}

		private void Awake()
		{
			this.baseHitPoints = this.hitPoints;
			if (this.aimTarget != null)
			{
				this.aimTarget.SetActive(false);
			}
		}

		[DebuggerHidden]
		private IEnumerator AwakeCoroutine(float delaySeconds = 0f)
		{
			TrainingEnemy.u003cAwakeCoroutineu003ec__Iterator121 variable = null;
			return variable;
		}

		[DebuggerHidden]
		private IEnumerator DieCoroutine()
		{
			TrainingEnemy.u003cDieCoroutineu003ec__Iterator122 variable = null;
			return variable;
		}

		[DebuggerHidden]
		private IEnumerator HighlightHitCoroutine()
		{
			TrainingEnemy.u003cHighlightHitCoroutineu003ec__Iterator120 variable = null;
			return variable;
		}

		public void SetTexture(Texture needTx)
		{
			if (this.meshRender != null)
			{
				this.meshRender.sharedMaterial.mainTexture = needTx;
			}
		}

		private void ShowHeadShotEffect()
		{
			if (Device.isPixelGunLow)
			{
				return;
			}
			HitParticle currentParticle = HeadShotStackController.sharedController.GetCurrentParticle(false);
			if (currentParticle != null && this.HeadCollider != null)
			{
				Vector3 vector3 = (!(this.HeadCollider is BoxCollider) ? ((SphereCollider)this.HeadCollider).center : ((BoxCollider)this.HeadCollider).center);
				currentParticle.StartShowParticle(base.transform.position, base.transform.rotation, false, this.HeadCollider.transform.TransformPoint(vector3));
			}
		}

		private void ShowHitEffect()
		{
			if (Device.isPixelGunLow)
			{
				return;
			}
			HitParticle currentParticle = HitStackController.sharedController.GetCurrentParticle(false);
			if (currentParticle != null)
			{
				currentParticle.StartShowParticle(base.transform.position, base.transform.rotation, false, base.transform.position + (Vector3.up * this.heightFlyOutHitEffect));
			}
		}

		private void Start()
		{
			this._audioSource = base.GetComponent<AudioSource>();
			this._animation = base.GetComponent<Animation>();
			if (this._animation != null)
			{
				this._animation.Play("Dummy_Idle", PlayMode.StopSameLayer);
			}
			this.meshRender = base.GetComponentInChildren<SkinnedMeshRenderer>();
			if (this.meshRender)
			{
				this.meshRender.sharedMaterial = new Material(this.meshRender.sharedMaterial);
				this.skinTexture = this.meshRender.sharedMaterial.mainTexture;
			}
		}

		public void WakeUp(float delaySeconds = 0f)
		{
			base.StartCoroutine(this.AwakeCoroutine(delaySeconds));
		}

		private enum State
		{
			None,
			Awakened,
			Dead
		}
	}
}