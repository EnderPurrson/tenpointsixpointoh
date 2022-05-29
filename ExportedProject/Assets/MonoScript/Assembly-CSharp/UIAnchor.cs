using System;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Anchor")]
[ExecuteInEditMode]
public class UIAnchor : MonoBehaviour
{
	public Camera uiCamera;

	public GameObject container;

	public UIAnchor.Side side = UIAnchor.Side.Center;

	public bool runOnlyOnce = true;

	public Vector2 relativeOffset = Vector2.zero;

	public Vector2 pixelOffset = Vector2.zero;

	[HideInInspector]
	[SerializeField]
	private UIWidget widgetContainer;

	private Transform mTrans;

	private Animation mAnim;

	private Rect mRect = new Rect();

	private UIRoot mRoot;

	private bool mStarted;

	public UIAnchor()
	{
	}

	private void Awake()
	{
		this.mTrans = base.transform;
		this.mAnim = base.GetComponent<Animation>();
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
		this.mRoot = NGUITools.FindInParents<UIRoot>(base.gameObject);
		if (this.uiCamera == null)
		{
			this.uiCamera = NGUITools.FindCameraForLayer(base.gameObject.layer);
		}
		this.Update();
		this.mStarted = true;
	}

	private void Update()
	{
		UIWidget component;
		UIPanel uIPanel;
		if (this.mAnim != null && this.mAnim.enabled && this.mAnim.isPlaying)
		{
			return;
		}
		bool flag = false;
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
		if (uIWidget != null)
		{
			Bounds bound = uIWidget.CalculateBounds(this.container.transform.parent);
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
				float single = (this.mRoot == null ? 0.5f : (float)this.mRoot.activeHeight / (float)Screen.height * 0.5f);
				this.mRect.xMin = (float)(-Screen.width) * single;
				this.mRect.yMin = (float)(-Screen.height) * single;
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
			flag = true;
			this.mRect = this.uiCamera.pixelRect;
		}
		else
		{
			Transform transforms = this.container.transform.parent;
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
		float single1 = (this.mRect.xMin + this.mRect.xMax) * 0.5f;
		float single2 = (this.mRect.yMin + this.mRect.yMax) * 0.5f;
		Vector3 worldPoint = new Vector3(single1, single2, 0f);
		if (this.side != UIAnchor.Side.Center)
		{
			if (this.side == UIAnchor.Side.Right || this.side == UIAnchor.Side.TopRight || this.side == UIAnchor.Side.BottomRight)
			{
				worldPoint.x = this.mRect.xMax;
			}
			else if (this.side == UIAnchor.Side.Top || this.side == UIAnchor.Side.Center || this.side == UIAnchor.Side.Bottom)
			{
				worldPoint.x = single1;
			}
			else
			{
				worldPoint.x = this.mRect.xMin;
			}
			if (this.side == UIAnchor.Side.Top || this.side == UIAnchor.Side.TopRight || this.side == UIAnchor.Side.TopLeft)
			{
				worldPoint.y = this.mRect.yMax;
			}
			else if (this.side == UIAnchor.Side.Left || this.side == UIAnchor.Side.Center || this.side == UIAnchor.Side.Right)
			{
				worldPoint.y = single2;
			}
			else
			{
				worldPoint.y = this.mRect.yMin;
			}
		}
		float single3 = this.mRect.width;
		float single4 = this.mRect.height;
		worldPoint.x = worldPoint.x + (this.pixelOffset.x + this.relativeOffset.x * single3);
		worldPoint.y = worldPoint.y + (this.pixelOffset.y + this.relativeOffset.y * single4);
		if (!flag)
		{
			worldPoint.x = Mathf.Round(worldPoint.x);
			worldPoint.y = Mathf.Round(worldPoint.y);
			if (uIPanel1 != null)
			{
				worldPoint = uIPanel1.cachedTransform.TransformPoint(worldPoint);
			}
			else if (this.container != null)
			{
				Transform transforms1 = this.container.transform.parent;
				if (transforms1 != null)
				{
					worldPoint = transforms1.TransformPoint(worldPoint);
				}
			}
			worldPoint.z = this.mTrans.position.z;
		}
		else
		{
			if (this.uiCamera.orthographic)
			{
				worldPoint.x = Mathf.Round(worldPoint.x);
				worldPoint.y = Mathf.Round(worldPoint.y);
			}
			Vector3 screenPoint = this.uiCamera.WorldToScreenPoint(this.mTrans.position);
			worldPoint.z = screenPoint.z;
			worldPoint = this.uiCamera.ScreenToWorldPoint(worldPoint);
		}
		if (flag && this.uiCamera.orthographic && this.mTrans.parent != null)
		{
			worldPoint = this.mTrans.parent.InverseTransformPoint(worldPoint);
			worldPoint.x = (float)Mathf.RoundToInt(worldPoint.x);
			worldPoint.y = (float)Mathf.RoundToInt(worldPoint.y);
			if (this.mTrans.localPosition != worldPoint)
			{
				this.mTrans.localPosition = worldPoint;
			}
		}
		else if (this.mTrans.position != worldPoint)
		{
			this.mTrans.position = worldPoint;
		}
		if (this.runOnlyOnce && Application.isPlaying)
		{
			base.enabled = false;
		}
	}

	public enum Side
	{
		BottomLeft,
		Left,
		TopLeft,
		Top,
		TopRight,
		Right,
		BottomRight,
		Bottom,
		Center
	}
}