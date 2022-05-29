using GooglePlayGames.BasicApi.Events;
using GooglePlayGames.BasicApi.Multiplayer;
using GooglePlayGames.BasicApi.Quests;
using GooglePlayGames.BasicApi.SavedGame;
using GooglePlayGames.OurUtils;
using System;
using UnityEngine.SocialPlatforms;

namespace GooglePlayGames.BasicApi
{
	public class DummyClient : IPlayGamesClient
	{
		public DummyClient()
		{
		}

		public void Authenticate(Action<bool> callback, bool silent)
		{
			DummyClient.LogUsage();
			if (callback != null)
			{
				callback(false);
			}
		}

		public string GetAccessToken()
		{
			DummyClient.LogUsage();
			return "DummyAccessToken";
		}

		public Achievement GetAchievement(string achId)
		{
			DummyClient.LogUsage();
			return null;
		}

		public IntPtr GetApiClient()
		{
			DummyClient.LogUsage();
			return IntPtr.Zero;
		}

		public IEventsClient GetEventsClient()
		{
			DummyClient.LogUsage();
			return null;
		}

		public IUserProfile[] GetFriends()
		{
			DummyClient.LogUsage();
			return new IUserProfile[0];
		}

		public void GetIdToken(Action<string> idTokenCallback)
		{
			DummyClient.LogUsage();
			if (idTokenCallback != null)
			{
				idTokenCallback("DummyIdToken");
			}
		}

		public Invitation GetInvitationFromNotification()
		{
			DummyClient.LogUsage();
			return null;
		}

		public void GetPlayerStats(Action<CommonStatusCodes, PlayerStats> callback)
		{
			DummyClient.LogUsage();
			callback(17, new PlayerStats());
		}

		public IQuestsClient GetQuestsClient()
		{
			DummyClient.LogUsage();
			return null;
		}

		public IRealTimeMultiplayerClient GetRtmpClient()
		{
			DummyClient.LogUsage();
			return null;
		}

		public ISavedGameClient GetSavedGameClient()
		{
			DummyClient.LogUsage();
			return null;
		}

		public void GetServerAuthCode(string serverClientId, Action<CommonStatusCodes, string> callback)
		{
			DummyClient.LogUsage();
			if (callback != null)
			{
				callback(17, "DummyServerAuthCode");
			}
		}

		public ITurnBasedMultiplayerClient GetTbmpClient()
		{
			DummyClient.LogUsage();
			return null;
		}

		public string GetToken()
		{
			return "DummyToken";
		}

		public string GetUserDisplayName()
		{
			DummyClient.LogUsage();
			return "Player";
		}

		public string GetUserEmail()
		{
			return string.Empty;
		}

		public void GetUserEmail(Action<CommonStatusCodes, string> callback)
		{
			DummyClient.LogUsage();
			if (callback != null)
			{
				callback(17, null);
			}
		}

		public string GetUserId()
		{
			DummyClient.LogUsage();
			return "DummyID";
		}

		public string GetUserImageUrl()
		{
			DummyClient.LogUsage();
			return null;
		}

		public bool HasInvitationFromNotification()
		{
			DummyClient.LogUsage();
			return false;
		}

		public void IncrementAchievement(string achId, int steps, Action<bool> callback)
		{
			DummyClient.LogUsage();
			if (callback != null)
			{
				callback(false);
			}
		}

		public bool IsAuthenticated()
		{
			DummyClient.LogUsage();
			return false;
		}

		public int LeaderboardMaxResults()
		{
			return 25;
		}

		public void LoadAchievements(Action<Achievement[]> callback)
		{
			DummyClient.LogUsage();
			if (callback != null)
			{
				callback(null);
			}
		}

		public void LoadFriends(Action<bool> callback)
		{
			DummyClient.LogUsage();
			callback(false);
		}

		public void LoadMoreScores(ScorePageToken token, int rowCount, Action<LeaderboardScoreData> callback)
		{
			DummyClient.LogUsage();
			if (callback != null)
			{
				callback(new LeaderboardScoreData(token.LeaderboardId, ResponseStatus.LicenseCheckFailed));
			}
		}

		public void LoadScores(string leaderboardId, LeaderboardStart start, int rowCount, LeaderboardCollection collection, LeaderboardTimeSpan timeSpan, Action<LeaderboardScoreData> callback)
		{
			DummyClient.LogUsage();
			if (callback != null)
			{
				callback(new LeaderboardScoreData(leaderboardId, ResponseStatus.LicenseCheckFailed));
			}
		}

		public void LoadUsers(string[] userIds, Action<IUserProfile[]> callback)
		{
			DummyClient.LogUsage();
			if (callback != null)
			{
				callback(null);
			}
		}

		private static void LogUsage()
		{
			Logger.d("Received method call on DummyClient - using stub implementation.");
		}

		public void RegisterInvitationDelegate(InvitationReceivedDelegate invitationDelegate)
		{
			DummyClient.LogUsage();
		}

		public void RevealAchievement(string achId, Action<bool> callback)
		{
			DummyClient.LogUsage();
			if (callback != null)
			{
				callback(false);
			}
		}

		public void SetStepsAtLeast(string achId, int steps, Action<bool> callback)
		{
			DummyClient.LogUsage();
			if (callback != null)
			{
				callback(false);
			}
		}

		public void ShowAchievementsUI(Action<UIStatus> callback)
		{
			DummyClient.LogUsage();
			if (callback != null)
			{
				callback(-4);
			}
		}

		public void ShowLeaderboardUI(string leaderboardId, LeaderboardTimeSpan span, Action<UIStatus> callback)
		{
			DummyClient.LogUsage();
			if (callback != null)
			{
				callback(-4);
			}
		}

		public void SignOut()
		{
			DummyClient.LogUsage();
		}

		public void SubmitScore(string leaderboardId, long score, Action<bool> callback)
		{
			DummyClient.LogUsage();
			if (callback != null)
			{
				callback(false);
			}
		}

		public void SubmitScore(string leaderboardId, long score, string metadata, Action<bool> callback)
		{
			DummyClient.LogUsage();
			if (callback != null)
			{
				callback(false);
			}
		}

		public void UnlockAchievement(string achId, Action<bool> callback)
		{
			DummyClient.LogUsage();
			if (callback != null)
			{
				callback(false);
			}
		}
	}
}