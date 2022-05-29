using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Drag Object")]
[ExecuteInEditMode]
public class UIDragObject : MonoBehaviour
{
	public Transform target;

	public UIPanel panelRegion;

	public Vector3 scrollMomentum = Vector3.zero;

	public bool restrictWithinPanel;

	public UIRect contentRect;

	public UIDragObject.DragEffect dragEffect = UIDragObject.DragEffect.MomentumAndSpring;

	public float momentumAmount = 35f;

	[SerializeField]
	protected Vector3 scale = new Vector3(1f, 1f, 0f);

	[HideInInspector]
	[SerializeField]
	private float scrollWheelFactor;

	private Plane mPlane;

	private Vector3 mTargetPos;

	private Vector3 mLastPos;

	private Vector3 mMomentum = Vector3.zero;

	private Vector3 mScroll = Vector3.zero;

	private Bounds mBounds;

	private int mTouchID;

	private bool mStarted;

	private bool mPressed;

	public Vector3 dragMovement
	{
		get
		{
			return this.scale;
		}
		set
		{
			this.scale = value;
		}
	}

	public UIDragObject()
	{
	}

	public void CancelMovement()
	{
		if (this.target != null)
		{
			Vector3 num = this.target.localPosition;
			num.x = (float)Mathf.RoundToInt(num.x);
			num.y = (float)Mathf.RoundToInt(num.y);
			num.z = (float)Mathf.RoundToInt(num.z);
			this.target.localPosition = num;
		}
		this.mTargetPos = (this.target == null ? Vector3.zero : this.target.position);
		this.mMomentum = Vector3.zero;
		this.mScroll = Vector3.zero;
	}

	public void CancelSpring()
	{
		SpringPosition component = this.target.GetComponent<SpringPosition>();
		if (component != null)
		{
			component.enabled = false;
		}
	}

	private void FindPanel()
	{
		UIPanel uIPanel;
		if (this.target == null)
		{
			uIPanel = null;
		}
		else
		{
			uIPanel = UIPanel.Find(this.target.transform.parent);
		}
		this.panelRegion = uIPanel;
		if (this.panelRegion == null)
		{
			this.restrictWithinPanel = false;
		}
	}

	private void LateUpdate()
	{
		if (this.target == null)
		{
			return;
		}
		float single = RealTime.deltaTime;
		this.mMomentum -= this.mScroll;
		this.mScroll = NGUIMath.SpringLerp(this.mScroll, Vector3.zero, 20f, single);
		if (this.mMomentum.magnitude < 0.0001f)
		{
			return;
		}
		if (this.mPressed)
		{
			NGUIMath.SpringDampen(ref this.mMomentum, 9f, single);
		}
		else
		{
			if (this.panelRegion == null)
			{
				this.FindPanel();
			}
			this.Move(NGUIMath.SpringDampen(ref this.mMomentum, 9f, single));
			if (this.restrictWithinPanel && this.panelRegion != null)
			{
				this.UpdateBounds();
				if (!this.panelRegion.ConstrainTargetToBounds(this.target, ref this.mBounds, this.dragEffect == UIDragObject.DragEffect.None))
				{
					this.CancelSpring();
				}
				else
				{
					this.CancelMovement();
				}
			}
			NGUIMath.SpringDampen(ref this.mMomentum, 9f, single);
			if (this.mMomentum.magnitude < 0.0001f)
			{
				this.CancelMovement();
			}
		}
	}

	private void Move(Vector3 worldDelta)
	{
		if (this.panelRegion == null)
		{
			Transform transforms = this.target;
			transforms.position = transforms.position + worldDelta;
		}
		else
		{
			this.mTargetPos += worldDelta;
			Transform transforms1 = this.target.parent;
			Rigidbody component = this.target.GetComponent<Rigidbody>();
			if (transforms1 != null)
			{
				Vector3 vector3 = transforms1.worldToLocalMatrix.MultiplyPoint3x4(this.mTargetPos);
				vector3.x = Mathf.Round(vector3.x);
				vector3.y = Mathf.Round(vector3.y);
				if (component == null)
				{
					this.target.localPosition = vector3;
				}
				else
				{
					vector3 = transforms1.localToWorldMatrix.MultiplyPoint3x4(vector3);
					component.position = vector3;
				}
			}
			else if (component == null)
			{
				this.target.position = this.mTargetPos;
			}
			else
			{
				component.position = this.mTargetPos;
			}
			UIScrollView uIScrollView = this.panelRegion.GetComponent<UIScrollView>();
			if (uIScrollView != null)
			{
				uIScrollView.UpdateScrollbars(true);
			}
		}
	}

