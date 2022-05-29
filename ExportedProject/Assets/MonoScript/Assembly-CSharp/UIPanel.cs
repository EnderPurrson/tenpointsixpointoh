using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/UI/NGUI Panel")]
[ExecuteInEditMode]
public class UIPanel : UIRect
{
	public static List<UIPanel> list;

	public UIPanel.OnGeometryUpdated onGeometryUpdated;

	public bool showInPanelTool = true;

	public bool generateNormals;

	public bool widgetsAreStatic;

	public bool cullWhileDragging = true;

	public bool alwaysOnScreen;

	public bool anchorOffset;

	public bool softBorderPadding = true;

	public UIPanel.RenderQueue renderQueue;

	public int startingRenderQueue = 3000;

	[NonSerialized]
	public List<UIWidget> widgets = new List<UIWidget>();

	[NonSerialized]
	public List<UIDrawCall> drawCalls = new List<UIDrawCall>();

	[NonSerialized]
	public Matrix4x4 worldToLocal = Matrix4x4.identity;

	[NonSerialized]
	public Vector4 drawCallClipRange = new Vector4(0f, 0f, 1f, 1f);

	public UIPanel.OnClippingMoved onClipMove;

	[HideInInspector]
	[SerializeField]
	private Texture2D mClipTexture;

	[HideInInspector]
	[SerializeField]
	private float mAlpha = 1f;

	[HideInInspector]
	[SerializeField]
	private UIDrawCall.Clipping mClipping;

	[HideInInspector]
	[SerializeField]
	private Vector4 mClipRange = new Vector4(0f, 0f, 300f, 200f);

	[HideInInspector]
	[SerializeField]
	private Vector2 mClipSoftness = new Vector2(4f, 4f);

	[HideInInspector]
	[SerializeField]
	private int mDepth;

	[HideInInspector]
	[SerializeField]
	private int mSortingOrder;

	private bool mRebuild;

	private bool mResized;

	[SerializeField]
	private Vector2 mClipOffset = Vector2.zero;

	private int mMatrixFrame = -1;

	private int mAlphaFrameID;

	private int mLayer = -1;

	private static float[] mTemp;

	private Vector2 mMin = Vector2.zero;

	private Vector2 mMax = Vector2.zero;

	private bool mHalfPixelOffset;

	private bool mSortWidgets;

	private bool mUpdateScroll;

	private UIPanel mParentPanel;

	private static Vector3[] mCorners;

	private static int mUpdateFrame;

	private UIDrawCall.OnRenderCallback mOnRender;

	private bool mForced;

	public override float alpha
	{
		get
		{
			return this.mAlpha;
		}
		set
		{
			float single = Mathf.Clamp01(value);
			if (this.mAlpha != single)
			{
				this.mAlphaFrameID = -1;
				this.mResized = true;
				this.mAlpha = single;
				this.SetDirty();
			}
		}
	}

	public Vector4 baseClipRegion
	{
		get
		{
			return this.mClipRange;
		}
		set
		{
			if (Mathf.Abs(this.mClipRange.x - value.x) > 0.001f || Mathf.Abs(this.mClipRange.y - value.y) > 0.001f || Mathf.Abs(this.mClipRange.z - value.z) > 0.001f || Mathf.Abs(this.mClipRange.w - value.w) > 0.001f)
			{
				this.mResized = true;
				this.mClipRange = value;
				this.mMatrixFrame = -1;
				UIScrollView component = base.GetComponent<UIScrollView>();
				if (component != null)
				{
					component.UpdatePosition();
				}
				if (this.onClipMove != null)
				{
					this.onClipMove(this);
				}
			}
		}
	}

	public override bool canBeAnchored
	{
		get
		{
			return this.mClipping != UIDrawCall.Clipping.None;
		}
	}

	public int clipCount
	{
		get
		{
			int num = 0;
			for (UIPanel i = this; i != null; i = i.mParentPanel)
			{
				if (i.mClipping == UIDrawCall.Clipping.SoftClip || i.mClipping == UIDrawCall.Clipping.TextureMask)
				{
					num++;
				}
			}
			return num;
		}
	}

	public Vector2 clipOffset
	{
		get
		{
			return this.mClipOffset;
		}
		set
		{
			if (Mathf.Abs(this.mClipOffset.x - value.x) > 0.001f || Mathf.Abs(this.mClipOffset.y - value.y) > 0.001f)
			{
				this.mClipOffset = value;
				this.InvalidateClipping();
				if (this.onClipMove != null)
				{
					this.onClipMove(this);
				}
			}
		}
	}

	public UIDrawCall.Clipping clipping
	{
		get
		{
			return this.mClipping;
		}
		set
		{
			if (this.mClipping != value)
			{
				this.mResized = true;
				this.mClipping = value;
				this.mMatrixFrame = -1;
			}
		}
	}

	[Obsolete("Use 'finalClipRegion' or 'baseClipRegion' instead")]
	public Vector4 clipRange
	{
		get
		{
			return this.baseClipRegion;
		}
		set
		{
			this.baseClipRegion = value;
		}
	}

	[Obsolete("Use 'hasClipping' or 'hasCumulativeClipping' instead")]
	public bool clipsChildren
	{
		get
		{
			return this.hasCumulativeClipping;
		}
	}

	public Vector2 clipSoftness
	{
		get
		{
			return this.mClipSoftness;
		}
		set
		{
			if (this.mClipSoftness != value)
			{
				this.mClipSoftness = value;
			}
		}
	}

