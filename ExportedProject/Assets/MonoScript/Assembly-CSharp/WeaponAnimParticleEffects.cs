using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class WeaponAnimParticleEffects : MonoBehaviour
{
	public WeaponAnimEffectData[] effects;

	private List<GameObject> _eo;

	private WeaponAnimEffectData _currentEffect;

	private bool _isInit;

	private string _lastAnimationName;

	private bool _isCanStopNotLoopEffect;

	private List<GameObject> _effectObjects
	{
		get
		{
			if (this._eo == null)
			{
				this._eo = new List<GameObject>();
				WeaponAnimEffectData[] weaponAnimEffectDataArray = this.effects;
				for (int i = 0; i < (int)weaponAnimEffectDataArray.Length; i++)
				{
					ParticleSystem[] particleSystemArray = weaponAnimEffectDataArray[i].particleSystems;
					for (int j = 0; j < (int)particleSystemArray.Length; j++)
					{
						ParticleSystem particleSystem = particleSystemArray[j];
						this._eo.Add(particleSystem.gameObject);
					}
				}
			}
			return this._eo;
		}
	}

	public WeaponAnimParticleEffects()
	{
	}

	public void ChangeEffectAfterStopAnimation()
	{
		this._isCanStopNotLoopEffect = true;
		string namePlayingAnimation = this.GetNamePlayingAnimation();
		if (!string.IsNullOrEmpty(namePlayingAnimation))
		{
			this.OnStartAnimEffects(namePlayingAnimation);
		}
	}

	private bool CheckSkipStartEffectForAnimation(string animationName)
	{
		if (this._currentEffect == null)
		{
			return false;
		}
		if (this._currentEffect.isLoop)
		{
			return this._lastAnimationName == animationName;
		}
		WeaponAnimEffectData effectData = this.GetEffectData(animationName);
		if (effectData == null)
		{
			return false;
		}
		if (effectData != null && !effectData.isLoop)
		{
			base.CancelInvoke("ChangeEffectAfterStopAnimation");
			return false;
		}
		return !this._isCanStopNotLoopEffect;
	}

	public void DisableEffectForAnimation(string animName)
	{
		WeaponAnimEffectData effectData = this.GetEffectData(animName);
		if (effectData != null)
		{
			effectData.isPlaying = false;
			this.SetActiveEffect(effectData, false);
			this._currentEffect = null;
		}
	}

	private WeaponAnimEffectData GetEffectData(string animationName)
	{
		return this.effects.FirstOrDefault<WeaponAnimEffectData>((WeaponAnimEffectData e) => e.animationName == animationName);
	}

	public List<GameObject> GetListAnimEffects()
	{
		return this._effectObjects;
	}

	private string GetNamePlayingAnimation()
	{
		string str;
		if (base.GetComponent<Animation>() == null)
		{
			return string.Empty;
		}
		IEnumerator enumerator = base.GetComponent<Animation>().GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				AnimationState current = (AnimationState)enumerator.Current;
				if (!base.GetComponent<Animation>().IsPlaying(current.name))
				{
					continue;
				}
				str = current.name;
				return str;
			}
			return string.Empty;
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable == null)
			{
			}
			disposable.Dispose();
		}
		return str;
	}

	private void InitiAnimatonEventForEffect(WeaponAnimEffectData effectData)
	{
		AnimationClip clip = base.GetComponent<Animation>().GetClip(effectData.animationName);
		if (clip == null)
		{
			return;
		}
		if (!clip.events.Any<AnimationEvent>((AnimationEvent e) => (!(e.stringParameter == effectData.animationName) || !(e.functionName == "OnStartAnimEffects") ? false : Math.Abs(e.time) < 0.001f)))
		{
			AnimationEvent animationEvent = new AnimationEvent()
			{
				stringParameter = effectData.animationName,
				functionName = "OnStartAnimEffects",
				time = 0f
			};
			clip.AddEvent(animationEvent);
		}
		if (!clip.events.Any<AnimationEvent>((AnimationEvent e) => (!(e.stringParameter == effectData.animationName) || !(e.functionName == "OnAnimationFinished") ? false : Math.Abs(e.time - clip.length) < 0.001f)))
		{
			AnimationEvent animationEvent1 = new AnimationEvent()
			{
				stringParameter = effectData.animationName,
				functionName = "OnAnimationFinished",
				time = clip.length
			};
			clip.AddEvent(animationEvent1);
		}
		effectData.animationLength = (!effectData.isLoop ? clip.length : 0f);
	}

	private void OnAnimationFinished(string animationName)
	{
		Debug.Log(string.Concat("===== finished '", animationName, "'"));
		WeaponAnimEffectData effectData = this.GetEffectData(animationName);
		if (effectData != null)
		{
			effectData.isPlaying = false;
		}
	}

	private void OnStartAnimEffects(string animationName)
	{
		bool flag;
		if (this.CheckSkipStartEffectForAnimation(animationName))
		{
			return;
		}
		this._lastAnimationName = animationName;
		WeaponAnimEffectData effectData = this.GetEffectData(animationName);
		if (effectData == null)
		{
			return;
		}
		bool flag1 = false;
		if (this._currentEffect != null)
		{
			if (!this._currentEffect.particleSystems.SequenceEqual<ParticleSystem>(effectData.particleSystems))
			{
				flag = false;
			}
			else
			{
				flag = (!this._currentEffect.isLoop ? false : effectData.isLoop);
			}
			flag1 = flag;
			if (!flag1)
			{
				this.SetActiveEffect(this._currentEffect, false);
			}
		}
		this._currentEffect = effectData;
		if (effectData == null)
		{
			return;
		}
		if (!flag1)
		{
			this.SetActiveEffect(effectData, true);
			effectData.isPlaying = true;
		}
		if (!effectData.isLoop)
		{
			this._isCanStopNotLoopEffect = false;
			base.Invoke("ChangeEffectAfterStopAnimation", effectData.animationLength);
		}
	}

	private void SetActiveEffect(WeaponAnimEffectData effectData, bool active)
	{
		if (effectData == null || effectData.particleSystems == null)
		{
			return;
		}
		if (active && effectData.blockAtPlay && effectData.isPlaying)
		{
			return;
		}
		ParticleSystem[] particleSystemArray = effectData.particleSystems;
		for (int i = 0; i < (int)particleSystemArray.Length; i++)
		{
			ParticleSystem particleSystem = particleSystemArray[i];
			if (active)
			{
				particleSystem.gameObject.SetActive(true);
				if (effectData.EmitCount >= 0)
				{
					particleSystem.Emit(effectData.EmitCount);
				}
				else
				{
					particleSystem.Play();
				}
			}
			else if (effectData.EmitCount < 0)
			{
				particleSystem.gameObject.SetActive(false);
			}
		}
	}

	private void Start()
	{
		if (!this._isInit)
		{
			WeaponAnimEffectData[] weaponAnimEffectDataArray = this.effects;
			for (int i = 0; i < (int)weaponAnimEffectDataArray.Length; i++)
			{
				this.InitiAnimatonEventForEffect(weaponAnimEffectDataArray[i]);
			}
			this._isInit = true;
		}
	}
}