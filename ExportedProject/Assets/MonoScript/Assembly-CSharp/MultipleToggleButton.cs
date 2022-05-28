using System;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;

public class MultipleToggleButton : MonoBehaviour
{
	public ToggleButton[] buttons;

	private int _selectedIndex;

	public int SelectedIndex
	{
		get
		{
			return _selectedIndex;
		}
		set
		{
			if (buttons == null || value == -1)
			{
				return;
			}
			_selectedIndex = value;
			for (int i = 0; i < buttons.Length; i++)
			{
				if (i != _selectedIndex)
				{
					buttons[i].IsChecked = false;
				}
			}
			EventHandler<MultipleToggleEventArgs> clicked = this.Clicked;
			if (clicked != null)
			{
				clicked(this, new MultipleToggleEventArgs
				{
					Num = _selectedIndex
				});
			}
		}
	}

	public event EventHandler<MultipleToggleEventArgs> Clicked;

	private void Start()
	{
		if (buttons != null)
		{
			for (int i = 0; i < buttons.Length; i++)
			{
				buttons[i].Clicked += _003CStart_003Em__1CD;
			}
		}
	}

	[CompilerGenerated]
	private void _003CStart_003Em__1CD(object sender, ToggleButtonEventArgs e)
	{
		if (e.IsChecked)
		{
			SelectedIndex = Array.IndexOf(buttons, sender as ToggleButton);
		}
	}
}
