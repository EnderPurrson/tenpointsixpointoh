using GooglePlayGames;
using GooglePlayGames.BasicApi.Multiplayer;
using GooglePlayGames.OurUtils;
using System;
using System.Runtime.CompilerServices;

namespace GooglePlayGames.BasicApi
{
	public struct PlayGamesClientConfiguration
	{
		public readonly static PlayGamesClientConfiguration DefaultConfiguration;

		private readonly bool mEnableSavedGames;

		private readonly bool mRequireGooglePlus;

		private readonly InvitationReceivedDelegate mInvitationDelegate;

		private readonly GooglePlayGames.BasicApi.Multiplayer.MatchDelegate mMatchDelegate;

		private readonly string mPermissionRationale;

		public bool EnableSavedGames
		{
			get
			{
				return this.mEnableSavedGames;
			}
		}

		public InvitationReceivedDelegate InvitationDelegate
		{
			get
			{
				return this.mInvitationDelegate;
			}
		}

		public GooglePlayGames.BasicApi.Multiplayer.MatchDelegate MatchDelegate
		{
			get
			{
				return this.mMatchDelegate;
			}
		}

		public string PermissionRationale
		{
			get
			{
				return this.mPermissionRationale;
			}
		}

		public bool RequireGooglePlus
		{
			get
			{
				return this.mRequireGooglePlus;
			}
		}

		static PlayGamesClientConfiguration()
		{
			PlayGamesClientConfiguration.DefaultConfiguration = (new PlayGamesClientConfiguration.Builder()).WithPermissionRationale("Select email address to send to this game or hit cancel to not share.").Build();
		}

		private PlayGamesClientConfiguration(PlayGamesClientConfiguration.Builder builder)
		{
			this.mEnableSavedGames = builder.HasEnableSaveGames();
			this.mInvitationDelegate = builder.GetInvitationDelegate();
			this.mMatchDelegate = builder.GetMatchDelegate();
			this.mPermissionRationale = builder.GetPermissionRationale();
			this.mRequireGooglePlus = builder.HasRequireGooglePlus();
		}

		public class Builder
		{
			private bool mEnableSaveGames;

			private bool mRequireGooglePlus;

			private InvitationReceivedDelegate mInvitationDelegate;

			private GooglePlayGames.BasicApi.Multiplayer.MatchDelegate mMatchDelegate;

			private string mRationale;

			public Builder()
			{
			}

			public PlayGamesClientConfiguration Build()
			{
				this.mRequireGooglePlus = GameInfo.RequireGooglePlus();
				return new PlayGamesClientConfiguration(this);
			}

			public PlayGamesClientConfiguration.Builder EnableSavedGames()
			{
				this.mEnableSaveGames = true;
				return this;
			}

			internal InvitationReceivedDelegate GetInvitationDelegate()
			{
				return this.mInvitationDelegate;
			}

			internal GooglePlayGames.BasicApi.Multiplayer.MatchDelegate GetMatchDelegate()
			{
				return this.mMatchDelegate;
			}

			internal string GetPermissionRationale()
			{
				return this.mRationale;
			}

			internal bool HasEnableSaveGames()
			{
				return this.mEnableSaveGames;
			}

			internal bool HasRequireGooglePlus()
			{
				return this.mRequireGooglePlus;
			}

			public PlayGamesClientConfiguration.Builder RequireGooglePlus()
			{
				this.mRequireGooglePlus = true;
				return this;
			}

			public PlayGamesClientConfiguration.Builder WithInvitationDelegate(InvitationReceivedDelegate invitationDelegate)
			{
				this.mInvitationDelegate = Misc.CheckNotNull<InvitationReceivedDelegate>(invitationDelegate);
				return this;
			}

			public PlayGamesClientConfiguration.Builder WithMatchDelegate(GooglePlayGames.BasicApi.Multiplayer.MatchDelegate matchDelegate)
			{
				this.mMatchDelegate = Misc.CheckNotNull<GooglePlayGames.BasicApi.Multiplayer.MatchDelegate>(matchDelegate);
				return this;
			}

			public PlayGamesClientConfiguration.Builder WithPermissionRationale(string rationale)
			{
				this.mRationale = rationale;
				return this;
			}
		}
	}
}