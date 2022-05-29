using System;
using UnityEngine;

public sealed class CameraTouchControlScheme_CleanNGUI : CameraTouchControlScheme
{
	public float firstDragClampedMax = 5f;

	private bool _limitDragDelta;

	public CameraTouchControlScheme_CleanNGUI()
	{
	}

	public override void ApplyDeltaTo(Vector2 deltaPosition, Transform yawTransform, Transform pitchTransform, float sensitivity, bool invert)
	{
		Vector2 vector2 = ((deltaPosition * sensitivity) * 30f) / (float)Screen.width;
		yawTransform.Rotate(0f, vector2.x, 0f, Space.World);
		pitchTransform.Rotate((!invert ? -1f : 1f) * vector2.y, 0f, 0f);
	}

	public override void OnDrag(Vector2 delta)
	{
		if (!this._limitDragDelta)
		{
			this._deltaPosition = delta;
		}
		else
		{
			this._limitDragDelta = false;
			this._deltaPosition = Vector2.ClampMagnitude(delta, this.firstDragClampedMax);
		}
		WeaponManager.sharedManager.myPlayerMoveC.mySkinName.MoveCamera(this._deltaPosition);
		this.Reset();
	}

	public override void OnPress(bool isDown)
	{
		this._limitDragDelta = isDown;
	}

	public override void Reset()
	{
		this._deltaPosition = Vector2.zero;
		this._limitDragDelta = false;
	}
}