	public Texture2D clipTexture
	{
		get
		{
			return this.mClipTexture;
		}
		set
		{
			if (this.mClipTexture != value)
			{
				this.mClipTexture = value;
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
				this.mDepth = value;
				UIPanel.list.Sort(new Comparison<UIPanel>(UIPanel.CompareFunc));
			}
		}
	}

	public Vector3 drawCallOffset
	{
		get
		{
			if (!(base.anchorCamera != null) || !this.mCam.orthographic)
			{
				return Vector3.zero;
			}
			Vector2 windowSize = this.GetWindowSize();
			float single = (base.root == null ? 1f : base.root.pixelSizeAdjustment);
			float single1 = single / windowSize.y / this.mCam.orthographicSize;
			bool flag = this.mHalfPixelOffset;
			bool flag1 = this.mHalfPixelOffset;
			if ((Mathf.RoundToInt(windowSize.x) & 1) == 1)
			{
				flag = !flag;
			}
			if ((Mathf.RoundToInt(windowSize.y) & 1) == 1)
			{
				flag1 = !flag1;
			}
			return new Vector3((!flag ? 0f : -single1), (!flag1 ? 0f : single1));
		}
	}

	public Vector4 finalClipRegion
	{
		get
		{
			Vector2 viewSize = this.GetViewSize();
			if (this.mClipping == UIDrawCall.Clipping.None)
			{
				return new Vector4(0f, 0f, viewSize.x, viewSize.y);
			}
			return new Vector4(this.mClipRange.x + this.mClipOffset.x, this.mClipRange.y + this.mClipOffset.y, viewSize.x, viewSize.y);
		}
	}

	public bool halfPixelOffset
	{
		get
		{
			return this.mHalfPixelOffset;
		}
	}

	public bool hasClipping
	{
		get
		{
			return (this.mClipping == UIDrawCall.Clipping.SoftClip ? true : this.mClipping == UIDrawCall.Clipping.TextureMask);
		}
	}

	public bool hasCumulativeClipping
	{
		get
		{
			return this.clipCount != 0;
		}
	}

	public float height
	{
		get
		{
			return this.GetViewSize().y;
		}
	}

	public override Vector3[] localCorners
	{
		get
		{
			if (this.mClipping == UIDrawCall.Clipping.None)
			{
				Vector3[] vector3Array = this.worldCorners;
				Transform transforms = base.cachedTransform;
				for (int i = 0; i < 4; i++)
				{
					vector3Array[i] = transforms.InverseTransformPoint(vector3Array[i]);
				}
				return vector3Array;
			}
			float single = this.mClipOffset.x + this.mClipRange.x - 0.5f * this.mClipRange.z;
			float single1 = this.mClipOffset.y + this.mClipRange.y - 0.5f * this.mClipRange.w;
			float single2 = single + this.mClipRange.z;
			float single3 = single1 + this.mClipRange.w;
			UIPanel.mCorners[0] = new Vector3(single, single1);
			UIPanel.mCorners[1] = new Vector3(single, single3);
			UIPanel.mCorners[2] = new Vector3(single2, single3);
			UIPanel.mCorners[3] = new Vector3(single2, single1);
			return UIPanel.mCorners;
		}
	}

	public static int nextUnusedDepth
	{
		get
		{
			int num = -2147483648;
			int num1 = 0;
			int count = UIPanel.list.Count;
			while (num1 < count)
			{
				num = Mathf.Max(num, UIPanel.list[num1].depth);
				num1++;
			}
			return (num != -2147483648 ? num + 1 : 0);
		}
	}

	public UIPanel parentPanel
	{
		get
		{
			return this.mParentPanel;
		}
	}

	public int sortingOrder
	{
		get
		{
			return this.mSortingOrder;
		}
		set
		{
			if (this.mSortingOrder != value)
			{
				this.mSortingOrder = value;
				this.UpdateDrawCalls();
			}
		}
	}

	public bool usedForUI
	{
		get
		{
			return (base.anchorCamera == null ? false : this.mCam.orthographic);
		}
	}

	public float width
	{
		get
		{
			return this.GetViewSize().x;
		}
	}

	public override Vector3[] worldCorners
	{
		get
		{
			if (this.mClipping == UIDrawCall.Clipping.None)
			{
				if (base.anchorCamera != null)
				{
					return this.mCam.GetWorldCorners(base.cameraRayDistance);
				}
				Vector2 viewSize = this.GetViewSize();
				float single = -0.5f * viewSize.x;
				float single1 = -0.5f * viewSize.y;
				float single2 = single + viewSize.x;
				float single3 = single1 + viewSize.y;
				UIPanel.mCorners[0] = new Vector3(single, single1);
				UIPanel.mCorners[1] = new Vector3(single, single3);
				UIPanel.mCorners[2] = new Vector3(single2, single3);
				UIPanel.mCorners[3] = new Vector3(single2, single1);
				if (this.anchorOffset && (this.mCam == null || this.mCam.transform.parent != base.cachedTransform))
				{
					Vector3 vector3 = base.cachedTransform.position;
					for (int i = 0; i < 4; i++)
					{
						UIPanel.mCorners[i] += vector3;
					}
				}
			}
			else
			{
				float single4 = this.mClipOffset.x + this.mClipRange.x - 0.5f * this.mClipRange.z;
				float single5 = this.mClipOffset.y + this.mClipRange.y - 0.5f * this.mClipRange.w;
				float single6 = single4 + this.mClipRange.z;
				float single7 = single5 + this.mClipRange.w;
				Transform transforms = base.cachedTransform;
				UIPanel.mCorners[0] = transforms.TransformPoint(single4, single5, 0f);
				UIPanel.mCorners[1] = transforms.TransformPoint(single4, single7, 0f);
				UIPanel.mCorners[2] = transforms.TransformPoint(single6, single7, 0f);
				UIPanel.mCorners[3] = transforms.TransformPoint(single6, single5, 0f);
			}
			return UIPanel.mCorners;
		}
	}

