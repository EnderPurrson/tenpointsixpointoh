using System;
using System.Runtime.CompilerServices;

public sealed class EditorClosingEventArgs : EventArgs
{
	public bool ClanLogoSaved
	{
		get;
		set;
	}

	public EditorClosingEventArgs()
	{
	}
}