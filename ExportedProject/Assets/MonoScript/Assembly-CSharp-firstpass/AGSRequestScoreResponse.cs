using System;
using System.Collections;

public class AGSRequestScoreResponse : AGSRequestResponse
{
	public string leaderboardId;

	public LeaderboardScope scope;

	public int rank;

	public long score;

	public AGSRequestScoreResponse()
	{
	}

	public static AGSRequestScoreResponse FromJSON(string json)
	{
		AGSRequestScoreResponse blankResponseWithError;
		try
		{
			AGSRequestScoreResponse aGSRequestScoreResponse = new AGSRequestScoreResponse();
			Hashtable hashtables = json.hashtableFromJson();
			aGSRequestScoreResponse.error = (!hashtables.ContainsKey("error") ? string.Empty : hashtables["error"].ToString());
			aGSRequestScoreResponse.userData = (!hashtables.ContainsKey("userData") ? 0 : int.Parse(hashtables["userData"].ToString()));
			aGSRequestScoreResponse.leaderboardId = (!hashtables.ContainsKey("leaderboardId") ? string.Empty : hashtables["leaderboardId"].ToString());
			aGSRequestScoreResponse.rank = (!hashtables.ContainsKey("rank") ? -1 : int.Parse(hashtables["rank"].ToString()));
			aGSRequestScoreResponse.score = (!hashtables.ContainsKey("score") ? (long)-1 : long.Parse(hashtables["score"].ToString()));
			aGSRequestScoreResponse.scope = (LeaderboardScope)((int)Enum.Parse(typeof(LeaderboardScope), hashtables["scope"].ToString()));
			blankResponseWithError = aGSRequestScoreResponse;
		}
		catch (Exception exception)
		{
			AGSClient.LogGameCircleError(exception.ToString());
			blankResponseWithError = AGSRequestScoreResponse.GetBlankResponseWithError("ERROR_PARSING_JSON", string.Empty, LeaderboardScope.GlobalAllTime, 0);
		}
		return blankResponseWithError;
	}

	public static AGSRequestScoreResponse GetBlankResponseWithError(string error, string leaderboardId = "", LeaderboardScope scope = 0, int userData = 0)
	{
		AGSRequestScoreResponse aGSRequestScoreResponse = new AGSRequestScoreResponse()
		{
			error = error,
			userData = userData,
			leaderboardId = leaderboardId,
			scope = scope,
			rank = -1,
			score = (long)-1
		};
		return aGSRequestScoreResponse;
	}

	public static AGSRequestScoreResponse GetPlatformNotSupportedResponse(string leaderboardId, LeaderboardScope scope, int userData)
	{
		return AGSRequestScoreResponse.GetBlankResponseWithError("PLATFORM_NOT_SUPPORTED", leaderboardId, scope, userData);
	}
}