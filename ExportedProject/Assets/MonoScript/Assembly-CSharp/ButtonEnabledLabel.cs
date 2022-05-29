using System;
using UnityEngine;

public class ButtonEnabledLabel : MonoBehaviour
{
	public UIButton myButton;

	public GameObject enabledLabel;

	public GameObject disableLabel;

	public ButtonEnabledLabel()
	{
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (this.myButton.isEnabled && !this.enabledLabel.activeSelf)
		{
			this.enabledLabel.SetActive(true);
			this.disableLabel.SetActive(false);
		}
		if (!this.myButton.isEnabled && this.enabledLabel.activeSelf)
		{
			this.enabledLabel.SetActive(false);
			this.disableLabel.SetActive(true);
		}
	}
}