	static UIPanel()
	{
		UIPanel.list = new List<UIPanel>();
		UIPanel.mTemp = new float[4];
		UIPanel.mCorners = new Vector3[4];
		UIPanel.mUpdateFrame = -1;
	}

	public UIPanel()
	{
	}

	public void AddWidget(UIWidget w)
	{
		this.mUpdateScroll = true;
		if (this.widgets.Count == 0)
		{
			this.widgets.Add(w);
		}
		else if (this.mSortWidgets)
		{
			this.widgets.Add(w);
			this.SortWidgets();
		}
		else if (UIWidget.PanelCompareFunc(w, this.widgets[0]) != -1)
		{
			int count = this.widgets.Count;
			while (count > 0)
			{
				int num = count - 1;
				count = num;
				if (UIWidget.PanelCompareFunc(w, this.widgets[num]) != -1)
				{
					this.widgets.Insert(count + 1, w);
					break;
				}
			}
		}
		else
		{
			this.widgets.Insert(0, w);
		}
		this.FindDrawCall(w);
	}

	public bool Affects(UIWidget w)
	{
		if (w == null)
		{
			return false;
		}
		UIPanel uIPanel = w.panel;
		if (uIPanel == null)
		{
			return false;
		}
		for (UIPanel i = this; i != null; i = i.mParentPanel)
		{
			if (i == uIPanel)
			{
				return true;
			}
			if (!i.hasCumulativeClipping)
			{
				return false;
			}
		}
		return false;
	}

	protected override void Awake()
	{
		base.Awake();
		this.mHalfPixelOffset = (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.XBOX360 || Application.platform == RuntimePlatform.WindowsWebPlayer ? true : Application.platform == RuntimePlatform.WindowsEditor);
		if (this.mHalfPixelOffset && SystemInfo.graphicsDeviceVersion.Contains("Direct3D"))
		{
			this.mHalfPixelOffset = SystemInfo.graphicsShaderLevel < 40;
		}
	}

	public virtual Vector3 CalculateConstrainOffset(Vector2 min, Vector2 max)
	{
		Vector4 vector4 = this.finalClipRegion;
		float single = vector4.z * 0.5f;
		float single1 = vector4.w * 0.5f;
		Vector2 vector2 = new Vector2(min.x, min.y);
		Vector2 vector21 = new Vector2(max.x, max.y);
		Vector2 vector22 = new Vector2(vector4.x - single, vector4.y - single1);
		Vector2 vector23 = new Vector2(vector4.x + single, vector4.y + single1);
		if (this.softBorderPadding && this.clipping == UIDrawCall.Clipping.SoftClip)
		{
			vector22.x += this.mClipSoftness.x;
			vector22.y += this.mClipSoftness.y;
			vector23.x -= this.mClipSoftness.x;
			vector23.y -= this.mClipSoftness.y;
		}
		return NGUIMath.ConstrainRect(vector2, vector21, vector22, vector23);
	}

	public override float CalculateFinalAlpha(int frameID)
	{
		if (this.mAlphaFrameID != frameID)
		{
			this.mAlphaFrameID = frameID;
			UIRect uIRect = base.parent;
			this.finalAlpha = (base.parent == null ? this.mAlpha : uIRect.CalculateFinalAlpha(frameID) * this.mAlpha);
		}
		return this.finalAlpha;
	}

	public static int CompareFunc(UIPanel a, UIPanel b)
	{
		if (!(a != b) || !(a != null) || !(b != null))
		{
			return 0;
		}
		if (a.mDepth < b.mDepth)
		{
			return -1;
		}
		if (a.mDepth > b.mDepth)
		{
			return 1;
		}
		return (a.GetInstanceID() >= b.GetInstanceID() ? 1 : -1);
	}

	public bool ConstrainTargetToBounds(Transform target, ref Bounds targetBounds, bool immediate)
	{
		Vector3 vector3 = targetBounds.min;
		Vector3 vector31 = targetBounds.max;
		float single = 1f;
		if (this.mClipping == UIDrawCall.Clipping.None)
		{
			UIRoot uIRoot = base.root;
			if (uIRoot != null)
			{
				single = uIRoot.pixelSizeAdjustment;
			}
		}
		if (single != 1f)
		{
			vector3 /= single;
			vector31 /= single;
		}
		Vector3 vector32 = this.CalculateConstrainOffset(vector3, vector31) * single;
		if (vector32.sqrMagnitude <= 0f)
		{
			return false;
		}
		if (!immediate)
		{
			SpringPosition springPosition = SpringPosition.Begin(target.gameObject, target.localPosition + vector32, 13f);
			springPosition.ignoreTimeScale = true;
			springPosition.worldSpace = false;
		}
		else
		{
			Transform transforms = target;
			transforms.localPosition = transforms.localPosition + vector32;
			targetBounds.center = targetBounds.center + vector32;
			SpringPosition component = target.GetComponent<SpringPosition>();
			if (component != null)
			{
				component.enabled = false;
			}
		}
		return true;
	}

	public bool ConstrainTargetToBounds(Transform target, bool immediate)
	{
		Bounds bound = NGUIMath.CalculateRelativeWidgetBounds(base.cachedTransform, target);
		return this.ConstrainTargetToBounds(target, ref bound, immediate);
	}

