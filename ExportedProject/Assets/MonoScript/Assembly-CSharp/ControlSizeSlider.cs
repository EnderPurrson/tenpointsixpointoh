using System;
using System.Runtime.CompilerServices;
using UnityEngine;

internal sealed class ControlSizeSlider : MonoBehaviour
{
	public UISlider slider;

	private EventHandler<ControlSizeSlider.EnabledChangedEventArgs> EnabledChanged;

	public ControlSizeSlider()
	{
	}

	private void OnDisable()
	{
		EventHandler<ControlSizeSlider.EnabledChangedEventArgs> enabledChanged = this.EnabledChanged;
		if (enabledChanged != null)
		{
			enabledChanged(this.slider, new ControlSizeSlider.EnabledChangedEventArgs()
			{
				Enabled = false
			});
		}
	}

	private void OnEnable()
	{
		EventHandler<ControlSizeSlider.EnabledChangedEventArgs> enabledChanged = this.EnabledChanged;
		if (enabledChanged != null)
		{
			enabledChanged(this.slider, new ControlSizeSlider.EnabledChangedEventArgs()
			{
				Enabled = true
			});
		}
	}

	public event EventHandler<ControlSizeSlider.EnabledChangedEventArgs> EnabledChanged
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.EnabledChanged += value;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.EnabledChanged -= value;
		}
	}

	public class EnabledChangedEventArgs : EventArgs
	{
		public bool Enabled
		{
			get;
			set;
		}

		public EnabledChangedEventArgs()
		{
		}
	}
}