using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/NGUI Progress Bar")]
[ExecuteInEditMode]
public class UIProgressBar : UIWidgetContainer
{
	public static UIProgressBar current;

	public UIProgressBar.OnDragFinished onDragFinished;

	public Transform thumb;

	[HideInInspector]
	[SerializeField]
	protected UIWidget mBG;

	[HideInInspector]
	[SerializeField]
	protected UIWidget mFG;

	[HideInInspector]
	[SerializeField]
	protected float mValue = 1f;

	[HideInInspector]
	[SerializeField]
	protected UIProgressBar.FillDirection mFill;

	protected Transform mTrans;

	protected bool mIsDirty;

	protected Camera mCam;

	protected float mOffset;

	public int numberOfSteps;

	public List<EventDelegate> onChange = new List<EventDelegate>();

	public float alpha
	{
		get
		{
			if (this.mFG != null)
			{
				return this.mFG.alpha;
			}
			if (this.mBG == null)
			{
				return 1f;
			}
			return this.mBG.alpha;
		}
		set
		{
			if (this.mFG != null)
			{
				this.mFG.alpha = value;
				if (this.mFG.GetComponent<Collider>() != null)
				{
					this.mFG.GetComponent<Collider>().enabled = this.mFG.alpha > 0.001f;
				}
				else if (this.mFG.GetComponent<Collider2D>() != null)
				{
					this.mFG.GetComponent<Collider2D>().enabled = this.mFG.alpha > 0.001f;
				}
			}
			if (this.mBG != null)
			{
				this.mBG.alpha = value;
				if (this.mBG.GetComponent<Collider>() != null)
				{
					this.mBG.GetComponent<Collider>().enabled = this.mBG.alpha > 0.001f;
				}
				else if (this.mBG.GetComponent<Collider2D>() != null)
				{
					this.mBG.GetComponent<Collider2D>().enabled = this.mBG.alpha > 0.001f;
				}
			}
			if (this.thumb != null)
			{
				UIWidget component = this.thumb.GetComponent<UIWidget>();
				if (component != null)
				{
					component.alpha = value;
					if (component.GetComponent<Collider>() != null)
					{
						component.GetComponent<Collider>().enabled = component.alpha > 0.001f;
					}
					else if (component.GetComponent<Collider2D>() != null)
					{
						component.GetComponent<Collider2D>().enabled = component.alpha > 0.001f;
					}
				}
			}
		}
	}

	public UIWidget backgroundWidget
	{
		get
		{
			return this.mBG;
		}
		set
		{
			if (this.mBG != value)
			{
				this.mBG = value;
				this.mIsDirty = true;
			}
		}
	}

	public Camera cachedCamera
	{
		get
		{
			if (this.mCam == null)
			{
				this.mCam = NGUITools.FindCameraForLayer(base.gameObject.layer);
			}
			return this.mCam;
		}
	}

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

	public UIProgressBar.FillDirection fillDirection
	{
		get
		{
			return this.mFill;
		}
		set
		{
			if (this.mFill != value)
			{
				this.mFill = value;
				this.ForceUpdate();
			}
		}
	}

	public UIWidget foregroundWidget
	{
		get
		{
			return this.mFG;
		}
		set
		{
			if (this.mFG != value)
			{
				this.mFG = value;
				this.mIsDirty = true;
			}
		}
	}

	protected bool isHorizontal
	{
		get
		{
			return (this.mFill == UIProgressBar.FillDirection.LeftToRight ? true : this.mFill == UIProgressBar.FillDirection.RightToLeft);
		}
	}

	protected bool isInverted
	{
		get
		{
			return (this.mFill == UIProgressBar.FillDirection.RightToLeft ? true : this.mFill == UIProgressBar.FillDirection.TopToBottom);
		}
	}

	public float @value
	{
		get
		{
			if (this.numberOfSteps <= 1)
			{
				return this.mValue;
			}
			return Mathf.Round(this.mValue * (float)(this.numberOfSteps - 1)) / (float)(this.numberOfSteps - 1);
		}
		set
		{
			float single = Mathf.Clamp01(value);
			if (this.mValue != single)
			{
				float single1 = this.@value;
				this.mValue = single;
				if (single1 != this.@value)
				{
					this.ForceUpdate();
					if (NGUITools.GetActive(this) && EventDelegate.IsValid(this.onChange))
					{
						UIProgressBar.current = this;
						EventDelegate.Execute(this.onChange);
						UIProgressBar.current = null;
					}
				}
			}
		}
	}

