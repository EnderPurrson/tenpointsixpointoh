using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class GameCircleSocial : ISocialPlatform
{
	private AGSSocialLocalUser gameCircleLocalUser = new AGSSocialLocalUser();

	private int requestID;

	private Action<bool> authenticationCallback;

	private Dictionary<int, Action<bool>> simpleCallbacks;

	private Dictionary<int, Action<IAchievementDescription[]>> loadAchievementDescriptionsCallbacks;

	private Dictionary<int, Action<IAchievement[]>> loadAchievementsCallbacks;

	private Dictionary<int, AGSSocialLeaderboard> leaderboardForRequest;

	private Dictionary<int, Action<IScore[]>> loadScoresCallbacks;

	private static GameCircleSocial socialInstance;

	public static GameCircleSocial Instance
	{
		get
		{
			return GameCircleSocial.socialInstance;
		}
	}

	public ILocalUser localUser
	{
		get
		{
			return this.gameCircleLocalUser;
		}
	}

	static GameCircleSocial()
	{
		GameCircleSocial.socialInstance = new GameCircleSocial();
	}

	private GameCircleSocial()
	{
		this.requestID = 1;
		this.simpleCallbacks = new Dictionary<int, Action<bool>>();
		this.loadAchievementDescriptionsCallbacks = new Dictionary<int, Action<IAchievementDescription[]>>();
		this.loadAchievementsCallbacks = new Dictionary<int, Action<IAchievement[]>>();
		this.leaderboardForRequest = new Dictionary<int, AGSSocialLeaderboard>();
		this.loadScoresCallbacks = new Dictionary<int, Action<IScore[]>>();
		AGSClient.ServiceReadyEvent += new Action(this.OnServiceReady);
		AGSClient.ServiceNotReadyEvent += new Action<string>(this.OnServiceNotReady);
		AGSAchievementsClient.UpdateAchievementCompleted += new Action<AGSUpdateAchievementResponse>(this.OnUpdateAchievementCompleted);
		AGSAchievementsClient.RequestAchievementsCompleted += new Action<AGSRequestAchievementsResponse>(this.OnRequestAchievementsCompleted);
		AGSLeaderboardsClient.SubmitScoreCompleted += new Action<AGSSubmitScoreResponse>(this.OnSubmitScoreCompleted);
		AGSLeaderboardsClient.RequestScoresCompleted += new Action<AGSRequestScoresResponse>(this.OnRequestScoresCompleted);
		AGSLeaderboardsClient.RequestLocalPlayerScoreCompleted += new Action<AGSRequestScoreResponse>(this.OnRequestLocalPlayerScoreCompleted);
		AGSPlayerClient.RequestLocalPlayerCompleted += new Action<AGSRequestPlayerResponse>(this.OnRequestPlayerCompleted);
		AGSPlayerClient.RequestFriendIdsCompleted += new Action<AGSRequestFriendIdsResponse>(this.OnRequestFriendIdsCompleted);
		AGSPlayerClient.RequestBatchFriendsCompleted += new Action<AGSRequestBatchFriendsResponse>(this.OnRequestBatchFriendsCompleted);
	}

	public void Authenticate(ILocalUser user, Action<bool> callback)
	{
		this.authenticationCallback = callback;
		Debug.Log("[Rilisoft] GameCircleSocial.Authenticate(user, callback) > AGSClient.Init(true, true, true).");
		AGSClient.Init(true, true, true);
	}

	public IAchievement CreateAchievement()
	{
		return new AGSSocialAchievement();
	}

	public ILeaderboard CreateLeaderboard()
	{
		return new AGSSocialLeaderboard();
	}

	private LeaderboardScope fromTimeScope(TimeScope scope)
	{
		switch (scope)
		{
			case TimeScope.Today:
			{
				return LeaderboardScope.GlobalDay;
			}
			case TimeScope.Week:
			{
				return LeaderboardScope.GlobalWeek;
			}
			case TimeScope.AllTime:
			{
				return LeaderboardScope.GlobalAllTime;
			}
		}
		return LeaderboardScope.GlobalAllTime;
	}

	public bool GetLoading(ILeaderboard board)
	{
		if (board != null)
		{
			return board.loading;
		}
		AGSClient.LogGameCircleError("GetLoading \"board\" argument should not be null");
		return false;
	}

	public void LoadAchievementDescriptions(Action<IAchievementDescription[]> callback)
	{
		if (callback == null)
		{
			AGSClient.LogGameCircleError("LoadAchievementDescriptions \"callback\" argument should not be null");
			return;
		}
		this.loadAchievementDescriptionsCallbacks.Add(this.requestID, callback);
		GameCircleSocial gameCircleSocial = this;
		int num = gameCircleSocial.requestID;
		int num1 = num;
		gameCircleSocial.requestID = num + 1;
		AGSAchievementsClient.RequestAchievements(num1);
	}

	public void LoadAchievements(Action<IAchievement[]> callback)
	{
		if (callback == null)
		{
			AGSClient.LogGameCircleError("LoadAchievements \"callback\" argument should not be null");
			return;
		}
		this.loadAchievementsCallbacks.Add(this.requestID, callback);
		GameCircleSocial gameCircleSocial = this;
		int num = gameCircleSocial.requestID;
		int num1 = num;
		gameCircleSocial.requestID = num + 1;
		AGSAchievementsClient.RequestAchievements(num1);
	}

	public void LoadFriends(ILocalUser user, Action<bool> callback)
	{
		if (user == null)
		{
			AGSClient.LogGameCircleError("LoadFriends \"user\" argument should not be null");
			return;
		}
		user.LoadFriends(callback);
	}

	public void LoadScores(string leaderboardID, Action<IScore[]> callback)
	{
		this.loadScoresCallbacks.Add(this.requestID, callback);
		GameCircleSocial gameCircleSocial = this;
		int num = gameCircleSocial.requestID;
		int num1 = num;
		gameCircleSocial.requestID = num + 1;
		AGSLeaderboardsClient.RequestLeaderboards(num1);
	}

	public void LoadScores(ILeaderboard board, Action<bool> callback)
	{
		if (board == null)
		{
			AGSClient.LogGameCircleError("LoadScores \"board\" argument should not be null");
			return;
		}
		board.LoadScores(callback);
	}

	public void LoadUsers(string[] userIDs, Action<IUserProfile[]> callback)
	{
		AGSClient.LogGameCircleError("ISocialPlatform.LoadUsers is not available for GameCircle");
	}

	private void OnRequestAchievementsCompleted(AGSRequestAchievementsResponse response)
	{
		Action<IAchievement[]> item;
		Action<IAchievementDescription[]> action;
		if (this.loadAchievementDescriptionsCallbacks.ContainsKey(response.userData))
		{
			if (!this.loadAchievementDescriptionsCallbacks.ContainsKey(response.userData))
			{
				action = null;
			}
			else
			{
				action = this.loadAchievementDescriptionsCallbacks[response.userData];
			}
			Action<IAchievementDescription[]> action1 = action;
			if (action1 != null)
			{
				AGSSocialAchievement[] aGSSocialAchievement = new AGSSocialAchievement[response.achievements.Count];
				for (int i = 0; i < response.achievements.Count; i++)
				{
					aGSSocialAchievement[i] = new AGSSocialAchievement(response.achievements[i]);
				}
				action1(aGSSocialAchievement);
			}
		}
		if (this.loadAchievementsCallbacks.ContainsKey(response.userData))
		{
			if (!this.loadAchievementsCallbacks.ContainsKey(response.userData))
			{
				item = null;
			}
			else
			{
				item = this.loadAchievementsCallbacks[response.userData];
			}
			Action<IAchievement[]> action2 = item;
			if (action2 != null)
			{
				AGSSocialAchievement[] aGSSocialAchievementArray = new AGSSocialAchievement[response.achievements.Count];
				for (int j = 0; j < response.achievements.Count; j++)
				{
					aGSSocialAchievementArray[j] = new AGSSocialAchievement(response.achievements[j]);
				}
				action2(aGSSocialAchievementArray);
			}
		}
		this.loadAchievementDescriptionsCallbacks.Remove(response.userData);
	}

	private void OnRequestBatchFriendsCompleted(AGSRequestBatchFriendsResponse response)
	{
		Action<bool> item;
		if (!response.IsError())
		{
			AGSSocialLocalUser.friendList = new List<AGSSocialUser>();
			foreach (AGSPlayer friend in response.friends)
			{
				AGSSocialLocalUser.friendList.Add(new AGSSocialUser(friend));
			}
		}
		if (!this.simpleCallbacks.ContainsKey(response.userData))
		{
			item = null;
		}
		else
		{
			item = this.simpleCallbacks[response.userData];
		}
		Action<bool> action = item;
		if (action != null)
		{
			action(!response.IsError());
		}
		this.simpleCallbacks.Remove(response.userData);
	}

	private void OnRequestFriendIdsCompleted(AGSRequestFriendIdsResponse response)
	{
		Action<bool> item;
		if (!response.IsError())
		{
			AGSPlayerClient.RequestBatchFriends(response.friendIds, response.userData);
		}
		else
		{
			if (!this.simpleCallbacks.ContainsKey(response.userData))
			{
				item = null;
			}
			else
			{
				item = this.simpleCallbacks[response.userData];
			}
			Action<bool> action = item;
			if (action != null)
			{
				action(false);
			}
			this.simpleCallbacks.Remove(response.userData);
		}
	}

	private void OnRequestLocalPlayerScoreCompleted(AGSRequestScoreResponse response)
	{
		AGSSocialLeaderboard item;
		if (!this.leaderboardForRequest.ContainsKey(response.userData))
		{
			item = null;
		}
		else
		{
			item = this.leaderboardForRequest[response.userData];
		}
		AGSSocialLeaderboard aGSSocialLeaderboard = item;
		if (aGSSocialLeaderboard != null)
		{
			aGSSocialLeaderboard.localPlayerScore = response.score;
			aGSSocialLeaderboard.localPlayerRank = response.rank;
		}
		this.leaderboardForRequest.Remove(response.userData);
	}

	private void OnRequestPlayerCompleted(AGSRequestPlayerResponse response)
	{
		Action<bool> item;
		AGSSocialLocalUser.player = response.player;
		if (!this.simpleCallbacks.ContainsKey(response.userData))
		{
			item = null;
		}
		else
		{
			item = this.simpleCallbacks[response.userData];
		}
		Action<bool> action = item;
		if (action != null)
		{
			action(!response.IsError());
		}
		this.simpleCallbacks.Remove(response.userData);
	}

	private void OnRequestScoresCompleted(AGSRequestScoresResponse response)
	{
		AGSSocialLeaderboard item;
		Action<bool> action;
		Action<IScore[]> item1;
		if (!this.leaderboardForRequest.ContainsKey(response.userData))
		{
			item = null;
		}
		else
		{
			item = this.leaderboardForRequest[response.userData];
		}
		AGSSocialLeaderboard aGSSocialLeaderboardScore = item;
		if (aGSSocialLeaderboardScore != null && !response.IsError())
		{
			aGSSocialLeaderboardScore.scores = new IScore[response.scores.Count];
			for (int i = 0; i < response.scores.Count; i++)
			{
				aGSSocialLeaderboardScore.scores[i] = new AGSSocialLeaderboardScore(response.scores[i], response.leaderboard);
			}
		}
		if (!this.simpleCallbacks.ContainsKey(response.userData))
		{
			action = null;
		}
		else
		{
			action = this.simpleCallbacks[response.userData];
		}
		Action<bool> action1 = action;
		if (action1 != null)
		{
			action1(!response.IsError());
		}
		if (!this.loadScoresCallbacks.ContainsKey(response.userData))
		{
			item1 = null;
		}
		else
		{
			item1 = this.loadScoresCallbacks[response.userData];
		}
		Action<IScore[]> action2 = item1;
		if (action2 != null)
		{
			IScore[] scoreArray = new IScore[response.scores.Count];
			for (int j = 0; j < response.scores.Count; j++)
			{
				scoreArray[j] = new AGSSocialLeaderboardScore(response.scores[j], response.leaderboard);
			}
			action2(scoreArray);
		}
		this.leaderboardForRequest.Remove(response.userData);
		this.simpleCallbacks.Remove(response.userData);
	}

	private void OnServiceNotReady(string error)
	{
		if (this.authenticationCallback != null)
		{
			this.authenticationCallback(false);
		}
	}

	private void OnServiceReady()
	{
		if (this.authenticationCallback != null)
		{
			this.authenticationCallback(true);
		}
	}

	private void OnSubmitScoreCompleted(AGSSubmitScoreResponse response)
	{
		Action<bool> item;
		if (!this.simpleCallbacks.ContainsKey(response.userData))
		{
			item = null;
		}
		else
		{
			item = this.simpleCallbacks[response.userData];
		}
		Action<bool> action = item;
		if (action != null)
		{
			action(!response.IsError());
		}
		this.simpleCallbacks.Remove(response.userData);
	}

	private void OnUpdateAchievementCompleted(AGSUpdateAchievementResponse response)
	{
		Action<bool> item;
		if (!this.simpleCallbacks.ContainsKey(response.userData))
		{
			item = null;
		}
		else
		{
			item = this.simpleCallbacks[response.userData];
		}
		Action<bool> action = item;
		if (action != null)
		{
			action(!response.IsError());
		}
		this.simpleCallbacks.Remove(response.userData);
	}

	public void ReportProgress(string achievementID, double progress, Action<bool> callback)
	{
		this.simpleCallbacks.Add(this.requestID, callback);
		float single = (float)progress;
		GameCircleSocial gameCircleSocial = this;
		int num = gameCircleSocial.requestID;
		int num1 = num;
		gameCircleSocial.requestID = num + 1;
		AGSAchievementsClient.UpdateAchievementProgress(achievementID, single, num1);
	}

	public void ReportScore(long score, string board, Action<bool> callback)
	{
		this.simpleCallbacks.Add(this.requestID, callback);
		GameCircleSocial gameCircleSocial = this;
		int num = gameCircleSocial.requestID;
		int num1 = num;
		gameCircleSocial.requestID = num + 1;
		AGSLeaderboardsClient.SubmitScore(board, score, num1);
	}

	public void RequestFriends(Action<bool> callback)
	{
		this.simpleCallbacks.Add(this.requestID, callback);
		GameCircleSocial gameCircleSocial = this;
		int num = gameCircleSocial.requestID;
		int num1 = num;
		gameCircleSocial.requestID = num + 1;
		AGSPlayerClient.RequestFriendIds(num1);
	}

	public void RequestLocalPlayer(Action<bool> callback)
	{
		this.simpleCallbacks.Add(this.requestID, callback);
		GameCircleSocial gameCircleSocial = this;
		int num = gameCircleSocial.requestID;
		int num1 = num;
		gameCircleSocial.requestID = num + 1;
		AGSPlayerClient.RequestLocalPlayer(num1);
	}

	public void RequestLocalUserScore(AGSSocialLeaderboard leaderboard)
	{
		this.leaderboardForRequest.Add(this.requestID, leaderboard);
		string str = leaderboard.id;
		LeaderboardScope leaderboardScope = this.fromTimeScope(leaderboard.timeScope);
		GameCircleSocial gameCircleSocial = this;
		int num = gameCircleSocial.requestID;
		int num1 = num;
		gameCircleSocial.requestID = num + 1;
		AGSLeaderboardsClient.RequestScores(str, leaderboardScope, num1);
	}

	public void RequestScores(AGSSocialLeaderboard leaderboard, Action<bool> callback)
	{
		this.leaderboardForRequest.Add(this.requestID, leaderboard);
		this.simpleCallbacks.Add(this.requestID, callback);
		string str = leaderboard.id;
		LeaderboardScope leaderboardScope = this.fromTimeScope(leaderboard.timeScope);
		GameCircleSocial gameCircleSocial = this;
		int num = gameCircleSocial.requestID;
		int num1 = num;
		gameCircleSocial.requestID = num + 1;
		AGSLeaderboardsClient.RequestScores(str, leaderboardScope, num1);
	}

	public void ShowAchievementsUI()
	{
		AGSAchievementsClient.ShowAchievementsOverlay();
	}

	public void ShowLeaderboardUI()
	{
		AGSLeaderboardsClient.ShowLeaderboardsOverlay();
	}
}