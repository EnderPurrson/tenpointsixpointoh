using System;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Stretch")]
[ExecuteInEditMode]
public class UIStretch : MonoBehaviour
{
	public Camera uiCamera;

	public GameObject container;

	public UIStretch.Style style;

	public bool runOnlyOnce = true;

	public Vector2 relativeSize = Vector2.one;

	public Vector2 initialSize = Vector2.one;

	public Vector2 borderPadding = Vector2.zero;

	[HideInInspector]
	[SerializeField]
	private UIWidget widgetContainer;

	private Transform mTrans;

	private UIWidget mWidget;

	private UISprite mSprite;

	private UIPanel mPanel;

	private UIRoot mRoot;

	private Animation mAnim;

	private Rect mRect;

	private bool mStarted;

	public UIStretch()
	{
	}

	private void Awake()
	{
		this.mAnim = base.GetComponent<Animation>();
		this.mRect = new Rect();
		this.mTrans = base.transform;
		this.mWidget = base.GetComponent<UIWidget>();
		this.mSprite = base.GetComponent<UISprite>();
		this.mPanel = base.GetComponent<UIPanel>();
		UICamera.onScreenResize += new UICamera.OnScreenResize(this.ScreenSizeChanged);
	}

	private void OnDestroy()
	{
		UICamera.onScreenResize -= new UICamera.OnScreenResize(this.ScreenSizeChanged);
	}

	private void ScreenSizeChanged()
	{
		if (this.mStarted && this.runOnlyOnce)
		{
			this.Update();
		}
	}

	private void Start()
	{
		if (this.container == null && this.widgetContainer != null)
		{
			this.container = this.widgetContainer.gameObject;
			this.widgetContainer = null;
		}
		if (this.uiCamera == null)
		{
			this.uiCamera = NGUITools.FindCameraForLayer(base.gameObject.layer);
		}
		this.mRoot = NGUITools.FindInParents<UIRoot>(base.gameObject);
		this.Update();
		this.mStarted = true;
	}

