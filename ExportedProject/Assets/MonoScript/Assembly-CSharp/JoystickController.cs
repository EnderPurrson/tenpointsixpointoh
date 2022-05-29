using System;
using UnityEngine;

internal sealed class JoystickController : MonoBehaviour
{
	public static UIJoystick leftJoystick;

	public static TouchPadController rightJoystick;

	public static TouchPadInJoystick leftTouchPad;

	public UIJoystick _leftJoystick;

	public TouchPadController _rightJoystick;

	public TouchPadInJoystick _leftTouchPad;

	static JoystickController()
	{
	}

	public JoystickController()
	{
	}

	private void Awake()
	{
		JoystickController.leftJoystick = this._leftJoystick;
		JoystickController.rightJoystick = this._rightJoystick;
		JoystickController.leftTouchPad = this._leftTouchPad;
	}

	public static bool IsButtonFireUp()
	{
		return (JoystickController.leftTouchPad.isShooting ? false : !JoystickController.rightJoystick.isShooting);
	}

	private void OnDestroy()
	{
		JoystickController.leftJoystick = null;
		JoystickController.rightJoystick = null;
		JoystickController.leftTouchPad = null;
	}
}