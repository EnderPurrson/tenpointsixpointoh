using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class AGSSocialUser : IUserProfile
{
	private AGSPlayer player;

	public string id
	{
		get
		{
			return this.player.playerId;
		}
	}

	public Texture2D image
	{
		get
		{
			AGSClient.LogGameCircleError("ILocalUser.image.get is not available for GameCircle");
			return null;
		}
	}

	public bool isFriend
	{
		get
		{
			bool flag;
			List<AGSSocialUser>.Enumerator enumerator = AGSSocialLocalUser.friendList.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.id != this.id)
					{
						continue;
					}
					flag = true;
					return flag;
				}
				return false;
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
			return flag;
		}
	}

	public UserState state
	{
		get
		{
			AGSClient.LogGameCircleError("ILocalUser.state.get is not available for GameCircle");
			return UserState.Offline;
		}
	}

	public string userName
	{
		get
		{
			return this.player.@alias;
		}
	}

	public AGSSocialUser()
	{
		this.player = AGSPlayer.GetBlankPlayer();
	}

	public AGSSocialUser(AGSPlayer player)
	{
		this.player = (player != null ? player : AGSPlayer.GetBlankPlayer());
	}
}