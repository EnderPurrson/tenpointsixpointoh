using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public static class ScoreExtensions
{
	public static void AddScore(this PhotonPlayer player, int scoreToAddToCurrent)
	{
		int score = player.GetScore() + scoreToAddToCurrent;
		Hashtable hashtable = new Hashtable();
		hashtable["score"] = score;
		player.SetCustomProperties(hashtable, null, false);
	}

	public static int GetScore(this PhotonPlayer player)
	{
		object obj;
		if (!player.customProperties.TryGetValue("score", out obj))
		{
			return 0;
		}
		return (int)obj;
	}

	public static void SetScore(this PhotonPlayer player, int newScore)
	{
		Hashtable hashtable = new Hashtable();
		hashtable["score"] = newScore;
		player.SetCustomProperties(hashtable, null, false);
	}
}