using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class CameraTouchControlScheme_UFPS : CameraTouchControlScheme
{
	public float startMovingThresholdSq = 4f;

	public Vector2 mouseLookSensitivity = new Vector2(2.8f, 1.876f);

	public int mouseLookSmoothSteps = 10;

	public float mouseLookSmoothWeight = 0.15f;

	public bool mouseLookAcceleration;

	public float mouseLookAccelerationThreshold = 0.4f;

	private bool _grabTouches;

	private int _touchId;

	private Vector2 _firstTouchPosition;

	private Vector2 _previousTouchPosition;

	private Vector2 _currentTouchPosition;

	private bool _isTouchInputValid;

	private bool _isTouchMoving;

	private Quaternion _originalRotationPitch;

	private Quaternion _originalRotationYaw;

	private Vector2? _pitchYaw;

	private Vector2 m_MouseLookSmoothMove = Vector2.zero;

	private List<Vector2> m_MouseLookSmoothBuffer = new List<Vector2>();

	private int m_LastMouseLookFrame = -1;

	private Vector2 m_CurrentMouseLook = Vector2.zero;

	private float Delta
	{
		get
		{
			return Time.deltaTime * 30f;
		}
	}

	public CameraTouchControlScheme_UFPS()
	{
	}

	public override void ApplyDeltaTo(Vector2 deltaPosition, Transform yawTransform, Transform pitchTransform, float sensitivity, bool invert)
	{
		deltaPosition = deltaPosition * (sensitivity * 0.01f);
		deltaPosition = new Vector2(deltaPosition.x, deltaPosition.y);
		Vector2 mouseLook = this.GetMouseLook(deltaPosition);
		if (!this._isTouchInputValid)
		{
			this._pitchYaw = null;
		}
		else
		{
			if (!this._pitchYaw.HasValue)
			{
				this._originalRotationPitch = pitchTransform.localRotation;
				this._originalRotationYaw = yawTransform.rotation;
				this._pitchYaw = new Vector2?(Vector2.zero);
			}
			Vector2 value = this._pitchYaw.Value;
			value.x += mouseLook.y;
			value.y += mouseLook.x;
			if (value.x > 180f)
			{
				value.x -= 360f;
			}
			if (value.y > 180f)
			{
				value.y -= 360f;
			}
			if (value.x < -180f)
			{
				value.x += 360f;
			}
			if (value.y < -180f)
			{
				value.y += 360f;
			}
			value.x = Mathf.Clamp(value.x, -89.5f, 89.5f);
			value.y = Mathf.Clamp(value.y, -360f, 360f);
			this._pitchYaw = new Vector2?(value);
			yawTransform.rotation = this._originalRotationYaw;
			pitchTransform.localRotation = this._originalRotationPitch;
			Quaternion quaternion = this._originalRotationYaw;
			Vector2 vector2 = this._pitchYaw.Value;
			yawTransform.rotation = quaternion * Quaternion.Euler(0f, vector2.y, 0f);
			Transform transforms = pitchTransform;
			Quaternion quaternion1 = this._originalRotationPitch;
			Vector2 value1 = this._pitchYaw.Value;
			transforms.localRotation = quaternion1 * Quaternion.Euler(value1.x * (!invert ? -1f : 1f), 0f, 0f);
		}
	}

	public Vector2 GetMouseLook(Vector2 touchDeltaPosition)
	{
		if (this.m_LastMouseLookFrame == Time.frameCount)
		{
			return this.m_CurrentMouseLook;
		}
		this.m_LastMouseLookFrame = Time.frameCount;
		this.m_MouseLookSmoothMove.x = touchDeltaPosition.x * Time.timeScale;
		this.m_MouseLookSmoothMove.y = touchDeltaPosition.y * Time.timeScale;
		this.mouseLookSmoothSteps = Mathf.Clamp(this.mouseLookSmoothSteps, 1, 20);
		this.mouseLookSmoothWeight = Mathf.Clamp01(this.mouseLookSmoothWeight);
		while (this.m_MouseLookSmoothBuffer.Count > this.mouseLookSmoothSteps)
		{
			this.m_MouseLookSmoothBuffer.RemoveAt(0);
		}
		this.m_MouseLookSmoothBuffer.Add(this.m_MouseLookSmoothMove);
		float delta = 1f;
		Vector2 item = Vector2.zero;
		float single = 0f;
		for (int i = this.m_MouseLookSmoothBuffer.Count - 1; i > 0; i--)
		{
			item = item + (this.m_MouseLookSmoothBuffer[i] * delta);
			single = single + 1f * delta;
			delta = delta * (this.mouseLookSmoothWeight / this.Delta);
		}
		single = Mathf.Max(1f, single);
		this.m_CurrentMouseLook = CameraTouchControlScheme_UFPS.NaNSafeVector2(item / single, new Vector2());
		float single1 = 0f;
		float single2 = Mathf.Abs(this.m_CurrentMouseLook.x);
		float single3 = Mathf.Abs(this.m_CurrentMouseLook.y);
		if (this.mouseLookAcceleration)
		{
			single1 = Mathf.Sqrt(single2 * single2 + single3 * single3) / this.Delta;
			single1 = (single1 > this.mouseLookAccelerationThreshold ? single1 : 0f);
		}
		ref Vector2 mCurrentMouseLook = ref this.m_CurrentMouseLook;
		mCurrentMouseLook.x = mCurrentMouseLook.x * (this.mouseLookSensitivity.x + single1);
		ref Vector2 vector2Pointer = ref this.m_CurrentMouseLook;
		vector2Pointer.y = vector2Pointer.y * (this.mouseLookSensitivity.y + single1);
		return this.m_CurrentMouseLook;
	}

	private static Vector2 NaNSafeVector2(Vector2 vector, Vector2 prevVector = default(Vector2))
	{
		vector.x = (!double.IsNaN((double)vector.x) ? vector.x : prevVector.x);
		vector.y = (!double.IsNaN((double)vector.y) ? vector.y : prevVector.y);
		return vector;
	}

	public override void OnPress(bool isDown)
	{
		if (isDown && this._touchId == -100 || !isDown && this._touchId != -100)
		{
			this._grabTouches = isDown;
			this._touchId = (!isDown ? -100 : UICamera.currentTouchID);
			this._firstTouchPosition = UICamera.currentTouch.pos;
			this._previousTouchPosition = this._firstTouchPosition;
			this._currentTouchPosition = this._firstTouchPosition;
			this._isTouchMoving = false;
		}
	}

	public override void OnUpdate()
	{
		this._isTouchInputValid = false;
		Touch? nullable = null;
		if (this._grabTouches)
		{
			Touch[] touchArray = Input.touches;
			int num = 0;
			while (num < (int)touchArray.Length)
			{
				Touch touch = touchArray[num];
				if (touch.fingerId != this._touchId || touch.phase != TouchPhase.Moved && touch.phase != TouchPhase.Stationary)
				{
					num++;
				}
				else
				{
					this._isTouchInputValid = true;
					this._previousTouchPosition = this._currentTouchPosition;
					this._currentTouchPosition = touch.position;
					nullable = new Touch?(touch);
					break;
				}
			}
		}
		this._deltaPosition = Vector2.zero;
		if (this._isTouchInputValid)
		{
			if (this._isTouchMoving || (this._currentTouchPosition - this._firstTouchPosition).sqrMagnitude >= this.startMovingThresholdSq)
			{
				if (this._isTouchMoving)
				{
					this._deltaPosition = this._currentTouchPosition - this._previousTouchPosition;
				}
				else
				{
					this._isTouchMoving = true;
				}
			}
		}
	}

	public override void Reset()
	{
		this._deltaPosition = Vector2.zero;
		this._grabTouches = false;
		this._touchId = -100;
		this._isTouchInputValid = false;
		this._isTouchMoving = false;
	}
}