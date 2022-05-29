using System;
using UnityEngine;

internal sealed class PressDetector : MonoBehaviour
{
	public static EventHandler<EventArgs> PressedDown;

	public PressDetector()
	{
	}

	private void OnPress(bool isDown)
	{
		EventHandler<EventArgs> pressedDown = PressDetector.PressedDown;
		if (pressedDown != null)
		{
			pressedDown(base.gameObject, EventArgs.Empty);
		}
	}
}