using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Events;
using GooglePlayGames.BasicApi.Multiplayer;
using GooglePlayGames.BasicApi.Quests;
using GooglePlayGames.BasicApi.SavedGame;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.Native.PInvoke;
using GooglePlayGames.OurUtils;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace GooglePlayGames.Native
{
	public class NativeClient : IPlayGamesClient
	{
		private enum AuthState
		{
			Unauthenticated = 0,
			Authenticated = 1,
			SilentPending = 2
		}

		[CompilerGenerated]
		private sealed class _003CAsOnGameThreadCallback_003Ec__AnonStorey208<T>
		{
			internal Action<T> callback;

			internal void _003C_003Em__93(T result)
			{
				InvokeCallbackOnGameThread(callback, result);
			}
		}

		[CompilerGenerated]
		private sealed class _003CInvokeCallbackOnGameThread_003Ec__AnonStorey209<T>
		{
			internal Action<T> callback;

			internal T data;

			internal void _003C_003Em__94()
			{
				GooglePlayGames.OurUtils.Logger.d("Invoking user callback on game thread");
				callback(data);
			}
		}

		[CompilerGenerated]
		private sealed class _003CHandleInvitation_003Ec__AnonStorey20A
		{
			internal Action<Invitation, bool> currentHandler;

			internal Invitation invite;

			internal bool shouldAutolaunch;

			internal void _003C_003Em__96()
			{
				currentHandler(invite, shouldAutolaunch);
			}
		}

		[CompilerGenerated]
		private sealed class _003CGetUserEmail_003Ec__AnonStorey20B
		{
			private sealed class _003CGetUserEmail_003Ec__AnonStorey20C
			{
				internal CommonStatusCodes status;

				internal string email;

				internal _003CGetUserEmail_003Ec__AnonStorey20B _003C_003Ef__ref_0024523;

				internal void _003C_003Em__AF()
				{
					_003C_003Ef__ref_0024523.callback(status, email);
				}
			}

			internal Action<CommonStatusCodes, string> callback;

			internal void _003C_003Em__97()
			{
				callback(CommonStatusCodes.SignInRequired, null);
			}

			internal void _003C_003Em__98(CommonStatusCodes status, string email)
			{
				_003CGetUserEmail_003Ec__AnonStorey20C _003CGetUserEmail_003Ec__AnonStorey20C = new _003CGetUserEmail_003Ec__AnonStorey20C();
				_003CGetUserEmail_003Ec__AnonStorey20C._003C_003Ef__ref_0024523 = this;
				_003CGetUserEmail_003Ec__AnonStorey20C.status = status;
				_003CGetUserEmail_003Ec__AnonStorey20C.email = email;
				PlayGamesHelperObject.RunOnGameThread(_003CGetUserEmail_003Ec__AnonStorey20C._003C_003Em__AF);
			}
		}

		[CompilerGenerated]
		private sealed class _003CGetIdToken_003Ec__AnonStorey20D
		{
			internal Action<string> idTokenCallback;

			internal void _003C_003Em__99()
			{
				idTokenCallback(null);
			}

			internal void _003C_003Em__9A()
			{
				idTokenCallback(null);
			}
		}

		[CompilerGenerated]
		private sealed class _003CGetServerAuthCode_003Ec__AnonStorey20E
		{
			private sealed class _003CGetServerAuthCode_003Ec__AnonStorey20F
			{
				internal CommonStatusCodes responseCode;

				internal _003CGetServerAuthCode_003Ec__AnonStorey20E _003C_003Ef__ref_0024526;
			}

			private sealed class _003CGetServerAuthCode_003Ec__AnonStorey210
			{
				internal string authCode;

				internal _003CGetServerAuthCode_003Ec__AnonStorey20E _003C_003Ef__ref_0024526;

				internal _003CGetServerAuthCode_003Ec__AnonStorey20F _003C_003Ef__ref_0024527;

				internal void _003C_003Em__B0()
				{
					_003C_003Ef__ref_0024526.callback(_003C_003Ef__ref_0024527.responseCode, authCode);
				}
			}

			internal Action<CommonStatusCodes, string> callback;

			internal void _003C_003Em__9B(GooglePlayGames.Native.PInvoke.GameServices.FetchServerAuthCodeResponse serverAuthCodeResponse)
			{
				_003CGetServerAuthCode_003Ec__AnonStorey20F _003CGetServerAuthCode_003Ec__AnonStorey20F = new _003CGetServerAuthCode_003Ec__AnonStorey20F();
				_003CGetServerAuthCode_003Ec__AnonStorey20F._003C_003Ef__ref_0024526 = this;
				_003CGetServerAuthCode_003Ec__AnonStorey20F.responseCode = ConversionUtils.ConvertResponseStatusToCommonStatus(serverAuthCodeResponse.Status());
				if (_003CGetServerAuthCode_003Ec__AnonStorey20F.responseCode != 0 && _003CGetServerAuthCode_003Ec__AnonStorey20F.responseCode != CommonStatusCodes.SuccessCached)
				{
					GooglePlayGames.OurUtils.Logger.e("Error loading server auth code: " + serverAuthCodeResponse.Status());
				}
				if (callback != null)
				{
					_003CGetServerAuthCode_003Ec__AnonStorey210 _003CGetServerAuthCode_003Ec__AnonStorey = new _003CGetServerAuthCode_003Ec__AnonStorey210();
					_003CGetServerAuthCode_003Ec__AnonStorey._003C_003Ef__ref_0024526 = this;
					_003CGetServerAuthCode_003Ec__AnonStorey._003C_003Ef__ref_0024527 = _003CGetServerAuthCode_003Ec__AnonStorey20F;
					_003CGetServerAuthCode_003Ec__AnonStorey.authCode = serverAuthCodeResponse.Code();
					PlayGamesHelperObject.RunOnGameThread(_003CGetServerAuthCode_003Ec__AnonStorey._003C_003Em__B0);
				}
			}
		}

		[CompilerGenerated]
		private sealed class _003CLoadFriends_003Ec__AnonStorey211
		{
			internal Action<bool> callback;

			internal NativeClient _003C_003Ef__this;

			internal void _003C_003Em__9C()
			{
				callback(false);
			}

			internal void _003C_003Em__9D()
			{
				callback(true);
			}

			internal void _003C_003Em__9E(ResponseStatus status, List<GooglePlayGames.BasicApi.Multiplayer.Player> players)
			{
				if (status == ResponseStatus.Success || status == ResponseStatus.SuccessWithStale)
				{
					_003C_003Ef__this.mFriends = players;
					PlayGamesHelperObject.RunOnGameThread(_003C_003Em__B1);
				}
				else
				{
					_003C_003Ef__this.mFriends = new List<GooglePlayGames.BasicApi.Multiplayer.Player>();
					GooglePlayGames.OurUtils.Logger.e(string.Concat("Got ", status, " loading friends"));
					PlayGamesHelperObject.RunOnGameThread(_003C_003Em__B2);
				}
			}

			internal void _003C_003Em__B1()
			{
				callback(true);
			}

			internal void _003C_003Em__B2()
			{
				callback(false);
			}
		}

		[CompilerGenerated]
		private sealed class _003CHandleAuthTransition_003Ec__AnonStorey212
		{
			internal uint currentAuthGeneration;

			internal NativeClient _003C_003Ef__this;

			internal void _003C_003Em__A0(GooglePlayGames.Native.PInvoke.AchievementManager.FetchAllResponse results)
			{
				_003C_003Ef__this.PopulateAchievements(currentAuthGeneration, results);
			}

			internal void _003C_003Em__A1(GooglePlayGames.Native.PInvoke.PlayerManager.FetchSelfResponse results)
			{
				_003C_003Ef__this.PopulateUser(currentAuthGeneration, results);
			}
		}

		[CompilerGenerated]
		private sealed class _003CGetPlayerStats_003Ec__AnonStorey213
		{
			private sealed class _003CGetPlayerStats_003Ec__AnonStorey214
			{
				internal CommonStatusCodes responseCode;

				internal _003CGetPlayerStats_003Ec__AnonStorey213 _003C_003Ef__ref_0024531;

				internal void _003C_003Em__B4()
				{
					_003C_003Ef__ref_0024531.callback(responseCode, new GooglePlayGames.BasicApi.PlayerStats());
				}
			}

			private sealed class _003CGetPlayerStats_003Ec__AnonStorey215
			{
				internal GooglePlayGames.BasicApi.PlayerStats stats;

				internal _003CGetPlayerStats_003Ec__AnonStorey213 _003C_003Ef__ref_0024531;

				internal _003CGetPlayerStats_003Ec__AnonStorey214 _003C_003Ef__ref_0024532;

				internal void _003C_003Em__B3()
				{
					_003C_003Ef__ref_0024531.callback(_003C_003Ef__ref_0024532.responseCode, stats);
				}
			}

			internal Action<CommonStatusCodes, GooglePlayGames.BasicApi.PlayerStats> callback;

			internal void _003C_003Em__A2(GooglePlayGames.Native.PInvoke.StatsManager.FetchForPlayerResponse playerStatsResponse)
			{
				_003CGetPlayerStats_003Ec__AnonStorey214 _003CGetPlayerStats_003Ec__AnonStorey = new _003CGetPlayerStats_003Ec__AnonStorey214();
				_003CGetPlayerStats_003Ec__AnonStorey._003C_003Ef__ref_0024531 = this;
				_003CGetPlayerStats_003Ec__AnonStorey.responseCode = ConversionUtils.ConvertResponseStatusToCommonStatus(playerStatsResponse.Status());
				if (_003CGetPlayerStats_003Ec__AnonStorey.responseCode != 0 && _003CGetPlayerStats_003Ec__AnonStorey.responseCode != CommonStatusCodes.SuccessCached)
				{
					GooglePlayGames.OurUtils.Logger.e("Error loading PlayerStats: " + playerStatsResponse.Status());
				}
				if (callback != null)
				{
					if (playerStatsResponse.PlayerStats() != null)
					{
						_003CGetPlayerStats_003Ec__AnonStorey215 _003CGetPlayerStats_003Ec__AnonStorey2 = new _003CGetPlayerStats_003Ec__AnonStorey215();
						_003CGetPlayerStats_003Ec__AnonStorey2._003C_003Ef__ref_0024531 = this;
						_003CGetPlayerStats_003Ec__AnonStorey2._003C_003Ef__ref_0024532 = _003CGetPlayerStats_003Ec__AnonStorey;
						_003CGetPlayerStats_003Ec__AnonStorey2.stats = playerStatsResponse.PlayerStats().AsPlayerStats();
						PlayGamesHelperObject.RunOnGameThread(_003CGetPlayerStats_003Ec__AnonStorey2._003C_003Em__B3);
					}
					else
					{
						PlayGamesHelperObject.RunOnGameThread(_003CGetPlayerStats_003Ec__AnonStorey._003C_003Em__B4);
					}
				}
			}
		}

		[CompilerGenerated]
		private sealed class _003CLoadUsers_003Ec__AnonStorey216
		{
			private sealed class _003CLoadUsers_003Ec__AnonStorey217
			{
				internal IUserProfile[] users;

				internal _003CLoadUsers_003Ec__AnonStorey216 _003C_003Ef__ref_0024534;

				internal void _003C_003Em__B5()
				{
					_003C_003Ef__ref_0024534.callback(users);
				}
			}

			internal Action<IUserProfile[]> callback;

			internal void _003C_003Em__A3(NativePlayer[] nativeUsers)
			{
				_003CLoadUsers_003Ec__AnonStorey217 _003CLoadUsers_003Ec__AnonStorey = new _003CLoadUsers_003Ec__AnonStorey217();
				_003CLoadUsers_003Ec__AnonStorey._003C_003Ef__ref_0024534 = this;
				_003CLoadUsers_003Ec__AnonStorey.users = new IUserProfile[nativeUsers.Length];
				for (int i = 0; i < _003CLoadUsers_003Ec__AnonStorey.users.Length; i++)
				{
					_003CLoadUsers_003Ec__AnonStorey.users[i] = nativeUsers[i].AsPlayer();
				}
				PlayGamesHelperObject.RunOnGameThread(_003CLoadUsers_003Ec__AnonStorey._003C_003Em__B5);
			}
		}

		[CompilerGenerated]
		private sealed class _003CLoadAchievements_003Ec__AnonStorey218
		{
			internal Action<GooglePlayGames.BasicApi.Achievement[]> callback;

			internal GooglePlayGames.BasicApi.Achievement[] data;

			internal void _003C_003Em__A4()
			{
				callback(data);
			}
		}

		[CompilerGenerated]
		private sealed class _003CUnlockAchievement_003Ec__AnonStorey219
		{
			internal string achId;

			internal NativeClient _003C_003Ef__this;

			internal void _003C_003Em__A6(GooglePlayGames.BasicApi.Achievement a)
			{
				a.IsUnlocked = true;
				_003C_003Ef__this.GameServices().AchievementManager().Unlock(achId);
			}
		}

		[CompilerGenerated]
		private sealed class _003CRevealAchievement_003Ec__AnonStorey21A
		{
			internal string achId;

			internal NativeClient _003C_003Ef__this;

			internal void _003C_003Em__A8(GooglePlayGames.BasicApi.Achievement a)
			{
				a.IsRevealed = true;
				_003C_003Ef__this.GameServices().AchievementManager().Reveal(achId);
			}
		}

		[CompilerGenerated]
		private sealed class _003CUpdateAchievement_003Ec__AnonStorey21B
		{
			internal string achId;

			internal Action<bool> callback;

			internal NativeClient _003C_003Ef__this;

			internal void _003C_003Em__A9(GooglePlayGames.Native.PInvoke.AchievementManager.FetchResponse rsp)
			{
				if (rsp.Status() == CommonErrorStatus.ResponseStatus.VALID)
				{
					_003C_003Ef__this.mAchievements.Remove(achId);
					_003C_003Ef__this.mAchievements.Add(achId, rsp.Achievement().AsAchievement());
					callback(true);
					return;
				}
				GooglePlayGames.OurUtils.Logger.e("Cannot refresh achievement " + achId + ": " + rsp.Status());
				callback(false);
			}
		}

		[CompilerGenerated]
		private sealed class _003CIncrementAchievement_003Ec__AnonStorey21C
		{
			internal string achId;

			internal Action<bool> callback;

			internal NativeClient _003C_003Ef__this;

			internal void _003C_003Em__AA(GooglePlayGames.Native.PInvoke.AchievementManager.FetchResponse rsp)
			{
				if (rsp.Status() == CommonErrorStatus.ResponseStatus.VALID)
				{
					_003C_003Ef__this.mAchievements.Remove(achId);
					_003C_003Ef__this.mAchievements.Add(achId, rsp.Achievement().AsAchievement());
					callback(true);
					return;
				}
				GooglePlayGames.OurUtils.Logger.e("Cannot refresh achievement " + achId + ": " + rsp.Status());
				callback(false);
			}
		}

		[CompilerGenerated]
		private sealed class _003CSetStepsAtLeast_003Ec__AnonStorey21D
		{
			internal string achId;

			internal Action<bool> callback;

			internal NativeClient _003C_003Ef__this;

			internal void _003C_003Em__AB(GooglePlayGames.Native.PInvoke.AchievementManager.FetchResponse rsp)
			{
				if (rsp.Status() == CommonErrorStatus.ResponseStatus.VALID)
				{
					_003C_003Ef__this.mAchievements.Remove(achId);
					_003C_003Ef__this.mAchievements.Add(achId, rsp.Achievement().AsAchievement());
					callback(true);
					return;
				}
				GooglePlayGames.OurUtils.Logger.e("Cannot refresh achievement " + achId + ": " + rsp.Status());
				callback(false);
			}
		}

		[CompilerGenerated]
		private sealed class _003CShowAchievementsUI_003Ec__AnonStorey21E
		{
			internal Action<UIStatus> cb;

			internal void _003C_003Em__AC(CommonErrorStatus.UIStatus result)
			{
				cb((UIStatus)result);
			}
		}

		[CompilerGenerated]
		private sealed class _003CShowLeaderboardUI_003Ec__AnonStorey21F
		{
			internal Action<UIStatus> cb;

			internal void _003C_003Em__AD(CommonErrorStatus.UIStatus result)
			{
				cb((UIStatus)result);
			}
		}

		[CompilerGenerated]
		private sealed class _003CRegisterInvitationDelegate_003Ec__AnonStorey220
		{
			internal InvitationReceivedDelegate invitationDelegate;

			internal void _003C_003Em__AE(Invitation invitation, bool autoAccept)
			{
				invitationDelegate(invitation, autoAccept);
			}
		}

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

		private volatile AuthState mAuthState;

		private volatile uint mAuthGeneration;

		private volatile bool mSilentAuthFailed;

		private volatile bool friendsLoading;

		private string rationale;

		private int webclientWarningFreq = 100000;

		private int noWebClientIdWarningCount;

		[CompilerGenerated]
		private static Predicate<GooglePlayGames.BasicApi.Achievement> _003C_003Ef__am_0024cache18;

		[CompilerGenerated]
		private static Predicate<GooglePlayGames.BasicApi.Achievement> _003C_003Ef__am_0024cache19;

		internal NativeClient(PlayGamesClientConfiguration configuration, IClientImpl clientImpl)
		{
			PlayGamesHelperObject.CreateObject();
			mConfiguration = Misc.CheckNotNull(configuration);
			this.clientImpl = clientImpl;
			rationale = configuration.PermissionRationale;
			if (string.IsNullOrEmpty(rationale))
			{
				rationale = "Select email address to send to this game or hit cancel to not share.";
			}
		}

		private GooglePlayGames.Native.PInvoke.GameServices GameServices()
		{
			//Discarded unreachable code: IL_0019
			lock (GameServicesLock)
			{
				return mServices;
			}
		}

		public void Authenticate(Action<bool> callback, bool silent)
		{
			lock (AuthStateLock)
			{
				if (mAuthState == AuthState.Authenticated)
				{
					InvokeCallbackOnGameThread(callback, true);
					return;
				}
				if (mSilentAuthFailed && silent)
				{
					InvokeCallbackOnGameThread(callback, false);
					return;
				}
				if (callback != null)
				{
					if (silent)
					{
						mSilentAuthCallbacks = (Action<bool>)Delegate.Combine(mSilentAuthCallbacks, callback);
					}
					else
					{
						mPendingAuthCallbacks = (Action<bool>)Delegate.Combine(mPendingAuthCallbacks, callback);
					}
				}
			}
			InitializeGameServices();
			friendsLoading = false;
			if (!silent)
			{
				GameServices().StartAuthorizationUI();
			}
		}

		private static Action<T> AsOnGameThreadCallback<T>(Action<T> callback)
		{
			_003CAsOnGameThreadCallback_003Ec__AnonStorey208<T> _003CAsOnGameThreadCallback_003Ec__AnonStorey = new _003CAsOnGameThreadCallback_003Ec__AnonStorey208<T>();
			_003CAsOnGameThreadCallback_003Ec__AnonStorey.callback = callback;
			if (_003CAsOnGameThreadCallback_003Ec__AnonStorey.callback == null)
			{
				return _003CAsOnGameThreadCallback_00601_003Em__92;
			}
			return _003CAsOnGameThreadCallback_003Ec__AnonStorey._003C_003Em__93;
		}

		private static void InvokeCallbackOnGameThread<T>(Action<T> callback, T data)
		{
			_003CInvokeCallbackOnGameThread_003Ec__AnonStorey209<T> _003CInvokeCallbackOnGameThread_003Ec__AnonStorey = new _003CInvokeCallbackOnGameThread_003Ec__AnonStorey209<T>();
			_003CInvokeCallbackOnGameThread_003Ec__AnonStorey.callback = callback;
			_003CInvokeCallbackOnGameThread_003Ec__AnonStorey.data = data;
			if (_003CInvokeCallbackOnGameThread_003Ec__AnonStorey.callback != null)
			{
				PlayGamesHelperObject.RunOnGameThread(_003CInvokeCallbackOnGameThread_003Ec__AnonStorey._003C_003Em__94);
			}
		}

		private void InitializeGameServices()
		{
			lock (GameServicesLock)
			{
				if (mServices != null)
				{
					return;
				}
				using (GameServicesBuilder gameServicesBuilder = GameServicesBuilder.Create())
				{
					using (PlatformConfiguration configRef = clientImpl.CreatePlatformConfiguration())
					{
						RegisterInvitationDelegate(mConfiguration.InvitationDelegate);
						gameServicesBuilder.SetOnAuthFinishedCallback(HandleAuthTransition);
						gameServicesBuilder.SetOnTurnBasedMatchEventCallback(_003CInitializeGameServices_003Em__95);
						gameServicesBuilder.SetOnMultiplayerInvitationEventCallback(HandleInvitation);
						if (mConfiguration.EnableSavedGames)
						{
							gameServicesBuilder.EnableSnapshots();
						}
						if (mConfiguration.RequireGooglePlus)
						{
							gameServicesBuilder.RequireGooglePlus();
						}
						Debug.Log("Building GPG services, implicitly attempts silent auth");
						mAuthState = AuthState.SilentPending;
						mServices = gameServicesBuilder.Build(configRef);
						mEventsClient = new NativeEventClient(new GooglePlayGames.Native.PInvoke.EventManager(mServices));
						mQuestsClient = new NativeQuestClient(new GooglePlayGames.Native.PInvoke.QuestManager(mServices));
						mTurnBasedClient = new NativeTurnBasedMultiplayerClient(this, new TurnBasedManager(mServices));
						mTurnBasedClient.RegisterMatchDelegate(mConfiguration.MatchDelegate);
						mRealTimeClient = new NativeRealtimeMultiplayerClient(this, new RealtimeManager(mServices));
						if (mConfiguration.EnableSavedGames)
						{
							mSavedGameClient = new NativeSavedGameClient(new GooglePlayGames.Native.PInvoke.SnapshotManager(mServices));
						}
						else
						{
							mSavedGameClient = new UnsupportedSavedGamesClient("You must enable saved games before it can be used. See PlayGamesClientConfiguration.Builder.EnableSavedGames.");
						}
						mAuthState = AuthState.SilentPending;
						mTokenClient = clientImpl.CreateTokenClient((mUser != null) ? mUser.id : null, false);
					}
				}
			}
		}

		internal void HandleInvitation(GooglePlayGames.Native.Cwrapper.Types.MultiplayerEvent eventType, string invitationId, GooglePlayGames.Native.PInvoke.MultiplayerInvitation invitation)
		{
			_003CHandleInvitation_003Ec__AnonStorey20A _003CHandleInvitation_003Ec__AnonStorey20A = new _003CHandleInvitation_003Ec__AnonStorey20A();
			_003CHandleInvitation_003Ec__AnonStorey20A.currentHandler = mInvitationDelegate;
			if (_003CHandleInvitation_003Ec__AnonStorey20A.currentHandler == null)
			{
				GooglePlayGames.OurUtils.Logger.d(string.Concat("Received ", eventType, " for invitation ", invitationId, " but no handler was registered."));
			}
			else if (eventType == GooglePlayGames.Native.Cwrapper.Types.MultiplayerEvent.REMOVED)
			{
				GooglePlayGames.OurUtils.Logger.d("Ignoring REMOVED for invitation " + invitationId);
			}
			else
			{
				_003CHandleInvitation_003Ec__AnonStorey20A.shouldAutolaunch = eventType == GooglePlayGames.Native.Cwrapper.Types.MultiplayerEvent.UPDATED_FROM_APP_LAUNCH;
				_003CHandleInvitation_003Ec__AnonStorey20A.invite = invitation.AsInvitation();
				PlayGamesHelperObject.RunOnGameThread(_003CHandleInvitation_003Ec__AnonStorey20A._003C_003Em__96);
			}
		}

		public string GetUserEmail()
		{
			if (!IsAuthenticated())
			{
				Debug.Log("Cannot get API client - not authenticated");
				return null;
			}
			mTokenClient.SetRationale(rationale);
			return mTokenClient.GetEmail();
		}

		public void GetUserEmail(Action<CommonStatusCodes, string> callback)
		{
			_003CGetUserEmail_003Ec__AnonStorey20B _003CGetUserEmail_003Ec__AnonStorey20B = new _003CGetUserEmail_003Ec__AnonStorey20B();
			_003CGetUserEmail_003Ec__AnonStorey20B.callback = callback;
			if (!IsAuthenticated())
			{
				Debug.Log("Cannot get API client - not authenticated");
				if (_003CGetUserEmail_003Ec__AnonStorey20B.callback != null)
				{
					PlayGamesHelperObject.RunOnGameThread(_003CGetUserEmail_003Ec__AnonStorey20B._003C_003Em__97);
					return;
				}
			}
			mTokenClient.SetRationale(rationale);
			mTokenClient.GetEmail(_003CGetUserEmail_003Ec__AnonStorey20B._003C_003Em__98);
		}

		[Obsolete("Use GetServerAuthCode() then exchange it for a token")]
		public string GetAccessToken()
		{
			if (!IsAuthenticated())
			{
				Debug.Log("Cannot get API client - not authenticated");
				return null;
			}
			if (!GameInfo.WebClientIdInitialized())
			{
				if (noWebClientIdWarningCount++ % webclientWarningFreq == 0)
				{
					Debug.LogError("Web client ID has not been set, cannot request access token.");
					noWebClientIdWarningCount = noWebClientIdWarningCount / webclientWarningFreq + 1;
				}
				return null;
			}
			mTokenClient.SetRationale(rationale);
			return mTokenClient.GetAccessToken();
		}

		[Obsolete("Use GetServerAuthCode() then exchange it for a token")]
		public void GetIdToken(Action<string> idTokenCallback)
		{
			_003CGetIdToken_003Ec__AnonStorey20D _003CGetIdToken_003Ec__AnonStorey20D = new _003CGetIdToken_003Ec__AnonStorey20D();
			_003CGetIdToken_003Ec__AnonStorey20D.idTokenCallback = idTokenCallback;
			if (!IsAuthenticated())
			{
				Debug.Log("Cannot get API client - not authenticated");
				PlayGamesHelperObject.RunOnGameThread(_003CGetIdToken_003Ec__AnonStorey20D._003C_003Em__99);
			}
			if (!GameInfo.WebClientIdInitialized())
			{
				if (noWebClientIdWarningCount++ % webclientWarningFreq == 0)
				{
					Debug.LogError("Web client ID has not been set, cannot request id token.");
					noWebClientIdWarningCount = noWebClientIdWarningCount / webclientWarningFreq + 1;
				}
				PlayGamesHelperObject.RunOnGameThread(_003CGetIdToken_003Ec__AnonStorey20D._003C_003Em__9A);
			}
			mTokenClient.SetRationale(rationale);
			mTokenClient.GetIdToken(string.Empty, AsOnGameThreadCallback(_003CGetIdToken_003Ec__AnonStorey20D.idTokenCallback));
		}

		public void GetServerAuthCode(string serverClientId, Action<CommonStatusCodes, string> callback)
		{
			_003CGetServerAuthCode_003Ec__AnonStorey20E _003CGetServerAuthCode_003Ec__AnonStorey20E = new _003CGetServerAuthCode_003Ec__AnonStorey20E();
			_003CGetServerAuthCode_003Ec__AnonStorey20E.callback = callback;
			mServices.FetchServerAuthCode(serverClientId, _003CGetServerAuthCode_003Ec__AnonStorey20E._003C_003Em__9B);
		}

		public bool IsAuthenticated()
		{
			//Discarded unreachable code: IL_001e
			lock (AuthStateLock)
			{
				return mAuthState == AuthState.Authenticated;
			}
		}

		public void LoadFriends(Action<bool> callback)
		{
			_003CLoadFriends_003Ec__AnonStorey211 _003CLoadFriends_003Ec__AnonStorey = new _003CLoadFriends_003Ec__AnonStorey211();
			_003CLoadFriends_003Ec__AnonStorey.callback = callback;
			_003CLoadFriends_003Ec__AnonStorey._003C_003Ef__this = this;
			if (!IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.d("Cannot loadFriends when not authenticated");
				PlayGamesHelperObject.RunOnGameThread(_003CLoadFriends_003Ec__AnonStorey._003C_003Em__9C);
			}
			else if (mFriends != null)
			{
				PlayGamesHelperObject.RunOnGameThread(_003CLoadFriends_003Ec__AnonStorey._003C_003Em__9D);
			}
			else
			{
				mServices.PlayerManager().FetchFriends(_003CLoadFriends_003Ec__AnonStorey._003C_003Em__9E);
			}
		}

		public IUserProfile[] GetFriends()
		{
			if (mFriends == null && !friendsLoading)
			{
				GooglePlayGames.OurUtils.Logger.w("Getting friends before they are loaded!!!");
				friendsLoading = true;
				LoadFriends(_003CGetFriends_003Em__9F);
			}
			return (mFriends != null) ? mFriends.ToArray() : new IUserProfile[0];
		}

		private void PopulateAchievements(uint authGeneration, GooglePlayGames.Native.PInvoke.AchievementManager.FetchAllResponse response)
		{
			if (authGeneration != mAuthGeneration)
			{
				GooglePlayGames.OurUtils.Logger.d("Received achievement callback after signout occurred, ignoring");
				return;
			}
			GooglePlayGames.OurUtils.Logger.d("Populating Achievements, status = " + response.Status());
			lock (AuthStateLock)
			{
				if (response.Status() != CommonErrorStatus.ResponseStatus.VALID && response.Status() != CommonErrorStatus.ResponseStatus.VALID_BUT_STALE)
				{
					GooglePlayGames.OurUtils.Logger.e("Error retrieving achievements - check the log for more information. Failing signin.");
					Action<bool> action = mPendingAuthCallbacks;
					mPendingAuthCallbacks = null;
					if (action != null)
					{
						InvokeCallbackOnGameThread(action, false);
					}
					SignOut();
					return;
				}
				Dictionary<string, GooglePlayGames.BasicApi.Achievement> dictionary = new Dictionary<string, GooglePlayGames.BasicApi.Achievement>();
				foreach (NativeAchievement item in response)
				{
					using (item)
					{
						dictionary[item.Id()] = item.AsAchievement();
					}
				}
				GooglePlayGames.OurUtils.Logger.d("Found " + dictionary.Count + " Achievements");
				mAchievements = dictionary;
			}
			GooglePlayGames.OurUtils.Logger.d("Maybe finish for Achievements");
			MaybeFinishAuthentication();
		}

		private void MaybeFinishAuthentication()
		{
			Action<bool> action = null;
			lock (AuthStateLock)
			{
				if (mUser == null || mAchievements == null)
				{
					GooglePlayGames.OurUtils.Logger.d(string.Concat("Auth not finished. User=", mUser, " achievements=", mAchievements));
					return;
				}
				GooglePlayGames.OurUtils.Logger.d("Auth finished. Proceeding.");
				action = mPendingAuthCallbacks;
				mPendingAuthCallbacks = null;
				mAuthState = AuthState.Authenticated;
			}
			if (action != null)
			{
				GooglePlayGames.OurUtils.Logger.d("Invoking Callbacks: " + action);
				InvokeCallbackOnGameThread(action, true);
			}
		}

		private void PopulateUser(uint authGeneration, GooglePlayGames.Native.PInvoke.PlayerManager.FetchSelfResponse response)
		{
			GooglePlayGames.OurUtils.Logger.d("Populating User");
			if (authGeneration != mAuthGeneration)
			{
				GooglePlayGames.OurUtils.Logger.d("Received user callback after signout occurred, ignoring");
				return;
			}
			lock (AuthStateLock)
			{
				if (response.Status() != CommonErrorStatus.ResponseStatus.VALID && response.Status() != CommonErrorStatus.ResponseStatus.VALID_BUT_STALE)
				{
					GooglePlayGames.OurUtils.Logger.e("Error retrieving user, signing out");
					Action<bool> action = mPendingAuthCallbacks;
					mPendingAuthCallbacks = null;
					if (action != null)
					{
						InvokeCallbackOnGameThread(action, false);
					}
					SignOut();
					return;
				}
				mUser = response.Self().AsPlayer();
				mFriends = null;
				mTokenClient = clientImpl.CreateTokenClient(mUser.id, true);
			}
			GooglePlayGames.OurUtils.Logger.d("Found User: " + mUser);
			GooglePlayGames.OurUtils.Logger.d("Maybe finish for User");
			MaybeFinishAuthentication();
		}

		private void HandleAuthTransition(GooglePlayGames.Native.Cwrapper.Types.AuthOperation operation, CommonErrorStatus.AuthStatus status)
		{
			GooglePlayGames.OurUtils.Logger.d(string.Concat("Starting Auth Transition. Op: ", operation, " status: ", status));
			lock (AuthStateLock)
			{
				switch (operation)
				{
				case GooglePlayGames.Native.Cwrapper.Types.AuthOperation.SIGN_IN:
					if (status == CommonErrorStatus.AuthStatus.VALID)
					{
						_003CHandleAuthTransition_003Ec__AnonStorey212 _003CHandleAuthTransition_003Ec__AnonStorey = new _003CHandleAuthTransition_003Ec__AnonStorey212();
						_003CHandleAuthTransition_003Ec__AnonStorey._003C_003Ef__this = this;
						if (mSilentAuthCallbacks != null)
						{
							mPendingAuthCallbacks = (Action<bool>)Delegate.Combine(mPendingAuthCallbacks, mSilentAuthCallbacks);
							mSilentAuthCallbacks = null;
						}
						_003CHandleAuthTransition_003Ec__AnonStorey.currentAuthGeneration = mAuthGeneration;
						mServices.AchievementManager().FetchAll(_003CHandleAuthTransition_003Ec__AnonStorey._003C_003Em__A0);
						mServices.PlayerManager().FetchSelf(_003CHandleAuthTransition_003Ec__AnonStorey._003C_003Em__A1);
					}
					else if (mAuthState == AuthState.SilentPending)
					{
						mSilentAuthFailed = true;
						mAuthState = AuthState.Unauthenticated;
						Action<bool> callback = mSilentAuthCallbacks;
						mSilentAuthCallbacks = null;
						Debug.Log("Invoking callbacks, AuthState changed from silentPending to Unauthenticated.");
						InvokeCallbackOnGameThread(callback, false);
						if (mPendingAuthCallbacks != null)
						{
							Debug.Log("there are pending auth callbacks - starting AuthUI");
							GameServices().StartAuthorizationUI();
						}
					}
					else
					{
						Debug.Log(string.Concat("AuthState == ", mAuthState, " calling auth callbacks with failure"));
						UnpauseUnityPlayer();
						Action<bool> callback2 = mPendingAuthCallbacks;
						mPendingAuthCallbacks = null;
						InvokeCallbackOnGameThread(callback2, false);
					}
					break;
				case GooglePlayGames.Native.Cwrapper.Types.AuthOperation.SIGN_OUT:
					ToUnauthenticated();
					break;
				default:
					GooglePlayGames.OurUtils.Logger.e("Unknown AuthOperation " + operation);
					break;
				}
			}
		}

		private void UnpauseUnityPlayer()
		{
		}

		private void ToUnauthenticated()
		{
			lock (AuthStateLock)
			{
				mUser = null;
				mFriends = null;
				mAchievements = null;
				mAuthState = AuthState.Unauthenticated;
				mTokenClient = clientImpl.CreateTokenClient(null, true);
				mAuthGeneration++;
			}
		}

		public void SignOut()
		{
			ToUnauthenticated();
			if (GameServices() != null)
			{
				GameServices().SignOut();
			}
		}

		public string GetUserId()
		{
			if (mUser == null)
			{
				return null;
			}
			return mUser.id;
		}

		public string GetUserDisplayName()
		{
			if (mUser == null)
			{
				return null;
			}
			return mUser.userName;
		}

		public string GetUserImageUrl()
		{
			if (mUser == null)
			{
				return null;
			}
			return mUser.AvatarURL;
		}

		public void GetPlayerStats(Action<CommonStatusCodes, GooglePlayGames.BasicApi.PlayerStats> callback)
		{
			_003CGetPlayerStats_003Ec__AnonStorey213 _003CGetPlayerStats_003Ec__AnonStorey = new _003CGetPlayerStats_003Ec__AnonStorey213();
			_003CGetPlayerStats_003Ec__AnonStorey.callback = callback;
			mServices.StatsManager().FetchForPlayer(_003CGetPlayerStats_003Ec__AnonStorey._003C_003Em__A2);
		}

		public void LoadUsers(string[] userIds, Action<IUserProfile[]> callback)
		{
			_003CLoadUsers_003Ec__AnonStorey216 _003CLoadUsers_003Ec__AnonStorey = new _003CLoadUsers_003Ec__AnonStorey216();
			_003CLoadUsers_003Ec__AnonStorey.callback = callback;
			mServices.PlayerManager().FetchList(userIds, _003CLoadUsers_003Ec__AnonStorey._003C_003Em__A3);
		}

		public GooglePlayGames.BasicApi.Achievement GetAchievement(string achId)
		{
			if (mAchievements == null || !mAchievements.ContainsKey(achId))
			{
				return null;
			}
			return mAchievements[achId];
		}

		public void LoadAchievements(Action<GooglePlayGames.BasicApi.Achievement[]> callback)
		{
			_003CLoadAchievements_003Ec__AnonStorey218 _003CLoadAchievements_003Ec__AnonStorey = new _003CLoadAchievements_003Ec__AnonStorey218();
			_003CLoadAchievements_003Ec__AnonStorey.callback = callback;
			_003CLoadAchievements_003Ec__AnonStorey.data = new GooglePlayGames.BasicApi.Achievement[mAchievements.Count];
			mAchievements.Values.CopyTo(_003CLoadAchievements_003Ec__AnonStorey.data, 0);
			PlayGamesHelperObject.RunOnGameThread(_003CLoadAchievements_003Ec__AnonStorey._003C_003Em__A4);
		}

		public void UnlockAchievement(string achId, Action<bool> callback)
		{
			_003CUnlockAchievement_003Ec__AnonStorey219 _003CUnlockAchievement_003Ec__AnonStorey = new _003CUnlockAchievement_003Ec__AnonStorey219();
			_003CUnlockAchievement_003Ec__AnonStorey.achId = achId;
			_003CUnlockAchievement_003Ec__AnonStorey._003C_003Ef__this = this;
			string achId2 = _003CUnlockAchievement_003Ec__AnonStorey.achId;
			if (_003C_003Ef__am_0024cache18 == null)
			{
				_003C_003Ef__am_0024cache18 = _003CUnlockAchievement_003Em__A5;
			}
			UpdateAchievement("Unlock", achId2, callback, _003C_003Ef__am_0024cache18, _003CUnlockAchievement_003Ec__AnonStorey._003C_003Em__A6);
		}

		public void RevealAchievement(string achId, Action<bool> callback)
		{
			_003CRevealAchievement_003Ec__AnonStorey21A _003CRevealAchievement_003Ec__AnonStorey21A = new _003CRevealAchievement_003Ec__AnonStorey21A();
			_003CRevealAchievement_003Ec__AnonStorey21A.achId = achId;
			_003CRevealAchievement_003Ec__AnonStorey21A._003C_003Ef__this = this;
			string achId2 = _003CRevealAchievement_003Ec__AnonStorey21A.achId;
			if (_003C_003Ef__am_0024cache19 == null)
			{
				_003C_003Ef__am_0024cache19 = _003CRevealAchievement_003Em__A7;
			}
			UpdateAchievement("Reveal", achId2, callback, _003C_003Ef__am_0024cache19, _003CRevealAchievement_003Ec__AnonStorey21A._003C_003Em__A8);
		}

		private void UpdateAchievement(string updateType, string achId, Action<bool> callback, Predicate<GooglePlayGames.BasicApi.Achievement> alreadyDone, Action<GooglePlayGames.BasicApi.Achievement> updateAchievment)
		{
			_003CUpdateAchievement_003Ec__AnonStorey21B _003CUpdateAchievement_003Ec__AnonStorey21B = new _003CUpdateAchievement_003Ec__AnonStorey21B();
			_003CUpdateAchievement_003Ec__AnonStorey21B.achId = achId;
			_003CUpdateAchievement_003Ec__AnonStorey21B.callback = callback;
			_003CUpdateAchievement_003Ec__AnonStorey21B._003C_003Ef__this = this;
			_003CUpdateAchievement_003Ec__AnonStorey21B.callback = AsOnGameThreadCallback(_003CUpdateAchievement_003Ec__AnonStorey21B.callback);
			Misc.CheckNotNull(_003CUpdateAchievement_003Ec__AnonStorey21B.achId);
			InitializeGameServices();
			GooglePlayGames.BasicApi.Achievement achievement = GetAchievement(_003CUpdateAchievement_003Ec__AnonStorey21B.achId);
			if (achievement == null)
			{
				GooglePlayGames.OurUtils.Logger.d("Could not " + updateType + ", no achievement with ID " + _003CUpdateAchievement_003Ec__AnonStorey21B.achId);
				_003CUpdateAchievement_003Ec__AnonStorey21B.callback(false);
			}
			else if (alreadyDone(achievement))
			{
				GooglePlayGames.OurUtils.Logger.d("Did not need to perform " + updateType + ": on achievement " + _003CUpdateAchievement_003Ec__AnonStorey21B.achId);
				_003CUpdateAchievement_003Ec__AnonStorey21B.callback(true);
			}
			else
			{
				GooglePlayGames.OurUtils.Logger.d("Performing " + updateType + " on " + _003CUpdateAchievement_003Ec__AnonStorey21B.achId);
				updateAchievment(achievement);
				GameServices().AchievementManager().Fetch(_003CUpdateAchievement_003Ec__AnonStorey21B.achId, _003CUpdateAchievement_003Ec__AnonStorey21B._003C_003Em__A9);
			}
		}

		public void IncrementAchievement(string achId, int steps, Action<bool> callback)
		{
			_003CIncrementAchievement_003Ec__AnonStorey21C _003CIncrementAchievement_003Ec__AnonStorey21C = new _003CIncrementAchievement_003Ec__AnonStorey21C();
			_003CIncrementAchievement_003Ec__AnonStorey21C.achId = achId;
			_003CIncrementAchievement_003Ec__AnonStorey21C.callback = callback;
			_003CIncrementAchievement_003Ec__AnonStorey21C._003C_003Ef__this = this;
			Misc.CheckNotNull(_003CIncrementAchievement_003Ec__AnonStorey21C.achId);
			_003CIncrementAchievement_003Ec__AnonStorey21C.callback = AsOnGameThreadCallback(_003CIncrementAchievement_003Ec__AnonStorey21C.callback);
			InitializeGameServices();
			GooglePlayGames.BasicApi.Achievement achievement = GetAchievement(_003CIncrementAchievement_003Ec__AnonStorey21C.achId);
			if (achievement == null)
			{
				GooglePlayGames.OurUtils.Logger.e("Could not increment, no achievement with ID " + _003CIncrementAchievement_003Ec__AnonStorey21C.achId);
				_003CIncrementAchievement_003Ec__AnonStorey21C.callback(false);
			}
			else if (!achievement.IsIncremental)
			{
				GooglePlayGames.OurUtils.Logger.e("Could not increment, achievement with ID " + _003CIncrementAchievement_003Ec__AnonStorey21C.achId + " was not incremental");
				_003CIncrementAchievement_003Ec__AnonStorey21C.callback(false);
			}
			else if (steps < 0)
			{
				GooglePlayGames.OurUtils.Logger.e("Attempted to increment by negative steps");
				_003CIncrementAchievement_003Ec__AnonStorey21C.callback(false);
			}
			else
			{
				GameServices().AchievementManager().Increment(_003CIncrementAchievement_003Ec__AnonStorey21C.achId, Convert.ToUInt32(steps));
				GameServices().AchievementManager().Fetch(_003CIncrementAchievement_003Ec__AnonStorey21C.achId, _003CIncrementAchievement_003Ec__AnonStorey21C._003C_003Em__AA);
			}
		}

		public void SetStepsAtLeast(string achId, int steps, Action<bool> callback)
		{
			_003CSetStepsAtLeast_003Ec__AnonStorey21D _003CSetStepsAtLeast_003Ec__AnonStorey21D = new _003CSetStepsAtLeast_003Ec__AnonStorey21D();
			_003CSetStepsAtLeast_003Ec__AnonStorey21D.achId = achId;
			_003CSetStepsAtLeast_003Ec__AnonStorey21D.callback = callback;
			_003CSetStepsAtLeast_003Ec__AnonStorey21D._003C_003Ef__this = this;
			Misc.CheckNotNull(_003CSetStepsAtLeast_003Ec__AnonStorey21D.achId);
			_003CSetStepsAtLeast_003Ec__AnonStorey21D.callback = AsOnGameThreadCallback(_003CSetStepsAtLeast_003Ec__AnonStorey21D.callback);
			InitializeGameServices();
			GooglePlayGames.BasicApi.Achievement achievement = GetAchievement(_003CSetStepsAtLeast_003Ec__AnonStorey21D.achId);
			if (achievement == null)
			{
				GooglePlayGames.OurUtils.Logger.e("Could not increment, no achievement with ID " + _003CSetStepsAtLeast_003Ec__AnonStorey21D.achId);
				_003CSetStepsAtLeast_003Ec__AnonStorey21D.callback(false);
			}
			else if (!achievement.IsIncremental)
			{
				GooglePlayGames.OurUtils.Logger.e("Could not increment, achievement with ID " + _003CSetStepsAtLeast_003Ec__AnonStorey21D.achId + " is not incremental");
				_003CSetStepsAtLeast_003Ec__AnonStorey21D.callback(false);
			}
			else if (steps < 0)
			{
				GooglePlayGames.OurUtils.Logger.e("Attempted to increment by negative steps");
				_003CSetStepsAtLeast_003Ec__AnonStorey21D.callback(false);
			}
			else
			{
				GameServices().AchievementManager().SetStepsAtLeast(_003CSetStepsAtLeast_003Ec__AnonStorey21D.achId, Convert.ToUInt32(steps));
				GameServices().AchievementManager().Fetch(_003CSetStepsAtLeast_003Ec__AnonStorey21D.achId, _003CSetStepsAtLeast_003Ec__AnonStorey21D._003C_003Em__AB);
			}
		}

		public void ShowAchievementsUI(Action<UIStatus> cb)
		{
			_003CShowAchievementsUI_003Ec__AnonStorey21E _003CShowAchievementsUI_003Ec__AnonStorey21E = new _003CShowAchievementsUI_003Ec__AnonStorey21E();
			_003CShowAchievementsUI_003Ec__AnonStorey21E.cb = cb;
			if (IsAuthenticated())
			{
				Action<CommonErrorStatus.UIStatus> callback = Callbacks.NoopUICallback;
				if (_003CShowAchievementsUI_003Ec__AnonStorey21E.cb != null)
				{
					callback = _003CShowAchievementsUI_003Ec__AnonStorey21E._003C_003Em__AC;
				}
				callback = AsOnGameThreadCallback(callback);
				GameServices().AchievementManager().ShowAllUI(callback);
			}
		}

		public int LeaderboardMaxResults()
		{
			return GameServices().LeaderboardManager().LeaderboardMaxResults;
		}

		public void ShowLeaderboardUI(string leaderboardId, LeaderboardTimeSpan span, Action<UIStatus> cb)
		{
			_003CShowLeaderboardUI_003Ec__AnonStorey21F _003CShowLeaderboardUI_003Ec__AnonStorey21F = new _003CShowLeaderboardUI_003Ec__AnonStorey21F();
			_003CShowLeaderboardUI_003Ec__AnonStorey21F.cb = cb;
			if (IsAuthenticated())
			{
				Action<CommonErrorStatus.UIStatus> callback = Callbacks.NoopUICallback;
				if (_003CShowLeaderboardUI_003Ec__AnonStorey21F.cb != null)
				{
					callback = _003CShowLeaderboardUI_003Ec__AnonStorey21F._003C_003Em__AD;
				}
				callback = AsOnGameThreadCallback(callback);
				if (leaderboardId == null)
				{
					GameServices().LeaderboardManager().ShowAllUI(callback);
				}
				else
				{
					GameServices().LeaderboardManager().ShowUI(leaderboardId, span, callback);
				}
			}
		}

		public void LoadScores(string leaderboardId, LeaderboardStart start, int rowCount, LeaderboardCollection collection, LeaderboardTimeSpan timeSpan, Action<LeaderboardScoreData> callback)
		{
			callback = AsOnGameThreadCallback(callback);
			GameServices().LeaderboardManager().LoadLeaderboardData(leaderboardId, start, rowCount, collection, timeSpan, mUser.id, callback);
		}

		public void LoadMoreScores(ScorePageToken token, int rowCount, Action<LeaderboardScoreData> callback)
		{
			callback = AsOnGameThreadCallback(callback);
			GameServices().LeaderboardManager().LoadScorePage(null, rowCount, token, callback);
		}

		public void SubmitScore(string leaderboardId, long score, Action<bool> callback)
		{
			callback = AsOnGameThreadCallback(callback);
			if (!IsAuthenticated())
			{
				callback(false);
			}
			InitializeGameServices();
			if (leaderboardId == null)
			{
				throw new ArgumentNullException("leaderboardId");
			}
			GameServices().LeaderboardManager().SubmitScore(leaderboardId, score, null);
			callback(true);
		}

		public void SubmitScore(string leaderboardId, long score, string metadata, Action<bool> callback)
		{
			callback = AsOnGameThreadCallback(callback);
			if (!IsAuthenticated())
			{
				callback(false);
			}
			InitializeGameServices();
			if (leaderboardId == null)
			{
				throw new ArgumentNullException("leaderboardId");
			}
			GameServices().LeaderboardManager().SubmitScore(leaderboardId, score, metadata);
			callback(true);
		}

		public IRealTimeMultiplayerClient GetRtmpClient()
		{
			//Discarded unreachable code: IL_0028
			if (!IsAuthenticated())
			{
				return null;
			}
			lock (GameServicesLock)
			{
				return mRealTimeClient;
			}
		}

		public ITurnBasedMultiplayerClient GetTbmpClient()
		{
			//Discarded unreachable code: IL_001b
			lock (GameServicesLock)
			{
				return mTurnBasedClient;
			}
		}

		public ISavedGameClient GetSavedGameClient()
		{
			//Discarded unreachable code: IL_001b
			lock (GameServicesLock)
			{
				return mSavedGameClient;
			}
		}

		public IEventsClient GetEventsClient()
		{
			//Discarded unreachable code: IL_001b
			lock (GameServicesLock)
			{
				return mEventsClient;
			}
		}

		public IQuestsClient GetQuestsClient()
		{
			//Discarded unreachable code: IL_001b
			lock (GameServicesLock)
			{
				return mQuestsClient;
			}
		}

		public void RegisterInvitationDelegate(InvitationReceivedDelegate invitationDelegate)
		{
			_003CRegisterInvitationDelegate_003Ec__AnonStorey220 _003CRegisterInvitationDelegate_003Ec__AnonStorey = new _003CRegisterInvitationDelegate_003Ec__AnonStorey220();
			_003CRegisterInvitationDelegate_003Ec__AnonStorey.invitationDelegate = invitationDelegate;
			if (_003CRegisterInvitationDelegate_003Ec__AnonStorey.invitationDelegate == null)
			{
				mInvitationDelegate = null;
			}
			else
			{
				mInvitationDelegate = Callbacks.AsOnGameThreadCallback<Invitation, bool>(_003CRegisterInvitationDelegate_003Ec__AnonStorey._003C_003Em__AE);
			}
		}

		public string GetToken()
		{
			if (mTokenClient != null)
			{
				return mTokenClient.GetAccessToken();
			}
			return null;
		}

		public IntPtr GetApiClient()
		{
			return InternalHooks.InternalHooks_GetApiClient(mServices.AsHandle());
		}

		[CompilerGenerated]
		private static void _003CAsOnGameThreadCallback_00601_003Em__92<T>(T P_0)
		{
		}

		[CompilerGenerated]
		private void _003CInitializeGameServices_003Em__95(GooglePlayGames.Native.Cwrapper.Types.MultiplayerEvent eventType, string matchId, NativeTurnBasedMatch match)
		{
			mTurnBasedClient.HandleMatchEvent(eventType, matchId, match);
		}

		[CompilerGenerated]
		private void _003CGetFriends_003Em__9F(bool ok)
		{
			GooglePlayGames.OurUtils.Logger.d("loading: " + ok + " mFriends = " + mFriends);
			if (!ok)
			{
				GooglePlayGames.OurUtils.Logger.e("Friends list did not load successfully.  Disabling loading until re-authenticated");
			}
			friendsLoading = !ok;
		}

		[CompilerGenerated]
		private static bool _003CUnlockAchievement_003Em__A5(GooglePlayGames.BasicApi.Achievement a)
		{
			return a.IsUnlocked;
		}

		[CompilerGenerated]
		private static bool _003CRevealAchievement_003Em__A7(GooglePlayGames.BasicApi.Achievement a)
		{
			return a.IsRevealed;
		}
	}
}
