using System;
using System.Diagnostics;
using UnityEngine;

[AddComponentMenu("NGUI/UI/NGUI Widget")]
[ExecuteInEditMode]
public class UIWidget : UIRect
{
	[HideInInspector]
	[SerializeField]
	protected Color mColor = Color.white;

	[HideInInspector]
	[SerializeField]
	protected UIWidget.Pivot mPivot = UIWidget.Pivot.Center;

	[HideInInspector]
	[SerializeField]
	protected int mWidth = 100;

	[HideInInspector]
	[SerializeField]
	protected int mHeight = 100;

	[HideInInspector]
	[SerializeField]
	protected int mDepth;

	public UIWidget.OnDimensionsChanged onChange;

	public UIWidget.OnPostFillCallback onPostFill;

	public UIDrawCall.OnRenderCallback mOnRender;

	public bool autoResizeBoxCollider;

	public bool hideIfOffScreen;

	public UIWidget.AspectRatioSource keepAspectRatio;

	public float aspectRatio = 1f;

	public UIWidget.HitCheck hitCheck;

	[NonSerialized]
	public UIPanel panel;

	[NonSerialized]
	public UIGeometry geometry = new UIGeometry();

	[NonSerialized]
	public bool fillGeometry = true;

	[NonSerialized]
	protected bool mPlayMode = true;

	[NonSerialized]
	protected Vector4 mDrawRegion = new Vector4(0f, 0f, 1f, 1f);

	[NonSerialized]
	private Matrix4x4 mLocalToPanel;

	[NonSerialized]
	private bool mIsVisibleByAlpha = true;

	[NonSerialized]
	private bool mIsVisibleByPanel = true;

	[NonSerialized]
	private bool mIsInFront = true;

	[NonSerialized]
	private float mLastAlpha;

	[NonSerialized]
	private bool mMoved;

	[NonSerialized]
	public UIDrawCall drawCall;

	[NonSerialized]
	protected Vector3[] mCorners = new Vector3[4];

	[NonSerialized]
	private int mAlphaFrameID = -1;

	private int mMatrixFrame = -1;

	private Vector3 mOldV0;

	private Vector3 mOldV1;

	public override float alpha
	{
		get
		{
			return this.mColor.a;
		}
		set
		{
			if (this.mColor.a != value)
			{
				this.mColor.a = value;
				this.Invalidate(true);
			}
		}
	}

	public virtual Vector4 border
	{
		get
		{
			return Vector4.zero;
		}
		set
		{
		}
	}

	public Color color
	{
		get
		{
			return this.mColor;
		}
		set
		{
			if (this.mColor != value)
			{
				bool flag = this.mColor.a != value.a;
				this.mColor = value;
				this.Invalidate(flag);
			}
		}
	}

	public int depth
	{
		get
		{
			return this.mDepth;
		}
		set
		{
			if (this.mDepth != value)
			{
				if (this.panel != null)
				{
					this.panel.RemoveWidget(this);
				}
				this.mDepth = value;
				if (this.panel != null)
				{
					this.panel.AddWidget(this);
					if (!Application.isPlaying)
					{
						this.panel.SortWidgets();
						this.panel.RebuildAllDrawCalls();
					}
				}
			}
		}
	}

	public virtual Vector4 drawingDimensions
	{
		get
		{
			Vector2 vector2 = this.pivotOffset;
			float single = -vector2.x * (float)this.mWidth;
			float single1 = -vector2.y * (float)this.mHeight;
			float single2 = single + (float)this.mWidth;
			float single3 = single1 + (float)this.mHeight;
			return new Vector4((this.mDrawRegion.x != 0f ? Mathf.Lerp(single, single2, this.mDrawRegion.x) : single), (this.mDrawRegion.y != 0f ? Mathf.Lerp(single1, single3, this.mDrawRegion.y) : single1), (this.mDrawRegion.z != 1f ? Mathf.Lerp(single, single2, this.mDrawRegion.z) : single2), (this.mDrawRegion.w != 1f ? Mathf.Lerp(single1, single3, this.mDrawRegion.w) : single3));
		}
	}

	public Vector4 drawRegion
	{
		get
		{
			return this.mDrawRegion;
		}
		set
		{
			if (this.mDrawRegion != value)
			{
				this.mDrawRegion = value;
				if (this.autoResizeBoxCollider)
				{
					this.ResizeCollider();
				}
				this.MarkAsChanged();
			}
		}
	}

