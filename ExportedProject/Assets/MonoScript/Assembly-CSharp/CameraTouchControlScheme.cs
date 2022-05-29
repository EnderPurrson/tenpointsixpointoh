using System;
using UnityEngine;

public abstract class CameraTouchControlScheme
{
	protected Vector2 _deltaPosition;

	public Vector2 DeltaPosition
	{
		get
		{
			return this._deltaPosition;
		}
	}

	protected CameraTouchControlScheme()
	{
	}

	public abstract void ApplyDeltaTo(Vector2 deltaPosition, Transform yawTransform, Transform pitchTransform, float sensitivity, bool invert);

	public virtual void OnDrag(Vector2 delta)
	{
	}

	public virtual void OnPress(bool isDown)
	{
	}

	public virtual void OnUpdate()
	{
	}

	public abstract void Reset();

	public void ResetDelta()
	{
		this._deltaPosition = Vector2.zero;
	}
}