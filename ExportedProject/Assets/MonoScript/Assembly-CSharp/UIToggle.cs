using AnimationOrTween;
using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Toggle")]
[ExecuteInEditMode]
public class UIToggle : UIWidgetContainer
{
	public static BetterList<UIToggle> list;

	public static UIToggle current;

	public int @group;

	public UIWidget activeSprite;

	public Animation activeAnimation;

	public Animator animator;

	public UITweener tween;

	public bool startsActive;

	public bool instantTween;

	public bool optionCanBeNone;

	public List<EventDelegate> onChange = new List<EventDelegate>();

	public UIToggle.Validate validator;

	[HideInInspector]
	[SerializeField]
	private UISprite checkSprite;

	[HideInInspector]
	[SerializeField]
	private Animation checkAnimation;

	[HideInInspector]
	[SerializeField]
	private GameObject eventReceiver;

	[HideInInspector]
	[SerializeField]
	private string functionName = "OnActivate";

	[HideInInspector]
	[SerializeField]
	private bool startsChecked;

	private bool mIsActive = true;

	private bool mStarted;

	[Obsolete("Use 'value' instead")]
	public bool isChecked
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

	public bool isColliderEnabled
	{
		get
		{
			Collider component = base.GetComponent<Collider>();
			if (component != null)
			{
				return component.enabled;
			}
			Collider2D collider2D = base.GetComponent<Collider2D>();
			return (collider2D == null ? false : collider2D.enabled);
		}
	}

	public bool @value
	{
		get
		{
			return (!this.mStarted ? this.startsActive : this.mIsActive);
		}
		set
		{
			if (!this.mStarted)
			{
				this.startsActive = value;
			}
			else if (this.@group == 0 || value || this.optionCanBeNone || !this.mStarted)
			{
				this.Set(value);
			}
		}
	}

	static UIToggle()
	{
		UIToggle.list = new BetterList<UIToggle>();
	}

	public UIToggle()
	{
	}

	public static UIToggle GetActiveToggle(int group)
	{
		for (int i = 0; i < UIToggle.list.size; i++)
		{
			UIToggle item = UIToggle.list[i];
			if (item != null && item.@group == group && item.mIsActive)
			{
				return item;
			}
		}
		return null;
	}

	private void OnClick()
	{
		if (base.enabled && this.isColliderEnabled && UICamera.currentTouchID != -2)
		{
			this.@value = !this.@value;
		}
	}

	private void OnDisable()
	{
		UIToggle.list.Remove(this);
	}

	private void OnEnable()
	{
		UIToggle.list.Add(this);
	}

	public void Set(bool state)
	{
		if (this.validator != null && !this.validator(state))
		{
			return;
		}
		if (!this.mStarted)
		{
			this.mIsActive = state;
			this.startsActive = state;
			if (this.activeSprite != null)
			{
				this.activeSprite.alpha = (!state ? 0f : 1f);
			}
		}
		else if (this.mIsActive != state)
		{
			if (this.@group != 0 && state)
			{
				int num = 0;
				int num1 = UIToggle.list.size;
				while (num < num1)
				{
					UIToggle item = UIToggle.list[num];
					if (item != this && item.@group == this.@group)
					{
						item.Set(false);
					}
					if (UIToggle.list.size == num1)
					{
						num++;
					}
					else
					{
						num1 = UIToggle.list.size;
						num = 0;
					}
				}
			}
			this.mIsActive = state;
			if (this.activeSprite != null)
			{
				if (this.instantTween || !NGUITools.GetActive(this))
				{
					this.activeSprite.alpha = (!this.mIsActive ? 0f : 1f);
				}
				else
				{
					TweenAlpha.Begin(this.activeSprite.gameObject, 0.15f, (!this.mIsActive ? 0f : 1f));
				}
			}
			if (UIToggle.current == null)
			{
				UIToggle uIToggle = UIToggle.current;
				UIToggle.current = this;
				if (EventDelegate.IsValid(this.onChange))
				{
					EventDelegate.Execute(this.onChange);
				}
				else if (this.eventReceiver != null && !string.IsNullOrEmpty(this.functionName))
				{
					this.eventReceiver.SendMessage(this.functionName, this.mIsActive, SendMessageOptions.DontRequireReceiver);
				}
				UIToggle.current = uIToggle;
			}
			if (this.animator != null)
			{
				ActiveAnimation activeAnimation = ActiveAnimation.Play(this.animator, null, (!state ? Direction.Reverse : Direction.Forward), EnableCondition.IgnoreDisabledState, DisableCondition.DoNotDisable);
				if (activeAnimation != null && (this.instantTween || !NGUITools.GetActive(this)))
				{
					activeAnimation.Finish();
				}
			}
			else if (this.activeAnimation != null)
			{
				ActiveAnimation activeAnimation1 = ActiveAnimation.Play(this.activeAnimation, null, (!state ? Direction.Reverse : Direction.Forward), EnableCondition.IgnoreDisabledState, DisableCondition.DoNotDisable);
				if (activeAnimation1 != null && (this.instantTween || !NGUITools.GetActive(this)))
				{
					activeAnimation1.Finish();
				}
			}
			else if (this.tween != null)
			{
				bool active = NGUITools.GetActive(this);
				if (this.tween.tweenGroup == 0)
				{
					this.tween.Play(state);
					if (this.instantTween || !active)
					{
						this.tween.tweenFactor = (!state ? 0f : 1f);
					}
				}
				else
				{
					UITweener[] componentsInChildren = this.tween.GetComponentsInChildren<UITweener>();
					int num2 = 0;
					int length = (int)componentsInChildren.Length;
					while (num2 < length)
					{
						UITweener uITweener = componentsInChildren[num2];
						if (uITweener.tweenGroup == this.tween.tweenGroup)
						{
							uITweener.Play(state);
							if (this.instantTween || !active)
							{
								uITweener.tweenFactor = (!state ? 0f : 1f);
							}
						}
						num2++;
					}
				}
			}
		}
	}

	private void Start()
	{
		if (this.startsChecked)
		{
			this.startsChecked = false;
			this.startsActive = true;
		}
		if (Application.isPlaying)
		{
			this.mIsActive = !this.startsActive;
			this.mStarted = true;
			bool flag = this.instantTween;
			this.instantTween = true;
			this.Set(this.startsActive);
			this.instantTween = flag;
		}
		else
		{
			if (this.checkSprite != null && this.activeSprite == null)
			{
				this.activeSprite = this.checkSprite;
				this.checkSprite = null;
			}
			if (this.checkAnimation != null && this.activeAnimation == null)
			{
				this.activeAnimation = this.checkAnimation;
				this.checkAnimation = null;
			}
			if (Application.isPlaying && this.activeSprite != null)
			{
				this.activeSprite.alpha = (!this.startsActive ? 0f : 1f);
			}
			if (EventDelegate.IsValid(this.onChange))
			{
				this.eventReceiver = null;
				this.functionName = null;
			}
		}
	}

	public delegate bool Validate(bool choice);
}