	public bool hasBoxCollider
	{
		get
		{
			if (base.GetComponent<Collider>() is BoxCollider)
			{
				return true;
			}
			return base.GetComponent<BoxCollider2D>() != null;
		}
	}

	public bool hasVertices
	{
		get
		{
			return (this.geometry == null ? false : this.geometry.hasVertices);
		}
	}

	public int height
	{
		get
		{
			return this.mHeight;
		}
		set
		{
			int num = this.minHeight;
			if (value < num)
			{
				value = num;
			}
			if (this.mHeight != value && this.keepAspectRatio != UIWidget.AspectRatioSource.BasedOnWidth)
			{
				if (!this.isAnchoredVertically)
				{
					this.SetDimensions(this.mWidth, value);
				}
				else if (!(this.bottomAnchor.target != null) || !(this.topAnchor.target != null))
				{
					if (this.bottomAnchor.target == null)
					{
						NGUIMath.AdjustWidget(this, 0f, (float)(this.mHeight - value), 0f, 0f);
					}
					else
					{
						NGUIMath.AdjustWidget(this, 0f, 0f, 0f, (float)(value - this.mHeight));
					}
				}
				else if (this.mPivot == UIWidget.Pivot.BottomLeft || this.mPivot == UIWidget.Pivot.Bottom || this.mPivot == UIWidget.Pivot.BottomRight)
				{
					NGUIMath.AdjustWidget(this, 0f, 0f, 0f, (float)(value - this.mHeight));
				}
				else if (this.mPivot == UIWidget.Pivot.TopLeft || this.mPivot == UIWidget.Pivot.Top || this.mPivot == UIWidget.Pivot.TopRight)
				{
					NGUIMath.AdjustWidget(this, 0f, (float)(this.mHeight - value), 0f, 0f);
				}
				else
				{
					int num1 = value - this.mHeight;
					num1 = num1 - (num1 & 1);
					if (num1 != 0)
					{
						NGUIMath.AdjustWidget(this, 0f, (float)(-num1) * 0.5f, 0f, (float)num1 * 0.5f);
					}
				}
			}
		}
	}

	public bool isVisible
	{
		get
		{
			return (!this.mIsVisibleByPanel || !this.mIsVisibleByAlpha || !this.mIsInFront || this.finalAlpha <= 0.001f ? false : NGUITools.GetActive(this));
		}
	}

	public Vector3 localCenter
	{
		get
		{
			Vector3[] vector3Array = this.localCorners;
			return Vector3.Lerp(vector3Array[0], vector3Array[2], 0.5f);
		}
	}

	public override Vector3[] localCorners
	{
		get
		{
			Vector2 vector2 = this.pivotOffset;
			float single = -vector2.x * (float)this.mWidth;
			float single1 = -vector2.y * (float)this.mHeight;
			float single2 = single + (float)this.mWidth;
			float single3 = single1 + (float)this.mHeight;
			this.mCorners[0] = new Vector3(single, single1);
			this.mCorners[1] = new Vector3(single, single3);
			this.mCorners[2] = new Vector3(single2, single3);
			this.mCorners[3] = new Vector3(single2, single1);
			return this.mCorners;
		}
	}

	public virtual Vector2 localSize
	{
		get
		{
			Vector3[] vector3Array = this.localCorners;
			return vector3Array[2] - vector3Array[0];
		}
	}

	public virtual Texture mainTexture
	{
		get
		{
			Texture texture;
			Material material = this.material;
			if (material == null)
			{
				texture = null;
			}
			else
			{
				texture = material.mainTexture;
			}
			return texture;
		}
		set
		{
			throw new NotImplementedException(string.Concat(base.GetType(), " has no mainTexture setter"));
		}
	}

	public virtual Material material
	{
		get
		{
			return null;
		}
		set
		{
			throw new NotImplementedException(string.Concat(base.GetType(), " has no material setter"));
		}
	}

	public virtual int minHeight
	{
		get
		{
			return 2;
		}
	}

	public virtual int minWidth
	{
		get
		{
			return 2;
		}
	}

	public UIDrawCall.OnRenderCallback onRender
	{
		get
		{
			return this.mOnRender;
		}
		set
		{
			if (this.mOnRender != value)
			{
				if (this.drawCall != null && this.drawCall.onRender != null && this.mOnRender != null)
				{
					this.drawCall.onRender -= this.mOnRender;
				}
				this.mOnRender = value;
				if (this.drawCall != null)
				{
					this.drawCall.onRender += value;
				}
			}
		}
	}