	public UIProgressBar()
	{
	}

	public virtual void ForceUpdate()
	{
		this.mIsDirty = false;
		bool flag = false;
		if (this.mFG != null)
		{
			UIBasicSprite uIBasicSprite = this.mFG as UIBasicSprite;
			if (this.isHorizontal)
			{
				if (!(uIBasicSprite != null) || uIBasicSprite.type != UIBasicSprite.Type.Filled)
				{
					this.mFG.drawRegion = (!this.isInverted ? new Vector4(0f, 0f, this.@value, 1f) : new Vector4(1f - this.@value, 0f, 1f, 1f));
					this.mFG.enabled = true;
					flag = this.@value < 0.001f;
				}
				else
				{
					if (uIBasicSprite.fillDirection == UIBasicSprite.FillDirection.Horizontal || uIBasicSprite.fillDirection == UIBasicSprite.FillDirection.Vertical)
					{
						uIBasicSprite.fillDirection = UIBasicSprite.FillDirection.Horizontal;
						uIBasicSprite.invert = this.isInverted;
					}
					uIBasicSprite.fillAmount = this.@value;
				}
			}
			else if (!(uIBasicSprite != null) || uIBasicSprite.type != UIBasicSprite.Type.Filled)
			{
				this.mFG.drawRegion = (!this.isInverted ? new Vector4(0f, 0f, 1f, this.@value) : new Vector4(0f, 1f - this.@value, 1f, 1f));
				this.mFG.enabled = true;
				flag = this.@value < 0.001f;
			}
			else
			{
				if (uIBasicSprite.fillDirection == UIBasicSprite.FillDirection.Horizontal || uIBasicSprite.fillDirection == UIBasicSprite.FillDirection.Vertical)
				{
					uIBasicSprite.fillDirection = UIBasicSprite.FillDirection.Vertical;
					uIBasicSprite.invert = this.isInverted;
				}
				uIBasicSprite.fillAmount = this.@value;
			}
		}
		if (this.thumb != null && (this.mFG != null || this.mBG != null))
		{
			Vector3[] vector3Array = (this.mFG == null ? this.mBG.localCorners : this.mFG.localCorners);
			Vector4 vector4 = (this.mFG == null ? this.mBG.border : this.mFG.border);
			vector3Array[0].x += vector4.x;
			vector3Array[1].x += vector4.x;
			vector3Array[2].x -= vector4.z;
			vector3Array[3].x -= vector4.z;
			vector3Array[0].y += vector4.y;
			vector3Array[1].y -= vector4.w;
			vector3Array[2].y -= vector4.w;
			vector3Array[3].y += vector4.y;
			Transform transforms = (this.mFG == null ? this.mBG.cachedTransform : this.mFG.cachedTransform);
			for (int i = 0; i < 4; i++)
			{
				vector3Array[i] = transforms.TransformPoint(vector3Array[i]);
			}
			if (!this.isHorizontal)
			{
				Vector3 vector3 = Vector3.Lerp(vector3Array[0], vector3Array[3], 0.5f);
				Vector3 vector31 = Vector3.Lerp(vector3Array[1], vector3Array[2], 0.5f);
				this.SetThumbPosition(Vector3.Lerp(vector3, vector31, (!this.isInverted ? this.@value : 1f - this.@value)));
			}
			else
			{
				Vector3 vector32 = Vector3.Lerp(vector3Array[0], vector3Array[1], 0.5f);
				Vector3 vector33 = Vector3.Lerp(vector3Array[2], vector3Array[3], 0.5f);
				this.SetThumbPosition(Vector3.Lerp(vector32, vector33, (!this.isInverted ? this.@value : 1f - this.@value)));
			}
		}
		if (flag)
		{
			this.mFG.enabled = false;
		}
	}