	private void FillAllDrawCalls()
	{
		for (int i = 0; i < this.drawCalls.Count; i++)
		{
			UIDrawCall.Destroy(this.drawCalls[i]);
		}
		this.drawCalls.Clear();
		Material material = null;
		Texture texture = null;
		Shader shader = null;
		UIDrawCall uIDrawCall = null;
		int num = 0;
		if (this.mSortWidgets)
		{
			this.SortWidgets();
		}
		for (int j = 0; j < this.widgets.Count; j++)
		{
			UIWidget item = this.widgets[j];
			if (!item.isVisible || !item.hasVertices)
			{
				item.drawCall = null;
			}
			else
			{
				Material material1 = item.material;
				Texture texture1 = item.mainTexture;
				Shader shader1 = item.shader;
				if (material != material1 || texture != texture1 || shader != shader1)
				{
					if (uIDrawCall != null && uIDrawCall.verts.size != 0)
					{
						this.drawCalls.Add(uIDrawCall);
						uIDrawCall.UpdateGeometry(num);
						uIDrawCall.onRender = this.mOnRender;
						this.mOnRender = null;
						num = 0;
						uIDrawCall = null;
					}
					material = material1;
					texture = texture1;
					shader = shader1;
				}
				if (material != null || shader != null || texture != null)
				{
					if (uIDrawCall != null)
					{
						int num1 = item.depth;
						if (num1 < uIDrawCall.depthStart)
						{
							uIDrawCall.depthStart = num1;
						}
						if (num1 > uIDrawCall.depthEnd)
						{
							uIDrawCall.depthEnd = num1;
						}
					}
					else
					{
						uIDrawCall = UIDrawCall.Create(this, material, texture, shader);
						uIDrawCall.depthStart = item.depth;
						uIDrawCall.depthEnd = uIDrawCall.depthStart;
						uIDrawCall.panel = this;
					}
					item.drawCall = uIDrawCall;
					num++;
					if (!this.generateNormals)
					{
						item.WriteToBuffers(uIDrawCall.verts, uIDrawCall.uvs, uIDrawCall.cols, null, null);
					}
					else
					{
						item.WriteToBuffers(uIDrawCall.verts, uIDrawCall.uvs, uIDrawCall.cols, uIDrawCall.norms, uIDrawCall.tans);
					}
					if (item.mOnRender != null)
					{
						if (this.mOnRender != null)
						{
							this.mOnRender += item.mOnRender;
						}
						else
						{
							this.mOnRender = item.mOnRender;
						}
					}
				}
			}
		}
		if (uIDrawCall != null && uIDrawCall.verts.size != 0)
		{
			this.drawCalls.Add(uIDrawCall);
			uIDrawCall.UpdateGeometry(num);
			uIDrawCall.onRender = this.mOnRender;
			this.mOnRender = null;
		}
	}

	public bool FillDrawCall(UIDrawCall dc)
	{
		if (dc != null)
		{
			dc.isDirty = false;
			int num = 0;
			int num1 = 0;
			while (num1 < this.widgets.Count)
			{
				UIWidget item = this.widgets[num1];
				if (item != null)
				{
					if (item.drawCall == dc)
					{
						if (!item.isVisible || !item.hasVertices)
						{
							item.drawCall = null;
						}
						else
						{
							num++;
							if (!this.generateNormals)
							{
								item.WriteToBuffers(dc.verts, dc.uvs, dc.cols, null, null);
							}
							else
							{
								item.WriteToBuffers(dc.verts, dc.uvs, dc.cols, dc.norms, dc.tans);
							}
							if (item.mOnRender != null)
							{
								if (this.mOnRender != null)
								{
									this.mOnRender += item.mOnRender;
								}
								else
								{
									this.mOnRender = item.mOnRender;
								}
							}
						}
					}
					num1++;
				}
				else
				{
					this.widgets.RemoveAt(num1);
				}
			}
			if (dc.verts.size != 0)
			{
				dc.UpdateGeometry(num);
				dc.onRender = this.mOnRender;
				this.mOnRender = null;
				return true;
			}
		}
		return false;
	}

	public static UIPanel Find(Transform trans)
	{
		return UIPanel.Find(trans, false, -1);
	}

	public static UIPanel Find(Transform trans, bool createIfMissing)
	{
		return UIPanel.Find(trans, createIfMissing, -1);
	}

	public static UIPanel Find(Transform trans, bool createIfMissing, int layer)
	{
		UIPanel uIPanel;
		UIPanel uIPanel1 = NGUITools.FindInParents<UIPanel>(trans);
		if (uIPanel1 != null)
		{
			return uIPanel1;
		}
		while (trans.parent != null)
		{
			trans = trans.parent;
		}
		if (!createIfMissing)
		{
			uIPanel = null;
		}
		else
		{
			uIPanel = NGUITools.CreateUI(trans, false, layer);
		}
		return uIPanel;
	}

	public UIDrawCall FindDrawCall(UIWidget w)
	{
		Material material = w.material;
		Texture texture = w.mainTexture;
		int num = w.depth;
		for (int i = 0; i < this.drawCalls.Count; i++)
		{
			UIDrawCall item = this.drawCalls[i];
			int num1 = (i != 0 ? this.drawCalls[i - 1].depthEnd + 1 : -2147483648);
			if (num1 <= num && (i + 1 != this.drawCalls.Count ? this.drawCalls[i + 1].depthStart - 1 : 2147483647) >= num)
			{
				if (!(item.baseMaterial == material) || !(item.mainTexture == texture))
				{
					this.mRebuild = true;
				}
				else if (w.isVisible)
				{
					w.drawCall = item;
					if (w.hasVertices)
					{
						item.isDirty = true;
					}
					return item;
				}
				return null;
			}
		}
		this.mRebuild = true;
		return null;
	}

