using System;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;

public class ToggleButton : MonoBehaviour
{
	public UIButton offButton;

	public UIButton onButton;

	public bool useForMultipleToggle = true;

	private bool _isChecked;

	public bool IsChecked
	{
		get
		{
			return _isChecked;
		}
		set
		{
			SetCheckedWithoutEvent(value);
			EventHandler<ToggleButtonEventArgs> clicked = this.Clicked;
			if (clicked != null)
			{
				clicked(this, new ToggleButtonEventArgs
				{
					IsChecked = _isChecked
				});
			}
		}
	}

	public event EventHandler<ToggleButtonEventArgs> Clicked;

	public void SetCheckedImage(bool c)
	{
		offButton.gameObject.SetActive(!c);
		onButton.gameObject.SetActive(c);
		if (useForMultipleToggle)
		{
			onButton.isEnabled = !onButton.gameObject.activeSelf;
		}
	}

	public void SetCheckedWithoutEvent(bool val)
	{
		_isChecked = val;
		offButton.gameObject.SetActive(!_isChecked);
		onButton.gameObject.SetActive(_isChecked);
		if (useForMultipleToggle)
		{
			onButton.isEnabled = !onButton.gameObject.activeSelf;
		}
	}

	private void Start()
	{
		onButton.GetComponent<ButtonHandler>().Clicked += _003CStart_003Em__58B;
		offButton.GetComponent<ButtonHandler>().Clicked += _003CStart_003Em__58C;
	}

	[CompilerGenerated]
	private void _003CStart_003Em__58B(object sender, EventArgs e)
	{
		IsChecked = false;
	}

	[CompilerGenerated]
	private void _003CStart_003Em__58C(object sender, EventArgs e)
	{
		IsChecked = true;
	}
}
