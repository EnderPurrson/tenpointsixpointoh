using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Scroll View")]
[ExecuteInEditMode]
[RequireComponent(typeof(UIPanel))]
public class UIScrollView : MonoBehaviour
{
	public static BetterList<UIScrollView> list;

	public UIScrollView.Movement movement;

	public UIScrollView.DragEffect dragEffect = UIScrollView.DragEffect.MomentumAndSpring;

	public bool restrictWithinPanel = true;

	public bool disableDragIfFits;

	public bool smoothDragStart = true;

	public bool iOSDragEmulation = true;

	public float scrollWheelFactor = 0.25f;

	public float momentumAmount = 35f;

	public float dampenStrength = 9f;

	public UIProgressBar horizontalScrollBar;

	public UIProgressBar verticalScrollBar;

	public UIScrollView.ShowCondition showScrollBars = UIScrollView.ShowCondition.OnlyIfNeeded;

	public Vector2 customMovement = new Vector2(1f, 0f);

	public UIWidget.Pivot contentPivot;

	public UIScrollView.OnDragNotification onDragStarted;

	public UIScrollView.OnDragNotification onDragFinished;

	public UIScrollView.OnDragNotification onMomentumMove;

	public UIScrollView.OnDragNotification onStoppedMoving;

	[HideInInspector]
	[SerializeField]
	private Vector3 scale = new Vector3(1f, 0f, 0f);

	[HideInInspector]
	[SerializeField]
	private Vector2 relativePositionOnReset = Vector2.zero;

	protected Transform mTrans;

	protected UIPanel mPanel;

	protected Plane mPlane;

	protected Vector3 mLastPos;

	protected bool mPressed;

	protected Vector3 mMomentum = Vector3.zero;

	protected float mScroll;

	protected Bounds mBounds;

	protected bool mCalculatedBounds;

	protected bool mShouldMove;

	protected bool mIgnoreCallbacks;

	protected int mDragID = -10;

	protected Vector2 mDragStartOffset = Vector2.zero;

	protected bool mDragStarted;

	[NonSerialized]
	private bool mStarted;

	[HideInInspector]
	public UICenterOnChild centerOnChild;

	public virtual Bounds bounds
	{
		get
		{
			if (!this.mCalculatedBounds)
			{
				this.mCalculatedBounds = true;
				this.mTrans = base.transform;
				this.mBounds = NGUIMath.CalculateRelativeWidgetBounds(this.mTrans, this.mTrans);
			}
			return this.mBounds;
		}
	}

	public bool canMoveHorizontally
	{
		get
		{
			bool flag;
			if (this.movement == UIScrollView.Movement.Horizontal || this.movement == UIScrollView.Movement.Unrestricted)
			{
				flag = true;
			}
			else
			{
				flag = (this.movement != UIScrollView.Movement.Custom ? false : this.customMovement.x != 0f);
			}
			return flag;
		}
	}

	public bool canMoveVertically
	{
		get
		{
			bool flag;
			if (this.movement == UIScrollView.Movement.Vertical || this.movement == UIScrollView.Movement.Unrestricted)
			{
				flag = true;
			}
			else
			{
				flag = (this.movement != UIScrollView.Movement.Custom ? false : this.customMovement.y != 0f);
			}
			return flag;
		}
	}

	public Vector3 currentMomentum
	{
		get
		{
			return this.mMomentum;
		}
		set
		{
			this.mMomentum = value;
			this.mShouldMove = true;
		}
	}

	public bool isDragging
	{
		get
		{
			return (!this.mPressed ? false : this.mDragStarted);
		}
	}

	public UIPanel panel
	{
		get
		{
			return this.mPanel;
		}
	}

