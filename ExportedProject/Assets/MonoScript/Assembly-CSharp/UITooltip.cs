using System;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Tooltip")]
public class UITooltip : MonoBehaviour
{
	protected static UITooltip mInstance;

	public Camera uiCamera;

	public UILabel text;

	public GameObject tooltipRoot;

	public UISprite background;

	public float appearSpeed = 10f;

	public bool scalingTransitions = true;

	protected GameObject mTooltip;

	protected Transform mTrans;

	protected float mTarget;

	protected float mCurrent;

	protected Vector3 mPos;

	protected Vector3 mSize = Vector3.zero;

	protected UIWidget[] mWidgets;

	public static bool isVisible
	{
		get
		{
			return (UITooltip.mInstance == null ? false : UITooltip.mInstance.mTarget == 1f);
		}
	}

	public UITooltip()
	{
	}

	private void Awake()
	{
		UITooltip.mInstance = this;
	}

	public static void Hide()
	{
		if (UITooltip.mInstance != null)
		{
			UITooltip.mInstance.mTooltip = null;
			UITooltip.mInstance.mTarget = 0f;
		}
	}

	private void OnDestroy()
	{
		UITooltip.mInstance = null;
	}

	protected virtual void SetAlpha(float val)
	{
		int num = 0;
		int length = (int)this.mWidgets.Length;
		while (num < length)
		{
			UIWidget uIWidget = this.mWidgets[num];
			Color color = uIWidget.color;
			color.a = val;
			uIWidget.color = color;
			num++;
		}
	}

	protected virtual void SetText(string tooltipText)
	{
		if (!(this.text != null) || string.IsNullOrEmpty(tooltipText))
		{
			this.mTooltip = null;
			this.mTarget = 0f;
		}
		else
		{
			this.mTarget = 1f;
			this.mTooltip = UICamera.tooltipObject;
			this.text.text = tooltipText;
			this.mPos = UICamera.lastEventPosition;
			Transform transforms = this.text.transform;
			Vector3 vector3 = transforms.localPosition;
			Vector3 vector31 = transforms.localScale;
			this.mSize = this.text.printedSize;
			this.mSize.x *= vector31.x;
			this.mSize.y *= vector31.y;
			if (this.background != null)
			{
				Vector4 vector4 = this.background.border;
				ref Vector3 vector3Pointer = ref this.mSize;
				vector3Pointer.x = vector3Pointer.x + (vector4.x + vector4.z + (vector3.x - vector4.x) * 2f);
				ref Vector3 vector3Pointer1 = ref this.mSize;
				vector3Pointer1.y = vector3Pointer1.y + (vector4.y + vector4.w + (-vector3.y - vector4.y) * 2f);
				this.background.width = Mathf.RoundToInt(this.mSize.x);
				this.background.height = Mathf.RoundToInt(this.mSize.y);
			}
			if (this.uiCamera == null)
			{
				if (this.mPos.x + this.mSize.x > (float)Screen.width)
				{
					this.mPos.x = (float)Screen.width - this.mSize.x;
				}
				if (this.mPos.y - this.mSize.y < 0f)
				{
					this.mPos.y = this.mSize.y;
				}
				ref Vector3 vector3Pointer2 = ref this.mPos;
				vector3Pointer2.x = vector3Pointer2.x - (float)Screen.width * 0.5f;
				ref Vector3 vector3Pointer3 = ref this.mPos;
				vector3Pointer3.y = vector3Pointer3.y - (float)Screen.height * 0.5f;
			}
			else
			{
				this.mPos.x = Mathf.Clamp01(this.mPos.x / (float)Screen.width);
				this.mPos.y = Mathf.Clamp01(this.mPos.y / (float)Screen.height);
				float single = this.uiCamera.orthographicSize / this.mTrans.parent.lossyScale.y;
				float single1 = (float)Screen.height * 0.5f / single;
				Vector2 vector2 = new Vector2(single1 * this.mSize.x / (float)Screen.width, single1 * this.mSize.y / (float)Screen.height);
				this.mPos.x = Mathf.Min(this.mPos.x, 1f - vector2.x);
				this.mPos.y = Mathf.Max(this.mPos.y, vector2.y);
				this.mTrans.position = this.uiCamera.ViewportToWorldPoint(this.mPos);
				this.mPos = this.mTrans.localPosition;
				this.mPos.x = Mathf.Round(this.mPos.x);
				this.mPos.y = Mathf.Round(this.mPos.y);
				this.mTrans.localPosition = this.mPos;
			}
			if (this.tooltipRoot == null)
			{
				this.text.BroadcastMessage("UpdateAnchors");
			}
			else
			{
				this.tooltipRoot.BroadcastMessage("UpdateAnchors");
			}
		}
	}

	public static void Show(string text)
	{
		if (UITooltip.mInstance != null)
		{
			UITooltip.mInstance.SetText(text);
		}
	}

	[Obsolete("Use UITooltip.Show instead")]
	public static void ShowText(string text)
	{
		if (UITooltip.mInstance != null)
		{
			UITooltip.mInstance.SetText(text);
		}
	}

	protected virtual void Start()
	{
		this.mTrans = base.transform;
		this.mWidgets = base.GetComponentsInChildren<UIWidget>();
		this.mPos = this.mTrans.localPosition;
		if (this.uiCamera == null)
		{
			this.uiCamera = NGUITools.FindCameraForLayer(base.gameObject.layer);
		}
		this.SetAlpha(0f);
	}

	protected virtual void Update()
	{
		if (this.mTooltip != UICamera.tooltipObject)
		{
			this.mTooltip = null;
			this.mTarget = 0f;
		}
		if (this.mCurrent != this.mTarget)
		{
			this.mCurrent = Mathf.Lerp(this.mCurrent, this.mTarget, RealTime.deltaTime * this.appearSpeed);
			if (Mathf.Abs(this.mCurrent - this.mTarget) < 0.001f)
			{
				this.mCurrent = this.mTarget;
			}
			this.SetAlpha(this.mCurrent * this.mCurrent);
			if (this.scalingTransitions)
			{
				Vector3 vector3 = this.mSize * 0.25f;
				vector3.y = -vector3.y;
				Vector3 vector31 = Vector3.one * (1.5f - this.mCurrent * 0.5f);
				Vector3 vector32 = Vector3.Lerp(this.mPos - vector3, this.mPos, this.mCurrent);
				this.mTrans.localPosition = vector32;
				this.mTrans.localScale = vector31;
			}
		}
	}
}