	public UIWidget.Pivot pivot
	{
		get
		{
			return this.mPivot;
		}
		set
		{
			if (this.mPivot != value)
			{
				Vector3 vector3 = this.worldCorners[0];
				this.mPivot = value;
				this.mChanged = true;
				Vector3 vector31 = this.worldCorners[0];
				Transform transforms = base.cachedTransform;
				Vector3 vector32 = transforms.position;
				float single = transforms.localPosition.z;
				vector32.x = vector32.x + (vector3.x - vector31.x);
				vector32.y = vector32.y + (vector3.y - vector31.y);
				base.cachedTransform.position = vector32;
				vector32 = base.cachedTransform.localPosition;
				vector32.x = Mathf.Round(vector32.x);
				vector32.y = Mathf.Round(vector32.y);
				vector32.z = single;
				base.cachedTransform.localPosition = vector32;
			}
		}
	}

	public Vector2 pivotOffset
	{
		get
		{
			return NGUIMath.GetPivotOffset(this.pivot);
		}
	}

	public UIWidget.Pivot rawPivot
	{
		get
		{
			return this.mPivot;
		}
		set
		{
			if (this.mPivot != value)
			{
				this.mPivot = value;
				if (this.autoResizeBoxCollider)
				{
					this.ResizeCollider();
				}
				this.MarkAsChanged();
			}
		}
	}

	public int raycastDepth
	{
		get
		{
			if (this.panel == null)
			{
				this.CreatePanel();
			}
			return (this.panel == null ? this.mDepth : this.mDepth + this.panel.depth * 1000);
		}
	}

	[Obsolete("There is no relative scale anymore. Widgets now have width and height instead")]
	public Vector2 relativeSize
	{
		get
		{
			return Vector2.one;
		}
	}

	public virtual Shader shader
	{
		get
		{
			Shader shader;
			Material material = this.material;
			if (material == null)
			{
				shader = null;
			}
			else
			{
				shader = material.shader;
			}
			return shader;
		}
		set
		{
			throw new NotImplementedException(string.Concat(base.GetType(), " has no shader setter"));
		}
	}

	public int width
	{
		get
		{
			return this.mWidth;
		}
		set
		{
			int num = this.minWidth;
			if (value < num)
			{
				value = num;
			}
			if (this.mWidth != value && this.keepAspectRatio != UIWidget.AspectRatioSource.BasedOnHeight)
			{
				if (!this.isAnchoredHorizontally)
				{
					this.SetDimensions(value, this.mHeight);
				}
				else if (!(this.leftAnchor.target != null) || !(this.rightAnchor.target != null))
				{
					if (this.leftAnchor.target == null)
					{
						NGUIMath.AdjustWidget(this, (float)(this.mWidth - value), 0f, 0f, 0f);
					}
					else
					{
						NGUIMath.AdjustWidget(this, 0f, 0f, (float)(value - this.mWidth), 0f);
					}
				}
				else if (this.mPivot == UIWidget.Pivot.BottomLeft || this.mPivot == UIWidget.Pivot.Left || this.mPivot == UIWidget.Pivot.TopLeft)
				{
					NGUIMath.AdjustWidget(this, 0f, 0f, (float)(value - this.mWidth), 0f);
				}
				else if (this.mPivot == UIWidget.Pivot.BottomRight || this.mPivot == UIWidget.Pivot.Right || this.mPivot == UIWidget.Pivot.TopRight)
				{
					NGUIMath.AdjustWidget(this, (float)(this.mWidth - value), 0f, 0f, 0f);
				}
				else
				{
					int num1 = value - this.mWidth;
					num1 = num1 - (num1 & 1);
					if (num1 != 0)
					{
						NGUIMath.AdjustWidget(this, (float)(-num1) * 0.5f, 0f, (float)num1 * 0.5f, 0f);
					}
				}
			}
		}
	}

	public Vector3 worldCenter
	{
		get
		{
			return base.cachedTransform.TransformPoint(this.localCenter);
		}
	}

