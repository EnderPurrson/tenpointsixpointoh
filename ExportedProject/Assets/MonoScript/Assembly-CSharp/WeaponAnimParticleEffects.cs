using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class WeaponAnimParticleEffects : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CInitiAnimatonEventForEffect_003Ec__AnonStorey303
	{
		internal WeaponAnimEffectData effectData;

		internal AnimationClip animationClip;

		internal bool _003C_003Em__477(AnimationEvent e)
		{
			return e.stringParameter == effectData.animationName && e.functionName == "OnStartAnimEffects" && Math.Abs(e.time) < 0.001f;
		}

		internal bool _003C_003Em__478(AnimationEvent e)
		{
			return e.stringParameter == effectData.animationName && e.functionName == "OnAnimationFinished" && Math.Abs(e.time - animationClip.length) < 0.001f;
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetEffectData_003Ec__AnonStorey304
	{
		internal string animationName;

		internal bool _003C_003Em__479(WeaponAnimEffectData e)
		{
			return e.animationName == animationName;
		}
	}

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
			if (_eo == null)
			{
				_eo = new List<GameObject>();
				WeaponAnimEffectData[] array = effects;
				foreach (WeaponAnimEffectData weaponAnimEffectData in array)
				{
					ParticleSystem[] particleSystems = weaponAnimEffectData.particleSystems;
					foreach (ParticleSystem particleSystem in particleSystems)
					{
						_eo.Add(particleSystem.gameObject);
					}
				}
			}
			return _eo;
		}
	}

	private void Start()
	{
		if (!_isInit)
		{
			WeaponAnimEffectData[] array = effects;
			foreach (WeaponAnimEffectData effectData in array)
			{
				InitiAnimatonEventForEffect(effectData);
			}
			_isInit = true;
		}
	}

	public List<GameObject> GetListAnimEffects()
	{
		return _effectObjects;
	}

	private void InitiAnimatonEventForEffect(WeaponAnimEffectData effectData)
	{
		_003CInitiAnimatonEventForEffect_003Ec__AnonStorey303 _003CInitiAnimatonEventForEffect_003Ec__AnonStorey = new _003CInitiAnimatonEventForEffect_003Ec__AnonStorey303();
		_003CInitiAnimatonEventForEffect_003Ec__AnonStorey.effectData = effectData;
		_003CInitiAnimatonEventForEffect_003Ec__AnonStorey.animationClip = GetComponent<Animation>().GetClip(_003CInitiAnimatonEventForEffect_003Ec__AnonStorey.effectData.animationName);
		if (!(_003CInitiAnimatonEventForEffect_003Ec__AnonStorey.animationClip == null))
		{
			if (!_003CInitiAnimatonEventForEffect_003Ec__AnonStorey.animationClip.events.Any(_003CInitiAnimatonEventForEffect_003Ec__AnonStorey._003C_003Em__477))
			{
				AnimationEvent animationEvent = new AnimationEvent();
				animationEvent.stringParameter = _003CInitiAnimatonEventForEffect_003Ec__AnonStorey.effectData.animationName;
				animationEvent.functionName = "OnStartAnimEffects";
				animationEvent.time = 0f;
				_003CInitiAnimatonEventForEffect_003Ec__AnonStorey.animationClip.AddEvent(animationEvent);
			}
			if (!_003CInitiAnimatonEventForEffect_003Ec__AnonStorey.animationClip.events.Any(_003CInitiAnimatonEventForEffect_003Ec__AnonStorey._003C_003Em__478))
			{
				AnimationEvent animationEvent2 = new AnimationEvent();
				animationEvent2.stringParameter = _003CInitiAnimatonEventForEffect_003Ec__AnonStorey.effectData.animationName;
				animationEvent2.functionName = "OnAnimationFinished";
				animationEvent2.time = _003CInitiAnimatonEventForEffect_003Ec__AnonStorey.animationClip.length;
				_003CInitiAnimatonEventForEffect_003Ec__AnonStorey.animationClip.AddEvent(animationEvent2);
			}
			_003CInitiAnimatonEventForEffect_003Ec__AnonStorey.effectData.animationLength = ((!_003CInitiAnimatonEventForEffect_003Ec__AnonStorey.effectData.isLoop) ? _003CInitiAnimatonEventForEffect_003Ec__AnonStorey.animationClip.length : 0f);
		}
	}

	public void DisableEffectForAnimation(string animName)
	{
		WeaponAnimEffectData effectData = GetEffectData(animName);
		if (effectData != null)
		{
			effectData.isPlaying = false;
			SetActiveEffect(effectData, false);
			_currentEffect = null;
		}
	}

	private void SetActiveEffect(WeaponAnimEffectData effectData, bool active)
	{
		if (effectData == null || effectData.particleSystems == null || (active && effectData.blockAtPlay && effectData.isPlaying))
		{
			return;
		}
		ParticleSystem[] particleSystems = effectData.particleSystems;
		foreach (ParticleSystem particleSystem in particleSystems)
		{
			if (active)
			{
				particleSystem.gameObject.SetActive(true);
				if (effectData.EmitCount < 0)
				{
					particleSystem.Play();
				}
				else
				{
					particleSystem.Emit(effectData.EmitCount);
				}
			}
			else if (effectData.EmitCount < 0)
			{
				particleSystem.gameObject.SetActive(false);
			}
		}
	}

	private WeaponAnimEffectData GetEffectData(string animationName)
	{
		_003CGetEffectData_003Ec__AnonStorey304 _003CGetEffectData_003Ec__AnonStorey = new _003CGetEffectData_003Ec__AnonStorey304();
		_003CGetEffectData_003Ec__AnonStorey.animationName = animationName;
		return effects.FirstOrDefault(_003CGetEffectData_003Ec__AnonStorey._003C_003Em__479);
	}

	private bool CheckSkipStartEffectForAnimation(string animationName)
	{
		if (_currentEffect == null)
		{
			return false;
		}
		if (_currentEffect.isLoop)
		{
			return _lastAnimationName == animationName;
		}
		WeaponAnimEffectData effectData = GetEffectData(animationName);
		if (effectData == null)
		{
			return false;
		}
		if (effectData != null && !effectData.isLoop)
		{
			CancelInvoke("ChangeEffectAfterStopAnimation");
			return false;
		}
		return !_isCanStopNotLoopEffect;
	}

	private void OnStartAnimEffects(string animationName)
	{
		if (CheckSkipStartEffectForAnimation(animationName))
		{
			return;
		}
		_lastAnimationName = animationName;
		WeaponAnimEffectData effectData = GetEffectData(animationName);
		if (effectData == null)
		{
			return;
		}
		bool flag = false;
		if (_currentEffect != null)
		{
			flag = _currentEffect.particleSystems.SequenceEqual(effectData.particleSystems) && _currentEffect.isLoop && effectData.isLoop;
			if (!flag)
			{
				SetActiveEffect(_currentEffect, false);
			}
		}
		_currentEffect = effectData;
		if (effectData != null)
		{
			if (!flag)
			{
				SetActiveEffect(effectData, true);
				effectData.isPlaying = true;
			}
			if (!effectData.isLoop)
			{
				_isCanStopNotLoopEffect = false;
				Invoke("ChangeEffectAfterStopAnimation", effectData.animationLength);
			}
		}
	}

	private void OnAnimationFinished(string animationName)
	{
		Debug.Log("===== finished '" + animationName + "'");
		WeaponAnimEffectData effectData = GetEffectData(animationName);
		if (effectData != null)
		{
			effectData.isPlaying = false;
		}
	}

	private string GetNamePlayingAnimation()
	{
		if (GetComponent<Animation>() == null)
		{
			return string.Empty;
		}
		foreach (AnimationState item in GetComponent<Animation>())
		{
			if (GetComponent<Animation>().IsPlaying(item.name))
			{
				return item.name;
			}
		}
		return string.Empty;
	}

	public void ChangeEffectAfterStopAnimation()
	{
		_isCanStopNotLoopEffect = true;
		string namePlayingAnimation = GetNamePlayingAnimation();
		if (!string.IsNullOrEmpty(namePlayingAnimation))
		{
			OnStartAnimEffects(namePlayingAnimation);
		}
	}
}
