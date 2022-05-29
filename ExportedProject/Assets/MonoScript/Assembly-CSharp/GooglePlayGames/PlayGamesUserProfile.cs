using GooglePlayGames.OurUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace GooglePlayGames
{
	public class PlayGamesUserProfile : IUserProfile
	{
		private string mDisplayName;

		private string mPlayerId;

		private string mAvatarUrl;

		private volatile bool mImageLoading;

		private Texture2D mImage;

		public string AvatarURL
		{
			get
			{
				return this.mAvatarUrl;
			}
		}

		public string id
		{
			get
			{
				return this.mPlayerId;
			}
		}

		public Texture2D image
		{
			get
			{
				if (!this.mImageLoading && this.mImage == null && !string.IsNullOrEmpty(this.AvatarURL))
				{
					UnityEngine.Debug.Log(string.Concat("Starting to load image: ", this.AvatarURL));
					this.mImageLoading = true;
					PlayGamesHelperObject.RunCoroutine(this.LoadImage());
				}
				return this.mImage;
			}
		}

		public bool isFriend
		{
			get
			{
				return true;
			}
		}

		public UserState state
		{
			get
			{
				return UserState.Online;
			}
		}

		public string userName
		{
			get
			{
				return this.mDisplayName;
			}
		}

		internal PlayGamesUserProfile(string displayName, string playerId, string avatarUrl)
		{
			this.mDisplayName = displayName;
			this.mPlayerId = playerId;
			this.mAvatarUrl = avatarUrl;
			this.mImageLoading = false;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (object.ReferenceEquals(this, obj))
			{
				return true;
			}
			if (!(obj is PlayGamesUserProfile))
			{
				return false;
			}
			return this.mPlayerId.Equals(((PlayGamesUserProfile)obj).mPlayerId);
		}

		public override int GetHashCode()
		{
			return typeof(PlayGamesUserProfile).GetHashCode() ^ this.mPlayerId.GetHashCode();
		}

		[DebuggerHidden]
		internal IEnumerator LoadImage()
		{
			PlayGamesUserProfile.u003cLoadImageu003ec__Iterator7C variable = null;
			return variable;
		}

		protected void ResetIdentity(string displayName, string playerId, string avatarUrl)
		{
			this.mDisplayName = displayName;
			this.mPlayerId = playerId;
			this.mAvatarUrl = avatarUrl;
			this.mImageLoading = false;
		}

		public override string ToString()
		{
			return string.Format("[Player: '{0}' (id {1})]", this.mDisplayName, this.mPlayerId);
		}
	}
}