	public override Vector3[] worldCorners
	{
		get
		{
			Vector2 vector2 = this.pivotOffset;
			float single = -vector2.x * (float)this.mWidth;
			float single1 = -vector2.y * (float)this.mHeight;
			float single2 = single + (float)this.mWidth;
			float single3 = single1 + (float)this.mHeight;
			Transform transforms = base.cachedTransform;
			this.mCorners[0] = transforms.TransformPoint(single, single1, 0f);
			this.mCorners[1] = transforms.TransformPoint(single, single3, 0f);
			this.mCorners[2] = transforms.TransformPoint(single2, single3, 0f);
			this.mCorners[3] = transforms.TransformPoint(single2, single1, 0f);
			return this.mCorners;
		}
	}

	public UIWidget()
	{
	}

	protected override void Awake()
	{
		base.Awake();
		this.mPlayMode = Application.isPlaying;
	}

	public Bounds CalculateBounds()
	{
		return this.CalculateBounds(null);
	}

	public Bounds CalculateBounds(Transform relativeParent)
	{
		if (relativeParent == null)
		{
			Vector3[] vector3Array = this.localCorners;
			Bounds bound = new Bounds(vector3Array[0], Vector3.zero);
			for (int i = 1; i < 4; i++)
			{
				bound.Encapsulate(vector3Array[i]);
			}
			return bound;
		}
		Matrix4x4 matrix4x4 = relativeParent.worldToLocalMatrix;
		Vector3[] vector3Array1 = this.worldCorners;
		Bounds bound1 = new Bounds(matrix4x4.MultiplyPoint3x4(vector3Array1[0]), Vector3.zero);
		for (int j = 1; j < 4; j++)
		{
			bound1.Encapsulate(matrix4x4.MultiplyPoint3x4(vector3Array1[j]));
		}
		return bound1;
	}

	public float CalculateCumulativeAlpha(int frameID)
	{
		UIRect uIRect = base.parent;
		return (uIRect == null ? this.mColor.a : uIRect.CalculateFinalAlpha(frameID) * this.mColor.a);
	}

	public override float CalculateFinalAlpha(int frameID)
	{
		if (this.mAlphaFrameID != frameID)
		{
			this.mAlphaFrameID = frameID;
			this.UpdateFinalAlpha(frameID);
		}
		return this.finalAlpha;
	}

	public void CheckLayer()
	{
		if (this.panel != null && this.panel.gameObject.layer != base.gameObject.layer)
		{
			UnityEngine.Debug.LogWarning("You can't place widgets on a layer different than the UIPanel that manages them.\nIf you want to move widgets to a different layer, parent them to a new panel instead.", this);
			base.gameObject.layer = this.panel.gameObject.layer;
		}
	}

