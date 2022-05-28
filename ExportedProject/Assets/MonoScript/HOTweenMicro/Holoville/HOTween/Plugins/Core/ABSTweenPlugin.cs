using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using Holoville.HOTween.Core;
using Holoville.HOTween.Core.Easing;
using UnityEngine;

namespace Holoville.HOTween.Plugins.Core
{
	public abstract class ABSTweenPlugin
	{
		protected object _startVal;

		protected object _endVal;

		protected float _duration;

		private bool _initialized;

		private bool _easeReversed;

		protected string _propName;

		internal global::System.Type targetType;

		protected TweenDelegate.EaseFunc ease;

		protected bool isRelative;

		protected bool ignoreAccessor;

		private EaseType easeType;

		private EaseInfo easeInfo;

		private EaseCurve easeCurve;

		internal bool wasStarted;

		private bool speedBasedDurationWasSet;

		private int prevCompletedLoops;

		private bool _useSpeedTransformAccessors;

		private Transform _transformTarget;

		private TweenDelegate.HOAction<Vector3> _setTransformVector3;

		private TweenDelegate.HOFunc<Vector3> _getTransformVector3;

		private TweenDelegate.HOAction<Quaternion> _setTransformQuaternion;

		private TweenDelegate.HOFunc<Quaternion> _getTransformQuaternion;

		internal PropertyInfo propInfo;

		internal FieldInfo fieldInfo;

		protected Tweener tweenObj;

		protected abstract object startVal { get; set; }

		protected abstract object endVal { get; set; }

		internal bool initialized
		{
			get
			{
				return _initialized;
			}
		}

		internal float duration
		{
			get
			{
				return _duration;
			}
		}

		internal bool easeReversed
		{
			get
			{
				return _easeReversed;
			}
		}

		internal string propName
		{
			get
			{
				return _propName;
			}
		}

		internal virtual int pluginId
		{
			get
			{
				return -1;
			}
		}

		protected ABSTweenPlugin(object p_endVal, bool p_isRelative)
		{
			isRelative = p_isRelative;
			_endVal = p_endVal;
		}

		protected ABSTweenPlugin(object p_endVal, EaseType p_easeType, bool p_isRelative)
		{
			isRelative = p_isRelative;
			_endVal = p_endVal;
			easeType = p_easeType;
			easeInfo = EaseInfo.GetEaseInfo(p_easeType);
			ease = easeInfo.ease;
		}

		protected ABSTweenPlugin(object p_endVal, AnimationCurve p_easeAnimCurve, bool p_isRelative)
		{
			isRelative = p_isRelative;
			_endVal = p_endVal;
			easeType = EaseType.AnimationCurve;
			easeCurve = new EaseCurve(p_easeAnimCurve);
			easeInfo = null;
			ease = easeCurve.Evaluate;
		}

