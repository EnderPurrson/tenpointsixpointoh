using Rilisoft;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class SettingsToggleButtons : MonoBehaviour
{
	public UIButton offButton;

	public UIButton onButton;

	private bool _isChecked;

	private UIToggle _toggleVal;

	private EventHandler<ToggleButtonEventArgs> Clicked;

	private UIToggle _toggle
	{
		get
		{
			if (this._toggleVal == null)
			{
				this._toggleVal = base.gameObject.GetComponentInChildren<UIToggle>(true);
				if (this._toggleVal != null)
				{
					this._toggleVal.onChange.Add(new EventDelegate(new EventDelegate.Callback(this.OnValueChanged)));
				}
			}
			return this._toggleVal;
		}
	}

	public bool IsChecked
	{
		get
		{
			if (this._toggle == null)
			{
				return this._isChecked;
			}
			return this._toggle.@value;
		}
		set
		{
			if (this._toggle == null)
			{
				this._isChecked = value;
				this.offButton.isEnabled = this._isChecked;
				this.onButton.isEnabled = !this._isChecked;
				EventHandler<ToggleButtonEventArgs> clicked = this.Clicked;
				if (clicked != null)
				{
					clicked(this, new ToggleButtonEventArgs()
					{
						IsChecked = this._isChecked
					});
				}
			}
			else
			{
				this._toggle.@value = value;
			}
		}
	}

	public SettingsToggleButtons()
	{
	}

	private void OnValueChanged()
	{
		EventHandler<ToggleButtonEventArgs> clicked = this.Clicked;
		if (clicked != null)
		{
			ToggleButtonEventArgs toggleButtonEventArg = new ToggleButtonEventArgs()
			{
				IsChecked = this._toggle.@value
			};
			clicked(this, toggleButtonEventArg);
		}
	}

	private void Start()
	{
		if (this._toggle == null)
		{
			this.onButton.GetComponent<ButtonHandler>().Clicked += new EventHandler((object sender, EventArgs e) => this.IsChecked = true);
			this.offButton.GetComponent<ButtonHandler>().Clicked += new EventHandler((object sender, EventArgs e) => this.IsChecked = false);
		}
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