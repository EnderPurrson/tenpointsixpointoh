using System;
using System.Runtime.CompilerServices;

namespace Rilisoft
{
	public sealed class MultipleToggleEventArgs : EventArgs
	{
		public int Num
		{
			get;
			set;
		}

		public MultipleToggleEventArgs()
		{
		}
	}
}