	private void Update()
	{
		UIWidget component;
		UIPanel uIPanel;
		if (this.mAnim != null && this.mAnim.isPlaying)
		{
			return;
		}
		if (this.style != UIStretch.Style.None)
		{
			if (this.container != null)
			{
				component = this.container.GetComponent<UIWidget>();
			}
			else
			{
				component = null;
			}
			UIWidget uIWidget = component;
			if (!(this.container == null) || !(uIWidget == null))
			{
				uIPanel = this.container.GetComponent<UIPanel>();
			}
			else
			{
				uIPanel = null;
			}
			UIPanel uIPanel1 = uIPanel;
			float single = 1f;
			if (uIWidget != null)
			{
				Bounds bound = uIWidget.CalculateBounds(base.transform.parent);
				Vector3 vector3 = bound.min;
				this.mRect.x = vector3.x;
				Vector3 vector31 = bound.min;
				this.mRect.y = vector31.y;
				Vector3 vector32 = bound.size;
				this.mRect.width = vector32.x;
				Vector3 vector33 = bound.size;
				this.mRect.height = vector33.y;
			}
			else if (uIPanel1 != null)
			{
				if (uIPanel1.clipping != UIDrawCall.Clipping.None)
				{
					Vector4 vector4 = uIPanel1.finalClipRegion;
					this.mRect.x = vector4.x - vector4.z * 0.5f;
					this.mRect.y = vector4.y - vector4.w * 0.5f;
					this.mRect.width = vector4.z;
					this.mRect.height = vector4.w;
				}
				else
				{
					float single1 = (this.mRoot == null ? 0.5f : (float)this.mRoot.activeHeight / (float)Screen.height * 0.5f);
					this.mRect.xMin = (float)(-Screen.width) * single1;
					this.mRect.yMin = (float)(-Screen.height) * single1;
					this.mRect.xMax = -this.mRect.xMin;
					this.mRect.yMax = -this.mRect.yMin;
				}
			}
			else if (this.container == null)
			{
				if (this.uiCamera == null)
				{
					return;
				}
				this.mRect = this.uiCamera.pixelRect;
				if (this.mRoot != null)
				{
					single = this.mRoot.pixelSizeAdjustment;
				}
			}
			else
			{
				Transform transforms = base.transform.parent;
				Bounds bound1 = (transforms == null ? NGUIMath.CalculateRelativeWidgetBounds(this.container.transform) : NGUIMath.CalculateRelativeWidgetBounds(transforms, this.container.transform));
				Vector3 vector34 = bound1.min;
				this.mRect.x = vector34.x;
				Vector3 vector35 = bound1.min;
				this.mRect.y = vector35.y;
				Vector3 vector36 = bound1.size;
				this.mRect.width = vector36.x;
				Vector3 vector37 = bound1.size;
				this.mRect.height = vector37.y;
			}
			float single2 = this.mRect.width;
			float single3 = this.mRect.height;
			if (single != 1f && single3 > 1f)
			{
				float single4 = (float)this.mRoot.activeHeight / single3;
				single2 *= single4;
				single3 *= single4;
			}
			Vector3 vector38 = (this.mWidget == null ? this.mTrans.localScale : new Vector3((float)this.mWidget.width, (float)this.mWidget.height));
			if (this.style == UIStretch.Style.BasedOnHeight)
			{
				vector38.x = this.relativeSize.x * single3;
				vector38.y = this.relativeSize.y * single3;
			}
			else if (this.style == UIStretch.Style.FillKeepingRatio)
			{
				float single5 = single2 / single3;
				if (this.initialSize.x / this.initialSize.y >= single5)
				{
					float single6 = single3 / this.initialSize.y;
					vector38.x = this.initialSize.x * single6;
					vector38.y = single3;
				}
				else
				{
					float single7 = single2 / this.initialSize.x;
					vector38.x = single2;
					vector38.y = this.initialSize.y * single7;
				}
			}
			else if (this.style != UIStretch.Style.FitInternalKeepingRatio)
			{
				if (this.style != UIStretch.Style.Vertical)
				{
					vector38.x = this.relativeSize.x * single2;
				}
				if (this.style != UIStretch.Style.Horizontal)
				{
					vector38.y = this.relativeSize.y * single3;
				}
			}
			else
			{
				float single8 = single2 / single3;
				if (this.initialSize.x / this.initialSize.y <= single8)
				{
					float single9 = single3 / this.initialSize.y;
					vector38.x = this.initialSize.x * single9;
					vector38.y = single3;
				}
				else
				{
					float single10 = single2 / this.initialSize.x;
					vector38.x = single2;
					vector38.y = this.initialSize.y * single10;
				}
			}
			if (this.mSprite != null)
			{
				float single11 = (this.mSprite.atlas == null ? 1f : this.mSprite.atlas.pixelSize);
				vector38.x = vector38.x - this.borderPadding.x * single11;
				vector38.y = vector38.y - this.borderPadding.y * single11;
				if (this.style != UIStretch.Style.Vertical)
				{
					this.mSprite.width = Mathf.RoundToInt(vector38.x);
				}
				if (this.style != UIStretch.Style.Horizontal)
				{
					this.mSprite.height = Mathf.RoundToInt(vector38.y);
				}
				vector38 = Vector3.one;
			}
			else if (this.mWidget != null)
			{
				if (this.style != UIStretch.Style.Vertical)
				{
					this.mWidget.width = Mathf.RoundToInt(vector38.x - this.borderPadding.x);
				}
				if (this.style != UIStretch.Style.Horizontal)
				{
					this.mWidget.height = Mathf.RoundToInt(vector38.y - this.borderPadding.y);
				}
				vector38 = Vector3.one;
			}
			else if (this.mPanel == null)
			{
				if (this.style != UIStretch.Style.Vertical)
				{
					vector38.x -= this.borderPadding.x;
				}
				if (this.style != UIStretch.Style.Horizontal)
				{
					vector38.y -= this.borderPadding.y;
				}
			}
			else
			{
				Vector4 vector41 = this.mPanel.baseClipRegion;
				if (this.style != UIStretch.Style.Vertical)
				{
					vector41.z = vector38.x - this.borderPadding.x;
				}
				if (this.style != UIStretch.Style.Horizontal)
				{
					vector41.w = vector38.y - this.borderPadding.y;
				}
				this.mPanel.baseClipRegion = vector41;
				vector38 = Vector3.one;
			}
			if (this.mTrans.localScale != vector38)
			{
				this.mTrans.localScale = vector38;
			}
			if (this.runOnlyOnce && Application.isPlaying)
			{
				base.enabled = false;
			}
		}
	}

	public enum Style
	{
		None,
		Horizontal,
		Vertical,
		Both,
		BasedOnHeight,
		FillKeepingRatio,
		FitInternalKeepingRatio
	}
}