	protected virtual float LocalToValue(Vector2 localPos)
	{
		if (this.mFG == null)
		{
			return this.@value;
		}
		Vector3[] vector3Array = this.mFG.localCorners;
		Vector3 vector3 = vector3Array[2] - vector3Array[0];
		if (this.isHorizontal)
		{
			float single = (localPos.x - vector3Array[0].x) / vector3.x;
			return (!this.isInverted ? single : 1f - single);
		}
		float single1 = (localPos.y - vector3Array[0].y) / vector3.y;
		return (!this.isInverted ? single1 : 1f - single1);
	}

	public virtual void OnPan(Vector2 delta)
	{
		if (base.enabled)
		{
			switch (this.mFill)
			{
				case UIProgressBar.FillDirection.LeftToRight:
				{
					float single = Mathf.Clamp01(this.mValue + delta.x);
					this.@value = single;
					this.mValue = single;
					break;
				}
				case UIProgressBar.FillDirection.RightToLeft:
				{
					float single1 = Mathf.Clamp01(this.mValue - delta.x);
					this.@value = single1;
					this.mValue = single1;
					break;
				}
				case UIProgressBar.FillDirection.BottomToTop:
				{
					float single2 = Mathf.Clamp01(this.mValue + delta.y);
					this.@value = single2;
					this.mValue = single2;
					break;
				}
				case UIProgressBar.FillDirection.TopToBottom:
				{
					float single3 = Mathf.Clamp01(this.mValue - delta.y);
					this.@value = single3;
					this.mValue = single3;
					break;
				}
			}
		}
	}

	protected virtual void OnStart()
	{
	}

	protected void OnValidate()
	{
		if (!NGUITools.GetActive(this))
		{
			float single = Mathf.Clamp01(this.mValue);
			if (this.mValue != single)
			{
				this.mValue = single;
			}
			if (this.numberOfSteps < 0)
			{
				this.numberOfSteps = 0;
			}
			else if (this.numberOfSteps > 20)
			{
				this.numberOfSteps = 20;
			}
		}
		else
		{
			this.Upgrade();
			this.mIsDirty = true;
			float single1 = Mathf.Clamp01(this.mValue);
			if (this.mValue != single1)
			{
				this.mValue = single1;
			}
			if (this.numberOfSteps < 0)
			{
				this.numberOfSteps = 0;
			}
			else if (this.numberOfSteps > 20)
			{
				this.numberOfSteps = 20;
			}
			this.ForceUpdate();
		}
	}

	protected float ScreenToValue(Vector2 screenPos)
	{
		float single;
		Transform transforms = this.cachedTransform;
		Plane plane = new Plane(transforms.rotation * Vector3.back, transforms.position);
		Ray ray = this.cachedCamera.ScreenPointToRay(screenPos);
		if (!plane.Raycast(ray, out single))
		{
			return this.@value;
		}
		return this.LocalToValue(transforms.InverseTransformPoint(ray.GetPoint(single)));
	}

	protected void SetThumbPosition(Vector3 worldPos)
	{
		Transform transforms = this.thumb.parent;
		if (transforms != null)
		{
			worldPos = transforms.InverseTransformPoint(worldPos);
			worldPos.x = Mathf.Round(worldPos.x);
			worldPos.y = Mathf.Round(worldPos.y);
			worldPos.z = 0f;
			if (Vector3.Distance(this.thumb.localPosition, worldPos) > 0.001f)
			{
				this.thumb.localPosition = worldPos;
			}
		}
		else if (Vector3.Distance(this.thumb.position, worldPos) > 1E-05f)
		{
			this.thumb.position = worldPos;
		}
	}

	protected void Start()
	{
		this.Upgrade();
		if (Application.isPlaying)
		{
			if (this.mBG != null)
			{
				this.mBG.autoResizeBoxCollider = true;
			}
			this.OnStart();
			if (UIProgressBar.current == null && this.onChange != null)
			{
				UIProgressBar.current = this;
				EventDelegate.Execute(this.onChange);
				UIProgressBar.current = null;
			}
		}
		this.ForceUpdate();
	}

	protected void Update()
	{
		if (this.mIsDirty)
		{
			this.ForceUpdate();
		}
	}

	protected virtual void Upgrade()
	{
	}

	public enum FillDirection
	{
		LeftToRight,
		RightToLeft,
		BottomToTop,
		TopToBottom
	}

	public delegate void OnDragFinished();
}