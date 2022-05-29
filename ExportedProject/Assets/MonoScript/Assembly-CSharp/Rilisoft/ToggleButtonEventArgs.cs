using System;
using System.Runtime.CompilerServices;

namespace Rilisoft
{
	public sealed class ToggleButtonEventArgs : EventArgs
	{
		public bool IsChecked
		{
			get;
			set;
		}

		public ToggleButtonEventArgs()
		{
		}
	}
}