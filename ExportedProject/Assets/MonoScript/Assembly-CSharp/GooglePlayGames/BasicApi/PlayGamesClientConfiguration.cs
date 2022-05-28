using System.Runtime.CompilerServices;
using GooglePlayGames.BasicApi.Multiplayer;
using GooglePlayGames.OurUtils;

namespace GooglePlayGames.BasicApi
{
	public struct PlayGamesClientConfiguration
	{
		public class Builder
		{
			private bool mEnableSaveGames;

			private bool mRequireGooglePlus;

			private InvitationReceivedDelegate mInvitationDelegate;

			private MatchDelegate mMatchDelegate;

			private string mRationale;

			[CompilerGenerated]
			private static InvitationReceivedDelegate _003C_003Ef__am_0024cache5;

			[CompilerGenerated]
			private static MatchDelegate _003C_003Ef__am_0024cache6;

			public Builder()
			{
				if (_003C_003Ef__am_0024cache5 == null)
				{
					_003C_003Ef__am_0024cache5 = _003CmInvitationDelegate_003Em__78;
				}
				mInvitationDelegate = _003C_003Ef__am_0024cache5;
				if (_003C_003Ef__am_0024cache6 == null)
				{
					_003C_003Ef__am_0024cache6 = _003CmMatchDelegate_003Em__79;
				}
				mMatchDelegate = _003C_003Ef__am_0024cache6;
				base._002Ector();
			}

			public Builder EnableSavedGames()
			{
				mEnableSaveGames = true;
				return this;
			}

			public Builder RequireGooglePlus()
			{
				mRequireGooglePlus = true;
				return this;
			}

			public Builder WithInvitationDelegate(InvitationReceivedDelegate invitationDelegate)
			{
				mInvitationDelegate = Misc.CheckNotNull(invitationDelegate);
				return this;
			}

			public Builder WithMatchDelegate(MatchDelegate matchDelegate)
			{
				mMatchDelegate = Misc.CheckNotNull(matchDelegate);
				return this;
			}

			public Builder WithPermissionRationale(string rationale)
			{
				mRationale = rationale;
				return this;
			}

			public PlayGamesClientConfiguration Build()
			{
				mRequireGooglePlus = GameInfo.RequireGooglePlus();
				return new PlayGamesClientConfiguration(this);
			}

			internal bool HasEnableSaveGames()
			{
				return mEnableSaveGames;
			}

			internal bool HasRequireGooglePlus()
			{
				return mRequireGooglePlus;
			}

			internal MatchDelegate GetMatchDelegate()
			{
				return mMatchDelegate;
			}

			internal InvitationReceivedDelegate GetInvitationDelegate()
			{
				return mInvitationDelegate;
			}

			internal string GetPermissionRationale()
			{
				return mRationale;
			}

			[CompilerGenerated]
			private static void _003CmInvitationDelegate_003Em__78(Invitation P_0, bool P_1)
			{
			}

			[CompilerGenerated]
			private static void _003CmMatchDelegate_003Em__79(TurnBasedMatch P_0, bool P_1)
			{
			}
		}

		public static readonly PlayGamesClientConfiguration DefaultConfiguration = new Builder().WithPermissionRationale("Select email address to send to this game or hit cancel to not share.").Build();

		private readonly bool mEnableSavedGames;

		private readonly bool mRequireGooglePlus;

		private readonly InvitationReceivedDelegate mInvitationDelegate;

		private readonly MatchDelegate mMatchDelegate;

		private readonly string mPermissionRationale;

		public bool EnableSavedGames
		{
			get
			{
				return mEnableSavedGames;
			}
		}

		public bool RequireGooglePlus
		{
			get
			{
				return mRequireGooglePlus;
			}
		}

		public InvitationReceivedDelegate InvitationDelegate
		{
			get
			{
				return mInvitationDelegate;
			}
		}

		public MatchDelegate MatchDelegate
		{
			get
			{
				return mMatchDelegate;
			}
		}

		public string PermissionRationale
		{
			get
			{
				return mPermissionRationale;
			}
		}

		private PlayGamesClientConfiguration(Builder builder)
		{
			mEnableSavedGames = builder.HasEnableSaveGames();
			mInvitationDelegate = builder.GetInvitationDelegate();
			mMatchDelegate = builder.GetMatchDelegate();
			mPermissionRationale = builder.GetPermissionRationale();
			mRequireGooglePlus = builder.HasRequireGooglePlus();
		}
	}
}
