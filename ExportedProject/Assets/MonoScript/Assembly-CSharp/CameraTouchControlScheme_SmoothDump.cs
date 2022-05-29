using System;
using UnityEngine;

public sealed class CameraTouchControlScheme_SmoothDump : CameraTouchControlScheme
{
	public float startMovingThresholdSq = 4f;

	public float senseModifier = 0.03f;

	public Vector2 senseModifierByAxis = new Vector2(1f, 0.8f);

	public float dampingTime = 0.05f;

	private bool _grabTouches;

	private int _touchId;

	private Vector2 _firstTouchPosition;

	private Vector2 _previousTouchPosition;

	private Vector2 _currentTouchPosition;

	private bool _isTouchInputValid;

	private bool _isTouchMoving;

	private Quaternion _originalRotationPitch;

	private Quaternion _originalRotationYaw;

	private Vector2? _followPitchYaw;

	private Vector2 _followPitchYawVelocity;

	private Vector2? _targetPitchYaw;

	public CameraTouchControlScheme_SmoothDump()
	{
	}

	public override void ApplyDeltaTo(Vector2 deltaPosition, Transform yawTransform, Transform pitchTransform, float sensitivity, bool invert)
	{
		if (!this._isTouchInputValid)
		{
			this._followPitchYaw = this._targetPitchYaw;
			this._followPitchYawVelocity = Vector2.zero;
			this._targetPitchYaw = null;
		}
		else
		{
			if (!this._followPitchYaw.HasValue)
			{
				this._originalRotationPitch = pitchTransform.localRotation;
				this._originalRotationYaw = yawTransform.rotation;
				this._followPitchYaw = new Vector2?(Vector2.zero);
				this._targetPitchYaw = new Vector2?(Vector2.zero);
			}
			Vector2 value = this._followPitchYaw.Value;
			Vector2 vector2 = this._targetPitchYaw.Value;
			if (vector2.x > 180f)
			{
				vector2.x -= 360f;
				value.x -= 360f;
			}
			if (vector2.y > 180f)
			{
				vector2.y -= 360f;
				value.y -= 360f;
			}
			if (vector2.x < -180f)
			{
				vector2.x += 360f;
				value.x += 360f;
			}
			if (vector2.y < -180f)
			{
				vector2.y += 360f;
				value.y += 360f;
			}
			vector2.x = vector2.x + deltaPosition.y * sensitivity * this.senseModifier * this.senseModifierByAxis.y;
			vector2.y = vector2.y + deltaPosition.x * sensitivity * this.senseModifier * this.senseModifierByAxis.x;
			vector2.x = Mathf.Clamp(vector2.x, -65f, 80f);
			value.x = Mathf.SmoothDamp(value.x, vector2.x, ref this._followPitchYawVelocity.x, this.dampingTime);
			value.y = Mathf.SmoothDamp(value.y, vector2.y, ref this._followPitchYawVelocity.y, this.dampingTime);
			this._followPitchYaw = new Vector2?(value);
			this._targetPitchYaw = new Vector2?(vector2);
		}
		if (this._followPitchYaw.HasValue)
		{
			Quaternion quaternion = this._originalRotationYaw;
			Vector2 value1 = this._followPitchYaw.Value;
			yawTransform.rotation = quaternion * Quaternion.Euler(0f, value1.y, 0f);
			Transform transforms = pitchTransform;
			Quaternion quaternion1 = this._originalRotationPitch;
			Vector2 vector21 = this._followPitchYaw.Value;
			transforms.localRotation = quaternion1 * Quaternion.Euler(vector21.x * (!invert ? -1f : 1f), 0f, 0f);
		}
		if (!this._isTouchInputValid)
		{
			this._followPitchYaw = null;
		}
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