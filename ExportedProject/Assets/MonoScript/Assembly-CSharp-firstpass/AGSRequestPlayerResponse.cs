using System;
using System.Collections;

public class AGSRequestPlayerResponse : AGSRequestResponse
{
	public AGSPlayer player;

	public AGSRequestPlayerResponse()
	{
	}

	public static AGSRequestPlayerResponse FromJSON(string json)
	{
		AGSRequestPlayerResponse blankResponseWithError;
		try
		{
			AGSRequestPlayerResponse aGSRequestPlayerResponse = new AGSRequestPlayerResponse();
			Hashtable hashtables = json.hashtableFromJson();
			aGSRequestPlayerResponse.error = (!hashtables.ContainsKey("error") ? string.Empty : hashtables["error"].ToString());
			aGSRequestPlayerResponse.userData = (!hashtables.ContainsKey("userData") ? 0 : int.Parse(hashtables["userData"].ToString()));
			aGSRequestPlayerResponse.player = (!hashtables.ContainsKey("player") ? AGSPlayer.GetBlankPlayer() : AGSPlayer.fromHashtable(hashtables["player"] as Hashtable));
			blankResponseWithError = aGSRequestPlayerResponse;
		}
		catch (Exception exception)
		{
			AGSClient.LogGameCircleError(exception.ToString());
			blankResponseWithError = AGSRequestPlayerResponse.GetBlankResponseWithError("ERROR_PARSING_JSON", 0);
		}
		return blankResponseWithError;
	}

	public static AGSRequestPlayerResponse GetBlankResponseWithError(string error, int userData = 0)
	{
		AGSRequestPlayerResponse aGSRequestPlayerResponse = new AGSRequestPlayerResponse()
		{
			error = error,
			userData = userData,
			player = AGSPlayer.GetBlankPlayer()
		};
		return aGSRequestPlayerResponse;
	}

	public static AGSRequestPlayerResponse GetPlatformNotSupportedResponse(int userData)
	{
		return AGSRequestPlayerResponse.GetBlankResponseWithError("PLATFORM_NOT_SUPPORTED", userData);
	}
}