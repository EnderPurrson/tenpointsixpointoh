using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.GameCenter;

public class GameCenterSingleton
{
	private static GameCenterSingleton instance;

	private static string _leaderboardID;

	public static string SurvivalTableID;

	public string bestScore = "0";

	private IAchievement[] achievements;

	public static GameCenterSingleton Instance
	{
		get
		{
			if (GameCenterSingleton.instance == null)
			{
				GameCenterSingleton.instance = new GameCenterSingleton();
				GameCenterSingleton.instance.Initialize();
			}
			return GameCenterSingleton.instance;
		}
	}

	static GameCenterSingleton()
	{
		GameCenterSingleton._leaderboardID = (!GlobalGameController.isFullVersion ? "zombieslayerslite" : "zombieslayers");
		GameCenterSingleton.SurvivalTableID = "arena_heroes";
	}

	public GameCenterSingleton()
	{
	}

	public bool AddAchievementProgress(string achievementID, float percentageToAdd)
	{
		IAchievement achievement = this.GetAchievement(achievementID);
		if (achievement == null)
		{
			return this.ReportAchievementProgress(achievementID, percentageToAdd);
		}
		return this.ReportAchievementProgress(achievementID, (float)achievement.percentCompleted + percentageToAdd);
	}

	private IAchievement GetAchievement(string achievementID)
	{
		if (this.achievements != null)
		{
			IAchievement[] achievementArray = this.achievements;
			for (int i = 0; i < (int)achievementArray.Length; i++)
			{
				IAchievement achievement = achievementArray[i];
				if (achievement.id == achievementID)
				{
					return achievement;
				}
			}
		}
		return null;
	}

	public void GetScore()
	{
		Social.LoadScores(GameCenterSingleton._leaderboardID, (IScore[] scores) => {
			if ((int)scores.Length <= 0)
			{
				Debug.Log("No scores loaded");
			}
			else
			{
				Debug.Log(string.Concat("Got ", (int)scores.Length, " scores"));
				if ((int)scores.Length > 0)
				{
					this.bestScore = scores[0].formattedValue;
					if (this.bestScore == null || this.bestScore.Equals(string.Empty))
					{
						this.bestScore = "0";
					}
				}
			}
			this.bestScore = "0";
		});
	}

	public void Initialize()
	{
		if (!this.IsUserAuthenticated())
		{
			Social.localUser.Authenticate(new Action<bool>(this.ProcessAuthentication));
		}
	}

	private bool IsAchievementComplete(string achievementID)
	{
		if (this.achievements != null)
		{
			IAchievement[] achievementArray = this.achievements;
			for (int i = 0; i < (int)achievementArray.Length; i++)
			{
				IAchievement achievement = achievementArray[i];
				if (achievement.id == achievementID && achievement.completed)
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool IsUserAuthenticated()
	{
		if (Social.localUser.authenticated)
		{
			return true;
		}
		Debug.Log("User not Authenticated");
		return false;
	}

	private void LoadAchievements()
	{
		Social.LoadAchievements(new Action<IAchievement[]>(this.ProcessLoadedAchievements));
	}

	private void ProcessAuthentication(bool success)
	{
		if (!success)
		{
			Debug.Log("Failed to authenticate");
		}
		else
		{
			Debug.Log("Authenticated, checking achievements");
			this.GetScore();
		}
	}

	private void ProcessLoadedAchievements(IAchievement[] achievements)
	{
		if (this.achievements != null)
		{
			this.achievements = null;
		}
		if ((int)achievements.Length != 0)
		{
			Debug.Log(string.Concat("Got ", (int)achievements.Length, " achievements"));
			this.achievements = achievements;
		}
		else
		{
			Debug.Log("Error: no achievements found");
		}
	}

	public bool ReportAchievementProgress(string achievementID, float progressCompleted)
	{
		if (!Social.localUser.authenticated)
		{
			Debug.Log("ERROR: GameCenter user not authenticated");
			return false;
		}
		if (this.IsAchievementComplete(achievementID))
		{
			return true;
		}
		bool flag = false;
		Social.ReportProgress(achievementID, (double)progressCompleted, (bool result) => {
			if (!result)
			{
				flag = false;
				Debug.Log("Failed to report progress");
			}
			else
			{
				flag = true;
				this.LoadAchievements();
				Debug.Log("Successfully reported progress");
			}
		});
		return flag;
	}

	public void ReportScore(long score, string tableName = null)
	{
		if (tableName == null)
		{
			tableName = GameCenterSingleton._leaderboardID;
		}
		Debug.Log(string.Concat(new object[] { "Reporting score ", score, " on leaderboard ", tableName }));
		Social.ReportScore(score, tableName, (bool success) => Debug.Log((!success ? "Failed to report score" : "Reported score successfully")));
	}

	public void ResetAchievements()
	{
		GameCenterPlatform.ResetAllAchievements(new Action<bool>(this.ResetAchievementsHandler));
	}

	private void ResetAchievementsHandler(bool status)
	{
		if (!status)
		{
			Debug.Log("Achievements reset failure!");
		}
		else
		{
			if (this.achievements != null)
			{
				this.achievements = null;
			}
			this.LoadAchievements();
			Debug.Log("Achievements successfully resetted!");
		}
	}

	public void ShowAchievementUI()
	{
		if (this.IsUserAuthenticated())
		{
			Social.ShowAchievementsUI();
		}
	}

	public void ShowLeaderboardUI()
	{
		if (this.IsUserAuthenticated())
		{
			Social.ShowLeaderboardUI();
		}
	}

	public void updateGameCenter()
	{
		GameCenterSingleton.instance = new GameCenterSingleton();
		GameCenterSingleton.instance.Initialize();
	}
}