	private void OnDisable()
	{
		this.mStarted = false;
	}

	private void OnDrag(Vector2 delta)
	{
		if (this.mPressed && this.mTouchID == UICamera.currentTouchID && base.enabled && NGUITools.GetActive(base.gameObject) && this.target != null)
		{
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
			Ray ray = UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos);
			float single = 0f;
			if (this.mPlane.Raycast(ray, out single))
			{
				Vector3 point = ray.GetPoint(single);
				Vector3 vector3 = point - this.mLastPos;
				this.mLastPos = point;
				if (!this.mStarted)
				{
					this.mStarted = true;
					vector3 = Vector3.zero;
				}
				if (vector3.x != 0f || vector3.y != 0f)
				{
					vector3 = this.target.InverseTransformDirection(vector3);
					vector3.Scale(this.scale);
					vector3 = this.target.TransformDirection(vector3);
				}
				if (this.dragEffect != UIDragObject.DragEffect.None)
				{
					this.mMomentum = Vector3.Lerp(this.mMomentum, this.mMomentum + (vector3 * (0.01f * this.momentumAmount)), 0.67f);
				}
				Vector3 vector31 = this.target.localPosition;
				this.Move(vector3);
				if (this.restrictWithinPanel)
				{
					this.mBounds.center = this.mBounds.center + (this.target.localPosition - vector31);
					if (this.dragEffect != UIDragObject.DragEffect.MomentumAndSpring && this.panelRegion.ConstrainTargetToBounds(this.target, ref this.mBounds, true))
					{
						this.CancelMovement();
					}
				}
			}
		}
	}

	private void OnEnable()
	{
		if (this.scrollWheelFactor != 0f)
		{
			this.scrollMomentum = this.scale * this.scrollWheelFactor;
			this.scrollWheelFactor = 0f;
		}
		if (this.contentRect == null && this.target != null && Application.isPlaying)
		{
			UIWidget component = this.target.GetComponent<UIWidget>();
			if (component != null)
			{
				this.contentRect = component;
			}
		}
		this.mTargetPos = (this.target == null ? Vector3.zero : this.target.position);
	}

	private void OnPress(bool pressed)
	{
		if (UICamera.currentTouchID == -2 || UICamera.currentTouchID == -3)
		{
			return;
		}
		float single = Time.timeScale;
		if (single < 0.01f && single != 0f)
		{
			return;
		}
		if (base.enabled && NGUITools.GetActive(base.gameObject) && this.target != null)
		{
			if (pressed)
			{
				if (!this.mPressed)
				{
					this.mTouchID = UICamera.currentTouchID;
					this.mPressed = true;
					this.mStarted = false;
					this.CancelMovement();
					if (this.restrictWithinPanel && this.panelRegion == null)
					{
						this.FindPanel();
					}
					if (this.restrictWithinPanel)
					{
						this.UpdateBounds();
					}
					this.CancelSpring();
					Transform transforms = UICamera.currentCamera.transform;
					this.mPlane = new Plane((this.panelRegion == null ? transforms.rotation : this.panelRegion.cachedTransform.rotation) * Vector3.back, UICamera.lastWorldPosition);
				}
			}
			else if (this.mPressed && this.mTouchID == UICamera.currentTouchID)
			{
				this.mPressed = false;
				if (this.restrictWithinPanel && this.dragEffect == UIDragObject.DragEffect.MomentumAndSpring && this.panelRegion.ConstrainTargetToBounds(this.target, ref this.mBounds, false))
				{
					this.CancelMovement();
				}
			}
		}
	}

	private void OnScroll(float delta)
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject))
		{
			UIDragObject uIDragObject = this;
			uIDragObject.mScroll = uIDragObject.mScroll - (this.scrollMomentum * (delta * 0.05f));
		}
	}

	private void UpdateBounds()
	{
		if (!this.contentRect)
		{
			this.mBounds = NGUIMath.CalculateRelativeWidgetBounds(this.panelRegion.cachedTransform, this.target);
		}
		else
		{
			Matrix4x4 matrix4x4 = this.panelRegion.cachedTransform.worldToLocalMatrix;
			Vector3[] vector3Array = this.contentRect.worldCorners;
			for (int i = 0; i < 4; i++)
			{
				vector3Array[i] = matrix4x4.MultiplyPoint3x4(vector3Array[i]);
			}
			this.mBounds = new Bounds(vector3Array[0], Vector3.zero);
			for (int j = 1; j < 4; j++)
			{
				this.mBounds.Encapsulate(vector3Array[j]);
			}
		}
	}

	public enum DragEffect
	{
		None,
		Momentum,
		MomentumAndSpring
	}
}