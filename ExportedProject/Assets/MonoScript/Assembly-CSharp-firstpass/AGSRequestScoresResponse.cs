using System;
using System.Collections;
using System.Collections.Generic;

public class AGSRequestScoresResponse : AGSRequestResponse
{
	public string leaderboardId;

	public AGSLeaderboard leaderboard;

	public LeaderboardScope scope;

	public List<AGSScore> scores;

	public AGSRequestScoresResponse()
	{
	}

	public static AGSRequestScoresResponse FromJSON(string json)
	{
		AGSRequestScoresResponse blankResponseWithError;
		try
		{
			AGSRequestScoresResponse aGSRequestScoresResponse = new AGSRequestScoresResponse();
			Hashtable hashtables = json.hashtableFromJson();
			aGSRequestScoresResponse.error = (!hashtables.ContainsKey("error") ? string.Empty : hashtables["error"].ToString());
			aGSRequestScoresResponse.userData = (!hashtables.ContainsKey("userData") ? 0 : int.Parse(hashtables["userData"].ToString()));
			aGSRequestScoresResponse.leaderboardId = (!hashtables.ContainsKey("leaderboardId") ? string.Empty : hashtables["leaderboardId"].ToString());
			if (!hashtables.ContainsKey("leaderboard"))
			{
				aGSRequestScoresResponse.leaderboard = AGSLeaderboard.GetBlankLeaderboard();
			}
			else
			{
				aGSRequestScoresResponse.leaderboard = AGSLeaderboard.fromHashtable(hashtables["leaderboard"] as Hashtable);
			}
			aGSRequestScoresResponse.scores = new List<AGSScore>();
			if (hashtables.Contains("scores"))
			{
				IEnumerator enumerator = (hashtables["scores"] as ArrayList).GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						Hashtable current = (Hashtable)enumerator.Current;
						aGSRequestScoresResponse.scores.Add(AGSScore.fromHashtable(current));
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
			aGSRequestScoresResponse.scope = (LeaderboardScope)((int)Enum.Parse(typeof(LeaderboardScope), hashtables["scope"].ToString()));
			blankResponseWithError = aGSRequestScoresResponse;
		}
		catch (Exception exception)
		{
			AGSClient.LogGameCircleError(exception.ToString());
			blankResponseWithError = AGSRequestScoresResponse.GetBlankResponseWithError("ERROR_PARSING_JSON", string.Empty, LeaderboardScope.GlobalAllTime, 0);
		}
		return blankResponseWithError;
	}

	public static AGSRequestScoresResponse GetBlankResponseWithError(string error, string leaderboardId = "", LeaderboardScope scope = 0, int userData = 0)
	{
		AGSRequestScoresResponse aGSRequestScoresResponse = new AGSRequestScoresResponse()
		{
			error = error,
			userData = userData,
			leaderboardId = leaderboardId,
			scope = scope,
			scores = new List<AGSScore>()
		};
		return aGSRequestScoresResponse;
	}

	public static AGSRequestScoresResponse GetPlatformNotSupportedResponse(string leaderboardId, LeaderboardScope scope, int userData)
	{
		return AGSRequestScoresResponse.GetBlankResponseWithError("PLATFORM_NOT_SUPPORTED", leaderboardId, scope, userData);
	}
}