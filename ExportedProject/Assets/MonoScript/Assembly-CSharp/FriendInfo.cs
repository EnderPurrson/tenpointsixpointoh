using System;
using System.Runtime.CompilerServices;

public class FriendInfo
{
	public bool IsInRoom
	{
		get
		{
			return (!this.IsOnline ? false : !string.IsNullOrEmpty(this.Room));
		}
	}

	public bool IsOnline
	{
		get;
		protected internal set;
	}

	public string Name
	{
		get;
		protected internal set;
	}

	public string Room
	{
		get;
		protected internal set;
	}

	public FriendInfo()
	{
	}

	public override string ToString()
	{
		object obj;
		string name = this.Name;
		if (this.IsOnline)
		{
			obj = (!this.IsInRoom ? "on master" : "playing");
		}
		else
		{
			obj = "offline";
		}
		return string.Format("{0}\t is: {1}", name, obj);
	}
}