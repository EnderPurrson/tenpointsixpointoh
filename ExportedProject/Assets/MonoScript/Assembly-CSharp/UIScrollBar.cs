using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/NGUI Scroll Bar")]
[ExecuteInEditMode]
public class UIScrollBar : UISlider
{
	[HideInInspector]
	[SerializeField]
	protected float mSize = 1f;

	[HideInInspector]
	[SerializeField]
	private float mScroll;

	[HideInInspector]
	[SerializeField]
	private UIScrollBar.Direction mDir = UIScrollBar.Direction.Upgraded;

	public float barSize
	{
		get
		{
			return this.mSize;
		}
		set
		{
			float single = Mathf.Clamp01(value);
			if (this.mSize != single)
			{
				this.mSize = single;
				this.mIsDirty = true;
				if (NGUITools.GetActive(this))
				{
					if (UIProgressBar.current == null && this.onChange != null)
					{
						UIProgressBar.current = this;
						EventDelegate.Execute(this.onChange);
						UIProgressBar.current = null;
					}
					this.ForceUpdate();
				}
			}
		}
	}

	[Obsolete("Use 'value' instead")]
	public float scrollValue
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

	public UIScrollBar()
	{
	}

	public override void ForceUpdate()
	{
		if (this.mFG == null)
		{
			base.ForceUpdate();
		}
		else
		{
			this.mIsDirty = false;
			float single = Mathf.Clamp01(this.mSize) * 0.5f;
			float single1 = Mathf.Lerp(single, 1f - single, base.@value);
			float single2 = single1 - single;
			float single3 = single1 + single;
			if (!base.isHorizontal)
			{
				this.mFG.drawRegion = (!base.isInverted ? new Vector4(0f, single2, 1f, single3) : new Vector4(0f, 1f - single3, 1f, 1f - single2));
			}
			else
			{
				this.mFG.drawRegion = (!base.isInverted ? new Vector4(single2, 0f, single3, 1f) : new Vector4(1f - single3, 0f, 1f - single2, 1f));
			}
			if (this.thumb != null)
			{
				Vector4 vector4 = this.mFG.drawingDimensions;
				Vector3 vector3 = new Vector3(Mathf.Lerp(vector4.x, vector4.z, 0.5f), Mathf.Lerp(vector4.y, vector4.w, 0.5f));
				base.SetThumbPosition(this.mFG.cachedTransform.TransformPoint(vector3));
			}
		}
	}

	protected override float LocalToValue(Vector2 localPos)
	{
		if (this.mFG == null)
		{
			return base.LocalToValue(localPos);
		}
		float single = Mathf.Clamp01(this.mSize) * 0.5f;
		float single1 = single;
		float single2 = 1f - single;
		Vector3[] vector3Array = this.mFG.localCorners;
		if (base.isHorizontal)
		{
			single1 = Mathf.Lerp(vector3Array[0].x, vector3Array[2].x, single1);
			single2 = Mathf.Lerp(vector3Array[0].x, vector3Array[2].x, single2);
			float single3 = single2 - single1;
			if (single3 == 0f)
			{
				return base.@value;
			}
			return (!base.isInverted ? (localPos.x - single1) / single3 : (single2 - localPos.x) / single3);
		}
		single1 = Mathf.Lerp(vector3Array[0].y, vector3Array[1].y, single1);
		single2 = Mathf.Lerp(vector3Array[3].y, vector3Array[2].y, single2);
		float single4 = single2 - single1;
		if (single4 == 0f)
		{
			return base.@value;
		}
		return (!base.isInverted ? (localPos.y - single1) / single4 : (single2 - localPos.y) / single4);
	}

	protected override void OnStart()
	{
		base.OnStart();
		if (this.mFG != null && this.mFG.gameObject != base.gameObject)
		{
			if ((this.mFG.GetComponent<Collider>() != null ? false : this.mFG.GetComponent<Collider2D>() == null))
			{
				return;
			}
			UIEventListener boolDelegate = UIEventListener.Get(this.mFG.gameObject);
			boolDelegate.onPress += new UIEventListener.BoolDelegate(this.OnPressForeground);
			boolDelegate.onDrag += new UIEventListener.VectorDelegate(this.OnDragForeground);
			this.mFG.autoResizeBoxCollider = true;
		}
	}

	protected override void Upgrade()
	{
		if (this.mDir != UIScrollBar.Direction.Upgraded)
		{
			this.mValue = this.mScroll;
			if (this.mDir != UIScrollBar.Direction.Horizontal)
			{
				this.mFill = (!this.mInverted ? UIProgressBar.FillDirection.TopToBottom : UIProgressBar.FillDirection.BottomToTop);
			}
			else
			{
				this.mFill = (!this.mInverted ? UIProgressBar.FillDirection.LeftToRight : UIProgressBar.FillDirection.RightToLeft);
			}
			this.mDir = UIScrollBar.Direction.Upgraded;
		}
	}

	private enum Direction
	{
		Horizontal,
		Vertical,
		Upgraded
	}
}