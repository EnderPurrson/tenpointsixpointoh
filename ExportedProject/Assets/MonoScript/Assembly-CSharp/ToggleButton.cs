using Rilisoft;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ToggleButton : MonoBehaviour
{
	public UIButton offButton;

	public UIButton onButton;

	public bool useForMultipleToggle = true;

	private bool _isChecked;

	private EventHandler<ToggleButtonEventArgs> Clicked;

	public bool IsChecked
	{
		get
		{
			return this._isChecked;
		}
		set
		{
			this.SetCheckedWithoutEvent(value);
			EventHandler<ToggleButtonEventArgs> clicked = this.Clicked;
			if (clicked != null)
			{
				clicked(this, new ToggleButtonEventArgs()
				{
					IsChecked = this._isChecked
				});
			}
		}
	}

	public ToggleButton()
	{
	}

	public void SetCheckedImage(bool c)
	{
		this.offButton.gameObject.SetActive(!c);
		this.onButton.gameObject.SetActive(c);
		if (this.useForMultipleToggle)
		{
			this.onButton.isEnabled = !this.onButton.gameObject.activeSelf;
		}
	}

	public void SetCheckedWithoutEvent(bool val)
	{
		this._isChecked = val;
		this.offButton.gameObject.SetActive(!this._isChecked);
		this.onButton.gameObject.SetActive(this._isChecked);
		if (this.useForMultipleToggle)
		{
			this.onButton.isEnabled = !this.onButton.gameObject.activeSelf;
		}
	}

	private void Start()
	{
		this.onButton.GetComponent<ButtonHandler>().Clicked += new EventHandler((object sender, EventArgs e) => this.IsChecked = false);
		this.offButton.GetComponent<ButtonHandler>().Clicked += new EventHandler((object sender, EventArgs e) => this.IsChecked = true);
	}

	public event EventHandler<ToggleButtonEventArgs> Clicked
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.Clicked += value;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.Clicked -= value;
		}
	}
}