using System;
using UnityEngine;

public class UIShowControlScheme : MonoBehaviour
{
	public GameObject target;

	public bool mouse;

	public bool touch;

	public bool controller = true;

	public UIShowControlScheme()
	{
	}

	private void OnDisable()
	{
		UICamera.onSchemeChange -= new UICamera.OnSchemeChange(this.OnScheme);
	}

	private void OnEnable()
	{
		UICamera.onSchemeChange += new UICamera.OnSchemeChange(this.OnScheme);
		this.OnScheme();
	}

	private void OnScheme()
	{
		if (this.target != null)
		{
			UICamera.ControlScheme controlScheme = UICamera.currentScheme;
			if (controlScheme == UICamera.ControlScheme.Mouse)
			{
				this.target.SetActive(this.mouse);
			}
			else if (controlScheme == UICamera.ControlScheme.Touch)
			{
				this.target.SetActive(this.touch);
			}
			else if (controlScheme == UICamera.ControlScheme.Controller)
			{
				this.target.SetActive(this.controller);
			}
		}
	}
}