		internal virtual void Init(Tweener p_tweenObj, string p_propertyName, EaseType p_easeType, global::System.Type p_targetType, PropertyInfo p_propertyInfo, FieldInfo p_fieldInfo)
		{
			TweenDelegate.HOAction<Vector3> hOAction = null;
			TweenDelegate.HOFunc<Vector3> hOFunc = null;
			TweenDelegate.HOAction<Vector3> hOAction2 = null;
			TweenDelegate.HOFunc<Vector3> hOFunc2 = null;
			TweenDelegate.HOAction<Vector3> hOAction3 = null;
			TweenDelegate.HOFunc<Vector3> hOFunc3 = null;
			TweenDelegate.HOAction<Quaternion> hOAction4 = null;
			TweenDelegate.HOFunc<Quaternion> hOFunc4 = null;
			TweenDelegate.HOAction<Quaternion> hOAction5 = null;
			TweenDelegate.HOFunc<Quaternion> hOFunc5 = null;
			_initialized = true;
			tweenObj = p_tweenObj;
			_propName = p_propertyName;
			targetType = p_targetType;
			if ((easeType != EaseType.AnimationCurve && easeInfo == null) || tweenObj.speedBased || (easeType == EaseType.AnimationCurve && easeCurve == null))
			{
				SetEase(p_easeType);
			}
			_duration = tweenObj.duration;
			if (targetType == typeof(Transform))
			{
				_transformTarget = p_tweenObj.target as Transform;
				_useSpeedTransformAccessors = true;
				string text;
				if ((text = _propName) == null)
				{
					goto IL_0217;
				}
				if (!(text == "position"))
				{
					if (!(text == "localPosition"))
					{
						if (!(text == "localScale"))
						{
							if (!(text == "rotation"))
							{
								if (!(text == "localRotation"))
								{
									goto IL_0217;
								}
								if (hOAction5 == null)
								{
									hOAction5 = _003CInit_003Eb__8;
								}
								_setTransformQuaternion = hOAction5;
								if (hOFunc5 == null)
								{
									hOFunc5 = _003CInit_003Eb__9;
								}
								_getTransformQuaternion = hOFunc5;
							}
							else
							{
								if (hOAction4 == null)
								{
									hOAction4 = _003CInit_003Eb__6;
								}
								_setTransformQuaternion = hOAction4;
								if (hOFunc4 == null)
								{
									hOFunc4 = _003CInit_003Eb__7;
								}
								_getTransformQuaternion = hOFunc4;
							}
						}
						else
						{
							if (hOAction3 == null)
							{
								hOAction3 = _003CInit_003Eb__4;
							}
							_setTransformVector3 = hOAction3;
							if (hOFunc3 == null)
							{
								hOFunc3 = _003CInit_003Eb__5;
							}
							_getTransformVector3 = hOFunc3;
						}
					}
					else
					{
						if (hOAction2 == null)
						{
							hOAction2 = _003CInit_003Eb__2;
						}
						_setTransformVector3 = hOAction2;
						if (hOFunc2 == null)
						{
							hOFunc2 = _003CInit_003Eb__3;
						}
						_getTransformVector3 = hOFunc2;
					}
				}
				else
				{
					if (hOAction == null)
					{
						hOAction = _003CInit_003Eb__0;
					}
					_setTransformVector3 = hOAction;
					if (hOFunc == null)
					{
						hOFunc = _003CInit_003Eb__1;
					}
					_getTransformVector3 = hOFunc;
				}
			}
			goto IL_0225;
			IL_0225:
			if (!_useSpeedTransformAccessors)
			{
				propInfo = p_propertyInfo;
				fieldInfo = p_fieldInfo;
			}
			return;
			IL_0217:
			_transformTarget = null;
			_useSpeedTransformAccessors = false;
			goto IL_0225;
		}

		internal void Startup()
		{
			Startup(false);
		}

		internal void Startup(bool p_onlyCalcSpeedBasedDur)
		{
			if (wasStarted)
			{
				TweenWarning.Log(string.Concat(new object[5] { "Startup() for plugin ", this, " (target: ", tweenObj.target, ") has already been called. Startup() won't execute twice." }));
				return;
			}
			object obj = null;
			object obj2 = null;
			if (p_onlyCalcSpeedBasedDur)
			{
				if (tweenObj.speedBased && !speedBasedDurationWasSet)
				{
					obj = _startVal;
					obj2 = _endVal;
				}
			}
			else
			{
				wasStarted = true;
			}
			if (tweenObj.isFrom)
			{
				object obj3 = _endVal;
				endVal = GetValue();
				startVal = obj3;
			}
			else
			{
				endVal = _endVal;
				startVal = GetValue();
			}
			SetChangeVal();
			if (tweenObj.speedBased && !speedBasedDurationWasSet)
			{
				_duration = GetSpeedBasedDuration(_duration);
				speedBasedDurationWasSet = true;
				if (p_onlyCalcSpeedBasedDur)
				{
					_startVal = obj;
					_endVal = obj2;
				}
			}
		}

		internal void ForceSetSpeedBasedDuration()
		{
			if (!speedBasedDurationWasSet)
			{
				Startup(true);
			}
		}

		internal virtual bool ValidateTarget(object p_target)
		{
			return true;
		}

		internal void Update(float p_totElapsed)
		{
			if (tweenObj.loopType == LoopType.Incremental)
			{
				if (prevCompletedLoops != tweenObj.completedLoops)
				{
					int num = tweenObj.completedLoops;
					if (tweenObj._loops != -1 && num >= tweenObj._loops)
					{
						num--;
					}
					int num2 = num - prevCompletedLoops;
					if (num2 != 0)
					{
						SetIncremental(num2);
						prevCompletedLoops = num;
					}
				}
			}
			else if (prevCompletedLoops != 0)
			{
				SetIncremental(-prevCompletedLoops);
				prevCompletedLoops = 0;
			}
			if (p_totElapsed > _duration)
			{
				p_totElapsed = _duration;
			}
			DoUpdate(p_totElapsed);
		}

		protected abstract void DoUpdate(float p_totElapsed);

		internal virtual void Rewind()
		{
			SetValue(startVal);
		}

		internal virtual void Complete()
		{
			SetValue(_endVal);
		}

		internal void ReverseEase()
		{
			_easeReversed = !_easeReversed;
			if (easeType != EaseType.AnimationCurve && easeInfo.inverseEase != null)
			{
				ease = (_easeReversed ? easeInfo.inverseEase : easeInfo.ease);
			}
		}

