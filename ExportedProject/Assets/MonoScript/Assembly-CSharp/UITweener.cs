using AnimationOrTween;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class UITweener : MonoBehaviour
{
	public static UITweener current;

	[HideInInspector]
	public UITweener.Method method;

	[HideInInspector]
	public UITweener.Style style;

	[HideInInspector]
	public AnimationCurve animationCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f) });

	[HideInInspector]
	public bool ignoreTimeScale = true;

	[HideInInspector]
	public float delay;

	[HideInInspector]
	public float duration = 1f;

	[HideInInspector]
	public bool steeperCurves;

	[HideInInspector]
	public int tweenGroup;

	[HideInInspector]
	public List<EventDelegate> onFinished = new List<EventDelegate>();

	[HideInInspector]
	public GameObject eventReceiver;

	[HideInInspector]
	public string callWhenFinished;

	private bool mStarted;

	private float mStartTime;

	private float mDuration;

	private float mAmountPerDelta = 1000f;

	private float mFactor;

	private List<EventDelegate> mTemp;

	public float amountPerDelta
	{
		get
		{
			if (this.mDuration != this.duration)
			{
				this.mDuration = this.duration;
				this.mAmountPerDelta = Mathf.Abs((this.duration <= 0f ? 1000f : 1f / this.duration)) * Mathf.Sign(this.mAmountPerDelta);
			}
			return this.mAmountPerDelta;
		}
	}

	public Direction direction
	{
		get
		{
			return (this.amountPerDelta >= 0f ? Direction.Forward : Direction.Reverse);
		}
	}

	public float tweenFactor
	{
		get
		{
			return this.mFactor;
		}
		set
		{
			this.mFactor = Mathf.Clamp01(value);
		}
	}

	protected UITweener()
	{
	}

	public void AddOnFinished(EventDelegate.Callback del)
	{
		EventDelegate.Add(this.onFinished, del);
	}

	public void AddOnFinished(EventDelegate del)
	{
		EventDelegate.Add(this.onFinished, del);
	}

	public static T Begin<T>(GameObject go, float duration)
	where T : UITweener
	{
		T component = go.GetComponent<T>();
		if (component != null && component.tweenGroup != 0)
		{
			component = (T)null;
			T[] components = go.GetComponents<T>();
			int num = 0;
			int length = (int)components.Length;
			while (num < length)
			{
				component = components[num];
				if (!(component != null) || component.tweenGroup != 0)
				{
					component = (T)null;
					num++;
				}
				else
				{
					break;
				}
			}
		}
		if (component == null)
		{
			component = go.AddComponent<T>();
			if (component == null)
			{
				Debug.LogError(string.Concat(new object[] { "Unable to add ", typeof(T), " to ", NGUITools.GetHierarchy(go) }), go);
				return (T)null;
			}
		}
		component.mStarted = false;
		component.duration = duration;
		component.mFactor = 0f;
		component.mAmountPerDelta = Mathf.Abs(component.amountPerDelta);
		component.style = UITweener.Style.Once;
		component.animationCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f) });
		component.eventReceiver = null;
		component.callWhenFinished = null;
		component.enabled = true;
		return component;
	}

	private float BounceLogic(float val)
	{
		if (val < 0.363636f)
		{
			val = 7.5685f * val * val;
		}
		else if (val < 0.727272f)
		{
			float single = val - 0.545454f;
			val = single;
			val = 7.5625f * single * val + 0.75f;
		}
		else if (val >= 0.90909f)
		{
			float single1 = val - 0.9545454f;
			val = single1;
			val = 7.5625f * single1 * val + 0.984375f;
		}
		else
		{
			float single2 = val - 0.818181f;
			val = single2;
			val = 7.5625f * single2 * val + 0.9375f;
		}
		return val;
	}

	private void OnDisable()
	{
		this.mStarted = false;
	}

	protected abstract void OnUpdate(float factor, bool isFinished);

	[Obsolete("Use PlayForward() instead")]
	public void Play()
	{
		this.Play(true);
	}

	public void Play(bool forward)
	{
		this.mAmountPerDelta = Mathf.Abs(this.amountPerDelta);
		if (!forward)
		{
			this.mAmountPerDelta = -this.mAmountPerDelta;
		}
		base.enabled = true;
		this.Update();
	}

	public void PlayForward()
	{
		this.Play(true);
	}

	public void PlayReverse()
	{
		this.Play(false);
	}

	public void RemoveOnFinished(EventDelegate del)
	{
		if (this.onFinished != null)
		{
			this.onFinished.Remove(del);
		}
		if (this.mTemp != null)
		{
			this.mTemp.Remove(del);
		}
	}

	private void Reset()
	{
		if (!this.mStarted)
		{
			this.SetStartToCurrentValue();
			this.SetEndToCurrentValue();
		}
	}

	public void ResetToBeginning()
	{
		this.mStarted = false;
		this.mFactor = (this.amountPerDelta >= 0f ? 0f : 1f);
		this.Sample(this.mFactor, false);
	}

	public void Sample(float factor, bool isFinished)
	{
		float single = Mathf.Clamp01(factor);
		if (this.method == UITweener.Method.EaseIn)
		{
			single = 1f - Mathf.Sin(1.5707964f * (1f - single));
			if (this.steeperCurves)
			{
				single *= single;
			}
		}
		else if (this.method == UITweener.Method.EaseOut)
		{
			single = Mathf.Sin(1.5707964f * single);
			if (this.steeperCurves)
			{
				single = 1f - single;
				single = 1f - single * single;
			}
		}
		else if (this.method == UITweener.Method.EaseInOut)
		{
			single = single - Mathf.Sin(single * 6.2831855f) / 6.2831855f;
			if (this.steeperCurves)
			{
				single = single * 2f - 1f;
				float single1 = Mathf.Sign(single);
				single = 1f - Mathf.Abs(single);
				single = 1f - single * single;
				single = single1 * single * 0.5f + 0.5f;
			}
		}
		else if (this.method == UITweener.Method.BounceIn)
		{
			single = this.BounceLogic(single);
		}
		else if (this.method == UITweener.Method.BounceOut)
		{
			single = 1f - this.BounceLogic(1f - single);
		}
		this.OnUpdate((this.animationCurve == null ? single : this.animationCurve.Evaluate(single)), isFinished);
	}

	public virtual void SetEndToCurrentValue()
	{
	}

	public void SetOnFinished(EventDelegate.Callback del)
	{
		EventDelegate.Set(this.onFinished, del);
	}

	public void SetOnFinished(EventDelegate del)
	{
		EventDelegate.Set(this.onFinished, del);
	}

	public virtual void SetStartToCurrentValue()
	{
	}

	protected virtual void Start()
	{
		this.Update();
	}

	public void Toggle()
	{
		if (this.mFactor <= 0f)
		{
			this.mAmountPerDelta = Mathf.Abs(this.amountPerDelta);
		}
		else
		{
			this.mAmountPerDelta = -this.amountPerDelta;
		}
		base.enabled = true;
	}

	private void Update()
	{
		float single = (!this.ignoreTimeScale ? Time.deltaTime : RealTime.deltaTime);
		float single1 = (!this.ignoreTimeScale ? Time.time : RealTime.time);
		if (!this.mStarted)
		{
			this.mStarted = true;
			this.mStartTime = single1 + this.delay;
		}
		if (single1 < this.mStartTime)
		{
			return;
		}
		UITweener uITweener = this;
		uITweener.mFactor = uITweener.mFactor + this.amountPerDelta * single;
		if (this.style == UITweener.Style.Loop)
		{
			if (this.mFactor > 1f)
			{
				this.mFactor -= Mathf.Floor(this.mFactor);
			}
		}
		else if (this.style == UITweener.Style.PingPong)
		{
			if (this.mFactor > 1f)
			{
				this.mFactor = 1f - (this.mFactor - Mathf.Floor(this.mFactor));
				this.mAmountPerDelta = -this.mAmountPerDelta;
			}
			else if (this.mFactor < 0f)
			{
				this.mFactor = -this.mFactor;
				this.mFactor -= Mathf.Floor(this.mFactor);
				this.mAmountPerDelta = -this.mAmountPerDelta;
			}
		}
		if (this.style != UITweener.Style.Once || this.duration != 0f && this.mFactor <= 1f && this.mFactor >= 0f)
		{
			this.Sample(this.mFactor, false);
		}
		else
		{
			this.mFactor = Mathf.Clamp01(this.mFactor);
			this.Sample(this.mFactor, true);
			base.enabled = false;
			if (UITweener.current != this)
			{
				UITweener uITweener1 = UITweener.current;
				UITweener.current = this;
				if (this.onFinished != null)
				{
					this.mTemp = this.onFinished;
					this.onFinished = new List<EventDelegate>();
					EventDelegate.Execute(this.mTemp);
					for (int i = 0; i < this.mTemp.Count; i++)
					{
						EventDelegate item = this.mTemp[i];
						if (item != null && !item.oneShot)
						{
							EventDelegate.Add(this.onFinished, item, item.oneShot);
						}
					}
					this.mTemp = null;
				}
				if (this.eventReceiver != null && !string.IsNullOrEmpty(this.callWhenFinished))
				{
					this.eventReceiver.SendMessage(this.callWhenFinished, this, SendMessageOptions.DontRequireReceiver);
				}
				UITweener.current = uITweener1;
			}
		}
	}

	public enum Method
	{
		Linear,
		EaseIn,
		EaseOut,
		EaseInOut,
		BounceIn,
		BounceOut
	}

	public enum Style
	{
		Once,
		Loop,
		PingPong
	}
}