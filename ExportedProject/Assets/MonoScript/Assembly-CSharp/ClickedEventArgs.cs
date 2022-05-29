using System;

public sealed class ClickedEventArgs : EventArgs
{
	private readonly string _id;

	public string Id
	{
		get
		{
			return this._id;
		}
	}

	public ClickedEventArgs(string id)
	{
		this._id = id ?? string.Empty;
	}
}