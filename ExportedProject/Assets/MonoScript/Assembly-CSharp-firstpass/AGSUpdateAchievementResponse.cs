using System;
using System.Collections;

public class AGSUpdateAchievementResponse : AGSRequestResponse
{
	public string achievementId;

	public AGSUpdateAchievementResponse()
	{
	}

	public static AGSUpdateAchievementResponse FromJSON(string json)
	{
		AGSUpdateAchievementResponse blankResponseWithError;
		try
		{
			AGSUpdateAchievementResponse aGSUpdateAchievementResponse = new AGSUpdateAchievementResponse();
			Hashtable hashtables = json.hashtableFromJson();
			aGSUpdateAchievementResponse.error = (!hashtables.ContainsKey("error") ? string.Empty : hashtables["error"].ToString());
			aGSUpdateAchievementResponse.userData = (!hashtables.ContainsKey("userData") ? 0 : int.Parse(hashtables["userData"].ToString()));
			aGSUpdateAchievementResponse.achievementId = (!hashtables.ContainsKey("achievementId") ? string.Empty : hashtables["achievementId"].ToString());
			blankResponseWithError = aGSUpdateAchievementResponse;
		}
		catch (Exception exception)
		{
			AGSClient.LogGameCircleError(exception.ToString());
			blankResponseWithError = AGSUpdateAchievementResponse.GetBlankResponseWithError("ERROR_PARSING_JSON", string.Empty, 0);
		}
		return blankResponseWithError;
	}

	public static AGSUpdateAchievementResponse GetBlankResponseWithError(string error, string achievementId = "", int userData = 0)
	{
		AGSUpdateAchievementResponse aGSUpdateAchievementResponse = new AGSUpdateAchievementResponse()
		{
			error = error,
			userData = userData,
			achievementId = achievementId
		};
		return aGSUpdateAchievementResponse;
	}

	public static AGSUpdateAchievementResponse GetPlatformNotSupportedResponse(string achievementId, int userData)
	{
		return AGSUpdateAchievementResponse.GetBlankResponseWithError("PLATFORM_NOT_SUPPORTED", achievementId, userData);
	}
}