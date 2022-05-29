using System;
using UnityEngine;

[AddComponentMenu("NGUI/Tween/Tween Position")]
public class TweenPosition : UITweener
{
	public Vector3 @from;

	public Vector3 to;

	[HideInInspector]
	public bool worldSpace;

	private Transform mTrans;

	private UIRect mRect;

	public Transform cachedTransform
	{
		get
		{
			if (this.mTrans == null)
			{
				this.mTrans = base.transform;
			}
			return this.mTrans;
		}
	}

	[Obsolete("Use 'value' instead")]
	public Vector3 position
	{
		get
		{
			return this.@value;
		}
		set
		{
			this.@value = value;
		}
	}

	public Vector3 @value
	{
		get
		{
			return (!this.worldSpace ? this.cachedTransform.localPosition : this.cachedTransform.position);
		}
		set
		{
			if (this.mRect != null && this.mRect.isAnchored && !this.worldSpace)
			{
				value -= this.cachedTransform.localPosition;
				NGUIMath.MoveRect(this.mRect, value.x, value.y);
			}
			else if (!this.worldSpace)
			{
				this.cachedTransform.localPosition = value;
			}
			else
			{
				this.cachedTransform.position = value;
			}
		}
	}

	public TweenPosition()
	{
	}

	private void Awake()
	{
		this.mRect = base.GetComponent<UIRect>();
	}

	public static TweenPosition Begin(GameObject go, float duration, Vector3 pos)
	{
		TweenPosition tweenPosition = UITweener.Begin<TweenPosition>(go, duration);
		tweenPosition.@from = tweenPosition.@value;
		tweenPosition.to = pos;
		if (duration <= 0f)
		{
			tweenPosition.Sample(1f, true);
			tweenPosition.enabled = false;
		}
		return tweenPosition;
	}

	public static TweenPosition Begin(GameObject go, float duration, Vector3 pos, bool worldSpace)
	{
		TweenPosition tweenPosition = UITweener.Begin<TweenPosition>(go, duration);
		tweenPosition.worldSpace = worldSpace;
		tweenPosition.@from = tweenPosition.@value;
		tweenPosition.to = pos;
		if (duration <= 0f)
		{
			tweenPosition.Sample(1f, true);
			tweenPosition.enabled = false;
		}
		return tweenPosition;
	}

	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.@value = (this.@from * (1f - factor)) + (this.to * factor);
	}

	[ContextMenu("Assume value of 'To'")]
	private void SetCurrentValueToEnd()
	{
		this.@value = this.to;
	}

	[ContextMenu("Assume value of 'From'")]
	private void SetCurrentValueToStart()
	{
		this.@value = this.@from;
	}

	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue()
	{
		this.to = this.@value;
	}

	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue()
	{
		this.@from = this.@value;
	}
}