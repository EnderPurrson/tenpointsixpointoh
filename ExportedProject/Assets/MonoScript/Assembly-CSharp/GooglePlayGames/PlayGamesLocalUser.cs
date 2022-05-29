using GooglePlayGames.BasicApi;
using System;
using System.Runtime.CompilerServices;
using UnityEngine.SocialPlatforms;

namespace GooglePlayGames
{
	public class PlayGamesLocalUser : PlayGamesUserProfile, IUserProfile, ILocalUser
	{
		internal PlayGamesPlatform mPlatform;

		private string emailAddress;

		private PlayerStats mStats;

		[Obsolete("Use PlayGamesPlatform.GetServerAuthCode()")]
		public string accessToken
		{
			get
			{
				return (!this.authenticated ? string.Empty : this.mPlatform.GetAccessToken());
			}
		}

		public bool authenticated
		{
			get
			{
				return this.mPlatform.IsAuthenticated();
			}
		}

		public new string AvatarURL
		{
			get
			{
				string empty = string.Empty;
				if (this.authenticated)
				{
					empty = this.mPlatform.GetUserImageUrl();
					if (!base.id.Equals(empty))
					{
						base.ResetIdentity(this.mPlatform.GetUserDisplayName(), this.mPlatform.GetUserId(), empty);
					}
				}
				return empty;
			}
		}

		public string Email
		{
			get
			{
				if (this.authenticated && string.IsNullOrEmpty(this.emailAddress))
				{
					this.emailAddress = this.mPlatform.GetUserEmail();
					this.emailAddress = this.emailAddress ?? string.Empty;
				}
				return (!this.authenticated ? string.Empty : this.emailAddress);
			}
		}

		public IUserProfile[] friends
		{
			get
			{
				return this.mPlatform.GetFriends();
			}
		}

		public new string id
		{
			get
			{
				string empty = string.Empty;
				if (this.authenticated)
				{
					empty = this.mPlatform.GetUserId();
					if (!base.id.Equals(empty))
					{
						base.ResetIdentity(this.mPlatform.GetUserDisplayName(), empty, this.mPlatform.GetUserImageUrl());
					}
				}
				return empty;
			}
		}

		public new bool isFriend
		{
			get
			{
				return true;
			}
		}

		public new UserState state
		{
			get
			{
				return UserState.Online;
			}
		}

		public bool underage
		{
			get
			{
				return true;
			}
		}

		public new string userName
		{
			get
			{
				string empty = string.Empty;
				if (this.authenticated)
				{
					empty = this.mPlatform.GetUserDisplayName();
					if (!base.userName.Equals(empty))
					{
						base.ResetIdentity(empty, this.mPlatform.GetUserId(), this.mPlatform.GetUserImageUrl());
					}
				}
				return empty;
			}
		}

		internal PlayGamesLocalUser(PlayGamesPlatform plaf) : base("localUser", string.Empty, string.Empty)
		{
			this.mPlatform = plaf;
			this.emailAddress = null;
			this.mStats = null;
		}

		public void Authenticate(Action<bool> callback)
		{
			this.mPlatform.Authenticate(callback);
		}

		public void Authenticate(Action<bool> callback, bool silent)
		{
			this.mPlatform.Authenticate(callback, silent);
		}

		[Obsolete("Use PlayGamesPlatform.GetServerAuthCode()")]
		public void GetIdToken(Action<string> idTokenCallback)
		{
			if (!this.authenticated)
			{
				idTokenCallback(null);
			}
			else
			{
				this.mPlatform.GetIdToken(idTokenCallback);
			}
		}

		public void GetStats(Action<CommonStatusCodes, PlayerStats> callback)
		{
			if (this.mStats == null || !this.mStats.Valid)
			{
				this.mPlatform.GetPlayerStats((CommonStatusCodes rc, PlayerStats stats) => {
					this.mStats = stats;
					callback(rc, stats);
				});
			}
			else
			{
				callback(0, this.mStats);
			}
		}

		public void LoadFriends(Action<bool> callback)
		{
			this.mPlatform.LoadFriends(this, callback);
		}
	}
}