		internal void SetEase(EaseType p_easeType)
		{
			easeType = p_easeType;
			if (easeType == EaseType.AnimationCurve)
			{
				if (tweenObj._easeAnimationCurve != null)
				{
					easeCurve = new EaseCurve(tweenObj._easeAnimationCurve);
					ease = easeCurve.Evaluate;
				}
				else
				{
					easeType = EaseType.EaseOutQuad;
					easeInfo = EaseInfo.GetEaseInfo(easeType);
					ease = easeInfo.ease;
				}
			}
			else
			{
				easeInfo = EaseInfo.GetEaseInfo(easeType);
				ease = easeInfo.ease;
			}
			if (_easeReversed && easeInfo.inverseEase != null)
			{
				ease = easeInfo.inverseEase;
			}
		}

		protected abstract float GetSpeedBasedDuration(float p_speed);

		internal ABSTweenPlugin CloneBasic()
		{
			return Activator.CreateInstance(base.GetType(), new object[3]
			{
				(tweenObj != null && tweenObj.isFrom) ? _startVal : _endVal,
				easeType,
				isRelative
			}) as ABSTweenPlugin;
		}

		protected abstract void SetChangeVal();

		internal void ForceSetIncremental(int p_diffIncr)
		{
			SetIncremental(p_diffIncr);
		}

		protected abstract void SetIncremental(int p_diffIncr);

		protected virtual void SetValue(object p_value)
		{
			if (_useSpeedTransformAccessors)
			{
				if (_setTransformVector3 != null)
				{
					_setTransformVector3((Vector3)p_value);
				}
				else
				{
					_setTransformQuaternion((Quaternion)p_value);
				}
				return;
			}
			if (propInfo != null)
			{
				try
				{
					propInfo.SetValue(tweenObj.target, p_value, (object[])default(object[]));
					return;
				}
				catch (InvalidCastException)
				{
					propInfo.SetValue(tweenObj.target, (object)(int)Math.Floor((double)(float)p_value), (object[])default(object[]));
					return;
				}
				catch (ArgumentException)
				{
					propInfo.SetValue(tweenObj.target, (object)(int)Math.Floor((double)(float)p_value), (object[])default(object[]));
					return;
				}
			}
			try
			{
				fieldInfo.SetValue(tweenObj.target, p_value);
			}
			catch (InvalidCastException)
			{
				fieldInfo.SetValue(tweenObj.target, (object)(int)Math.Floor((double)(float)p_value));
			}
			catch (ArgumentException)
			{
				fieldInfo.SetValue(tweenObj.target, (object)(int)Math.Floor((double)(float)p_value));
			}
		}

		protected virtual object GetValue()
		{
			if (_useSpeedTransformAccessors)
			{
				if (_getTransformVector3 != null)
				{
					return _getTransformVector3();
				}
				return _getTransformQuaternion();
			}
			if (propInfo != null)
			{
				return ((MethodBase)propInfo.GetGetMethod()).Invoke(tweenObj.target, (object[])default(object[]));
			}
			return fieldInfo.GetValue(tweenObj.target);
		}

		[CompilerGenerated]
		private void _003CInit_003Eb__0(Vector3 value)
		{
			_transformTarget.position = value;
		}

		[CompilerGenerated]
		private Vector3 _003CInit_003Eb__1()
		{
			return _transformTarget.position;
		}

		[CompilerGenerated]
		private void _003CInit_003Eb__2(Vector3 value)
		{
			_transformTarget.localPosition = value;
		}

		[CompilerGenerated]
		private Vector3 _003CInit_003Eb__3()
		{
			return _transformTarget.localPosition;
		}

		[CompilerGenerated]
		private void _003CInit_003Eb__4(Vector3 value)
		{
			_transformTarget.localScale = value;
		}

		[CompilerGenerated]
		private Vector3 _003CInit_003Eb__5()
		{
			return _transformTarget.localScale;
		}

		[CompilerGenerated]
		private void _003CInit_003Eb__6(Quaternion value)
		{
			_transformTarget.rotation = value;
		}

		[CompilerGenerated]
		private Quaternion _003CInit_003Eb__7()
		{
			return _transformTarget.rotation;
		}

		[CompilerGenerated]
		private void _003CInit_003Eb__8(Quaternion value)
		{
			_transformTarget.localRotation = value;
		}

		[CompilerGenerated]
		private Quaternion _003CInit_003Eb__9()
		{
			return _transformTarget.localRotation;
		}
	}
}
