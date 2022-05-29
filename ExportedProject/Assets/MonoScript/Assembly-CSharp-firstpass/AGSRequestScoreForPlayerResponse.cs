using System;
using System.Collections;

public class AGSRequestScoreForPlayerResponse : AGSRequestScoreResponse
{
	public string playerId;

	public AGSRequestScoreForPlayerResponse()
	{
	}

	public static new AGSRequestScoreForPlayerResponse FromJSON(string json)
	{
		AGSRequestScoreForPlayerResponse blankResponseWithError;
		try
		{
			AGSRequestScoreForPlayerResponse aGSRequestScoreForPlayerResponse = new AGSRequestScoreForPlayerResponse();
			Hashtable hashtables = json.hashtableFromJson();
			aGSRequestScoreForPlayerResponse.error = (!hashtables.ContainsKey("error") ? string.Empty : hashtables["error"].ToString());
			aGSRequestScoreForPlayerResponse.userData = (!hashtables.ContainsKey("userData") ? 0 : int.Parse(hashtables["userData"].ToString()));
			aGSRequestScoreForPlayerResponse.leaderboardId = (!hashtables.ContainsKey("leaderboardId") ? string.Empty : hashtables["leaderboardId"].ToString());
			aGSRequestScoreForPlayerResponse.rank = (!hashtables.ContainsKey("rank") ? -1 : int.Parse(hashtables["rank"].ToString()));
			aGSRequestScoreForPlayerResponse.score = (!hashtables.ContainsKey("score") ? (long)-1 : long.Parse(hashtables["score"].ToString()));
			aGSRequestScoreForPlayerResponse.scope = (LeaderboardScope)((int)Enum.Parse(typeof(LeaderboardScope), hashtables["scope"].ToString()));
			aGSRequestScoreForPlayerResponse.playerId = (!hashtables.Contains("playerId") ? string.Empty : hashtables["playerId"].ToString());
			blankResponseWithError = aGSRequestScoreForPlayerResponse;
		}
		catch (Exception exception)
		{
			AGSClient.LogGameCircleError(exception.ToString());
			blankResponseWithError = AGSRequestScoreForPlayerResponse.GetBlankResponseWithError("ERROR_PARSING_JSON", string.Empty, string.Empty, LeaderboardScope.GlobalAllTime, 0);
		}
		return blankResponseWithError;
	}

	public static AGSRequestScoreForPlayerResponse GetBlankResponseWithError(string error, string leaderboardId = "", string playerId = "", LeaderboardScope scope = 0, int userData = 0)
	{
		AGSRequestScoreForPlayerResponse aGSRequestScoreForPlayerResponse = new AGSRequestScoreForPlayerResponse()
		{
			error = error,
			playerId = playerId,
			userData = userData,
			leaderboardId = leaderboardId,
			scope = scope,
			rank = -1,
			score = (long)-1
		};
		return aGSRequestScoreForPlayerResponse;
	}

	public static AGSRequestScoreForPlayerResponse GetPlatformNotSupportedResponse(string leaderboardId, string playerId, LeaderboardScope scope, int userData)
	{
		return AGSRequestScoreForPlayerResponse.GetBlankResponseWithError("PLATFORM_NOT_SUPPORTED", leaderboardId, playerId, scope, userData);
	}
}