using System;
using System.Collections.Generic;

public sealed class CurrentCampaignGame
{
	public static string boXName;

	public static string levelSceneName;

	public static float _levelStartedAtTime;

	public static bool withoutHits;

	public static bool completeInTime;

	public static int currentLevel
	{
		get
		{
			if (!Switcher.sceneNameToGameNum.ContainsKey(CurrentCampaignGame.levelSceneName))
			{
				return 0;
			}
			return Switcher.sceneNameToGameNum[CurrentCampaignGame.levelSceneName];
		}
	}

	static CurrentCampaignGame()
	{
		CurrentCampaignGame.boXName = string.Empty;
		CurrentCampaignGame.levelSceneName = string.Empty;
	}

	public CurrentCampaignGame()
	{
	}

	public static void ResetConditionParameters()
	{
		CurrentCampaignGame.withoutHits = true;
		CurrentCampaignGame.completeInTime = true;
	}
}