	public UIPanel CreatePanel()
	{
		if (this.mStarted && this.panel == null && base.enabled && NGUITools.GetActive(base.gameObject))
		{
			this.panel = UIPanel.Find(base.cachedTransform, true, base.cachedGameObject.layer);
			if (this.panel != null)
			{
				this.mParentFound = false;
				this.panel.AddWidget(this);
				this.CheckLayer();
				this.Invalidate(true);
			}
		}
		return this.panel;
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	public static int FullCompareFunc(UIWidget left, UIWidget right)
	{
		int num = UIPanel.CompareFunc(left.panel, right.panel);
		return (num != 0 ? num : UIWidget.PanelCompareFunc(left, right));
	}

	public override Vector3[] GetSides(Transform relativeTo)
	{
		Vector2 vector2 = this.pivotOffset;
		float single = -vector2.x * (float)this.mWidth;
		float single1 = -vector2.y * (float)this.mHeight;
		float single2 = single + (float)this.mWidth;
		float single3 = single1 + (float)this.mHeight;
		float single4 = (single + single2) * 0.5f;
		float single5 = (single1 + single3) * 0.5f;
		Transform transforms = base.cachedTransform;
		this.mCorners[0] = transforms.TransformPoint(single, single5, 0f);
		this.mCorners[1] = transforms.TransformPoint(single4, single3, 0f);
		this.mCorners[2] = transforms.TransformPoint(single2, single5, 0f);
		this.mCorners[3] = transforms.TransformPoint(single4, single1, 0f);
		if (relativeTo != null)
		{
			for (int i = 0; i < 4; i++)
			{
				this.mCorners[i] = relativeTo.InverseTransformPoint(this.mCorners[i]);
			}
		}
		return this.mCorners;
	}

	public override void Invalidate(bool includeChildren)
	{
		this.mChanged = true;
		this.mAlphaFrameID = -1;
		if (this.panel != null)
		{
			bool flag = (this.hideIfOffScreen || this.panel.hasCumulativeClipping ? this.panel.IsVisible(this) : true);
			this.UpdateVisibility(this.CalculateCumulativeAlpha(Time.frameCount) > 0.001f, flag);
			this.UpdateFinalAlpha(Time.frameCount);
			if (includeChildren)
			{
				base.Invalidate(true);
			}
		}
	}

	public virtual void MakePixelPerfect()
	{
		Vector3 vector3 = base.cachedTransform.localPosition;
		vector3.z = Mathf.Round(vector3.z);
		vector3.x = Mathf.Round(vector3.x);
		vector3.y = Mathf.Round(vector3.y);
		base.cachedTransform.localPosition = vector3;
		Vector3 vector31 = base.cachedTransform.localScale;
		base.cachedTransform.localScale = new Vector3(Mathf.Sign(vector31.x), Mathf.Sign(vector31.y), 1f);
	}

	public virtual void MarkAsChanged()
	{
		if (NGUITools.GetActive(this))
		{
			this.mChanged = true;
			if (this.panel != null && base.enabled && NGUITools.GetActive(base.gameObject) && !this.mPlayMode)
			{
				this.SetDirty();
				this.CheckLayer();
			}
		}
	}

	protected override void OnAnchor()
	{
		float single;
		float single1;
		float single2;
		float single3;
		Transform transforms = base.cachedTransform;
		Transform transforms1 = transforms.parent;
		Vector3 vector3 = transforms.localPosition;
		Vector2 vector2 = this.pivotOffset;
		if (!(this.leftAnchor.target == this.bottomAnchor.target) || !(this.leftAnchor.target == this.rightAnchor.target) || !(this.leftAnchor.target == this.topAnchor.target))
		{
			this.mIsInFront = true;
			if (!this.leftAnchor.target)
			{
				single = vector3.x - vector2.x * (float)this.mWidth;
			}
			else
			{
				Vector3[] sides = this.leftAnchor.GetSides(transforms1);
				if (sides == null)
				{
					Vector3 localPos = base.GetLocalPos(this.leftAnchor, transforms1);
					single = localPos.x + (float)this.leftAnchor.absolute;
				}
				else
				{
					single = NGUIMath.Lerp(sides[0].x, sides[2].x, this.leftAnchor.relative) + (float)this.leftAnchor.absolute;
				}
			}
			if (!this.rightAnchor.target)
			{
				single2 = vector3.x - vector2.x * (float)this.mWidth + (float)this.mWidth;
			}
			else
			{
				Vector3[] vector3Array = this.rightAnchor.GetSides(transforms1);
				if (vector3Array == null)
				{
					Vector3 localPos1 = base.GetLocalPos(this.rightAnchor, transforms1);
					single2 = localPos1.x + (float)this.rightAnchor.absolute;
				}
				else
				{
					single2 = NGUIMath.Lerp(vector3Array[0].x, vector3Array[2].x, this.rightAnchor.relative) + (float)this.rightAnchor.absolute;
				}
			}
			if (!this.bottomAnchor.target)
			{
				single1 = vector3.y - vector2.y * (float)this.mHeight;
			}
			else
			{
				Vector3[] sides1 = this.bottomAnchor.GetSides(transforms1);
				if (sides1 == null)
				{
					Vector3 vector31 = base.GetLocalPos(this.bottomAnchor, transforms1);
					single1 = vector31.y + (float)this.bottomAnchor.absolute;
				}
				else
				{
					single1 = NGUIMath.Lerp(sides1[3].y, sides1[1].y, this.bottomAnchor.relative) + (float)this.bottomAnchor.absolute;
				}
			}
			if (!this.topAnchor.target)
			{
				single3 = vector3.y - vector2.y * (float)this.mHeight + (float)this.mHeight;
			}
			else
			{
				Vector3[] vector3Array1 = this.topAnchor.GetSides(transforms1);
				if (vector3Array1 == null)
				{
					Vector3 localPos2 = base.GetLocalPos(this.topAnchor, transforms1);
					single3 = localPos2.y + (float)this.topAnchor.absolute;
				}
				else
				{
					single3 = NGUIMath.Lerp(vector3Array1[3].y, vector3Array1[1].y, this.topAnchor.relative) + (float)this.topAnchor.absolute;
				}
			}
		}
		else
		{
			Vector3[] sides2 = this.leftAnchor.GetSides(transforms1);
			if (sides2 == null)
			{
				Vector3 vector32 = base.GetLocalPos(this.leftAnchor, transforms1);
				single = vector32.x + (float)this.leftAnchor.absolute;
				single1 = vector32.y + (float)this.bottomAnchor.absolute;
				single2 = vector32.x + (float)this.rightAnchor.absolute;
				single3 = vector32.y + (float)this.topAnchor.absolute;
				this.mIsInFront = (!this.hideIfOffScreen ? true : vector32.z >= 0f);
			}
			else
			{
				single = NGUIMath.Lerp(sides2[0].x, sides2[2].x, this.leftAnchor.relative) + (float)this.leftAnchor.absolute;
				single2 = NGUIMath.Lerp(sides2[0].x, sides2[2].x, this.rightAnchor.relative) + (float)this.rightAnchor.absolute;
				single1 = NGUIMath.Lerp(sides2[3].y, sides2[1].y, this.bottomAnchor.relative) + (float)this.bottomAnchor.absolute;
				single3 = NGUIMath.Lerp(sides2[3].y, sides2[1].y, this.topAnchor.relative) + (float)this.topAnchor.absolute;
				this.mIsInFront = true;
			}
		}
		Vector3 vector33 = new Vector3(Mathf.Lerp(single, single2, vector2.x), Mathf.Lerp(single1, single3, vector2.y), vector3.z)
		{
			x = Mathf.Round(vector33.x),
			y = Mathf.Round(vector33.y)
		};
		int num = Mathf.FloorToInt(single2 - single + 0.5f);
		int num1 = Mathf.FloorToInt(single3 - single1 + 0.5f);
		if (this.keepAspectRatio != UIWidget.AspectRatioSource.Free && this.aspectRatio != 0f)
		{
			if (this.keepAspectRatio != UIWidget.AspectRatioSource.BasedOnHeight)
			{
				num1 = Mathf.RoundToInt((float)num / this.aspectRatio);
			}
			else
			{
				num = Mathf.RoundToInt((float)num1 * this.aspectRatio);
			}
		}
		if (num < this.minWidth)
		{
			num = this.minWidth;
		}
		if (num1 < this.minHeight)
		{
			num1 = this.minHeight;
		}
		if (Vector3.SqrMagnitude(vector3 - vector33) > 0.001f)
		{
			base.cachedTransform.localPosition = vector33;
			if (this.mIsInFront)
			{
				this.mChanged = true;
			}
		}
		if (this.mWidth != num || this.mHeight != num1)
		{
			this.mWidth = num;
			this.mHeight = num1;
			if (this.mIsInFront)
			{
				this.mChanged = true;
			}
			if (this.autoResizeBoxCollider)
			{
				this.ResizeCollider();
			}
		}
	}

	private void OnApplicationPause(bool paused)
	{
		if (!paused)
		{
			this.MarkAsChanged();
		}
	}

	private void OnDestroy()
	{
		this.RemoveFromPanel();
	}

	protected override void OnDisable()
	{
		this.RemoveFromPanel();
		base.OnDisable();
	}

	public virtual void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
	}

