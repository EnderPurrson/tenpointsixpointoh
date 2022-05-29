using System;
using System.Collections;
using System.Collections.Generic;

public class AGSRequestPercentilesResponse : AGSRequestResponse
{
	public string leaderboardId;

	public AGSLeaderboard leaderboard;

	public List<AGSLeaderboardPercentile> percentiles;

	public int userIndex;

	public LeaderboardScope scope;

	public AGSRequestPercentilesResponse()
	{
	}

	public static AGSRequestPercentilesResponse FromJSON(string json)
	{
		AGSRequestPercentilesResponse blankResponseWithError;
		try
		{
			AGSRequestPercentilesResponse aGSRequestPercentilesResponse = new AGSRequestPercentilesResponse();
			Hashtable hashtables = json.hashtableFromJson();
			aGSRequestPercentilesResponse.error = (!hashtables.ContainsKey("error") ? string.Empty : hashtables["error"].ToString());
			aGSRequestPercentilesResponse.userData = (!hashtables.ContainsKey("userData") ? 0 : int.Parse(hashtables["userData"].ToString()));
			aGSRequestPercentilesResponse.leaderboardId = (!hashtables.ContainsKey("leaderboardId") ? string.Empty : hashtables["leaderboardId"].ToString());
			if (!hashtables.ContainsKey("leaderboard"))
			{
				aGSRequestPercentilesResponse.leaderboard = AGSLeaderboard.GetBlankLeaderboard();
			}
			else
			{
				aGSRequestPercentilesResponse.leaderboard = AGSLeaderboard.fromHashtable(hashtables["leaderboard"] as Hashtable);
			}
			aGSRequestPercentilesResponse.percentiles = new List<AGSLeaderboardPercentile>();
			if (hashtables.Contains("percentiles"))
			{
				IEnumerator enumerator = (hashtables["percentiles"] as ArrayList).GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						Hashtable current = (Hashtable)enumerator.Current;
						aGSRequestPercentilesResponse.percentiles.Add(AGSLeaderboardPercentile.fromHashTable(current));
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
			aGSRequestPercentilesResponse.userIndex = (!hashtables.ContainsKey("userIndex") ? -1 : int.Parse(hashtables["userIndex"].ToString()));
			aGSRequestPercentilesResponse.scope = (LeaderboardScope)((int)Enum.Parse(typeof(LeaderboardScope), hashtables["scope"].ToString()));
			blankResponseWithError = aGSRequestPercentilesResponse;
		}
		catch (Exception exception)
		{
			AGSClient.LogGameCircleError(exception.ToString());
			blankResponseWithError = AGSRequestPercentilesResponse.GetBlankResponseWithError("ERROR_PARSING_JSON", string.Empty, LeaderboardScope.GlobalAllTime, 0);
		}
		return blankResponseWithError;
	}

	public static AGSRequestPercentilesResponse GetBlankResponseWithError(string error, string leaderboardId = "", LeaderboardScope scope = 0, int userData = 0)
	{
		AGSRequestPercentilesResponse aGSRequestPercentilesResponse = new AGSRequestPercentilesResponse()
		{
			error = error,
			userData = userData,
			leaderboardId = leaderboardId,
			scope = scope,
			leaderboard = AGSLeaderboard.GetBlankLeaderboard(),
			percentiles = new List<AGSLeaderboardPercentile>(),
			userIndex = -1
		};
		aGSRequestPercentilesResponse.scope = scope;
		return aGSRequestPercentilesResponse;
	}

	public static AGSRequestPercentilesResponse GetPlatformNotSupportedResponse(string leaderboardId, LeaderboardScope scope, int userData)
	{
		return AGSRequestPercentilesResponse.GetBlankResponseWithError("PLATFORM_NOT_SUPPORTED", leaderboardId, scope, userData);
	}
}