	private void FindParent()
	{
		UIPanel uIPanel;
		Transform transforms = base.cachedTransform.parent;
		if (transforms == null)
		{
			uIPanel = null;
		}
		else
		{
			uIPanel = NGUITools.FindInParents<UIPanel>(transforms.gameObject);
		}
		this.mParentPanel = uIPanel;
	}

	public override Vector3[] GetSides(Transform relativeTo)
	{
		if (this.mClipping == UIDrawCall.Clipping.None)
		{
			if (!(base.anchorCamera != null) || !this.anchorOffset)
			{
				return base.GetSides(relativeTo);
			}
			Vector3[] sides = this.mCam.GetSides(base.cameraRayDistance);
			Vector3 vector3 = base.cachedTransform.position;
			for (int i = 0; i < 4; i++)
			{
				sides[i] += vector3;
			}
			if (relativeTo != null)
			{
				for (int j = 0; j < 4; j++)
				{
					sides[j] = relativeTo.InverseTransformPoint(sides[j]);
				}
			}
			return sides;
		}
		float single = this.mClipOffset.x + this.mClipRange.x - 0.5f * this.mClipRange.z;
		float single1 = this.mClipOffset.y + this.mClipRange.y - 0.5f * this.mClipRange.w;
		float single2 = single + this.mClipRange.z;
		float single3 = single1 + this.mClipRange.w;
		float single4 = (single + single2) * 0.5f;
		float single5 = (single1 + single3) * 0.5f;
		Transform transforms = base.cachedTransform;
		UIRect.mSides[0] = transforms.TransformPoint(single, single5, 0f);
		UIRect.mSides[1] = transforms.TransformPoint(single4, single3, 0f);
		UIRect.mSides[2] = transforms.TransformPoint(single2, single5, 0f);
		UIRect.mSides[3] = transforms.TransformPoint(single4, single1, 0f);
		if (relativeTo != null)
		{
			for (int k = 0; k < 4; k++)
			{
				UIRect.mSides[k] = relativeTo.InverseTransformPoint(UIRect.mSides[k]);
			}
		}
		return UIRect.mSides;
	}

	public Vector2 GetViewSize()
	{
		if (this.mClipping == UIDrawCall.Clipping.None)
		{
			return NGUITools.screenSize;
		}
		return new Vector2(this.mClipRange.z, this.mClipRange.w);
	}

	public Vector2 GetWindowSize()
	{
		UIRoot uIRoot = base.root;
		Vector2 pixelSizeAdjustment = NGUITools.screenSize;
		if (uIRoot != null)
		{
			pixelSizeAdjustment *= uIRoot.GetPixelSizeAdjustment(Mathf.RoundToInt(pixelSizeAdjustment.y));
		}
		return pixelSizeAdjustment;
	}

	public override void Invalidate(bool includeChildren)
	{
		this.mAlphaFrameID = -1;
		base.Invalidate(includeChildren);
	}

	private void InvalidateClipping()
	{
		this.mResized = true;
		this.mMatrixFrame = -1;
		int num = 0;
		int count = UIPanel.list.Count;
		while (num < count)
		{
			UIPanel item = UIPanel.list[num];
			if (item != this && item.parentPanel == this)
			{
				item.InvalidateClipping();
			}
			num++;
		}
	}

