using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Events;
using GooglePlayGames.BasicApi.Multiplayer;
using GooglePlayGames.BasicApi.Nearby;
using GooglePlayGames.BasicApi.Quests;
using GooglePlayGames.BasicApi.SavedGame;
using GooglePlayGames.OurUtils;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace GooglePlayGames
{
	public class PlayGamesPlatform : ISocialPlatform
	{
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

		public IEventsClient Events
		{
			get
			{
				return this.mClient.GetEventsClient();
			}
		}

		public static PlayGamesPlatform Instance
		{
			get
			{
				if (PlayGamesPlatform.sInstance == null)
				{
					GooglePlayGames.OurUtils.Logger.d("Instance was not initialized, using default configuration.");
					PlayGamesPlatform.InitializeInstance(PlayGamesClientConfiguration.DefaultConfiguration);
				}
				return PlayGamesPlatform.sInstance;
			}
		}

		public ILocalUser localUser
		{
			get
			{
				return this.mLocalUser;
			}
		}

		public static INearbyConnectionClient Nearby
		{
			get
			{
				if (PlayGamesPlatform.sNearbyConnectionClient == null && !PlayGamesPlatform.sNearbyInitializePending)
				{
					PlayGamesPlatform.sNearbyInitializePending = true;
					PlayGamesPlatform.InitializeNearby(null);
				}
				return PlayGamesPlatform.sNearbyConnectionClient;
			}
		}

		public IQuestsClient Quests
		{
			get
			{
				return this.mClient.GetQuestsClient();
			}
		}

		public IRealTimeMultiplayerClient RealTime
		{
			get
			{
				return this.mClient.GetRtmpClient();
			}
		}

		public ISavedGameClient SavedGame
		{
			get
			{
				return this.mClient.GetSavedGameClient();
			}
		}

		public ITurnBasedMultiplayerClient TurnBased
		{
			get
			{
				return this.mClient.GetTbmpClient();
			}
		}

		static PlayGamesPlatform()
		{
		}

		internal PlayGamesPlatform(IPlayGamesClient client)
		{
			this.mClient = Misc.CheckNotNull<IPlayGamesClient>(client);
			this.mLocalUser = new PlayGamesLocalUser(this);
			this.mConfiguration = PlayGamesClientConfiguration.DefaultConfiguration;
		}

		private PlayGamesPlatform(PlayGamesClientConfiguration configuration)
		{
			this.mLocalUser = new PlayGamesLocalUser(this);
			this.mConfiguration = configuration;
		}

		public static PlayGamesPlatform Activate()
		{
			GooglePlayGames.OurUtils.Logger.d("Activating PlayGamesPlatform.");
			Social.Active = PlayGamesPlatform.Instance;
			GooglePlayGames.OurUtils.Logger.d(string.Concat("PlayGamesPlatform activated: ", Social.Active));
			return PlayGamesPlatform.Instance;
		}

		public void AddIdMapping(string fromId, string toId)
		{
			this.mIdMap[fromId] = toId;
		}

		public void Authenticate(Action<bool> callback)
		{
			this.Authenticate(callback, false);
		}

		public void Authenticate(Action<bool> callback, bool silent)
		{
			if (this.mClient == null)
			{
				GooglePlayGames.OurUtils.Logger.d("Creating platform-specific Play Games client.");
				this.mClient = PlayGamesClientFactory.GetPlatformPlayGamesClient(this.mConfiguration);
			}
			this.mClient.Authenticate(callback, silent);
		}

		public void Authenticate(ILocalUser unused, Action<bool> callback)
		{
			this.Authenticate(callback, false);
		}

		public IAchievement CreateAchievement()
		{
			return new PlayGamesAchievement();
		}

		public ILeaderboard CreateLeaderboard()
		{
			return new PlayGamesLeaderboard(this.mDefaultLbUi);
		}

		public string GetAccessToken()
		{
			if (this.mClient == null)
			{
				return null;
			}
			return this.mClient.GetAccessToken();
		}

		public Achievement GetAchievement(string achievementId)
		{
			if (!this.IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("GetAchievement can only be called after authentication.");
				return null;
			}
			return this.mClient.GetAchievement(achievementId);
		}

		public IntPtr GetApiClient()
		{
			return this.mClient.GetApiClient();
		}

		internal IUserProfile[] GetFriends()
		{
			if (this.IsAuthenticated())
			{
				return this.mClient.GetFriends();
			}
			GooglePlayGames.OurUtils.Logger.d("Cannot get friends when not authenticated!");
			return new IUserProfile[0];
		}

		public void GetIdToken(Action<string> idTokenCallback)
		{
			if (this.mClient == null)
			{
				GooglePlayGames.OurUtils.Logger.e("No client available, calling back with null.");
				idTokenCallback(null);
			}
			else
			{
				this.mClient.GetIdToken(idTokenCallback);
			}
		}

		public bool GetLoading(ILeaderboard board)
		{
			return (board == null ? false : board.loading);
		}

		public void GetPlayerStats(Action<CommonStatusCodes, PlayerStats> callback)
		{
			if (this.mClient == null || !this.mClient.IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("GetPlayerStats can only be called after authentication.");
				callback(4, new PlayerStats());
			}
			else
			{
				this.mClient.GetPlayerStats(callback);
			}
		}

		public void GetServerAuthCode(Action<CommonStatusCodes, string> callback)
		{
			if (this.mClient == null || !this.mClient.IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("GetServerAuthCode can only be called after authentication.");
				callback(4, string.Empty);
			}
			else if (!GameInfo.WebClientIdInitialized())
			{
				GooglePlayGames.OurUtils.Logger.e("GetServerAuthCode requires a webClientId.");
				callback(10, string.Empty);
			}
			else
			{
				this.mClient.GetServerAuthCode(string.Empty, callback);
			}
		}

		public string GetToken()
		{
			return this.mClient.GetToken();
		}

		public string GetUserDisplayName()
		{
			if (!this.IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("GetUserDisplayName can only be called after authentication.");
				return string.Empty;
			}
			return this.mClient.GetUserDisplayName();
		}

		public string GetUserEmail()
		{
			if (this.mClient == null)
			{
				return null;
			}
			return this.mClient.GetUserEmail();
		}

		public void GetUserEmail(Action<CommonStatusCodes, string> callback)
		{
			this.mClient.GetUserEmail(callback);
		}

		public string GetUserId()
		{
			if (!this.IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("GetUserId() can only be called after authentication.");
				return "0";
			}
			return this.mClient.GetUserId();
		}

		public string GetUserImageUrl()
		{
			if (!this.IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("GetUserImageUrl can only be called after authentication.");
				return null;
			}
			return this.mClient.GetUserImageUrl();
		}

		internal void HandleLoadingScores(PlayGamesLeaderboard board, LeaderboardScoreData scoreData, Action<bool> callback)
		{
			bool flag = board.SetFromData(scoreData);
			if (!flag || board.HasAllScores() || scoreData.NextPageToken == null)
			{
				callback(flag);
			}
			else
			{
				int num = board.range.count - board.ScoreCount;
				this.mClient.LoadMoreScores(scoreData.NextPageToken, num, (LeaderboardScoreData nextScoreData) => this.HandleLoadingScores(board, nextScoreData, callback));
			}
		}

		public void IncrementAchievement(string achievementID, int steps, Action<bool> callback)
		{
			if (!this.IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("IncrementAchievement can only be called after authentication.");
				if (callback != null)
				{
					callback(false);
				}
				return;
			}
			GooglePlayGames.OurUtils.Logger.d(string.Concat(new object[] { "IncrementAchievement: ", achievementID, ", steps ", steps }));
			achievementID = this.MapId(achievementID);
			this.mClient.IncrementAchievement(achievementID, steps, callback);
		}

		public static void InitializeInstance(PlayGamesClientConfiguration configuration)
		{
			if (PlayGamesPlatform.sInstance != null)
			{
				GooglePlayGames.OurUtils.Logger.w("PlayGamesPlatform already initialized. Ignoring this call.");
				return;
			}
			PlayGamesPlatform.sInstance = new PlayGamesPlatform(configuration);
		}

		public static void InitializeNearby(Action<INearbyConnectionClient> callback)
		{
			Debug.Log("Calling InitializeNearby!");
			if (PlayGamesPlatform.sNearbyConnectionClient == null)
			{
				NearbyConnectionClientFactory.Create((INearbyConnectionClient client) => {
					Debug.Log("Nearby Client Created!!");
					PlayGamesPlatform.sNearbyConnectionClient = client;
					if (callback == null)
					{
						Debug.Log("Initialize Nearby callback is null");
					}
					else
					{
						callback(client);
					}
				});
			}
			else if (callback == null)
			{
				Debug.Log("Nearby Already initialized");
			}
			else
			{
				Debug.Log("Nearby Already initialized: calling callback directly");
				callback(PlayGamesPlatform.sNearbyConnectionClient);
			}
		}

		public bool IsAuthenticated()
		{
			return (this.mClient == null ? false : this.mClient.IsAuthenticated());
		}

		public void LoadAchievementDescriptions(Action<IAchievementDescription[]> callback)
		{
			if (!this.IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("LoadAchievementDescriptions can only be called after authentication.");
				callback(null);
			}
			this.mClient.LoadAchievements((Achievement[] ach) => {
				IAchievementDescription[] playGamesAchievement = new IAchievementDescription[(int)ach.Length];
				for (int i = 0; i < (int)playGamesAchievement.Length; i++)
				{
					playGamesAchievement[i] = new PlayGamesAchievement(ach[i]);
				}
				callback(playGamesAchievement);
			});
		}

		public void LoadAchievements(Action<IAchievement[]> callback)
		{
			if (!this.IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("LoadAchievements can only be called after authentication.");
				callback(null);
			}
			this.mClient.LoadAchievements((Achievement[] ach) => {
				IAchievement[] playGamesAchievement = new IAchievement[(int)ach.Length];
				for (int i = 0; i < (int)playGamesAchievement.Length; i++)
				{
					playGamesAchievement[i] = new PlayGamesAchievement(ach[i]);
				}
				callback(playGamesAchievement);
			});
		}

		public void LoadFriends(ILocalUser user, Action<bool> callback)
		{
			if (!this.IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("LoadScores can only be called after authentication.");
				if (callback != null)
				{
					callback(false);
				}
			}
			this.mClient.LoadFriends(callback);
		}

		public void LoadMoreScores(ScorePageToken token, int rowCount, Action<LeaderboardScoreData> callback)
		{
			if (this.IsAuthenticated())
			{
				this.mClient.LoadMoreScores(token, rowCount, callback);
				return;
			}
			GooglePlayGames.OurUtils.Logger.e("LoadMoreScores can only be called after authentication.");
			callback(new LeaderboardScoreData(token.LeaderboardId, ResponseStatus.NotAuthorized));
		}

		public void LoadScores(string leaderboardId, Action<IScore[]> callback)
		{
			this.LoadScores(leaderboardId, LeaderboardStart.PlayerCentered, this.mClient.LeaderboardMaxResults(), LeaderboardCollection.Public, LeaderboardTimeSpan.AllTime, (LeaderboardScoreData scoreData) => callback(scoreData.Scores));
		}

		public void LoadScores(string leaderboardId, LeaderboardStart start, int rowCount, LeaderboardCollection collection, LeaderboardTimeSpan timeSpan, Action<LeaderboardScoreData> callback)
		{
			if (!this.IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("LoadScores can only be called after authentication.");
				callback(new LeaderboardScoreData(leaderboardId, ResponseStatus.NotAuthorized));
				return;
			}
			this.mClient.LoadScores(leaderboardId, start, rowCount, collection, timeSpan, callback);
		}

		public void LoadScores(ILeaderboard board, Action<bool> callback)
		{
			LeaderboardTimeSpan leaderboardTimeSpan;
			if (!this.IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("LoadScores can only be called after authentication.");
				if (callback != null)
				{
					callback(false);
				}
			}
			switch (board.timeScope)
			{
				case TimeScope.Today:
				{
					leaderboardTimeSpan = LeaderboardTimeSpan.Daily;
					break;
				}
				case TimeScope.Week:
				{
					leaderboardTimeSpan = LeaderboardTimeSpan.Weekly;
					break;
				}
				case TimeScope.AllTime:
				{
					leaderboardTimeSpan = LeaderboardTimeSpan.AllTime;
					break;
				}
				default:
				{
					leaderboardTimeSpan = LeaderboardTimeSpan.AllTime;
					break;
				}
			}
			((PlayGamesLeaderboard)board).loading = true;
			GooglePlayGames.OurUtils.Logger.d(string.Concat(new object[] { "LoadScores, board=", board, " callback is ", callback }));
			this.mClient.LoadScores(board.id, LeaderboardStart.PlayerCentered, (board.range.count <= 0 ? this.mClient.LeaderboardMaxResults() : board.range.count), (board.userScope != UserScope.FriendsOnly ? LeaderboardCollection.Public : LeaderboardCollection.Social), leaderboardTimeSpan, (LeaderboardScoreData scoreData) => this.HandleLoadingScores((PlayGamesLeaderboard)board, scoreData, callback));
		}

		public void LoadUsers(string[] userIds, Action<IUserProfile[]> callback)
		{
			if (!this.IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("GetUserId() can only be called after authentication.");
				callback(new IUserProfile[0]);
			}
			this.mClient.LoadUsers(userIds, callback);
		}

		private string MapId(string id)
		{
			if (id == null)
			{
				return null;
			}
			if (!this.mIdMap.ContainsKey(id))
			{
				return id;
			}
			string item = this.mIdMap[id];
			GooglePlayGames.OurUtils.Logger.d(string.Concat("Mapping alias ", id, " to ID ", item));
			return item;
		}

		public void RegisterInvitationDelegate(InvitationReceivedDelegate deleg)
		{
			this.mClient.RegisterInvitationDelegate(deleg);
		}

		public void ReportProgress(string achievementID, double progress, Action<bool> callback)
		{
			if (!this.IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("ReportProgress can only be called after authentication.");
				if (callback != null)
				{
					callback(false);
				}
				return;
			}
			GooglePlayGames.OurUtils.Logger.d(string.Concat(new object[] { "ReportProgress, ", achievementID, ", ", progress }));
			achievementID = this.MapId(achievementID);
			if (progress < 1E-06)
			{
				GooglePlayGames.OurUtils.Logger.d("Progress 0.00 interpreted as request to reveal.");
				this.mClient.RevealAchievement(achievementID, callback);
				return;
			}
			bool isIncremental = false;
			int currentSteps = 0;
			int totalSteps = 0;
			Achievement achievement = this.mClient.GetAchievement(achievementID);
			if (achievement != null)
			{
				isIncremental = achievement.IsIncremental;
				currentSteps = achievement.CurrentSteps;
				totalSteps = achievement.TotalSteps;
				GooglePlayGames.OurUtils.Logger.d(string.Concat("Achievement is ", (!isIncremental ? "STANDARD" : "INCREMENTAL")));
				if (isIncremental)
				{
					GooglePlayGames.OurUtils.Logger.d(string.Concat(new object[] { "Current steps: ", currentSteps, "/", totalSteps }));
				}
			}
			else
			{
				GooglePlayGames.OurUtils.Logger.w(string.Concat("Unable to locate achievement ", achievementID));
				GooglePlayGames.OurUtils.Logger.w("As a quick fix, assuming it's standard.");
				isIncremental = false;
			}
			if (isIncremental)
			{
				GooglePlayGames.OurUtils.Logger.d(string.Concat("Progress ", progress, " interpreted as incremental target (approximate)."));
				if (progress >= 0 && progress <= 1)
				{
					GooglePlayGames.OurUtils.Logger.w(string.Concat("Progress ", progress, " is less than or equal to 1. You might be trying to use values in the range of [0,1], while values are expected to be within the range [0,100]. If you are using the latter, you can safely ignore this message."));
				}
				int num = (int)(progress / 100 * (double)totalSteps);
				int num1 = num - currentSteps;
				GooglePlayGames.OurUtils.Logger.d(string.Concat(new object[] { "Target steps: ", num, ", cur steps:", currentSteps }));
				GooglePlayGames.OurUtils.Logger.d(string.Concat("Steps to increment: ", num1));
				if (num1 >= 0)
				{
					this.mClient.IncrementAchievement(achievementID, num1, callback);
				}
			}
			else if (progress < 100)
			{
				GooglePlayGames.OurUtils.Logger.d(string.Concat("Progress ", progress, " not enough to unlock non-incremental achievement."));
			}
			else
			{
				GooglePlayGames.OurUtils.Logger.d(string.Concat("Progress ", progress, " interpreted as UNLOCK."));
				this.mClient.UnlockAchievement(achievementID, callback);
			}
		}

		public void ReportScore(long score, string board, Action<bool> callback)
		{
			if (!this.IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("ReportScore can only be called after authentication.");
				if (callback != null)
				{
					callback(false);
				}
				return;
			}
			GooglePlayGames.OurUtils.Logger.d(string.Concat(new object[] { "ReportScore: score=", score, ", board=", board }));
			string str = this.MapId(board);
			this.mClient.SubmitScore(str, score, callback);
		}

		public void ReportScore(long score, string board, string metadata, Action<bool> callback)
		{
			if (!this.IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("ReportScore can only be called after authentication.");
				if (callback != null)
				{
					callback(false);
				}
				return;
			}
			GooglePlayGames.OurUtils.Logger.d(string.Concat(new object[] { "ReportScore: score=", score, ", board=", board, " metadata=", metadata }));
			string str = this.MapId(board);
			this.mClient.SubmitScore(str, score, metadata, callback);
		}

		public void SetDefaultLeaderboardForUI(string lbid)
		{
			GooglePlayGames.OurUtils.Logger.d(string.Concat("SetDefaultLeaderboardForUI: ", lbid));
			if (lbid != null)
			{
				lbid = this.MapId(lbid);
			}
			this.mDefaultLbUi = lbid;
		}

		public void SetStepsAtLeast(string achievementID, int steps, Action<bool> callback)
		{
			if (!this.IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("SetStepsAtLeast can only be called after authentication.");
				if (callback != null)
				{
					callback(false);
				}
				return;
			}
			GooglePlayGames.OurUtils.Logger.d(string.Concat(new object[] { "SetStepsAtLeast: ", achievementID, ", steps ", steps }));
			achievementID = this.MapId(achievementID);
			this.mClient.SetStepsAtLeast(achievementID, steps, callback);
		}

		public void ShowAchievementsUI()
		{
			this.ShowAchievementsUI(null);
		}

		public void ShowAchievementsUI(Action<UIStatus> callback)
		{
			if (!this.IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("ShowAchievementsUI can only be called after authentication.");
				return;
			}
			GooglePlayGames.OurUtils.Logger.d(string.Concat("ShowAchievementsUI callback is ", callback));
			this.mClient.ShowAchievementsUI(callback);
		}

		public void ShowLeaderboardUI()
		{
			GooglePlayGames.OurUtils.Logger.d("ShowLeaderboardUI with default ID");
			this.ShowLeaderboardUI(this.MapId(this.mDefaultLbUi), null);
		}

		public void ShowLeaderboardUI(string leaderboardId)
		{
			if (leaderboardId != null)
			{
				leaderboardId = this.MapId(leaderboardId);
			}
			this.mClient.ShowLeaderboardUI(leaderboardId, LeaderboardTimeSpan.AllTime, null);
		}

		public void ShowLeaderboardUI(string leaderboardId, Action<UIStatus> callback)
		{
			this.ShowLeaderboardUI(leaderboardId, LeaderboardTimeSpan.AllTime, callback);
		}

		public void ShowLeaderboardUI(string leaderboardId, LeaderboardTimeSpan span, Action<UIStatus> callback)
		{
			if (!this.IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("ShowLeaderboardUI can only be called after authentication.");
				callback(-3);
				return;
			}
			GooglePlayGames.OurUtils.Logger.d(string.Concat(new object[] { "ShowLeaderboardUI, lbId=", leaderboardId, " callback is ", callback }));
			this.mClient.ShowLeaderboardUI(leaderboardId, span, callback);
		}

		public void SignOut()
		{
			if (this.mClient != null)
			{
				this.mClient.SignOut();
			}
			this.mLocalUser = new PlayGamesLocalUser(this);
		}
	}
}