	protected virtual bool shouldMove
	{
		get
		{
			if (!this.disableDragIfFits)
			{
				return true;
			}
			if (this.mPanel == null)
			{
				this.mPanel = base.GetComponent<UIPanel>();
			}
			Vector4 vector4 = this.mPanel.finalClipRegion;
			Bounds bound = this.bounds;
			float single = (vector4.z != 0f ? vector4.z * 0.5f : (float)Screen.width);
			float single1 = (vector4.w != 0f ? vector4.w * 0.5f : (float)Screen.height);
			if (this.canMoveHorizontally)
			{
				if (bound.min.x < vector4.x - single)
				{
					return true;
				}
				if (bound.max.x > vector4.x + single)
				{
					return true;
				}
			}
			if (this.canMoveVertically)
			{
				if (bound.min.y < vector4.y - single1)
				{
					return true;
				}
				if (bound.max.y > vector4.y + single1)
				{
					return true;
				}
			}
			return false;
		}
	}

	public virtual bool shouldMoveHorizontally
	{
		get
		{
			float single = this.bounds.size.x;
			if (this.mPanel.clipping == UIDrawCall.Clipping.SoftClip)
			{
				Vector2 vector2 = this.mPanel.clipSoftness;
				single = single + vector2.x * 2f;
			}
			return Mathf.RoundToInt(single - this.mPanel.width) > 0;
		}
	}

	public virtual bool shouldMoveVertically
	{
		get
		{
			float single = this.bounds.size.y;
			if (this.mPanel.clipping == UIDrawCall.Clipping.SoftClip)
			{
				Vector2 vector2 = this.mPanel.clipSoftness;
				single = single + vector2.y * 2f;
			}
			return Mathf.RoundToInt(single - this.mPanel.height) > 0;
		}
	}

	static UIScrollView()
	{
		UIScrollView.list = new BetterList<UIScrollView>();
	}

	public UIScrollView()
	{
	}

	private void Awake()
	{
		this.mTrans = base.transform;
		this.mPanel = base.GetComponent<UIPanel>();
		if (this.mPanel.clipping == UIDrawCall.Clipping.None)
		{
			this.mPanel.clipping = UIDrawCall.Clipping.ConstrainButDontClip;
		}
		if (this.movement != UIScrollView.Movement.Custom && this.scale.sqrMagnitude > 0.001f)
		{
			if (this.scale.x == 1f && this.scale.y == 0f)
			{
				this.movement = UIScrollView.Movement.Horizontal;
			}
			else if (this.scale.x == 0f && this.scale.y == 1f)
			{
				this.movement = UIScrollView.Movement.Vertical;
			}
			else if (this.scale.x != 1f || this.scale.y != 1f)
			{
				this.movement = UIScrollView.Movement.Custom;
				this.customMovement.x = this.scale.x;
				this.customMovement.y = this.scale.y;
			}
			else
			{
				this.movement = UIScrollView.Movement.Unrestricted;
			}
			this.scale = Vector3.zero;
		}
		if (this.contentPivot == UIWidget.Pivot.TopLeft && this.relativePositionOnReset != Vector2.zero)
		{
			this.contentPivot = NGUIMath.GetPivot(new Vector2(this.relativePositionOnReset.x, 1f - this.relativePositionOnReset.y));
			this.relativePositionOnReset = Vector2.zero;
		}
	}

	private void CheckScrollbars()
	{
		if (this.horizontalScrollBar != null)
		{
			EventDelegate.Add(this.horizontalScrollBar.onChange, new EventDelegate.Callback(this.OnScrollBar));
			this.horizontalScrollBar.BroadcastMessage("CacheDefaultColor", SendMessageOptions.DontRequireReceiver);
			this.horizontalScrollBar.alpha = (this.showScrollBars == UIScrollView.ShowCondition.Always || this.shouldMoveHorizontally ? 1f : 0f);
		}
		if (this.verticalScrollBar != null)
		{
			EventDelegate.Add(this.verticalScrollBar.onChange, new EventDelegate.Callback(this.OnScrollBar));
			this.verticalScrollBar.BroadcastMessage("CacheDefaultColor", SendMessageOptions.DontRequireReceiver);
			this.verticalScrollBar.alpha = (this.showScrollBars == UIScrollView.ShowCondition.Always || this.shouldMoveVertically ? 1f : 0f);
		}
	}

	public void DisableSpring()
	{
		SpringPanel component = base.GetComponent<SpringPanel>();
		if (component != null)
		{
			component.enabled = false;
		}
	}

