using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Draggable Camera")]
[RequireComponent(typeof(Camera))]
public class UIDraggableCamera : MonoBehaviour
{
	public Transform rootForBounds;

	public Vector2 scale = Vector2.one;

	public float scrollWheelFactor;

	public UIDragObject.DragEffect dragEffect = UIDragObject.DragEffect.MomentumAndSpring;

	public bool smoothDragStart = true;

	public float momentumAmount = 35f;

	private Camera mCam;

	private Transform mTrans;

	private bool mPressed;

	private Vector2 mMomentum = Vector2.zero;

	private Bounds mBounds;

	private float mScroll;

	private UIRoot mRoot;

	private bool mDragStarted;

	public Vector2 currentMomentum
	{
		get
		{
			return this.mMomentum;
		}
		set
		{
			this.mMomentum = value;
		}
	}

	public UIDraggableCamera()
	{
	}

	private Vector3 CalculateConstrainOffset()
	{
		if (this.rootForBounds == null || this.rootForBounds.childCount == 0)
		{
			return Vector3.zero;
		}
		Rect rect = this.mCam.rect;
		float single = rect.xMin * (float)Screen.width;
		Rect rect1 = this.mCam.rect;
		Vector3 vector3 = new Vector3(single, rect1.yMin * (float)Screen.height, 0f);
		Rect rect2 = this.mCam.rect;
		float single1 = rect2.xMax * (float)Screen.width;
		Rect rect3 = this.mCam.rect;
		Vector3 worldPoint = new Vector3(single1, rect3.yMax * (float)Screen.height, 0f);
		vector3 = this.mCam.ScreenToWorldPoint(vector3);
		worldPoint = this.mCam.ScreenToWorldPoint(worldPoint);
		return NGUIMath.ConstrainRect(new Vector2(this.mBounds.min.x, this.mBounds.min.y), new Vector2(this.mBounds.max.x, this.mBounds.max.y), vector3, worldPoint);
	}

	public bool ConstrainToBounds(bool immediate)
	{
		if (this.mTrans != null && this.rootForBounds != null)
		{
			Vector3 vector3 = this.CalculateConstrainOffset();
			if (vector3.sqrMagnitude > 0f)
			{
				if (!immediate)
				{
					SpringPosition springPosition = SpringPosition.Begin(base.gameObject, this.mTrans.position - vector3, 13f);
					springPosition.ignoreTimeScale = true;
					springPosition.worldSpace = true;
				}
				else
				{
					Transform transforms = this.mTrans;
					transforms.position = transforms.position - vector3;
				}
				return true;
			}
		}
		return false;
	}

	public void Drag(Vector2 delta)
	{
		if (this.smoothDragStart && !this.mDragStarted)
		{
			this.mDragStarted = true;
			return;
		}
		UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
		if (this.mRoot != null)
		{
			delta *= this.mRoot.pixelSizeAdjustment;
		}
		Vector2 vector2 = Vector2.Scale(delta, -this.scale);
		Transform transforms = this.mTrans;
		transforms.localPosition = transforms.localPosition + vector2;
		this.mMomentum = Vector2.Lerp(this.mMomentum, this.mMomentum + (vector2 * (0.01f * this.momentumAmount)), 0.67f);
		if (this.dragEffect != UIDragObject.DragEffect.MomentumAndSpring && this.ConstrainToBounds(true))
		{
			this.mMomentum = Vector2.zero;
			this.mScroll = 0f;
		}
	}

	public void Press(bool isPressed)
	{
		if (isPressed)
		{
			this.mDragStarted = false;
		}
		if (this.rootForBounds != null)
		{
			this.mPressed = isPressed;
			if (isPressed)
			{
				this.mBounds = NGUIMath.CalculateAbsoluteWidgetBounds(this.rootForBounds);
				this.mMomentum = Vector2.zero;
				this.mScroll = 0f;
				SpringPosition component = base.GetComponent<SpringPosition>();
				if (component != null)
				{
					component.enabled = false;
				}
			}
			else if (this.dragEffect == UIDragObject.DragEffect.MomentumAndSpring)
			{
				this.ConstrainToBounds(false);
			}
		}
	}

	public void Scroll(float delta)
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject))
		{
			if (Mathf.Sign(this.mScroll) != Mathf.Sign(delta))
			{
				this.mScroll = 0f;
			}
			UIDraggableCamera uIDraggableCamera = this;
			uIDraggableCamera.mScroll = uIDraggableCamera.mScroll + delta * this.scrollWheelFactor;
		}
	}

	private void Start()
	{
		this.mCam = base.GetComponent<Camera>();
		this.mTrans = base.transform;
		this.mRoot = NGUITools.FindInParents<UIRoot>(base.gameObject);
		if (this.rootForBounds == null)
		{
			Debug.LogError(string.Concat(NGUITools.GetHierarchy(base.gameObject), " needs the 'Root For Bounds' parameter to be set"), this);
			base.enabled = false;
		}
	}

	private void Update()
	{
		float single = RealTime.deltaTime;
		if (!this.mPressed)
		{
			UIDraggableCamera uIDraggableCamera = this;
			uIDraggableCamera.mMomentum = uIDraggableCamera.mMomentum + (this.scale * (this.mScroll * 20f));
			this.mScroll = NGUIMath.SpringLerp(this.mScroll, 0f, 20f, single);
			if (this.mMomentum.magnitude > 0.01f)
			{
				Transform transforms = this.mTrans;
				transforms.localPosition = transforms.localPosition + NGUIMath.SpringDampen(ref this.mMomentum, 9f, single);
				this.mBounds = NGUIMath.CalculateAbsoluteWidgetBounds(this.rootForBounds);
				if (!this.ConstrainToBounds(this.dragEffect == UIDragObject.DragEffect.None))
				{
					SpringPosition component = base.GetComponent<SpringPosition>();
					if (component != null)
					{
						component.enabled = false;
					}
				}
				return;
			}
			this.mScroll = 0f;
		}
		else
		{
			SpringPosition springPosition = base.GetComponent<SpringPosition>();
			if (springPosition != null)
			{
				springPosition.enabled = false;
			}
			this.mScroll = 0f;
		}
		NGUIMath.SpringDampen(ref this.mMomentum, 9f, single);
	}
}