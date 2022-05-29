using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class AGSSocialLocalUser : AGSSocialUser, ILocalUser, IUserProfile
{
	public static AGSPlayer player;

	public static List<AGSSocialUser> friendList;

	public bool authenticated
	{
		get
		{
			return AGSPlayerClient.IsSignedIn();
		}
	}

	public IUserProfile[] friends
	{
		get
		{
			return AGSSocialLocalUser.friendList.ToArray();
		}
	}

	public bool underage
	{
		get
		{
			AGSClient.LogGameCircleError("ILocalUser.underage.get is not available for GameCircle");
			return false;
		}
	}

	static AGSSocialLocalUser()
	{
		AGSSocialLocalUser.player = AGSPlayer.GetBlankPlayer();
		AGSSocialLocalUser.friendList = new List<AGSSocialUser>();
	}

	public AGSSocialLocalUser()
	{
	}

	public void Authenticate(Action<bool> callback)
	{
		GameCircleSocial.Instance.RequestLocalPlayer(callback);
		Social.Active.Authenticate(this, callback);
	}

	public void LoadFriends(Action<bool> callback)
	{
		GameCircleSocial.Instance.RequestFriends(callback);
	}
}