using Rilisoft;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MultipleToggleButton : MonoBehaviour
{
	public ToggleButton[] buttons;

	private int _selectedIndex;

	private EventHandler<MultipleToggleEventArgs> Clicked;

	public int SelectedIndex
	{
		get
		{
			return this._selectedIndex;
		}
		set
		{
			if (this.buttons == null || value == -1)
			{
				return;
			}
			this._selectedIndex = value;
			for (int i = 0; i < (int)this.buttons.Length; i++)
			{
				if (i != this._selectedIndex)
				{
					this.buttons[i].IsChecked = false;
				}
			}
			EventHandler<MultipleToggleEventArgs> clicked = this.Clicked;
			if (clicked != null)
			{
				clicked(this, new MultipleToggleEventArgs()
				{
					Num = this._selectedIndex
				});
			}
		}
	}

	public MultipleToggleButton()
	{
	}

	private void Start()
	{
		if (this.buttons != null)
		{
			for (int i = 0; i < (int)this.buttons.Length; i++)
			{
				this.buttons[i].Clicked += new EventHandler<ToggleButtonEventArgs>((object sender, ToggleButtonEventArgs e) => {
					if (e.IsChecked)
					{
						this.SelectedIndex = Array.IndexOf<ToggleButton>(this.buttons, sender as ToggleButton);
					}
				});
			}
		}
	}

	public event EventHandler<MultipleToggleEventArgs> Clicked
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