using System;
using System.Collections;

public class AGSSubmitScoreResponse : AGSRequestResponse
{
	public string leaderboardId;

	public AGSSubmitScoreResponse()
	{
	}

	public static AGSSubmitScoreResponse FromJSON(string json)
	{
		AGSSubmitScoreResponse blankResponseWithError;
		try
		{
			AGSSubmitScoreResponse aGSSubmitScoreResponse = new AGSSubmitScoreResponse();
			Hashtable hashtables = json.hashtableFromJson();
			aGSSubmitScoreResponse.error = (!hashtables.ContainsKey("error") ? string.Empty : hashtables["error"].ToString());
			aGSSubmitScoreResponse.userData = (!hashtables.ContainsKey("userData") ? 0 : int.Parse(hashtables["userData"].ToString()));
			aGSSubmitScoreResponse.leaderboardId = (!hashtables.ContainsKey("leaderboardId") ? string.Empty : hashtables["leaderboardId"].ToString());
			blankResponseWithError = aGSSubmitScoreResponse;
		}
		catch (Exception exception)
		{
			AGSClient.LogGameCircleError(exception.ToString());
			blankResponseWithError = AGSSubmitScoreResponse.GetBlankResponseWithError("ERROR_PARSING_JSON", string.Empty, 0);
		}
		return blankResponseWithError;
	}

	public static AGSSubmitScoreResponse GetBlankResponseWithError(string error, string leaderboardId = "", int userData = 0)
	{
		AGSSubmitScoreResponse aGSSubmitScoreResponse = new AGSSubmitScoreResponse()
		{
			error = error,
			userData = userData,
			leaderboardId = leaderboardId
		};
		return aGSSubmitScoreResponse;
	}

	public static AGSSubmitScoreResponse GetPlatformNotSupportedResponse(string leaderboardId, int userData)
	{
		return AGSSubmitScoreResponse.GetBlankResponseWithError("PLATFORM_NOT_SUPPORTED", leaderboardId, userData);
	}
}