using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Events;
using GooglePlayGames.BasicApi.Multiplayer;
using GooglePlayGames.BasicApi.Nearby;
using GooglePlayGames.BasicApi.Quests;
using GooglePlayGames.BasicApi.SavedGame;
using GooglePlayGames.OurUtils;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace GooglePlayGames
{
	public class PlayGamesPlatform : ISocialPlatform
	{
		[CompilerGenerated]
		private sealed class _003CInitializeNearby_003Ec__AnonStorey1F8
		{
			internal Action<INearbyConnectionClient> callback;

			internal void _003C_003Em__7B(INearbyConnectionClient client)
			{
				Debug.Log("Nearby Client Created!!");
				sNearbyConnectionClient = client;
				if (callback != null)
				{
					callback(client);
				}
				else
				{
					Debug.Log("Initialize Nearby callback is null");
				}
			}
		}

		[CompilerGenerated]
		private sealed class _003CLoadAchievementDescriptions_003Ec__AnonStorey1F9
		{
			internal Action<IAchievementDescription[]> callback;

			internal void _003C_003Em__7C(Achievement[] ach)
			{
				IAchievementDescription[] array = new IAchievementDescription[ach.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = new PlayGamesAchievement(ach[i]);
				}
				callback(array);
			}
		}

		[CompilerGenerated]
		private sealed class _003CLoadAchievements_003Ec__AnonStorey1FA
		{
			internal Action<IAchievement[]> callback;

			internal void _003C_003Em__7D(Achievement[] ach)
			{
				IAchievement[] array = new IAchievement[ach.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = new PlayGamesAchievement(ach[i]);
				}
				callback(array);
			}
		}

		[CompilerGenerated]
		private sealed class _003CLoadScores_003Ec__AnonStorey1FB
		{
			internal Action<IScore[]> callback;

			internal void _003C_003Em__7E(LeaderboardScoreData scoreData)
			{
				callback(scoreData.Scores);
			}
		}

		[CompilerGenerated]
		private sealed class _003CLoadScores_003Ec__AnonStorey1FC
		{
			internal ILeaderboard board;

			internal Action<bool> callback;

			internal PlayGamesPlatform _003C_003Ef__this;

			internal void _003C_003Em__7F(LeaderboardScoreData scoreData)
			{
				_003C_003Ef__this.HandleLoadingScores((PlayGamesLeaderboard)board, scoreData, callback);
			}
		}

		[CompilerGenerated]
		private sealed class _003CHandleLoadingScores_003Ec__AnonStorey1FD
		{
			internal PlayGamesLeaderboard board;

			internal Action<bool> callback;

			internal PlayGamesPlatform _003C_003Ef__this;

			internal void _003C_003Em__80(LeaderboardScoreData nextScoreData)
			{
				_003C_003Ef__this.HandleLoadingScores(board, nextScoreData, callback);
			}
		}

		private static volatile PlayGamesPlatform sInstance;

		private static volatile bool sNearbyInitializePending;

		private static volatile INearbyConnectionClient sNearbyConnectionClient;

		private readonly PlayGamesClientConfiguration mConfiguration;

		private PlayGamesLocalUser mLocalUser;

		private IPlayGamesClient mClient;

		private string mDefaultLbUi;

		private Dictionary<string, string> mIdMap = new Dictionary<string, string>();

		public static bool DebugLogEnabled
		{
			get
			{
				return GooglePlayGames.OurUtils.Logger.DebugLogEnabled;
			}
			set
			{
				GooglePlayGames.OurUtils.Logger.DebugLogEnabled = value;
			}
		}

		public static PlayGamesPlatform Instance
		{
			get
			{
				if (sInstance == null)
				{
					GooglePlayGames.OurUtils.Logger.d("Instance was not initialized, using default configuration.");
					InitializeInstance(PlayGamesClientConfiguration.DefaultConfiguration);
				}
				return sInstance;
			}
		}

		public static INearbyConnectionClient Nearby
		{
			get
			{
				if (sNearbyConnectionClient == null && !sNearbyInitializePending)
				{
					sNearbyInitializePending = true;
					InitializeNearby(null);
				}
				return sNearbyConnectionClient;
			}
		}

		public IRealTimeMultiplayerClient RealTime
		{
			get
			{
				return mClient.GetRtmpClient();
			}
		}

		public ITurnBasedMultiplayerClient TurnBased
		{
			get
			{
				return mClient.GetTbmpClient();
			}
		}

		public ISavedGameClient SavedGame
		{
			get
			{
				return mClient.GetSavedGameClient();
			}
		}

		public IEventsClient Events
		{
			get
			{
				return mClient.GetEventsClient();
			}
		}

		public IQuestsClient Quests
		{
			get
			{
				return mClient.GetQuestsClient();
			}
		}

		public ILocalUser localUser
		{
			get
			{
				return mLocalUser;
			}
		}

		internal PlayGamesPlatform(IPlayGamesClient client)
		{
			mClient = Misc.CheckNotNull(client);
			mLocalUser = new PlayGamesLocalUser(this);
			mConfiguration = PlayGamesClientConfiguration.DefaultConfiguration;
		}

		private PlayGamesPlatform(PlayGamesClientConfiguration configuration)
		{
			mLocalUser = new PlayGamesLocalUser(this);
			mConfiguration = configuration;
		}

		public static void InitializeInstance(PlayGamesClientConfiguration configuration)
		{
			if (sInstance != null)
			{
				GooglePlayGames.OurUtils.Logger.w("PlayGamesPlatform already initialized. Ignoring this call.");
			}
			else
			{
				sInstance = new PlayGamesPlatform(configuration);
			}
		}

		public static void InitializeNearby(Action<INearbyConnectionClient> callback)
		{
			_003CInitializeNearby_003Ec__AnonStorey1F8 _003CInitializeNearby_003Ec__AnonStorey1F = new _003CInitializeNearby_003Ec__AnonStorey1F8();
			_003CInitializeNearby_003Ec__AnonStorey1F.callback = callback;
			Debug.Log("Calling InitializeNearby!");
			if (sNearbyConnectionClient == null)
			{
				NearbyConnectionClientFactory.Create(_003CInitializeNearby_003Ec__AnonStorey1F._003C_003Em__7B);
			}
			else if (_003CInitializeNearby_003Ec__AnonStorey1F.callback != null)
			{
				Debug.Log("Nearby Already initialized: calling callback directly");
				_003CInitializeNearby_003Ec__AnonStorey1F.callback(sNearbyConnectionClient);
			}
			else
			{
				Debug.Log("Nearby Already initialized");
			}
		}

		public static PlayGamesPlatform Activate()
		{
			GooglePlayGames.OurUtils.Logger.d("Activating PlayGamesPlatform.");
			Social.Active = Instance;
			GooglePlayGames.OurUtils.Logger.d("PlayGamesPlatform activated: " + Social.Active);
			return Instance;
		}

		public IntPtr GetApiClient()
		{
			return mClient.GetApiClient();
		}

		public void AddIdMapping(string fromId, string toId)
		{
			mIdMap[fromId] = toId;
		}

		public void Authenticate(Action<bool> callback)
		{
			Authenticate(callback, false);
		}

		public void Authenticate(Action<bool> callback, bool silent)
		{
			if (mClient == null)
			{
				GooglePlayGames.OurUtils.Logger.d("Creating platform-specific Play Games client.");
				mClient = PlayGamesClientFactory.GetPlatformPlayGamesClient(mConfiguration);
			}
			mClient.Authenticate(callback, silent);
		}

		public void Authenticate(ILocalUser unused, Action<bool> callback)
		{
			Authenticate(callback, false);
		}

		public bool IsAuthenticated()
		{
			return mClient != null && mClient.IsAuthenticated();
		}

		public void SignOut()
		{
			if (mClient != null)
			{
				mClient.SignOut();
			}
			mLocalUser = new PlayGamesLocalUser(this);
		}

		public void LoadUsers(string[] userIds, Action<IUserProfile[]> callback)
		{
			if (!IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("GetUserId() can only be called after authentication.");
				callback(new IUserProfile[0]);
			}
			mClient.LoadUsers(userIds, callback);
		}

		public string GetUserId()
		{
			if (!IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("GetUserId() can only be called after authentication.");
				return "0";
			}
			return mClient.GetUserId();
		}

		public void GetIdToken(Action<string> idTokenCallback)
		{
			if (mClient != null)
			{
				mClient.GetIdToken(idTokenCallback);
				return;
			}
			GooglePlayGames.OurUtils.Logger.e("No client available, calling back with null.");
			idTokenCallback(null);
		}

		public string GetAccessToken()
		{
			if (mClient != null)
			{
				return mClient.GetAccessToken();
			}
			return null;
		}

		public void GetServerAuthCode(Action<CommonStatusCodes, string> callback)
		{
			if (mClient != null && mClient.IsAuthenticated())
			{
				if (GameInfo.WebClientIdInitialized())
				{
					mClient.GetServerAuthCode(string.Empty, callback);
					return;
				}
				GooglePlayGames.OurUtils.Logger.e("GetServerAuthCode requires a webClientId.");
				callback(CommonStatusCodes.DeveloperError, string.Empty);
			}
			else
			{
				GooglePlayGames.OurUtils.Logger.e("GetServerAuthCode can only be called after authentication.");
				callback(CommonStatusCodes.SignInRequired, string.Empty);
			}
		}

		public string GetUserEmail()
		{
			if (mClient != null)
			{
				return mClient.GetUserEmail();
			}
			return null;
		}

		public void GetUserEmail(Action<CommonStatusCodes, string> callback)
		{
			mClient.GetUserEmail(callback);
		}

		public void GetPlayerStats(Action<CommonStatusCodes, PlayerStats> callback)
		{
			if (mClient != null && mClient.IsAuthenticated())
			{
				mClient.GetPlayerStats(callback);
				return;
			}
			GooglePlayGames.OurUtils.Logger.e("GetPlayerStats can only be called after authentication.");
			callback(CommonStatusCodes.SignInRequired, new PlayerStats());
		}

		public Achievement GetAchievement(string achievementId)
		{
			if (!IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("GetAchievement can only be called after authentication.");
				return null;
			}
			return mClient.GetAchievement(achievementId);
		}

		public string GetUserDisplayName()
		{
			if (!IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("GetUserDisplayName can only be called after authentication.");
				return string.Empty;
			}
			return mClient.GetUserDisplayName();
		}

		public string GetUserImageUrl()
		{
			if (!IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("GetUserImageUrl can only be called after authentication.");
				return null;
			}
			return mClient.GetUserImageUrl();
		}

		public void ReportProgress(string achievementID, double progress, Action<bool> callback)
		{
			if (!IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("ReportProgress can only be called after authentication.");
				if (callback != null)
				{
					callback(false);
				}
				return;
			}
			GooglePlayGames.OurUtils.Logger.d("ReportProgress, " + achievementID + ", " + progress);
			achievementID = MapId(achievementID);
			if (progress < 1E-06)
			{
				GooglePlayGames.OurUtils.Logger.d("Progress 0.00 interpreted as request to reveal.");
				mClient.RevealAchievement(achievementID, callback);
				return;
			}
			bool flag = false;
			int num = 0;
			int num2 = 0;
			Achievement achievement = mClient.GetAchievement(achievementID);
			if (achievement == null)
			{
				GooglePlayGames.OurUtils.Logger.w("Unable to locate achievement " + achievementID);
				GooglePlayGames.OurUtils.Logger.w("As a quick fix, assuming it's standard.");
				flag = false;
			}
			else
			{
				flag = achievement.IsIncremental;
				num = achievement.CurrentSteps;
				num2 = achievement.TotalSteps;
				GooglePlayGames.OurUtils.Logger.d("Achievement is " + ((!flag) ? "STANDARD" : "INCREMENTAL"));
				if (flag)
				{
					GooglePlayGames.OurUtils.Logger.d("Current steps: " + num + "/" + num2);
				}
			}
			if (flag)
			{
				GooglePlayGames.OurUtils.Logger.d("Progress " + progress + " interpreted as incremental target (approximate).");
				if (progress >= 0.0 && progress <= 1.0)
				{
					GooglePlayGames.OurUtils.Logger.w("Progress " + progress + " is less than or equal to 1. You might be trying to use values in the range of [0,1], while values are expected to be within the range [0,100]. If you are using the latter, you can safely ignore this message.");
				}
				int num3 = (int)(progress / 100.0 * (double)num2);
				int num4 = num3 - num;
				GooglePlayGames.OurUtils.Logger.d("Target steps: " + num3 + ", cur steps:" + num);
				GooglePlayGames.OurUtils.Logger.d("Steps to increment: " + num4);
				if (num4 >= 0)
				{
					mClient.IncrementAchievement(achievementID, num4, callback);
				}
			}
			else if (progress >= 100.0)
			{
				GooglePlayGames.OurUtils.Logger.d("Progress " + progress + " interpreted as UNLOCK.");
				mClient.UnlockAchievement(achievementID, callback);
			}
			else
			{
				GooglePlayGames.OurUtils.Logger.d("Progress " + progress + " not enough to unlock non-incremental achievement.");
			}
		}

		public void IncrementAchievement(string achievementID, int steps, Action<bool> callback)
		{
			if (!IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("IncrementAchievement can only be called after authentication.");
				if (callback != null)
				{
					callback(false);
				}
			}
			else
			{
				GooglePlayGames.OurUtils.Logger.d("IncrementAchievement: " + achievementID + ", steps " + steps);
				achievementID = MapId(achievementID);
				mClient.IncrementAchievement(achievementID, steps, callback);
			}
		}

		public void SetStepsAtLeast(string achievementID, int steps, Action<bool> callback)
		{
			if (!IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("SetStepsAtLeast can only be called after authentication.");
				if (callback != null)
				{
					callback(false);
				}
			}
			else
			{
				GooglePlayGames.OurUtils.Logger.d("SetStepsAtLeast: " + achievementID + ", steps " + steps);
				achievementID = MapId(achievementID);
				mClient.SetStepsAtLeast(achievementID, steps, callback);
			}
		}

		public void LoadAchievementDescriptions(Action<IAchievementDescription[]> callback)
		{
			_003CLoadAchievementDescriptions_003Ec__AnonStorey1F9 _003CLoadAchievementDescriptions_003Ec__AnonStorey1F = new _003CLoadAchievementDescriptions_003Ec__AnonStorey1F9();
			_003CLoadAchievementDescriptions_003Ec__AnonStorey1F.callback = callback;
			if (!IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("LoadAchievementDescriptions can only be called after authentication.");
				_003CLoadAchievementDescriptions_003Ec__AnonStorey1F.callback(null);
			}
			mClient.LoadAchievements(_003CLoadAchievementDescriptions_003Ec__AnonStorey1F._003C_003Em__7C);
		}

		public void LoadAchievements(Action<IAchievement[]> callback)
		{
			_003CLoadAchievements_003Ec__AnonStorey1FA _003CLoadAchievements_003Ec__AnonStorey1FA = new _003CLoadAchievements_003Ec__AnonStorey1FA();
			_003CLoadAchievements_003Ec__AnonStorey1FA.callback = callback;
			if (!IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("LoadAchievements can only be called after authentication.");
				_003CLoadAchievements_003Ec__AnonStorey1FA.callback(null);
			}
			mClient.LoadAchievements(_003CLoadAchievements_003Ec__AnonStorey1FA._003C_003Em__7D);
		}

		public IAchievement CreateAchievement()
		{
			return new PlayGamesAchievement();
		}

		public void ReportScore(long score, string board, Action<bool> callback)
		{
			if (!IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("ReportScore can only be called after authentication.");
				if (callback != null)
				{
					callback(false);
				}
			}
			else
			{
				GooglePlayGames.OurUtils.Logger.d("ReportScore: score=" + score + ", board=" + board);
				string leaderboardId = MapId(board);
				mClient.SubmitScore(leaderboardId, score, callback);
			}
		}

		public void ReportScore(long score, string board, string metadata, Action<bool> callback)
		{
			if (!IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("ReportScore can only be called after authentication.");
				if (callback != null)
				{
					callback(false);
				}
				return;
			}
			GooglePlayGames.OurUtils.Logger.d("ReportScore: score=" + score + ", board=" + board + " metadata=" + metadata);
			string leaderboardId = MapId(board);
			mClient.SubmitScore(leaderboardId, score, metadata, callback);
		}

		public void LoadScores(string leaderboardId, Action<IScore[]> callback)
		{
			_003CLoadScores_003Ec__AnonStorey1FB _003CLoadScores_003Ec__AnonStorey1FB = new _003CLoadScores_003Ec__AnonStorey1FB();
			_003CLoadScores_003Ec__AnonStorey1FB.callback = callback;
			LoadScores(leaderboardId, LeaderboardStart.PlayerCentered, mClient.LeaderboardMaxResults(), LeaderboardCollection.Public, LeaderboardTimeSpan.AllTime, _003CLoadScores_003Ec__AnonStorey1FB._003C_003Em__7E);
		}

		public void LoadScores(string leaderboardId, LeaderboardStart start, int rowCount, LeaderboardCollection collection, LeaderboardTimeSpan timeSpan, Action<LeaderboardScoreData> callback)
		{
			if (!IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("LoadScores can only be called after authentication.");
				callback(new LeaderboardScoreData(leaderboardId, ResponseStatus.NotAuthorized));
			}
			else
			{
				mClient.LoadScores(leaderboardId, start, rowCount, collection, timeSpan, callback);
			}
		}

		public void LoadMoreScores(ScorePageToken token, int rowCount, Action<LeaderboardScoreData> callback)
		{
			if (!IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("LoadMoreScores can only be called after authentication.");
				callback(new LeaderboardScoreData(token.LeaderboardId, ResponseStatus.NotAuthorized));
			}
			else
			{
				mClient.LoadMoreScores(token, rowCount, callback);
			}
		}

		public ILeaderboard CreateLeaderboard()
		{
			return new PlayGamesLeaderboard(mDefaultLbUi);
		}

		public void ShowAchievementsUI()
		{
			ShowAchievementsUI(null);
		}

		public void ShowAchievementsUI(Action<UIStatus> callback)
		{
			if (!IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("ShowAchievementsUI can only be called after authentication.");
				return;
			}
			GooglePlayGames.OurUtils.Logger.d("ShowAchievementsUI callback is " + callback);
			mClient.ShowAchievementsUI(callback);
		}

		public void ShowLeaderboardUI()
		{
			GooglePlayGames.OurUtils.Logger.d("ShowLeaderboardUI with default ID");
			ShowLeaderboardUI(MapId(mDefaultLbUi), null);
		}

		public void ShowLeaderboardUI(string leaderboardId)
		{
			if (leaderboardId != null)
			{
				leaderboardId = MapId(leaderboardId);
			}
			mClient.ShowLeaderboardUI(leaderboardId, LeaderboardTimeSpan.AllTime, null);
		}

		public void ShowLeaderboardUI(string leaderboardId, Action<UIStatus> callback)
		{
			ShowLeaderboardUI(leaderboardId, LeaderboardTimeSpan.AllTime, callback);
		}

		public void ShowLeaderboardUI(string leaderboardId, LeaderboardTimeSpan span, Action<UIStatus> callback)
		{
			if (!IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("ShowLeaderboardUI can only be called after authentication.");
				callback(UIStatus.NotAuthorized);
				return;
			}
			GooglePlayGames.OurUtils.Logger.d("ShowLeaderboardUI, lbId=" + leaderboardId + " callback is " + callback);
			mClient.ShowLeaderboardUI(leaderboardId, span, callback);
		}

		public void SetDefaultLeaderboardForUI(string lbid)
		{
			GooglePlayGames.OurUtils.Logger.d("SetDefaultLeaderboardForUI: " + lbid);
			if (lbid != null)
			{
				lbid = MapId(lbid);
			}
			mDefaultLbUi = lbid;
		}

		public void LoadFriends(ILocalUser user, Action<bool> callback)
		{
			if (!IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("LoadScores can only be called after authentication.");
				if (callback != null)
				{
					callback(false);
				}
			}
			mClient.LoadFriends(callback);
		}

		public void LoadScores(ILeaderboard board, Action<bool> callback)
		{
			_003CLoadScores_003Ec__AnonStorey1FC _003CLoadScores_003Ec__AnonStorey1FC = new _003CLoadScores_003Ec__AnonStorey1FC();
			_003CLoadScores_003Ec__AnonStorey1FC.board = board;
			_003CLoadScores_003Ec__AnonStorey1FC.callback = callback;
			_003CLoadScores_003Ec__AnonStorey1FC._003C_003Ef__this = this;
			if (!IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("LoadScores can only be called after authentication.");
				if (_003CLoadScores_003Ec__AnonStorey1FC.callback != null)
				{
					_003CLoadScores_003Ec__AnonStorey1FC.callback(false);
				}
			}
			LeaderboardTimeSpan timeSpan;
			switch (_003CLoadScores_003Ec__AnonStorey1FC.board.timeScope)
			{
			case TimeScope.AllTime:
				timeSpan = LeaderboardTimeSpan.AllTime;
				break;
			case TimeScope.Week:
				timeSpan = LeaderboardTimeSpan.Weekly;
				break;
			case TimeScope.Today:
				timeSpan = LeaderboardTimeSpan.Daily;
				break;
			default:
				timeSpan = LeaderboardTimeSpan.AllTime;
				break;
			}
			((PlayGamesLeaderboard)_003CLoadScores_003Ec__AnonStorey1FC.board).loading = true;
			GooglePlayGames.OurUtils.Logger.d(string.Concat("LoadScores, board=", _003CLoadScores_003Ec__AnonStorey1FC.board, " callback is ", _003CLoadScores_003Ec__AnonStorey1FC.callback));
			mClient.LoadScores(_003CLoadScores_003Ec__AnonStorey1FC.board.id, LeaderboardStart.PlayerCentered, (_003CLoadScores_003Ec__AnonStorey1FC.board.range.count <= 0) ? mClient.LeaderboardMaxResults() : _003CLoadScores_003Ec__AnonStorey1FC.board.range.count, (_003CLoadScores_003Ec__AnonStorey1FC.board.userScope != UserScope.FriendsOnly) ? LeaderboardCollection.Public : LeaderboardCollection.Social, timeSpan, _003CLoadScores_003Ec__AnonStorey1FC._003C_003Em__7F);
		}

		public bool GetLoading(ILeaderboard board)
		{
			return board != null && board.loading;
		}

		public void RegisterInvitationDelegate(InvitationReceivedDelegate deleg)
		{
			mClient.RegisterInvitationDelegate(deleg);
		}

		public string GetToken()
		{
			return mClient.GetToken();
		}

		internal void HandleLoadingScores(PlayGamesLeaderboard board, LeaderboardScoreData scoreData, Action<bool> callback)
		{
			_003CHandleLoadingScores_003Ec__AnonStorey1FD _003CHandleLoadingScores_003Ec__AnonStorey1FD = new _003CHandleLoadingScores_003Ec__AnonStorey1FD();
			_003CHandleLoadingScores_003Ec__AnonStorey1FD.board = board;
			_003CHandleLoadingScores_003Ec__AnonStorey1FD.callback = callback;
			_003CHandleLoadingScores_003Ec__AnonStorey1FD._003C_003Ef__this = this;
			bool flag = _003CHandleLoadingScores_003Ec__AnonStorey1FD.board.SetFromData(scoreData);
			if (flag && !_003CHandleLoadingScores_003Ec__AnonStorey1FD.board.HasAllScores() && scoreData.NextPageToken != null)
			{
				int rowCount = _003CHandleLoadingScores_003Ec__AnonStorey1FD.board.range.count - _003CHandleLoadingScores_003Ec__AnonStorey1FD.board.ScoreCount;
				mClient.LoadMoreScores(scoreData.NextPageToken, rowCount, _003CHandleLoadingScores_003Ec__AnonStorey1FD._003C_003Em__80);
			}
			else
			{
				_003CHandleLoadingScores_003Ec__AnonStorey1FD.callback(flag);
			}
		}

		internal IUserProfile[] GetFriends()
		{
			if (!IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.d("Cannot get friends when not authenticated!");
				return new IUserProfile[0];
			}
			return mClient.GetFriends();
		}

		private string MapId(string id)
		{
			if (id == null)
			{
				return null;
			}
			if (mIdMap.ContainsKey(id))
			{
				string text = mIdMap[id];
				GooglePlayGames.OurUtils.Logger.d("Mapping alias " + id + " to ID " + text);
				return text;
			}
			return id;
		}
	}
}
