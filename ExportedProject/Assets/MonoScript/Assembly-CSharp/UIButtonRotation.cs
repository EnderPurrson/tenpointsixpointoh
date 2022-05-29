using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Button Rotation")]
public class UIButtonRotation : MonoBehaviour
{
	public Transform tweenTarget;

	public Vector3 hover = Vector3.zero;

	public Vector3 pressed = Vector3.zero;

	public float duration = 0.2f;

	private Quaternion mRot;

	private bool mStarted;

	public UIButtonRotation()
	{
	}

	private void OnDisable()
	{
		if (this.mStarted && this.tweenTarget != null)
		{
			TweenRotation component = this.tweenTarget.GetComponent<TweenRotation>();
			if (component != null)
			{
				component.@value = this.mRot;
				component.enabled = false;
			}
		}
	}

	private void OnEnable()
	{
		if (this.mStarted)
		{
			this.OnHover(UICamera.IsHighlighted(base.gameObject));
		}
	}

	private void OnHover(bool isOver)
	{
		if (base.enabled)
		{
			if (!this.mStarted)
			{
				this.Start();
			}
			TweenRotation.Begin(this.tweenTarget.gameObject, this.duration, (!isOver ? this.mRot : this.mRot * Quaternion.Euler(this.hover))).method = UITweener.Method.EaseInOut;
		}
	}

	private void OnPress(bool isPressed)
	{
		Quaternion quaternion;
		if (base.enabled)
		{
			if (!this.mStarted)
			{
				this.Start();
			}
			GameObject gameObject = this.tweenTarget.gameObject;
			float single = this.duration;
			if (!isPressed)
			{
				quaternion = (!UICamera.IsHighlighted(base.gameObject) ? this.mRot : this.mRot * Quaternion.Euler(this.hover));
			}
			else
			{
				quaternion = this.mRot * Quaternion.Euler(this.pressed);
			}
			TweenRotation.Begin(gameObject, single, quaternion).method = UITweener.Method.EaseInOut;
		}
	}

	private void OnSelect(bool isSelected)
	{
		if (base.enabled && (!isSelected || UICamera.currentScheme == UICamera.ControlScheme.Controller))
		{
			this.OnHover(isSelected);
		}
	}

	private void Start()
	{
		if (!this.mStarted)
		{
			this.mStarted = true;
			if (this.tweenTarget == null)
			{
				this.tweenTarget = base.transform;
			}
			this.mRot = this.tweenTarget.localRotation;
		}
	}
}