	public void Drag()
	{
		if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
		{
			return;
		}
		if (base.enabled && NGUITools.GetActive(base.gameObject) && this.mShouldMove)
		{
			if (this.mDragID == -10)
			{
				this.mDragID = UICamera.currentTouchID;
			}
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
			if (this.smoothDragStart && !this.mDragStarted)
			{
				this.mDragStarted = true;
				this.mDragStartOffset = UICamera.currentTouch.totalDelta;
				if (this.onDragStarted != null)
				{
					this.onDragStarted();
				}
			}
			Ray ray = (!this.smoothDragStart ? UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos) : UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos - this.mDragStartOffset));
			float single = 0f;
			if (this.mPlane.Raycast(ray, out single))
			{
				Vector3 point = ray.GetPoint(single);
				Vector3 vector3 = point - this.mLastPos;
				this.mLastPos = point;
				if (vector3.x != 0f || vector3.y != 0f || vector3.z != 0f)
				{
					vector3 = this.mTrans.InverseTransformDirection(vector3);
					if (this.movement == UIScrollView.Movement.Horizontal)
					{
						vector3.y = 0f;
						vector3.z = 0f;
					}
					else if (this.movement == UIScrollView.Movement.Vertical)
					{
						vector3.x = 0f;
						vector3.z = 0f;
					}
					else if (this.movement != UIScrollView.Movement.Unrestricted)
					{
						vector3.Scale(this.customMovement);
					}
					else
					{
						vector3.z = 0f;
					}
					vector3 = this.mTrans.TransformDirection(vector3);
				}
				if (this.dragEffect != UIScrollView.DragEffect.None)
				{
					this.mMomentum = Vector3.Lerp(this.mMomentum, this.mMomentum + (vector3 * (0.01f * this.momentumAmount)), 0.67f);
				}
				else
				{
					this.mMomentum = Vector3.zero;
				}
				if (!this.iOSDragEmulation || this.dragEffect != UIScrollView.DragEffect.MomentumAndSpring)
				{
					this.MoveAbsolute(vector3);
				}
				else if (this.mPanel.CalculateConstrainOffset(this.bounds.min, this.bounds.max).magnitude <= 1f)
				{
					this.MoveAbsolute(vector3);
				}
				else
				{
					this.MoveAbsolute(vector3 * 0.5f);
					this.mMomentum *= 0.5f;
				}
				if (this.restrictWithinPanel && this.mPanel.clipping != UIDrawCall.Clipping.None && this.dragEffect != UIScrollView.DragEffect.MomentumAndSpring)
				{
					this.RestrictWithinBounds(true, this.canMoveHorizontally, this.canMoveVertically);
				}
			}
		}
	}

	public void InvalidateBounds()
	{
		this.mCalculatedBounds = false;
	}

	private void LateUpdate()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		float single = RealTime.deltaTime;
		if (this.showScrollBars != UIScrollView.ShowCondition.Always && (this.verticalScrollBar || this.horizontalScrollBar))
		{
			bool flag = false;
			bool flag1 = false;
			if (this.showScrollBars != UIScrollView.ShowCondition.WhenDragging || this.mDragID != -10 || this.mMomentum.magnitude > 0.01f)
			{
				flag = this.shouldMoveVertically;
				flag1 = this.shouldMoveHorizontally;
			}
			if (this.verticalScrollBar)
			{
				float single1 = this.verticalScrollBar.alpha;
				single1 = single1 + (!flag ? -single * 3f : single * 6f);
				single1 = Mathf.Clamp01(single1);
				if (this.verticalScrollBar.alpha != single1)
				{
					this.verticalScrollBar.alpha = single1;
				}
			}
			if (this.horizontalScrollBar)
			{
				float single2 = this.horizontalScrollBar.alpha;
				single2 = single2 + (!flag1 ? -single * 3f : single * 6f);
				single2 = Mathf.Clamp01(single2);
				if (this.horizontalScrollBar.alpha != single2)
				{
					this.horizontalScrollBar.alpha = single2;
				}
			}
		}
		if (!this.mShouldMove)
		{
			return;
		}
		if (this.mPressed)
		{
			this.mScroll = 0f;
			NGUIMath.SpringDampen(ref this.mMomentum, 9f, single);
		}
		else if (this.mMomentum.magnitude > 0.0001f || this.mScroll != 0f)
		{
			if (this.movement == UIScrollView.Movement.Horizontal)
			{
				this.mMomentum -= this.mTrans.TransformDirection(new Vector3(this.mScroll * 0.05f, 0f, 0f));
			}
			else if (this.movement == UIScrollView.Movement.Vertical)
			{
				this.mMomentum -= this.mTrans.TransformDirection(new Vector3(0f, this.mScroll * 0.05f, 0f));
			}
			else if (this.movement != UIScrollView.Movement.Unrestricted)
			{
				this.mMomentum -= this.mTrans.TransformDirection(new Vector3(this.mScroll * this.customMovement.x * 0.05f, this.mScroll * this.customMovement.y * 0.05f, 0f));
			}
			else
			{
				this.mMomentum -= this.mTrans.TransformDirection(new Vector3(this.mScroll * 0.05f, this.mScroll * 0.05f, 0f));
			}
			this.mScroll = NGUIMath.SpringLerp(this.mScroll, 0f, 20f, single);
			Vector3 vector3 = NGUIMath.SpringDampen(ref this.mMomentum, this.dampenStrength, single);
			this.MoveAbsolute(vector3);
			if (this.restrictWithinPanel && this.mPanel.clipping != UIDrawCall.Clipping.None)
			{
				if (!NGUITools.GetActive(this.centerOnChild))
				{
					this.RestrictWithinBounds(false, this.canMoveHorizontally, this.canMoveVertically);
				}
				else if (this.centerOnChild.nextPageThreshold == 0f)
				{
					this.centerOnChild.Recenter();
				}
				else
				{
					this.mMomentum = Vector3.zero;
					this.mScroll = 0f;
				}
			}
			if (this.onMomentumMove != null)
			{
				this.onMomentumMove();
			}
		}
		else
		{
			this.mScroll = 0f;
			this.mMomentum = Vector3.zero;
			SpringPanel component = base.GetComponent<SpringPanel>();
			if (component != null && component.enabled)
			{
				return;
			}
			this.mShouldMove = false;
			if (this.onStoppedMoving != null)
			{
				this.onStoppedMoving();
			}
		}
	}

	public void MoveAbsolute(Vector3 absolute)
	{
		Vector3 vector3 = this.mTrans.InverseTransformPoint(absolute);
		Vector3 vector31 = this.mTrans.InverseTransformPoint(Vector3.zero);
		this.MoveRelative(vector3 - vector31);
	}

	public virtual void MoveRelative(Vector3 relative)
	{
		Transform transforms = this.mTrans;
		transforms.localPosition = transforms.localPosition + relative;
		Vector2 vector2 = this.mPanel.clipOffset;
		vector2.x -= relative.x;
		vector2.y -= relative.y;
		this.mPanel.clipOffset = vector2;
		this.UpdateScrollbars(false);
	}

	private void OnDisable()
	{
		UIScrollView.list.Remove(this);
	}

	private void OnEnable()
	{
		UIScrollView.list.Add(this);
		if (this.mStarted && Application.isPlaying)
		{
			this.CheckScrollbars();
		}
	}

	public void OnPan(Vector2 delta)
	{
		if (this.horizontalScrollBar != null)
		{
			this.horizontalScrollBar.OnPan(delta);
		}
		if (this.verticalScrollBar != null)
		{
			this.verticalScrollBar.OnPan(delta);
		}
		if (this.horizontalScrollBar == null && this.verticalScrollBar == null)
		{
			if (this.scale.x != 0f)
			{
				this.Scroll(delta.x);
			}
			else if (this.scale.y != 0f)
			{
				this.Scroll(delta.y);
			}
		}
	}

	public void OnScrollBar()
	{
		if (!this.mIgnoreCallbacks)
		{
			this.mIgnoreCallbacks = true;
			float single = (this.horizontalScrollBar == null ? 0f : this.horizontalScrollBar.@value);
			this.SetDragAmount(single, (this.verticalScrollBar == null ? 0f : this.verticalScrollBar.@value), false);
			this.mIgnoreCallbacks = false;
		}
	}

	public void Press(bool pressed)
	{
		if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
		{
			return;
		}
		if (this.smoothDragStart && pressed)
		{
			this.mDragStarted = false;
			this.mDragStartOffset = Vector2.zero;
		}
		if (base.enabled && NGUITools.GetActive(base.gameObject))
		{
			if (!pressed && this.mDragID == UICamera.currentTouchID)
			{
				this.mDragID = -10;
			}
			this.mCalculatedBounds = false;
			this.mShouldMove = this.shouldMove;
			if (!this.mShouldMove)
			{
				return;
			}
			this.mPressed = pressed;
			if (pressed)
			{
				this.mMomentum = Vector3.zero;
				this.mScroll = 0f;
				this.DisableSpring();
				this.mLastPos = UICamera.lastWorldPosition;
				this.mPlane = new Plane(this.mTrans.rotation * Vector3.back, this.mLastPos);
				Vector2 vector2 = this.mPanel.clipOffset;
				vector2.x = Mathf.Round(vector2.x);
				vector2.y = Mathf.Round(vector2.y);
				this.mPanel.clipOffset = vector2;
				Vector3 vector3 = this.mTrans.localPosition;
				vector3.x = Mathf.Round(vector3.x);
				vector3.y = Mathf.Round(vector3.y);
				this.mTrans.localPosition = vector3;
				if (!this.smoothDragStart)
				{
					this.mDragStarted = true;
					this.mDragStartOffset = Vector2.zero;
					if (this.onDragStarted != null)
					{
						this.onDragStarted();
					}
				}
			}
			else if (!this.centerOnChild)
			{
				if (this.restrictWithinPanel && this.mPanel.clipping != UIDrawCall.Clipping.None)
				{
					this.RestrictWithinBounds(this.dragEffect == UIScrollView.DragEffect.None, this.canMoveHorizontally, this.canMoveVertically);
				}
				if (this.mDragStarted && this.onDragFinished != null)
				{
					this.onDragFinished();
				}
				if (!this.mShouldMove && this.onStoppedMoving != null)
				{
					this.onStoppedMoving();
				}
			}
			else
			{
				this.centerOnChild.Recenter();
			}
		}
	}

	[ContextMenu("Reset Clipping Position")]
	public void ResetPosition()
	{
		if (NGUITools.GetActive(this))
		{
			this.mCalculatedBounds = false;
			Vector2 pivotOffset = NGUIMath.GetPivotOffset(this.contentPivot);
			this.SetDragAmount(pivotOffset.x, 1f - pivotOffset.y, false);
			this.SetDragAmount(pivotOffset.x, 1f - pivotOffset.y, true);
		}
	}

	public bool RestrictWithinBounds(bool instant)
	{
		return this.RestrictWithinBounds(instant, true, true);
	}

	public bool RestrictWithinBounds(bool instant, bool horizontal, bool vertical)
	{
		if (this.mPanel == null)
		{
			return false;
		}
		Bounds bound = this.bounds;
		Vector3 vector3 = this.mPanel.CalculateConstrainOffset(bound.min, bound.max);
		if (!horizontal)
		{
			vector3.x = 0f;
		}
		if (!vertical)
		{
			vector3.y = 0f;
		}
		if (vector3.sqrMagnitude <= 0.1f)
		{
			return false;
		}
		if (instant || this.dragEffect != UIScrollView.DragEffect.MomentumAndSpring)
		{
			this.MoveRelative(vector3);
			if (Mathf.Abs(vector3.x) > 0.01f)
			{
				this.mMomentum.x = 0f;
			}
			if (Mathf.Abs(vector3.y) > 0.01f)
			{
				this.mMomentum.y = 0f;
			}
			if (Mathf.Abs(vector3.z) > 0.01f)
			{
				this.mMomentum.z = 0f;
			}
			this.mScroll = 0f;
		}
		else
		{
			Vector3 vector31 = this.mTrans.localPosition + vector3;
			vector31.x = Mathf.Round(vector31.x);
			vector31.y = Mathf.Round(vector31.y);
			SpringPanel.Begin(this.mPanel.gameObject, vector31, 13f).strength = 8f;
		}
		return true;
	}

	public void Scroll(float delta)
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject) && this.scrollWheelFactor != 0f)
		{
			this.DisableSpring();
			this.mShouldMove |= this.shouldMove;
			if (Mathf.Sign(this.mScroll) != Mathf.Sign(delta))
			{
				this.mScroll = 0f;
			}
			UIScrollView uIScrollView = this;
			uIScrollView.mScroll = uIScrollView.mScroll + delta * this.scrollWheelFactor;
		}
	}

	public virtual void SetDragAmount(float x, float y, bool updateScrollbars)
	{
		if (this.mPanel == null)
		{
			this.mPanel = base.GetComponent<UIPanel>();
		}
		this.DisableSpring();
		Bounds bound = this.bounds;
		if (bound.min.x == bound.max.x || bound.min.y == bound.max.y)
		{
			return;
		}
		Vector4 vector4 = this.mPanel.finalClipRegion;
		float single = vector4.z * 0.5f;
		float single1 = vector4.w * 0.5f;
		float single2 = bound.min.x + single;
		float single3 = bound.max.x - single;
		float single4 = bound.min.y + single1;
		float single5 = bound.max.y - single1;
		if (this.mPanel.clipping == UIDrawCall.Clipping.SoftClip)
		{
			single2 -= this.mPanel.clipSoftness.x;
			single3 += this.mPanel.clipSoftness.x;
			single4 -= this.mPanel.clipSoftness.y;
			single5 += this.mPanel.clipSoftness.y;
		}
		float single6 = Mathf.Lerp(single2, single3, x);
		float single7 = Mathf.Lerp(single5, single4, y);
		if (!updateScrollbars)
		{
			Vector3 vector3 = this.mTrans.localPosition;
			if (this.canMoveHorizontally)
			{
				vector3.x = vector3.x + (vector4.x - single6);
			}
			if (this.canMoveVertically)
			{
				vector3.y = vector3.y + (vector4.y - single7);
			}
			this.mTrans.localPosition = vector3;
		}
		if (this.canMoveHorizontally)
		{
			vector4.x = single6;
		}
		if (this.canMoveVertically)
		{
			vector4.y = single7;
		}
		Vector4 vector41 = this.mPanel.baseClipRegion;
		this.mPanel.clipOffset = new Vector2(vector4.x - vector41.x, vector4.y - vector41.y);
		if (updateScrollbars)
		{
			this.UpdateScrollbars(this.mDragID == -10);
		}
	}

	private void Start()
	{
		this.mStarted = true;
		if (Application.isPlaying)
		{
			this.CheckScrollbars();
		}
	}

	public void UpdatePosition()
	{
		if (!this.mIgnoreCallbacks && (this.horizontalScrollBar != null || this.verticalScrollBar != null))
		{
			this.mIgnoreCallbacks = true;
			this.mCalculatedBounds = false;
			Vector2 pivotOffset = NGUIMath.GetPivotOffset(this.contentPivot);
			float single = (this.horizontalScrollBar == null ? pivotOffset.x : this.horizontalScrollBar.@value);
			this.SetDragAmount(single, (this.verticalScrollBar == null ? 1f - pivotOffset.y : this.verticalScrollBar.@value), false);
			this.UpdateScrollbars(true);
			this.mIgnoreCallbacks = false;
		}
	}

	public void UpdateScrollbars()
	{
		this.UpdateScrollbars(true);
	}

	public virtual void UpdateScrollbars(bool recalculateBounds)
	{
		if (this.mPanel == null)
		{
			return;
		}
		if (this.horizontalScrollBar != null || this.verticalScrollBar != null)
		{
			if (recalculateBounds)
			{
				this.mCalculatedBounds = false;
				this.mShouldMove = this.shouldMove;
			}
			Bounds bound = this.bounds;
			Vector2 vector2 = bound.min;
			Vector2 vector21 = bound.max;
			if (this.horizontalScrollBar != null && vector21.x > vector2.x)
			{
				Vector4 vector4 = this.mPanel.finalClipRegion;
				int num = Mathf.RoundToInt(vector4.z);
				if ((num & 1) != 0)
				{
					num--;
				}
				float single = (float)num * 0.5f;
				single = Mathf.Round(single);
				if (this.mPanel.clipping == UIDrawCall.Clipping.SoftClip)
				{
					single -= this.mPanel.clipSoftness.x;
				}
				float single1 = vector21.x - vector2.x;
				float single2 = single * 2f;
				float single3 = vector2.x;
				float single4 = vector21.x;
				float single5 = vector4.x - single;
				float single6 = vector4.x + single;
				single3 = single5 - single3;
				single4 -= single6;
				this.UpdateScrollbars(this.horizontalScrollBar, single3, single4, single1, single2, false);
			}
			if (this.verticalScrollBar != null && vector21.y > vector2.y)
			{
				Vector4 vector41 = this.mPanel.finalClipRegion;
				int num1 = Mathf.RoundToInt(vector41.w);
				if ((num1 & 1) != 0)
				{
					num1--;
				}
				float single7 = (float)num1 * 0.5f;
				single7 = Mathf.Round(single7);
				if (this.mPanel.clipping == UIDrawCall.Clipping.SoftClip)
				{
					single7 -= this.mPanel.clipSoftness.y;
				}
				float single8 = vector21.y - vector2.y;
				float single9 = single7 * 2f;
				float single10 = vector2.y;
				float single11 = vector21.y;
				float single12 = vector41.y - single7;
				float single13 = vector41.y + single7;
				single10 = single12 - single10;
				single11 -= single13;
				this.UpdateScrollbars(this.verticalScrollBar, single10, single11, single8, single9, true);
			}
		}
		else if (recalculateBounds)
		{
			this.mCalculatedBounds = false;
		}
	}

	protected void UpdateScrollbars(UIProgressBar slider, float contentMin, float contentMax, float contentSize, float viewSize, bool inverted)
	{
		float single;
		float single1;
		float single2;
		if (slider == null)
		{
			return;
		}
		this.mIgnoreCallbacks = true;
		if (viewSize >= contentSize)
		{
			contentMin = Mathf.Clamp01(-contentMin / contentSize);
			contentMax = Mathf.Clamp01(-contentMax / contentSize);
			single = contentMin + contentMax;
			UIProgressBar uIProgressBar = slider;
			if (!inverted)
			{
				single1 = (single <= 0.001f ? 1f : contentMin / single);
			}
			else
			{
				single1 = (single <= 0.001f ? 0f : 1f - contentMin / single);
			}
			uIProgressBar.@value = single1;
			if (contentSize > 0f)
			{
				contentMin = Mathf.Clamp01(contentMin / contentSize);
				contentMax = Mathf.Clamp01(contentMax / contentSize);
				single = contentMin + contentMax;
			}
		}
		else
		{
			contentMin = Mathf.Clamp01(contentMin / contentSize);
			contentMax = Mathf.Clamp01(contentMax / contentSize);
			single = contentMin + contentMax;
			UIProgressBar uIProgressBar1 = slider;
			if (!inverted)
			{
				single2 = (single <= 0.001f ? 1f : contentMin / single);
			}
			else
			{
				single2 = (single <= 0.001f ? 0f : 1f - contentMin / single);
			}
			uIProgressBar1.@value = single2;
		}
		UIScrollBar uIScrollBar = slider as UIScrollBar;
		if (uIScrollBar != null)
		{
			uIScrollBar.barSize = 1f - single;
		}
		this.mIgnoreCallbacks = false;
	}

	public enum DragEffect
	{
		None,
		Momentum,
		MomentumAndSpring
	}

	public enum Movement
	{
		Horizontal,
		Vertical,
		Unrestricted,
		Custom
	}

	public delegate void OnDragNotification();

	public enum ShowCondition
	{
		Always,
		OnlyIfNeeded,
		WhenDragging
	}
}