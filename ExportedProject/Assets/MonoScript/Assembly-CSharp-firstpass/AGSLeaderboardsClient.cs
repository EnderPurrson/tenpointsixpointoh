using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AGSLeaderboardsClient : MonoBehaviour
{
	private static AmazonJavaWrapper JavaObject;

	private readonly static string PROXY_CLASS_NAME;

	static AGSLeaderboardsClient()
	{
		AGSLeaderboardsClient.PROXY_CLASS_NAME = "com.amazon.ags.api.unity.LeaderboardsClientProxyImpl";
		AGSLeaderboardsClient.JavaObject = new AmazonJavaWrapper();
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass(AGSLeaderboardsClient.PROXY_CLASS_NAME))
		{
			if (androidJavaClass.GetRawClass() != IntPtr.Zero)
			{
				AGSLeaderboardsClient.JavaObject.setAndroidJavaObject(androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]));
			}
			else
			{
				AGSClient.LogGameCircleWarning(string.Concat("No java class ", AGSLeaderboardsClient.PROXY_CLASS_NAME, " present, can't use AGSLeaderboardsClient"));
			}
		}
	}

	public AGSLeaderboardsClient()
	{
	}

	public static void RequestLeaderboards(int userData = 0)
	{
		AGSLeaderboardsClient.JavaObject.Call("requestLeaderboards", new object[] { 0 });
	}

	public static void RequestLeaderboardsFailed(string json)
	{
		AGSRequestLeaderboardsResponse aGSRequestLeaderboardsResponse = AGSRequestLeaderboardsResponse.FromJSON(json);
		if (aGSRequestLeaderboardsResponse.IsError() && AGSLeaderboardsClient.RequestLeaderboardsFailedEvent != null)
		{
			AGSLeaderboardsClient.RequestLeaderboardsFailedEvent(aGSRequestLeaderboardsResponse.error);
		}
		if (AGSLeaderboardsClient.RequestLeaderboardsCompleted != null)
		{
			AGSLeaderboardsClient.RequestLeaderboardsCompleted(aGSRequestLeaderboardsResponse);
		}
	}

	public static void RequestLeaderboardsSucceeded(string json)
	{
		AGSRequestLeaderboardsResponse aGSRequestLeaderboardsResponse = AGSRequestLeaderboardsResponse.FromJSON(json);
		if (!aGSRequestLeaderboardsResponse.IsError() && AGSLeaderboardsClient.RequestLeaderboardsSucceededEvent != null)
		{
			AGSLeaderboardsClient.RequestLeaderboardsSucceededEvent(aGSRequestLeaderboardsResponse.leaderboards);
		}
		if (AGSLeaderboardsClient.RequestLeaderboardsCompleted != null)
		{
			AGSLeaderboardsClient.RequestLeaderboardsCompleted(aGSRequestLeaderboardsResponse);
		}
	}

	public static void RequestLocalPlayerScore(string leaderboardId, LeaderboardScope scope, int userData = 0)
	{
		AGSLeaderboardsClient.JavaObject.Call("requestLocalPlayerScore", new object[] { leaderboardId, (int)scope, 0 });
	}

	public static void RequestLocalPlayerScoreFailed(string json)
	{
		AGSRequestScoreResponse aGSRequestScoreResponse = AGSRequestScoreResponse.FromJSON(json);
		if (aGSRequestScoreResponse.IsError() && AGSLeaderboardsClient.RequestLocalPlayerScoreFailedEvent != null)
		{
			AGSLeaderboardsClient.RequestLocalPlayerScoreFailedEvent(aGSRequestScoreResponse.leaderboardId, aGSRequestScoreResponse.error);
		}
		if (AGSLeaderboardsClient.RequestLocalPlayerScoreCompleted != null)
		{
			AGSLeaderboardsClient.RequestLocalPlayerScoreCompleted(aGSRequestScoreResponse);
		}
	}

	public static void RequestLocalPlayerScoreSucceeded(string json)
	{
		AGSRequestScoreResponse aGSRequestScoreResponse = AGSRequestScoreResponse.FromJSON(json);
		if (!aGSRequestScoreResponse.IsError() && AGSLeaderboardsClient.RequestLocalPlayerScoreSucceededEvent != null)
		{
			AGSLeaderboardsClient.RequestLocalPlayerScoreSucceededEvent(aGSRequestScoreResponse.leaderboardId, aGSRequestScoreResponse.rank, aGSRequestScoreResponse.score);
		}
		if (AGSLeaderboardsClient.RequestLocalPlayerScoreCompleted != null)
		{
			AGSLeaderboardsClient.RequestLocalPlayerScoreCompleted(aGSRequestScoreResponse);
		}
	}

	public static void RequestPercentileRanks(string leaderboardId, LeaderboardScope scope, int userData = 0)
	{
		AGSLeaderboardsClient.JavaObject.Call("requestPercentileRanks", new object[] { leaderboardId, (int)scope, userData });
	}

	public static void RequestPercentileRanksFailed(string json)
	{
		AGSRequestPercentilesResponse aGSRequestPercentilesResponse = AGSRequestPercentilesResponse.FromJSON(json);
		if (aGSRequestPercentilesResponse.IsError() && AGSLeaderboardsClient.RequestPercentileRanksFailedEvent != null)
		{
			AGSLeaderboardsClient.RequestPercentileRanksFailedEvent(aGSRequestPercentilesResponse.leaderboardId, aGSRequestPercentilesResponse.error);
		}
		if (AGSLeaderboardsClient.RequestPercentileRanksCompleted != null)
		{
			AGSLeaderboardsClient.RequestPercentileRanksCompleted(aGSRequestPercentilesResponse);
		}
	}

	public static void RequestPercentileRanksForPlayer(string leaderboardId, string playerId, LeaderboardScope scope, int userData = 0)
	{
		AGSLeaderboardsClient.JavaObject.Call("requestPercentileRanksForPlayer", new object[] { leaderboardId, playerId, (int)scope, userData });
	}

	public static void RequestPercentileRanksForPlayerComplete(string json)
	{
		if (AGSLeaderboardsClient.RequestPercentileRanksForPlayerCompleted != null)
		{
			AGSLeaderboardsClient.RequestPercentileRanksForPlayerCompleted(AGSRequestPercentilesForPlayerResponse.FromJSON(json));
		}
	}

	public static void RequestPercentileRanksSucceeded(string json)
	{
		AGSRequestPercentilesResponse aGSRequestPercentilesResponse = AGSRequestPercentilesResponse.FromJSON(json);
		if (!aGSRequestPercentilesResponse.IsError() && AGSLeaderboardsClient.RequestPercentileRanksSucceededEvent != null)
		{
			AGSLeaderboardsClient.RequestPercentileRanksSucceededEvent(aGSRequestPercentilesResponse.leaderboard, aGSRequestPercentilesResponse.percentiles, aGSRequestPercentilesResponse.userIndex);
		}
		if (AGSLeaderboardsClient.RequestPercentileRanksCompleted != null)
		{
			AGSLeaderboardsClient.RequestPercentileRanksCompleted(aGSRequestPercentilesResponse);
		}
	}

	public static void RequestScoreForPlayer(string leaderboardId, string playerId, LeaderboardScope scope, int userData = 0)
	{
		AGSLeaderboardsClient.JavaObject.Call("requestScoreForPlayer", new object[] { leaderboardId, playerId, (int)scope, userData });
	}

	public static void RequestScoreForPlayerComplete(string json)
	{
		if (AGSLeaderboardsClient.RequestScoreForPlayerCompleted != null)
		{
			AGSLeaderboardsClient.RequestScoreForPlayerCompleted(AGSRequestScoreForPlayerResponse.FromJSON(json));
		}
	}

	public static void RequestScores(string leaderboardId, LeaderboardScope scope, int userData = 0)
	{
		AGSLeaderboardsClient.JavaObject.Call("requestScores", new object[] { leaderboardId, (int)scope, userData });
	}

	public static void RequestScoresFailed(string json)
	{
		if (AGSLeaderboardsClient.RequestScoresCompleted != null)
		{
			AGSLeaderboardsClient.RequestScoresCompleted(AGSRequestScoresResponse.FromJSON(json));
		}
	}

	public static void RequestScoresSucceeded(string json)
	{
		if (AGSLeaderboardsClient.RequestScoresCompleted != null)
		{
			AGSLeaderboardsClient.RequestScoresCompleted(AGSRequestScoresResponse.FromJSON(json));
		}
	}

	public static void ShowLeaderboardsOverlay()
	{
		AGSLeaderboardsClient.JavaObject.Call("showLeaderboardsOverlay", new object[0]);
	}

	public static void SubmitScore(string leaderboardId, long score, int userData = 0)
	{
		AGSLeaderboardsClient.JavaObject.Call("submitScore", new object[] { leaderboardId, score, userData });
	}

	public static void SubmitScoreFailed(string json)
	{
		AGSSubmitScoreResponse aGSSubmitScoreResponse = AGSSubmitScoreResponse.FromJSON(json);
		if (aGSSubmitScoreResponse.IsError() && AGSLeaderboardsClient.SubmitScoreFailedEvent != null)
		{
			AGSLeaderboardsClient.SubmitScoreFailedEvent(aGSSubmitScoreResponse.leaderboardId, aGSSubmitScoreResponse.error);
		}
		if (AGSLeaderboardsClient.SubmitScoreCompleted != null)
		{
			AGSLeaderboardsClient.SubmitScoreCompleted(aGSSubmitScoreResponse);
		}
	}

	public static void SubmitScoreSucceeded(string json)
	{
		AGSSubmitScoreResponse aGSSubmitScoreResponse = AGSSubmitScoreResponse.FromJSON(json);
		if (!aGSSubmitScoreResponse.IsError() && AGSLeaderboardsClient.SubmitScoreSucceededEvent != null)
		{
			AGSLeaderboardsClient.SubmitScoreSucceededEvent(aGSSubmitScoreResponse.leaderboardId);
		}
		if (AGSLeaderboardsClient.SubmitScoreCompleted != null)
		{
			AGSLeaderboardsClient.SubmitScoreCompleted(aGSSubmitScoreResponse);
		}
	}

	public static event Action<AGSRequestLeaderboardsResponse> RequestLeaderboardsCompleted;

	[Obsolete("RequestLeaderboardsFailedEvent is deprecated. Use RequestLeaderboardsCompleted instead.")]
	public static event Action<string> RequestLeaderboardsFailedEvent;

	[Obsolete("RequestLeaderboardsSucceededEvent is deprecated. Use RequestLeaderboardsCompleted instead.")]
	public static event Action<List<AGSLeaderboard>> RequestLeaderboardsSucceededEvent;

	public static event Action<AGSRequestScoreResponse> RequestLocalPlayerScoreCompleted;

	[Obsolete("RequestLocalPlayerScoreFailedEvent is deprecated. Use RequestLocalPlayerScoreCompleted instead.")]
	public static event Action<string, string> RequestLocalPlayerScoreFailedEvent;

	[Obsolete("RequestLocalPlayerScoreSucceededEvent is deprecated. Use RequestLocalPlayerScoreCompleted instead.")]
	public static event Action<string, int, long> RequestLocalPlayerScoreSucceededEvent;

	public static event Action<AGSRequestPercentilesResponse> RequestPercentileRanksCompleted;

	[Obsolete("RequestPercentileRanksFailedEvent is deprecated. Use RequestPercentileRanksCompleted instead.")]
	public static event Action<string, string> RequestPercentileRanksFailedEvent;

	public static event Action<AGSRequestPercentilesForPlayerResponse> RequestPercentileRanksForPlayerCompleted;

	[Obsolete("RequestPercentileRanksSucceededEvent is deprecated. Use RequestPercentileRanksCompleted instead.")]
	public static event Action<AGSLeaderboard, List<AGSLeaderboardPercentile>, int> RequestPercentileRanksSucceededEvent;

	public static event Action<AGSRequestScoreForPlayerResponse> RequestScoreForPlayerCompleted;

	public static event Action<AGSRequestScoresResponse> RequestScoresCompleted;

	public static event Action<AGSSubmitScoreResponse> SubmitScoreCompleted;

	[Obsolete("SubmitScoreFailedEvent is deprecated. Use SubmitScoreCompleted instead.")]
	public static event Action<string, string> SubmitScoreFailedEvent;

	[Obsolete("SubmitScoreSucceededEvent is deprecated. Use SubmitScoreCompleted instead.")]
	public static event Action<string> SubmitScoreSucceededEvent;
}