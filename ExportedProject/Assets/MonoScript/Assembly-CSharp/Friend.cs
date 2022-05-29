using System;
using UnityEngine;

public sealed class Friend
{
	public string name;

	public string id;

	public bool isUser;

	public Texture2D avatar;

	public DateTime nextVisit;

	public UILabel timeLabel;

	public UITexture avatarTexture;

	public Friend(string friendName, string friendId, bool friendIsUser)
	{
		this.name = friendName;
		this.id = friendId;
		this.isUser = friendIsUser;
		this.avatar = null;
	}

	public void SetAvatar(Texture2D txt)
	{
		this.avatar = txt;
	}

	public void SetAvatarObj(UITexture aT)
	{
		this.avatarTexture = aT;
	}

	public void SetTimeLabel(UILabel tL)
	{
		this.timeLabel = tL;
	}

	public void SetTimeLastVisit(DateTime visitTime)
	{
		this.nextVisit = visitTime;
	}
}