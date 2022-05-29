using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/NGUI Slider")]
[ExecuteInEditMode]
public class UISlider : UIProgressBar
{
	[HideInInspector]
	[SerializeField]
	private Transform foreground;

	[HideInInspector]
	[SerializeField]
	private float rawValue = 1f;

	[HideInInspector]
	[SerializeField]
	private UISlider.Direction direction = UISlider.Direction.Upgraded;

	[HideInInspector]
	[SerializeField]
	protected bool mInverted;

	[Obsolete("Use 'fillDirection' instead")]
	public bool inverted
	{
		get
		{
			return base.isInverted;
		}
		set
		{
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

	[Obsolete("Use 'value' instead")]
	public float sliderValue
	{
		get
		{
			return base.@value;
		}
		set
		{
			base.@value = value;
		}
	}

	public UISlider()
	{
	}

	protected void OnDragBackground(GameObject go, Vector2 delta)
	{
		if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
		{
			return;
		}
		this.mCam = UICamera.currentCamera;
		base.@value = base.ScreenToValue(UICamera.lastEventPosition);
	}

	protected void OnDragForeground(GameObject go, Vector2 delta)
	{
		if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
		{
			return;
		}
		this.mCam = UICamera.currentCamera;
		base.@value = this.mOffset + base.ScreenToValue(UICamera.lastEventPosition);
	}

	public override void OnPan(Vector2 delta)
	{
		if (base.enabled && this.isColliderEnabled)
		{
			base.OnPan(delta);
		}
	}

	protected void OnPressBackground(GameObject go, bool isPressed)
	{
		if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
		{
			return;
		}
		this.mCam = UICamera.currentCamera;
		base.@value = base.ScreenToValue(UICamera.lastEventPosition);
		if (!isPressed && this.onDragFinished != null)
		{
			this.onDragFinished();
		}
	}

	protected void OnPressForeground(GameObject go, bool isPressed)
	{
		if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
		{
			return;
		}
		this.mCam = UICamera.currentCamera;
		if (isPressed)
		{
			this.mOffset = (this.mFG != null ? base.@value - base.ScreenToValue(UICamera.lastEventPosition) : 0f);
		}
		else if (this.onDragFinished != null)
		{
			this.onDragFinished();
		}
	}

	protected override void OnStart()
	{
		UIEventListener boolDelegate = UIEventListener.Get((!(this.mBG != null) || !(this.mBG.GetComponent<Collider>() != null) && !(this.mBG.GetComponent<Collider2D>() != null) ? base.gameObject : this.mBG.gameObject));
		boolDelegate.onPress += new UIEventListener.BoolDelegate(this.OnPressBackground);
		boolDelegate.onDrag += new UIEventListener.VectorDelegate(this.OnDragBackground);
		if (this.thumb != null && (this.thumb.GetComponent<Collider>() != null || this.thumb.GetComponent<Collider2D>() != null) && (this.mFG == null || this.thumb != this.mFG.cachedTransform))
		{
			UIEventListener vectorDelegate = UIEventListener.Get(this.thumb.gameObject);
			vectorDelegate.onPress += new UIEventListener.BoolDelegate(this.OnPressForeground);
			vectorDelegate.onDrag += new UIEventListener.VectorDelegate(this.OnDragForeground);
		}
	}

	protected override void Upgrade()
	{
		if (this.direction != UISlider.Direction.Upgraded)
		{
			this.mValue = this.rawValue;
			if (this.foreground != null)
			{
				this.mFG = this.foreground.GetComponent<UIWidget>();
			}
			if (this.direction != UISlider.Direction.Horizontal)
			{
				this.mFill = (!this.mInverted ? UIProgressBar.FillDirection.BottomToTop : UIProgressBar.FillDirection.TopToBottom);
			}
			else
			{
				this.mFill = (!this.mInverted ? UIProgressBar.FillDirection.LeftToRight : UIProgressBar.FillDirection.RightToLeft);
			}
			this.direction = UISlider.Direction.Upgraded;
		}
	}

	private enum Direction
	{
		Horizontal,
		Vertical,
		Upgraded
	}
}