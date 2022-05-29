using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Events;
using GooglePlayGames.BasicApi.Multiplayer;
using GooglePlayGames.BasicApi.Quests;
using GooglePlayGames.BasicApi.SavedGame;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.Native.PInvoke;
using GooglePlayGames.OurUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace GooglePlayGames.Native
{
	public class NativeClient : IPlayGamesClient
	{
		private readonly IClientImpl clientImpl;

		private readonly object GameServicesLock = new object();

		private readonly object AuthStateLock = new object();

		private readonly PlayGamesClientConfiguration mConfiguration;

		private GooglePlayGames.Native.PInvoke.GameServices mServices;

		private volatile NativeTurnBasedMultiplayerClient mTurnBasedClient;

		private volatile NativeRealtimeMultiplayerClient mRealTimeClient;

		private volatile ISavedGameClient mSavedGameClient;

		private volatile IEventsClient mEventsClient;

		private volatile IQuestsClient mQuestsClient;

		private volatile TokenClient mTokenClient;

		private volatile Action<Invitation, bool> mInvitationDelegate;

		private volatile Dictionary<string, GooglePlayGames.BasicApi.Achievement> mAchievements;

		private volatile GooglePlayGames.BasicApi.Multiplayer.Player mUser;

		private volatile List<GooglePlayGames.BasicApi.Multiplayer.Player> mFriends;

		private volatile Action<bool> mPendingAuthCallbacks;

		private volatile Action<bool> mSilentAuthCallbacks;

		private volatile NativeClient.AuthState mAuthState;

		private volatile uint mAuthGeneration;

		private volatile bool mSilentAuthFailed;

		private volatile bool friendsLoading;

		private string rationale;

		private int webclientWarningFreq = 100000;

		private int noWebClientIdWarningCount;

		internal NativeClient(PlayGamesClientConfiguration configuration, IClientImpl clientImpl)
		{
			PlayGamesHelperObject.CreateObject();
			this.mConfiguration = Misc.CheckNotNull<PlayGamesClientConfiguration>(configuration);
			this.clientImpl = clientImpl;
			this.rationale = configuration.PermissionRationale;
			if (string.IsNullOrEmpty(this.rationale))
			{
				this.rationale = "Select email address to send to this game or hit cancel to not share.";
			}
		}

		private static Action<T> AsOnGameThreadCallback<T>(Action<T> callback)
		{
			if (callback == null)
			{
				return (T argument0) => {
				};
			}
			return (T result) => NativeClient.InvokeCallbackOnGameThread<T>(callback, result);
		}

		public void Authenticate(Action<bool> callback, bool silent)
		{
			object authStateLock = this.AuthStateLock;
			Monitor.Enter(authStateLock);
			try
			{
				if (this.mAuthState == NativeClient.AuthState.Authenticated)
				{
					NativeClient.InvokeCallbackOnGameThread<bool>(callback, true);
					return;
				}
				else if (this.mSilentAuthFailed && silent)
				{
					NativeClient.InvokeCallbackOnGameThread<bool>(callback, false);
					return;
				}
				else if (callback != null)
				{
					if (!silent)
					{
						this.mPendingAuthCallbacks += callback;
					}
					else
					{
						this.mSilentAuthCallbacks += callback;
					}
				}
			}
			finally
			{
				Monitor.Exit(authStateLock);
			}
			this.InitializeGameServices();
			this.friendsLoading = false;
			if (!silent)
			{
				this.GameServices().StartAuthorizationUI();
			}
		}

		private GooglePlayGames.Native.PInvoke.GameServices GameServices()
		{
			GooglePlayGames.Native.PInvoke.GameServices gameService;
			object gameServicesLock = this.GameServicesLock;
			Monitor.Enter(gameServicesLock);
			try
			{
				gameService = this.mServices;
			}
			finally
			{
				Monitor.Exit(gameServicesLock);
			}
			return gameService;
		}

		[Obsolete("Use GetServerAuthCode() then exchange it for a token")]
		public string GetAccessToken()
		{
			if (!this.IsAuthenticated())
			{
				Debug.Log("Cannot get API client - not authenticated");
				return null;
			}
			if (GameInfo.WebClientIdInitialized())
			{
				this.mTokenClient.SetRationale(this.rationale);
				return this.mTokenClient.GetAccessToken();
			}
			NativeClient nativeClient = this;
			int num = nativeClient.noWebClientIdWarningCount;
			int num1 = num;
			nativeClient.noWebClientIdWarningCount = num + 1;
			if (num1 % this.webclientWarningFreq == 0)
			{
				Debug.LogError("Web client ID has not been set, cannot request access token.");
				this.noWebClientIdWarningCount = this.noWebClientIdWarningCount / this.webclientWarningFreq + 1;
			}
			return null;
		}

		public GooglePlayGames.BasicApi.Achievement GetAchievement(string achId)
		{
			if (this.mAchievements == null || !this.mAchievements.ContainsKey(achId))
			{
				return null;
			}
			return this.mAchievements[achId];
		}

		public IntPtr GetApiClient()
		{
			return InternalHooks.InternalHooks_GetApiClient(this.mServices.AsHandle());
		}

		public IEventsClient GetEventsClient()
		{
			IEventsClient eventsClient;
			object gameServicesLock = this.GameServicesLock;
			Monitor.Enter(gameServicesLock);
			try
			{
				eventsClient = this.mEventsClient;
			}
			finally
			{
				Monitor.Exit(gameServicesLock);
			}
			return eventsClient;
		}

		public IUserProfile[] GetFriends()
		{
			IUserProfile[] array;
			if (this.mFriends == null && !this.friendsLoading)
			{
				GooglePlayGames.OurUtils.Logger.w("Getting friends before they are loaded!!!");
				this.friendsLoading = true;
				this.LoadFriends((bool ok) => {
					GooglePlayGames.OurUtils.Logger.d(string.Concat(new object[] { "loading: ", ok, " mFriends = ", this.mFriends }));
					if (!ok)
					{
						GooglePlayGames.OurUtils.Logger.e("Friends list did not load successfully.  Disabling loading until re-authenticated");
					}
					this.friendsLoading = !ok;
				});
			}
			if (this.mFriends != null)
			{
				array = this.mFriends.ToArray();
			}
			else
			{
				array = new IUserProfile[0];
			}
			return array;
		}

		[Obsolete("Use GetServerAuthCode() then exchange it for a token")]
		public void GetIdToken(Action<string> idTokenCallback)
		{
			if (!this.IsAuthenticated())
			{
				Debug.Log("Cannot get API client - not authenticated");
				PlayGamesHelperObject.RunOnGameThread(() => idTokenCallback(null));
			}
			if (!GameInfo.WebClientIdInitialized())
			{
				NativeClient nativeClient = this;
				int num = nativeClient.noWebClientIdWarningCount;
				int num1 = num;
				nativeClient.noWebClientIdWarningCount = num + 1;
				if (num1 % this.webclientWarningFreq == 0)
				{
					Debug.LogError("Web client ID has not been set, cannot request id token.");
					this.noWebClientIdWarningCount = this.noWebClientIdWarningCount / this.webclientWarningFreq + 1;
				}
				PlayGamesHelperObject.RunOnGameThread(() => idTokenCallback(null));
			}
			this.mTokenClient.SetRationale(this.rationale);
			this.mTokenClient.GetIdToken(string.Empty, NativeClient.AsOnGameThreadCallback<string>(idTokenCallback));
		}

		public void GetPlayerStats(Action<CommonStatusCodes, GooglePlayGames.BasicApi.PlayerStats> callback)
		{
			this.mServices.StatsManager().FetchForPlayer((GooglePlayGames.Native.PInvoke.StatsManager.FetchForPlayerResponse playerStatsResponse) => {
				CommonStatusCodes commonStatus = ConversionUtils.ConvertResponseStatusToCommonStatus(playerStatsResponse.Status());
				if (commonStatus != CommonStatusCodes.Success && commonStatus != CommonStatusCodes.SuccessCached)
				{
					GooglePlayGames.OurUtils.Logger.e(string.Concat("Error loading PlayerStats: ", playerStatsResponse.Status().ToString()));
				}
				if (callback != null)
				{
					if (playerStatsResponse.PlayerStats() == null)
					{
						PlayGamesHelperObject.RunOnGameThread(() => callback(commonStatus, new GooglePlayGames.BasicApi.PlayerStats()));
					}
					else
					{
						GooglePlayGames.BasicApi.PlayerStats playerStat = playerStatsResponse.PlayerStats().AsPlayerStats();
						PlayGamesHelperObject.RunOnGameThread(() => callback(commonStatus, playerStat));
					}
				}
			});
		}

		public IQuestsClient GetQuestsClient()
		{
			IQuestsClient questsClient;
			object gameServicesLock = this.GameServicesLock;
			Monitor.Enter(gameServicesLock);
			try
			{
				questsClient = this.mQuestsClient;
			}
			finally
			{
				Monitor.Exit(gameServicesLock);
			}
			return questsClient;
		}

		public IRealTimeMultiplayerClient GetRtmpClient()
		{
			// 
			// Current member / type: GooglePlayGames.BasicApi.Multiplayer.IRealTimeMultiplayerClient GooglePlayGames.Native.NativeClient::GetRtmpClient()
			// File path: c:\Users\lbert\Downloads\AF3DWBexsd0viV96e5U9-SkM_V5zvgedtPgl0ckOW0viY3BQRpH0nOQr2srRNskocOff7lYXZtSb-RdgwIBSTEfKABF0f2FHtkSZj0j6yPgtI2YdrQdKtFI\assets\bin\Data\Managed\Assembly-CSharp.dll
			// 
			// Product version: 0.9.2.0
			// Exception in: GooglePlayGames.BasicApi.Multiplayer.IRealTimeMultiplayerClient GetRtmpClient()
			// 
			// Object reference not set to an instance of an object.
			//    at Telerik.JustDecompiler.Steps.RebuildLockStatements.get_Lock() in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Steps\RebuildLockStatements.cs:line 93
			//    at Telerik.JustDecompiler.Steps.RebuildLockStatements.VisitBlockStatement(BlockStatement node) in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Steps\RebuildLockStatements.cs:line 24
			//    at Telerik.JustDecompiler.Ast.BaseCodeVisitor.Visit(ICodeNode node) in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Ast\BaseCodeVisitor.cs:line 69
			//    at Telerik.JustDecompiler.Steps.RebuildLockStatements.Process(DecompilationContext context, BlockStatement body) in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Steps\RebuildLockStatements.cs:line 18
			//    at Telerik.JustDecompiler.Decompiler.DecompilationPipeline.RunInternal(MethodBody body, BlockStatement block, ILanguage language) in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Decompiler\DecompilationPipeline.cs:line 81
			//    at Telerik.JustDecompiler.Decompiler.DecompilationPipeline.Run(MethodBody body, ILanguage language) in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Decompiler\DecompilationPipeline.cs:line 70
			//    at Telerik.JustDecompiler.Decompiler.Extensions.RunPipeline(DecompilationPipeline pipeline, ILanguage language, MethodBody body, DecompilationContext& context) in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Decompiler\Extensions.cs:line 95
			//    at Telerik.JustDecompiler.Decompiler.Extensions.Decompile(MethodBody body, ILanguage language, DecompilationContext& context, TypeSpecificContext typeContext) in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Decompiler\Extensions.cs:line 61
			//    at Telerik.JustDecompiler.Decompiler.WriterContextServices.BaseWriterContextService.DecompileMethod(ILanguage language, MethodDefinition method, TypeSpecificContext typeContext) in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Decompiler\WriterContextServices\BaseWriterContextService.cs:line 118
			// 
			// mailto: JustDecompilePublicFeedback@telerik.com

		}

		public ISavedGameClient GetSavedGameClient()
		{
			ISavedGameClient savedGameClient;
			object gameServicesLock = this.GameServicesLock;
			Monitor.Enter(gameServicesLock);
			try
			{
				savedGameClient = this.mSavedGameClient;
			}
			finally
			{
				Monitor.Exit(gameServicesLock);
			}
			return savedGameClient;
		}

		public void GetServerAuthCode(string serverClientId, Action<CommonStatusCodes, string> callback)
		{
			this.mServices.FetchServerAuthCode(serverClientId, (GooglePlayGames.Native.PInvoke.GameServices.FetchServerAuthCodeResponse serverAuthCodeResponse) => {
				CommonStatusCodes commonStatus = ConversionUtils.ConvertResponseStatusToCommonStatus(serverAuthCodeResponse.Status());
				if (commonStatus != CommonStatusCodes.Success && commonStatus != CommonStatusCodes.SuccessCached)
				{
					GooglePlayGames.OurUtils.Logger.e(string.Concat("Error loading server auth code: ", serverAuthCodeResponse.Status().ToString()));
				}
				if (callback != null)
				{
					string str = serverAuthCodeResponse.Code();
					PlayGamesHelperObject.RunOnGameThread(() => callback(commonStatus, str));
				}
			});
		}

		public ITurnBasedMultiplayerClient GetTbmpClient()
		{
			ITurnBasedMultiplayerClient turnBasedMultiplayerClient;
			object gameServicesLock = this.GameServicesLock;
			Monitor.Enter(gameServicesLock);
			try
			{
				turnBasedMultiplayerClient = this.mTurnBasedClient;
			}
			finally
			{
				Monitor.Exit(gameServicesLock);
			}
			return turnBasedMultiplayerClient;
		}

		public string GetToken()
		{
			if (this.mTokenClient == null)
			{
				return null;
			}
			return this.mTokenClient.GetAccessToken();
		}

		public string GetUserDisplayName()
		{
			if (this.mUser == null)
			{
				return null;
			}
			return this.mUser.userName;
		}

		public string GetUserEmail()
		{
			if (!this.IsAuthenticated())
			{
				Debug.Log("Cannot get API client - not authenticated");
				return null;
			}
			this.mTokenClient.SetRationale(this.rationale);
			return this.mTokenClient.GetEmail();
		}

		public void GetUserEmail(Action<CommonStatusCodes, string> callback)
		{
			if (!this.IsAuthenticated())
			{
				Debug.Log("Cannot get API client - not authenticated");
				if (callback != null)
				{
					PlayGamesHelperObject.RunOnGameThread(() => callback(4, null));
					return;
				}
			}
			this.mTokenClient.SetRationale(this.rationale);
			this.mTokenClient.GetEmail((CommonStatusCodes status, string email) => PlayGamesHelperObject.RunOnGameThread(() => callback(status, email)));
		}

		public string GetUserId()
		{
			if (this.mUser == null)
			{
				return null;
			}
			return this.mUser.id;
		}

		public string GetUserImageUrl()
		{
			if (this.mUser == null)
			{
				return null;
			}
			return this.mUser.AvatarURL;
		}

		private void HandleAuthTransition(GooglePlayGames.Native.Cwrapper.Types.AuthOperation operation, CommonErrorStatus.AuthStatus status)
		{
			GooglePlayGames.OurUtils.Logger.d(string.Concat(new object[] { "Starting Auth Transition. Op: ", operation, " status: ", status }));
			object authStateLock = this.AuthStateLock;
			Monitor.Enter(authStateLock);
			try
			{
				GooglePlayGames.Native.Cwrapper.Types.AuthOperation authOperation = operation;
				if (authOperation != GooglePlayGames.Native.Cwrapper.Types.AuthOperation.SIGN_IN)
				{
					if (authOperation == GooglePlayGames.Native.Cwrapper.Types.AuthOperation.SIGN_OUT)
					{
						this.ToUnauthenticated();
					}
					else
					{
						GooglePlayGames.OurUtils.Logger.e(string.Concat("Unknown AuthOperation ", operation));
					}
				}
				else if (status == CommonErrorStatus.AuthStatus.VALID)
				{
					if (this.mSilentAuthCallbacks != null)
					{
						this.mPendingAuthCallbacks += this.mSilentAuthCallbacks;
						this.mSilentAuthCallbacks = null;
					}
					uint num = this.mAuthGeneration;
					this.mServices.AchievementManager().FetchAll((GooglePlayGames.Native.PInvoke.AchievementManager.FetchAllResponse results) => this.PopulateAchievements(num, results));
					this.mServices.PlayerManager().FetchSelf((GooglePlayGames.Native.PInvoke.PlayerManager.FetchSelfResponse results) => this.PopulateUser(num, results));
				}
				else if (this.mAuthState != NativeClient.AuthState.SilentPending)
				{
					Debug.Log(string.Concat("AuthState == ", (NativeClient.AuthState)this.mAuthState, " calling auth callbacks with failure"));
					this.UnpauseUnityPlayer();
					Action<bool> action = this.mPendingAuthCallbacks;
					this.mPendingAuthCallbacks = null;
					NativeClient.InvokeCallbackOnGameThread<bool>(action, false);
				}
				else
				{
					this.mSilentAuthFailed = true;
					this.mAuthState = NativeClient.AuthState.Unauthenticated;
					Action<bool> action1 = this.mSilentAuthCallbacks;
					this.mSilentAuthCallbacks = null;
					Debug.Log("Invoking callbacks, AuthState changed from silentPending to Unauthenticated.");
					NativeClient.InvokeCallbackOnGameThread<bool>(action1, false);
					if (this.mPendingAuthCallbacks != null)
					{
						Debug.Log("there are pending auth callbacks - starting AuthUI");
						this.GameServices().StartAuthorizationUI();
					}
				}
			}
			finally
			{
				Monitor.Exit(authStateLock);
			}
		}

		internal void HandleInvitation(GooglePlayGames.Native.Cwrapper.Types.MultiplayerEvent eventType, string invitationId, GooglePlayGames.Native.PInvoke.MultiplayerInvitation invitation)
		{
			Action<Invitation, bool> action = this.mInvitationDelegate;
			if (action != null)
			{
				if (eventType == GooglePlayGames.Native.Cwrapper.Types.MultiplayerEvent.REMOVED)
				{
					GooglePlayGames.OurUtils.Logger.d(string.Concat("Ignoring REMOVED for invitation ", invitationId));
					return;
				}
				bool flag = eventType == GooglePlayGames.Native.Cwrapper.Types.MultiplayerEvent.UPDATED_FROM_APP_LAUNCH;
				Invitation invitation1 = invitation.AsInvitation();
				PlayGamesHelperObject.RunOnGameThread(() => action(invitation1, flag));
				return;
			}
			GooglePlayGames.OurUtils.Logger.d(string.Concat(new object[] { "Received ", eventType, " for invitation ", invitationId, " but no handler was registered." }));
		}

		public void IncrementAchievement(string achId, int steps, Action<bool> callback)
		{
			Action<bool> action = callback;
			Misc.CheckNotNull<string>(achId);
			action = NativeClient.AsOnGameThreadCallback<bool>(action);
			this.InitializeGameServices();
			GooglePlayGames.BasicApi.Achievement achievement = this.GetAchievement(achId);
			if (achievement == null)
			{
				GooglePlayGames.OurUtils.Logger.e(string.Concat("Could not increment, no achievement with ID ", achId));
				action(false);
				return;
			}
			if (!achievement.IsIncremental)
			{
				GooglePlayGames.OurUtils.Logger.e(string.Concat("Could not increment, achievement with ID ", achId, " was not incremental"));
				action(false);
				return;
			}
			if (steps < 0)
			{
				GooglePlayGames.OurUtils.Logger.e("Attempted to increment by negative steps");
				action(false);
				return;
			}
			this.GameServices().AchievementManager().Increment(achId, Convert.ToUInt32(steps));
			this.GameServices().AchievementManager().Fetch(achId, (GooglePlayGames.Native.PInvoke.AchievementManager.FetchResponse rsp) => {
				if (rsp.Status() != CommonErrorStatus.ResponseStatus.VALID)
				{
					GooglePlayGames.OurUtils.Logger.e(string.Concat(new object[] { "Cannot refresh achievement ", achId, ": ", rsp.Status() }));
					action(false);
				}
				else
				{
					this.mAchievements.Remove(achId);
					this.mAchievements.Add(achId, rsp.Achievement().AsAchievement());
					action(true);
				}
			});
		}

		private void InitializeGameServices()
		{
			string str;
			object gameServicesLock = this.GameServicesLock;
			Monitor.Enter(gameServicesLock);
			try
			{
				if (this.mServices == null)
				{
					using (GameServicesBuilder gameServicesBuilder = GameServicesBuilder.Create())
					{
						using (PlatformConfiguration platformConfiguration = this.clientImpl.CreatePlatformConfiguration())
						{
							this.RegisterInvitationDelegate(this.mConfiguration.InvitationDelegate);
							gameServicesBuilder.SetOnAuthFinishedCallback(new GameServicesBuilder.AuthFinishedCallback(this.HandleAuthTransition));
							gameServicesBuilder.SetOnTurnBasedMatchEventCallback((GooglePlayGames.Native.Cwrapper.Types.MultiplayerEvent eventType, string matchId, NativeTurnBasedMatch match) => this.mTurnBasedClient.HandleMatchEvent(eventType, matchId, match));
							gameServicesBuilder.SetOnMultiplayerInvitationEventCallback(new Action<GooglePlayGames.Native.Cwrapper.Types.MultiplayerEvent, string, GooglePlayGames.Native.PInvoke.MultiplayerInvitation>(this.HandleInvitation));
							if (this.mConfiguration.EnableSavedGames)
							{
								gameServicesBuilder.EnableSnapshots();
							}
							if (this.mConfiguration.RequireGooglePlus)
							{
								gameServicesBuilder.RequireGooglePlus();
							}
							Debug.Log("Building GPG services, implicitly attempts silent auth");
							this.mAuthState = NativeClient.AuthState.SilentPending;
							this.mServices = gameServicesBuilder.Build(platformConfiguration);
							this.mEventsClient = new NativeEventClient(new GooglePlayGames.Native.PInvoke.EventManager(this.mServices));
							this.mQuestsClient = new NativeQuestClient(new GooglePlayGames.Native.PInvoke.QuestManager(this.mServices));
							this.mTurnBasedClient = new NativeTurnBasedMultiplayerClient(this, new TurnBasedManager(this.mServices));
							this.mTurnBasedClient.RegisterMatchDelegate(this.mConfiguration.MatchDelegate);
							this.mRealTimeClient = new NativeRealtimeMultiplayerClient(this, new RealtimeManager(this.mServices));
							if (!this.mConfiguration.EnableSavedGames)
							{
								this.mSavedGameClient = new UnsupportedSavedGamesClient("You must enable saved games before it can be used. See PlayGamesClientConfiguration.Builder.EnableSavedGames.");
							}
							else
							{
								this.mSavedGameClient = new NativeSavedGameClient(new GooglePlayGames.Native.PInvoke.SnapshotManager(this.mServices));
							}
							this.mAuthState = NativeClient.AuthState.SilentPending;
							IClientImpl clientImpl = this.clientImpl;
							if (this.mUser != null)
							{
								str = this.mUser.id;
							}
							else
							{
								str = null;
							}
							this.mTokenClient = clientImpl.CreateTokenClient(str, false);
						}
					}
				}
			}
			finally
			{
				Monitor.Exit(gameServicesLock);
			}
		}

		private static void InvokeCallbackOnGameThread<T>(Action<T> callback, T data)
		{
			if (callback == null)
			{
				return;
			}
			PlayGamesHelperObject.RunOnGameThread(() => {
				GooglePlayGames.OurUtils.Logger.d("Invoking user callback on game thread");
				callback(data);
			});
		}

		public bool IsAuthenticated()
		{
			bool flag;
			object authStateLock = this.AuthStateLock;
			Monitor.Enter(authStateLock);
			try
			{
				flag = this.mAuthState == NativeClient.AuthState.Authenticated;
			}
			finally
			{
				Monitor.Exit(authStateLock);
			}
			return flag;
		}

		public int LeaderboardMaxResults()
		{
			return this.GameServices().LeaderboardManager().LeaderboardMaxResults;
		}

		public void LoadAchievements(Action<GooglePlayGames.BasicApi.Achievement[]> callback)
		{
			GooglePlayGames.BasicApi.Achievement[] achievementArray = new GooglePlayGames.BasicApi.Achievement[this.mAchievements.Count];
			this.mAchievements.Values.CopyTo(achievementArray, 0);
			PlayGamesHelperObject.RunOnGameThread(() => callback(achievementArray));
		}

		public void LoadFriends(Action<bool> callback)
		{
			if (!this.IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.d("Cannot loadFriends when not authenticated");
				PlayGamesHelperObject.RunOnGameThread(() => callback(false));
				return;
			}
			if (this.mFriends != null)
			{
				PlayGamesHelperObject.RunOnGameThread(() => callback(true));
				return;
			}
			this.mServices.PlayerManager().FetchFriends((ResponseStatus status, List<GooglePlayGames.BasicApi.Multiplayer.Player> players) => {
				if (status == ResponseStatus.Success || status == ResponseStatus.SuccessWithStale)
				{
					this.mFriends = players;
					PlayGamesHelperObject.RunOnGameThread(() => callback(true));
				}
				else
				{
					this.mFriends = new List<GooglePlayGames.BasicApi.Multiplayer.Player>();
					GooglePlayGames.OurUtils.Logger.e(string.Concat("Got ", status, " loading friends"));
					PlayGamesHelperObject.RunOnGameThread(() => callback(false));
				}
			});
		}

		public void LoadMoreScores(ScorePageToken token, int rowCount, Action<LeaderboardScoreData> callback)
		{
			callback = NativeClient.AsOnGameThreadCallback<LeaderboardScoreData>(callback);
			this.GameServices().LeaderboardManager().LoadScorePage(null, rowCount, token, callback);
		}

		public void LoadScores(string leaderboardId, LeaderboardStart start, int rowCount, LeaderboardCollection collection, LeaderboardTimeSpan timeSpan, Action<LeaderboardScoreData> callback)
		{
			callback = NativeClient.AsOnGameThreadCallback<LeaderboardScoreData>(callback);
			this.GameServices().LeaderboardManager().LoadLeaderboardData(leaderboardId, start, rowCount, collection, timeSpan, this.mUser.id, callback);
		}

		public void LoadUsers(string[] userIds, Action<IUserProfile[]> callback)
		{
			this.mServices.PlayerManager().FetchList(userIds, (NativePlayer[] nativeUsers) => {
				IUserProfile[] userProfileArray = new IUserProfile[(int)nativeUsers.Length];
				for (int i = 0; i < (int)userProfileArray.Length; i++)
				{
					userProfileArray[i] = nativeUsers[i].AsPlayer();
				}
				PlayGamesHelperObject.RunOnGameThread(() => callback(userProfileArray));
			});
		}

		private void MaybeFinishAuthentication()
		{
			Action<bool> action = null;
			object authStateLock = this.AuthStateLock;
			Monitor.Enter(authStateLock);
			try
			{
				if (this.mUser == null || this.mAchievements == null)
				{
					GooglePlayGames.OurUtils.Logger.d(string.Concat(new object[] { "Auth not finished. User=", this.mUser, " achievements=", this.mAchievements }));
					return;
				}
				else
				{
					GooglePlayGames.OurUtils.Logger.d("Auth finished. Proceeding.");
					action = this.mPendingAuthCallbacks;
					this.mPendingAuthCallbacks = null;
					this.mAuthState = NativeClient.AuthState.Authenticated;
				}
			}
			finally
			{
				Monitor.Exit(authStateLock);
			}
			if (action != null)
			{
				GooglePlayGames.OurUtils.Logger.d(string.Concat("Invoking Callbacks: ", action));
				NativeClient.InvokeCallbackOnGameThread<bool>(action, true);
			}
		}

		private void PopulateAchievements(uint authGeneration, GooglePlayGames.Native.PInvoke.AchievementManager.FetchAllResponse response)
		{
			if (authGeneration != this.mAuthGeneration)
			{
				GooglePlayGames.OurUtils.Logger.d("Received achievement callback after signout occurred, ignoring");
				return;
			}
			GooglePlayGames.OurUtils.Logger.d(string.Concat("Populating Achievements, status = ", response.Status()));
			object authStateLock = this.AuthStateLock;
			Monitor.Enter(authStateLock);
			try
			{
				if (response.Status() == CommonErrorStatus.ResponseStatus.VALID || response.Status() == CommonErrorStatus.ResponseStatus.VALID_BUT_STALE)
				{
					Dictionary<string, GooglePlayGames.BasicApi.Achievement> strs = new Dictionary<string, GooglePlayGames.BasicApi.Achievement>();
					IEnumerator<NativeAchievement> enumerator = response.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							NativeAchievement current = enumerator.Current;
							using (current)
							{
								strs[current.Id()] = current.AsAchievement();
							}
						}
					}
					finally
					{
						if (enumerator == null)
						{
						}
						enumerator.Dispose();
					}
					GooglePlayGames.OurUtils.Logger.d(string.Concat("Found ", strs.Count, " Achievements"));
					this.mAchievements = strs;
				}
				else
				{
					GooglePlayGames.OurUtils.Logger.e("Error retrieving achievements - check the log for more information. Failing signin.");
					Action<bool> action = this.mPendingAuthCallbacks;
					this.mPendingAuthCallbacks = null;
					if (action != null)
					{
						NativeClient.InvokeCallbackOnGameThread<bool>(action, false);
					}
					this.SignOut();
					return;
				}
			}
			finally
			{
				Monitor.Exit(authStateLock);
			}
			GooglePlayGames.OurUtils.Logger.d("Maybe finish for Achievements");
			this.MaybeFinishAuthentication();
		}

		private void PopulateUser(uint authGeneration, GooglePlayGames.Native.PInvoke.PlayerManager.FetchSelfResponse response)
		{
			// 
			// Current member / type: System.Void GooglePlayGames.Native.NativeClient::PopulateUser(System.UInt32,GooglePlayGames.Native.PInvoke.PlayerManager/FetchSelfResponse)
			// File path: c:\Users\lbert\Downloads\AF3DWBexsd0viV96e5U9-SkM_V5zvgedtPgl0ckOW0viY3BQRpH0nOQr2srRNskocOff7lYXZtSb-RdgwIBSTEfKABF0f2FHtkSZj0j6yPgtI2YdrQdKtFI\assets\bin\Data\Managed\Assembly-CSharp.dll
			// 
			// Product version: 0.9.2.0
			// Exception in: System.Void PopulateUser(System.UInt32,GooglePlayGames.Native.PInvoke.PlayerManager/FetchSelfResponse)
			// 
			// Object reference not set to an instance of an object.
			//    at Telerik.JustDecompiler.Steps.RebuildLockStatements.get_Lock() in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Steps\RebuildLockStatements.cs:line 93
			//    at Telerik.JustDecompiler.Steps.RebuildLockStatements.VisitBlockStatement(BlockStatement node) in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Steps\RebuildLockStatements.cs:line 24
			//    at Telerik.JustDecompiler.Ast.BaseCodeVisitor.Visit(ICodeNode node) in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Ast\BaseCodeVisitor.cs:line 69
			//    at Telerik.JustDecompiler.Steps.RebuildLockStatements.Process(DecompilationContext context, BlockStatement body) in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Steps\RebuildLockStatements.cs:line 18
			//    at Telerik.JustDecompiler.Decompiler.DecompilationPipeline.RunInternal(MethodBody body, BlockStatement block, ILanguage language) in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Decompiler\DecompilationPipeline.cs:line 81
			//    at Telerik.JustDecompiler.Decompiler.DecompilationPipeline.Run(MethodBody body, ILanguage language) in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Decompiler\DecompilationPipeline.cs:line 70
			//    at Telerik.JustDecompiler.Decompiler.Extensions.RunPipeline(DecompilationPipeline pipeline, ILanguage language, MethodBody body, DecompilationContext& context) in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Decompiler\Extensions.cs:line 95
			//    at Telerik.JustDecompiler.Decompiler.Extensions.Decompile(MethodBody body, ILanguage language, DecompilationContext& context, TypeSpecificContext typeContext) in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Decompiler\Extensions.cs:line 61
			//    at Telerik.JustDecompiler.Decompiler.WriterContextServices.BaseWriterContextService.DecompileMethod(ILanguage language, MethodDefinition method, TypeSpecificContext typeContext) in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Decompiler\WriterContextServices\BaseWriterContextService.cs:line 118
			// 
			// mailto: JustDecompilePublicFeedback@telerik.com

		}

		public void RegisterInvitationDelegate(InvitationReceivedDelegate invitationDelegate)
		{
			if (invitationDelegate != null)
			{
				this.mInvitationDelegate = Callbacks.AsOnGameThreadCallback<Invitation, bool>((Invitation invitation, bool autoAccept) => invitationDelegate(invitation, autoAccept));
			}
			else
			{
				this.mInvitationDelegate = null;
			}
		}

		public void RevealAchievement(string achId, Action<bool> callback)
		{
			this.UpdateAchievement("Reveal", achId, callback, (GooglePlayGames.BasicApi.Achievement a) => a.IsRevealed, (GooglePlayGames.BasicApi.Achievement a) => {
				a.IsRevealed = true;
				this.GameServices().AchievementManager().Reveal(achId);
			});
		}

		public void SetStepsAtLeast(string achId, int steps, Action<bool> callback)
		{
			Action<bool> action = callback;
			Misc.CheckNotNull<string>(achId);
			action = NativeClient.AsOnGameThreadCallback<bool>(action);
			this.InitializeGameServices();
			GooglePlayGames.BasicApi.Achievement achievement = this.GetAchievement(achId);
			if (achievement == null)
			{
				GooglePlayGames.OurUtils.Logger.e(string.Concat("Could not increment, no achievement with ID ", achId));
				action(false);
				return;
			}
			if (!achievement.IsIncremental)
			{
				GooglePlayGames.OurUtils.Logger.e(string.Concat("Could not increment, achievement with ID ", achId, " is not incremental"));
				action(false);
				return;
			}
			if (steps < 0)
			{
				GooglePlayGames.OurUtils.Logger.e("Attempted to increment by negative steps");
				action(false);
				return;
			}
			this.GameServices().AchievementManager().SetStepsAtLeast(achId, Convert.ToUInt32(steps));
			this.GameServices().AchievementManager().Fetch(achId, (GooglePlayGames.Native.PInvoke.AchievementManager.FetchResponse rsp) => {
				if (rsp.Status() != CommonErrorStatus.ResponseStatus.VALID)
				{
					GooglePlayGames.OurUtils.Logger.e(string.Concat(new object[] { "Cannot refresh achievement ", achId, ": ", rsp.Status() }));
					action(false);
				}
				else
				{
					this.mAchievements.Remove(achId);
					this.mAchievements.Add(achId, rsp.Achievement().AsAchievement());
					action(true);
				}
			});
		}

		public void ShowAchievementsUI(Action<UIStatus> cb)
		{
			if (!this.IsAuthenticated())
			{
				return;
			}
			Action<CommonErrorStatus.UIStatus> noopUICallback = Callbacks.NoopUICallback;
			if (cb != null)
			{
				noopUICallback = (CommonErrorStatus.UIStatus result) => cb(result);
			}
			noopUICallback = NativeClient.AsOnGameThreadCallback<CommonErrorStatus.UIStatus>(noopUICallback);
			this.GameServices().AchievementManager().ShowAllUI(noopUICallback);
		}

		public void ShowLeaderboardUI(string leaderboardId, LeaderboardTimeSpan span, Action<UIStatus> cb)
		{
			if (!this.IsAuthenticated())
			{
				return;
			}
			Action<CommonErrorStatus.UIStatus> noopUICallback = Callbacks.NoopUICallback;
			if (cb != null)
			{
				noopUICallback = (CommonErrorStatus.UIStatus result) => cb(result);
			}
			noopUICallback = NativeClient.AsOnGameThreadCallback<CommonErrorStatus.UIStatus>(noopUICallback);
			if (leaderboardId != null)
			{
				this.GameServices().LeaderboardManager().ShowUI(leaderboardId, span, noopUICallback);
			}
			else
			{
				this.GameServices().LeaderboardManager().ShowAllUI(noopUICallback);
			}
		}

		public void SignOut()
		{
			this.ToUnauthenticated();
			if (this.GameServices() == null)
			{
				return;
			}
			this.GameServices().SignOut();
		}

		public void SubmitScore(string leaderboardId, long score, Action<bool> callback)
		{
			callback = NativeClient.AsOnGameThreadCallback<bool>(callback);
			if (!this.IsAuthenticated())
			{
				callback(false);
			}
			this.InitializeGameServices();
			if (leaderboardId == null)
			{
				throw new ArgumentNullException("leaderboardId");
			}
			this.GameServices().LeaderboardManager().SubmitScore(leaderboardId, score, null);
			callback(true);
		}

		public void SubmitScore(string leaderboardId, long score, string metadata, Action<bool> callback)
		{
			callback = NativeClient.AsOnGameThreadCallback<bool>(callback);
			if (!this.IsAuthenticated())
			{
				callback(false);
			}
			this.InitializeGameServices();
			if (leaderboardId == null)
			{
				throw new ArgumentNullException("leaderboardId");
			}
			this.GameServices().LeaderboardManager().SubmitScore(leaderboardId, score, metadata);
			callback(true);
		}

		private void ToUnauthenticated()
		{
			object authStateLock = this.AuthStateLock;
			Monitor.Enter(authStateLock);
			try
			{
				this.mUser = null;
				this.mFriends = null;
				this.mAchievements = null;
				this.mAuthState = NativeClient.AuthState.Unauthenticated;
				this.mTokenClient = this.clientImpl.CreateTokenClient(null, true);
				this.mAuthGeneration++;
			}
			finally
			{
				Monitor.Exit(authStateLock);
			}
		}

		public void UnlockAchievement(string achId, Action<bool> callback)
		{
			this.UpdateAchievement("Unlock", achId, callback, (GooglePlayGames.BasicApi.Achievement a) => a.IsUnlocked, (GooglePlayGames.BasicApi.Achievement a) => {
				a.IsUnlocked = true;
				this.GameServices().AchievementManager().Unlock(achId);
			});
		}

		private void UnpauseUnityPlayer()
		{
		}

		private void UpdateAchievement(string updateType, string achId, Action<bool> callback, Predicate<GooglePlayGames.BasicApi.Achievement> alreadyDone, Action<GooglePlayGames.BasicApi.Achievement> updateAchievment)
		{
			Action<bool> action = callback;
			action = NativeClient.AsOnGameThreadCallback<bool>(action);
			Misc.CheckNotNull<string>(achId);
			this.InitializeGameServices();
			GooglePlayGames.BasicApi.Achievement achievement = this.GetAchievement(achId);
			if (achievement == null)
			{
				GooglePlayGames.OurUtils.Logger.d(string.Concat("Could not ", updateType, ", no achievement with ID ", achId));
				action(false);
				return;
			}
			if (alreadyDone(achievement))
			{
				GooglePlayGames.OurUtils.Logger.d(string.Concat("Did not need to perform ", updateType, ": on achievement ", achId));
				action(true);
				return;
			}
			GooglePlayGames.OurUtils.Logger.d(string.Concat("Performing ", updateType, " on ", achId));
			updateAchievment(achievement);
			this.GameServices().AchievementManager().Fetch(achId, (GooglePlayGames.Native.PInvoke.AchievementManager.FetchResponse rsp) => {
				if (rsp.Status() != CommonErrorStatus.ResponseStatus.VALID)
				{
					GooglePlayGames.OurUtils.Logger.e(string.Concat(new object[] { "Cannot refresh achievement ", achId, ": ", rsp.Status() }));
					action(false);
				}
				else
				{
					this.mAchievements.Remove(achId);
					this.mAchievements.Add(achId, rsp.Achievement().AsAchievement());
					action(true);
				}
			});
		}

		private enum AuthState
		{
			Unauthenticated,
			Authenticated,
			SilentPending
		}
	}
}