	protected override void OnInit()
	{
		base.OnInit();
		this.RemoveFromPanel();
		this.mMoved = true;
		if (this.mWidth == 100 && this.mHeight == 100 && base.cachedTransform.localScale.magnitude > 8f)
		{
			this.UpgradeFrom265();
			base.cachedTransform.localScale = Vector3.one;
		}
		base.Update();
	}

	protected override void OnStart()
	{
		this.CreatePanel();
	}

	protected override void OnUpdate()
	{
		if (this.panel == null)
		{
			this.CreatePanel();
		}
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	public static int PanelCompareFunc(UIWidget left, UIWidget right)
	{
		if (left.mDepth < right.mDepth)
		{
			return -1;
		}
		if (left.mDepth > right.mDepth)
		{
			return 1;
		}
		Material material = left.material;
		Material material1 = right.material;
		if (material == material1)
		{
			return 0;
		}
		if (material == null)
		{
			return 1;
		}
		if (material1 == null)
		{
			return -1;
		}
		return (material.GetInstanceID() >= material1.GetInstanceID() ? 1 : -1);
	}

	public override void ParentHasChanged()
	{
		base.ParentHasChanged();
		if (this.panel != null)
		{
			UIPanel uIPanel = UIPanel.Find(base.cachedTransform, true, base.cachedGameObject.layer);
			if (this.panel != uIPanel)
			{
				this.RemoveFromPanel();
				this.CreatePanel();
			}
		}
	}

	public void RemoveFromPanel()
	{
		if (this.panel != null)
		{
			this.panel.RemoveWidget(this);
			this.panel = null;
		}
		this.drawCall = null;
	}

	public void ResizeCollider()
	{
		if (NGUITools.GetActive(this))
		{
			NGUITools.UpdateWidgetCollider(base.gameObject);
		}
	}

	public void SetDimensions(int w, int h)
	{
		if (this.mWidth != w || this.mHeight != h)
		{
			this.mWidth = w;
			this.mHeight = h;
			if (this.keepAspectRatio == UIWidget.AspectRatioSource.BasedOnWidth)
			{
				this.mHeight = Mathf.RoundToInt((float)this.mWidth / this.aspectRatio);
			}
			else if (this.keepAspectRatio == UIWidget.AspectRatioSource.BasedOnHeight)
			{
				this.mWidth = Mathf.RoundToInt((float)this.mHeight * this.aspectRatio);
			}
			else if (this.keepAspectRatio == UIWidget.AspectRatioSource.Free)
			{
				this.aspectRatio = (float)this.mWidth / (float)this.mHeight;
			}
			this.mMoved = true;
			if (this.autoResizeBoxCollider)
			{
				this.ResizeCollider();
			}
			this.MarkAsChanged();
		}
	}

	public void SetDirty()
	{
		if (this.drawCall != null)
		{
			this.drawCall.isDirty = true;
		}
		else if (this.isVisible && this.hasVertices)
		{
			this.CreatePanel();
		}
	}

	public override void SetRect(float x, float y, float width, float height)
	{
		Vector2 vector2 = this.pivotOffset;
		float single = Mathf.Lerp(x, x + width, vector2.x);
		float single1 = Mathf.Lerp(y, y + height, vector2.y);
		int num = Mathf.FloorToInt(width + 0.5f);
		int num1 = Mathf.FloorToInt(height + 0.5f);
		if (vector2.x == 0.5f)
		{
			num = num >> 1 << 1;
		}
		if (vector2.y == 0.5f)
		{
			num1 = num1 >> 1 << 1;
		}
		Transform transforms = base.cachedTransform;
		Vector3 vector3 = transforms.localPosition;
		vector3.x = Mathf.Floor(single + 0.5f);
		vector3.y = Mathf.Floor(single1 + 0.5f);
		if (num < this.minWidth)
		{
			num = this.minWidth;
		}
		if (num1 < this.minHeight)
		{
			num1 = this.minHeight;
		}
		transforms.localPosition = vector3;
		this.width = num;
		this.height = num1;
		if (base.isAnchored)
		{
			transforms = transforms.parent;
			if (this.leftAnchor.target)
			{
				this.leftAnchor.SetHorizontal(transforms, x);
			}
			if (this.rightAnchor.target)
			{
				this.rightAnchor.SetHorizontal(transforms, x + width);
			}
			if (this.bottomAnchor.target)
			{
				this.bottomAnchor.SetVertical(transforms, y);
			}
			if (this.topAnchor.target)
			{
				this.topAnchor.SetVertical(transforms, y + height);
			}
		}
	}

	protected void UpdateFinalAlpha(int frameID)
	{
		if (!this.mIsVisibleByAlpha || !this.mIsInFront)
		{
			this.finalAlpha = 0f;
		}
		else
		{
			UIRect uIRect = base.parent;
			this.finalAlpha = (uIRect == null ? this.mColor.a : uIRect.CalculateFinalAlpha(frameID) * this.mColor.a);
		}
	}

	public bool UpdateGeometry(int frame)
	{
		float single = this.CalculateFinalAlpha(frame);
		if (this.mIsVisibleByAlpha && this.mLastAlpha != single)
		{
			this.mChanged = true;
		}
		this.mLastAlpha = single;
		if (this.mChanged)
		{
			if (this.mIsVisibleByAlpha && single > 0.001f && this.shader != null)
			{
				bool flag = this.geometry.hasVertices;
				if (this.fillGeometry)
				{
					this.geometry.Clear();
					this.OnFill(this.geometry.verts, this.geometry.uvs, this.geometry.cols);
				}
				if (!this.geometry.hasVertices)
				{
					this.mChanged = false;
					return flag;
				}
				if (this.mMatrixFrame != frame)
				{
					this.mLocalToPanel = this.panel.worldToLocal * base.cachedTransform.localToWorldMatrix;
					this.mMatrixFrame = frame;
				}
				this.geometry.ApplyTransform(this.mLocalToPanel, this.panel.generateNormals);
				this.mMoved = false;
				this.mChanged = false;
				return true;
			}
			if (this.geometry.hasVertices)
			{
				if (this.fillGeometry)
				{
					this.geometry.Clear();
				}
				this.mMoved = false;
				this.mChanged = false;
				return true;
			}
		}
		else if (this.mMoved && this.geometry.hasVertices)
		{
			if (this.mMatrixFrame != frame)
			{
				this.mLocalToPanel = this.panel.worldToLocal * base.cachedTransform.localToWorldMatrix;
				this.mMatrixFrame = frame;
			}
			this.geometry.ApplyTransform(this.mLocalToPanel, this.panel.generateNormals);
			this.mMoved = false;
			this.mChanged = false;
			return true;
		}
		this.mMoved = false;
		this.mChanged = false;
		return false;
	}

	public bool UpdateTransform(int frame)
	{
		Transform transforms = base.cachedTransform;
		this.mPlayMode = Application.isPlaying;
		if (this.mMoved)
		{
			this.mMoved = true;
			this.mMatrixFrame = -1;
			transforms.hasChanged = false;
			Vector2 vector2 = this.pivotOffset;
			float single = -vector2.x * (float)this.mWidth;
			float single1 = -vector2.y * (float)this.mHeight;
			float single2 = single + (float)this.mWidth;
			float single3 = single1 + (float)this.mHeight;
			this.mOldV0 = this.panel.worldToLocal.MultiplyPoint3x4(transforms.TransformPoint(single, single1, 0f));
			this.mOldV1 = this.panel.worldToLocal.MultiplyPoint3x4(transforms.TransformPoint(single2, single3, 0f));
		}
		else if (!this.panel.widgetsAreStatic && transforms.hasChanged)
		{
			this.mMatrixFrame = -1;
			transforms.hasChanged = false;
			Vector2 vector21 = this.pivotOffset;
			float single4 = -vector21.x * (float)this.mWidth;
			float single5 = -vector21.y * (float)this.mHeight;
			float single6 = single4 + (float)this.mWidth;
			float single7 = single5 + (float)this.mHeight;
			Vector3 vector3 = this.panel.worldToLocal.MultiplyPoint3x4(transforms.TransformPoint(single4, single5, 0f));
			Vector3 vector31 = this.panel.worldToLocal.MultiplyPoint3x4(transforms.TransformPoint(single6, single7, 0f));
			if (Vector3.SqrMagnitude(this.mOldV0 - vector3) > 1E-06f || Vector3.SqrMagnitude(this.mOldV1 - vector31) > 1E-06f)
			{
				this.mMoved = true;
				this.mOldV0 = vector3;
				this.mOldV1 = vector31;
			}
		}
		if (this.mMoved && this.onChange != null)
		{
			this.onChange();
		}
		return (this.mMoved ? true : this.mChanged);
	}

	public bool UpdateVisibility(bool visibleByAlpha, bool visibleByPanel)
	{
		if (this.mIsVisibleByAlpha == visibleByAlpha && this.mIsVisibleByPanel == visibleByPanel)
		{
			return false;
		}
		this.mChanged = true;
		this.mIsVisibleByAlpha = visibleByAlpha;
		this.mIsVisibleByPanel = visibleByPanel;
		return true;
	}

	protected virtual void UpgradeFrom265()
	{
		Vector3 vector3 = base.cachedTransform.localScale;
		this.mWidth = Mathf.Abs(Mathf.RoundToInt(vector3.x));
		this.mHeight = Mathf.Abs(Mathf.RoundToInt(vector3.y));
		NGUITools.UpdateWidgetCollider(base.gameObject, true);
	}

	public void WriteToBuffers(BetterList<Vector3> v, BetterList<Vector2> u, BetterList<Color32> c, BetterList<Vector3> n, BetterList<Vector4> t)
	{
		this.geometry.WriteToBuffers(v, u, c, n, t);
	}

	public enum AspectRatioSource
	{
		Free,
		BasedOnWidth,
		BasedOnHeight
	}

	public delegate bool HitCheck(Vector3 worldPos);

	public delegate void OnDimensionsChanged();

	public delegate void OnPostFillCallback(UIWidget widget, int bufferOffset, BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols);

	public enum Pivot
	{
		TopLeft,
		Top,
		TopRight,
		Left,
		Center,
		Right,
		BottomLeft,
		Bottom,
		BottomRight
	}
}