	public bool IsVisible(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
	{
		this.UpdateTransformMatrix();
		a = this.worldToLocal.MultiplyPoint3x4(a);
		b = this.worldToLocal.MultiplyPoint3x4(b);
		c = this.worldToLocal.MultiplyPoint3x4(c);
		d = this.worldToLocal.MultiplyPoint3x4(d);
		UIPanel.mTemp[0] = a.x;
		UIPanel.mTemp[1] = b.x;
		UIPanel.mTemp[2] = c.x;
		UIPanel.mTemp[3] = d.x;
		float single = Mathf.Min(UIPanel.mTemp);
		float single1 = Mathf.Max(UIPanel.mTemp);
		UIPanel.mTemp[0] = a.y;
		UIPanel.mTemp[1] = b.y;
		UIPanel.mTemp[2] = c.y;
		UIPanel.mTemp[3] = d.y;
		float single2 = Mathf.Min(UIPanel.mTemp);
		float single3 = Mathf.Max(UIPanel.mTemp);
		if (single1 < this.mMin.x)
		{
			return false;
		}
		if (single3 < this.mMin.y)
		{
			return false;
		}
		if (single > this.mMax.x)
		{
			return false;
		}
		if (single2 > this.mMax.y)
		{
			return false;
		}
		return true;
	}

	public bool IsVisible(Vector3 worldPos)
	{
		if (this.mAlpha < 0.001f)
		{
			return false;
		}
		if (this.mClipping == UIDrawCall.Clipping.None || this.mClipping == UIDrawCall.Clipping.ConstrainButDontClip)
		{
			return true;
		}
		this.UpdateTransformMatrix();
		Vector3 vector3 = this.worldToLocal.MultiplyPoint3x4(worldPos);
		if (vector3.x < this.mMin.x)
		{
			return false;
		}
		if (vector3.y < this.mMin.y)
		{
			return false;
		}
		if (vector3.x > this.mMax.x)
		{
			return false;
		}
		if (vector3.y > this.mMax.y)
		{
			return false;
		}
		return true;
	}

	public bool IsVisible(UIWidget w)
	{
		UIPanel uIPanel = this;
		Vector3[] vector3Array = null;
		while (uIPanel != null)
		{
			if ((uIPanel.mClipping == UIDrawCall.Clipping.None || uIPanel.mClipping == UIDrawCall.Clipping.ConstrainButDontClip) && !w.hideIfOffScreen)
			{
				uIPanel = uIPanel.mParentPanel;
			}
			else
			{
				if (vector3Array == null)
				{
					vector3Array = w.worldCorners;
				}
				if (!uIPanel.IsVisible(vector3Array[0], vector3Array[1], vector3Array[2], vector3Array[3]))
				{
					return false;
				}
				uIPanel = uIPanel.mParentPanel;
			}
		}
		return true;
	}

	private void LateUpdate()
	{
		if (UIPanel.mUpdateFrame != Time.frameCount)
		{
			UIPanel.mUpdateFrame = Time.frameCount;
			int num = 0;
			int count = UIPanel.list.Count;
			while (num < count)
			{
				UIPanel.list[num].UpdateSelf();
				num++;
			}
			int count1 = 3000;
			int num1 = 0;
			int count2 = UIPanel.list.Count;
			while (num1 < count2)
			{
				UIPanel item = UIPanel.list[num1];
				if (item.renderQueue == UIPanel.RenderQueue.Automatic)
				{
					item.startingRenderQueue = count1;
					item.UpdateDrawCalls();
					count1 += item.drawCalls.Count;
				}
				else if (item.renderQueue != UIPanel.RenderQueue.StartAt)
				{
					item.UpdateDrawCalls();
					if (item.drawCalls.Count != 0)
					{
						count1 = Mathf.Max(count1, item.startingRenderQueue + 1);
					}
				}
				else
				{
					item.UpdateDrawCalls();
					if (item.drawCalls.Count != 0)
					{
						count1 = Mathf.Max(count1, item.startingRenderQueue + item.drawCalls.Count);
					}
				}
				num1++;
			}
		}
	}

	protected override void OnAnchor()
	{
		float single;
		float single1;
		float single2;
		float single3;
		if (this.mClipping == UIDrawCall.Clipping.None)
		{
			return;
		}
		Transform transforms = base.cachedTransform;
		Transform transforms1 = transforms.parent;
		Vector2 viewSize = this.GetViewSize();
		Vector2 vector2 = transforms.localPosition;
		if (!(this.leftAnchor.target == this.bottomAnchor.target) || !(this.leftAnchor.target == this.rightAnchor.target) || !(this.leftAnchor.target == this.topAnchor.target))
		{
			if (!this.leftAnchor.target)
			{
				single = this.mClipRange.x - 0.5f * viewSize.x;
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
				single2 = this.mClipRange.x + 0.5f * viewSize.x;
			}
			else
			{
				Vector3[] vector3Array = this.rightAnchor.GetSides(transforms1);
				if (vector3Array == null)
				{
					Vector3 vector3 = base.GetLocalPos(this.rightAnchor, transforms1);
					single2 = vector3.x + (float)this.rightAnchor.absolute;
				}
				else
				{
					single2 = NGUIMath.Lerp(vector3Array[0].x, vector3Array[2].x, this.rightAnchor.relative) + (float)this.rightAnchor.absolute;
				}
			}
			if (!this.bottomAnchor.target)
			{
				single1 = this.mClipRange.y - 0.5f * viewSize.y;
			}
			else
			{
				Vector3[] sides1 = this.bottomAnchor.GetSides(transforms1);
				if (sides1 == null)
				{
					Vector3 localPos1 = base.GetLocalPos(this.bottomAnchor, transforms1);
					single1 = localPos1.y + (float)this.bottomAnchor.absolute;
				}
				else
				{
					single1 = NGUIMath.Lerp(sides1[3].y, sides1[1].y, this.bottomAnchor.relative) + (float)this.bottomAnchor.absolute;
				}
			}
			if (!this.topAnchor.target)
			{
				single3 = this.mClipRange.y + 0.5f * viewSize.y;
			}
			else
			{
				Vector3[] vector3Array1 = this.topAnchor.GetSides(transforms1);
				if (vector3Array1 == null)
				{
					Vector3 vector31 = base.GetLocalPos(this.topAnchor, transforms1);
					single3 = vector31.y + (float)this.topAnchor.absolute;
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
				Vector2 vector21 = base.GetLocalPos(this.leftAnchor, transforms1);
				single = vector21.x + (float)this.leftAnchor.absolute;
				single1 = vector21.y + (float)this.bottomAnchor.absolute;
				single2 = vector21.x + (float)this.rightAnchor.absolute;
				single3 = vector21.y + (float)this.topAnchor.absolute;
			}
			else
			{
				single = NGUIMath.Lerp(sides2[0].x, sides2[2].x, this.leftAnchor.relative) + (float)this.leftAnchor.absolute;
				single2 = NGUIMath.Lerp(sides2[0].x, sides2[2].x, this.rightAnchor.relative) + (float)this.rightAnchor.absolute;
				single1 = NGUIMath.Lerp(sides2[3].y, sides2[1].y, this.bottomAnchor.relative) + (float)this.bottomAnchor.absolute;
				single3 = NGUIMath.Lerp(sides2[3].y, sides2[1].y, this.topAnchor.relative) + (float)this.topAnchor.absolute;
			}
		}
		single = single - (vector2.x + this.mClipOffset.x);
		single2 = single2 - (vector2.x + this.mClipOffset.x);
		single1 = single1 - (vector2.y + this.mClipOffset.y);
		single3 = single3 - (vector2.y + this.mClipOffset.y);
		float single4 = Mathf.Lerp(single, single2, 0.5f);
		float single5 = Mathf.Lerp(single1, single3, 0.5f);
		float single6 = single2 - single;
		float single7 = single3 - single1;
		float single8 = Mathf.Max(2f, this.mClipSoftness.x);
		float single9 = Mathf.Max(2f, this.mClipSoftness.y);
		if (single6 < single8)
		{
			single6 = single8;
		}
		if (single7 < single9)
		{
			single7 = single9;
		}
		this.baseClipRegion = new Vector4(single4, single5, single6, single7);
	}

	protected override void OnDisable()
	{
		int num = 0;
		int count = this.drawCalls.Count;
		while (num < count)
		{
			UIDrawCall item = this.drawCalls[num];
			if (item != null)
			{
				UIDrawCall.Destroy(item);
			}
			num++;
		}
		this.drawCalls.Clear();
		UIPanel.list.Remove(this);
		this.mAlphaFrameID = -1;
		this.mMatrixFrame = -1;
		if (UIPanel.list.Count == 0)
		{
			UIDrawCall.ReleaseAll();
			UIPanel.mUpdateFrame = -1;
		}
		base.OnDisable();
	}

	protected override void OnEnable()
	{
		this.mRebuild = true;
		this.mAlphaFrameID = -1;
		this.mMatrixFrame = -1;
		this.OnStart();
		base.OnEnable();
		this.mMatrixFrame = -1;
	}

	protected override void OnInit()
	{
		UICamera component;
		if (UIPanel.list.Contains(this))
		{
			return;
		}
		base.OnInit();
		this.FindParent();
		if (base.GetComponent<Rigidbody>() == null && this.mParentPanel == null)
		{
			if (base.anchorCamera == null)
			{
				component = null;
			}
			else
			{
				component = this.mCam.GetComponent<UICamera>();
			}
			UICamera uICamera = component;
			if (uICamera != null && (uICamera.eventType == UICamera.EventType.UI_3D || uICamera.eventType == UICamera.EventType.World_3D))
			{
				Rigidbody rigidbody = base.gameObject.AddComponent<Rigidbody>();
				rigidbody.isKinematic = true;
				rigidbody.useGravity = false;
			}
		}
		this.mRebuild = true;
		this.mAlphaFrameID = -1;
		this.mMatrixFrame = -1;
		UIPanel.list.Add(this);
		UIPanel.list.Sort(new Comparison<UIPanel>(UIPanel.CompareFunc));
	}

	protected override void OnStart()
	{
		this.mLayer = base.cachedGameObject.layer;
	}

	public override void ParentHasChanged()
	{
		base.ParentHasChanged();
		this.FindParent();
	}

	[ContextMenu("Force Refresh")]
	public void RebuildAllDrawCalls()
	{
		this.mRebuild = true;
	}

	public void Refresh()
	{
		this.mRebuild = true;
		UIPanel.mUpdateFrame = -1;
		if (UIPanel.list.Count > 0)
		{
			UIPanel.list[0].LateUpdate();
		}
	}

	public void RemoveWidget(UIWidget w)
	{
		if (this.widgets.Remove(w) && w.drawCall != null)
		{
			int num = w.depth;
			if (num == w.drawCall.depthStart || num == w.drawCall.depthEnd)
			{
				this.mRebuild = true;
			}
			w.drawCall.isDirty = true;
			w.drawCall = null;
		}
	}

	public void SetDirty()
	{
		int num = 0;
		int count = this.drawCalls.Count;
		while (num < count)
		{
			this.drawCalls[num].isDirty = true;
			num++;
		}
		this.Invalidate(true);
	}

	public override void SetRect(float x, float y, float width, float height)
	{
		int num = Mathf.FloorToInt(width + 0.5f);
		int num1 = Mathf.FloorToInt(height + 0.5f);
		num = num >> 1 << 1;
		num1 = num1 >> 1 << 1;
		Transform transforms = base.cachedTransform;
		Vector3 vector3 = transforms.localPosition;
		vector3.x = Mathf.Floor(x + 0.5f);
		vector3.y = Mathf.Floor(y + 0.5f);
		if (num < 2)
		{
			num = 2;
		}
		if (num1 < 2)
		{
			num1 = 2;
		}
		this.baseClipRegion = new Vector4(vector3.x, vector3.y, (float)num, (float)num1);
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

	public void SortWidgets()
	{
		this.mSortWidgets = false;
		this.widgets.Sort(new Comparison<UIWidget>(UIWidget.PanelCompareFunc));
	}

	private void UpdateDrawCalls()
	{
		Vector3 num;
		bool flag;
		Transform transforms = base.cachedTransform;
		bool flag1 = this.usedForUI;
		if (this.clipping == UIDrawCall.Clipping.None)
		{
			this.drawCallClipRange = Vector4.zero;
		}
		else
		{
			this.drawCallClipRange = this.finalClipRegion;
			this.drawCallClipRange.z *= 0.5f;
			this.drawCallClipRange.w *= 0.5f;
		}
		int num1 = Screen.width;
		int num2 = Screen.height;
		if (this.drawCallClipRange.z == 0f)
		{
			this.drawCallClipRange.z = (float)num1 * 0.5f;
		}
		if (this.drawCallClipRange.w == 0f)
		{
			this.drawCallClipRange.w = (float)num2 * 0.5f;
		}
		if (this.halfPixelOffset)
		{
			this.drawCallClipRange.x -= 0.5f;
			this.drawCallClipRange.y += 0.5f;
		}
		if (!flag1)
		{
			num = transforms.position;
		}
		else
		{
			Transform transforms1 = base.cachedTransform.parent;
			num = base.cachedTransform.localPosition;
			if (this.clipping != UIDrawCall.Clipping.None)
			{
				num.x = (float)Mathf.RoundToInt(num.x);
				num.y = (float)Mathf.RoundToInt(num.y);
			}
			if (transforms1 != null)
			{
				num = transforms1.TransformPoint(num);
			}
			num += this.drawCallOffset;
		}
		Quaternion quaternion = transforms.rotation;
		Vector3 vector3 = transforms.lossyScale;
		for (int i = 0; i < this.drawCalls.Count; i++)
		{
			UIDrawCall item = this.drawCalls[i];
			Transform transforms2 = item.cachedTransform;
			transforms2.position = num;
			transforms2.rotation = quaternion;
			transforms2.localScale = vector3;
			item.renderQueue = (this.renderQueue != UIPanel.RenderQueue.Explicit ? this.startingRenderQueue + i : this.startingRenderQueue);
			UIDrawCall uIDrawCall = item;
			if (!this.alwaysOnScreen)
			{
				flag = false;
			}
			else
			{
				flag = (this.mClipping == UIDrawCall.Clipping.None ? true : this.mClipping == UIDrawCall.Clipping.ConstrainButDontClip);
			}
			uIDrawCall.alwaysOnScreen = flag;
			item.sortingOrder = this.mSortingOrder;
			item.clipTexture = this.mClipTexture;
		}
	}

	private void UpdateLayers()
	{
		if (this.mLayer != base.cachedGameObject.layer)
		{
			this.mLayer = this.mGo.layer;
			int num = 0;
			int count = this.widgets.Count;
			while (num < count)
			{
				UIWidget item = this.widgets[num];
				if (item && item.parent == this)
				{
					item.gameObject.layer = this.mLayer;
				}
				num++;
			}
			base.ResetAnchors();
			for (int i = 0; i < this.drawCalls.Count; i++)
			{
				this.drawCalls[i].gameObject.layer = this.mLayer;
			}
		}
	}

	private void UpdateSelf()
	{
		this.UpdateTransformMatrix();
		this.UpdateLayers();
		this.UpdateWidgets();
		if (!this.mRebuild)
		{
			int num = 0;
			while (num < this.drawCalls.Count)
			{
				UIDrawCall item = this.drawCalls[num];
				if (!item.isDirty || this.FillDrawCall(item))
				{
					num++;
				}
				else
				{
					UIDrawCall.Destroy(item);
					this.drawCalls.RemoveAt(num);
				}
			}
		}
		else
		{
			this.mRebuild = false;
			this.FillAllDrawCalls();
		}
		if (this.mUpdateScroll)
		{
			this.mUpdateScroll = false;
			UIScrollView component = base.GetComponent<UIScrollView>();
			if (component != null)
			{
				component.UpdateScrollbars();
			}
		}
	}

	private void UpdateTransformMatrix()
	{
		int num = Time.frameCount;
		if (base.cachedTransform.hasChanged)
		{
			this.mTrans.hasChanged = false;
			this.mMatrixFrame = -1;
		}
		if (this.mMatrixFrame != num)
		{
			this.mMatrixFrame = num;
			this.worldToLocal = this.mTrans.worldToLocalMatrix;
			Vector2 viewSize = this.GetViewSize() * 0.5f;
			float single = this.mClipOffset.x + this.mClipRange.x;
			float single1 = this.mClipOffset.y + this.mClipRange.y;
			this.mMin.x = single - viewSize.x;
			this.mMin.y = single1 - viewSize.y;
			this.mMax.x = single + viewSize.x;
			this.mMax.y = single1 + viewSize.y;
		}
	}

	private void UpdateWidgets()
	{
		bool flag;
		bool flag1 = false;
		bool flag2 = false;
		bool flag3 = this.hasCumulativeClipping;
		if (!this.cullWhileDragging)
		{
			for (int i = 0; i < UIScrollView.list.size; i++)
			{
				UIScrollView item = UIScrollView.list[i];
				if (item.panel == this && item.isDragging)
				{
					flag2 = true;
				}
			}
		}
		if (this.mForced != flag2)
		{
			this.mForced = flag2;
			this.mResized = true;
		}
		int num = Time.frameCount;
		int num1 = 0;
		int count = this.widgets.Count;
		while (num1 < count)
		{
			UIWidget uIWidget = this.widgets[num1];
			if (uIWidget.panel == this && uIWidget.enabled)
			{
				if (uIWidget.UpdateTransform(num) || this.mResized)
				{
					bool flag4 = (flag2 ? true : uIWidget.CalculateCumulativeAlpha(num) > 0.001f);
					UIWidget uIWidget1 = uIWidget;
					bool flag5 = flag4;
					if (flag2)
					{
						flag = true;
					}
					else
					{
						flag = (flag3 || uIWidget.hideIfOffScreen ? this.IsVisible(uIWidget) : true);
					}
					uIWidget1.UpdateVisibility(flag5, flag);
				}
				if (uIWidget.UpdateGeometry(num))
				{
					flag1 = true;
					if (!this.mRebuild)
					{
						if (uIWidget.drawCall == null)
						{
							this.FindDrawCall(uIWidget);
						}
						else
						{
							uIWidget.drawCall.isDirty = true;
						}
					}
				}
			}
			num1++;
		}
		if (flag1 && this.onGeometryUpdated != null)
		{
			this.onGeometryUpdated();
		}
		this.mResized = false;
	}

	public delegate void OnClippingMoved(UIPanel panel);

	public delegate void OnGeometryUpdated();

	public enum RenderQueue
	{
		Automatic,
		StartAt,
		Explicit
	}
}