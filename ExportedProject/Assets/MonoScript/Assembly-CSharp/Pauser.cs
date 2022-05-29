using System;
using UnityEngine;

internal sealed class Pauser : MonoBehaviour
{
	public static Pauser sharedPauser;

	private Action OnPlayerAddedAction;

	public bool pausedVar;

	public bool paused
	{
		get
		{
			return this.pausedVar;
		}
		set
		{
			this.pausedVar = value;
			if (JoystickController.leftJoystick == null || JoystickController.rightJoystick == null)
			{
				return;
			}
			if (!this.pausedVar)
			{
				JoystickController.leftJoystick.transform.parent.gameObject.SetActive(true);
				JoystickController.rightJoystick.gameObject.SetActive(true);
			}
			else
			{
				JoystickController.leftJoystick.transform.parent.gameObject.SetActive(false);
				JoystickController.rightJoystick.gameObject.SetActive(false);
			}
		}
	}

	static Pauser()
	{
	}

	public Pauser()
	{
	}

	private void OnDestroy()
	{
		Pauser.sharedPauser = null;
	}

	private void Start()
	{
		Pauser.sharedPauser = this;
	}
}