using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class CameraTouchControlScheme_LowPassFilter : CameraTouchControlScheme
{
	public float dragClampInterval = 1.5f;

	public float dragClamp = 1f;

	public float lerpCoeff = 0.25f;

	private Vector2? _accumulatedDrag;

	private Vector2? _unfilteredAccumulatedDrag;

	private bool limitDrag;

	private bool firstDrag;

	private Vector2? _azimuthTilt;

	public CameraTouchControlScheme_LowPassFilter()
	{
	}

	public override void ApplyDeltaTo(Vector2 deltaPosition, Transform yawTransform, Transform pitchTransform, float sensitivity, bool invert)
	{
		if (!this._accumulatedDrag.HasValue)
		{
			this._azimuthTilt = null;
		}
		else if (!this._azimuthTilt.HasValue)
		{
			float single = yawTransform.rotation.eulerAngles.y;
			Vector3 vector3 = pitchTransform.localEulerAngles;
			this._azimuthTilt = new Vector2?(new Vector2(single, vector3.x));
		}
		else
		{
			Vector2 value = this._accumulatedDrag.Value;
			float single1 = sensitivity / 30f;
			Vector2 vector2 = this._azimuthTilt.Value;
			yawTransform.rotation = Quaternion.Euler(0f, vector2.x + value.x * single1, 0f);
			float value1 = this._azimuthTilt.Value.y;
			if (value1 > 180f)
			{
				value1 -= 360f;
			}
			float single2 = value1 + value.y * (!invert ? -1f : 1f) * single1;
			if (single2 > 80f)
			{
				single2 = 80f;
			}
			if (single2 < -65f)
			{
				single2 = -65f;
			}
			pitchTransform.localRotation = Quaternion.Euler(single2, 0f, 0f);
		}
	}

	[DebuggerHidden]
	[Obfuscation(Exclude=true)]
	private IEnumerator CancelLimitDrag()
	{
		CameraTouchControlScheme_LowPassFilter.u003cCancelLimitDragu003ec__Iterator1B4 variable = null;
		return variable;
	}

	public override void OnDrag(Vector2 delta)
	{
		if (!this.firstDrag)
		{
			this._deltaPosition = delta;
		}
		this.firstDrag = false;
		if (this.limitDrag)
		{
			this.limitDrag = false;
			if (JoystickController.rightJoystick)
			{
				JoystickController.rightJoystick.StopCoroutine(this.CancelLimitDrag());
			}
			this._deltaPosition = Vector2.ClampMagnitude(delta, this.dragClamp);
		}
		if (this._accumulatedDrag.HasValue && this._unfilteredAccumulatedDrag.HasValue)
		{
			Vector2 vector2 = this._deltaPosition;
			Vector2 value = this._unfilteredAccumulatedDrag.Value + vector2;
			Vector2 value1 = this._accumulatedDrag.Value;
			Vector2 vector21 = Vector2.Lerp(value1, value, this.lerpCoeff);
			this._accumulatedDrag = new Vector2?(vector21);
			this._unfilteredAccumulatedDrag = new Vector2?(value);
		}
		WeaponManager.sharedManager.myPlayerMoveC.mySkinName.MoveCamera(this._deltaPosition);
		this.Reset();
	}

	public override void OnPress(bool isDown)
	{
		if (!isDown)
		{
			this._accumulatedDrag = null;
			this._unfilteredAccumulatedDrag = null;
		}
		else
		{
			this._accumulatedDrag = new Vector2?(Vector2.zero);
			this._unfilteredAccumulatedDrag = new Vector2?(Vector2.zero);
		}
		this.firstDrag = isDown;
		this.limitDrag = isDown;
		if (isDown)
		{
			if (JoystickController.rightJoystick)
			{
				JoystickController.rightJoystick.StartCoroutine(this.CancelLimitDrag());
			}
		}
		else if (JoystickController.rightJoystick)
		{
			JoystickController.rightJoystick.StopCoroutine(this.CancelLimitDrag());
		}
	}

	public override void Reset()
	{
		this._deltaPosition = Vector2.zero;
		this._accumulatedDrag = null;
		this._unfilteredAccumulatedDrag = null;
	}
}