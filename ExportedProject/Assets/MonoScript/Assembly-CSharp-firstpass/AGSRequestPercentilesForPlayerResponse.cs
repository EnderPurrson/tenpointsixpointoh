using System;
using System.Collections;
using System.Collections.Generic;

public class AGSRequestPercentilesForPlayerResponse : AGSRequestPercentilesResponse
{
	public string playerId;

	public AGSRequestPercentilesForPlayerResponse()
	{
	}

	public static new AGSRequestPercentilesForPlayerResponse FromJSON(string json)
	{
		AGSRequestPercentilesForPlayerResponse blankResponseWithError;
		try
		{
			AGSRequestPercentilesForPlayerResponse aGSRequestPercentilesForPlayerResponse = new AGSRequestPercentilesForPlayerResponse();
			Hashtable hashtables = json.hashtableFromJson();
			aGSRequestPercentilesForPlayerResponse.error = (!hashtables.ContainsKey("error") ? string.Empty : hashtables["error"].ToString());
			aGSRequestPercentilesForPlayerResponse.userData = (!hashtables.ContainsKey("userData") ? 0 : int.Parse(hashtables["userData"].ToString()));
			aGSRequestPercentilesForPlayerResponse.leaderboardId = (!hashtables.ContainsKey("leaderboardId") ? string.Empty : hashtables["leaderboardId"].ToString());
			if (!hashtables.ContainsKey("leaderboard"))
			{
				aGSRequestPercentilesForPlayerResponse.leaderboard = AGSLeaderboard.GetBlankLeaderboard();
			}
			else
			{
				aGSRequestPercentilesForPlayerResponse.leaderboard = AGSLeaderboard.fromHashtable(hashtables["leaderboard"] as Hashtable);
			}
			aGSRequestPercentilesForPlayerResponse.percentiles = new List<AGSLeaderboardPercentile>();
			if (hashtables.Contains("percentiles"))
			{
				IEnumerator enumerator = (hashtables["percentiles"] as ArrayList).GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						Hashtable current = (Hashtable)enumerator.Current;
						aGSRequestPercentilesForPlayerResponse.percentiles.Add(AGSLeaderboardPercentile.fromHashTable(current));
					}
				}
				finally
				{
					IDisposable disposable = enumerator as IDisposable;
					if (disposable == null)
					{
					}
					disposable.Dispose();
				}
			}
			aGSRequestPercentilesForPlayerResponse.userIndex = (!hashtables.ContainsKey("userIndex") ? -1 : int.Parse(hashtables["userIndex"].ToString()));
			aGSRequestPercentilesForPlayerResponse.scope = (LeaderboardScope)((int)Enum.Parse(typeof(LeaderboardScope), hashtables["scope"].ToString()));
			aGSRequestPercentilesForPlayerResponse.playerId = (!hashtables.ContainsKey("playerId") ? string.Empty : hashtables["playerId"].ToString());
			blankResponseWithError = aGSRequestPercentilesForPlayerResponse;
		}
		catch (Exception exception)
		{
			AGSClient.LogGameCircleError(exception.ToString());
			blankResponseWithError = AGSRequestPercentilesForPlayerResponse.GetBlankResponseWithError("ERROR_PARSING_JSON", string.Empty, string.Empty, LeaderboardScope.GlobalAllTime, 0);
		}
		return blankResponseWithError;
	}

	public static AGSRequestPercentilesForPlayerResponse GetBlankResponseWithError(string error, string leaderboardId = "", string playerId = "", LeaderboardScope scope = 0, int userData = 0)
	{
		AGSRequestPercentilesForPlayerResponse aGSRequestPercentilesForPlayerResponse = new AGSRequestPercentilesForPlayerResponse()
		{
			error = error,
			userData = userData,
			leaderboardId = leaderboardId,
			scope = scope,
			leaderboard = AGSLeaderboard.GetBlankLeaderboard(),
			percentiles = new List<AGSLeaderboardPercentile>(),
			userIndex = -1
		};
		aGSRequestPercentilesForPlayerResponse.scope = scope;
		aGSRequestPercentilesForPlayerResponse.playerId = playerId;
		return aGSRequestPercentilesForPlayerResponse;
	}

	public static AGSRequestPercentilesForPlayerResponse GetPlatformNotSupportedResponse(string leaderboardId, string playerId, LeaderboardScope scope, int userData)
	{
		return AGSRequestPercentilesForPlayerResponse.GetBlankResponseWithError("PLATFORM_NOT_SUPPORTED", leaderboardId, playerId, scope, userData);
	}
}